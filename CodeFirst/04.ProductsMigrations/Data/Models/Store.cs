using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_SalesDatabase.Data.Models
{
    /*
    	StoreId
    	Name (up to 80 characters, unicode)
    	Sales

    */
    public class Store
    {

        public Store()
        {
            this.Sales = new HashSet<Sale>();
        }
        public int StoreId { get; set; }

        [MaxLength(DataValidations.StoreName)]
        public string Name { get; set; }

        public ICollection<Sale> Sales { get; set; }
    }
}
