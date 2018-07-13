using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFS_Server
{

    interface IData
    {
        string Name { get; set; }

        StoreData ParentDirectory { get; set; }
    }
}
