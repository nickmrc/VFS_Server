using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace VFS_Server
{
    [Serializable]
    class Directory: StoreData, IData
    {
        public StoreData ParentDirectory { get; set; }

        public Directory(string name, StoreData store)
        {
            this.Name = name;
            data = new List<IData>();
            ParentDirectory = store;
        }

        public bool isEmpty()
        {
            return !this.data.Any();
        }



    }
}
