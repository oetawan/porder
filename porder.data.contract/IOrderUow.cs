using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using porder.model;

namespace porder.data.contract
{
    public interface IOrderUow
    {
        IRepository<Grouping> Groups { get; }
        void Commit();
    }
}