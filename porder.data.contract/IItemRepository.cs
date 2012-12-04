using porder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace porder.data.contract
{
    public interface IItemRepository
    {
        IList<Item> FindItemByGroup(int groupId);
        IList<Item> SearchItem(string keyword);
        IList<ItemPrice> ItemPrice(Int16 priceCatId);
    }
}