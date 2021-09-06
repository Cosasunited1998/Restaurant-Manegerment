using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; }
            private set { BillInfoDAO.instance = value; }
        }

        private BillInfoDAO() { }
        public void DeleteBillInfoByFoodID(int id)
        {
            DataProvider.Instance.ExcuteQuery("DELETE dbo.BillInfor WHERE idFood = " + id);
        }

        public List<Menu1> GetListBillInfo(int id)
        {
            List<Menu1> listBillInfo = new List<Menu1>();

            DataTable data = DataProvider.Instance.ExcuteQuery("SELECT * FROM dbo.BillInfor WHERE idBill = " + id);

            foreach (DataRow item in data.Rows)
            {
                Menu1 info = new Menu1(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }
        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExcuteNonQuery("exec USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }
    }
}
