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

namespace TKHCI.Cell_Module.Cell_CRM
{
    public partial class Cell_MAIN_CRM : Form
    {
        #region
        #endregion
        #region START FORM HERE ADDS
        private readonly string load_salGrade = "SELECT  'Select Zone Name' as zoneName union all SELECT distinct ZoneTB.Zone_Name from ZoneTB with (nolock)";

        KeeperDBEntities DBS = new KeeperDBEntities();
        decimal ID;
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        public static class UserSession
        {
            public static string Username;
        }
        #endregion
        public Cell_MAIN_CRM()
        {
            InitializeComponent();
            Load_Zone_ID();
            Keepers_Zone_LoadID();
            _ = DefualtLoad();
        }
        public async Task DefualtLoad()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Load_Zone_ID());
            tasks.Add(Keepers_Cell_LoadID());
            await Task.WhenAll(tasks);
        }
        #region LOAD CELL & ZONE ID
        private void Keepers_Zone_LoadID()
        {
            string constr = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_Zone_LoadID", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            sqlConnection.Close();
            if (xml < 1)
            {
                ID = 1;
                Tx_Zone_ID.Text = $"KHZ-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else if (xml >= 1)
            {
                ID = xml + 1;
                Tx_Zone_ID.Text = $"KHZ-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else return;
        }
        private Task Keepers_Cell_LoadID()
        {
            string constr = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_Cell_LoadID", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            sqlConnection.Close();
            if (xml < 1)
            {
                ID = 1;
                Tx_Cell_ID_Add.Text = $"KHC-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else if (xml >= 1)
            {
                ID = xml + 1;
                Tx_Cell_ID_Add.Text = $"KHC-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }return Task.CompletedTask;
        }
        #endregion
        #region COMBO LOAD
        private Task Load_Zone_ID()
        {

            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(load_salGrade, Con);
            DataTable glDepo = new DataTable();
            da.Fill(glDepo);
            Combo_Zone_ID.DataSource = glDepo;
            Combo_Zone_ID.DisplayMember = "zoneName";
            Combo_Zone_ID.ValueMember = "zoneName";
            Con.Close();
            return Task.CompletedTask;
        }
        #endregion
        #region CELL & ZONE DATA CELL CLICK
        private void LoadZonal_DataGriD_View()
        {
            var user1 = DBS.ZoneTBs.FirstOrDefault(a => a.Zone_ID.Equals(UserSession.Username));
            if (!String.IsNullOrEmpty(UserSession.Username))
            {
                if (user1 != null)
                {
                    Tx_Zone_ID.Text = user1.Zone_ID;
                    Tx_Zone_Name.Text = user1.Zone_Name;
                    Tx_ZoneGEO_Loc.Text = user1.Zone_Location;
                    Tx_Zone_Address.Text = user1.Zonel_Adress;
                    Tx_Zone_Asst_Name.Text = user1.Zone_Asst_Name;
                    Tx_Zone_Email.Text = user1.Zone_Email;
                    Tx_Zone_LeadName.Text = user1.Zone_Lead_Name;
                    Tx_Zone_Phone.Text = user1.Zone_Phone;
                    Combo_Zone_Status.Text = user1.Zone_Status;
                }
                else { return; }
            }
            else
            {
                return;
            }

        }
        private void LoadCell_DataGriD_View()
        {
            var user1 = DBS.CellTBs.FirstOrDefault(a => a.Cell_ID.Equals(UserSession.Username));
            if (!String.IsNullOrEmpty(UserSession.Username))
            {
                if (user1 != null)
                {
                    Tx_Cell_ID.Text = user1.Cell_ID;
                    Tx_Cell_Name.Text = user1.Cell_Name;
                    Tx_Cell_GEO_Loc.Text = user1.Cell_Location;
                    Tx_Cell_Address.Text = user1.Cell_Adress;
                    Tx_Cell_Asst_Name.Text = user1.Cell_Asst_Name;
                    Tx_Cell_Email.Text = user1.Cell_Email;
                    Tx_Cell_LeadName.Text = user1.Cell_Lead_Name;
                    Tx_Cell_Phone.Text = user1.Cell_Phone;
                    Combo_Cell_Status.Text = user1.Cell_Status;
                    Com_Zone_ID.Text = user1.Zonal_Name;
                    Combo_Cell_Status.Text = user1.Zonal_Name;
                    //Tx_Zone_IDOne.Text = user1.Zonal_Name;
                }
                else { return; }
            }
            else
            {
                return;
            }

        }
        #endregion
        #region LOAD CELL & ZONE DB DATA

        public object PopulateDataGridView_ZONE()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_LoadSearch_Zone]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", txtSearchCell_By_Name.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        private object PopulateDataGridView_CELL()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_LoadSearch_Cell", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", txtSearchCell_By_Name.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public void Load_Cell_GridView()
        {
            DataGrib_Cell_Info.DataSource = this.PopulateDataGridView_CELL();
        }
        public void Load_ZONE_DATAGRID()
        {
            DataGrib_Zone.DataSource = this.PopulateDataGridView_ZONE();
        }
        #endregion
        #region DATA GRID CELL & ZONE CLICK
        private void DataGrib_Zone_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_Hide_Zonal_ID.Text = DataGrib_Zone.SelectedRows[0].Cells[2].Value.ToString();
            UserSession.Username = Tx_Hide_Zonal_ID.Text;
            LoadZonal_DataGriD_View();
        }
        private void DataGrib_Cell_Info_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_Hide_Cell_ID.Text = DataGrib_Cell_Info.SelectedRows[0].Cells[0].Value.ToString();
            UserSession.Username = Tx_Hide_Cell_ID.Text;
            LoadCell_DataGriD_View();
        }

        #endregion
        #region CELL & ZONE SEARCH
        private void Tx_Search_Zone_TextChanged(object sender, EventArgs e)
        {
            DataGrib_Zone.DataSource = this.PopulateDataGridView_ZONE();
        }
        private void txtSearchCell_By_Name_TextChanged(object sender, EventArgs e)
        {
            DataGrib_Cell_Info.DataSource = this.PopulateDataGridView_CELL();
        }
        #endregion
        #region CLEAR CELL & ZONE FORMS
        private void BT_Clear_Form_Click(object sender, EventArgs e)
        {
            Keepers_Zone_LoadID();
            Tx_ZoneGEO_Loc.Text = "";
            Tx_Zone_Address.Text = "";
            Tx_Zone_Asst_Name.Text = "";
            Tx_Zone_Email.Text = "";
            Tx_Zone_LeadName.Text = "";
            Tx_Zone_Name.Text = "";
            Tx_Zone_Phone.Text = "";
            Combo_Zone_Status.SelectedIndex = 0;
        }
        private void BT_Clear_Add_Click(object sender, EventArgs e)
        {
            Keepers_Cell_LoadID();
            Tx_Cell_Address_Add.Text = "";
            Tx_Cell_Name_Add.Text = ""; ;
            Tx_Cell_Email_Add.Text = "";
            Tx_Cell_LeadName_Add.Text = "";
            Tx_Cell_Gio_Loc_Add.Text = "";
            Tx_Cell_Asst_Name_Add.Text = "";
            Tx_Cell_Phone_Add.Text = "";
            Combo_Cell_Status_Add.SelectedIndex = 0;
            Combo_Zone_ID.SelectedIndex = 0;

        }

        public void ClearCell()
        {
            Tx_Cell_Name.Text = "";
            Tx_Cell_LeadName.Text = "";
            Tx_Cell_Asst_Name.Text = "";
            Tx_Cell_Address.Text = "";
            Tx_Cell_GEO_Loc.Text = "";
            Com_Zone_ID.SelectedIndex = 0;
            Tx_Cell_ID.Text = "";
            Tx_Cell_Email.Text = "";
            Tx_Cell_Phone.Text = "";
            Combo_Cell_Status.SelectedIndex = 0;

        }
        #endregion
        #region ADD CELL & ZONE TO DB
        private void BT_Add_Zone_Click(object sender, EventArgs e)
        {
            ZoneTB css = new ZoneTB();
            try
            {

                if (Tx_Zone_ID.Text != "" && Tx_ZoneGEO_Loc.Text != "" && Tx_Zone_Address.Text != "" && Tx_Zone_Asst_Name.Text != "" && Tx_Zone_Email.Text != "" && Tx_Zone_LeadName.Text != "" && Tx_Zone_Name.Text != ""
                    && Tx_Zone_Phone.Text != "" && Combo_Zone_Status.SelectedIndex != 0)
                {

                    css.Zone_Email = Tx_Zone_Email.Text;
                    css.Zone_Phone = Tx_Zone_Phone.Text;
                    css.Zone_ID = Tx_Zone_ID.Text;
                    css.Zonel_Adress = Tx_Zone_Address.Text;
                    css.CreateDT = DateTime.Now;
                    css.Zone_Lead_Name = Tx_Zone_LeadName.Text;
                    css.Zone_Name = Tx_Zone_Name.Text;
                    css.CreatedBy = TKHCI_Main_Login.SysUserLogin.Username;
                    css.Zone_Status = Combo_Zone_Status.Text;
                    css.Zone_Location = Tx_ZoneGEO_Loc.Text;
                    css.Zone_Asst_Name = Tx_Zone_Asst_Name.Text;

                    string message = "Are you sure you want to add this record?";
                    string title = "Adding Record...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.ZoneTBs.Add(css);
                        DBS.SaveChanges();
                        MessageBox.Show("Zone Created Successfully");
                        BT_Clear_Form_Click(null, null);
                        Load_ZONE_DATAGRID();
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
                        Error_Fill_All_Form eropen = new Error_Fill_All_Form();
                        eropen.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void But_Add_Cell_Add_Click(object sender, EventArgs e)
        {
            CellTB tbCell = new CellTB();

            if (Tx_Cell_ID_Add.Text != "" && Tx_Cell_Gio_Loc_Add.Text != "" && Tx_Cell_Address_Add.Text != "" && Tx_Cell_Asst_Name_Add.Text != "" && 
                Tx_Cell_Email_Add.Text != "" && Tx_Cell_LeadName_Add.Text != "" && Tx_Cell_Name_Add.Text != ""
                && Tx_Cell_Phone_Add.Text != "" && Combo_Cell_Status_Add.SelectedIndex != 0)
            {
                tbCell.Cell_Adress = Tx_Cell_Address_Add.Text;
                tbCell.Cell_Asst_Name = Tx_Cell_Name_Add.Text;
                tbCell.Cell_Email = Tx_Cell_Email_Add.Text;
                tbCell.Cell_ID = Tx_Cell_ID_Add.Text;
                tbCell.Cell_Lead_Name = Tx_Cell_LeadName_Add.Text;
                tbCell.Cell_Location = Tx_Cell_Gio_Loc_Add.Text;
                tbCell.CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                tbCell.CreateDT = DateTime.Now;
                tbCell.Cell_Status = Combo_Cell_Status_Add.Text;
                tbCell.Zonal_Name = Combo_Zone_ID.Text;
                tbCell.Cell_Phone = Tx_Cell_Phone_Add.Text;
                tbCell.Cell_Name = Tx_Cell_Name_Add.Text;
                tbCell.Cell_Phone = Tx_Cell_Phone_Add.Text;

                string message = "Are you sure you want to add this record?";
                string title = "Adding Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.CellTBs.Add(tbCell);
                    DBS.SaveChanges();
                    MessageBox.Show("Zone Created Successfully");
                    BT_Clear_Add_Click(null,null);
                    Load_Cell_GridView();

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
                    Error_Fill_All_Form eropen = new Error_Fill_All_Form();
                    eropen.Show();
                }
            }
        }
        #endregion
        #region UPDATE CELL & ZONE
        private void BT_Update_Zone_Click(object sender, EventArgs e)
        {
            string updateZone = Tx_Hide_Zonal_ID.Text;
            var updt = (from s in DBS.ZoneTBs where s.Zone_ID == updateZone select s).FirstOrDefault();

            updt.Zone_Email = Tx_Zone_Email.Text;
            updt.Zone_Phone = Tx_Zone_Phone.Text;
            updt.Zone_ID = Tx_Zone_ID.Text;
            updt.Zonel_Adress = Tx_Zone_Address.Text;
            updt.Zone_Lead_Name = Tx_Zone_LeadName.Text;
            updt.Zone_Name = Tx_Zone_Name.Text;
            updt.CreatedBy = TKHCI_Main_Login.SysUserLogin.Username;
            updt.Zone_Status = Combo_Zone_Status.Text;
            updt.Zone_Location = Tx_ZoneGEO_Loc.Text;
            updt.Zone_Asst_Name = Tx_Zone_Asst_Name.Text;

            string message = "Are you sure you want to update this record?";
            string title = "updating Record...";
            MessageBoxButtons but = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, but);
            if (result == DialogResult.Yes)
            {

                DBS.SaveChanges();
                MessageBox.Show("Zone Record Updated Successfully");
                Load_ZONE_DATAGRID();
                BT_Clear_Form_Click(null, null);
            }
            else return;
        }
        private void But_Update_Click(object sender, EventArgs e)
        {
            string updateZone = Tx_Hide_Cell_ID.Text;
            var updt = (from s in DBS.CellTBs where s.Cell_ID == updateZone select s).FirstOrDefault();

            updt.Cell_Email = Tx_Cell_Email.Text;
            updt.Cell_Phone = Tx_Cell_Phone.Text;
            updt.Cell_ID = Tx_Cell_ID.Text;
            updt.Cell_Adress = Tx_Cell_Address.Text;
            updt.Cell_Lead_Name = Tx_Cell_LeadName.Text;
            updt.Cell_Name = Tx_Cell_Name.Text;
            updt.Cell_Status = Combo_Cell_Status.Text;
            updt.Cell_Location = Tx_Cell_GEO_Loc.Text;
            updt.Cell_Asst_Name = Tx_Cell_Asst_Name.Text;
            updt.Zonal_Name = Com_Zone_ID.Text;


            string message = "Are you sure you want to update this record?";
            string title = "updating Record...";
            MessageBoxButtons but = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, but);
            if (result == DialogResult.Yes)
            {

                DBS.SaveChanges();
                MessageBox.Show("Cell Record Updated Successfully");
                ClearCell();
                Load_Cell_GridView();

            }
            else return;
        }
        #endregion
        #region DELETE CELL & ZONE
        private void BT_Del_Zone_Click(object sender, EventArgs e)
        {

            ZoneTB acc = new ZoneTB();
            if (Tx_Hide_Zonal_ID.Text == "")
            {
                return;
            }
            else
            {
                string Del = Tx_Hide_Zonal_ID.Text;
                if (!ID.Equals(null))
                {
                    var cat = (from s in DBS.ZoneTBs where s.Zone_ID.Equals(Tx_Hide_Zonal_ID.Text) select s).First();

                    string message = "Are you sure you want to delete this Product?";
                    string title = "Deleting Product...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.ZoneTBs.Remove(cat);
                        DBS.SaveChanges();
                        MessageBox.Show("Product Deleted");
                        Load_ZONE_DATAGRID();
                        BT_Clear_Form_Click(null, null);
                    }
                    else return;
                }
                else MessageBox.Show("Select User to delete");

            }
        }
        private void BUT_Del_Click(object sender, EventArgs e)
        {
            CellTB acc = new CellTB();
            if (Tx_Hide_Cell_ID.Text == "")
            {
                MessageBox.Show("Kindly elect Cell to Delete");
            }
            else
            {
                string Del = Tx_Hide_Cell_ID.Text;
                if (!Del.Equals(null))
                {
                    var cat = (from s in DBS.CellTBs where s.Cell_ID.Equals(Tx_Hide_Cell_ID.Text) select s).First();

                    string message = "Are you sure you want to delete this Product?";
                    string title = "Deleting Product...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.CellTBs.Remove(cat);
                        DBS.SaveChanges();
                        MessageBox.Show("Product Deleted");
                        ClearCell();
                        Load_Cell_GridView();
                        Tx_Hide_Cell_ID.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Kindly elect Cell to Delete");
                    }
                }
                else MessageBox.Show("Select User to delete");
            }
        }
        #endregion
        #region MAIN LOAD FORM
        private void Cell_MAIN_CRM_Load(object sender, EventArgs e)
        {
            DataGrib_Zone.DataSource = this.PopulateDataGridView_ZONE();
            Load_Cell_GridView();

        }



        #endregion

        
    }
}
