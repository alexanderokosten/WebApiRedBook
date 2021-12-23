using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RedBookWebApplication.Model
{
    public partial class ClassItem
    {
        public ClassItem()
        {
            Item = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }


        public virtual Type Type { get; set; }
        public virtual ICollection<Item> Item { get; set; }
    }
}
