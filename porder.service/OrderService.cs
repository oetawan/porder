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
        IVendorRepository vendorRepository;
        IOrderRepository orderRepository;

        public OrderService()
        {
            this.dbContext = new OrderDbContext();
            this.groupingRepository = new GroupingRepository(this.dbContext);
            this.itemRepository = new ItemRepository(this.dbContext);
            this.vendorRepository = new VendorRepository(this.dbContext);
            this.orderRepository = new OrderRepository(this.dbContext);
        }

        public IList<model.Grouping> AllGroups()
        {
            return groupingRepository.FindAll();
        }

        public IList<model.Item> FindItemByGroup(int groupId)
        {
            return itemRepository.FindItemByGroup(groupId);
        }

        public IList<model.Item> SearchItem(string keyword)
        {
            return itemRepository.SearchItem(keyword);
        }

        public model.Vendor FindVendorByCode(string vendorCode)
        {
            return vendorRepository.FindByCode(vendorCode);
        }

        public model.CreateOrderResponse CreateOrder(model.Order order)
        {
            try
            {
                orderRepository.Add(order);
                return new model.CreateOrderResponse { 
                    Error = false
                };
            }
            catch (Exception ex)
            {
                return new model.CreateOrderResponse
                {
                    Error = true,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}