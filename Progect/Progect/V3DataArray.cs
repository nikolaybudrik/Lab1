using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Progect
{
    class V3DataArray : V3Data
    {
        public int CountX { get; }
        public int CountY { get; }
        public double StepX { get; }
        public double StepY { get; }
        public Vector2[,] Array { get; }

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

    }
}
