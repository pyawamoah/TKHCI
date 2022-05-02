using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TKHCI.Admin_Module.Department
{
    public partial class DeptModule : Form
    {
        private readonly string load_DeptName = "Select 'Select Department Name' as DeptName union all SELECT distinct DepartmentTB.Dept_Name DeptName from DepartmentTB with (nolock);";

        private readonly string Load_ManName = " Select 'Select Manager Name' as ManName union all SELECT distinct ManagersTB.Manager_Name cellName from ManagersTB WITH (NOLOCK)";
        KeeperDBEntities DBS = new KeeperDBEntities();
        decimal ID;
        public static class UserSession
        {
            public static string Dept_ID;
        }
        public static class UserSession2
        {
            public static string Man_ID;
        }
        public DeptModule()
        {
            InitializeComponent();
            Keepers_Cell_LoadID();
            Keepers_Manager_LoadID();
            Load_Combo_Box();
        }
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        private Task Load_Combo_Box()
        {
            DataTable manName = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(Load_ManName, Con);
            da1.Fill(manName);
            Combo_Man_Name.DataSource = manName;
            Combo_Man_Name.DisplayMember = "ManName";
            Combo_Man_Name.ValueMember = "ManName";
            Con.Close();

            return Task.CompletedTask;
        }

        #region Department Block
        public void Off()
        {
            Tx_Unit1.Visible = false;
            Tx_Unit2.Visible = false;
            Tx_Unit3.Visible = false;
            Tx_Unit4.Visible = false;
            Tx_Unit5.Visible = false;
            Tx_Unit6.Visible = false;
            Tx_Unit7.Visible = false;
            Tx_Unit8.Visible = false;
            Tx_Unit9.Visible = false;
            Tx_Unit10.Visible = false;
            Tx_Unit11.Visible = false;



            Pn_1.Visible = false;
            Pn_2.Visible = false;
            Pn_3.Visible = false;
            Pn_4.Visible = false;
            Pn_5.Visible = false;
            Pn_6.Visible = false;
            Pn_7.Visible = false;
            Pn_8.Visible = false;
            Pn_9.Visible = false;
            Pn_10.Visible = false;
            Pn_11.Visible = false;
        }
        private void Combo_No_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Combo_Unit_No.SelectedIndex == 0)
            {
                Off();
            }
            else if (Combo_Unit_No.SelectedIndex == 1)
            {
                Tx_Unit1.Visible = true;
                Pn_1.Visible = true;

                Tx_Unit2.Visible = false;
                Tx_Unit3.Visible = false;
                Tx_Unit4.Visible = false;
                Tx_Unit5.Visible = false;
                Tx_Unit6.Visible = false;
                Tx_Unit7.Visible = false;
                Tx_Unit8.Visible = false;
                Tx_Unit9.Visible = false;
                Tx_Unit10.Visible = false;
                Tx_Unit11.Visible = false;


                Pn_2.Visible = false;
                Pn_3.Visible = false;
                Pn_4.Visible = false;
                Pn_5.Visible = false;
                Pn_6.Visible = false;
                Pn_7.Visible = false;
                Pn_8.Visible = false;
                Pn_9.Visible = false;
                Pn_10.Visible = false;
                Pn_11.Visible = false;

            }
            else if (Combo_Unit_No.SelectedIndex == 2)
            {
                Tx_Unit1.Visible = true;
                Tx_Unit2.Visible = true;
                Pn_1.Visible = true;
                Pn_2.Visible = true;

                Tx_Unit3.Visible = false;
                Tx_Unit4.Visible = false;
                Tx_Unit5.Visible = false;
                Tx_Unit6.Visible = false;
                Tx_Unit7.Visible = false;
                Tx_Unit8.Visible = false;
                Tx_Unit9.Visible = false;
                Tx_Unit10.Visible = false;
                Tx_Unit11.Visible = false;

                Pn_3.Visible = false;
                Pn_4.Visible = false;
                Pn_5.Visible = false;
                Pn_6.Visible = false;
                Pn_7.Visible = false;
                Pn_8.Visible = false;
                Pn_9.Visible = false;
                Pn_10.Visible = false;
                Pn_11.Visible = false;

            }
            else if (Combo_Unit_No.SelectedIndex == 3)
            {
                Tx_Unit1.Visible = true;
                Tx_Unit2.Visible = true;
                Tx_Unit3.Visible = true;

                Pn_1.Visible = true;
                Pn_2.Visible = true;
                Pn_3.Visible = true;


                Tx_Unit4.Visible = false;
                Tx_Unit5.Visible = false;
                Tx_Unit6.Visible = false;
                Tx_Unit7.Visible = false;
                Tx_Unit8.Visible = false;
                Tx_Unit9.Visible = false;
                Tx_Unit10.Visible = false;
                Tx_Unit11.Visible = false;



                Pn_4.Visible = false;
                Pn_5.Visible = false;
                Pn_6.Visible = false;
                Pn_7.Visible = false;
                Pn_8.Visible = false;
                Pn_9.Visible = false;
                Pn_10.Visible = false;
                Pn_11.Visible = false;
            }
            else if (Combo_Unit_No.SelectedIndex == 4)
            {
                Tx_Unit1.Visible = true;
                Tx_Unit2.Visible = true;
                Tx_Unit3.Visible = true;
                Tx_Unit4.Visible = true;
                Pn_1.Visible = true;
                Pn_2.Visible = true;
                Pn_3.Visible = true;
                Pn_4.Visible = true;


                Tx_Unit5.Visible = false;
                Tx_Unit6.Visible = false;
                Tx_Unit7.Visible = false;
                Tx_Unit8.Visible = false;
                Tx_Unit9.Visible = false;
                Tx_Unit10.Visible = false;
                Tx_Unit11.Visible = false;



                Pn_5.Visible = false;
                Pn_6.Visible = false;
                Pn_7.Visible = false;
                Pn_8.Visible = false;
                Pn_9.Visible = false;
                Pn_10.Visible = false;
                Pn_11.Visible = false;
            }
            else if (Combo_Unit_No.SelectedIndex == 5)
            {

                Tx_Unit1.Visible = true;
                Tx_Unit2.Visible = true;
                Tx_Unit3.Visible = true;
                Tx_Unit4.Visible = true;
                Tx_Unit5.Visible = true;

                Pn_1.Visible = true;
                Pn_2.Visible = true;
                Pn_3.Visible = true;
                Pn_4.Visible = true;
                Pn_5.Visible = true;



                Tx_Unit6.Visible = false;
                Tx_Unit7.Visible = false;
                Tx_Unit8.Visible = false;
                Tx_Unit9.Visible = false;
                Tx_Unit10.Visible = false;
                Tx_Unit11.Visible = false;



                Pn_6.Visible = false;
                Pn_7.Visible = false;
                Pn_8.Visible = false;
                Pn_9.Visible = false;
                Pn_10.Visible = false;
                Pn_11.Visible = false;
            }
            else if (Combo_Unit_No.SelectedIndex == 6)
            {

                Tx_Unit1.Visible = true;
                Tx_Unit2.Visible = true;
                Tx_Unit3.Visible = true;
                Tx_Unit4.Visible = true;
                Tx_Unit5.Visible = true;
                Tx_Unit6.Visible = true;

                Pn_1.Visible = true;
                Pn_2.Visible = true;
                Pn_3.Visible = true;
                Pn_4.Visible = true;
                Pn_5.Visible = true;
                Pn_6.Visible = true;


                Tx_Unit7.Visible = false;
                Tx_Unit8.Visible = false;
                Tx_Unit9.Visible = false;
                Tx_Unit10.Visible = false;
                Tx_Unit11.Visible = false;


                Pn_7.Visible = false;
                Pn_8.Visible = false;
                Pn_9.Visible = false;
                Pn_10.Visible = false;
                Pn_11.Visible = false;
            }
            else if (Combo_Unit_No.SelectedIndex == 7)
            {

                Tx_Unit1.Visible = true;
                Tx_Unit2.Visible = true;
                Tx_Unit3.Visible = true;
                Tx_Unit4.Visible = true;
                Tx_Unit5.Visible = true;
                Tx_Unit6.Visible = true;
                Tx_Unit7.Visible = true;

                Pn_1.Visible = true;
                Pn_2.Visible = true;
                Pn_3.Visible = true;
                Pn_4.Visible = true;
                Pn_5.Visible = true;
                Pn_6.Visible = true;
                Pn_7.Visible = true;


                Tx_Unit8.Visible = false;
                Tx_Unit9.Visible = false;
                Tx_Unit10.Visible = false;
                Tx_Unit11.Visible = false;



                Pn_8.Visible = false;
                Pn_9.Visible = false;
                Pn_10.Visible = false;
                Pn_11.Visible = false;
            }


            else if (Combo_Unit_No.SelectedIndex == 8)
            {

                Tx_Unit1.Visible = true;
                Tx_Unit2.Visible = true;
                Tx_Unit3.Visible = true;
                Tx_Unit4.Visible = true;
                Tx_Unit5.Visible = true;
                Tx_Unit6.Visible = true;
                Tx_Unit7.Visible = true;
                Tx_Unit8.Visible = true;

                Pn_1.Visible = true;
                Pn_2.Visible = true;
                Pn_3.Visible = true;
                Pn_4.Visible = true;
                Pn_5.Visible = true;
                Pn_6.Visible = true;
                Pn_7.Visible = true;
                Pn_8.Visible = true;


                Tx_Unit9.Visible = false;
                Tx_Unit10.Visible = false;
                Tx_Unit11.Visible = false;



                Pn_9.Visible = false;
                Pn_10.Visible = false;
                Pn_11.Visible = false;
            }
            else if (Combo_Unit_No.SelectedIndex == 9)
            {

                Tx_Unit1.Visible = true;
                Tx_Unit2.Visible = true;
                Tx_Unit3.Visible = true;
                Tx_Unit4.Visible = true;
                Tx_Unit5.Visible = true;
                Tx_Unit6.Visible = true;
                Tx_Unit7.Visible = true;
                Tx_Unit8.Visible = true;
                Tx_Unit9.Visible = true;

                Pn_1.Visible = true;
                Pn_2.Visible = true;
                Pn_3.Visible = true;
                Pn_4.Visible = true;
                Pn_5.Visible = true;
                Pn_6.Visible = true;
                Pn_7.Visible = true;
                Pn_8.Visible = true;
                Pn_9.Visible = true;

                Tx_Unit10.Visible = false;
                Tx_Unit11.Visible = false;

                Pn_10.Visible = false;
                Pn_11.Visible = false;
            }
            else if (Combo_Unit_No.SelectedIndex == 10)
            {
                Tx_Unit1.Visible = true;
                Tx_Unit2.Visible = true;
                Tx_Unit3.Visible = true;
                Tx_Unit4.Visible = true;
                Tx_Unit5.Visible = true;
                Tx_Unit6.Visible = true;
                Tx_Unit7.Visible = true;
                Tx_Unit8.Visible = true;
                Tx_Unit9.Visible = true;
                Tx_Unit10.Visible = true;

                Pn_1.Visible = true;
                Pn_2.Visible = true;
                Pn_3.Visible = true;
                Pn_4.Visible = true;
                Pn_5.Visible = true;
                Pn_6.Visible = true;
                Pn_7.Visible = true;
                Pn_8.Visible = true;
                Pn_9.Visible = true;
                Pn_10.Visible = true;


                Tx_Unit11.Visible = false;
                Pn_11.Visible = false;
            }
            else if (Combo_Unit_No.SelectedIndex == 11)
            {
                Tx_Unit1.Visible = true;
                Tx_Unit2.Visible = true;
                Tx_Unit3.Visible = true;
                Tx_Unit4.Visible = true;
                Tx_Unit5.Visible = true;
                Tx_Unit6.Visible = true;
                Tx_Unit7.Visible = true;
                Tx_Unit8.Visible = true;
                Tx_Unit9.Visible = true;
                Tx_Unit10.Visible = true;
                Tx_Unit11.Visible = true;

                Pn_1.Visible = true;
                Pn_2.Visible = true;
                Pn_3.Visible = true;
                Pn_4.Visible = true;
                Pn_5.Visible = true;
                Pn_6.Visible = true;
                Pn_7.Visible = true;
                Pn_8.Visible = true;
                Pn_9.Visible = true;
                Pn_10.Visible = true;
                Pn_11.Visible = true;


            }
            else
            {
                Off();

            }
        }

        private void DeptModule_Load(object sender, EventArgs e)
        {
            Off();
            Load_From_Table_New_Data();
            Load_From_Table_New_Data2();
        }
        private void Keepers_Cell_LoadID()
        {
            string constr = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_Dept_LoadID", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            sqlConnection.Close();


            if (xml < 1)
            {
                ID = 1;
                Tx_Dept_No.Text = $"KHDept-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else if (xml >= 1)
            {
                ID = xml + 1;
                Tx_Dept_No.Text = $"KHDept-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else return;

        }
        public bool IsFormOpen(Type formType)
        {
            foreach (Form form in Application.OpenForms)
                if (form.GetType().Name == form.Name)
                    return true;
            return false;
        }
        public object PopulateDataGridView()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_Load_and_Search_Dept", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_By_Name.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public void Load_From_Table_New_Data()
        {
            DataGridView_Dept.DataSource = this.PopulateDataGridView();
        }

        private void BT_Add_Dept_Click(object sender, EventArgs e)
        {
            DepartmentTB dedpt = new DepartmentTB();

            if (Combo_Unit_No.SelectedIndex != -1 || Combo_Unit_No.SelectedText != "No.of Units in the Dept")
            {
                dedpt.CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                dedpt.CreateDT = DateTime.Now;
                dedpt.Dept_ID = Tx_Dept_No.Text;
                dedpt.Dept_Name = Tx_Dept_Name.Text;
                dedpt.Dept_ManagerName = Combo_Man_Name.Text;
                dedpt.No_of_Unit = Convert.ToDecimal(Combo_Unit_No.Text);
                dedpt.Dept_Units1 = Tx_Unit1.Text;
                dedpt.Dept_Units2 = Tx_Unit2.Text;
                dedpt.Dept_Units3 = Tx_Unit3.Text;
                dedpt.Dept_Units4 = Tx_Unit4.Text;
                dedpt.Dept_Units5 = Tx_Unit5.Text;
                dedpt.Dept_Units6 = Tx_Unit6.Text;
                dedpt.Dept_Units7 = Tx_Unit7.Text;
                dedpt.Dept_Units8 = Tx_Unit8.Text;
                dedpt.Dept_Units9 = Tx_Unit9.Text;
                dedpt.Dept_Units10 = Tx_Unit10.Text;
                dedpt.Dept_Units11 = Tx_Unit11.Text;
                
                string message = "Are you sure you want to add this record?";
                string title = "Adding Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.DepartmentTBs.Add(dedpt);
                    DBS.SaveChanges();
                    MessageBox.Show("User Created Successfully");
                    Load_From_Table_New_Data();
                    BT_Clear_Dept_Click(null, null);
                }
                else return;

            }
            else
            {
                if (Application.OpenForms.OfType<Error_Pack.Error_Select_Unit_No>().Count() == 1)
                {
                    return;
                }
                else
                {
                    Error_Pack.Error_Select_Unit_No err2 = new Error_Pack.Error_Select_Unit_No();
                    err2.ShowDialog();
                }
            }
        }
        private void Load_Fam_DataGriD_View()
        {
            var DeptRow = DBS.DepartmentTBs.FirstOrDefault(a => a.Dept_ID.Equals(UserSession.Dept_ID));
            if (!String.IsNullOrEmpty(UserSession.Dept_ID))
            {
                if (DeptRow != null)
                {
                    Tx_Dept_No.Text = DeptRow.Dept_ID;
                    Tx_Dept_Name.Text = DeptRow.Dept_Name;
                    Combo_Dept_Manager.Text = DeptRow.Dept_ManagerName;
                    Combo_Unit_No.Text = Convert.ToString(DeptRow.No_of_Unit);
                    Tx_Unit1.Text = DeptRow.Dept_Units1;
                    Tx_Unit2.Text = DeptRow.Dept_Units2;
                    Tx_Unit3.Text = DeptRow.Dept_Units3;
                    Tx_Unit4.Text = DeptRow.Dept_Units4;
                    Tx_Unit5.Text = DeptRow.Dept_Units5;
                    Tx_Unit6.Text = DeptRow.Dept_Units6;
                    Tx_Unit7.Text = DeptRow.Dept_Units7;
                    Tx_Unit8.Text = DeptRow.Dept_Units8;
                    Tx_Unit9.Text = DeptRow.Dept_Units9;
                    Tx_Unit10.Text = DeptRow.Dept_Units10;
                    Tx_Unit11.Text = DeptRow.Dept_Units11;
                    Combo_Man_Name.Text = DeptRow.Dept_ManagerName;
                }
                else { return; }
            }
            else
            {
                return;
            }

        }

        private void DataGridView_Dept_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_Hide_Fam_ID.Text = DataGridView_Dept.SelectedRows[0].Cells[0].Value.ToString();
            UserSession.Dept_ID = Tx_Hide_Fam_ID.Text;
            Load_Fam_DataGriD_View();
        }
        private void BT_Update_Dept_Click(object sender, EventArgs e)
        {
            {
                if (Tx_Hide_Fam_ID.Text != null)
                {
                    string id = Tx_Hide_Fam_ID.Text;
                    var updateDept = (from s in DBS.DepartmentTBs where s.Dept_ID == id select s).FirstOrDefault();
                    updateDept.Dept_Name = Tx_Dept_Name.Text;
                    updateDept.Dept_ManagerName = Combo_Man_Name.Text;
                    updateDept.No_of_Unit = Convert.ToDecimal(Combo_Unit_No.Text);
                    updateDept.Dept_Units1 = Tx_Unit1.Text;
                    updateDept.Dept_Units2 = Tx_Unit2.Text;
                    updateDept.Dept_Units3 = Tx_Unit3.Text;
                    updateDept.Dept_Units4 = Tx_Unit4.Text;
                    updateDept.Dept_Units5 = Tx_Unit5.Text;
                    updateDept.Dept_Units6 = Tx_Unit6.Text;
                    updateDept.Dept_Units7 = Tx_Unit7.Text;
                    updateDept.Dept_Units8 = Tx_Unit8.Text;
                    updateDept.Dept_Units9 = Tx_Unit9.Text;
                    updateDept.Dept_Units10 = Tx_Unit10.Text;
                    updateDept.Dept_Units11 = Tx_Unit11.Text;


                    string message = "Are you sure you want to update this record?";
                    string title = "updating Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {

                        DBS.SaveChanges();
                        MessageBox.Show("Member Record Updated Successfully");
                        DataGridView_Dept.DataSource = this.PopulateDataGridView();
                        BT_Clear_Dept_Click(null, null);
                    }
                    else return;
                }
            }
        }

        private void BT_Dell_Dept_Click(object sender, EventArgs e)
        {
            DepartmentTB acc = new DepartmentTB();
            if (Tx_Hide_Fam_ID.Text == "")
            {
                return;
            }
            else
            {
                string ID = Tx_Hide_Fam_ID.Text;
                if (!ID.Equals(null))
                {
                    var delDept = (from s in DBS.DepartmentTBs where s.Dept_ID == ID select s).First();

                    string message = "Are you sure you want to delete this Product?";
                    string title = "Deleting Product...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.DepartmentTBs.Remove(delDept);
                        DBS.SaveChanges();
                        MessageBox.Show("Product Deleted");
                        DataGridView_Dept.DataSource = this.PopulateDataGridView();

                    }
                    else return;
                }
                else MessageBox.Show("Select User to delete");
            }
        }
        private void BT_Clear_Dept_Click(object sender, EventArgs e)
        {
            Off();
            Keepers_Cell_LoadID();
            Tx_Dept_Name.Text = "";
            Combo_Man_Name.SelectedIndex = 0;
            Combo_Unit_No.SelectedIndex = 0;
            Tx_Unit1.Text = "";
            Tx_Unit2.Text = "";
            Tx_Unit3.Text = "";
            Tx_Unit4.Text = "";
            Tx_Unit5.Text = "";
            Tx_Unit6.Text = "";
            Tx_Unit7.Text = "";
            Tx_Unit8.Text = "";
            Tx_Unit9.Text = "";
            Tx_Unit10.Text = "";
            Tx_Unit11.Text = "";
        }
        private void Tx_Search_By_Name_TextChanged(object sender, EventArgs e)
        {
            DataGridView_Dept.DataSource = this.PopulateDataGridView();
        }

        #endregion Department Block

        #region Managers Block
        //Manager Block starts here....
        public object PopulateDataGridView2()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_Load_and_Search_Manager", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_Manager.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public void Load_From_Table_New_Data2()
        {
            DataGrid_Manager.DataSource = this.PopulateDataGridView2();
        }
        private void Keepers_Manager_LoadID()
        {
            string constr = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_Manager_LoadID", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            sqlConnection.Close();


            if (xml < 1)
            {
                ID = 1;
                Tx_Manager_No.Text = $"KHM-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else if (xml >= 1)
            {
                ID = xml + 1;
                Tx_Manager_No.Text = $"KHM-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else return;  
        }

        

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ManagersTB man = new ManagersTB();
            if(Tx_Manager_No.Text!="" && Tx_Manager_Name.Text != "")
            {
                man.CreateDT = DateTime.Now;
                man.CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                man.Manager_Name = Tx_Manager_Name.Text;
                man.Manager_No=Tx_Manager_No.Text;

                string message = "Are you sure you want to add this record?";
                string title = "Adding Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.ManagersTBs.Add(man);
                    DBS.SaveChanges();
                    MessageBox.Show("User Created Successfully");
                    BT_Clear_Click(null, null);
                    Load_From_Table_New_Data2();
                }
                else return;
            }
        }
        private void Load_Man_DataGriD_View()
        {
            var ManRow = DBS.ManagersTBs.FirstOrDefault(a => a.Manager_No.Equals(UserSession2.Man_ID));
            if (!String.IsNullOrEmpty(UserSession2.Man_ID))
            {
                if (ManRow != null)
                {
                    Tx_Manager_Name.Text = ManRow.Manager_Name;
                    Tx_Manager_No.Text = ManRow.Manager_No;

                }
                else { return; }
            }
            else
            {
                return;
            }

        }

        private void DataGrid_Manager_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_Hide.Text = DataGrid_Manager.SelectedRows[0].Cells[1].Value.ToString();
            UserSession2.Man_ID = Tx_Hide.Text;
            Load_Man_DataGriD_View();
        }
        private void BT_Upate_Click(object sender, EventArgs e)
        {
            
            {
                if (Tx_Dept_No.Text != null)
                {
                    string id = Tx_Hide.Text;
                    var updateMam = (from s in DBS.ManagersTBs where s.Manager_No == id select s).FirstOrDefault();
                    updateMam.Manager_Name = Tx_Manager_Name.Text;
                    updateMam.Manager_No = Tx_Manager_No.Text;



                    string message = "Are you sure you want to update this record?";
                    string title = "updating Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {

                        DBS.SaveChanges();
                        MessageBox.Show("Member Record Updated Successfully");
                        DataGrid_Manager.DataSource = this.PopulateDataGridView2();
                        BT_Clear_Click(null, null);
                    }
                    else return;
                }
            }
        }
        private void BT_DelateM_Click(object sender, EventArgs e)
        {
            DepartmentTB acc = new DepartmentTB();
            if (Tx_Hide.Text == "")
            {
                return;
            }
            else
            {
                string ID = Tx_Hide.Text;
                if (!ID.Equals(null))
                {
                    var delMan = (from s in DBS.ManagersTBs where s.Manager_No == ID select s).First();

                    string message = "Are you sure you want to delete this Product?";
                    string title = "Deleting Product...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.ManagersTBs.Remove(delMan);
                        DBS.SaveChanges();
                        MessageBox.Show("Product Deleted");
                        DataGrid_Manager.DataSource = this.PopulateDataGridView2();
                        BT_Clear_Click(null, null);

                    }
                    else return;
                }
                else MessageBox.Show("Select User to delete");
            }
        }
        private void BT_Clear_Click(object sender, EventArgs e)
        {
            Tx_Manager_Name.Text = "";
            Tx_Manager_No.Text = "";
            Keepers_Manager_LoadID();
        }
      

        private void Tx_Search_Manager_TextChanged(object sender, EventArgs e)
        {
            DataGrid_Manager.DataSource = this.PopulateDataGridView2();
        }
        //Ends here....
        #endregion Managers Block

       
    }
}

