using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFS_Server
{
    [Serializable]
    class File: PrintingData, IData 
    {
        

        public StoreData ParentDirectory { get; set; }

        

        public File(string name, StoreData store)
        {
            this.Name = name;
            ParentDirectory = store;
            data = new List<IData>();
        }
    }
}
