using System;
using System.Collections.Generic;
using System.Linq;
using porder.service.contract;
using porder.data.contract;
using porder.data;

namespace porder.service
{
    public class OrderService : IOrderService
    {
        OrderDbContext dbContext;
        IGroupingRepository groupingRepository;
        IItemRepository itemRepository;

        public OrderService()
        {
            this.dbContext = new OrderDbContext();
            this.groupingRepository = new GroupingRepository(this.dbContext);
            this.itemRepository = new ItemRepository(this.dbContext);
        }

        public IList<model.Grouping> AllGroups()
        {
            return groupingRepository.FindAll();
        }

        public IList<model.Item> FindItemByGroup(int groupId)
        {
            return itemRepository.FindItemByGroup(groupId);
        }
    }
}