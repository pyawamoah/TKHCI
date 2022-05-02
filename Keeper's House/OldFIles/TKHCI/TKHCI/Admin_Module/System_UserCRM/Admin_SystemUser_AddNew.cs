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

namespace TKHCI
{
    public partial class Admin_SystemUser_AddNew : Form
    {
        KeeperDBEntities DBS = new KeeperDBEntities();
        private decimal ID;
        string gender = string.Empty;
        MemoryStream ms;
        public Admin_SystemUser_AddNew()
        {
            InitializeComponent();
            Keepers_LoadID();
        }

        private void Keepers_LoadID()
        {
            string constr = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(constr); 
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_SysUser_LoadID", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            sqlConnection.Close();


            if (xml< 1)
            {
                ID = 1;
                Tx_Empl_ID.Text = "TKHCS" + Convert.ToString(ID);
            }
            else if (xml >= 1)
            {
                ID = xml + 1;
                Tx_Empl_ID.Text = "TKHCS" + Convert.ToString(ID);
            }
            else return;
        }

        private void But_UploadImg_Click(object sender, EventArgs e)
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

        private void BT_ClearForm_Click(object sender, EventArgs e)
        {
            Keepers_LoadID();
            Tx_FName.Text = "";
            Tx_SName.Text = "";
            Tx_LName.Text = "";
            Tx_Phone.Text = "";
            Tx_City.Text = "";
            Tx_ZipCode.Text = "";
            Tx_Address.Text = "";
            Tx_Email.Text = "";
            LogUserImage.Image = null;
            Tx_NOK_Phone.Text = "";
            Combo_Dept.SelectedIndex = 0;
            Combo_SysRole.SelectedIndex = 0;
            ComBo_OffiCapacity.SelectedIndex = 0;
            Combo_Status.SelectedIndex = 0;
            Combo_MS.SelectedIndex = 0;
            Combo_Region.SelectedIndex = 0;
            Combo_Title.SelectedIndex = 0;
            Tx_Username.Text = "";
            Tx_NOK_Address.Text = "";
            Tx_NOK_Name.Text = "";
            Tx_NOK_Phone.Text = "";
            Combo_NOK_Relation.SelectedIndex = 0;
        }

        private void BT_AddNew_Click(object sender, EventArgs e)
        {
            UserTB acc = new UserTB();
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
                    if (Tx_Username.Text != "" && Tx_Password.Text != "" && Tx_RE_Password.Text != "")
                    {
                        if (Tx_Password.Text.ToString().Trim().ToLower() == Tx_RE_Password.Text.ToString().Trim().ToLower())
                        {
                            string UserName = Tx_Username.Text;
                            string Password = Cryptography.Encrypt(Tx_Password.Text.ToString());
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
                    if (Combo_Title.Text != "" && Tx_Address.Text != "" && Tx_City.Text != "" && Tx_Email.Text != "" && Tx_FName.Text != "" && Tx_LName.Text != ""
                        && Tx_NOK_Phone.Text != "" && Tx_SName.Text != "" && Tx_ZipCode.Text != "")
                    {
                        acc.Title = Combo_Title.Text;
                        acc.FName = Tx_FName.Text;
                        acc.MName = Tx_SName.Text;
                        acc.LName = Tx_LName.Text;
                        acc.DOB = Dt_DOB.Value.Date;
                        acc.Gender = gender;
                        acc.Phone = Tx_Phone.Text;
                        acc.City = Tx_City.Text;
                        acc.Zip_code = Tx_ZipCode.Text;
                        acc.Address = Tx_Address.Text;
                        acc.Region = Combo_Region.Text;
                        acc.Email = Tx_Email.Text;
                        acc.NOK_Phone = Tx_NOK_Phone.Text;
                        acc.Marrital_Status = Combo_MS.Text;
                        acc.Email = Tx_Email.Text;
                        acc.Dep = Combo_Dept.Text;
                        acc.SysRoll = Combo_SysRole.Text;
                        acc.OfficialCapacity = ComBo_OffiCapacity.Text;
                        acc.Member_Status = Combo_Status.Text;
                        acc.TKHS_ID = Tx_Empl_ID.Text;
                        //acc.UT_ID = Convert.ToDecimal(Tx_HideLoad.Text);
                        acc.JDate = DateTime.Now;
                        acc.Create_dt = DateTime.Now;
                        acc.CreatedBy = TKHCI_Main_Login.MainLogin.Username;
                        acc.Picture = ms.ToArray();
                        var fullname = new StringBuilder();
                        fullname.Append(Tx_FName.Text);
                        fullname.Append(" ");
                        fullname.Append(Tx_SName.Text);
                        fullname.Append(" ");
                        fullname.Append(Tx_LName.Text);
                        acc.Fullname = fullname.ToString();
                        acc.Pwd_lock = 0;
                        acc.Admin_loc = "Locked";
                        acc.NOKRela = Combo_NOK_Relation.Text;
                        acc.NOK_Address = Tx_NOK_Address.Text;
                        acc.NOK_Name = Tx_NOK_Name.Text;
                        acc.NOK_Phone = Tx_NOK_Phone.Text;

                        string message = "Are you sure you want to add this record?";
                        string title = "Adding Record...";
                        MessageBoxButtons but = MessageBoxButtons.YesNo;
                        DialogResult result = MessageBox.Show(message, title, but);
                        if (result == DialogResult.Yes)
                        {
                            DBS.UserTBs.Add(acc);
                            DBS.SaveChanges();
                            MessageBox.Show("User Created Successfully");
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

        private void BT_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
