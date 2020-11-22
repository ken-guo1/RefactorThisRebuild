using System.Text.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations;

namespace RefactorThisRebuild.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        [Required]
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }


        [JsonIgnore] 
        public bool IsNew { get; }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }
    }
}
