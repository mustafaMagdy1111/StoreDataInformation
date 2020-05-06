using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreDataInformation.Models
{
    public class CategoryProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Size { get; set; }
        public string PriceInLE { get; set; }
        public string Color { get; set; }
        public int NumberInStock { get; set; }
        public Category Category { get; set; }
        [Display(Name = "Category")]
        [Required]
        public byte CategoryId { get; set; }
    }
}