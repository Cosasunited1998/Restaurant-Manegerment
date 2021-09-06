using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Menu
    {

        public Menu(int id, int foodID, int billID, int count)
        {
            this.ID = id;
            this.FoodID = foodID;
            this.BillID = billID;
            this.Count = count;
        }


        public Menu(DataRow row)
        {
            {
                this.ID = (int)row["id"];
                this.FoodID = (int)row["idFood"];
                this.BillID = (int)row["idBill"];
                this.Count = (int)row["count"];
            }
        }

        private int count;
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private int foodID;
        public int FoodID
        {
            get { return foodID; }
            set { foodID = value; }
        }
        private int billID;
        public int BillID
        {
            get { return billID;}
            set { billID = value; }
        }
        private int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
    }
}
