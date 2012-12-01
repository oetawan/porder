using System;
using System.Linq;
using porder.model;
using System.Collections;
using System.Collections.Generic;
using porder.data.contract;
namespace porder.data
{
    public class VendorRepository: IVendorRepository
    {
        readonly OrderDbContext dbContext;

        public VendorRepository(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Vendor FindByCode(string code)
        {
            var vendor = dbContext.Database.SqlQuery<Vendor>(
                @"SELECT v.VendorId AS id, 
                         v.vndCode AS code, 
                         v.vndName AS name, 
                         v.vndAddress AS address, 
                         pcat.PriceCatId, 
                         pcat.pctCode AS pricecatcode, 
                         pcat.pctName AS pricecatname
                  FROM tblVendor AS v LEFT OUTER JOIN
                        tblPriceCat AS pcat ON pcat.PriceCatId = v.PriceCatId
                  WHERE (v.vndCode = {0})", code);

            return vendor.FirstOrDefault();
        }
    }
}