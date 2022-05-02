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

namespace TKHCI.Finance
{
    public partial class AssertModule : Form
    {
        #region LOAD SCRIPT
        private readonly string load_DeptName = "Select 'Select Department Name' as DeptName union all SELECT distinct DepartmentTB.Dept_Name DeptName from DepartmentTB with (nolock);";
        private readonly string Load_ManName = " Select 'Select Manager Name' as ManName union all SELECT distinct ManagersTB.Manager_Name cellName from ManagersTB WITH (NOLOCK)";
        private readonly string Load_CategoryNames = "Select 'Select Asset Category' as AssetCat union all SELECT distinct Asset_CategoryTB.Cat_Name cellName from Asset_CategoryTB WITH (NOLOCK)";
        KeeperDBEntities DBS = new KeeperDBEntities();
        string assetStatus = string.Empty;
        decimal Disposal;
        decimal Liability;
        decimal Asset;
        MemoryStream ms;
        byte[] updt_data;
        string extension;
        byte[] file;
        string liaType;
        Stream stream;
        string ID;
        public static class LiaClick
        {
            public static string lia;
        }           
        public static class AssetCLick
        {
            public static string asset;
        }
        public static class DisposalCLick
        {
            public static string dispose;
        }
        public static class Category
        {
            public static string catGORY;
        }
        #endregion
        public AssertModule()
        {
            InitializeComponent();
            _ = Page_Load();
        }
        #region B 1

        //*************************************************************
        //END

        public async Task Page_Load()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Load_Combo_Box());
            tasks.Add(Keepers_Load_DisposalID());
            tasks.Add(Keepers_Load_AssetID());
            tasks.Add(Keepers_Load_CategoryID());
            tasks.Add(Keepers_Load_Liability_ID());
            await Task.WhenAll(tasks);
        }

        #endregion
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        #region LOAD ID'S 
        private Task Keepers_Load_Liability_ID()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_Liability_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();


            if (xml < 1)
            {
                Liability = 1;
                Tx_Lia_ID.Text = $"LIA-{DateTime.Now.Year}-" + Convert.ToString(Liability);
            }
            else if (xml >= 1)
            {
                Liability = xml + 1;
                Tx_Lia_ID.Text = $"LIA-{DateTime.Now.Year}-" + Convert.ToString(Liability);
            }
            return Task.CompletedTask;

        }
        private Task Keepers_Load_DisposalID()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_Disposal_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();


            if (xml < 1)
            {
                Disposal = 1;
                Tx_Disposal_ID.Text = $"KASSET_D-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            else if (xml >= 1)
            {
                Disposal = xml + 1;
                Tx_Disposal_ID.Text = $"KASSET_D-{DateTime.Now.Year}-" + Convert.ToString(Disposal);
            }
            return Task.CompletedTask;

        }
        private Task Keepers_Load_AssetID()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_Asset_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();


            if (xml < 1)
            {
                Asset = 1;
                Tx_AssetNo.Text = $"KASSET-{DateTime.Now.Year}-" + Convert.ToString(Asset);
            }
            else if (xml >= 1)
            {
                Asset = xml + 1;
                Tx_AssetNo.Text = $"KASSET-{DateTime.Now.Year}-" + Convert.ToString(Asset);
            }
            return Task.CompletedTask;

        }

        private Task Keepers_Load_CategoryID()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_AssetCatgory_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();


            if (xml < 1)
            {
                Asset = 1;
                Tx_Cat_No.Text = $"K-AssetCat-{DateTime.Now.Year}-" + Convert.ToString(Asset);
            }
            else if (xml >= 1)
            {
                Asset = xml + 1;
                Tx_Cat_No.Text = $"AssetCat-{DateTime.Now.Year}-" + Convert.ToString(Asset);
            }
            return Task.CompletedTask;

        }
        //*************************************************************
        #endregion LOAD ID'S
        #region Populate & Search & GridView
        public object PopulateDataGridView_Liability()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_Liability]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Lia_Search.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateDataGridView_Asset()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_Asset]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_Asset.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateDataGridView_Disposal()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_Disposal]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_Disposal.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateDataGridView_Category()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_Load_and_Search_Asset_Category]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", Tx_Search_CAT.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        #endregion
        #region Load ComboBox
        private Task Load_Combo_Box()
        {
            Con.Open();
            SqlDataAdapter da = new SqlDataAdapter(load_DeptName, Con);
            DataTable DepName = new DataTable();
            da.Fill(DepName);
            Combo_Asset_Dept.DataSource = DepName;
            Combo_Asset_Dept.DisplayMember = "DeptName";
            Combo_Asset_Dept.ValueMember = "DeptName";

            DataTable manName = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(Load_ManName, Con);
            da1.Fill(manName);
            Combo_Manager_Name.DataSource = manName;
            Combo_Manager_Name.DisplayMember = "ManName";
            Combo_Manager_Name.ValueMember = "ManName";
            Con.Close();

            DataTable AssetCat = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(Load_CategoryNames, Con);
            da2.Fill(AssetCat);
            ComboBox_Asset_Cat.DataSource = AssetCat;
            ComboBox_Asset_Cat.DisplayMember = "AssetCat";
            ComboBox_Asset_Cat.ValueMember = "AssetCat";
            Con.Close();

            return Task.CompletedTask;
        }
        #endregion Load ComboBox 
        #region Searching Data on GridView
        private void Tx_Search_Disposal_TextChanged(object sender, EventArgs e)
        {
            DataGridView_Disposal.DataSource = this.PopulateDataGridView_Disposal();
        }
        private void Tx_Search_Asset_TextChanged(object sender, EventArgs e)
        {
            DataGridView_Asset.DataSource = this.PopulateDataGridView_Asset();
        }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            DataGridView_Category.DataSource = this.PopulateDataGridView_Category();
        }
        private void Tx_Lia_Search_TextChanged(object sender, EventArgs e)
        {
            Load_Liability();
        }

        #endregion
        #region DatagridView Cell Click
        private void CellClick_Liability()
        {
            var liaClick = DBS.LiabilitiesTBs.FirstOrDefault(a => a.LI_ID.Equals(LiaClick.lia));
            if (!String.IsNullOrEmpty(LiaClick.lia))
            {
                if (liaClick != null)
                {
                    Tx_Lia_ID.Text = liaClick.LI_ID;
                    Tx_Lia_Name.Text = liaClick.LI_Name;
                    Combo_Lia_Type.SelectedIndex = (int)liaClick.LI_Type;
                    Tx_Lia_Des.Text = liaClick.LI_Des;
                }
                else { return; }
            }
            else
            {
                return;
            }
        }
        private void CellClick_AssetDataGriD_View()
        {
            var assetClick = DBS.AssetTBs.FirstOrDefault(a => a.Asset_ID.Equals(AssetCLick.asset));
            if (!String.IsNullOrEmpty(AssetCLick.asset))
            {
                if (assetClick != null)
                {
                    var img = assetClick.Asset_Image;
                    var ms = new MemoryStream(img);
                    LogAssetImage.Image = Image.FromStream(ms);
                    Tx_AssetNo.Text = assetClick.Asset_ID;
                    Tx_Asset_Name.Text = assetClick.Asset_Name;
                    Tx_Model_No.Text = assetClick.Model_No;
                    Tx_SerialNumber.Text = assetClick.Serial_No;
                    Tx_Asset_Description.Text = assetClick.Description;
                    Tx_Unit_Price.Text = Convert.ToString(assetClick.Unit_Price);
                    DateTime_Pur_DT.Value = assetClick.Purchase_DT.Value.Date;
                    ComboBox_Asset_Cat.Text = assetClick.Asset_Categoty;
                    Combo_Asset_Dept.Text = assetClick.Dept;
                    Combo_Manager_Name.Text = assetClick.Manager_Name;
                    Tx_WarrantyMonth.Text = assetClick.WarrantyMonth;
                    DateTime_Next_Maintain.Value = assetClick.Next_mantain_DT.Value.Date;
                    Tx_Supplier_Name.Text = assetClick.Supplaier_Name;
                    Tx_Supplier_Phone.Text = assetClick.Supplier_Phone;
                    Tx_Warranty_Note.Text = assetClick.Asset_Note;
                    lbl_File_Upload.Text = assetClick.FilePath;
                    Tx_Warranty_File_Name.Text = assetClick.Warranty_File_Name;
                    Combo_Cur_ID.SelectedIndex = (int)assetClick.Cur_ID;
                }
                else { return; }
            }
            else
            {
                return;
            }
        }
        private void CellClick_DisposalDataGriD_View()
        {
            var disposeClick = DBS.Asset_DisposalTB.FirstOrDefault(a => a.Disposal_ID.Equals(DisposalCLick.dispose));
            if (!String.IsNullOrEmpty(DisposalCLick.dispose))
            {
                if (disposeClick != null)
                {
                    Tx_Disposal_ID.Text = disposeClick.Disposal_ID;
                    Tx_Dis_Asset_No.Text = disposeClick.AssetID;
                    Tx_Dis_UnitPrice_No.Text = Convert.ToString(disposeClick.UnitPrice);
                    Tx_Dis_Asset_NameDis.Text = disposeClick.AssetName;
                    DateTime_Dis_PurchaseDT.Value = disposeClick.PurchaseDT.Value.Date;
                    Tx_Dis_Purchase_AmtDis.Text = Convert.ToString(disposeClick.PurchaseAMT);
                    Tx_Dis_Total_Depreciation.Text = Convert.ToString(disposeClick.Total_Depreciation);
                    Tx_Dis_Dipreciation_Month.Text = Convert.ToString(disposeClick.DepreciationM);
                    DateTime_Disposal_DT.Value = disposeClick.DisposalDT.Value.Date;

                    if (disposeClick.MarkAS == "Disposed")
                    {
                        Radio_DisposeAsset.Checked = true;
                        Radio_NotDisposed.Checked = false;
                        Radio_Lost_Asset.Checked = false;
                    }
                    else if (disposeClick.MarkAS == "Not Disposed")
                    {
                        Radio_DisposeAsset.Checked = false;
                        Radio_NotDisposed.Checked = true;
                        Radio_Lost_Asset.Checked = false;
                    }
                    else
                    {
                        Radio_DisposeAsset.Checked = false;
                        Radio_NotDisposed.Checked = false;
                        Radio_Lost_Asset.Checked = true;
                    }
                }
                else { return; }
            }
            else
            {
                return;
            }
        }
        private void CellClick_CategoryDataGriD_View()
        {
            var catgoryClick = DBS.Asset_CategoryTB.FirstOrDefault(a => a.CAT_ID.Equals(Category.catGORY));
            if (!String.IsNullOrEmpty(Category.catGORY))
            {
                if (catgoryClick != null)
                {
                    Tx_Cat_No.Text = catgoryClick.CAT_ID;
                    Tx_Cat_Name.Text = catgoryClick.Cat_Name;
                    Tx_Cat_Description.Text = catgoryClick.Cat_Des;
                    Combo_Asset_Type.SelectedIndex = (int)catgoryClick.LI_Type;

                }
                else { return; }
            }
            else
            {
                return;
            }
        }
        private void DataGrid_LIABILITY_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_Hide_Lia.Text = DataGrid_LIABILITY.SelectedRows[0].Cells[1].Value.ToString();
            LiaClick.lia = Tx_Hide_Lia.Text;
            CellClick_Liability();
        }
        private void DataGridView_Asset_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_Hide_Asset.Text = DataGridView_Asset.SelectedRows[0].Cells[1].Value.ToString();
            AssetCLick.asset = Tx_Hide_Asset.Text;
            CellClick_AssetDataGriD_View();
        }


        private void DataGridView_Disposal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_Hide_Disposal.Text = DataGridView_Disposal.SelectedRows[0].Cells[1].Value.ToString();
            DisposalCLick.dispose = Tx_Hide_Disposal.Text;
            CellClick_DisposalDataGriD_View();
        }
        private void DataGridView_Category_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_Hide_Catgo.Text = DataGridView_Category.SelectedRows[0].Cells[2].Value.ToString();
            Category.catGORY = Tx_Hide_Catgo.Text;
            CellClick_CategoryDataGriD_View();
        }

        private void DataGridView_MAINDOC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Tx_Hide_Asset.Text = DataGridView_Asset.SelectedRows[0].Cells[1].Value.ToString();
            AssetCLick.asset = Tx_Hide_Asset.Text;
            CellClick_AssetDataGriD_View();

        }
        #endregion
        #region Load Data on Form: Load
        public void Load_Liability()
        {
            DataGrid_LIABILITY.DataSource = this.PopulateDataGridView_Liability();
        }
        public void Load_From_Table_New_Data_Asset()
        {
            DataGridView_Asset.DataSource = this.PopulateDataGridView_Asset();
        }
        public void Load_From_Table_New_Data_Dispose()
        {
            DataGridView_Disposal.DataSource = this.PopulateDataGridView_Disposal();
        }
        public void Load_From_Table_New_Data_Category()
        {
            DataGridView_Category.DataSource = this.PopulateDataGridView_Category();
        }
        public void Load_From_Table_New_Data_AssetMAIN_DOC()
        {
            DataGridView_MAINDOC.DataSource = this.PopulateDataGridView_Asset();
        }

        private void AssertModule_Load(object sender, EventArgs e)
        {
            Load_From_Table_New_Data_Asset();
            Load_From_Table_New_Data_Dispose();
            Load_From_Table_New_Data_Category();
            Load_From_Table_New_Data_AssetMAIN_DOC();
            Load_Liability();
            Tx_Error_Fill.Visible = false;

        }


        #endregion
        #region Asset and Disposal Data Capturing
        private void Bu_Browse_Image_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openbdlg = new OpenFileDialog();
                if (openbdlg.ShowDialog() == DialogResult.OK)
                {
                    Image img = Image.FromFile(openbdlg.FileName);
                    LogAssetImage.Image = img;
                    ms = new MemoryStream();
                    img.Save(ms, img.RawFormat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid File...");
            }
        }
        private void SaveFile(string filePath)


        {
            if(Tx_AssetNo.Text != "" && Tx_Model_No.Text != "" && Tx_SerialNumber.Text != "" && Tx_Unit_Price.Text != "" && Combo_Cur_ID.SelectedIndex != 0
                 && ComboBox_Asset_Cat.SelectedIndex != -1 && Combo_Asset_Dept.SelectedIndex != -1 && Combo_Manager_Name.SelectedIndex != -1
                 && Tx_WarrantyMonth.Text != "" && Tx_Supplier_Name.Text != "" && Tx_Supplier_Phone.Text != "" && LogAssetImage.Image != null && lbl_File_Upload.Text != "" && ms.Length != 0)
            {
                using (stream = File.OpenRead(filePath))
                {
                    file = new byte[stream.Length];
                    stream.Read(file, 0, file.Length);

                    extension = new FileInfo(filePath).Extension;
                }
            }
            else
            {
              
            }
            try
            {
                if (LogAssetImage.Image != null)
                {
                    if (Tx_AssetNo.Text != "" && Tx_Model_No.Text != "" && Tx_SerialNumber.Text != "" && Tx_Unit_Price.Text != "" && Combo_Cur_ID.SelectedIndex !=0
                 && ComboBox_Asset_Cat.SelectedIndex != -1 && Combo_Asset_Dept.SelectedIndex != -1 && Combo_Manager_Name.SelectedIndex != -1
                 && Tx_WarrantyMonth.Text != "" && Tx_Supplier_Name.Text != "" && Tx_Supplier_Phone.Text != "" && LogAssetImage.Image != null && lbl_File_Upload.Text != "" && ms.Length !=0)
                    {
                        AssetTB aset = new AssetTB();
                        aset.Asset_ID = Tx_AssetNo.Text;
                        aset.Asset_Name = Tx_Asset_Name.Text;
                        aset.Dept = Combo_Asset_Dept.Text;
                        aset.CreatedBY = TKHCI_Main_Login.MainLogin.Username;
                        aset.Model_No = Tx_Model_No.Text;
                        aset.Next_mantain_DT = DateTime_Next_Maintain.Value.Date;
                        aset.Purchase_DT = DateTime_Pur_DT.Value.Date;
                        aset.CreateDT = DateTime.Now;
                        aset.Description = Tx_Asset_Description.Text;
                        aset.Asset_Note = Tx_Warranty_Note.Text;
                        aset.Serial_No = Tx_SerialNumber.Text;
                        aset.Unit_Price = Convert.ToDecimal(Tx_Unit_Price.Text);
                        aset.Cur_ID = Combo_Cur_ID.SelectedIndex;
                        aset.Asset_Categoty = ComboBox_Asset_Cat.Text;
                        aset.Manager_Name = Combo_Manager_Name.Text;
                        aset.WarrantyMonth = Tx_WarrantyMonth.Text;
                        aset.Supplaier_Name = Tx_Supplier_Name.Text;
                        aset.Supplier_Phone = Tx_Supplier_Phone.Text;
                        aset.Asset_Image = ms.ToArray();
                        aset.Warranty_File = file;
                        aset.Extension = extension;
                        aset.FilePath = lbl_File_Upload.Text;
                        aset.Warranty_DT = DateTime_Wa_Issue_DT.Value.Date;
                        aset.Warranty_File_Name = Tx_Warranty_File_Name.Text;

                        string message = "Are you sure you want to add this record?";
                        string title = "Adding Record...";
                        MessageBoxButtons but = MessageBoxButtons.YesNo;
                        DialogResult result = MessageBox.Show(message, title, but);
                        if (result == DialogResult.Yes)
                        {
                            DBS.AssetTBs.Add(aset);
                            DBS.SaveChanges();
                            MessageBox.Show("User Created Successfully");
                            BT_Clear_Form_Asset_Click(null, null);
                            DataGridView_Asset.DataSource = this.PopulateDataGridView_Asset();
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

                            Tx_AssetNo.BorderColor = (Tx_AssetNo.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_Asset_Name.BorderColor = (Tx_Asset_Name.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_Model_No.BorderColor = (Tx_Model_No.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_SerialNumber.BorderColor = (Tx_SerialNumber.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_Unit_Price.BorderColor = (Tx_Unit_Price.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            ComboBox_Asset_Cat.BorderColor = (ComboBox_Asset_Cat.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Combo_Asset_Dept.BorderColor = (Combo_Asset_Dept.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Combo_Manager_Name.BorderColor = (Combo_Manager_Name.SelectedIndex <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_WarrantyMonth.BorderColor = (Tx_WarrantyMonth.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_Supplier_Name.BorderColor = (Tx_Supplier_Name.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_Supplier_Phone.BorderColor = (Tx_Supplier_Phone.Text.Length <= 0 ? Color.Red : Color.FromArgb(0, 0, 64));
                            Tx_Error_Fill.Visible = true;
                        }
                    }
                }
                else
                {
                    if (Application.OpenForms.OfType<Error_Pack.Error_Select_Asset_Img>().Count() == 1)
                    {
                        return;
                    }
                    else
                    {
                        Error_Pack.Error_Select_Asset_Img show = new Error_Pack.Error_Select_Asset_Img();
                        show.ShowDialog();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void BT_Add_Asset_Click(object sender, EventArgs e)
        {
            string UpdateFam = Tx_Hide_Asset.Text;
            var check = (from s in DBS.AssetTBs where s.Asset_ID == UpdateFam select s).FirstOrDefault();
            if (check==null)
            {
                SaveFile(lbl_File_Upload.Text);
            }
            else
            {
                MessageBox.Show(this, "Asset " + UpdateFam + " Already Exist" + " ," + " Kindly clear the form to add a new Asset", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }
        private void Bu_Browse_File_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            lbl_File_Upload.Text = ofd.FileName;

        }
        private void But_Upload_Warranty_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = System.IO.Path.GetFileName(openFileDialog_Warranty.FileName);
                if (filename == null)
                {
                    MessageBox.Show("Please select a valid document.");
                }
                else
                {
                    //we already define our connection globaly. We are just calling the object of connection.
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into doc (document)values('\\Document\\" + filename + "')", Con);
                    string path = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));
                    System.IO.File.Copy(openFileDialog_Warranty.FileName, path + "\\Document\\" + filename);
                    cmd.ExecuteNonQuery();
                    Con.Close();
                    MessageBox.Show("Document uploaded.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region Clear Form
        private void BT_Clear_Form_Asset_Click(object sender, EventArgs e)
        {
            Tx_Hide_Asset.Text = "";
            Combo_Cur_ID.SelectedIndex = 0; 
            Keepers_Load_AssetID();
            lbl_File_Upload.Text = "";
            Tx_Asset_Name.Text = "";
            Tx_Model_No.Text = "";
            Tx_SerialNumber.Text = "";
            Tx_Asset_Description.Text = "";
            Tx_Unit_Price.Text = "";
            ComboBox_Asset_Cat.SelectedIndex = -1;
            Combo_Asset_Dept.Text = "";
            Combo_Manager_Name.SelectedIndex = -1;
            Tx_WarrantyMonth.Text = "";
            Tx_Supplier_Name.Text = "";
            Tx_Supplier_Phone.Text = "";
            Tx_Warranty_Note.Text = "";
            LogAssetImage.Image = null;
            Tx_Warranty_File_Name.Text = "";
        }
        #endregion
        #region DISPOSAL
        private void Bu_Add_Dis_Click(object sender, EventArgs e)
        {
            Asset_DisposalTB dispose = new Asset_DisposalTB();

            if (Radio_DisposeAsset.Checked)
            {
                assetStatus = "Disposed";
            }
            else if (Radio_NotDisposed.Checked)
            {
                assetStatus = "Not Disposed";
            }
            else if (Radio_Lost_Asset.Checked)
            {
                assetStatus = "Lost";
            }

            if (Tx_Disposal_ID.Text != "" && Tx_Dis_Asset_No.Text != "" && Tx_Dis_UnitPrice_No.Text != "" &&
                Tx_Dis_Asset_NameDis.Text != "" && Tx_Dis_Purchase_AmtDis.Text != "" && Tx_Dis_Total_Depreciation.Text != ""
                && Tx_Dis_Dipreciation_Month.Text != "")
            {
                dispose.AssetID = Tx_Dis_Asset_No.Text;
                dispose.AssetName = Tx_Dis_Asset_NameDis.Text;
                dispose.MarkAS = assetStatus;
                dispose.DepreciationM = Convert.ToDecimal(Tx_Dis_Dipreciation_Month.Text);
                dispose.CreateBy = TKHCI_Main_Login.MainLogin.Username;
                dispose.CreateDT = DateTime.Now;
                dispose.Disposal_ID = Tx_Disposal_ID.Text;
                dispose.UnitPrice = Convert.ToDecimal(Tx_Dis_UnitPrice_No.Text);
                dispose.PurchaseDT = DateTime_Dis_PurchaseDT.Value.Date;
                dispose.Total_Depreciation = Convert.ToDecimal(Tx_Dis_Total_Depreciation.Text);
                dispose.DisposalDT = DateTime_Disposal_DT.Value.Date;
                string message = "Are you sure you want to add this record?";
                string title = "Adding Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.Asset_DisposalTB.Add(dispose);
                    DBS.SaveChanges();
                    MessageBox.Show("User Created Successfully");
                    Bu_Clear_Dis_Click(null, null);
                    DataGridView_Disposal.DataSource = this.PopulateDataGridView_Disposal();


                }
                else return;


            }
        }
        private void Bu_Clear_Dis_Click(object sender, EventArgs e)
        {
            Keepers_Load_DisposalID();
            Tx_Disposal_ID.Text = "";
            Tx_Dis_Asset_No.Text = "";
            Tx_Dis_UnitPrice_No.Text = "";
            Tx_Dis_Asset_NameDis.Text = "";
            Tx_Dis_Purchase_AmtDis.Text = "";
            Tx_Dis_Total_Depreciation.Text = "";
            Tx_Dis_Dipreciation_Month.Text = "";
            Radio_NotDisposed.Checked = true;

        }
        private void BT_Add_Cat_Click(object sender, EventArgs e)
        {
            Asset_CategoryTB cat = new Asset_CategoryTB();
            if (Tx_Cat_No.Text != "" && Tx_Cat_Name.Text != "" && Combo_Asset_Type.SelectedIndex !=0)
            {
                cat.CreateDT = DateTime.Now;
                cat.Cat_Des = Tx_Cat_Description.Text;
                cat.Cat_Name = Tx_Cat_Name.Text;
                cat.CreatedByW = TKHCI_Main_Login.MainLogin.Username;
                cat.CAT_ID = Tx_Cat_No.Text;
                cat.LI_Type = Combo_Asset_Type.SelectedIndex;

                string message = "Are you sure you want to add this record?";
                string title = "Adding Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.Asset_CategoryTB.Add(cat);
                    DBS.SaveChanges();
                    MessageBox.Show("User Created Successfully");
                    BT_Clear_Cat_Click(null, null);
                    Load_From_Table_New_Data_Category();
                    AssertModule_Load(null, null);


                }
                else return;
            }
        }
        private void BT_Clear_Cat_Click(object sender, EventArgs e)
        {
            Tx_Cat_No.Text = "";
            Tx_Cat_Name.Text = "";
            Tx_Cat_Description.Text = "";
            Combo_Asset_Type.SelectedIndex = 0;
            Keepers_Load_CategoryID();

        }
        #endregion
        #region UPdate Records
        private void BT_Update_Cat_Click(object sender, EventArgs e)
        {
            string UpdateFam = Tx_Hide_Catgo.Text;
            var updtCategory = (from s in DBS.Asset_CategoryTB where s.CAT_ID == UpdateFam select s).FirstOrDefault();

            if (Tx_Hide_Catgo.Text != null && Tx_Cat_Name.Text != "")
            {
                updtCategory.CreateDT = DateTime.Now;
                updtCategory.Cat_Des = Tx_Cat_Description.Text;
                updtCategory.Cat_Name = Tx_Cat_Name.Text;
                updtCategory.CreatedByW = TKHCI_Main_Login.MainLogin.Username;
                updtCategory.CAT_ID = Tx_Cat_No.Text;
                updtCategory.LI_Type = Convert.ToInt32(Combo_Asset_Type.Text);


                string message = "Are you sure you want to update this record?";
                string title = "updating Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {

                    DBS.SaveChanges();
                    MessageBox.Show("Family Record Updated Successfully");
                    DataGridView_Category.DataSource = this.PopulateDataGridView_Category();
                }
                else return;
            }
            else
            {
                MessageBox.Show("Select a Categoty to delete");
            }
        }
        private void SaveFile_Update(string filePath)
        {
            string UpdateFam = Tx_Hide_Asset.Text;
            var updAsset = (from s in DBS.AssetTBs where s.Asset_ID == UpdateFam select s).FirstOrDefault();
            filePath = updAsset.FilePath;

            if(filePath != null && filePath.Length != 0)
            {
                using (Stream stream = File.OpenRead(filePath))
                {
                    updt_data = new byte[stream.Length];
                    stream.Read(updt_data, 0, updt_data.Length);
                    extension = new FileInfo(filePath).Extension;
                }
            }


            if (Tx_Hide_Asset.Text != null && Tx_Asset_Name.Text != "")
            {
                updAsset.Asset_ID = Tx_AssetNo.Text;
                updAsset.Asset_Name = Tx_AssetNo.Text;
                updAsset.Dept = Combo_Asset_Dept.Text;
                updAsset.CreatedBY = TKHCI_Main_Login.MainLogin.Username;
                updAsset.Model_No = Tx_Model_No.Text;
                updAsset.Next_mantain_DT = DateTime_Next_Maintain.Value.Date;
                updAsset.Purchase_DT = DateTime_Pur_DT.Value.Date;
                updAsset.CreateDT = DateTime.Now;
                updAsset.Description = Tx_Asset_Description.Text;
                updAsset.Asset_Note = Tx_Warranty_Note.Text;
                updAsset.Serial_No = Tx_SerialNumber.Text;
                updAsset.Unit_Price = Convert.ToDecimal(Tx_Unit_Price.Text);
                updAsset.Asset_Categoty = ComboBox_Asset_Cat.Text;
                updAsset.Manager_Name = Combo_Manager_Name.Text;
                updAsset.WarrantyMonth = Tx_WarrantyMonth.Text;
                updAsset.Supplaier_Name = Tx_Supplier_Name.Text;
                updAsset.Supplier_Phone = Tx_Supplier_Phone.Text;
                updAsset.Warranty_DT = DateTime_Wa_Issue_DT.Value.Date;
                updAsset.Extension = extension;
                updAsset.Warranty_File = updt_data;
                updAsset.Warranty_File_Name = Tx_Warranty_File_Name.Text;
                updAsset.FilePath = lbl_File_Upload.Text;
                var img = LogAssetImage.Image;
                if (ms != null)
                {
                    img.Save(ms, img.RawFormat);
                    updAsset.Asset_Image = ms.ToArray();
                }

                string message = "Are you sure you want to update this Asset?";
                string title = "updating Asset...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {

                    DBS.SaveChanges();
                    MessageBox.Show("Asset Record Updated Successfully");
                    DataGridView_Category.DataSource = this.PopulateDataGridView_Category();
                    BT_Clear_Form_Asset_Click(null, null);
                }
                else return;
            }
            else
            {
                MessageBox.Show("Select an Asset to update..");
            }

        }
        private void BT_Update_Asset_Click(object sender, EventArgs e)
        {

            SaveFile_Update(lbl_File_Upload.Text);
        }
        private void Bu_Update_Dis_Click(object sender, EventArgs e)
        {
            string UpdateDisposal = Tx_Hide_Disposal.Text;
            var updtDisposal = (from s in DBS.Asset_DisposalTB where s.Disposal_ID == UpdateDisposal select s).FirstOrDefault();

            if (Tx_Hide_Disposal.Text != null && Tx_Dis_Asset_No.Text != "")
            {
                if (Radio_DisposeAsset.Checked)
                {
                    assetStatus = "Disposed";
                }
                else if (Radio_NotDisposed.Checked)
                {
                    assetStatus = "Not Disposed";
                }
                else if (Radio_Lost_Asset.Checked)
                {
                    assetStatus = "Lost";
                }
                if (Tx_Disposal_ID.Text != "" && Tx_Dis_Asset_No.Text != "" && Tx_Dis_UnitPrice_No.Text != "" &&
                Tx_Dis_Asset_NameDis.Text != "" && Tx_Dis_Purchase_AmtDis.Text != "" && Tx_Dis_Total_Depreciation.Text != ""
                && Tx_Dis_Dipreciation_Month.Text != "")
                {
                    updtDisposal.AssetID = Tx_Dis_Asset_No.Text;
                    updtDisposal.AssetName = Tx_Dis_Asset_NameDis.Text;
                    updtDisposal.MarkAS = assetStatus;
                    updtDisposal.DepreciationM = Convert.ToDecimal(Tx_Dis_Dipreciation_Month.Text);
                    updtDisposal.Disposal_ID = Tx_Disposal_ID.Text;
                    updtDisposal.UnitPrice = Convert.ToDecimal(Tx_Dis_UnitPrice_No.Text);
                    updtDisposal.PurchaseDT = DateTime_Dis_PurchaseDT.Value.Date;
                    updtDisposal.Total_Depreciation = Convert.ToDecimal(Tx_Dis_Total_Depreciation.Text);
                    updtDisposal.DisposalDT = DateTime_Disposal_DT.Value.Date;
                }

                string message = "Are you sure you want to update this record?";
                string title = "updating Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {

                    DBS.SaveChanges();
                    MessageBox.Show("Record Updated Successfully");
                    Bu_Clear_Dis_Click(null, null);
                    DataGridView_Disposal.DataSource = this.PopulateDataGridView_Disposal();
                }
                else return;
            }
            else
            {
                MessageBox.Show("Select a Disposal to update");
            }
        }
        #endregion UPdate Records
        #region B-3 Delete Records
        private void BT_Del_Cat_Click(object sender, EventArgs e)
        {
            Asset_CategoryTB catiGo = new Asset_CategoryTB();
            if (Tx_Hide_Catgo.Text == "")
            {
                MessageBox.Show("Select Family to delete");
            }
            else
            {
                //string Del = Tx_Hide_Fam_ID.Text;
                var catgoryDel = (from s in DBS.Asset_CategoryTB where s.CAT_ID.Equals(Tx_Hide_Catgo.Text) select s).First();

                string message = "Are you sure you want to delete this Categoty?";
                string title = "Deleting Categoty...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.Asset_CategoryTB.Remove(catgoryDel);
                    DBS.SaveChanges();
                    MessageBox.Show("Asset Category Deleted");
                    DataGridView_Category.DataSource = this.PopulateDataGridView_Category();
                }
                else return;

            }
        }
        private void BT_Dell_Asset_Click(object sender, EventArgs e)
        {
            AssetTB catiGo = new AssetTB();
            if (Tx_Hide_Asset.Text == "")
            {
                MessageBox.Show("Select Family to delete");
            }
            else
            {
                //string Del = Tx_Hide_Fam_ID.Text;
                var assetDel = (from s in DBS.AssetTBs where s.Asset_ID.Equals(Tx_Hide_Asset.Text) select s).First();

                string message = "Are you sure you want to delete this Asset?";
                string title = "Deleting Categoty...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.AssetTBs.Remove(assetDel);
                    DBS.SaveChanges();
                    MessageBox.Show("Asset  Deleted");
                    DataGridView_Asset.DataSource = this.PopulateDataGridView_Asset();
                    BT_Clear_Form_Asset_Click(null, null);
                }
                else return;

            }
        }
        private void Bu_Delete_Dis_Click(object sender, EventArgs e)
        {
            AssetTB catiGo = new AssetTB();
            if (Tx_Hide_Disposal.Text == "")
            {
                MessageBox.Show("Select Family to delete");
            }
            else
            {
                //string Del = Tx_Hide_Fam_ID.Text;
                var disposalDel = (from s in DBS.Asset_DisposalTB where s.Disposal_ID.Equals(Tx_Hide_Disposal.Text) select s).First();

                string message = "Are you sure you want to delete this record?";
                string title = "Deleting Categoty...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.Asset_DisposalTB.Remove(disposalDel);
                    DBS.SaveChanges();
                    MessageBox.Show("Record  Deleted");
                    Bu_Clear_Dis_Click(null, null);
                    DataGridView_Disposal.DataSource = this.PopulateDataGridView_Disposal();
                }
                else return;

            }
        }


        #endregion
        #region LIABILITY
        private void BU_Lia_Add_Click(object sender, EventArgs e)
        {

            LiabilitiesTB lia = new LiabilitiesTB();

            if (Tx_Lia_ID.Text !="" && Tx_Lia_Name.Text != "" && Combo_Lia_Type.SelectedIndex != 0 && Tx_Lia_Des.Text !="")
            {
                lia.LI_ID = Tx_Lia_ID.Text;
                lia.LI_Name = Tx_Lia_Name.Text;
                lia.LI_Type = Combo_Lia_Type.SelectedIndex;
                lia.LI_Des = Tx_Lia_Des.Text;
                lia.LI_CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                lia.LI_CreateDT = DateTime.Now;

                string message = "Are you sure you want to add this record?";
                string title = "Adding Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.LiabilitiesTBs.Add(lia);
                    DBS.SaveChanges();
                    MessageBox.Show("User Created Successfully");
                    BU_Lia_Clear_Click(null, null);
                    Load_Liability();
                }
                else return;
            }
        }
        private void BU_Lia_Upt_Click(object sender, EventArgs e)
        {
            if (Tx_Lia_ID.Text != "" && Tx_Lia_Name.Text != "" && Combo_Lia_Type.SelectedIndex != 0 && Tx_Lia_Des.Text != "")
            {
                ID = Tx_Hide_Lia.Text;
                var upLia = (from s in DBS.LiabilitiesTBs where s.LI_ID == ID select s).FirstOrDefault();

                upLia.LI_ID = Tx_Lia_ID.Text;
                upLia.LI_Name = Tx_Lia_Name.Text;
                upLia.LI_Type = Combo_Lia_Type.SelectedIndex;
                upLia.LI_Des = Tx_Lia_Des.Text;
                upLia.LI_CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                upLia.LI_CreateDT = DateTime.Now;

                string message = "Are you sure you want to update this record?";
                string title = "updating Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {

                    DBS.SaveChanges();
                    MessageBox.Show("Member Record Updated Successfully");
                    BU_Lia_Clear_Click(null, null);
                    Load_Liability();

                }
                else return;
            }
        }
        private void BU_Lia_Clear_Click(object sender, EventArgs e)
        {
            Keepers_Load_Liability_ID();
            Tx_Lia_Name.Text = "";
            Combo_Lia_Type.SelectedIndex = 0;
            Tx_Lia_Des.Text = "";
        }
        private void BU_Lia_Del_Click(object sender, EventArgs e)
        {

            LiabilitiesTB delLia = new LiabilitiesTB();
            if (Tx_Hide_Lia.Text == "")
            {
                return;
            }
            else
            {
                ID = Tx_Hide_Lia.Text;
                if (!ID.Equals(null))
                {
                    var cat = (from s in DBS.LiabilitiesTBs where s.LI_ID == ID select s).First();
                    string message = "Are you sure you want to delete this Product?";
                    string title = "Deleting Product...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {
                        DBS.LiabilitiesTBs.Remove(delLia);
                        DBS.SaveChanges();
                        MessageBox.Show("Product Deleted");
                        BU_Lia_Clear_Click(null, null);
                        Load_Liability();

                    }
                    else return;
                }
                else MessageBox.Show("Select User to delete");
            }
        }
        #endregion
    }
}
