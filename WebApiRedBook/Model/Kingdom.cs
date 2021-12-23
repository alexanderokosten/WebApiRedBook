using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApiRedBook.Model
{
    public partial class Kingdom
    {
        public Kingdom()
        {
            Type = new HashSet<Type>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Type> Type { get; set; }
    }
}
