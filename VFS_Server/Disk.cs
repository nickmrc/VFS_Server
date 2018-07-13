using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFS_Server
{
    [Serializable]
    class Disk: StoreData
    {
      

        public Disk(string name)
        {
            this.Name = name;
            data = new List<IData>();
        }



    }
}
