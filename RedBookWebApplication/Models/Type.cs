using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RedBookWebApplication.Model
{
    public partial class Type
    {
        public Type()
        {
            ClassItem = new HashSet<ClassItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int KingdomId { get; set; }

        public virtual Kingdom Kingdom { get; set; }
        public virtual ICollection<ClassItem> ClassItem { get; set; }
    }
}
