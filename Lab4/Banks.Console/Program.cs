using Banks.UI;

namespace Banks.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new AppManager().Run();
            /*var one = new One(new Temp("temp1str1", "temp1str2"), new Temp("temp2str1", "temp2str2"));
            var two = new Two(one.Temp1);
            System.Console.WriteLine(one.Temp1.Str1);
            System.Console.WriteLine(two.GetTemp.Str1);
            one.Temp1.Str1 = "temp1str1_new";
            System.Console.WriteLine(one.Temp1.Str1);
            System.Console.WriteLine(two.GetTemp.Str1);*/
        }

        public class One
        {
            public One(Temp temp1, Temp temp2)
            {
                Temp1 = temp1;
                Temp2 = temp2;
            }

            public Temp Temp1 { get; internal set; }
            public Temp Temp2 { get; internal set; }
        }

        public class Two
        {
            private readonly Temp _temp;
            public Two(Temp temp)
            {
                _temp = temp;
            }

            public Temp GetTemp => _temp;
        }

        public class Temp
        {
            public Temp(string str1, string str2)
            {
                Str1 = str1;
                Str2 = str2;
            }

            public string Str1 { get; internal set; }
            public string Str2 { get; internal set; }
        }
    }
}
