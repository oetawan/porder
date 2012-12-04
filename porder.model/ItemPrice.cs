using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace porder.model
{
    public class ItemPrice
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Int16 PriceCatId { get; set; }
        public string UnitCode { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string CurrencyId { get; set; }
        public decimal MinQty { get; set; }
        public decimal MaxQty { get; set; }
    }
}