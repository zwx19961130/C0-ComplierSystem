using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            Interpreter i = new Interpreter();
            i.Read();
            i.interpret();
        }
    }
    class Interpreter
    {
        int p = 0;                      //程序寄存器
        int b = 1;                      //基本寄存器
        int t = 0;                      //栈顶寄存器
        public const int STACKSIZE = 200;               //程序栈长度
        StreamReader sr;
        public struct instruction
        {
            public string f;
            public int l;
            public int a;
        }
        List<instruction> code = new List<instruction>();
        public void Read()
        {
            sr = new StreamReader(System.Environment.CurrentDirectory + "p_code.txt", Encoding.Default);
            string line = sr.ReadLine();
            while (line != null)
            {
                string[] temp = line.Split(' ');
                instruction i = new instruction();
                i.f = temp[0];
                i.l = int.Parse(temp[1]);
                i.a = int.Parse(temp[2]);
                code.Add(i);
                line = sr.ReadLine();
            }
        }
        public int Base(int l, int b, int[] s)
        {

            int b1;
            b1 = b;
            while (l > 0)
            {
                b1 = s[b1];
                l--;
            }
            return b1;

        }
        public void interpret()
        {


            int[] s = new int[STACKSIZE];
            instruction i;
            

            s[0] = s[1] = s[2] = s[3] = 0;
            do
            {
                i = code[p];


                p++;
                switch (i.f)
                {
                    case "lit":
                        t = t + 1;
                        s[t] = i.a;
                        break;
                    case "opr":
                        switch (i.a)
                        {
                            case 0:
                                t = b - 1;        //返回
                                p = s[t + 3];
                                b = s[t + 2];
                                break;
                            case 1:
                                s[t] = -s[t];      //取反
                                break;
                            case 2:
                                t = t - 1;           //加法
                                s[t] = s[t] + s[t + 1];
                                break;
                            case 3:
                                t = t - 1;           //减法
                                s[t] = s[t] - s[t + 1];
                                break;
                            case 4:
                                t = t - 1;           //乘法
                                s[t] = s[t] * s[t + 1];
                                break;
                            case 5:
                                t = t - 1;           //除法
                                s[t] = s[t] / s[t + 1];
                                break;
                            case 6:
                                if (s[t] % 2 == 0) s[t] = 0;
                                else s[t] = 1;
                                break;
                            case 8:
                                t = t - 1;           //下面都是判断
                                if (s[t] == s[t + 1]) s[t] = 1;
                                else s[t] = 0;
                                break;
                            case 9:
                                t = t - 1;
                                if (s[t] == s[t + 1]) s[t] = 0;
                                else s[t] = 1;
                                break;
                            case 10:
                                t = t - 1;
                                if (s[t] < s[t + 1]) s[t] = 1;
                                else s[t] = 0;
                                break;
                            case 11:
                                t = t - 1;
                                if (s[t] >= s[t + 1]) s[t] = 1;
                                else s[t] = 0;
                                break;
                            case 12:
                                t = t - 1;
                                if (s[t] > s[t + 1]) s[t] = 1;
                                else s[t] = 0;
                                break;
                            case 13:
                                t = t - 1;
                                if (s[t] <= s[t + 1]) s[t] = 1;
                                else s[t] = 0;
                                break;
                            case 14:
                                Console.WriteLine(s[t]);       //输出
                                t = t - 1;
                                break;
                            case 15:
                                Console.WriteLine();
                                break;
                            case 16:
                                t = t + 1;                  //输入
                                //Console.Write("?");
                                Console.WriteLine("请输入一个字符");
                                string s_temp = Console.ReadLine();
                                s[t] = int.Parse(s_temp);
                                break;
                        }
                        break;
                    case "lod":
                        t = t + 1;
                        s[t] = s[Base(i.l, b, s) + i.a];
                        break;
                    case "sto":
                        s[Base(i.l, b, s) + i.a] = s[t];
                        t = t - 1;
                        break;
                    case "cal":
                        s[t + 1] = Base(i.l, b, s);
                        s[t + 2] = b;
                        s[t + 3] = p;
                        b = t + 1;
                        p = i.a;
                        break;
                    case "ini":
                        t = t + i.a;
                        break;
                    case "jmp":
                        p = i.a;
                        break;
                    case "jpc":
                        if (s[t] == 0) p = i.a;
                        t = t - 1;
                        break;
                }
            } while (p != 0);
           
            Console.Read();
        }
    }
}
