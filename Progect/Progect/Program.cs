using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Linq;

namespace Progect
{
    delegate Vector2 FdblVector2(double x, double y);

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
            V3DataArray first = new V3DataArray("Str", DateTime.UtcNow, 4, 4, 1, 1, VectorMetods.foo);
            V3DataArray first2 = new V3DataArray("B", DateTime.Now, 2, 2, 2, 2, VectorMetods.foo);
            /*Console.WriteLine(first.ToLongString("e3"));
            V3DataList firstList = (V3DataList)first;
            Console.WriteLine(firstList.ToLongString("e3"));
            Console.WriteLine($"Count1:{first.Count} MaxDistance1:{first.MaxDistance}");
            Console.WriteLine($"Count2:{firstList.Count} MaxDistance2:{firstList.MaxDistance}");
            V3DataList second = new V3DataList("B", DateTime.UtcNow);
            Console.WriteLine(second.AddDefault(8, VectorMetods.foo2));*/
            V3MainCollection collection = new V3MainCollection();
            collection.Add(first);
            collection.Add(first2);
            /*Console.WriteLine(first.GetEnumerator());
            foreach(DataItem el in first)
            {
                Console.WriteLine(el);
            }*/
            /*foreach (IGrouping<double, DataItem> t in collection.Group_X)
            {
                Console.WriteLine(t.Key);
                foreach (var h in t)
                    Console.WriteLine(h);
                Console.WriteLine();
            }*/
            first.SaveBinary("test");
            first2.LoadBinary("test");
            Console.WriteLine(first2);

        }
    }
}
