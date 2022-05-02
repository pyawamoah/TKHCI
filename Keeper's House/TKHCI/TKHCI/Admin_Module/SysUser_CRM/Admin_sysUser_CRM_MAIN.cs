using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TKHCI.Admin_Module.SysUser_CRM
{
    public partial class Admin_sysUser_CRM_MAIN : Form
    {
        #region BEFORE MAIN FORM ADD'S
        private readonly string loadFam_ADD = "Select 'Select System User Family' as sysFAM union all SELECT distinct FamTB.Fam_Name FamilyName from FamTB with (nolock)";
        private readonly string loadFam_Upt = "Select 'Select System User Family' as sysFAM2 union all SELECT distinct FamTB.Fam_Name FamilyName from FamTB with (nolock)";

        KeeperDBEntities DBS = new KeeperDBEntities();
        MemoryStream ms;
        decimal id;
        string gender = string.Empty;
        int sysUser;
        private decimal ID;
        public static class UserSession
        {
            public static string Username;
        }
        #endregion
        public Admin_sysUser_CRM_MAIN()
        {
            InitializeComponent();
            Keepers_LOAD_userID();
            _ = Task_Load();
        }

        #region SQL CONNECTION
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        #endregion
        #region POPULATE DATAGRID
        public async Task Task_Load()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Load_Combo_Box());
            await Task.WhenAll(tasks);
        }
        private Task Load_Combo_Box()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(loadFam_ADD, Con);
            DataTable familyTB = new DataTable();
            da.Fill(familyTB);
            Combo_UserFam_Add.DataSource = familyTB;
            Combo_UserFam_Add.DisplayMember = "sysFAM";
            Combo_UserFam_Add.ValueMember = "sysFAM";

            DataTable famName_Upt = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(loadFam_Upt, Con);
            da1.Fill(famName_Upt);
            Combo_UserFam_Upt.DataSource = famName_Upt;
            Combo_UserFam_Upt.DisplayMember = "sysFAM2";
            Combo_UserFam_Upt.ValueMember = "sysFAM2";
            Con.Close();
            return Task.CompletedTask;
        }
        private void Keepers_LOAD_userID()
        {
            string constr = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_SysUser_LoadID", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            sqlConnection.Close();


            if (xml < 1)
            {
                ID = 1;
                Tx_Empl_ID_Add.Text = $"KSU-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else if (xml >= 1)
            {
                ID = xml + 1;
                Tx_Empl_ID_Add.Text = $"KSU-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else return;

        }
        public object PopulateDataGridView()
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
        }
        private void DataGrib_ReqType_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtUsername_Hide.Text = DataGrib_ReqType.SelectedRows[0].Cells[2].Value.ToString();
            UserSession.Username = txtUsername_Hide.Text;
            CELLCLICK(null, null);
        }
        private void CELLCLICK(object sender, EventArgs e)
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
                    Combo_UserFam_Upt.Text = user1.UserFam;
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


                    Tx_Empl_ID.Text = user1.TKHS_ID.ToString();
                    Combo_Dept.Text = user1.Dep;
                    Combo_SysRole.Text = user1.SysRoll;
                    ComBo_OffiCapacity.Text = user1.OfficialCapacity;
                    Combo_Status.Text = user1.Member_Status;
                    Tx_Username.Text = user1.Username;
                    Tx_NOK_Name_sys.Text = user1.NOK_Name;
                    Tx_NOK_Phone_sys.Text = user1.NOK_Phone;
                    Tx_NOK_Address_sys.Text = user1.NOK_Address; ;
                    Combo_NOK_Relation_sys.Text = user1.NOKRela;
                    Tx_HideID.Text = user1.UT_ID.ToString();

                }
            }
        }
        #endregion
        #region FORM LOAD
        private void Admin_sysUser_CRM_MAIN_Load(object sender, EventArgs e)
        {
            DataGrib_ReqType.DataSource = this.PopulateDataGridView();
        }
        #endregion
        #region SEARCH DATA
        private void txtSearch_By_Name_TextChanged(object sender, EventArgs e)
        {
            DataGrib_ReqType.DataSource = this.PopulateDataGridView();
        }
        #endregion
        #region UPDATE RECORD BASIC INFORMATION
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
                    cat.UT_ID = Convert.ToInt32(Tx_T_IDHide.Text);
                    cat.Title = Combo_Title.Text;
                    cat.FName = Tx_FName.Text;
                    cat.MName = Tx_SName.Text;
                    cat.LName = Tx_LName.Text;
                    cat.DOB = Dt_DOB.Value.Date;
                    cat.Phone = Tx_Phone.Text;
                    cat.Address = Tx_Address.Text;
                    cat.Marrital_Status = Combo_MS.Text;
                    cat.Gender = gender;
                    cat.UserFam = Combo_UserFam_Upt.Text;
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
        #endregion
        #region DELATE RECORD
        private void BUT_Del_Click(object sender, EventArgs e)
        {
            UserTB acc = new UserTB();
            if (Tx_T_IDHide.Text == "")
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
                        Admin_sysUser_CRM_MAIN_Load(null, null);
                    }
                    else return;
                }
                else MessageBox.Show("Select User to delete");
            }
        }
        #endregion
        #region UPDATE USER SYSTEM INFORMATION
        private void Bt_Update_Click(object sender, EventArgs e)
        {
            if (Tx_Username.Text != null)
            {
                sysUser = Convert.ToInt32(Tx_HideID.Text);
                var cat = (from s in DBS.UserTBs where s.UT_ID == sysUser select s).FirstOrDefault();
                string UserName = Tx_Username.Text;
                cat.UT_ID = sysUser;
                cat.Username = UserName;
                cat.NOK_Name = Tx_NOK_Name.Text;
                cat.NOK_Address = Tx_NOK_Address.Text;
                cat.NOK_Phone = Tx_NOK_Phone.Text;
                cat.NOKRela = Combo_NOK_Relation.Text;
                cat.TKHS_ID = Tx_Empl_ID.Text;
                cat.Dep = Combo_Dept.Text;
                cat.SysRoll = Combo_SysRole.Text;
                cat.OfficialCapacity = ComBo_OffiCapacity.Text;
                cat.Member_Status = Combo_Status.Text;
                cat.Username = Tx_Username.Text;

                string message = "Are you sure you want to update this record?";
                string title = "updating Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {

                    DBS.SaveChanges();
                    MessageBox.Show("User Record Updated Successfully");
                }
                else return;
            }
        }
        #endregion
        #region USE ME
        #endregion
        #region USER IMAGE UPDATE
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
        private void But_UploadImg_Add_Click(object sender, EventArgs e)
        {
            OpenFileDialog openbdlg = new OpenFileDialog();
            if (openbdlg.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openbdlg.FileName);
                LogUserImage_Add.Image = img;
                ms = new MemoryStream();
                img.Save(ms, img.RawFormat);
            }
        }
        #endregion
        #region ADD NEW USER
        private void BT_AddNew_Add_Click(object sender, EventArgs e)
        {
            UserTB acc = new UserTB();
            if (radioButtonMale_Add.Checked)
            {
                gender = "Male";
            }
            else if (radioButtonFemale_Add.Checked)
            {
                gender = "Female";
            }

            try
            {

                if (LogUserImage_Add.Image != null)
                {
                    if (Tx_Username_Add.Text != "" && Tx_Password_Add.Text != "" && Tx_RE_Password_Add.Text != "")
                    {
                        if (Tx_Password_Add.Text.ToString().Trim().ToLower() == Tx_RE_Password_Add.Text.ToString().Trim().ToLower())
                        {
                            string UserName = Tx_Username_Add.Text;
                            string Password = Cryptography.Encrypt(Tx_Password_Add.Text.ToString());
                            acc.Username = UserName;
                            acc.Password = Password;
                        }
                        else
                        {
                            if (Application.OpenForms.OfType<Error_PasswordMissMatchy>().Count() == 1)
                            {
                                return;
                            }
                            else
                            {
                                Error_PasswordMissMatchy err3 = new Error_PasswordMissMatchy();
                                err3.ShowDialog();
                            }
                        }

                    }
                    else
                    {
                        if (Application.OpenForms.OfType<Error_UsernameOrPasswordEmpty>().Count() == 1)
                        {
                            return;
                        }
                        else
                        {
                            Error_UsernameOrPasswordEmpty err1 = new Error_UsernameOrPasswordEmpty();
                            err1.ShowDialog();
                            return;
                        }
                    }
                    if (Combo_Title_Add.Text != "" && Tx_Address_Add.Text != "" && Tx_City_Add.Text != "" && Tx_Email_Add.Text != "" && Tx_FName_Add.Text != "" && Tx_LName_Add.Text != ""
                        && Tx_NOK_Phone_Add.Text != "" && Tx_SName_Add.Text != "" && Tx_ZipCode_Add.Text != "")
                    {
                        acc.Title = Combo_Title_Add.Text;
                        acc.FName = Tx_FName_Add.Text;
                        acc.MName = Tx_SName_Add.Text;
                        acc.LName = Tx_LName_Add.Text;
                        acc.DOB = Dt_DOB_Add.Value.Date;
                        acc.Gender = gender;
                        acc.Phone = Tx_Phone_Add.Text;
                        acc.City = Tx_City_Add.Text;
                        acc.Zip_code = Tx_ZipCode_Add.Text;
                        acc.Address = Tx_Address_Add.Text;
                        acc.Region = Combo_Region_Add.Text;
                        acc.Email = Tx_Email_Add.Text;
                        acc.UserFam = Combo_UserFam_Add.Text;
                        acc.NOK_Phone = Tx_NOK_Phone_Add.Text;
                        acc.Marrital_Status = Combo_MS_Add.Text;
                        acc.Email = Tx_Email_Add.Text;
                        acc.Dep = Combo_Dept_Add.Text;
                        acc.SysRoll = Combo_SysRole_Add.Text;
                        acc.OfficialCapacity = ComBo_OffiCapacity_Add.Text;
                        acc.Member_Status = Combo_Status_Add.Text;
                        acc.TKHS_ID = Tx_Empl_ID_Add.Text;
                        acc.JDate = DateTime.Now;
                        acc.Create_dt = DateTime.Now;
                        acc.CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                        acc.Picture = ms.ToArray();
                        var fullname = new StringBuilder();
                        fullname.Append(Tx_FName_Add.Text);
                        fullname.Append(" ");
                        fullname.Append(Tx_SName_Add.Text);
                        fullname.Append(" ");
                        fullname.Append(Tx_LName_Add.Text);
                        acc.Fullname = fullname.ToString();
                        acc.Pwd_lock = 0;
                        acc.Admin_loc = "Locked";
                        acc.NOKRela = Combo_NOK_Relation_Add.Text;
                        acc.NOK_Address = Tx_NOK_Address_Add.Text;
                        acc.NOK_Name = Tx_NOK_Name_Add.Text;
                        acc.NOK_Phone = Tx_NOK_Phone_Add.Text;
                        acc.securitySet = 1;

                        string message = "Are you sure you want to add this record?";
                        string title = "Adding Record...";
                        MessageBoxButtons but = MessageBoxButtons.YesNo;
                        DialogResult result = MessageBox.Show(message, title, but);
                        if (result == DialogResult.Yes)
                        {
                            DBS.UserTBs.Add(acc);
                            DBS.SaveChanges();
                            MessageBox.Show("User Created Successfully");
                            BT_ClearForm_Add_Click(null, null);
                            DataGrib_ReqType.DataSource = this.PopulateDataGridView();
                        }
                        else return;
                    }
                    else
                    {
                        if (Application.OpenForms.OfType<Error_Fill_All_Form>().Count() == 1)
                        {
                            return;
                        }
                        else
                        {
                            Error_Fill_All_Form err2 = new Error_Fill_All_Form();
                            err2.ShowDialog();
                        }
                    }
                }
                else
                {
                    if (Application.OpenForms.OfType<Error_Passport_PIc_Empty>().Count() == 1)
                    {
                        return;
                    }
                    else
                    {
                        Error_Passport_PIc_Empty err2 = new Error_Passport_PIc_Empty();
                        err2.ShowDialog();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region CLEAR ADD FORM
        private void BT_ClearForm_Add_Click(object sender, EventArgs e)
        {
            Tx_FName_Add.Text = "";
            Tx_SName_Add.Text = "";
            Tx_LName_Add.Text = "";
            Tx_Phone_Add.Text = "";
            Tx_City_Add.Text = "";
            Tx_ZipCode_Add.Text = "";
            Tx_Address_Add.Text = "";
            Tx_Email_Add.Text = "";
            LogUserImage_Add.Image = null;
            Tx_NOK_Phone_Add.Text = "";
            Combo_Dept_Add.SelectedIndex = 0;
            Combo_SysRole_Add.SelectedIndex = 0;
            ComBo_OffiCapacity_Add.SelectedIndex = 0;
            Combo_Status_Add.SelectedIndex = 0;
            Combo_MS_Add.SelectedIndex = 0;
            Combo_Region_Add.SelectedIndex = 0;
            Combo_Title_Add.SelectedIndex = 0;
            Tx_Username_Add.Text = "";
            Tx_NOK_Address_Add.Text = "";
            Tx_NOK_Name_Add.Text = "";
            Tx_NOK_Phone_Add.Text = "";
            Combo_NOK_Relation_Add.SelectedIndex = 0;
        }
        #endregion
    }
}
