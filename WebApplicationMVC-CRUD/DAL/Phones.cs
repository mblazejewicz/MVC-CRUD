namespace WebApplicationMVC_CRUD.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Phones
    {
        [Display(Name = "ID")]
        [Key]
        public int PhoneId { get; set; }

        [Required]
        [Display(Name = "Model Name")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        public int ManufacturerId { get; set; }

        public virtual Manufacturers Manufacturers { get; set; }
    }
}
