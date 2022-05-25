using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerForWindows.Models
{
    class DisplayConsole
    {
        public static void PrintFiles(int currentIndex, string path)
        {
            Console.Clear();

            string[] entries = Directory.GetFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < entries.Length; i++)
            {
                if (currentIndex == i)
                {
                    ConsoleColor current = Console.BackgroundColor;

                    Console.BackgroundColor = ConsoleColor.Yellow;
                    if (File.GetAttributes(entries[i]).HasFlag(FileAttributes.Directory))
                        PrintDir(entries[i]);
                    else
                        PrintFile(entries[i]);
                    Console.BackgroundColor = current;

                    continue;
                }
                if (File.GetAttributes(entries[i]).HasFlag(FileAttributes.Directory))
                    PrintDir(entries[i]);
                else
                    PrintFile(entries[i]);
            }
        }
        public static void PrintFile(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            FileInformation info = new FileInformation(fileInfo);

            Console.WriteLine($"{info.ToString()}");
        }
        public static void PrintDir(string directory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            Console.WriteLine($"{directoryInfo.Name}. {directoryInfo.CreationTime}");
        }
    }
}
