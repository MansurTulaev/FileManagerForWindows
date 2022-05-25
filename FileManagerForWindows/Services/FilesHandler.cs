using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerForWindows.Services
{
    class FilesHandler
    {

        public static void MoveFile(string path, string destinationFolder)
        {

            Console.WriteLine("Идет перемещение");
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {

                DirectoryInfo directoryInfo = new DirectoryInfo(path); 
                string destinationFolderInfo = Path.Combine(destinationFolder, directoryInfo.Name);
                if (Directory.Exists(destinationFolderInfo))
                    new DirectoryInfo(destinationFolderInfo).Delete(true);
                directoryInfo.MoveTo(destinationFolderInfo);
            }

            else
            {
                FileInfo fileInfo = new FileInfo(path);
                File.Move(path, Path.Combine(destinationFolder, fileInfo.Name));
            }

            Console.WriteLine("Файл скопирован");
        }

        public static void CopyFile(string path, string destinationFolder)
        {
            Console.WriteLine("Идет копирование");
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                DirectoryInfo destinationInfo = new DirectoryInfo(destinationFolder);
                foreach (FileInfo fi in directoryInfo.GetFiles())
                {
                    fi.CopyTo(Path.Combine(destinationFolder, fi.Name), true);
                }

                foreach (DirectoryInfo diSourceSubDir in directoryInfo.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                        destinationInfo.CreateSubdirectory(diSourceSubDir.Name);
                    CopyFile(diSourceSubDir.FullName, nextTargetSubDir.FullName);
                }
            }

            else
            {
                File.Copy(path, Path.Combine(destinationFolder, Path.GetFileName(path)), true);
            }
        }

        public static void DeleteFile(string path)
        {
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                Console.WriteLine("Идет удаление файла/папки");
                directoryInfo.Delete(true);
            }

            else
            {
                File.Delete(path);
                Console.ReadLine();
            }

        }
    }
}
