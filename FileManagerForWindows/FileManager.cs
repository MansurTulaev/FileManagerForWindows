using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FileManagerForWindows;
using System.Diagnostics;
using FileManagerForWindows.Models;
using FileManagerForWindows.Services;
using System.Text.Json;

namespace FileManagerForWindows
{
    class FileManager
    {
        public void RunFileManager()
        {
            ConfigurationsToSave currentConfigs = new ConfigurationsToSave();
            int currentIndex = 0;
            string path = "";
            try
            {
                Console.WriteLine(@"Хотите ли запустить последний период сохранения?
            Нажмите Y, чтобы запустить последний вариант сохранения
            Нажмите N, чтобы зпустить приложение заново");
                ConsoleKeyInfo info = Console.ReadKey();

                switch (info.Key) //загрузка предыдущего состояния или ввод пользователем новых данных
                {
                    case ConsoleKey.Y:
                        {
                            currentConfigs = ConfigurationsManager.ReadJson();
                            currentIndex = currentConfigs.CurrentIndex;
                            path = currentConfigs.FilePath;
                        }
                        break;
                    case ConsoleKey.N:
                        {
                            currentConfigs.CurrentIndex = currentIndex;
                            Console.WriteLine("Введите путь к файлу");
                            path = Console.ReadLine();
                            currentConfigs.FilePath = path;
                        }
                        break;
                }
                DisplayConsole.PrintFiles(currentIndex, path);

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

                                else
                                    currentIndex = Directory.GetFileSystemEntries(path).Count() - 1;

                                DisplayConsole.PrintFiles(currentIndex, path);
                            }
                            break;

                        case ConsoleKey.DownArrow:
                            {
                                currentIndex++;
                                if (currentIndex > (Directory.GetFileSystemEntries(path).Count()-1))
                                    currentIndex = 0;

                                DisplayConsole.PrintFiles(currentIndex, path);
                            }
                            break;

                        case ConsoleKey.Enter: //открытие
                            {
                                string file = Directory.GetFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly)[currentIndex];
                                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                                {
                                    DisplayConsole.PrintFiles(currentIndex, file);
                                    path = file;
                                }
      
                                else
                                    Process.Start(new ProcessStartInfo() { FileName = file, UseShellExecute = true });
                            }
                            break;

                        case ConsoleKey.M: //перемещение
                            {
                                string file = Directory.GetFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly)[currentIndex];
                                Console.WriteLine("Введите назначение");
                                string destinationFolder = Console.ReadLine();
                                FilesHandler.MoveFile(file, destinationFolder);
                                DisplayConsole.PrintFiles(currentIndex, path);

                            }
                            break;

                        case ConsoleKey.C: //копирование
                            {
                                string file = Directory.GetFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly)[currentIndex];
                                Console.WriteLine("Введите назначение");
                                string destinationFolder = Console.ReadLine();
                                FilesHandler.CopyFile(file, destinationFolder);
                                DisplayConsole.PrintFiles(currentIndex, path);

                            }
                            break;

                        case ConsoleKey.D: //удаление
                            {
                                string file = Directory.GetFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly)[currentIndex];
                                FilesHandler.DeleteFile(file);
                                DisplayConsole.PrintFiles(currentIndex, path);
                            }
                            break;



                        case ConsoleKey.Backspace: //ввод другой папки
                            {
                                Console.WriteLine("Введите путь");
                                path = Console.ReadLine();
                                DisplayConsole.PrintFiles(0, path);
                            }
                            break;


                        case ConsoleKey.Escape: //выход
                            {
                                Console.WriteLine("Идет выход из программы");
                                Console.WriteLine($"Сохранение последнего состояния");
                                currentConfigs.CurrentIndex = currentIndex;
                                currentConfigs.FilePath = path;
                                ConfigurationsManager.SaveToJson(currentConfigs);
                                return;
                            }
                    }
                }
            }

            catch (Exception e)
            {
                ExceptionHandler.SaveCurrentSettings( currentConfigs, e);
            }
            

        }

    }
}

