using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.Intrinsics;

namespace Progect
{
    struct DataItem
    {
        public double x { get; set; }
        public double y { get; set; }
        public Vector2 Vector { get; set; }
        public DataItem(double x, double y, Vector2 Vector)
        {
            this.x = x;
            this.y = y;
            this.Vector = Vector;
        }
        public String ToLongString(String format)
        {
            //return String.Format(format, x, y, Vector, Vector.Length());
            return "X:" + x.ToString(format) + " Y:" + y.ToString(format) + " <" + Vector.X.ToString(format)
                                + "  " + Vector.Y.ToString(format) + "> Lengnt:" + Vector.Length().ToString(format);
        }
        public override string ToString()
        {
            return x + " " + y + " " + Vector;
        }
    }

    delegate Vector2 FdblVector2(double x, double y);

    abstract class V3Data
    {
        public String Str { get; set; }
        public DateTime Dt { get; set; }
        protected int count;
        protected double maxDistance;
        public abstract int Count{ get; }
        public abstract double MaxDistance { get; }
        public V3Data(String str, DateTime dt)
        {
            Str = str;
            Dt = dt;
        }

        abstract public String ToLongString(String format);

        public override String ToString()
        {
            return (Str + " Date:" + Dt.ToString() + " Count:" +
                Count.ToString() + " MaxDistance:" + MaxDistance.ToString());
        }
        public String ToString(String format)
        {
            return (Str + " Date:" + Dt.ToString() + " Count:" +
                Count.ToString() + " MaxDistance:" + MaxDistance.ToString(format));
        }
    }


    class V3DataList : V3Data
    {

        List<DataItem> ListData { get; }
        public V3DataList(String str, DateTime dt) : base(str, dt)
        {
            ListData = new List<DataItem>();
            count = 0;
            maxDistance = 0;
        }
        public bool Add(DataItem newItem)
        {
            if (ListData.Exists(D => D.x == newItem.x && D.y == newItem.y))
            {
                return false;
            }
            ListData.Add(newItem);
            //Vector256<double> X1 = Vector256<double>.BroadcastScalarToVector256(&x);
            //Хотел через AVX но не робит.
            double X2 = newItem.x;
            double Y2 = newItem.y;
            double max = 0;
            for (int i = 0; i < count; i++)
            {
                double X1 = ListData[i].x;
                double Y1 = ListData[i].y;
                double range =(X1 - X2) * (X1 - X2) + (Y1 - Y2) * (Y1 - Y2);
                if (range > max)
                    max = range;
            }
            max = Math.Sqrt(max);
            if (max > maxDistance)
            {
                maxDistance = max;
            }
            count++;
            return true;
        }
        public int AddDefault(int nItems, FdblVector2 F)
        {
            Random rnd = new Random();
            int sum = 0;
            for (int i = 0; i < nItems; i++)
            {
                double x = rnd.NextDouble();
                double y = rnd.NextDouble();
                if (Add(new DataItem(x, y, F(x, y))))
                    sum++;
            }
            return sum;
        }

        public override int Count { get { return count; } }

        public override double MaxDistance { get { return maxDistance; } }


        public override string ToString()
        {
            return GetType().ToString() + ' ' + base.ToString();
        }
        public override string ToLongString(string format)
        {
            String ret = GetType().ToString() + ' ' + ToString(format) + "\nDataitems:";
            for(int i = 0; i < count; i++)
            {
                ret += "\n";
                ret += ListData[i].ToLongString(format);
            }
            return ret;
        }

    }


    class V3DataArray : V3Data
    {
        public int CountX { get; }
        public int CountY { get; }
        public double StepX { get; }
        public double StepY { get; }
        public Vector2[,] Array { get; }

        public V3DataArray(string str, DateTime dt):base(str, dt)
        {
            Array = new Vector2[0, 0];
        }
        public V3DataArray(string str, DateTime dt, int CountX, int CountY,
                           Double StepX, Double StepY, FdblVector2 F):base(str, dt)
        {
            this.CountX = CountX;
            this.CountY = CountY;
            this.StepX = StepX;
            this.StepY = StepY;
            Array = new Vector2[CountX, CountY];
            for(int i = 0; i < CountX; i++)
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

        public override double MaxDistance{ get { return maxDistance; } }

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

    class V3MainCollection
    {
        private List<V3Data> V3List;
        private int count;
        public int Count
        {
            get { return count; }
        }

        public V3Data this[int i]
        {
            get { return V3List[i]; }
        }

        public V3MainCollection()
        {
            count = 0;
            V3List = new List<V3Data>();
        }

        public bool Contains(String ID)
        {
            return V3List.Exists(D => D.Str == ID);
        }

        public bool Add(V3Data v3Data)
        {
            if(!Contains(v3Data.Str))
            {
                V3List.Add(v3Data);
                count++;
                return true;
            }
            return false;
        }

        public string ToLongString(String format)
        {
            String ret = "";
            for (int i = 0; i < count - 1; i++)
            {
                ret += V3List[i].ToLongString(format) + "\n";
            }
            ret += V3List[count - 1].ToLongString(format);
            return ret;
        }

        public override string ToString()
        {
            String ret = "";
            for (int i = 0; i < count - 1; i++)
            {
                ret += V3List[i].ToString() + "\n";
            }
            ret += V3List[count - 1].ToString();
            return ret;
        }
    }

    static class VectorMetods
    {
        public static Vector2 foo(double x, double y)
        {
            return new Vector2((float) -x, (float)(0.5 * y));
        }
        public static Vector2 foo2(double x, double y)
        {
            return new Vector2((float)x, (float)(y * y));
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            V3DataArray first = new V3DataArray("A", DateTime.Now, 4, 4, 0.4, 0.4, VectorMetods.foo);
            Console.WriteLine(first.ToLongString("e3"));
            V3DataList firstList = (V3DataList)first;
            Console.WriteLine(firstList.ToLongString("e3"));
            Console.WriteLine($"Count1:{first.Count} MaxDistance1:{first.MaxDistance}");
            Console.WriteLine($"Count2:{firstList.Count} MaxDistance2:{firstList.MaxDistance}");
            V3DataList second = new V3DataList("B", DateTime.UtcNow);
            Console.WriteLine(second.AddDefault(8, VectorMetods.foo2));
            V3MainCollection collection = new V3MainCollection();
            collection.Add(first);
            collection.Add(second);
            Console.WriteLine(collection.ToLongString("G4"));
            for(int i = 0; i < collection.Count; i++)
            {
                Console.WriteLine($"For {i} Count2:{collection[i].Count} MaxDistance2:{collection[i].MaxDistance}");
            }
        }
    }
}
