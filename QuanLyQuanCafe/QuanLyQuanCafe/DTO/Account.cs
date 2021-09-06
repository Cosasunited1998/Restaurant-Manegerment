using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
   public class Account
    {
        public Account(string userName, string displayName, int type, string password = null)
        {
            this.DisplayName = displayName;
            this.UserName = userName;
            this.Type = type;
            this.PassWord = password;
        }

        public Account(DataRow row)
        {
            this.DisplayName = row["displayName"].ToString();
            this.UserName = row["userName"].ToString();
            this.Type = (int)row["type"];
            this.PassWord = row["password"].ToString();
        }


        private string userName;
        private string displayName;
        private string passWord;
        private int type;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string DisplayName
        {        
             get { return displayName; }
             set { displayName = value; }
        }
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
