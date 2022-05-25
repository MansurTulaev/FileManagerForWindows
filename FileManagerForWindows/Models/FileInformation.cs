using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManagerForWindows.Models
{
    class FileInformation //модель для отображения данных о файле
    {
        public double FileSize { get; set; }
        public string CreationDate { get; set; }
        public string FileName { get; set; } 
        public string FileExtension { get; set; }

        public FileInformation (FileInfo fileInfo)
        {
            FileSize = Math.Round((double)fileInfo.Length / 1024, 2);
            CreationDate = fileInfo.CreationTime.ToString();
            FileName = fileInfo.Name;
            FileExtension = fileInfo.Extension;

        }

        public override string ToString()
        {
            return $"{FileName}. File extension: {FileExtension}. File size: {FileSize} kb. Creation Date {CreationDate}";
        }
    }
}
