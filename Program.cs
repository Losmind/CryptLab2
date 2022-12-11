using System;

namespace CryptLab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string reg1 = "111"; // 2 степень
            string reg2 = "1011"; // 3 степень
            string reg3 = "100101"; // 5 степень
            string reg4 = "10000011"; // 7 степень
            Console.WriteLine($"Регистр 2 степени: {reg1}");
            Console.WriteLine($"Регистр 3 степени: {reg2}");
            Console.WriteLine($"Регистр 5 степени: {reg3}");
            Console.WriteLine($"Регистр 7 степени: {reg4}");
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
            Console.WriteLine($"Итоговая последовательность: {z}");
            byte[] data = new byte[z.Length];
            int tmp;
            for (int i = 0; i < z.Length; i++)
            {
                tmp = Convert.ToInt32(z[i]) - 48;
                data[i] = (byte)tmp;
            }
            Console.WriteLine($"Сложность: { BerlekampMassey(data)}");
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
