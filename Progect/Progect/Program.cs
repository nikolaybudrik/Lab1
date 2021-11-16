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
        static void ChekFile()
        {
            V3DataArray first = new V3DataArray("Str", DateTime.UtcNow, 4, 3, 1, 1, VectorMetods.foo);
            first.SaveBinary("test");
            V3DataArray second = null;
            V3DataArray.LoadBinary("test", ref second);

            Console.WriteLine(first.ToLongString("F3"));
            Console.WriteLine(second.ToLongString("F3"));
            Console.WriteLine();

            V3DataList list1 = new V3DataList("abc", DateTime.Now);
            list1.AddDefault(6, VectorMetods.foo);
            list1.SaveAsText("test1");
            V3DataList list2 = null;
            V3DataList.LoadAsText("test1", ref list2);

            Console.WriteLine(list1.ToLongString("F3"));
            Console.WriteLine(list2.ToLongString("F3"));
            Console.WriteLine();
        }

        static void ChekLinQ()
        {
            V3DataArray first = new V3DataArray("Str", DateTime.UtcNow, 4, 3, 1, 1, VectorMetods.foo);
            V3DataArray second = new V3DataArray("Str", DateTime.UtcNow, 2, 5, 1, 1, VectorMetods.foo2);
            V3DataArray therd = new V3DataArray("Str", DateTime.UtcNow, 0, 5, 1, 1, VectorMetods.foo2);
            V3DataList list1 = new V3DataList("abc", DateTime.Now);
            list1.AddDefault(6, VectorMetods.foo);
            V3DataList list2 = new V3DataList("abc", DateTime.Now);
            V3MainCollection collection = new V3MainCollection();
            Console.WriteLine("Для Пустой");
            if (collection.Diferens == null)
                Console.WriteLine("Null");
            Console.WriteLine(collection.Average);
            collection.Add(first);
            collection.Add(second);
            collection.Add(therd);
            collection.Add(list1);
            collection.Add(list2);
            Console.WriteLine("Для Заполненой");
            Console.WriteLine(collection.Average);
            Console.WriteLine();
            foreach(float item in collection.Diferens)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            foreach (IGrouping<double, DataItem> g in collection.Group_X)
            {
                Console.WriteLine(g.Key);
                foreach(DataItem item in g)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine();
            }

        }

        static void Main(string[] args)
        {
            ChekLinQ();
        }
    }
}
