using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace MySuperPuperApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(@"Хотите ли запустить последний период сохранения?
            Нажмите Y, чтобы запустить последний вариант сохранения
            Нажмите N, чтобы зпустить приложение заново");
            ConsoleKeyInfo info = Console.ReadKey();
            ConfigurationsToSave currentConfigs = new ConfigurationsToSave();
            int currentIndex = 0;
            string path = "";
            int level = 0;
            switch (info.Key) //загрузка предыдущего состояния или ввод пользователем новых данных
            {
                case ConsoleKey.Y:
                    {
                        currentConfigs = ReadJson();
                        currentIndex = currentConfigs.CurrentIndex;
                        path = currentConfigs.FilePath;
                        level = currentConfigs.PagingNumber;
                    }
                    break;
                case ConsoleKey.N:
                    {
                        Console.WriteLine("Введите число элеметов для демонстрации");
                        level = Convert.ToInt32(Console.ReadLine());
                        currentIndex = 0;
                        Console.WriteLine("Введите путь к файлу");
                        path = Console.ReadLine();
                        currentConfigs = new ConfigurationsToSave();
                    }
                    break;
            }
            PrintFiles(currentIndex, path);

            try
            {
                while (true)
                {

                    Console.WriteLine(@"Нажмите ENTER, чтобы открыть файл
                      Нажмите C, чтобы скопировать файл или папку
                      Нажмите D, чтобы удалить файл или папку
                      Нажмите M, чтобы переместить файл или папку
                      Нажмите Esc, чтобы выйти из приложения
                      Нажмите Backspace, чтобы поменять папку
                      "); 
                    info = Console.ReadKey();


                    switch (info.Key) //в зависимости от кнопки можно переместить, открыть или удалить папку/файлы
                    {

                        case ConsoleKey.UpArrow:
                            {
                                if (currentIndex > 0)
                                {
                                    currentIndex--;
                                }

                                PrintFiles(currentIndex, path);
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            {
                                currentIndex++;

                                PrintFiles(currentIndex, path);
                            }
                            break;
                        case ConsoleKey.Enter: //открытие
                            {
                                string file = Directory.GetFiles(path)[currentIndex];
                                Process.Start(new ProcessStartInfo() { FileName = file, UseShellExecute = true });
                            }
                            break;
                        case ConsoleKey.M: //перемещение
                            {
                                string[] filesArray = Directory.GetFiles(path);
                                DirectoryInfo directoryInfo = new DirectoryInfo(filesArray[currentIndex]);
                                Console.WriteLine("Введите назначение");
                                string destinationFolder = Console.ReadLine();
                                foreach (FileInfo file in directoryInfo.EnumerateFiles(Directory.GetFiles(path)[currentIndex]))
                                {
                                    file.CopyTo(destinationFolder, true);
                                    file.Delete();
                                }
                                directoryInfo.EnumerateDirectories()
                                    .ToList().ForEach(f => f.MoveTo(destinationFolder));
                                Console.WriteLine("Файл скопирован");
                            }
                            break;
                        case ConsoleKey.D: //удаление
                            {
                                string[] filesArray = Directory.GetFiles(path);
                                DirectoryInfo directoryInfo = new DirectoryInfo(filesArray[currentIndex]);
                                Console.WriteLine("Идет удаление файла/папки");
                                foreach (FileInfo file in directoryInfo.EnumerateFiles(Directory.GetFiles(path)[currentIndex]))
                                {
                                    file.Delete();
                                }
                                directoryInfo.EnumerateDirectories()
                                    .ToList().ForEach(f => f.Delete());
                            }
                            break;
                        case ConsoleKey.Escape: //выход
                            {
                                Console.WriteLine("Идет выход из программы");
                                Console.WriteLine($"Сохранение последнего состояния");
                                currentConfigs.CurrentIndex = currentIndex;
                                currentConfigs.FilePath = path;
                                currentConfigs.PagingNumber = level;
                                SaveToJson(currentConfigs);
                                return;
                            }
                            break;
                        case ConsoleKey.Backspace: //ввод другой папки
                            {
                            Console.WriteLine("Введите путь");
                            path = Console.ReadLine();
                            PrintFiles(0, path);
                            }
                            break;
                            

                    }
                }


            }

            catch (Exception e) //если произошла ошибка, записать текст ошибки в документ и сохранить состояние
            {
                string exceptionMessage = $"Произошла ошибка. Сообщение ошибке: {e.Message}. Место ошибки: {e.Source}. Дата ошибки: {DateTime.Now.ToString()}";
                Console.WriteLine(exceptionMessage);
                File.AppendAllText($"errors\\exception_file_manager_{DateTime.Now.ToString("dd.MM.yyyy")}.txt", exceptionMessage);
                Console.WriteLine($"Сохранение последнего состояния");
                currentConfigs.CurrentIndex = currentIndex;
                currentConfigs.FilePath = path;
                currentConfigs.PagingNumber = level;
                SaveToJson(currentConfigs);
                return;
            }
        }

        public static void PrintFiles(int currentIndex, string path)
        {

            Console.Clear();

            string[] files = Directory.GetFiles(path);

            for (int i = 0; i < files.Length; i++)
            {
                if (currentIndex == i)
                {
                    ConsoleColor current = Console.BackgroundColor;

                    Console.BackgroundColor = ConsoleColor.Yellow;

                    PrintFile(files[i]);

                    Console.BackgroundColor = current;

                    continue;
                }

                PrintFile(files[i]);
            }
        }

        public static void PrintFile(string file)
        {
            FileInfo info = new FileInfo(file);

            Console.WriteLine($"{info.FullName} {info.Length} bytes");
        }

        public static void PrintDir(string directory, int level)
        {
            string[] dirs = Directory.GetDirectories(directory);

            for (int i = 0; i < dirs.Length; i++)
            {
                string childDir = dirs[i];

                for (int z = 0; z < level; z++)
                {
                    Console.Write("\t");
                }

                Console.WriteLine(childDir);

                PrintDir(childDir, level + 1);
            }
        }

        public static void SaveToJson(ConfigurationsToSave configs)
        {
            string json = JsonSerializer.Serialize(configs);
            File.WriteAllText("configs.json", json);
        }

        public static ConfigurationsToSave ReadJson()
        {
            string json = File.ReadAllText("configs.json");
            ConfigurationsToSave previousConfig = JsonSerializer.Deserialize<ConfigurationsToSave>(json);
            File.WriteAllText("configs.json", json);
            return previousConfig;
        }


    }
    class ConfigurationsToSave
    {
        private int _currentIndex;
        private int _pagingNumber;
        private string _filePath;
        public int CurrentIndex { get; set; }
        public int PagingNumber { get; set; }
        public string FilePath { get; set; }
    }

}