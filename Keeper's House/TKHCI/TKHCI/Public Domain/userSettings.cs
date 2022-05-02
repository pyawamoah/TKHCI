using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TKHCI.Public_Domain
{
    public partial class userSettings : Form
    {
        #region Form Open Add's
        KeeperDBEntities DBS = new KeeperDBEntities();
        MemoryStream ms;
        MemoryStream Fams;
        #endregion
        public userSettings()
        {
            InitializeComponent();
        }
        #region FORM LOAD MAIN
        private void userSettings_Load(object sender, EventArgs e)
        {
            #region Basic Info
            var user = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(TKHCI_Main_Login.MainLogin.Username));
            if (user != null)
            {
                Tx_Info_Name.Text = user.Fullname;
                byte[] img = user.Picture;
                MemoryStream ms = new MemoryStream(img);
                LogImage_User_Info.Image = Image.FromStream(ms);
                User_Image_Main.Image = Image.FromStream(ms);
                Tx_Info_Email.Text = user.Email;
                Tx_UserName.Text = user.Fullname;
                Tx_User_Dept.Text =user.Dep + " Department";
                Tx_User_Official_Role.Text = user.OfficialCapacity;
            }
            #endregion

            #region Family information
            var famName = DBS.FamTBs.FirstOrDefault(a => a.Fam_Name.Equals(TKHCI_Main_Login.familyInfo.famInfo));
            if (famName != null)
            {
                Tx_Fam_U_Name.Text = "Hello " + user.FName + ", ";
                byte[] imgFam = famName.Fam_Pic;
                MemoryStream Fams = new MemoryStream(imgFam);
                LogFam_Image.Image = Image.FromStream(Fams);


                Tx_Fam_Name.Text = famName.Fam_Name;
                Tx_Fam_Greetings.Text = famName.Fam_Greetings;

                Tx_Husband_Name.Text = famName.Husb_Name;
                Tx_Wife_Name.Text = famName.Wife_Name;

                Combo_NoOf_Chlld.Text = Convert.ToString(famName.No_Of_Child);
                Tx_Fam_Address.Text = famName.Address;
                Tx_Fam_Phone.Text = famName.Home_Phone;
                Tx_F_City.Text = famName.City;
                Combo_Region.Text = famName.Region;
                Tx_Child1.Text = famName.Name_Of_Child1;
                Tx_Child2.Text = famName.Name_Of_Child2;
                Tx_Child3.Text = famName.Name_Of_Child3;

                Tx_Child4.Text = famName.Name_Of_Child4;
                Tx_Child5.Text = famName.Name_Of_Child5;
                Tx_Child6.Text = famName.Name_Of_Child6;

            }
            #endregion
        }

        #endregion

        #region Basic Infor Load & Update Image
        private void BU_Browse_Image_Click(object sender, EventArgs e)
        {
            OpenFileDialog openbdlg = new OpenFileDialog();
            if (openbdlg.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openbdlg.FileName);
                LogImage_User_Info.Image = img;
                ms = new MemoryStream();
                img.Save(ms, img.RawFormat);

            }
        }
        private void BU_Update_Image_Click(object sender, EventArgs e)
        {
            var cat = (from s in DBS.UserTBs where s.Username == TKHCI_Main_Login.MainLogin.Username select s).FirstOrDefault();
            var img = LogImage_User_Info.Image;
            if (ms != null)
            {
                img.Save(ms, img.RawFormat);
                cat.Picture = ms.ToArray();
            }
            string message = "Are you sure you want to update your image?";
            string title = "updating image...";
            MessageBoxButtons but = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, but);
            if (result == DialogResult.Yes)
            {
                DBS.SaveChanges();
                MessageBox.Show("Image Updated Successfully");
                userSettings_Load(null, null);
            }
            else return; 
        }


        #endregion

        #region Load Image for Family
        private void BU_Browse_Fam_Img_Click(object sender, EventArgs e)
        {
            OpenFileDialog openbdlg = new OpenFileDialog();
            if (openbdlg.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openbdlg.FileName);
                LogFam_Image.Image = img;
                ms = new MemoryStream();
                img.Save(ms, img.RawFormat);

            }

        }

        #endregion

        #region Update Family info
        private void BU_Update_Info_Click(object sender, EventArgs e)
        {
            string UpdateFam = TKHCI_Main_Login.familyInfo.famInfo;
            var updt = (from s in DBS.FamTBs where s.Fam_Name == UpdateFam select s).FirstOrDefault();

            if (updt != null && Tx_Fam_Address.Text != "" && LogFam_Image.Image !=null)
            {
                updt.Address = Tx_Fam_Address.Text;
                updt.City = Tx_F_City.Text;             
                updt.Fam_Greetings = Tx_Fam_Greetings.Text;
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
                updt.Region = Combo_Region.Text;
                var img = LogFam_Image.Image;
                if (ms != null)
                {
                    img.Save(ms, img.RawFormat);
                    updt.Fam_Pic = ms.ToArray();
                }
                updt.CreatedBy = TKHCI_Main_Login.MainLogin.Username;

                string message = "Are you sure you want to update this record?";
                string title = "updating Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {

                    DBS.SaveChanges();
                    MessageBox.Show("Family Record Updated Successfully");
                    userSettings_Load(null, null);
                }
                else return;
            }
            else
            {
                MessageBox.Show("Select a family to update");
            }
        }
        #endregion

        #region Form Open Add's
        #endregion

        #region Form Open Add's
        #endregion

        #region Form Open Add's
        #endregion

        #region Form Open Add's
        #endregion

        #region Form Open Add's
        #endregion


    }
}
