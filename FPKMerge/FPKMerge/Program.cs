using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace FPKMerge
{
    class Program
    {
        static string input_folder;
        static string output_folder;

        static void Main(string[] args)
        {

            Console.WriteLine("FPKMerge");
            Console.WriteLine("Author: Cra0");
            Console.WriteLine("-----------------------");
            //get the data from the arg1
            if (args.Length == 0 || args.Length < 2)
            {
                Console.WriteLine("Usage: FPKMerge input_folder output_folder");
                Console.WriteLine("Example: FPKMerge C:/chunk0 C:/extracted");
                Console.ReadKey();
                return;
            }

            input_folder = args[0];
            output_folder = args[1];

            DirSearch(input_folder);

        }


        static void DirSearch(string dir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(dir))
                {
                    string dir_name = Path.GetFileName(d);
                    if (dir_name == "Assets")
                    {
                        MoveDirectory(d, output_folder);
                        Console.WriteLine(d);
                    }
                    DirSearch(d);
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void MoveDirectory(string source, string target)
        {
            var stack = new Stack<Folders>();
            stack.Push(new Folders(source, target));

            while (stack.Count > 0)
            {
                var folders = stack.Pop();
                Directory.CreateDirectory(folders.Target);
                foreach (var file in Directory.GetFiles(folders.Source, "*.*"))
                {
                    string targetFile = Path.Combine(folders.Target, Path.GetFileName(file));
                    if (File.Exists(targetFile)) File.Delete(targetFile);
                    File.Move(file, targetFile);
                }

                foreach (var folder in Directory.GetDirectories(folders.Source))
                {
                    stack.Push(new Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
                }
            }
            Directory.Delete(source, true);
        }
        public class Folders
        {
            public string Source
            {
                get;
                private set;
            }
            public string Target
            {
                get;
                private set;
            }

            public Folders(string source, string target)
            {
                Source = source;
                Target = target;
            }
        }


    }
}
