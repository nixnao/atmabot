﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AtmaBot.Common
{
    public class Configuration
    {
        [JsonIgnore]
        public static string FileName { get; set; } = "config/configuration.json";
        public ulong[] Owners { get; set; }
        public string Prefix { get; set; } = "!";
        public string Token { get; set; } = "";


        public static void EnsureExists()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var config = new Configuration();

                Console.WriteLine("Please enter your discord token: ");
                string token = Console.ReadLine();
                config.Token = token;

                config.SaveJson();
            }
        }

        public void SaveJson()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            File.WriteAllText(file, ToJson());
        }

        public static Configuration Load()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(file));
        }

        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);


    }

}
