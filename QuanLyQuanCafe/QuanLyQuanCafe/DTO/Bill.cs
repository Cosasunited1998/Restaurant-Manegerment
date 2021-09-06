using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Bill
    {
        public Bill(int id, DateTime? dataCheckIn, DateTime? dataCheckOut, int status, int discount = 0)
        {
            this.ID = id;
            this.DataCheckIn = dataCheckIn;
            this.DataCheckOut = dataCheckOut;
            this.Status = status;
            this.Discount = discount;
        }

        public Bill(DataRow row)
        {
            this.ID = (int) row["id"];
            this.DataCheckIn = (DateTime?)row["dateCheckIn"];

            var dateCheckOutTemp = row["dateCheckOut"];
            if(dateCheckOutTemp.ToString() != "")
            this.DataCheckOut = (DateTime?)dateCheckOutTemp;

            this.Status = (int)row["status"];

            if( row["discount"].ToString()!= "")
                {
                this.Discount = (int)row["discount"];
            }

            
        }
        private int discount;
        public int Discount 
        {
            get { return discount; }
            set { discount = value; }
        }
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        private int status;
         public int Status
        {
            get { return status; }
            set { status = value; }
        }
        private DateTime? dataCheckOut;

        public DateTime? DataCheckOut
        {
            get { return dataCheckOut; }
            set { dataCheckOut = value; }
        }
        private DateTime? dataCheckIn;
        public DateTime? DataCheckIn
        {
            get { return dataCheckIn; }
            set { dataCheckIn = value; }
        }
    }
}
