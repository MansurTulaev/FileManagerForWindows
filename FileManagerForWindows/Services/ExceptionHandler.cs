using FileManagerForWindows.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerForWindows.Services
{
    class ExceptionHandler
    {
        public ConfigurationsToSave Configurations { get; set; }
        public Exception AppException { get; set; }
        
        public ExceptionHandler (ConfigurationsToSave configurationsIncoming, Exception exception)
        {
            Configurations = configurationsIncoming;
            AppException = exception;
        }

        public ExceptionHandler ()
        {

        }
        public static void SaveCurrentSettings (ConfigurationsToSave configurations, Exception exception)
        {
            string exceptionMessage = $"Произошла ошибка. Сообщение ошибке: {exception.Message}. Место ошибки: {exception.Source}. Дата ошибки: {DateTime.Now.ToString()}";
            Console.WriteLine(exceptionMessage);
            File.AppendAllText($"C:\\Users\\mdtulaev\\Documents\\FileManagerForWindows\\errors\\exception_file_manager_{DateTime.Now.ToString("dd.MM.yyyy")}.txt", exceptionMessage);
            Console.WriteLine($"Сохранение последнего состояния");
            ConfigurationsManager.SaveToJson(configurations);
        }

    }
}
