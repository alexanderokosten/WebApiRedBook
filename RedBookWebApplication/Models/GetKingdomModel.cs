using RedBookWebApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiRedBook.Model
{
    public class GetKingdomModel
    {
        public IQueryable<Kingdom> Items { get; set; }
    }
}
