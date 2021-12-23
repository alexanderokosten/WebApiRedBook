using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApiRedBook.Model
{
    
    public partial class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Spread { get; set; }
        public string Number { get; set; }
        public string Biology { get; set; }
        public string LimitingFactors { get; set; }
        public string SecurityMeasures { get; set; }
        public int ClassItemId { get; set; }
        public int StatusId { get; set; }
        public string Image { get; set; }
      
        public virtual ClassItem ClassItem { get; set; }
        public virtual Status Status { get; set; }
    }
}
