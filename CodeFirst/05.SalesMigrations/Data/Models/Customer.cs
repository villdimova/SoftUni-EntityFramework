using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_SalesDatabase.Data.Models
{
    /*
    	CustomerId
    	Name (up to 100 characters, unicode)
    	Email (up to 80 characters, not unicode)
    	CreditCardNumber (string)
    	Sales

    */
    public class Customer
    {

        public Customer()
        {
            this.Sales = new HashSet<Sale>();
        }
        public int CustomerId { get; set; }

        [MaxLength(DataValidations.CustomerName)]
        public string Name { get; set; }


        [MaxLength(DataValidations.CustomerEmail)]
        public string Email { get; set; }

        public string CreditCardNumber { get; set; }

        public ICollection<Sale> Sales { get; set; }

    }
}
