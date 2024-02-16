using System;
using System.Threading;

namespace Discord_EHooker
{
    // kiddies gon be like: MADE BY ME: Xx_AN0NYM0U5_H4CK3R_BL1TZ_445_xX
    internal class Program
    {
        static void Main(string[] args)
        {
            //This shit does nothing... why tf did i add this
            Console.SetWindowPosition(0, 0);
            Console.Title = "Discord E-Hooker";
            Functions.WriteHookerMessage("Initializing... Please Wait...");
            Menus.HomeMenu();
        }    
    }
}