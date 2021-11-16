using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace Progect
{
    class V3DataArray : V3Data, IEnumerable<DataItem>
    {
        public int CountX { get; private set; }
        public int CountY { get; private set; }
        public double StepX { get; private set; }
        public double StepY { get; private set; }
        public Vector2[,] Array { get; private set; }

        public V3DataArray(string str, DateTime dt) : base(str, dt)
        {
            Array = new Vector2[0, 0];
        }
        public V3DataArray(string str, DateTime dt, int CountX, int CountY,
                           Double StepX, Double StepY, FdblVector2 F) : base(str, dt)
        {
            this.CountX = CountX;
            this.CountY = CountY;
            this.StepX = StepX;
            this.StepY = StepY;
            Array = new Vector2[CountX, CountY];
            for (int i = 0; i < CountX; i++)
            {
                for (int j = 0; j < CountY; j++)
                {
                    Array[i, j] = F(i * StepX, j * StepY);
                }
            }
            count = CountX * CountY;
            if (CountX == 0 || CountY == 0)
                maxDistance = 0.0;
            else
                maxDistance = Math.Sqrt((CountX - 1) * (CountX - 1) * StepX * StepX +
                                    (CountY - 1) * (CountY - 1) * StepY * StepY);
        }

        public override int Count { get { return count; } }

        public override double MaxDistance { get { return maxDistance; } }

        public override string ToString()
        {
            return (GetType().ToString() + ' ' + base.ToString() + " CountX:" + CountX.ToString() + " CountY:" +
                CountY.ToString() + " StepX:" + StepX.ToString() + " StepY:" + StepY.ToString());
        }

        public override string ToLongString(string format)
        {
            String ret = (GetType().ToString() + ' ' + ToString(format) + " CountX:" + CountX.ToString() + " CountY:" +
                CountY.ToString() + " StepX:" + StepX.ToString(format) + " StepY:" + StepY.ToString(format));
            ret += "\nDots Item:";
            for (int i = 0; i < CountX; i++)
            {
                for (int j = 0; j < CountY; j++)
                {
                    ret += ($"\nX:" + (i * StepX).ToString(format) + " Y:" + (j * StepY).ToString(format) +
                        " " + Array[i, j].ToString(format) + " Lenght:" + Array[i, j].Length().ToString(format));
                }
            }
            return ret;
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < CountX; i++)
                for (int j = 0; j < CountY; j++)
                {
                    yield return new DataItem(i * StepX, j * StepY, Array[i, j]);
                }
        }

        public static explicit operator V3DataList(V3DataArray DataArray)
        {
            V3DataList DataList = new V3DataList(DataArray.Str, DataArray.Dt);
            for (int i = 0; i < DataArray.CountX; i++)
            {
                for (int j = 0; j < DataArray.CountY; j++)
                {
                    DataList.Add(new DataItem(i * DataArray.StepX, j * DataArray.StepY, DataArray.Array[i, j]));
                }
            }
            return DataList;
        }

        public bool SaveBinary(string filename)
        {
            try
            {
                BinaryWriter writer = new BinaryWriter(new FileStream(filename, FileMode.OpenOrCreate));
                writer.Write(CountX);
                writer.Write(CountY);
                writer.Write(StepX);
                writer.Write(StepY);
                for (int i = 0; i < CountX; i++)
                {
                    for (int j = 0; j < CountY; j++)
                    {
                        writer.Write(Array[i, j].X);
                        writer.Write(Array[i, j].Y);
                    }
                }
                writer.Write(Dt.Year);
                writer.Write(Dt.Month);
                writer.Write(Dt.Day);
                writer.Write(Dt.Hour);
                writer.Write(Dt.Minute);
                writer.Write(Dt.Second);
                writer.Write(Dt.Millisecond);
                writer.Write(Str);
                writer.Close();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool LoadBinary(string filename)
        {
            try
            {
                BinaryReader reader = new BinaryReader(new FileStream(filename, FileMode.Open));
                CountX = reader.ReadInt32();
                CountY = reader.ReadInt32();
                StepX = reader.ReadDouble();
                StepY = reader.ReadDouble();
                Array = new Vector2[CountX, CountY];
                for (int i = 0; i < CountX; i++)
                {
                    for (int j = 0; j < CountY; j++)
                    {
                        float tmpX = reader.ReadSingle();
                        float tmpY = reader.ReadSingle();
                        Array[i, j] = new Vector2(tmpX, tmpY);
                    }
                }
                Dt = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(),
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
                Str = reader.ReadString();
                reader.Close();
                count = CountX * CountY;
                if (CountX == 0 || CountY == 0)
                    maxDistance = 0.0;
                else
                    maxDistance = Math.Sqrt((CountX - 1) * (CountX - 1) * StepX * StepX +
                                        (CountY - 1) * (CountY - 1) * StepY * StepY);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
