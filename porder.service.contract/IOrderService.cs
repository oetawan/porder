using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using porder.model;

namespace porder.service.contract
{
    [ServiceContract(Namespace = "urn:porder")]
    public interface IOrderService
    {
        [OperationContract]
        IList<Grouping> AllGroups();

        [OperationContract]
        IList<Item> FindItemByGroup(int groupId);

        [OperationContract]
        IList<Item> SearchItem(string keyword);

        [OperationContract]
        Vendor FindVendorByCode(string vendorCode);
    }
}