﻿using AtmaBot.Common;
using AtmaBot.Modules.Music;
using Discord;
using Discord.Addons.Interactive;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AtmaBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().StartAsync().GetAwaiter().GetResult();
        }


        private DiscordSocketClient _client;

        public async Task StartAsync()
        {
            // Ensures the configuration file has been created.
            Configuration.EnsureExists();

            // Creates a new client
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 1000
            });

            _client.Log += Log;
            //_client.JoinedGuild += Joined;
            //_client.UserJoined += UserJoinedServer;

            var config = Configuration.Load();
            var token = config.Token;
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            var services = ConfigureServices();
            await new CommandHandler().Install(services);

            await Task.Delay(-1); // Prevents Console Window from closing.
        }

        public IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<InteractiveService>()
                .AddSingleton<MusicService>();
            //.AddPaginator(_client, Log);
            return services.BuildServiceProvider();
        }

        private Task Log(LogMessage msg)
        {
            var cc = Console.ForegroundColor;
            string logPath = Path.Combine(AppContext.BaseDirectory, @"log.txt");

            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }



            Console.WriteLine($"{DateTime.Now,-19} [{msg.Severity,8}] {msg.Source}: {msg.Message}");

            Console.ForegroundColor = cc;
            return Task.CompletedTask;
        }



        //private async Task Joined(SocketGuild guild)
        //{
        //    var owner = guild.Owner;
        //    var testhouse = _client.GetGuild(286211518691934210);
        //    var rand = new Random();
        //    var reply =
        //    $"Members: {guild.MemberCount}\n" +
        //    $"Channels: {guild.Channels.Count}\n" +
        //    $"Default Channel: {guild.DefaultChannel.Name}\n" +
        //    $"Roles: {guild.Roles.Count}\n" +
        //    $"Age: {guild.CreatedAt.CompareTo(DateTime.Now)}\n" +
        //    $"";


        //    var embed = new EmbedBuilder()
        //    .WithTitle(guild.Name)
        //    .WithDescription(reply)
        //    .WithColor(new Color(rand.Next(30, 250), rand.Next(30, 250), rand.Next(30, 250)))
        //    .WithThumbnailUrl(guild.IconUrl)
        //    .Build();

        //    await testhouse.GetTextChannel(286221025408974849).SendMessageAsync("", embed: embed);

        //    await Nero.ServerIntro.JoinedServer(guild, owner);



        //    return;
        //}
    }
}
