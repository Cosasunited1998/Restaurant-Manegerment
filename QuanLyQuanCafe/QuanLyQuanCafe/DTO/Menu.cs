﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
   public class Menu1
    {

        public Menu1(string foodName, int count, float price, float totalPrice = 0)
        {
            this.FoodName = foodName;
            this.Count = count;
            this.Price = price;
            this.TotalPrice = totalPrice;
        }
        public Menu1(DataRow row)
        {
            this.FoodName = row["Name"].ToString();
            this.Count =(int)row["count"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.TotalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());
        }
        private string foodName;
        private int count;
        private float price;
        private float totalPrice;

        public string FoodName
        {
            get { return foodName; }
            set { foodName = value; }
        }
        public int Count {
            get { return count; }
            set { count = value; }
        }
        public float Price {
            get { return price; }
            set { price = value; }
        }
        public float TotalPrice {
            get { return totalPrice; }
            set { totalPrice = value; }
        }
    
    }
}
