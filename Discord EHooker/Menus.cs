using System;
using System.Threading;

namespace Discord_EHooker
{
    internal class Menus
    {
        //wow youre better with making console app menus but not simple websites
        public static void HomeMenu()
        {
            Console.Clear();
            Functions.WriteLogo();
            Functions.WriteHookerMessage($"Welcome, {Environment.UserName}");
            Functions.CreateSelection("1", "Spam Webhook");
            Functions.CreateSelection("2", "Start Bot");
            string input = Functions.ReadUserInput();
            if (input == "1")
            {
                WebhookMenu();
            } else if (input == "2")
            {
                BotCheck();
            } else
            {
                HomeMenu();
            }
        }
        public static void WebhookMenu()
        {
            Console.Clear();
            Functions.WriteLogo();
            Functions.WriteHookerMessage("Webhook Control\n");
            Functions.WriteHookerMessage("Please Enter Webhook Url:");
            string urlInput = Functions.ReadUserInput();
            Console.Clear();
            Functions.WriteLogo();
            Functions.WriteHookerMessage("What Shall I Say Master?");
            string message = Functions.ReadUserInput();
            Console.Clear();
            Functions.WriteLogo();
            Functions.WriteHookerMessage("Webhook Control\n");
            Functions.WriteHookerMessage("How Many Times Shall I Say It?");
            int value = Functions.ReadUserInputInt();
            Console.Clear();
            Functions.WriteLogo();
            Functions.WriteHookerMessage("Webhook Control\n");
            Functions.WriteHookerMessage("Understood! Time To Spam <3");
            Thread.Sleep(1000);
            Functions.WebhookSpam(urlInput, message, value);  
        }
        public static void BotCheck()
        {
            //to anyone that reads this... i struggled with using ulong
            Console.Clear();
            Functions.WriteLogo();
            Functions.WriteHookerMessage("Enter Bot Token: ");
            string token = Functions.ReadUserInput();
            Functions.Global.BotToken = token;
            Console.Clear();
            Functions.WriteLogo();
            Functions.WriteHookerMessage("Enter Guild Id: ");
            ulong value = Functions.ReadUserULongInput();   
            Functions.Global.GuildID = value;
            Console.Clear();
            Functions.WriteLogo();
            Functions.WriteHookerMessage("Enter Your User Id: ");
            ulong userid = Functions.ReadUserULongInput();
            Functions.Global.UserID = userid;
            try
            {
                Functions.BotConnect();
                Console.ReadLine();
            } catch (Exception ex)
            {
                Functions.WriteHookerMessage(ex.Message);
                Console.ReadLine();
            }
        }
        public static void BotMenu()
        {
            Console.Clear();
            Functions.WriteLogo();
            Functions.WriteHookerMessage("Bot Control\n");
            Functions.WriteHookerMessage("Here Are A List Of Bot Commands Master <3\n");
            Functions.CreateSelection("*", "/nuke                      (Deletes All Channels)");
            Functions.CreateSelection("*", "/flood <name> <amount>     (Flood Server With Custom Channels)");
            Functions.CreateSelection("*", "/spam <message> <amount>   (Spams Messages To All Channels)");
            Functions.CreateSelection("*", "/delroles                  (Deletes All Roles)");
            Functions.CreateSelection("*", "/massban                   (Bans All Members)");
            Functions.CreateSelection("*", "/dmspam <message> <amount> (Spams All Members DMs In The Guild)");
        }
    }
}