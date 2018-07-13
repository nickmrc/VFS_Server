using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFS_Server
{
    [Serializable]
    class StoreData: PrintingData
    {

        public StoreData Find(string name)
        {
            return data.Find(x => x.Name == name && x is StoreData) as StoreData;
        }

        public File FindFile(string name)
        {

            return data.Find(x => x.Name == name && x is File) as File;
           
        }

        public void CreateDirectory(string name)
        {
            data.Add(new Directory(name, this));
        }

        public void CreateFile(string name)
        {
            data.Add(new File(name, this));
        }

        public void DeleteDirectory(string name)
        {
            data.Remove((Directory)Find(name));
        }

        public void DeleteFile(string name)
        {
            data.Remove(FindFile(name));
        }

        public void Copy(IData obj)
        {
            IData item = ObjectCopier.Clone(obj as PrintingData) as IData;

            if (obj != null)
            {
                item.ParentDirectory = this;
                data.Add(item);
            }
        }

        public void Remove(IData obj)
        {
            if (obj != null) data.Remove(obj);
        }

    }
}
