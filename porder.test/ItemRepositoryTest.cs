using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using porder.data.contract;
using porder.data;
using System.Collections;
using porder.model;
using System.Collections.Generic;

namespace porder.test
{
    [TestClass]
    public class ItemRepositoryTest
    {
        [TestMethod]
        public void QueryItemPrice()
        {
            IItemRepository itemRepo = new ItemRepository(new OrderDbContext());
            IList<ItemPrice> result = itemRepo.ItemPrice(1);

            Assert.IsTrue(result.Count > 0);
        }
    }
}
