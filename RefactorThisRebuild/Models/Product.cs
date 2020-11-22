﻿
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RefactorThisRebuild.Models
{
    public class Product
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        [JsonIgnore]
        public ICollection<ProductOption> ProductOptions { get; set; }


        [JsonIgnore]
        public bool IsNew { get; }


        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }
        public Product(Guid id)
        {
            Id = id;
            IsNew = true;
        }
    }
}
