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
            return new Vector2((float) -x, (float)(-y));
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
            Console.WriteLine("Проверка Бинарного сохранения");
            Console.WriteLine();
            V3DataArray first = new V3DataArray("Str", DateTime.UtcNow, 2, 2, 1, 1, VectorMetods.foo);
            first.SaveBinary("testbin");
            V3DataArray second = null;
            V3DataArray.LoadBinary("testbin", ref second);

            Console.WriteLine(first.ToLongString("F3"));
            Console.WriteLine();
            Console.WriteLine(second.ToLongString("F3"));
            Console.WriteLine();

            Console.WriteLine("Проверка Текстового сохранения");
            Console.WriteLine();
            V3DataList list1 = new V3DataList("abc", DateTime.Now);
            list1.AddDefault(3, VectorMetods.foo);
            list1.SaveAsText("testtext.txt");
            V3DataList list2 = null;
            V3DataList.LoadAsText("testtext.txt", ref list2);

            Console.WriteLine(list1.ToLongString("F3"));
            Console.WriteLine();
            Console.WriteLine(list2.ToLongString("F3"));
            Console.WriteLine();
        }

        static void ChekLinQ()
        {
            Console.WriteLine("Проверка Свойств, Linq");
            V3DataArray first = new V3DataArray("first", DateTime.UtcNow, 1, 3, 1, 1, VectorMetods.foo);
            V3DataArray second = new V3DataArray("second", DateTime.UtcNow, 3, 1, 1, 1, VectorMetods.foo2);
            V3DataArray therd = new V3DataArray("therd", DateTime.UtcNow, 0, 5, 1, 1, VectorMetods.foo2);
            V3DataList list1 = new V3DataList("list1", DateTime.Now);
            list1.Add(new DataItem(1, 0, new Vector2(2, 2)));
            list1.Add(new DataItem(3, 0, new Vector2(2, 2)));
            V3DataList list2 = new V3DataList("list2", DateTime.Now);
            V3MainCollection collection = new V3MainCollection();
            Console.WriteLine("Для Пустой Коллекции:");
            Console.WriteLine();
            Console.WriteLine("Проверка на возвращения double.NaN для свойства Average");
            Console.WriteLine(collection.Average);
            Console.WriteLine();
            Console.WriteLine("Проверка на возвращения null для свойства Diferens");
            if (collection.Diferens == null)
                Console.WriteLine("Null");
            Console.WriteLine();
            Console.WriteLine("Проверка на возвращения null для свойства Group_X");
            if (collection.Group_X == null)
                Console.WriteLine("Null");
            collection.Add(first);
            collection.Add(second);
            collection.Add(therd);
            collection.Add(list1);
            collection.Add(list2);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Для Заполненой коллекции:");
            Console.WriteLine(collection.ToLongString("F3"));
            Console.WriteLine();
            Console.WriteLine("Проверка свойства Average:");
            Console.WriteLine($"{collection.Average} = (0 + 0 + 1 + 1 + 1 + 2 + 2 + 2) / 8");
            Console.WriteLine();
            Console.WriteLine("Проверка свойства Diferens:");
            int i = 0;
            foreach (float item in collection.Diferens)
            {
                i++;
                Console.WriteLine($"Для {i}-ого набора разница растояний {item}");
            }
            Console.WriteLine();
            Console.WriteLine("Проверка свойства Group_X:");
            foreach (IGrouping<double, DataItem> g in collection.Group_X)
            {
                Console.WriteLine($"для x = {g.Key} всe dataitems:");
                foreach(DataItem item in g)
                {
                    Console.WriteLine(item.ToLongString("F3"));
                }
                Console.WriteLine();
            }

        }

        static void Main(string[] args)
        {
            ChekFile();
            ChekLinQ();
        }
    }
}
