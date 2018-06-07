using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.SetConsoleFont(5);
            Console.WriteLine("liczymy");
            var t =SystemIcons.Information;

            var lit = "abcdqepolsrc";
            var licznik = 0;
            var hsx = "";
            var znak ="";
            //zaq = document.getElementById('haslo').value;
            for (int i = 1; i <= 5; i += 2)
            {
                licznik++;
                if ((licznik % 2) == 0) { znak = "_"; } else { znak = "x"; }
                hsx += lit.Substring(i,  1) + znak;
            }
            hsx += hsx.Substring(hsx.Length - 3);//, hsx.Length);

            Console.WriteLine(hsx);

            //for(int sek = 0; sek <= 60; sek++)
            //{
            //    for(int pomoc = 0; pomoc <= 500; pomoc++)
            //    {
            //        int ile= ((sek * (sek - 1)) / 2) * (pomoc % 2);
            //        if (ile==861)
            //        {
            //            Console.WriteLine("sek: " + sek + " pomoc: " + pomoc);
            //        }
            //    }
            //}

            Console.Read();
        }
    }
}
