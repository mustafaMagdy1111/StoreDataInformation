using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreDataInformation.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public AreaCode AreaCode { get; set; }

        [Display(Name = "AreaCode")]
        [Required]
        public byte AreaCodeId { get; set; }





    }
}