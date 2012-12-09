using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace porder.model
{
    public class Order
    {
        public string SOCode { get; set; }
        public DateTime SODate { get; set; }
        public string BranchID { get; set; }
        public string CurrencyId { get; set; }
        public string VendorID { get; set; }
        public decimal SOGrossAmt { get; set; }
        public decimal SONetAmt { get; set; }
        public string Username { get; set; }
        
        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public int SOSeq { get; set; }
        public int ItemID { get; set; }
        public string UnitCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal SubTotal { get; set; }
    }
}