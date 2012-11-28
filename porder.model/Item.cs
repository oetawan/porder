using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace porder.model
{
    public class Item
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int GroupingId { get; set; }
        public string Name { get; set; }
        public string UnitCode { get; set; }
        public int DecimalPlace { get; set; }
        public string CurrencyId { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public DateTime MfgDate { get; set; }
        public short TaxId { get; set; }
        public string TaxCode { get; set; }
        public decimal TaxType { get; set; }
        public string TaxName { get; set; }
        public double TaxRate { get; set; }
    }
}