using System;
using System.Collections.Generic;
using System.Linq;
using porder.model;
namespace porder.data.contract
{
    public interface IGroupingRepository
    {
        IList<Grouping> FindAll();
    }
}