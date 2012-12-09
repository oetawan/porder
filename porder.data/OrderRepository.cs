using System;
using System.Linq;
using porder.model;
using System.Collections;
using System.Collections.Generic;
using porder.data.contract;
using System.Transactions;
using System.Data.Common;
using System.Data.SqlClient;
namespace porder.data
{
    public class OrderRepository : IOrderRepository
    {
        readonly OrderDbContext dbContext;
        DbTransaction tran;
        DbConnection cn;
        Owner owner;
        Vendor vendor;
        int soId;

        public OrderRepository(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Owner GetOwner()
        {
            IEnumerable owner = dbContext.Database.SqlQuery(typeof(Owner),
                @"SELECT OwnerName, EmployeeId, DefTerm FROM tblOwner");
            return owner.Cast<Owner>().FirstOrDefault();
        }

        public Vendor GetVendor(string vendorCode)
        {
            return new VendorRepository(this.dbContext).FindByCode(vendorCode);
        }

        public void Add(Order order)
        {
            owner = GetOwner();
            vendor = GetVendor(order.VendorID);
            
            using (cn = dbContext.Database.Connection)
            {
                try
                {
                    cn.Open();
                    tran = cn.BeginTransaction();
                    DbCommand cmd = cn.CreateCommand();
                    cmd.Transaction = tran;
                    cmd.CommandText = @"INSERT INTO tblSO (SOCode, SODate, BranchId, CurrencyId, EmployeeId, VendorID, TermID, TaxID, SOGrossAmt, SONetAmt, StatusID, Operator, Computer, LastUpdate) 
                      VALUES (@SOCode, @SODate, @BranchId, @CurrencyId, @EmployeeId, @VendorId, @TermId, 0, @SOGrossAmt, @SONetAmt, 0, @Operator, @Computer, @LastUpdate); SELECT IDENT_CURRENT('tblSO');";

                    cmd.Parameters.Add(new SqlParameter("@SOCode", order.SOCode));
                    cmd.Parameters.Add(new SqlParameter("@SODate", order.SODate));
                    cmd.Parameters.Add(new SqlParameter("@BranchId", order.BranchID));
                    cmd.Parameters.Add(new SqlParameter("@CurrencyId", order.CurrencyId));
                    cmd.Parameters.Add(new SqlParameter("@EmployeeId", owner.EmployeeId));
                    cmd.Parameters.Add(new SqlParameter("@VendorId", vendor.Id));
                    cmd.Parameters.Add(new SqlParameter("@TermId", owner.DefTerm));
                    cmd.Parameters.Add(new SqlParameter("@SOGrossAmt", order.SOGrossAmt));
                    cmd.Parameters.Add(new SqlParameter("@SONetAmt", order.SONetAmt));
                    cmd.Parameters.Add(new SqlParameter("@Operator", order.Username));
                    cmd.Parameters.Add(new SqlParameter("@Computer", System.Environment.MachineName));
                    cmd.Parameters.Add(new SqlParameter("@LastUpdate", DateTime.Now));

                    soId = Convert.ToInt32(cmd.ExecuteScalar());

                    foreach (OrderItem item in order.Items)
                    {
                        AddOrderItem(item);
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    if (tran != null)
                        tran.Rollback();

                    throw ex;
                }
                finally
                {
                    cn.Close();
                }
            }
        }

        private void AddOrderItem(OrderItem item)
        {
            DbCommand cmd = cn.CreateCommand();
            cmd.Transaction = tran;
            cmd.CommandText= @"INSERT INTO tblSODetail (SOId, SOSeq, ItemID, UnitCode, Quantity, Price, GrossAmt, SubTotal)
                               VALUES (@SOId, @SOSeq, @ItemID, @UnitCode, @Quantity, @Price, @GrossAmt, @SubTotal)";

            cmd.Parameters.Add(new SqlParameter("@SOId", soId));
            cmd.Parameters.Add(new SqlParameter("@SOSeq", item.SOSeq));
            cmd.Parameters.Add(new SqlParameter("@ItemId", item.ItemID));
            cmd.Parameters.Add(new SqlParameter("@UnitCode", item.UnitCode));
            cmd.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
            cmd.Parameters.Add(new SqlParameter("@Price", item.Price));
            cmd.Parameters.Add(new SqlParameter("@GrossAmt", item.GrossAmt));
            cmd.Parameters.Add(new SqlParameter("@SubTotal", item.SubTotal));

            cmd.ExecuteScalar();
        }
    }
}