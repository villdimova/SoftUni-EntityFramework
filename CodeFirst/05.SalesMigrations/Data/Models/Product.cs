using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_SalesDatabase.Data.Models
{
    /*
    •	Product:
         	ProductId
         	Name (up to 50 characters, unicode)
         	Quantity (real number)
         	Price
         	Sales


     */


    public class Product
    {

        public Product()
        {
            this.Sales = new HashSet<Sale>();
        }
        public int ProductId { get; set; }

        [MaxLength(DataValidations.ProductName)]
        public string Name { get; set; }

        public double Quantity { get; set; }
        public decimal Price { get; set; }

        [MaxLength(DataValidations.ProductDescription)]
        [DefaultValue("No description")]
        public string Description { get; set; }

        public ICollection<Sale> Sales { get; set; }

    }
}
