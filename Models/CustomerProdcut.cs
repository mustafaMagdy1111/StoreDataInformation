using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreDataInformation.Models
{
    public class CustomerProdcut
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer customer { get; set; }
        public int CategoryProductId { get; set; }
        public CategoryProduct CategoryProduct { get; set; }
    }
}