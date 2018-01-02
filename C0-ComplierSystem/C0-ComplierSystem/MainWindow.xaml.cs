using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace C0_ComplierSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Compiler com;
        private Process p;
        bool runnable = false;


       

        private void ChooseFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.txt)|*.txt"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                //this.Text.Text = openFileDialog.FileName;
                StreamReader sr = new StreamReader(openFileDialog.FileName, Encoding.Default);
                String text = "";
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    text = text + line + "\r\n";
                }
                this.Input.Text = text;

                //读取文件并在System.Environment.CurrentDirectory + "/bin/code.txt"下面创建这个文件
                FileStream fs = new FileStream(System.Environment.CurrentDirectory + "code.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(text);
                sw.Flush();
                sw.Close();
                fs.Close();

            }
        }

        private void Compile_Click(object sender, RoutedEventArgs e)
        {
            runnable = false;
            FileStream fs = new FileStream(System.Environment.CurrentDirectory + "code.txt", FileMode.Create);
            
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(this.Input.Text);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();

            com = new Compiler();
            com.init();
            int error_num;
            string error_info, pcode;
            com.getInfo(out error_num, out error_info, out pcode);
            this.error.Text = "";
            this.pcode.Text = pcode;
            if (error_num > 0)
            {
                this.error.Text = "The program has " + error_num + " error!\r\n";
                this.error.Text += error_info;

            }
            else
            {
                this.error.Text = "Succeed!!!!\r\n";
                runnable = true;
            }
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {

            if (!runnable)
            {
                MessageBox.Show("无程序可运行");
                return;
            }
            if (p == null)
            {
                p = new System.Diagnostics.Process();
                p.StartInfo.FileName = System.Environment.CurrentDirectory + "interpreter.exe";
                p.Start();

            }
            else
            {
                if (p.HasExited) //是否正在运行
                {
                    p.Start();
                }
            }
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;


        }
    }








    //class Interpreter
    //{
    //    int p = 0;                      //程序寄存器
    //    int b = 1;                      //基本寄存器
    //    int t = 0;                      //栈顶寄存器
    //    public const int STACKSIZE = 200;               //程序栈长度
    //    StreamReader sr;
    //    public struct instruction
    //    {
    //        public string f;
    //        public int l;
    //        public int a;
    //    }
    //    List<instruction> code = new List<instruction>();
    //    public void Read()
    //    {
    //        sr = new StreamReader(System.Environment.CurrentDirectory + "p_code.txt", Encoding.Default);
    //        string line = sr.ReadLine();
    //        while (line != null)
    //        {
    //            string[] temp = line.Split(' ');
    //            instruction i = new instruction();
    //            i.f = temp[0];
    //            i.l = int.Parse(temp[1]);
    //            i.a = int.Parse(temp[2]);
    //            code.Add(i);
    //            line = sr.ReadLine();
    //        }
    //    }
    //    public int Base(int l, int b, int[] s)
    //    {

    //        int b1;
    //        b1 = b;
    //        while (l > 0)
    //        {
    //            b1 = s[b1];
    //            l--;
    //        }
    //        return b1;

    //    }
    //    public void interpret()
    //    {


    //        int[] s = new int[STACKSIZE];
    //        instruction i;
    //        //Console.WriteLine("Begin PL0");

    //        FileStream fs = new FileStream(System.Environment.CurrentDirectory + "result.txt", FileMode.Create);
    //        StreamWriter sw = new StreamWriter(fs);

            

    //        s[0] = s[1] = s[2] = s[3] = 0;
    //        do
    //        {
    //            i = code[p];


    //            p++;
    //            switch (i.f)
    //            {
    //                case "lit":
    //                    t = t + 1;
    //                    s[t] = i.a;
    //                    break;
    //                case "opr":
    //                    switch (i.a)
    //                    {
    //                        case 0:
    //                            t = b - 1;        //返回
    //                            p = s[t + 3];
    //                            b = s[t + 2];
    //                            break;
    //                        case 1:
    //                            s[t] = -s[t];      //取反
    //                            break;
    //                        case 2:
    //                            t = t - 1;           //加法
    //                            s[t] = s[t] + s[t + 1];
    //                            break;
    //                        case 3:
    //                            t = t - 1;           //减法
    //                            s[t] = s[t] - s[t + 1];
    //                            break;
    //                        case 4:
    //                            t = t - 1;           //乘法
    //                            s[t] = s[t] * s[t + 1];
    //                            break;
    //                        case 5:
    //                            t = t - 1;           //除法
    //                            s[t] = s[t] / s[t + 1];
    //                            break;
    //                        case 6:
    //                            if (s[t] % 2 == 0) s[t] = 0;
    //                            else s[t] = 1;
    //                            break;
    //                        case 8:
    //                            t = t - 1;           //下面都是判断
    //                            if (s[t] == s[t + 1]) s[t] = 1;
    //                            else s[t] = 0;
    //                            break;
    //                        case 9:
    //                            t = t - 1;
    //                            if (s[t] == s[t + 1]) s[t] = 0;
    //                            else s[t] = 1;
    //                            break;
    //                        case 10:
    //                            t = t - 1;
    //                            if (s[t] < s[t + 1]) s[t] = 1;
    //                            else s[t] = 0;
    //                            break;
    //                        case 11:
    //                            t = t - 1;
    //                            if (s[t] >= s[t + 1]) s[t] = 1;
    //                            else s[t] = 0;
    //                            break;
    //                        case 12:
    //                            t = t - 1;
    //                            if (s[t] > s[t + 1]) s[t] = 1;
    //                            else s[t] = 0;
    //                            break;
    //                        case 13:
    //                            t = t - 1;
    //                            if (s[t] <= s[t + 1]) s[t] = 1;
    //                            else s[t] = 0;
    //                            break;
    //                        case 14:
    //                            //Console.WriteLine(s[t]);       //输出
    //                            sw.WriteLine(s[t]);
    //                            t = t - 1;
    //                            break;
    //                        case 15:
    //                            //Console.WriteLine();
    //                            sw.WriteLine();
    //                            break;
    //                        case 16:
    //                            t = t + 1;                  //输入
    //                            //Console.Write("?");
    //                            sw.WriteLine("?");
    //                            //string s_temp = Console.ReadLine();
    //                            string s_temp = InputBox("请输入");

                                
                                

    //                            s[t] = int.Parse(s_temp);
    //                            break;
    //                    }
    //                    break;
    //                case "lod":
    //                    t = t + 1;
    //                    s[t] = s[Base(i.l, b, s) + i.a];
    //                    break;
    //                case "sto":
    //                    s[Base(i.l, b, s) + i.a] = s[t];
    //                    t = t - 1;
    //                    break;
    //                case "cal":
    //                    s[t + 1] = Base(i.l, b, s);
    //                    s[t + 2] = b;
    //                    s[t + 3] = p;
    //                    b = t + 1;
    //                    p = i.a;
    //                    break;
    //                case "ini":
    //                    t = t + i.a;
    //                    break;
    //                case "jmp":
    //                    p = i.a;
    //                    break;
    //                case "jpc":
    //                    if (s[t] == 0) p = i.a;
    //                    t = t - 1;
    //                    break;
    //            }
    //        } while (p != 0);
    //        //Console.WriteLine("PL0 END");
    //        //Console.Read();
    //        sw.Flush();
    //        sw.Close();
    //        fs.Close();
    //    }
    //}



    class Compiler
    {
        //相关宏定义
        public const int MAX_LENGTH = 10;               //单词串长度最大为10
        public const int MAX_LEV = 3;                   //层数为100
        public const int MAX_P = 300;                     //P-code数组的最大数
        public const int MAX_TABLE = 1000;               //符号表的最大长度
        public const int STACKSIZE = 1000;               //程序栈长度



        /// <summary>
        /// 枚举:符号集，代表各个属性
        /// </summary>

        public enum Type
        {
            CONST, VAR, PROCEDURE, IF, THEN, ELSE, WHILE, DO, BEGIN, END, REPEAT, UNTIL, READ, WRITE, CALL, ODD,//保留字
            LPAR, RPAR, COMMA, SEMI, POINT,  //分节符
            IDSY,   //标识符
            INTSY, //常数
            PLUSSY, MINUSSY, TIMES, DIVISY, EQL, GREATER, LESS, ASSIGNSY, NOTQUSY, NOTLESS, NOTGREATER  //运算符
        }
        public enum obj { constant, variable, procedur }           //符号表成员类型
        public enum fct { lit, opr, lod, sto, cal, ini, jmp, jpc }     //pcode 功能符号
        public struct table                                    //符号表结构体
        {
            public string name;
            public obj kind;
            public int val, level, adr, size;
        }
        //保留字数组
        string[] reserver = { "const", "var", "procedure", "if", "then", "else", "while", "do", "begin", "end", "repeat", "until", "read", "write", "call", "odd" };

        //pcode结构体
        public struct instruction
        {
            public fct f;
            public int l;
            public int a;
        }




        instruction[] code = new instruction[MAX_P];               //这个是P-code栈
        table[] Table = new table[MAX_TABLE];                      //这个是符号表
        List<Type> declbegsys = new List<Type>();                  //下面几个是各个子程序的合法前缀
        List<Type> statbegsys = new List<Type>();
        List<Type> faclbegsys = new List<Type>();
        List<Type> tempsetsys = new List<Type>();

        string token;                                              //用于词法分析，记录符号串
        char ch;                                                   //当前字符
        Type sym = new Type();                                     //词法分析结果，记录当前属性
        string id;                                                 //词法分析结果，标识符的名称
        int num;                                                   //词法分析结果，数据的值
        int point = 0;                                             //用于get_char中读取string
        int lev = 0, tx = 0, dx = 3;                               //用于控制table
        int cx;                                                    //p_code列表指针
        StreamReader sr;                                           //文件读取指针           
        int line_length = -1, line_num = 0;                        //读取文件的辅助
        string line;                                               //当前行内容
        int error_num = 0;
        string error_info = "";
        bool sw = false;
        public void getInfo(out int n, out string e, out string p)
        {
            n = error_num;
            e = error_info;
            p = "";
            for (int i = 0; i < cx; i++)
            {
                p += i + "\t" + code[i].f + "\t" + code[i].l + "\t" + code[i].a + "\r\n";
            }

        }

        public void output()
        {
            FileStream fs = new FileStream(System.Environment.CurrentDirectory + "p_code.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            //string text="";
            for (int i = 0; i < cx; i++)
            {
                sw.WriteLine(code[i].f + " " + code[i].l + " " + code[i].a);
            }
            // sw.WriteLine("Hello World!!!!");
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public void init()
        {

            sr = new StreamReader(System.Environment.CurrentDirectory + "code.txt", Encoding.Default);


            cx = 0;
            declbegsys.Add(Type.CONST);
            declbegsys.Add(Type.VAR);
            declbegsys.Add(Type.PROCEDURE);

            statbegsys.Add(Type.BEGIN);
            statbegsys.Add(Type.CALL);
            statbegsys.Add(Type.IF);
            statbegsys.Add(Type.WHILE);
            statbegsys.Add(Type.REPEAT);

            faclbegsys.Add(Type.IDSY);
            faclbegsys.Add(Type.INTSY);
            faclbegsys.Add(Type.LPAR);

            tempsetsys.Add(Type.POINT);
            tempsetsys.Add(Type.CONST);
            tempsetsys.Add(Type.VAR);
            tempsetsys.Add(Type.PROCEDURE);


            get_char();
            get_sym();

            block(0, statbegsys.Union(tempsetsys).ToList());
            output();
            sr.Close();
        }

        /// <summary>
        ///     错误信息，传入错误表示号，分析出行号和第几个数字，并记录当前错误信息
        /// </summary>
        /// <param name="e"></param>
        public void error(int e)
        {
            error_num++;
            error_info += "error:" + "行数:" + line_num + "  错误代码:" + e + "\r\n";
            Console.WriteLine("错误:   " + "行数:" + line_num + "第" + point + "个" + "     错误代码:" + e);
        }
        /// <summary>
        /// 获取字符，任务是读取文件，获取当前文件
        /// </summary>

        public void get_char()
        {
            if (sw) return;
            if (point >= line_length)
            {

                line = sr.ReadLine();
                while (line == "")
                {
                    Console.WriteLine(line);
                    line = sr.ReadLine();

                    line_num++;
                }
                if (line == null)
                {

                    error_info += "Program InComplete!\r\n";
                    Console.WriteLine("InComplete!");
                    ch = '.';
                    sw = true;
                    return;
                }
                line = line + " ";
                line_num++;
                Console.WriteLine(line);
                line_length = line.Length;
                point = 0;
            }
            ch = line[point];
            point++;

        }
        /// <summary>
        /// 识别当前字符是哪种类型
        /// </summary>
        public void get_sym()
        {

            token = "";

            while (ch == ' ' || ch == '\t')
            {
                get_char();
            }

            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))  //读取的是字母
            {

                while ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || ch >= '0' && ch <= '9')
                {
                    if (token.Length <= MAX_LENGTH) token += ch;


                    get_char();

                }
                bool flage = false;
                int i;
                for (i = 0; i < reserver.Length; i++)
                {

                    if (reserver[i] == token)
                    {

                        flage = true;
                        break;
                    }
                }
                if (flage)
                {
                    sym = (Type)i;

                }
                else
                {
                    sym = Type.IDSY;
                    id = token;

                }

            }
            else if (ch >= '0' && ch <= '9')//读取的是数字
            {

                while (ch >= '0' && ch <= '9')
                {
                    token += ch;
                    get_char();
                }
                int n = Convert.ToInt32(token);
                num = n;
                sym = Type.INTSY;
            }
            else         //读取的符号
            {


                if (ch == ':')
                {
                    get_char();
                    if (ch == '=')
                    {
                        sym = Type.ASSIGNSY;
                        get_char();
                    }
                    else
                    {
                        //error
                    }
                }
                else if (ch == '<')
                {
                    get_char();
                    if (ch == '=')
                    {
                        sym = Type.NOTGREATER;
                        get_char();
                    }
                    else if (ch == '>')
                    {
                        sym = Type.NOTQUSY;
                        get_char();
                    }
                    else
                    {
                        sym = Type.LESS;

                    }
                }
                else if (ch == '>')
                {
                    get_char();
                    if (ch == '=')
                    {
                        sym = Type.NOTLESS;
                        get_char();
                    }
                    else
                    {
                        sym = Type.GREATER;

                    }
                }
                else
                {
                    switch (ch)
                    {
                        case '+': sym = Type.PLUSSY; break;
                        case '-': sym = Type.MINUSSY; break;
                        case '*': sym = Type.TIMES; break;
                        case '/': sym = Type.DIVISY; break;
                        case '(': sym = Type.LPAR; break;
                        case ')': sym = Type.RPAR; break;
                        case '=': sym = Type.EQL; break;
                        case ',': sym = Type.COMMA; break;
                        case '.': sym = Type.POINT; break;
                        case ';': sym = Type.SEMI; break;
                    }
                    get_char();
                }

            }

        }

        /// <summary>
        ///  加载P_code，传入instruction的3个参数，创建instruction添加到code数组中
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void gen(fct x, int y, int z)
        {
            code[cx].f = x;
            code[cx].l = y;
            code[cx].a = z;
            cx++;
        }


        /// <summary>
        /// 测试当前字符是否符合当前子程序的前驱后继
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="n"></param>
        public void test(List<Type> s1, List<Type> s2, int n)
        {

            if (!s1.Contains(sym))
            {
                error(n);

                s1 = s1.Union(s2).ToList();
                while (!s1.Contains(sym))
                {
                    get_sym();
                    if (sw) break;
                }
            }
        }

        /// <summary>
        ///   把标识符放进table，只要传入标识符类型，然后方法根据全局变量录入符号。
        /// </summary>
        /// <param name="k"></param>
        public void enter(obj k)
        {
            tx = tx + 1;
            Table[tx].name = id;
            Table[tx].kind = k;
            switch (k)
            {
                case obj.constant:
                    if (num > Int32.MaxValue)
                    {
                        error(31);
                        num = 0;
                    }
                    Table[tx].val = num;
                    break;
                case obj.variable:
                    Table[tx].level = lev;
                    Table[tx].adr = dx;
                    dx++;
                    break;
                case obj.procedur:
                    Table[tx].level = lev;
                    break;
            }


        }

        /// <summary>
        /// 寻找符号在符号表中的位置
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int position(string ids)
        {
            Table[0].name = ids;
            for (int i = tx; i >= 0; i--)
            {
                if (ids == Table[i].name)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 声明常量子程序
        /// </summary>
        public void constdeclaration()
        {
            if (sym == Type.IDSY)
            {
                get_sym();
                if (sym == Type.EQL || sym == Type.ASSIGNSY)
                {
                    if (sym == Type.ASSIGNSY)
                    {
                        error(1);
                    }
                    get_sym();
                    if (sym == Type.INTSY)
                    {
                        enter(obj.constant);
                        get_sym();
                    }
                    else
                    {
                        error(2);
                    }
                }
                else
                {
                    error(3);
                }
            }
            else
            {
                error(4);
            }
        }

        /// <summary>
        /// 声明var子程序
        /// </summary>
        public void vardeclaration()
        {
            if (sym == Type.IDSY)
            {
                enter(obj.variable);
                get_sym();
            }
            else
            {
                error(4);
            }
        }

        /// <summary>
        /// 输出listcode子程序
        /// </summary>
        /// <param name="f"></param>
        public void listcode(int f)
        {
            for (int i = f; i < cx; i++)
            {
                Console.Write(code[i].f);
                Console.Write(code[i].l);
                Console.Write(code[i].a);
                Console.WriteLine();
            }

        }

        /// <summary>
        /// factor子程序
        /// </summary>
        /// <param name="fsys"></param>
        public void factor(List<Type> fsys)
        {
            // Console.WriteLine("Star-----factor");
            int i;
            List<Type> temp = new List<Type>();
            temp.Add(Type.RPAR);

            test(faclbegsys, fsys, 24);
            while (faclbegsys.Contains(sym))
            {
                if (sym == Type.IDSY)
                {

                    i = position(id);
                    if (i == 0)
                    {
                        error(11);
                    }
                    else
                    {

                        switch (Table[i].kind)
                        {
                            case obj.constant: gen(fct.lit, 0, Table[i].val); break;
                            case obj.variable: gen(fct.lod, lev - Table[i].level, Table[i].adr); break;
                            case obj.procedur: error(21); break;
                        }

                    }
                    get_sym();
                }
                else if (sym == Type.INTSY)
                {
                    if (num > Int32.MaxValue)          //输入数字过大
                    {
                        error(31);
                        num = 0;
                    }
                    gen(fct.lit, 0, num);
                    get_sym();
                }
                else if (sym == Type.LPAR)
                {
                    get_sym();
                    List<Type> tempset = temp.Union(fsys).ToList();
                    expression(tempset);
                    if (sym == Type.RPAR)
                    {
                        get_sym();
                    }
                    else error(23);

                }
                test(fsys, faclbegsys, 23);
            }
            // Console.WriteLine("End-----factor");
        }

        /// <summary>
        /// term子程序
        /// </summary>
        /// <param name="fsys"></param>
        public void term(List<Type> fsys)
        {
            //Console.WriteLine("Star-----term");
            List<Type> temp = new List<Type>();
            Type mulop;
            temp.Add(Type.TIMES);
            temp.Add(Type.DIVISY);
            List<Type> tempt = temp.Union(fsys).ToList();
            factor(tempt);
            while (temp.Contains(sym))
            {
                mulop = sym;
                get_sym();
                tempt = temp.Union(fsys).ToList();
                factor(tempt);
                if (mulop == Type.TIMES) gen(fct.opr, 0, 4);
                else gen(fct.opr, 0, 5);
            }
            //  Console.WriteLine("End-----term");
        }

        /// <summary>
        /// expression子程序
        /// </summary>
        /// <param name="fsys"></param>
        public void expression(List<Type> fsys)
        {
            // Console.WriteLine("Star-----expression");
            Type addop;
            List<Type> temp = new List<Type>();
            temp.Add(Type.PLUSSY);
            temp.Add(Type.MINUSSY);
            if (temp.Contains(sym))
            {
                addop = sym;
                get_sym();
                List<Type> tempt = temp.Union(fsys).ToList();
                term(tempt);
                if (addop == Type.MINUSSY)
                {
                    gen(fct.opr, 0, 1);
                }
            }
            else
            {
                List<Type> tempt = temp.Union(fsys).ToList();
                term(tempt);

            }
            while (temp.Contains(sym))
            {
                addop = sym;
                get_sym();
                term(fsys.Union(temp).ToList());
                if (addop == Type.PLUSSY) gen(fct.opr, 0, 2);
                else gen(fct.opr, 0, 3);


            }
            //Console.WriteLine("End-----expression");
        }

        /// <summary>
        /// condition子程序
        /// </summary>
        /// <param name="fsys"></param>
        public void condition(List<Type> fsys)
        {
            // Console.WriteLine("Star-----condition");
            Type relop;
            List<Type> temp = new List<Type>();
            temp.Add(Type.EQL);
            temp.Add(Type.NOTQUSY);
            temp.Add(Type.LESS);
            temp.Add(Type.GREATER);
            temp.Add(Type.NOTLESS);
            temp.Add(Type.NOTGREATER);
            if (sym == Type.ODD)
            {
                get_sym();
                expression(fsys);
                gen(fct.opr, 0, 6);
            }
            else
            {
                List<Type> tempt = temp.Union(fsys).ToList();
                expression(tempt);
                if (!temp.Contains(sym)) error(2);
                else
                {
                    //Console.WriteLine("11111"+sym);
                    relop = sym;
                    get_sym();
                    // Console.WriteLine("2222"+sym);
                    expression(fsys);

                    if (relop == Type.EQL) gen(fct.opr, 0, 8);
                    if (relop == Type.NOTQUSY) gen(fct.opr, 0, 9);
                    if (relop == Type.LESS) gen(fct.opr, 0, 10);
                    if (relop == Type.NOTLESS) gen(fct.opr, 0, 11);
                    if (relop == Type.GREATER) gen(fct.opr, 0, 12);
                    if (relop == Type.NOTGREATER) gen(fct.opr, 0, 13);

                }
            }
            // Console.WriteLine("End-----condition");
        }

        /// <summary>
        /// statement子程序
        /// </summary>
        /// <param name="fsys"></param>
        /// <param name="plev"></param>
        public void statement(List<Type> fsys, int plev)
        {

            int cx1, cx2, i;
            List<Type> temp1 = new List<Type>();
            List<Type> temp2 = new List<Type>();
            List<Type> temp3 = new List<Type>();
            List<Type> temp4 = new List<Type>();
            List<Type> temp5 = new List<Type>();
            List<Type> temp6 = new List<Type>();
            List<Type> temp7 = new List<Type>();
            List<Type> temp8 = new List<Type>();
            temp1.Add(Type.RPAR);
            temp1.Add(Type.COMMA);

            temp2.Add(Type.THEN);
            temp2.Add(Type.DO);

            temp3.Add(Type.SEMI);
            temp3.Add(Type.END);

            temp4.Add(Type.SEMI);

            temp5.Add(Type.DO);

            temp7.Add(Type.ELSE);
            temp8.Add(Type.UNTIL);
            temp8.Add(Type.SEMI);
            if (sym == Type.IDSY)
            {
                i = position(id);
                if (i == 0) error(11);
                else
                {
                    if (Table[i].kind != obj.variable)
                    {
                        error(12);
                        i = 0;
                    }
                }
                get_sym();
                if (sym == Type.ASSIGNSY) get_sym();
                else error(13);
                expression(fsys);
                if (i != -1)
                {
                    gen(fct.sto, plev - Table[i].level, Table[i].adr);
                }
            }
            else if (sym == Type.READ)
            {
                get_sym();
                if (sym != Type.LPAR) error(24);
                else
                {
                    do
                    {
                        get_sym();
                        if (sym == Type.IDSY) i = position(id);
                        else i = 0;
                        if (i == 0) error(35);
                        else
                        {
                            gen(fct.opr, 0, 16);
                            gen(fct.sto, plev - Table[i].level, Table[i].adr);
                        }
                        get_sym();
                    } while (sym == Type.COMMA);
                }
                if (sym != Type.RPAR)
                {
                    error(22);
                    while (!fsys.Contains(sym)) get_sym();
                }
                else get_sym();

            }
            else if (sym == Type.WRITE)
            {

                get_sym();
                if (sym != Type.LPAR) error(24);
                else
                {
                    do
                    {
                        get_sym();
                        List<Type> tempt = temp1.Union(fsys).ToList();
                        expression(tempt);
                        gen(fct.opr, 0, 14);
                    } while (sym == Type.COMMA);

                }

                if (sym != Type.RPAR) error(33);
                else get_sym();
            }
            else if (sym == Type.CALL)
            {
                get_sym();
                if (sym != Type.IDSY) error(14);
                else
                {
                    i = position(id);
                    if (i == 0) error(11);
                    else
                    {
                        if (Table[i].kind == obj.procedur)
                            gen(fct.cal, plev - Table[i].level, Table[i].adr);
                        else error(15);
                    }
                    get_sym();
                }
            }
            else if (sym == Type.IF)
            {
                get_sym();
                List<Type> tempt = temp2.Union(fsys).ToList();
                condition(tempt);
                if (sym == Type.THEN) get_sym();
                else error(16);
                cx1 = cx;

                gen(fct.jpc, 0, 0);
                statement(temp7.Union(fsys).ToList(), plev);

                if (sym == Type.ELSE)
                {
                    int temp_cx = cx;
                    gen(fct.jmp, 0, 0);
                    code[cx1].a = cx;
                    get_sym();

                    statement(fsys, plev);

                    code[temp_cx].a = cx;
                }
                else
                {
                    code[cx1].a = cx;
                }



            }
            else if (sym == Type.BEGIN)
            {
                get_sym();
                List<Type> tempt = temp3.Union(fsys).ToList();
                statement(tempt, plev);
                while (temp4.Union(statbegsys).ToList().Contains(sym))
                {
                    if (sym == Type.SEMI) get_sym();
                    else error(10);
                    statement(temp3.Union(fsys).ToList(), plev);
                }
                if (sym == Type.END)
                {
                    get_sym();
                }
                else
                {
                    error(17);
                }
            }
            else if (sym == Type.REPEAT)
            {
                get_sym();
                List<Type> tempt = temp8.Union(fsys).ToList();
                int cx_temp = cx;
                statement(tempt, plev);
                while (temp4.Union(statbegsys).ToList().Contains(sym))
                {
                    if (sym == Type.SEMI) get_sym();
                    else error(10);
                    statement(temp8.Union(fsys).ToList(), plev);
                }
                if (sym == Type.UNTIL)
                {
                    get_sym();
                    condition(fsys);
                    gen(fct.jpc, 0, cx_temp);
                }
                else
                {
                    error(19);
                }
            }
            else
            {
                if (sym == Type.WHILE)
                {
                    cx1 = cx;
                    get_sym();
                    condition(temp5.Union(fsys).ToList());
                    cx2 = cx;
                    gen(fct.jpc, 0, 0);
                    if (sym == Type.DO)
                    {
                        get_sym();
                    }
                    else error(18);
                    statement(fsys, plev);
                    gen(fct.jmp, 0, cx1);

                    code[cx2].a = cx;
                }
            }
            test(fsys, temp6, 19);

        }

        /// <summary>
        /// block子程序
        /// </summary>
        /// <param name="plev"></param>
        /// <param name="fsys"></param>
        public void block(int plev, List<Type> fsys)
        {

            int dx0 = MAX_LEV;
            int tx0, cx0;
            List<Type> temp1 = new List<Type>();
            List<Type> temp2 = new List<Type>();
            List<Type> temp3 = new List<Type>();
            List<Type> temp4 = new List<Type>();
            List<Type> temp5 = new List<Type>();

            temp1.Add(Type.SEMI);
            temp1.Add(Type.END);

            temp2.Add(Type.IDSY);
            temp2.Add(Type.PROCEDURE);

            temp3.Add(Type.SEMI);

            temp4.Add(Type.IDSY);

            lev = plev;
            tx0 = tx;
            Table[tx].adr = cx;
            gen(fct.jmp, 0, 1);
            if (plev > MAX_LEV) error(32);
            do
            {
                if (sym == Type.CONST)
                {

                    get_sym();
                    do
                    {

                        constdeclaration();
                        while (sym == Type.COMMA)
                        {
                            get_sym();
                            constdeclaration();
                        }
                        if (sym == Type.SEMI) get_sym();
                        else error(5);

                    } while (sym == Type.IDSY);
                }
                if (sym == Type.VAR)
                {
                    get_sym();
                    do
                    {
                        dx0++;
                        vardeclaration();
                        while (sym == Type.COMMA)
                        {
                            get_sym();
                            dx0++;
                            vardeclaration();
                        }
                        if (sym == Type.SEMI) get_sym();
                        else error(5);
                    } while (sym == Type.IDSY);
                }
                while (sym == Type.PROCEDURE)
                {
                    get_sym();
                    if (sym == Type.IDSY)
                    {
                        enter(obj.procedur);
                        get_sym();
                    }
                    else error(4);
                    if (sym == Type.SEMI) get_sym();
                    else error(5);
                    dx += 3;
                    block(plev + 1, temp3.Union(fsys).ToList());
                    lev = lev - 1;
                    if (sym == Type.SEMI)
                    {
                        get_sym();
                        test(statbegsys.Union(temp2).ToList(), fsys, 6);
                    }
                    else error(5);
                }
                test(statbegsys.Union(temp4).ToList(), declbegsys, 7);
            } while (declbegsys.Contains(sym));

            code[Table[tx0].adr].a = cx;

            Table[tx0].adr = cx;
            Table[tx0].size = dx0;
            cx0 = cx;
            gen(fct.ini, 0, dx0);



            statement(temp1.Union(fsys).ToList(), plev);

            gen(fct.opr, 0, 0);
            test(fsys, temp5, 8);

            listcode(cx0);

        }
    }

}

