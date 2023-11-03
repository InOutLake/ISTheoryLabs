using System;

namespace CSLAB1
{
    class Program
    {
        public static string func(string s, ref int a, out int d)
        {
            d = 2;
            s = s.Remove(0, a);
            a = s[0] - '0';
            return s.Remove(d, s.Count()-2);
        }
        public static void Main(string[] args)
        {
            Console.WriteLine("Минимальные и максимальные значения числовых форматов в C#:");

            Console.WriteLine("byte: {0} ... {1}", byte.MinValue, byte.MaxValue);
            Console.WriteLine("sbyte: {0} ... {1}", sbyte.MinValue, sbyte.MaxValue);
            Console.WriteLine("short: {0} ... {1}", short.MinValue, short.MaxValue);
            Console.WriteLine("ushort: {0} ... {1}", ushort.MinValue, ushort.MaxValue);
            Console.WriteLine("int: {0} ... {1}", int.MinValue, int.MaxValue);
            Console.WriteLine("uint: {0} ... {1}", uint.MinValue, uint.MaxValue);
            Console.WriteLine("long: {0} ... {1}", long.MinValue, long.MaxValue);
            Console.WriteLine("ulong: {0} ... {1}", ulong.MinValue, ulong.MaxValue);
            Console.WriteLine("float: {0} ... {1}", float.MinValue, float.MaxValue);
            Console.WriteLine("double: {0} ... {1}", double.MinValue, double.MaxValue);
            Console.WriteLine("decimal: {0} ... {1}", decimal.MinValue, decimal.MaxValue);


            Random random = new Random();
            while (true)
            {
                int start_val = random.Next(9);


                string  m = "String with n=" + start_val;
                int     a = int.Parse(m.Remove(0, m.Length - 1));
                char    c = (char)(m[0] - 'a');
                bool    b = a%2 > 0;
                byte    bn = (byte)c;
                decimal d = 123 - '0';
                double  db = a / 2.4;
                float   f = (float)db - bn;
                long    l = a * 1_523_532_234_643_000_000;
                sbyte   mbn = (sbyte)bn;
                        mbn *= -1;
                short   s = (short)mbn;
                uint    ui = (uint)c;
                ulong   ul = (ulong)l;
                ushort  us = 0;
                

                for (int i = 0; i < a; i++)
                {
                    if (i%3 == 0)
                    {
                        us += 1;
                    }
                }

                while (b)
                {
                    switch (bn%2) {
                        case 0:
                            bn += 1;
                            break;
                        case 1:
                            b = false;
                            break;
                    }
                }

                int k = 5;
                do
                {
                    f /= 2.1f;
                    k--;
                }
                while (m[k] != 'S');


                int outnum;
                m = func(m, ref a, out outnum);

                Console.WriteLine(m);
                Console.WriteLine(a);
                Console.WriteLine(c);
                Console.WriteLine(b);
                Console.WriteLine(bn);
                Console.WriteLine(d);
                Console.WriteLine(db);
                Console.WriteLine(f);
                Console.WriteLine(l);
                Console.WriteLine(mbn);
                Console.WriteLine(mbn);
                Console.WriteLine(s);
                Console.WriteLine(ui);
                Console.WriteLine(ul);
                Console.WriteLine(us);
                Console.ReadLine();
            }
        }
    }
}





