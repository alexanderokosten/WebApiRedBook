using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedBookWebApplication.Model
{

    [Serializable]
    public class GetItemsModel
    {
        
        public IEnumerable<Item> Items { get; set; }
        
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }
    }
}
