using FileManagerForWindows.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileManagerForWindows.Services
{
    class ConfigurationsManager //класс для хранения методов по работе с конфигом
    {
        
        public static void SaveToJson(ConfigurationsToSave configs)
        {
            string json = JsonSerializer.Serialize(configs);
            File.WriteAllText("configs.json", json);
        }

        public static ConfigurationsToSave ReadJson()
        {
            string json = File.ReadAllText("configs.json");
            ConfigurationsToSave previousConfig = JsonSerializer.Deserialize<ConfigurationsToSave>(json);
            return previousConfig;
        }
    }
}
