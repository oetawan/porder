using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace porder.model
{
    public class Vendor
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public short PriceCatId { get; set; }
        public string PriceCatCode { get; set; }
        public string PriceCatName { get; set; }
    }
}