using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiRedBook.Model
{
    public class GetItemsModel
    {
        public IEnumerable<Item> Items { get; set; }
        
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }
    }
}
