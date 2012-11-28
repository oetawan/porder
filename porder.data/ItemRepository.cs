using System;
using System.Linq;
using porder.model;
using System.Collections;
using System.Collections.Generic;
using porder.data.contract;
namespace porder.data
{
    public class ItemRepository: IItemRepository
    {
        readonly OrderDbContext dbContext;

        public ItemRepository(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IList<Item> FindItemByGroup(int groupId)
        {
            IEnumerable result = dbContext.Database.SqlQuery(typeof(Item),
                @"SELECT tblItem.ItemID AS id, 
                         tblItem.itmCode AS code, 
                         tblItem.GroupingId, 
                         tblItem.itmName AS name, 
                         tblItem.UnitCode, 
                         tblItem.itmDecPlace AS decimalplace, 
                         tblItem.CurrencyId, 
                         tblItem.itmPrice AS Price, 
                         tblItem.itmCostPrice AS costprice, 
                         tblItem.mfgDate, 
                         tblTax.TaxId, 
                         tblTax.Kode, 
                         tblTax.TaxType, 
                         tblTax.taxName, 
                         tblTax.taxRate
                  FROM tblItem INNER JOIN
                        tblTax ON tblItem.TaxId = tblTax.TaxId
                  WHERE (tblItem.itmDiscontinue = 0) AND 
                        (tblItem.deleted = 0) AND 
                        (tblItem.GroupingId = {0})", groupId);

            return result.Cast<Item>().ToList();
        }
    }
}