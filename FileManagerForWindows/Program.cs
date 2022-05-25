using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using FileManagerForWindows;

namespace MySuperPuperApp
{
    class Program
    {
        static void Main(string[] args)
        {
            FileManager fileManager = new FileManager();
            fileManager.RunFileManager();
        }
    }
}