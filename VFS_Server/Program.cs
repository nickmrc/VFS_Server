using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFS_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandParser parser = new CommandParser();
            FileSystem fileSystem = new FileSystem();

            Console.Title = "VFS.Server";

            string command = "";
            while (command.ToUpper()!="QUIT")
            {
                Console.Write("{0}> ", fileSystem.GetCurrentDirectory());
                command = Console.ReadLine();
                parser.Parse(command, fileSystem);
                Console.WriteLine();

            }

            
        }
    }
}
