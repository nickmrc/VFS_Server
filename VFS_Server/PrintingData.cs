using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFS_Server
{
    [Serializable]
    class PrintingData
    {
        public string Name { get; set; }

        public List<IData> data;

        public PrintingData FindAll(string name, bool isFrom)
        {
            if (isFrom)
            {
                return FindAllFrom(name);
            }
            else
            {
                return FindAllTo(name);
            }
        }

        private PrintingData FindAllTo(string name)
        {
            return  (data.Find(x => x.Name.Equals(name) && x is StoreData) as PrintingData)??(data.Find(x => x.Name.Equals(name) && x is File) as PrintingData) ;
        }

        private PrintingData FindAllFrom(string name)
        {
            return (data.Find(x => x.Name.Equals(name) && x is File) as PrintingData) ?? (data.Find(x => x.Name.Equals(name) && x is StoreData) as PrintingData);
        }




    }
}
