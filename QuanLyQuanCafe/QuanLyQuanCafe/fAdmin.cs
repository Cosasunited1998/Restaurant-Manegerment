using QuanLyQuanCafe.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            dgvFood.DataSource = foodList;
            dgvAcc.DataSource = accountList;
            LoadDateTimePickerView();
            LoadListViewByDate(dtpFromDate.Value, dtpToDate.Value);
            LoadListFood();
            LoadAccount();
            LoadCategoryIntoCombobox(cbbCategory);
            AddFoodBlinding();
            AddAccountBinding();
        }
        #region method
        void AddAccount(string username, string displayname, int type)
        {
            if(AccountDAO.Instance.InsertAccount(username, displayname, type))
            {
                MessageBox.Show("Thêm tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
                return;
            }
            LoadAccount();

        }
        void UpdateAccount(string username, string displayname, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(username, displayname, type))
            {
                MessageBox.Show("Thay đổi thông tin tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Thay đổi thông tin tài khoản thất bại");
            }
            LoadAccount();

        }
        void DeleteAccount(string username)
        {
            if (loginAccount.UserName.Equals(username))
            {
                MessageBox.Show("Đây là tài khoản đang được sử dụng!");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(username))
            {
                MessageBox.Show("Xóa thông tin tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Xóa thông tin tài khoản thất bại");
            }
            LoadAccount();

        }
        void ResetPass(string username)
        {
            if (AccountDAO.Instance.ResetPass(username))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công!");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txtSearchFood.Text);
        }

        private void btnViewCategory_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnAddAcc_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string displayname = txtDisplayName.Text;
            int type = (int)nmAcountType.Value;
            AddAccount(username, displayname, type);
        }

        private void btnUpdateAcc_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string displayname = txtDisplayName.Text;
            int type = (int)nmAcountType.Value;
            UpdateAccount(username, displayname, type);
        }

        private void btnDeleteAcc_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            DeleteAccount(username);
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            ResetPass(username);
        }
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);
            return listFood;
        }
        void AddAccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding ("Text",dgvAcc.DataSource,"UserName", true, DataSourceUpdateMode.Never));
            txtDisplayName.DataBindings.Add(new Binding("Text", dgvAcc.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmAcountType.DataBindings.Add(new Binding("Value", dgvAcc.DataSource, "Type", true, DataSourceUpdateMode.Never));


        }
        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadDateTimePickerView()
        {
            DateTime today = DateTime.Now;
            dtpFromDate.Value = new DateTime(today.Year,today.Month,1);
            dtpToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListViewByDate(DateTime checkIn, DateTime checkOut)
        {
           dgvBill.DataSource = BillDAO.Instance.GetBillByDate(checkIn, checkOut);
        }
        void AddFoodBlinding()
        {
            txtNameFood.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            IDFood.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        #endregion

        #region events
        private void btnView_Click(object sender, EventArgs e)
        {
            LoadListViewByDate(dtpFromDate.Value, dtpToDate.Value);
        }
        
        void LoadListFood()
        {
           foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
        private void btnViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void IDFood_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                    Category category = CategoryDAO.Instance.GetCategoryByID(id);
                    cbbCategory.SelectedItem = category;
                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbbCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbbCategory.SelectedIndex = index;
                }
            }
            catch { }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtNameFood.Text;
            int idCategory = (cbbCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            if (FoodDAO.Instance.InsertFood(name, idCategory, price))
            {
                MessageBox.Show("Thêm món thành công!");
                LoadListFood();
                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else MessageBox.Show("Không thể thêm được món");
        }

        private void btnUpdateFood_Click(object sender, EventArgs e)
        {
            string name = txtNameFood.Text;
            int idCategory = (cbbCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(IDFood.Text);
            if (FoodDAO.Instance.UpdateFood(name, idCategory, price, id))
            {
                MessageBox.Show("Thay đổi món thành công!");
                LoadListFood();
                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else MessageBox.Show("Không thể thay đổi món");
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(IDFood.Text);
            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công!");
                LoadListFood();
                if(deleteFood != null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else MessageBox.Show("Không thể xóa món");
        }
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }
        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }


        #endregion

        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txtShowPageBillNum.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBill(dtpFromDate.Value, dtpToDate.Value);
            int LastPage = sumRecord / 10;
            if (sumRecord % 10 != 0)
                LastPage++;
            txtShowPageBillNum.Text = LastPage.ToString();
        }

        private void txtShowPageBillNum_TextChanged(object sender, EventArgs e)
        {
            dgvBill.DataSource = BillDAO.Instance.GetBillByDateAndPage(dtpFromDate.Value, dtpToDate.Value, Convert.ToInt32(txtShowPageBillNum.Text));
        }

        private void btnPreviousBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtShowPageBillNum.Text);
            if (page > 1)
                page--;
            txtShowPageBillNum.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtShowPageBillNum.Text);
            int sumRecord = BillDAO.Instance.GetNumBill(dtpFromDate.Value, dtpToDate.Value);
            if (page < sumRecord)
                page++;
            txtShowPageBillNum.Text = page.ToString();

        }
    }

}
