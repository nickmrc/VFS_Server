using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFS_Server
{
    class CommandParser
    {
        public void Print(PrintingData st, string indent, bool last)
        {
            if ((st as Disk)!=null) Console.WriteLine(st.Name);
            else
            {
                Console.WriteLine(indent+ (last ? "|_" : "|_")+st.Name);

                if (last & !st.data.Any())
                {
                    Console.WriteLine(indent);
                }

                indent = indent + (last ? " " : "| ");
            }
            for (int i = 0; i < st.data.Count; i++)
            {
                Print((PrintingData)st.data[i], indent, i == st.data.Count - 1);

            }

        }

        public void Print(PrintingData st, string indent, bool last, ref string s)
        {
            if ((st as Disk) != null) s+=st.Name+"\n";
            else
            {
                s+=indent + (last ? "|_" : "|_") + st.Name+"\n";

                if (last & !st.data.Any())
                {
                    s+=indent+"\n";
                }

                indent = indent + (last ? " " : "| ");
            }
            for (int i = 0; i < st.data.Count; i++)
            {
                List<IData> sorted =  st.data.OrderBy(x => x.Name).ToList();
                    Print((PrintingData)sorted[i], indent, i == st.data.Count - 1, ref s);

            }

        }

        public string Print(PrintingData st)
        {
            string s="";
            Print(st, "", true, ref s);
            return s;
        }

        public void PrintPretty(StoreData st, string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("");
                //indent += "";
                indent += " |_";
            }
            else
            {
                //Console.Write("|_");
                indent += " |_ ";
            }
            Console.WriteLine(st.Name);

            for (int i = 0; i < st.data.Count; i++)
            {
                PrintPretty((StoreData) st.data[i], indent, i == st.data.Count - 1);
            }
        }

        public  void PrintTree(StoreData tree, String indent, bool last)
        {
           
            Console.WriteLine(indent + "|_" + tree.Name);
            indent += last ? " " : "| ";


            for (int i = 0; i < tree.data.Count; i++)
    {
                PrintTree((StoreData)tree.data[i], indent, i == tree.data.Count - 1);
              
            }
           
        }



        private PrintingData getPathObject(FileSystem fileSystem, string [] path, bool isFrom)
        {
            PrintingData data = fileSystem.Find(path[0].ToUpper());

            if (data == null)
            {
                data = fileSystem.CurrentDirectory.FindAll(path[0], isFrom);
                if (data != null)
                {
                    for (int i = 1; i < path.Length; i++)
                    {
                        if (data.FindAll(path[i], isFrom) != null)
                        {
                            data = data.FindAll(path[i], isFrom);
                            
                        }
                        else
                        {
                            Console.WriteLine("Путь не найден");
                            return null;

                            //ошибка команды - путь не найден
                        }

                        if (i.Equals(path.Length - 1))
                        {
                            //Путь from найден и надо искать путь to
                            return data;
                        }
                       

                    }

                }
                else
                {
                    Console.WriteLine("Путь не найден");
                    return null;
                    //ошибка команды - путь не найден
                }

            }

            for (int i = 1; i < path.Length; i++)
            {
                if (data.FindAll(path[i], isFrom) != null)
                {
                    data = data.FindAll(path[i], isFrom);
                    

                }
                else
                {
                    return null;
                    //ошибка команды - путь не найден
                }

                if (i.Equals(path.Length - 1))
                {
                    //Путь from найден и надо искать путь to
                    return data;
                }


                
            }

            return data;



        }



        public void Parse(string command, FileSystem fileSystem)
        {

            string[] predicates = System.Text.RegularExpressions.Regex.Replace(command.Trim(), @"\s+", " ").Split(' ');

            #region CD

            if (predicates[0].ToUpper().Equals("CD") && predicates.Length == 2)
            {
                char[] charsToTrim = { '\\' };

                string[] path = predicates[1].TrimEnd(charsToTrim).Split('\\');

                StoreData store = fileSystem.Find(path[0].ToUpper());

                if (store == null)
                {
                    store = fileSystem.CurrentDirectory.Find(path[0]);
                    if (store != null)
                    {
                        for (int i = 1; i < path.Length; i++)
                        {
                            if (store.Find(path[i]) != null)
                            {
                                store = store.Find(path[i]);
                            }
                            else
                            {
                                return;

                                //ошибка команды - путь не найден
                            }

                            if (i.Equals(path.Length - 1))
                            {
                                fileSystem.CurrentDirectory = store;
                                return;
                            }

                        }
                        fileSystem.CurrentDirectory = store;
                    }
                    else
                    {
                        return;
                        //ошибка команды - путь не найден
                    }

                }
                else
                {
                    if (path.Length==1) fileSystem.CurrentDirectory = store;
                    return;
                    
                }

                for (int i = 1; i < path.Length; i++)
                {
                    if (store.Find(path[i]) != null)
                    {
                        store = store.Find(path[i]);
                    }
                    else
                    {
                        return;
                        //ошибка команды - путь не найден
                    }

                    if (i.Equals(path.Length - 1))
                    {
                        fileSystem.CurrentDirectory = store;
                        return;
                    }

                }
                string s = "";

            }

            #endregion

            #region MD
            if (predicates[0].ToUpper().Equals("MD") && predicates.Length == 2)
            {
                char[] charsToTrim = { '\\' };

                string[] path = predicates[1].TrimEnd(charsToTrim).Split('\\');

                StoreData store = fileSystem.Find(path[0].ToUpper());

                //По относительному пути
                if (store == null)
                {
                    store = fileSystem.CurrentDirectory.Find(path[0]);

                    if (store != null)
                    {
                        Console.WriteLine("Такая директория уже существует");
                        return;

                    }
                    else
                    {
                        if (path.Length == 1)
                        {
                            Console.WriteLine("Директория создана");
                            fileSystem.CurrentDirectory.CreateDirectory(path[0]);
                        }

                        else
                        {
                            Console.WriteLine("Путь не найден");
                            return;
                            //ошибка - директория не существует
                        }
                    }
                }

                for (int i = 1; i < path.Length; i++)
                {
                    if (store.Find(path[i]) != null)
                    {
                        if (i != path.Length - 1) store = store.Find(path[i]);
                        else
                        {
                            //Такая директория уже существует
                            Console.WriteLine("Такая директория уже существует");
                            return;
                        }
                    }
                    else
                    {
                        if (i == path.Length - 1)
                        {
                            store.CreateDirectory(path[path.Length - 1]);
                            Console.WriteLine("Директория создана");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Путь не найден");
                            return;
                            
                            //ошибка команды - путь не найден
                        }

                    }

                    if (i.Equals(path.Length - 1))
                    {
                        fileSystem.CurrentDirectory = store;
                    }

                }
                string s = "";
            }

            #endregion

            #region RD

            if (predicates[0].ToUpper().Equals("RD") && predicates.Length == 2)
            {
                char[] charsToTrim = { '\\' };

                string[] path = predicates[1].TrimEnd(charsToTrim).Split('\\');

                StoreData store = fileSystem.Find(path[0].ToUpper());

                //По относительному пути
                if (store == null)
                {
                    store = fileSystem.CurrentDirectory.Find(path[0]);
                    if (store != null)
                    {
                        for (int i = 1; i < path.Length; i++)
                        {
                            if (store.Find(path[i]) != null)
                            {
                                store = store.Find(path[i]);
                            }
                            else
                            {
                                return;

                                //ошибка команды - путь не найден
                            }

                            if (i.Equals(path.Length - 1))
                            {

                                if ((store as Directory) != null)
                                {
                                    if ((store as Directory).isEmpty())
                                    {
                                        Console.WriteLine("Удалили директорию");
                                        (store as Directory).ParentDirectory.DeleteDirectory(path[path.Length - 1]);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Нельзя удалить директорию с поддиректориями");
                                    }
                                }
                                
                                return;
                            }

                        }
                        if ((store as Directory) != null)
                        {
                            if ((store as Directory).isEmpty())
                            {
                                Console.WriteLine("Удалили директорию");
                                (store as Directory).ParentDirectory.DeleteDirectory(path[path.Length - 1]);
                            }
                            else
                            {
                                Console.WriteLine("Нельзя удалить директорию с поддиректориями");
                            }
                        }
                    }
                    else
                    {
                        return;
                        //ошибка команды - путь не найден
                    }

                }

                for (int i = 1; i < path.Length; i++)
                {
                    if (store.Find(path[i]) != null)
                    {
                        store = store.Find(path[i]);
                    }
                    else
                    {
                        break;
                        //ошибка команды - путь не найден
                    }

                    if (i.Equals(path.Length - 1))
                    {
                        if ((store as Directory) != null )
                        {
                            if ((store as Directory).isEmpty())
                            {
                                Console.WriteLine("Удалили директорию");
                                (store as Directory).ParentDirectory.DeleteDirectory(path[path.Length - 1]);
                            }
                            else
                            {
                                Console.WriteLine("Нельзя удалить директорию с поддиректориями");
                            }

                        }
                    }

                }
                string s = "";

            }

            #endregion

            #region DELTREE

            if (predicates[0].ToUpper().Equals("DELTREE") && predicates.Length == 2)
            {
                char[] charsToTrim = { '\\' };

                string[] path = predicates[1].TrimEnd(charsToTrim).Split('\\');

                StoreData store = fileSystem.Find(path[0].ToUpper());

                //По относительному пути
                if (store == null)
                {
                    store = fileSystem.CurrentDirectory.Find(path[0]);
                    if (store != null)
                    {
                        for (int i = 1; i < path.Length; i++)
                        {
                            if (store.Find(path[i]) != null)
                            {
                                store = store.Find(path[i]);
                            }
                            else
                            {
                                return;

                                //ошибка команды - путь не найден
                            }

                            if (i.Equals(path.Length - 1))
                            {

                                if ((store as Directory) != null)
                                {
                                    Console.WriteLine("Удалили директорию");
                                    (store as Directory).ParentDirectory.DeleteDirectory(path[path.Length - 1]);
                                }

                                return;
                            }

                        }
                        if ((store as Directory) != null)
                        {
                            Console.WriteLine("Удалили директорию");
                            (store as Directory).ParentDirectory.DeleteDirectory(path[path.Length - 1]);
                        }
                    }
                    else
                    {
                        return;
                        //ошибка команды - путь не найден
                    }

                }

                for (int i = 1; i < path.Length; i++)
                {
                    if (store.Find(path[i]) != null)
                    {
                        store = store.Find(path[i]);
                    }
                    else
                    {
                        break;
                        //ошибка команды - путь не найден
                    }

                    if (i.Equals(path.Length - 1))
                    {
                        if ((store as Directory) != null)
                        {
                            Console.WriteLine("Удалили директорию");
                            (store as Directory).ParentDirectory.DeleteDirectory(path[path.Length - 1]);
                        }
                    }

                }
                string s = "";

            }

            #endregion

            #region Print

            if (predicates[0].ToUpper().Equals("PRINT") && predicates.Length == 1)
            {
                foreach (PrintingData VARIABLE in fileSystem.disks)
                {
                    Print(VARIABLE, "", true);
                }
            }

            #endregion

            #region MF

            if (predicates[0].ToUpper().Equals("MF") && predicates.Length == 2)
            {
                char[] charsToTrim = { '\\' };

                string[] path = predicates[1].TrimEnd(charsToTrim).Split('\\');

                StoreData store = fileSystem.Find(path[0].ToUpper());
                File file;

                //По относительному пути
                if (store == null)
                {
                    file = fileSystem.CurrentDirectory.FindFile(path[0]);
                    store = fileSystem.CurrentDirectory;

                    if (file != null)
                    {
                        Console.WriteLine("Такой файл уже существует");
                        return;
                    }
                    else
                    {
                        if (path.Length == 1)
                        {
                            Console.WriteLine("Файл создан");
                            fileSystem.CurrentDirectory.CreateFile(path[0]);
                            return;
                        }

                        else
                        {
                            Console.WriteLine("Путь не найден");
                            return;
                            //ошибка - директория не существует
                        }
                    }
                }

                for (int i = 1; i < path.Length-1; i++)
                {
                    if (store.Find(path[i]) != null)
                    {
                        store = store.Find(path[i]);
                        if (i == path.Length - 2)
                        {
                            store.CreateFile(path[path.Length - 1]);
                            Console.WriteLine("Файл создан");
                            return;
                        }

                       
                    }
                    else
                    {
                        Console.WriteLine("Путь не найден");
                        return;
                    }   

                }
                string s = "";
            }


            #endregion

            #region DEL

            if (predicates[0].ToUpper().Equals("DEL") && predicates.Length == 2)
            {
                char[] charsToTrim = { '\\' };

                string[] path = predicates[1].TrimEnd(charsToTrim).Split('\\');

                StoreData store = fileSystem.Find(path[0].ToUpper());
                File file;

                //По относительному пути

                if (store == null)
                {
                    store = fileSystem.CurrentDirectory;

                    if (path.Length == 1 )
                    {
                        if (store.FindFile(path[0]) != null)
                        {
                            store.DeleteFile(path[0]);
                            Console.WriteLine("Удалили файл");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Файла с таким именем не существует");
                            return;
                        }
                        
                    }
                    else
                    {
                        for (int i = 1; i < path.Length - 1; i++)
                        {
                            if (store.Find(path[i]) != null)
                            {
                                store = store.Find(path[i]);
                                if (i == path.Length - 2)
                                {
                                    if (store.FindFile(path[path.Length - 1]) != null)
                                    {
                                        store.DeleteFile(path[path.Length - 1]);
                                        Console.WriteLine("Удалили файл");
                                        return;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Файла с таким именем не существует");
                                        return;
                                    }
                                }
                            }

                            else
                            {
                                Console.WriteLine("Путь не найден");
                                return;
                            }
                        }
                    }


                }


                for (int i = 1; i < path.Length - 1; i++)
                {
                    if (store.Find(path[i]) != null)
                    {
                        store = store.Find(path[i]);
                        if (i == path.Length - 2)
                        {
                            if (store.FindFile(path[path.Length - 1]) != null)
                            {
                                store.DeleteFile(path[path.Length - 1]);
                                Console.WriteLine("Удалили файл");
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Файла с таким именем не существует");
                                return;
                            }
                        }


                    }
                    else
                    {
                        Console.WriteLine("Путь не найден");
                        return;
                    }

                }
                string s = "";

            }

            #endregion

            #region Copy

            if (predicates[0].ToUpper().Equals("COPY") && predicates.Length == 3)
            {
                char[] charsToTrim = { '\\' };

                string[] pathFrom = predicates[1].TrimEnd(charsToTrim).Split('\\');
                string[] pathTo = predicates[2].TrimEnd(charsToTrim).Split('\\');

                PrintingData from = getPathObject(fileSystem, pathFrom, true);
                PrintingData to = getPathObject(fileSystem, pathTo, false);

                if (from != null && (to as StoreData) != null)
                {
                    if (from is File)
                    {
                        if ((to as StoreData).FindFile(from.Name) == null)
                        {
                            (to as StoreData).Copy(from as IData);
                            Console.WriteLine("Файл скопирован");

                        }
                        else
                        {
                            from = getPathObject(fileSystem, pathFrom, false);
                        }
                    }

                    if (from is StoreData)
                    {
                        if ((to as StoreData).Find(from.Name) == null)
                        {
                            (to as StoreData).Copy(from as IData);
                           
                            Console.WriteLine("Директория была скопирована");

                        }
                        else
                        {
                            Console.WriteLine("Не удалось выполнить команду");
                        }
                    }
                }


            }

            #endregion

            #region Move

            if (predicates[0].ToUpper().Equals("MOVE") && predicates.Length == 3)
            {
                char[] charsToTrim = { '\\' };

                string[] pathFrom = predicates[1].TrimEnd(charsToTrim).Split('\\');
                string[] pathTo = predicates[2].TrimEnd(charsToTrim).Split('\\');

                PrintingData from = getPathObject(fileSystem, pathFrom, true);
                PrintingData to = getPathObject(fileSystem, pathTo, false);

                if (from != null && (to as StoreData) != null)
                {
                    if (from is File)
                    {
                        if ((to as StoreData).FindFile(from.Name) == null)
                        {
                            (to as StoreData).Copy(ObjectCopier.Clone(from) as IData);
                            (from as IData).ParentDirectory.Remove(from as IData);
                            Console.WriteLine("Файл был перемещен");

                        }
                        else
                        {
                            Console.WriteLine("Не удалось выполнить команду");
                        }
                    }

                    if (from is StoreData)
                    {
                        if ((to as StoreData).Find(from.Name) == null && to != from)
                        {
                            (to as StoreData).Copy(ObjectCopier.Clone(from) as IData);
                            (from as IData).ParentDirectory.Remove(from as IData);
                            Console.WriteLine("Директория была перемещен");

                        }
                        else
                        {
                            Console.WriteLine("Не удалось выполнить команду");
                        }
                    }
                }


                else
                {
                    Console.WriteLine("Невозможно переместить директорию в саму себя");
                    return;

                }

            }
                


            

            #endregion
        }
    }
}
