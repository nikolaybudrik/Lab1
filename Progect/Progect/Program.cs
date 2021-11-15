using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.Intrinsics;

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
