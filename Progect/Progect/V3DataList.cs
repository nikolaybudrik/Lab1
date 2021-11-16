using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progect
{
    class V3DataList : V3Data, IEnumerable<DataItem>
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
                double range = (X1 - X2) * (X1 - X2) + (Y1 - Y2) * (Y1 - Y2);
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
            for (int i = 0; i < count; i++)
            {
                ret += "\n";
                ret += ListData[i].ToLongString(format);
            }
            return ret;
        }
        public override IEnumerator<DataItem> GetEnumerator()
        {
            return ListData.GetEnumerator();
        }
        
        /*public bool SaveAsText(string filename)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filename);
                writer.WriteLine(count);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }*/
    }
}