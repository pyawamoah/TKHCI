using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TKHCI
{
    public partial class Admin_SystemUser_CRM : Form
    {
        private Form currentChildForm;
        KeeperDBEntities DBS = new KeeperDBEntities();
        MemoryStream ms;
        decimal id;
        string gender = string.Empty;
        public static class UserSession
        {
            public static string Username;
        }
        public Admin_SystemUser_CRM()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        private object PopulateDataGridView()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_LoadSearch_User", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", txtSearch_By_Name.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
            try
            {
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void OpenChildForm(Form childForm)
        {
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            SystemUser_Container.Controls.Add(childForm);
            SystemUser_Container.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        public bool IsFormOpen(Type formType)
        {
            foreach (Form form in Application.OpenForms)
                if (form.GetType().Name == form.Name)
                    return true;
            return false;
        }
        private void BT_BasicInfo_Click(object sender, EventArgs e)
        {
            var user1 = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(UserSession.Username));
            if (!String.IsNullOrEmpty(UserSession.Username))
            {
                if (user1 != null)
                {
                    Tx_FName.Text = user1.FName;
                    Tx_SName.Text = user1.MName;
                    Tx_LName.Text = user1.LName;
                    var dt = (DateTime)(user1.DOB);
                    Dt_DOB.Value = dt;
                    if (user1.Gender == "Male")
                    {
                        radioButtonMale.Checked = true;
                        radioButtonFemale.Checked = false;
                    }
                    else
                    {
                        radioButtonFemale.Checked = true;
                        radioButtonMale.Checked = false;
                    }
                    Tx_Phone.Text = user1.Phone;
                    Tx_City.Text = user1.City;
                    Tx_ZipCode.Text = user1.Zip_code;
                    Tx_Address.Text = user1.Address;
                    Tx_Email.Text = user1.Email;
                    var img = user1.Picture;
                    var ms = new MemoryStream(img);
                    LogUserImage.Image = Image.FromStream(ms);
                    Tx_NOK_Phone.Text = user1.NOK_Phone;
                    Combo_MS.Text = user1.Marrital_Status;
                    Combo_Region.Text = user1.Region;
                    Combo_Title.Text = user1.Title;
                    Tx_User_Name.Text = user1.Fullname;
                    Tx_NOK_Address.Text = user1.Address;
                    Tx_NOK_Phone.Text = user1.NOK_Phone;
                    Tx_NOK_Name.Text = user1.NOK_Name;
                    Combo_NOK_Relation.Text = user1.NOKRela;
                    Tx_T_IDHide.Text = Convert.ToString(user1.UT_ID);

                }
            }
        }
        private void BT_Sys_Info_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Admin_SystemUser_SysInfo>().Count() == 1)
            {
                if (Application.OpenForms.OfType<Error_DoubleOpenForm>().Count() == 1)
                {
                    return;
                }
                else
                {
                    Error_DoubleOpenForm eropen = new Error_DoubleOpenForm();
                    eropen.Show();
                }
            }
            else
            {
                OpenChildForm(new Admin_SystemUser_SysInfo());
            }
        }
        private void BT_AddNew_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Admin_SystemUser_AddNew>().Count() == 1)
            {
                if (Application.OpenForms.OfType<Error_DoubleOpenForm>().Count() == 1)
                {
                    return;
                }
                else
                {
                    Error_DoubleOpenForm eropen = new Error_DoubleOpenForm();
                    eropen.Show();
                }
            }
            else
            {
                OpenChildForm(new Admin_SystemUser_AddNew());
            }
        }
        private void DataGrib_ReqType_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtUsername_Hide.Text = DataGrib_ReqType.SelectedRows[0].Cells[2].Value.ToString();
            UserSession.Username = txtUsername_Hide.Text;
            BT_BasicInfo_Click(null, null);
        }
        private void Admin_SystemUser_CRM_Load(object sender, EventArgs e)
        {
            DataGrib_ReqType.DataSource = this.PopulateDataGridView();
        }
        private void txtSearch_By_Name_TextChanged(object sender, EventArgs e)
        {
            DataGrib_ReqType.DataSource = this.PopulateDataGridView();
        }
        private void But_Upload_Click(object sender, EventArgs e)
        {
            {
                OpenFileDialog openbdlg = new OpenFileDialog();
                if (openbdlg.ShowDialog() == DialogResult.OK)
                {
                    Image img = Image.FromFile(openbdlg.FileName);
                    LogUserImage.Image = img;
                    ms = new MemoryStream();
                    img.Save(ms, img.RawFormat);

                }
            }
        }
        private void But_Update_Click(object sender, EventArgs e)
        {
            if (radioButtonMale.Checked)
            {
                gender = "Male";
            }
            else if (radioButtonFemale.Checked)
            {
                gender = "Female";
            }
            if (LogUserImage.Image != null)
            {
                if (Tx_FName.Text != null)
                {
                    id = Convert.ToDecimal(Tx_T_IDHide.Text);
                    var cat = (from s in DBS.UserTBs where s.UT_ID == id select s).FirstOrDefault();
                    cat.UT_ID = Convert.ToDecimal(Tx_T_IDHide.Text);
                    cat.Title = Combo_Title.Text;
                    cat.FName = Tx_FName.Text;
                    cat.MName = Tx_SName.Text;
                    cat.LName = Tx_LName.Text;
                    cat.DOB = Dt_DOB.Value.Date;
                    cat.Phone = Tx_Phone.Text;
                    cat.Address = Tx_Address.Text;
                    cat.Marrital_Status = Combo_MS.Text;
                    cat.Gender = gender;
                    cat.City = Tx_City.Text;
                    cat.Region = Combo_Region.Text;
                    cat.Zip_code = Tx_ZipCode.Text;
                    cat.Email = Tx_Email.Text;
                    cat.Admin_loc = "Locked";
                    cat.Pwd_lock = 0;
                    var img = LogUserImage.Image;
                    if (ms != null)
                    {
                        img.Save(ms, img.RawFormat);
                        cat.Picture = ms.ToArray();
                    }
                    var fullname = new StringBuilder();
                    fullname.Append(Tx_FName.Text);
                    fullname.Append(" ");
                    fullname.Append(Tx_SName.Text);
                    fullname.Append(" ");
                    fullname.Append(Tx_LName.Text);
                    cat.Fullname = fullname.ToString();

                    string message = "Are you sure you want to update this record?";
                    string title = "updating Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {

                        DBS.SaveChanges();
                        MessageBox.Show("User Record Updated Successfully");
                        DataGrib_ReqType.DataSource = this.PopulateDataGridView();
                    }
                    else return;
                }
            }
        }
        private void BT_UpdatePhoto_Click(object sender, EventArgs e)
        {
            try
            {

                if (LogUserImage.Image != null)
                {
                    id = Convert.ToDecimal(Tx_T_IDHide.Text);
                    var cat = (from s in DBS.UserTBs where s.UT_ID == id select s).FirstOrDefault();
                    var img = LogUserImage.Image;
                    if (ms != null)
                    {
                        img.Save(ms, img.RawFormat);
                        cat.Picture = ms.ToArray();
                    }

                    string message = "Are you sure you want to update this record?";
                    string title = "updating Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {

                        DBS.SaveChanges();
                        MessageBox.Show("User Record Updated Successfully");
                        DataGrib_ReqType.DataSource = this.PopulateDataGridView();
                    }
                    else return;
                }
                else return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void BT_RefreshData_Click(object sender, EventArgs e)
        {
            DataGrib_ReqType.DataSource = this.PopulateDataGridView();
        }
        private void But_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void But_Del_Click(object sender, EventArgs e)
        {
            UserTB acc = new UserTB();
            if (Tx_T_IDHide.Text =="")
            {
                return;
            }
            else
            {
                decimal ID = Convert.ToDecimal(Tx_T_IDHide.Text);
                if (!ID.Equals(null))
                {
                    var cat = (from s in DBS.UserTBs where s.UT_ID == ID select s).First();

                    string message = "Are you sure you want to delete this Product?";
                    string title = "Deleting Product...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.UserTBs.Remove(cat);
                        DBS.SaveChanges();
                        MessageBox.Show("Product Deleted");
                        Admin_SystemUser_CRM_Load(null, null);
                    }
                    else return;
                }
                else MessageBox.Show("Select User to delete");
            }        
        }
    }
}
