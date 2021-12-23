using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RedBookWebApplication.Model
{
    public partial class Item
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "spread")]
        public string Spread { get; set; }
        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }
        [JsonProperty(PropertyName = "biology")]
        public string Biology { get; set; }
        [JsonProperty(PropertyName = "limitingFactors")]
        public string LimitingFactors { get; set; }
        [JsonProperty(PropertyName = "securityMeasures")]
        public string SecurityMeasures { get; set; }
        [JsonProperty(PropertyName = "classItemId")]
        public int ClassItemId { get; set; }
        [JsonProperty(PropertyName = "statusId")]
        public int StatusId { get; set; }
        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "classItem")]
        public virtual ClassItem ClassItem { get; set; }
        [JsonProperty(PropertyName = "status")]
        public virtual Status Status { get; set; }
    }
}
