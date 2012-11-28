using System;
using System.Linq;
using System.Data.Entity;
namespace porder.data
{
    public class OrderDbContext: DbContext
    {
        public OrderDbContext(): base(nameOrConnectionString: "Order")
        {
        }

        static OrderDbContext()
        {
            //Database.SetInitializer(new OrderDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}