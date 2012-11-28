using System;
using System.Linq;
using porder.model;
using System.Collections;
using System.Collections.Generic;
using porder.data.contract;
namespace porder.data
{
    public class GroupingRepository: IGroupingRepository
    {
        readonly OrderDbContext dbContext;

        public GroupingRepository(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IList<Grouping> FindAll()
        {
            IEnumerable allGroups = dbContext.Database.SqlQuery(typeof(Grouping), "select GroupingId, groCode as Code, groName as Name from tblGrouping");

            return allGroups.Cast<Grouping>().ToList();
        }
    }
}