using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCore
{
    public static class Converter
    {
        //ToDo Check Functions
        //ToDo Add Design
        #region From_Decimal
        public static string From_Decimal_To_B(double num)
        {
            string fract = Convert_Fract(num - Math.Truncate(num), 2);
            int number = (int)num;
            if (num < 1 && num > 0)
            {
                return Convert_Fract((double)num, 2);
            }
            bool isNegative = false;
            if (number < 0)
            {
                isNegative = true;
                number = -number;
            }
            int sys = 2;
            string res = "";
            int rest = 0;
            while (number >= sys)
            {
                rest = (int)number % sys;
                res += rest;

                number /= sys;
            }
            res += number;
            if (isNegative)
            {
                res = String.Join("", res.Reverse().ToArray());
                if (res.Length < 4)
                {
                    res = new string('0', 4 - res.Length) + res;
                }
                res = res.Replace('0', 'z');
                res = res.Replace('1', '0');
                res = res.Replace('z', '1');
                res = Add_1(res);
                return "1." + res;
            }
            return String.Join("", res.Reverse().ToArray()) + fract.Substring(1);
        }
        public static string From_Decimal_To_B(int num)
        {
            int number = (int)num;
            bool isNegative = false;
            if (number < 0)
            {
                isNegative = true;
                number = -number;
            }
            int sys = 2;
            string res = "";
            int rest = 0;
            while (number >= sys)
            {
                rest = (int)number % sys;
                res += rest;

                number /= sys;
            }
            res += number;
            if (isNegative)
            {
                res = String.Join("", res.Reverse().ToArray());
                if (res.Length < 4)
                {
                    res = new string('0', 4 - res.Length) + res;
                }
                res = res.Replace('0', 'z');
                res = res.Replace('1', '0');
                res = res.Replace('z', '1');
                res = Add_1(res);
                return "1." + res;
            }
            return String.Join("", res.Reverse().ToArray());
        }
        public static string From_Decimal_To_Sys(double num, int sys)
        {
            if (sys == 16) return From_Decimal_To_16(num);
            string fract = Convert_Fract(num - Math.Truncate(num), sys);
            if (num < 1)
            {
                return Convert_Fract(num, sys);
            }
            int number = (int)num;
            string res = "";
            int rest = 0;
            while (number >= sys)
            {
                rest = (int)number % sys;
                res += rest;

                number /= sys;
            }
            res += number;
            return String.Join("", res.Reverse().ToArray()) + fract.Substring(1);
        }
        public static string From_Decimal_To_Sys(int num, int sys)
        {
            if (num == 0) return "0";
            if (sys == 16) return From_Decimal_To_16(num);

            if (num < 1)
            {
                return Convert_Fract(num, sys);
            }
            int number = (int)num;
            string res = "";
            int rest = 0;
            while (number >= sys)
            {
                rest = (int)number % sys;
                res += rest;

                number /= sys;
            }
            res += number;
            return String.Join("", res.Reverse().ToArray());
        }
        public static string From_Decimal_To_16(double num)
        {
            string fract = Convert_Fract(num - Math.Truncate(num), 16);
            if (num < 1)
            {
                return Convert_Fract(num, 16);
            }
            int number = (int)num;
            int sys = 16;
            string res = "";
            if (number < sys)
                return number.ToHex();
            int rest = 0;
            while (number >= sys)
            {
                rest = number % sys;
                res += rest.ToHex();
                number /= sys;
            }
            res += number.ToHex();
            return String.Join("", res.Reverse().ToArray()) + fract.Substring(1);
        }
        public static string From_Decimal_To_16(int num)
        {

            int number = num;
            int sys = 16;
            string res = "";
            if (number < sys)
                return number.ToHex();
            int rest = 0;
            while (number >= sys)
            {
                rest = number % sys;
               
                res += rest.ToHex();
                number /= sys;
            }
            res += number.ToHex();
            return String.Join("", res.Reverse().ToArray());
        }


        #endregion
        #region From_Binnary
        public static string From_B_To_8(string num)
        {
            StringBuilder res = new StringBuilder();
            if (num.Length % 3 == 0)
            {
                for (int i = 0; i < num.Length; i += 3)
                {
                    res.Append(From_B_To_10(num.Substring(i, 3)).ToString());
                }
            }
            else if (num.Length % 3 == 1)
            {
                num = "00" + num;
                for (int i = 0; i < num.Length; i += 3)
                {
                    res.Append(From_B_To_10(num.Substring(i, 3)).ToString());
                }
            }
            else if (num.Length % 3 == 2)
            {
                num = '0' + num;
                for (int i = 0; i < num.Length; i += 3)
                {
                    res.Append(From_B_To_10(num.Substring(i, 3)).ToString());
                }
            }
            return res.ToString();
        }
        public static double From_B_To_10(string number)
        {
            string num = "";
            string rem = "";
            if (number.Contains('.'))
            {
                num = number.Split('.')[0];
                rem = number.Split('.')[1];
            }
            else
                num = number;

            double res = 0;
            for (int i = 0; i < num.Length; i++)
            {
                res += (int)((num[i].FromChar() * Math.Pow(2, num.Length - 1 - i)));
            }
            if (rem != "")
            {
               
                for (int i = 0; i < rem.Length; i++)
                {

                    res += ((rem[i].FromChar() * Math.Pow(2, -(i + 1))));
                }
            }
            return res;
        }
        public static string From_B_To_16(string num)
        {
            StringBuilder res = new StringBuilder();
            if (num.Length % 4 == 0)
            {
                for (int i = 0; i < num.Length; i += 4)
                {
                    res.Append(((int)From_B_To_10(num.Substring(i, 4))).ToHex());
                }
            }
            else if (num.Length % 4 == 1)
            {
                num = "000" + num;
                for (int i = 0; i < num.Length; i += 4)
                {
                    res.Append(((int)From_B_To_10(num.Substring(i, 4))).ToHex());
                }
            }
            else if (num.Length % 4 == 2)
            {
                num = "00" + num;
                for (int i = 0; i < num.Length; i += 4)
                {
                    res.Append(((int)From_B_To_10(num.Substring(i, 4))).ToHex());
                }
            }
            else
            {
                num = "0" + num;
                for (int i = 0; i < num.Length; i += 4)
                {
                    res.Append(((int)From_B_To_10(num.Substring(i, 4))).ToHex());
                }
            }
            return res.ToString();
        }
        #endregion
        #region From_Octal
        public static int From_Octal_To_Dec(string num)
        {
            int res = 0;
            for (int i = 0; i < num.Length; i++)
            {
                res += (int)((num[i].FromChar() * Math.Pow(8, num.Length - 1 - i)));
            }
            return res;
        }
        public static string From_Octal_To_B(string num)
        {
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < num.Length; i++)
            {
                string temp = From_Decimal_To_B(num[i].FromChar());
                while (temp.Length != 3)
                    temp = "0" + temp;

                res.Append(temp);
            }
            return res.ToString();
        }
        public static string From_Octal_To_Hex(string num)
        {
            return From_B_To_16(From_Octal_To_B(num));
        }
        #endregion

        #region From_Hexadecimal

        public static int From_Hex_To_D(string num)
        {
            int res = 0;
            for (int i = 0; i < num.Length; i++)
            {
                int val = num[i].ToString().FromHex();
                res += (int)((val * Math.Pow(16, num.Length - 1 - i)));
            }
            return res;
        }

        public static string From_Hex_To_B(string num)
        {
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < num.Length; i++)
            {
                string temp = From_Decimal_To_B(num[i].ToString().FromHex());
                while (temp.Length < 4)
                {
                    temp = "0" + temp;
                }
                res.Append(temp);
            }
            while (res[0] == '0') res.Remove(0, 1);
            return res.ToString();
        }
        public static string From_Hex_To_Octal(string num)
        {
            return From_B_To_8(From_Hex_To_B(num));
        }
        #endregion
        public static double From_Sys_To_10(string number, int sys)
        {
            if (number == "") return 0;
            string num = "";
            string rem = "";
            if (number.Contains('.'))
            {
                num = number.Split('.')[0];
                rem = number.Split('.')[1];
            }
            else
                num = number;

            double res = 0;
            for (int i = 0; i < num.Length; i++)
            {
                if (sys == 16)
                {
                    res += (int)((num[i].FromHex() * Math.Pow(sys, num.Length - 1 - i)));
                }
                else
                    res += (int)((num[i].FromChar() * Math.Pow(sys, num.Length - 1 - i)));
            }

            if (rem != "")
            {

                for (int i = 0; i < rem.Length; i++)
                {

                    res += ((rem[i].FromChar() * Math.Pow(2, -(i + 1))));
                }
                return res;
            }
            return (int)res;
        }
        public static string To_Add_Code(string res)
        {
            if (res.Length < 4)
            {
                res = new string('0', 4 - res.Length) + res;
            }
            res = res.Replace('0', 'z');
            res = res.Replace('1', '0');
            res = res.Replace('z', '1');
            res = Add_1(res);
            return res;
        }

        private static string Add_1(string str)
        {
            StringBuilder builder = new StringBuilder(str);

            for (int i = builder.Length - 1; i >= 0; i--)
            {
                if (i == 0 && builder[i] == '1')
                {
                    builder[i] = '0';
                    builder.Insert(0, '1');
                }

                if (builder[i] == '0')
                {
                    builder[i] = '1';
                    break;
                }
                if (builder[i] == '1')
                {
                    builder[i] = '0';
                }

            }

            return builder.ToString();
        }
        private static string Convert_Fract(double num, double sys)
        {
            string res = "";
            for (int i = 0; i < 6; i++)
            {
                int temp = (int)Math.Truncate(num * sys);
                if (sys == 16)
                    res += temp.ToHex();
                else
                    res += temp;
                num = num * sys - Math.Truncate(num * sys);
            }
            return "0." + res;
        }
    }

    public static class MyExtensions
    {
        public static Dictionary<int, string> HexDict;
        static MyExtensions()
        {
            Prepare_Dict();
        }
        public static int FromChar(this char x)
        {
            return x - '0';
        }
        public static int FromChar(this char x, int sys)
        {
            if (sys == 16) return x.FromHex();
            return x - '0';
        }
        public static string ToHex(this int num)
        {
            if (num < 10)
                return num.ToString();
            else
                return HexDict[num];
        }
        public static int FromHex(this string num)
        {
            if (num == "") return 0;
            int res = 0;
            bool Isnum = int.TryParse(num, out res);
            if (!Isnum)
            {
                res = HexDict.Where(x => x.Value == num).Select(x => x.Key).FirstOrDefault();
            }
            return res;
        }
        public static int FromHex(this char num)
        {
            if (num.ToString() == "") return 0;
            int res = 0;
            bool Isnum = int.TryParse(num.ToString(), out res);
            if (!Isnum)
            {
                res = HexDict.Where(x => x.Value == num.ToString()).Select(x => x.Key).FirstOrDefault();
            }
            return res;
        }
        private static void Prepare_Dict()
        {
            HexDict = new Dictionary<int, string>();
            HexDict.Add(10, "A");
            HexDict.Add(11, "B");
            HexDict.Add(12, "C");
            HexDict.Add(13, "D");
            HexDict.Add(14, "E");
            HexDict.Add(15, "F");


        }
        public static string[] MyToArray(this string str)
        {
            string[] res = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                res[i] = str[i].ToString();
            }
            return res;
        }

        public static int FromString(this string str, int sys)
        {
            if (sys == 16) return str.FromHex();
            else return int.Parse(str);
        }

    }
}
