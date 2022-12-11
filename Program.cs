using System;

namespace CryptLab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                string reg1 = "111"; // 2 степень
                string reg2 = "1011"; // 3 степень
                string reg3 = "111101"; // 5 степень
                string reg4 = "10111111"; // 7 степень  11100101
                Console.WriteLine($"Регистр 2 степени: {reg1}");
                Console.WriteLine($"Регистр 3 степени: {reg2}");
                Console.WriteLine($"Регистр 5 степени: {reg3}");
                Console.WriteLine($"Регистр 7 степени: {reg4}\r\n");
                string z = ""; // выходная последовательность
                int n; // длина последовательности
                char zi;
                char a1, a2, a3;
                char CON1, CON2, CON3;
                char XOR1;
                char new1reg1, new1reg2, new1reg3, new1reg4;
                Console.WriteLine("Введите длину последовательности: ");
                n = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < n; i++)
                {
                    a1 = XOR(reg1[2], reg2[3]);
                    a2 = reg3[5];
                    a3 = reg4[7];
                    CON1 = CON(a1, a2);
                    CON2 = CON(a1, a3);
                    CON3 = CON(a2, a3);
                    XOR1 = XOR(CON1, CON2);
                    zi = XOR(XOR1, CON3);
                    z = z + zi;
                    if (reg1[2] == zi)
                    {
                        new1reg1 = XOR(reg1[1], reg1[2]);
                        reg1 = reg1.Remove(reg1.Length - 1);
                        reg1 = new1reg1 + reg1;
                    }
                    if (reg2[3] == zi)
                    {
                        new1reg2 = XOR(reg2[2], reg2[3]);
                        reg2 = reg2.Remove(reg2.Length - 1);
                        reg2 = new1reg2 + reg2;
                    }
                    if (reg3[5] == zi)
                    {
                        new1reg3 = XOR(reg3[5], reg3[3]);
                        reg3 = reg3.Remove(reg3.Length - 1);
                        reg3 = new1reg3 + reg3;
                    }
                    if (reg4[7] == zi)
                    {
                        new1reg4 = XOR(reg4[6], reg4[7]);
                        reg4 = reg4.Remove(reg4.Length - 1);
                        reg4 = new1reg4 + reg4;
                    }
                }
                Console.WriteLine($"\r\nИтоговая последовательность: {z}");
                byte[] data = new byte[z.Length];
                int tmp;
                for (int i = 0; i < z.Length; i++)
                {
                    tmp = Convert.ToInt32(z[i]) - 48;
                    data[i] = (byte)tmp;
                }
                Console.WriteLine($"Сложность: {BerlekampMassey(data)}");
                NIST(data);

                int counter = 1;
                for (int i = 0; i < z.Length - 1; i++)
                {
                    if ((z[i] == '0' && z[i + 1] == '1') || (z[i] == '1' && z[i + 1] == '0'))
                    {
                        counter++;
                    }
                }
                Console.WriteLine($"Количество серий: {counter}");

                string sv = "";

                string sl = "";
                int c = 1;
                sv += z[0];

                for (int i = 1; i < z.Length; i++)
                {
                    if (z[i] == z[i - 1])
                    {
                        c += 1;
                    }
                    else
                    {
                        sl += c;
                        c = 1;
                        sv += z[i];
                    }
                }
                sl += c;
                c = 1;
                sv += z[z.Length - 1];

                //for (int i = 0; i < sl.Length; i++)
                //{
                //    Console.WriteLine($"{sv[i]} - {sl[i]}");
                //}

                int[] count = new int[9];

                for (int i = 0; i < sl.Length; i++)
                {
                    if (sl[i] == '1')
                    {
                        count[0] += 1;
                    }
                    else if (sl[i] == '2')
                    {
                        count[1] += 1;
                    }
                    else if (sl[i] == '3')
                    {
                        count[2] += 1;
                    }
                    else if (sl[i] == '4')
                    {
                        count[3] += 1;
                    }
                    else if (sl[i] == '5')
                    {
                        count[4] += 1;
                    }
                    else if (sl[i] == '6')
                    {
                        count[5] += 1;
                    }
                    else if (sl[i] == '7')
                    {
                        count[6] += 1;
                    }
                    else if (sl[i] == '8')
                    {
                        count[7] += 1;
                    }
                    else if (sl[i] == '9')
                    {
                        count[8] += 1;
                    }
                }

                bool yop = true;
                for (int i = 0, j = 1; i < count.Length; i++, j++)
                {
                    if (count[i] != 0)
                    {
                        Console.WriteLine($"Количество серий длиной {i + 1} = {count[i]}");
                    }
                    if (count[i] >= (counter / (Math.Pow(2, j))))
                    {
                        yop = false;
                    }
                }

                if (yop == false)
                {
                    Console.WriteLine("Второй постулат не выполняется.");
                }
                else
                {
                    Console.WriteLine("Второй постулат выполняется.");
                }

                Console.ReadLine();
            }
        }


        static char XOR(char a, char b)
        {
            char result;
            if (a == b) result = '0';
            else result = '1';
            return result;
        }
        static char CON(char a, char b)
        {
            char result;
            if (a == '1' && b == '1') result = '1';
            else result = '0';
            return result;
        }
        public static int BerlekampMassey(byte[] s)
        {
            int L, N, m, d;
            int n = s.Length;
            byte[] c = new byte[n];
            byte[] b = new byte[n];
            byte[] t = new byte[n];

            //Initialization
            b[0] = c[0] = 1;
            N = L = 0;
            m = -1;

            //Algorithm core
            while (N < n)
            {
                d = s[N];
                for (int i = 1; i <= L; i++)
                    d ^= c[i] & s[N - i]; //(d+=c[i]*s[N-i] mod 2)
                if (d == 1)
                {
                    Array.Copy(c, t, n); //T(D)<-C(D)
                    for (int i = 0; (i + N - m) < n; i++)
                        c[i + N - m] ^= b[i];
                    if (L <= (N >> 1))
                    {
                        L = N + 1 - L;
                        m = N;
                        Array.Copy(t, b, n); //B(D)<-T(D)
                    }
                }
                N++;
            }
            return L;
        }
    }
}
