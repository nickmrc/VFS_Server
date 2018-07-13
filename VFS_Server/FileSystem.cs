using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFS_Server
{
    [Serializable]
    class FileSystem
    {
        public List<Disk> disks;

        public FileSystem()
        {
            this.disks = new List<Disk>();
            disks.Add(new Disk("C:"));
           
            CurrentDirectory = disks[0];
        }

        public StoreData Find(string name)
        {
            return disks.Find(x => x.Name==name);
        }

        public StoreData CurrentDirectory { get; set; }

        public string GetCurrentDirectory()
        {
            if (CurrentDirectory as Disk != null) return CurrentDirectory.Name;
            
            Directory store = CurrentDirectory as Directory;

            string path = "";
            while (store!=null)
            {
                path = "\\" + store.Name + path;
                if ((store.ParentDirectory as Directory) != null)
                {
                    store = store.ParentDirectory as Directory;
                }
                else
                {
                    path = store.ParentDirectory.Name +path;
                    store = null;
                }
            }

            return path;
        }





    }
}
