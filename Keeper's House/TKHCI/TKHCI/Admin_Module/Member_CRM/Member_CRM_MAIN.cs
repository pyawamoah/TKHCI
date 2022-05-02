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

namespace TKHCI.Admin_Module.Member_CRM
{
    public partial class Member_CRM_MAIN : Form
    {
        #region
        #endregion
        #region FORM START ADD ON'S
        private readonly string loadMembersfam = "Select 'Select Member Family' as FamilyName union all SELECT distinct FamTB.Fam_Name FamilyName from FamTB with (nolock)";
        private readonly string loadCells = "Select 'Select Members Cell' as cellName union all SELECT distinct CellTB.Cell_Name cellName from CellTB WITH (NOLOCK)";

        private readonly string loadMembersfam2 = "Select 'Select Member Family' as FamilyName union all SELECT distinct FamTB.Fam_Name FamilyName from FamTB with (nolock)";
        private readonly string loadMembersfamMain = "Select 'Select Member Family' as FamilyName union all SELECT distinct FamTB.Fam_Name FamilyName from FamTB with (nolock)";
        private readonly string loadCells2 = "Select 'Select Members Cell' as cellName union all SELECT distinct CellTB.Cell_Name cellName from CellTB WITH (NOLOCK)";

        private readonly string serviceName = "Select 'Select Service Name' as serName union all SELECT distinct ServiceTB.Service_Name  from ServiceTB WITH (NOLOCK)";
        KeeperDBEntities DBS = new KeeperDBEntities();
        MemoryStream ms;
        MemoryStream ms2;
        public static class UserSession2
        {
            public static string FamID;
        }
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        decimal id;
        decimal ID;
        string gender = string.Empty;
        public static class UserSession
        {
            public static string MemberID;
        }
        #endregion
        public Member_CRM_MAIN()
        {
            InitializeComponent();
            _ = Task_Load();
        }
        #region TASK MASTER
        public async Task Task_Load()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Keepers_Member_LoadID());
            tasks.Add(Load_Combo_Box());
            tasks.Add(Load_Combo_Box2());
            tasks.Add(Keepers_FamID_Load());
            await Task.WhenAll(tasks);
        }
        #endregion
        #region ADD MEMBER & FAMILY ID's
        private Task Keepers_Member_LoadID()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_Member_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();
            if (xml < 1)
            {
                ID = 1;
                Tx_Mem_ID.Text = $"KM-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else if (xml >= 1)
            {
                ID = xml + 1;
                Tx_Mem_ID.Text = $"KM-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            return Task.CompletedTask;
        }
        private Task Keepers_FamID_Load()
        {
            string constr = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_Family_LoadID", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            sqlConnection.Close();


            if (xml < 1)
            {
                ID = 1;
                Tx_Fam_ID.Text = $"KHF-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else if (xml >= 1)
            {
                ID = xml + 1;
                Tx_Fam_ID.Text = $"KHF-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            return Task.CompletedTask;

        }
        #endregion
        #region LOAD COMBO BOX'S
        private Task Load_Combo_Box()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(loadMembersfam, Con);
            DataTable familyTB = new DataTable();
            da.Fill(familyTB);
            Combo_Family_Name_Updt.DataSource = familyTB;
            Combo_Family_Name_Updt.DisplayMember = "FamilyName";
            Combo_Family_Name_Updt.ValueMember = "FamilyName";

            DataTable cellTB = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(loadCells, Con);
            da1.Fill(cellTB);
            Combo_Mem_Cell_Name_Updt.DataSource = cellTB;
            Combo_Mem_Cell_Name_Updt.DisplayMember = "cellName";
            Combo_Mem_Cell_Name_Updt.ValueMember = "cellName";
            Con.Close();


            DataTable serName = new DataTable();
            SqlDataAdapter seN = new SqlDataAdapter(serviceName, Con);
            seN.Fill(serName);
            Combo_Service_Type_Updt.DataSource = serName;
            Combo_Service_Type_Updt.DisplayMember = "serName";
            Combo_Service_Type_Updt.ValueMember = "serName";
            Con.Close();




            
            return Task.CompletedTask;
        }
        private Task Load_Combo_Box2()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(loadMembersfam2, Con);
            DataTable familyTB = new DataTable();
            da.Fill(familyTB);
            Combo_Family_Name.DataSource = familyTB;
            Combo_Family_Name.DisplayMember = "FamilyName";
            Combo_Family_Name.ValueMember = "FamilyName";

            DataTable cellTB = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(loadCells2, Con);
            da1.Fill(cellTB);
            Combo_Mem_Cell_Name.DataSource = cellTB;
            Combo_Mem_Cell_Name.DisplayMember = "cellName";
            Combo_Mem_Cell_Name.ValueMember = "cellName";
            Con.Close();

            return Task.CompletedTask;
        }

        #endregion
        #region DATAGRID VIEW POPULATION
        public object PopulateDataGridView_Member()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_LoadSearch_Member", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", txtSearch_Member_By_Name.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateDataGridView_Fam()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_Load_and_Search_Fam", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_FAM.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public void LOAD_MEMBER_DATA()
        {
            DataGrib_MEMBER.DataSource = this.PopulateDataGridView_Member();
        }
        #endregion
        #region SEARCH OBJECT BY NAME
        private void txtSearch_Member_By_Name_TextChanged(object sender, EventArgs e)
        {
            DataGrib_MEMBER.DataSource = this.PopulateDataGridView_Member();
        }
        private void Tx_Search_FAM_TextChanged(object sender, EventArgs e)
        {
            DataGrib_Family.DataSource = this.PopulateDataGridView_Fam();
        }
        #endregion
        #region LOAD UPDATE FROM DATAGRIBVIEW TO TEXTBOX
        public void UpdateMember()
        {
            var user1 = DBS.MembTBs.FirstOrDefault(a => a.TKC_MemID.Equals(UserSession.MemberID));
            if (!String.IsNullOrEmpty(UserSession.MemberID))
            {
                if (user1 != null)
                {
                    Tx_FName_Updt.Text = user1.Fname;
                    Tx_SName_Updt.Text = user1.Sname;
                    Tx_LName_Updt.Text = user1.Lname;
                    var dt = (DateTime)(user1.DOB);
                    Dt_DOB_Updt.Value = dt;
                    if (user1.Gender == "Male")
                    {
                        radioButtonMale_Updt.Checked = true;
                        radioButtonFemale_Updt.Checked = false;
                    }
                    else
                    {
                        radioButtonFemale_Updt.Checked = true;
                        radioButtonMale_Updt.Checked = false;
                    }
                    Tx_Phone_Updt.Text = user1.Phone;
                    Tx_City_Updt.Text = user1.City;
                    Tx_ZipCode_Updt.Text = user1.Zip;
                    Tx_Address_Updt.Text = user1.Address;
                    Tx_Email_Updt.Text = user1.Email;
                    var img = user1.Picture;
                    var ms = new MemoryStream(img);
                    LogUserImage.Image = Image.FromStream(ms);
                    Tx_Emer_Contact_Updt.Text = user1.Emegency_Contact;
                    Combo_MS_Updt.Text = user1.Mari_Status;
                    Combo_Region_Updt.Text = user1.Region;
                    Combo_Title_Updt.Text = user1.Title;
                    Tx_User_Name.Text = user1.Fullname;
                    Tx_T_IDHide.Text = Convert.ToString(user1.MT_ID);
                    Tx_Job_Title_Updt.Text = user1.JobTitile;
                    Tx_Company_Name_Updt.Text = user1.Company_Name;
                    Combo_Fam_Role_Updt.Text = user1.Fam_Role;
                    Tx_Empl_ID_Updt.Text = user1.TKC_MemID;
                    Combo_Dept_Updt.Text = user1.Dept;
                    ComBo_OffiCapacity_Updt.Text = user1.Official_Capa;
                    Combo_Status_Updt.Text = user1.Member_Status;
                    Combo_Is_Baptised_Updt.Text = user1.Is_Baptised;
                    DateTime_BapDT_Updt.Value = user1.BaptismDT.Value.Date;
                    Combo_Family_Name_Updt.Text = user1.Fam_Name;
                    Combo_Mem_Cell_Name_Updt.Text = user1.Cell_Name;
                    Combo_Service_Type_Updt.Text = user1.Service_Name;
                }
            }
        }
        private void Load_Fam_DataGriD_View()
        {
            var FamRow = DBS.FamTBs.FirstOrDefault(a => a.Fam_ID.Equals(UserSession2.FamID));
            if (!String.IsNullOrEmpty(UserSession2.FamID))
            {
                if (FamRow != null)
                {
                    LogUserImage.Image = null;
                    Tx_Fam_Address.Text = FamRow.Address;
                    Tx_F_City.Text = FamRow.City;
                    Tx_Fam_Email.Text = FamRow.Email;
                    Tx_Fam_Greetings.Text = FamRow.Fam_Greetings;
                    Tx_Fam_Name.Text = FamRow.Fam_Name;
                    Tx_Fam_Phone.Text = FamRow.Home_Phone;
                    Tx_Husband_Name.Text = FamRow.Husb_Name;
                    Tx_Child1.Text = FamRow.Name_Of_Child1;
                    Tx_Child2.Text = FamRow.Name_Of_Child2;
                    Tx_Child3.Text = FamRow.Name_Of_Child3;
                    Tx_Child4.Text = FamRow.Name_Of_Child4;
                    Tx_Child5.Text = FamRow.Name_Of_Child5;
                    Tx_Child6.Text = FamRow.Name_Of_Child6;
                    Tx_Fam_ID.Text = FamRow.Fam_ID;
                    Tx_User_Name.Text = FamRow.Fam_Name;
                    Combo_NoOf_Chlld.Text = Convert.ToString(FamRow.No_Of_Child);
                    Tx_Wife_Name.Text = FamRow.Wife_Name;
                    Combo_Region.Text = FamRow.Region;
                    var img = FamRow.Fam_Pic;
                    var ms = new MemoryStream(img);
                    LogUserImage.Image = Image.FromStream(ms);
                    LogUserImage3.Image = Image.FromStream(ms);
                }
                else { return; }
            }
            else
            {
                return;
            }

        }
        public void Load_From_Family()
        {
            DataGrib_Family.DataSource = this.PopulateDataGridView_Fam();
        }
        #endregion
        #region CELL CONTENT CLICK
        private void DataGrib_MEMBER_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtUsername_Hide.Text = DataGrib_MEMBER.SelectedRows[0].Cells[0].Value.ToString();
            UserSession.MemberID = txtUsername_Hide.Text;
            UpdateMember();
        }
        private void DataGrib_Family_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_Hide_Fam_ID.Text = DataGrib_Family.SelectedRows[0].Cells[1].Value.ToString();
            UserSession2.FamID = Tx_Hide_Fam_ID.Text;
            Load_Fam_DataGriD_View();
 
        }
        #endregion
        #region MAIN FORM LOAD
        private void Member_CRM_MAIN_Load(object sender, EventArgs e)
        {
            LOAD_MEMBER_DATA();
            Load_From_Family();
            Tx_Error_Fill.Visible = false;
        }
        #endregion
        #region UPLOAD IMAGE
        private void BU_Mem_IMG_Select_upt_Click(object sender, EventArgs e)
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
        private void BT_UpdatePhoto_Update_Click(object sender, EventArgs e)
        {
            try
            {

                if (LogUserImage.Image != null)
                {
                    id = Convert.ToDecimal(Tx_T_IDHide.Text);
                    var cat = (from s in DBS.MembTBs where s.MT_ID == id select s).FirstOrDefault();
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
                        LOAD_MEMBER_DATA();
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
        private void But_UploadImg_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openbdlg = new OpenFileDialog();
                if (openbdlg.ShowDialog() == DialogResult.OK)
                {
                    Image img = Image.FromFile(openbdlg.FileName);
                    LogUserImage2.Image = img;
                    ms = new MemoryStream();
                    img.Save(ms, img.RawFormat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid File...");
            }
        }
        private void But_UploadImgFam_Click(object sender, EventArgs e)
        {
            OpenFileDialog openbdlg = new OpenFileDialog();
            if (openbdlg.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openbdlg.FileName);
                LogUserImage3.Image = img;
                ms = new MemoryStream();
                img.Save(ms, img.RawFormat);
            }
        }
        #endregion
        #region ADD MEMBER & FAMILY
        private void BT_AddNew_Member_Click(object sender, EventArgs e)
        {
            MembTB addMem = new MembTB();
            if (radioButtonMale.Checked)
            {
                gender = "Male";
            }
            else if (radioButtonFemale.Checked)
            {
                gender = "Female";
            }
            try
            {
                if (LogUserImage.Image != null)
                {
                    if (Tx_FName.Text != "" && Tx_SName.Text != "" && Tx_City.Text != "" && Tx_Address.Text != "" &&
                        Tx_Phone.Text != "" && Tx_Email.Text != "" && Tx_ZipCode.Text != "" && Combo_MS.SelectedIndex != 0 &&
                        Tx_Emer_Contact.Text != "" && Combo_Dept.SelectedIndex != 0 && Combo_Mem_Cell_Name.SelectedIndex != 0 &&
                        Combo_Is_Baptised.SelectedIndex != 0 && Combo_Service_Type.SelectedIndex != 0)
                    {

                        addMem.Title = Combo_Title.Text;
                        addMem.Fname = Tx_FName.Text;
                        addMem.Lname = Tx_LName.Text;
                        addMem.Sname = Tx_SName.Text;
                        addMem.DOB = DateTime_DOB.Value.Date;
                        addMem.Gender = gender;
                        addMem.DOB = DateTime_DOB.Value.Date;
                        addMem.JoinDT = DateTime.Now;
                        addMem.City = Tx_City.Text;
                        addMem.Address = Tx_Address.Text;
                        addMem.Phone = Tx_Phone.Text;
                        addMem.Region = Combo_Region.Text;
                        addMem.Email = Tx_Email.Text;
                        addMem.Zip = Tx_ZipCode.Text;
                        addMem.Mari_Status = Combo_MS.Text;
                        addMem.Emegency_Contact = Tx_Emer_Contact.Text;
                        addMem.Fam_Role = Combo_Fam_Role.Text;
                        addMem.Fam_Name = Combo_Family_Name.Text;
                        addMem.Fam_Role = Combo_Fam_Role.Text;
                        addMem.JobTitile = Tx_Job_Title.Text;
                        addMem.Company_Name = Tx_Company_Name.Text;
                        addMem.TKC_MemID = Tx_Mem_ID.Text;
                        addMem.Dept = Combo_Dept.Text;
                        addMem.Cell_Name = Combo_Mem_Cell_Name.Text;
                        addMem.Is_Baptised = Combo_Is_Baptised.Text;
                        addMem.BaptismDT = DateTime_BapDT.Value.Date;
                        addMem.Service_Name = Combo_Service_Type.Text;
                        addMem.CreatedBY = TKHCI_Main_Login.MainLogin.Username;
                        addMem.Picture = ms.ToArray();
                        addMem.Official_Capa = ComBo_OffiCapacity.Text;
                        var fullname = new StringBuilder();
                        fullname.Append(Tx_FName.Text);
                        fullname.Append(" ");
                        fullname.Append(Tx_SName.Text);
                        fullname.Append(" ");
                        fullname.Append(Tx_LName.Text);
                        addMem.Fullname = fullname.ToString();

                        string message = "Are you sure you want to add this record?";
                        string title = "Adding Record...";
                        MessageBoxButtons but = MessageBoxButtons.YesNo;
                        DialogResult result = MessageBox.Show(message, title, but);
                        if (result == DialogResult.Yes)
                        {
                            DBS.MembTBs.Add(addMem);
                            DBS.SaveChanges();
                            MessageBox.Show("User Created Successfully");
                            BT_ClearForm_Click(null, null);
                            LOAD_MEMBER_DATA();

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

                            Tx_FName.BorderColor = (Tx_FName.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_LName.BorderColor = (Tx_LName.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_City.BorderColor = (Tx_City.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_Address.BorderColor = (Tx_Address.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_Phone.BorderColor = (Tx_Phone.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_Email.BorderColor = (Tx_Email.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_ZipCode.BorderColor = (Tx_ZipCode.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Combo_MS.BorderColor = (Combo_MS.SelectedIndex <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_Emer_Contact.BorderColor = (Tx_Emer_Contact.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Combo_Dept.BorderColor = (Combo_Dept.SelectedIndex <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Combo_Mem_Cell_Name.BorderColor = (Combo_Mem_Cell_Name.SelectedIndex <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Combo_Is_Baptised.BorderColor = (Combo_Is_Baptised.SelectedIndex <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Combo_Service_Type.BorderColor = (Combo_Service_Type.SelectedIndex <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Combo_Status.BorderColor = (Combo_Status.SelectedIndex <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_Error_Fill.Visible = true;
                            Tx_Error_Fill.Visible = true;
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
        private void BT_Add_Fam_Click(object sender, EventArgs e)
        {
            FamTB fam = new FamTB();
            try
            {
                string UpdateFam = Tx_Hide_Fam_ID.Text;
                var check = (from s in DBS.FamTBs where s.Fam_ID == UpdateFam select s).FirstOrDefault();

                if (check == null)
                {
                    if (Tx_Fam_ID.Text != "" && Tx_Fam_Address.Text != "" && Tx_Fam_Name.Text != "" && Tx_Husband_Name.Text != "" && Tx_Wife_Name.Text != "" && LogUserImage3.Image != null)
                    {
                        fam.Address = Tx_Fam_Address.Text;
                        fam.City = Tx_F_City.Text;
                        fam.CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                        fam.CreateDT = DateTime.Now;
                        fam.Email = Tx_Fam_Email.Text;
                        fam.Fam_Greetings = Tx_Fam_Greetings.Text;
                        fam.Fam_ID = Tx_Fam_ID.Text;
                        fam.Fam_Name = Tx_Fam_Name.Text;
                        fam.Home_Phone = Tx_Fam_Phone.Text;
                        fam.Husb_Name = Tx_Husband_Name.Text;
                        fam.Name_Of_Child1 = Tx_Child1.Text;
                        fam.Name_Of_Child2 = Tx_Child2.Text;
                        fam.Name_Of_Child3 = Tx_Child3.Text;
                        fam.Name_Of_Child4 = Tx_Child4.Text;
                        fam.Name_Of_Child5 = Tx_Child5.Text;
                        fam.Name_Of_Child6 = Tx_Child6.Text;
                        fam.No_Of_Child = Convert.ToDecimal(Combo_NoOf_Chlld.Text);
                        fam.Wife_Name = Tx_Wife_Name.Text;
                        fam.Region = ComboBox_Region_Fam.Text;
                        fam.Fam_Pic = ms.ToArray();

                        string message = "Are you sure you want to add this record?";
                        string title = "Adding Record...";
                        MessageBoxButtons but = MessageBoxButtons.YesNo;
                        DialogResult result = MessageBox.Show(message, title, but);
                        if (result == DialogResult.Yes)
                        {
                            DBS.FamTBs.Add(fam);
                            DBS.SaveChanges();
                            MessageBox.Show("User Created Successfully");
                            BT_Clear_Form_Click(null, null);
                            Load_From_Family();


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
                    MessageBox.Show(this,"Account " + UpdateFam + " Already Exist" + " ," + " Kindly clear the form to add a new Family","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message,"",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region UPDATE MEMBER & FAMILY
        private void But_Update_Member_Click(object sender, EventArgs e)
        {

            if (radioButtonMale_Updt.Checked)
            {
                gender = "Male";
            }
            else if (radioButtonFemale_Updt.Checked)
            {
                gender = "Female";
            }
            if (LogUserImage.Image != null)
            {
                if (Tx_FName_Updt.Text != null)
                {
                    id = Convert.ToDecimal(Tx_T_IDHide.Text);
                    var addMem = (from s in DBS.MembTBs where s.MT_ID == id select s).FirstOrDefault();
                    addMem.Title = Combo_Title_Updt.Text;
                    addMem.Fname = Tx_FName_Updt.Text;
                    addMem.Lname = Tx_LName_Updt.Text;
                    addMem.Sname = Tx_SName_Updt.Text;
                    addMem.DOB = Dt_DOB_Updt.Value.Date;
                    addMem.Gender = gender;
                    addMem.City = Tx_City_Updt.Text;
                    addMem.Address = Tx_Address_Updt.Text;
                    addMem.Phone = Tx_Phone_Updt.Text;
                    addMem.Region = Combo_Region_Updt.Text;
                    addMem.Email = Tx_Email_Updt.Text;
                    addMem.Zip = Tx_ZipCode_Updt.Text;
                    addMem.Mari_Status = Combo_MS_Updt.Text;
                    addMem.Emegency_Contact = Tx_Emer_Contact_Updt.Text;
                    addMem.Fam_Role = Combo_Fam_Role_Updt.Text;
                    addMem.Fam_Name = Combo_Family_Name_Updt.Text;
                    addMem.Fam_Role = Combo_Fam_Role_Updt.Text;
                    addMem.JobTitile = Tx_Job_Title_Updt.Text;
                    addMem.Company_Name = Tx_Company_Name_Updt.Text;
                    addMem.TKC_MemID = Tx_Empl_ID_Updt.Text;
                    addMem.Dept = Combo_Dept_Updt.Text;
                    addMem.Cell_Name = Combo_Mem_Cell_Name_Updt.Text;
                    addMem.Is_Baptised = Combo_Is_Baptised_Updt.Text;
                    addMem.BaptismDT = DateTime_BapDT_Updt.Value.Date;
                    addMem.Service_Name = Combo_Service_Type_Updt.Text;
                    addMem.Member_Status = Combo_Status_Updt.Text;
                    addMem.CreatedBY = TKHCI_Main_Login.MainLogin.Username;

                    var img = LogUserImage.Image;
                    if (ms != null)
                    {
                        img.Save(ms, img.RawFormat);
                        addMem.Picture = ms.ToArray();
                    }
                    addMem.Official_Capa = ComBo_OffiCapacity_Updt.Text;
                    var fullname = new StringBuilder();
                    fullname.Append(Tx_FName_Updt.Text);
                    fullname.Append(" ");
                    fullname.Append(Tx_SName_Updt.Text);
                    fullname.Append(" ");
                    fullname.Append(Tx_LName_Updt.Text);
                    addMem.Fullname = fullname.ToString();

                    string message = "Are you sure you want to update this record?";
                    string title = "updating Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {

                        DBS.SaveChanges();
                        MessageBox.Show("Member Record Updated Successfully");
                        LOAD_MEMBER_DATA();
                        clearMember();

                    }
                    else return;
                }
            }
        }
        #endregion
        #region DELETE MEMBER & FAMILY
        private void Bt_Delete_Member_Click(object sender, EventArgs e)
        {
            MembTB acc = new MembTB();
            if (Tx_T_IDHide.Text == "")
            {
                return;
            }
            else
            {
                decimal ID = Convert.ToDecimal(Tx_T_IDHide.Text);
                if (!ID.Equals(null))
                {
                    var cat = (from s in DBS.MembTBs where s.MT_ID == ID select s).First();
                    string message = "Are you sure you want to delete this Product?";
                    string title = "Deleting Product...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.MembTBs.Remove(cat);
                        DBS.SaveChanges();
                        MessageBox.Show("Product Deleted");
                        LOAD_MEMBER_DATA();
                        clearMember();

                    }
                    else return;
                }
                else MessageBox.Show("Select User to delete");
            }
        }

        #endregion
        #region CLEAR MEMBER & FAMILY
        public void clearMember()
        {
            Combo_Title_Updt.SelectedIndex = 0;
            Tx_FName_Updt.Text = "";
            Tx_SName_Updt.Text = "";
            Tx_LName_Updt.Text = "";
            radioButtonFemale_Updt.Checked = true;
            Tx_City_Updt.Text = "";
            Tx_Phone_Updt.Text = "";
            Tx_Address_Updt.Text = "";
            Combo_Region_Updt.SelectedIndex = 0;
            Tx_Email_Updt.Text = "";
            Tx_ZipCode_Updt.Text = "";
            Combo_MS_Updt.SelectedIndex = 0;
            Tx_Emer_Contact_Updt.Text = "";
            Combo_Fam_Role_Updt.SelectedIndex = 0;
            Combo_Family_Name_Updt.SelectedIndex = 0;
            Tx_Job_Title_Updt.Text = "";
            Tx_Company_Name_Updt.Text = "";
            Tx_Empl_ID_Updt.Text = "";
            Combo_Dept_Updt.SelectedIndex = 0;
            Combo_Mem_Cell_Name_Updt.SelectedIndex = 0;
            ComBo_OffiCapacity_Updt.SelectedIndex = 0;
            Combo_Status_Updt.SelectedIndex = 0;
            Combo_Is_Baptised_Updt.SelectedIndex = 0;
            Combo_Service_Type_Updt.SelectedIndex = 0;
        }
        private void BT_ClearForm_Click(object sender, EventArgs e)
        {
            Keepers_Member_LoadID();
            Combo_Title.Text = "";
            Tx_LName.Text = "";
            Tx_FName.Text = "";
            Tx_SName.Text = "";
            Tx_SName.Text = "";
            Tx_City.Text = "";
            Tx_Address.Text = "";
            Tx_Phone.Text = "";
            Combo_Region.SelectedIndex = 0;
            Tx_Email.Text = "";
            Tx_ZipCode.Text = "";
            Combo_MS.SelectedIndex = 0;
            Tx_Emer_Contact.Text = "";
            Combo_Fam_Role.SelectedIndex = 0;
            Combo_Family_Name.SelectedIndex = 0;
            Combo_Fam_Role.SelectedIndex = 0;
            Tx_Job_Title.Text = "";
            Tx_Company_Name.Text = "";
            Combo_Dept.SelectedIndex = 0;
            Combo_Mem_Cell_Name.SelectedIndex = 0;
            Combo_Is_Baptised.SelectedIndex = 0;
            Combo_Service_Type.SelectedIndex = 0;
            ComBo_OffiCapacity.SelectedIndex = 0;
            Combo_Title.SelectedIndex = 0;
            Tx_Error_Fill.Visible = false;
            Tx_Error_Fill.Visible = false;
            LogUserImage2.Image = null;

        }
        private void BT_Clear_Form_Click(object sender, EventArgs e)
        {
            Keepers_FamID_Load();
            LogUserImage.Image = null;
            Tx_Fam_Address.Text = "";
            Tx_F_City.Text = "";
            Tx_Fam_Email.Text = "";
            Tx_Fam_Greetings.Text = "";
            Tx_Fam_Name.Text = "";
            Tx_Fam_Phone.Text = "";
            Tx_Husband_Name.Text = "";
            Tx_Child1.Text = "";
            Tx_Child2.Text = "";
            Tx_Child3.Text = "";
            Tx_Child4.Text = "";
            Tx_Child5.Text = "";
            Tx_Child6.Text = "";
            Combo_NoOf_Chlld.SelectedIndex = 0;
            Tx_Wife_Name.Text = "";
            Combo_Region.SelectedIndex = 0;
            LogUserImage3.Image = null;
            Tx_Hide_Fam_ID.Text = "";
        }

        #endregion
        private void BT_Del_Fam_Click(object sender, EventArgs e)
        {
            FamTB acc = new FamTB();
            if (Tx_Hide_Fam_ID.Text == "")
            {
                MessageBox.Show("Select Family to delete");
            }
            else
            {
                //string Del = Tx_Hide_Fam_ID.Text;
                var famDel = (from s in DBS.FamTBs where s.Fam_ID.Equals(Tx_Hide_Fam_ID.Text) select s).First();

                string message = "Are you sure you want to delete this Product?";
                string title = "Deleting Product...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.FamTBs.Remove(famDel);
                    DBS.SaveChanges();
                    MessageBox.Show("Product Deleted");
                    Load_From_Family();
                    BT_Clear_Form_Click(null,null);
                }
                else return;

            }
        }
        private void BT_Update_Fam_Click(object sender, EventArgs e)
        {

            string UpdateFam = Tx_Hide_Fam_ID.Text;
            var updt = (from s in DBS.FamTBs where s.Fam_ID == UpdateFam select s).FirstOrDefault();

            if (Tx_Hide_Fam_ID.Text != null && Tx_Fam_Address.Text != "")
            {
                updt.Address = Tx_Fam_Address.Text;
                updt.City = Tx_F_City.Text;
                updt.Email = Tx_Fam_Email.Text;
                updt.Fam_Greetings = Tx_Fam_Greetings.Text;
                updt.Fam_ID = Tx_Fam_ID.Text;
                updt.Fam_Name = Tx_Fam_Name.Text;
                updt.Home_Phone = Tx_Fam_Phone.Text;
                updt.Husb_Name = Tx_Husband_Name.Text;
                updt.Name_Of_Child1 = Tx_Child1.Text;
                updt.Name_Of_Child2 = Tx_Child2.Text;
                updt.Name_Of_Child3 = Tx_Child3.Text;
                updt.Name_Of_Child4 = Tx_Child4.Text;
                updt.Name_Of_Child5 = Tx_Child5.Text;
                updt.Name_Of_Child6 = Tx_Child6.Text;
                updt.No_Of_Child = Convert.ToDecimal(Combo_NoOf_Chlld.Text);
                updt.Wife_Name = Tx_Wife_Name.Text;
                updt.Region = ComboBox_Region_Fam.Text;
                var img = LogUserImage3.Image;
                if (ms != null)
                {
                    img.Save(ms, img.RawFormat);
                    updt.Fam_Pic = ms.ToArray();
                }

                string message = "Are you sure you want to update this record?";
                string title = "updating Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {

                    DBS.SaveChanges();
                    MessageBox.Show("Family Record Updated Successfully");
                    BT_Clear_Form_Click(null, null);
                    Load_From_Family();
                }
                else return;
            }
            else
            {
                MessageBox.Show("Select a family to update");
            }
        }
    }
}
