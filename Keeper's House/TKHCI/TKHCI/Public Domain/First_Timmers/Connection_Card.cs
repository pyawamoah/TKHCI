using NHibernate.Engine;
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

namespace TKHCI.Public_Domain.First_Timmers
{
    public partial class Connection_Card : Form
        
    {
        private readonly string loadMembers = "Select 'Select Who invited you' as Member union all SELECT distinct MembTB.Fullname FamilyName from MembTB with (nolock)";
        KeeperDBEntities DBS = new KeeperDBEntities();
        decimal ID;
        string serviceType;
        string WishToJoin;
        string flyer;
        string billboard;
        string socialmedia;
        string visit;
        string K_Jesus;
        string Talk_Pastor;
        string knowKHC;
        string KnowBapt;
        string churchWorker;
        decimal awardCount = 0;


        public static class UserSession
        {
            public static string Username;
        }
        public Connection_Card()
        {
            InitializeComponent();
            tmr.Start();
            _ = page_Load();
        }

        private void BT_Add_Card_Click(object sender, EventArgs e)
        {
            ConnectCardTB conCard = new ConnectCardTB();
            Invited_GuestTB invited_GuestTB = new Invited_GuestTB();
            if (RD_MWS.Checked)
            {
                serviceType = "Midweek Service";

            }
            else if (RD_MF.Checked)
            {
                serviceType = "Morning Flavour";
            }
            else if (RD_SS.Checked)
            {
                serviceType = "Sunday Service";
            }

            if (RD_BM.Checked)
            {
                WishToJoin = "Become a Member";
            }else if (RD_RRA.Checked)
            {
                WishToJoin = "Remain regular attendent";
            }else if (RD_AOV.Checked)
            {
                WishToJoin = "Am only visiting for today";
            }

            if (CB_BR_Jesus.Checked)
            {
                flyer = "Flyer";
            }

            if (CB_Invited.Checked)
            {
                visit = "Invited";
            }
            if (CB_Billboard.Checked)
            {
                billboard = "Billboard";
            }
            if (CB_Social.Checked)
            {
                socialmedia = "Social Media";
            }

            if (CB_BR_Jesus.Checked)
            {
                K_Jesus = "Beginning a relationship with Jesus";
            }
            if (CB_Talk_P.Checked)
            {
                Talk_Pastor = "Talking to a pastor";
            }
            if (CB_Get_Info_Keepers.Checked)
            {
                knowKHC = "Getting information about the Keepers House Chapel Int";
            }
            if (CB_Get_Info_Baptism.Checked)
            {
                KnowBapt = "Getting information on baptism";
            }
            if (CB_Church_Worker.Checked)
            {
                churchWorker = "Becoming a church worker";
            }

            conCard.Interested_IN1 = K_Jesus;
            conCard.Interested_IN2 = Talk_Pastor;
            conCard.Interested_IN3 = knowKHC;
            conCard.Interested_IN4 = churchWorker;
            conCard.Interested_IN5 = KnowBapt;


            conCard.King_Of_Service = serviceType;
            conCard.WishTo = WishToJoin;

            conCard.HearAt_US1 = flyer;
            conCard.HearAt_US2 = visit;
            conCard.HearAt_US3 = billboard;
            conCard.HearAt_US4 = socialmedia;

            conCard.ConCard_NO = Tx_Card_No.Text;
            conCard.Email = Tx_Email.Text;
            conCard.Address = Tx_Address.Text;
            conCard.CreatedBY = TKHCI_Main_Login.MainLogin.Username;
            conCard.Sname = Tx_SName.Text;
            conCard.Occupation = Tx_Job.Text;
            conCard.DOB = Dt_DOB.Value.Date;
            conCard.Contact_No = Tx_Contact.Text;
            conCard.Prayer_Rq = Tx_PrayRq.Text;
            conCard.InvitedBY = Tx_Invited_BY.Text;
            conCard.Member_ID = Tx_MemID.Text;

            var fullname = new StringBuilder();
            fullname.Append(Tx_OName.Text);
            fullname.Append(" ");
            fullname.Append(Tx_SName.Text);
            conCard.Fullname_Guest = fullname.ToString();

 
            invited_GuestTB.Ivite_Count = awardCount;
            decimal totalAward = awardCount + 1;

            invited_GuestTB.Name_of_Guest = fullname.ToString();
            invited_GuestTB.Ivite_Count = totalAward;
            invited_GuestTB.TKC_MemID = Tx_MemID.Text;
            invited_GuestTB.Name_of_Who_InviteedGuest = Tx_Invited_BY.Text;
            invited_GuestTB.CreateDT = DateTime.Now;
         

            string message = "Are you sure you want to add this record?";
            string title = "Adding Record...";
            MessageBoxButtons but = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, but);
            if (result == DialogResult.Yes)
            {
                DBS.ConnectCardTBs.Add(conCard);
                DBS.SaveChanges();

                DBS.Invited_GuestTB.Add(invited_GuestTB);
                DBS.SaveChanges();
                MessageBox.Show("User Created Successfully");
                Load_From_Table_New_Data();
                BT_Clear_Form_Click(null,null);



            }
            else return;

            
          


        }
        public async Task page_Load()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Keepers_LoadID());
            tasks.Add(Load_Combo_Box());
            await Task.WhenAll(tasks);
        }

        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        public object PopulateDataGridView()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_LoadSearch_MemberConCard]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", txtSearch_By_Name.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public object PopulateDataGridView2()
        {
            DataTable dt = new DataTable();
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("[usp_LoadSearch_MemberConCardLoad]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchText", txtSearch_By_Name.Text.Trim());
            command.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(dt);
            return dt;
        }
        public void Load_From_Table_New_Data()
        {
            DataGrib_ReqType.DataSource = this.PopulateDataGridView2();
        }
        private void txtSearch_By_Name_TextChanged(object sender, EventArgs e)
        {
            DataGridView.DataSource = this.PopulateDataGridView();
        }
        private void Connection_Card_Load(object sender, EventArgs e)
        {
            DataGridView.DataSource = this.PopulateDataGridView();
            Load_From_Table_New_Data();
        }
        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtUsername_Hide.Text = DataGridView.SelectedRows[0].Cells[0].Value.ToString();
            UserSession.Username = txtUsername_Hide.Text;
            Load_Combo_Box();
        }
        private Task Load_Combo_Box()
        {
            var user1 = DBS.MembTBs.FirstOrDefault(a => a.TKC_MemID.Equals(txtUsername_Hide.Text));
            if (user1 != null)
            {
                Tx_Invited_BY.Text = user1.Fullname;
                Tx_MemID.Text = user1.TKC_MemID;
            }


            return Task.CompletedTask;
        }
        private void DataGrib_Zone_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {



        }

        private Task Keepers_LoadID()
        {
            Con.Open();
            SqlCommand command = new SqlCommand("[usp_Con_Card_LoadID]", Con);
            command.CommandType = CommandType.StoredProcedure;
            var xml = (decimal)command.ExecuteScalar();
            Con.Close();


            if (xml < 1)
            {
                ID = 1;
                Tx_Card_No.Text = $"KCC-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            else if (xml >= 1)
            {
                ID = xml + 1;
                Tx_Card_No.Text = $"KCC-{DateTime.Now.Year}-" + Convert.ToString(ID);
            }
            return Task.CompletedTask;

        }
        private void InsertDATA()
        {
            string connection = @"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True";
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("usp_KH_Create_ConnectCard", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CardNum", Tx_Card_No.Text);
            command.Parameters.AddWithValue("@SurName", Tx_SName.Text);
            command.Parameters.AddWithValue("@OName", Tx_OName.Text);
            command.Parameters.AddWithValue("@ContactNo", Tx_Contact.Text);
            command.Parameters.AddWithValue("@Address", Tx_Address.Text);
            command.Parameters.AddWithValue("@Email", Tx_Email.Text);
            command.Parameters.AddWithValue("@Occupation", Tx_Job.Text);
            command.Parameters.AddWithValue("@Dob", Dt_DOB.Value);
            command.Parameters.AddWithValue("@KingOfService", Tx_Card_No.Text.Trim());
            command.Parameters.AddWithValue("@WishTo", Tx_Card_No.Text.Trim());
            command.Parameters.AddWithValue("@HeatAt_US1", CB_Flyer.Checked ? "Flyer" : null);
            command.Parameters.AddWithValue("@HeatAt_US2", CB_Invited.Checked ? "Invited" : null);
            command.Parameters.AddWithValue("@HeatAt_US3", CB_Billboard.Checked ? "Billboard" :  null);
            command.Parameters.AddWithValue("@HeatAt_US4", CB_Social.Checked ? "Social Media" : null);
            command.Parameters.AddWithValue("@Interested_IN1", CB_BR_Jesus.Checked ? "Beginning a relationship with Jesus" : null);
            command.Parameters.AddWithValue("@Interested_IN2", CB_Talk_P.Checked ? "Talking to a pastor" : null);
            command.Parameters.AddWithValue("@Interested_IN3", CB_Get_Info_Keepers.Checked ? "Getting information about the Keepers House Chapel Int" : null);
            command.Parameters.AddWithValue("@Interested_IN4", CB_Get_Info_Baptism.Checked ? "Getting information on baptism" :null);
            command.Parameters.AddWithValue("@Interested_IN5", CB_Church_Worker.Checked ? "Becoming a church worker" :null);

            command.Parameters.AddWithValue("@King_Of_Service", RD_MWS.Checked ? " Midweek Service" : null);
            command.Parameters.AddWithValue("@King_Of_Service", RD_MF.Checked ? "Morning Flavour" : null);
            command.Parameters.AddWithValue("@King_Of_Service", RD_SS.Checked ? "Sunday Service" : null); 
            command.Parameters.AddWithValue("@WishTo", RD_BM.Checked ? "Become a member" : null);
            command.Parameters.AddWithValue("@WishTo", RD_RRA.Checked ? "Remain regular attendent" : null);
            command.Parameters.AddWithValue("@WishTo", RD_AOV.Checked ? "Am only visiting for today" : null);


            command.Parameters.AddWithValue("@Prayer_Rq", Tx_PrayRq.Text);
            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            command.Parameters.AddWithValue("@CreatedBy", TKHCI_Main_Login.MainLogin.Username);

            command.ExecuteNonQuery();
        }



        private void tmr_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            DateTimeDisplay.Text = DateTime.Now.ToString();
            
        }



        private void BT_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BT_Clear_Form_Click(object sender, EventArgs e)
        {

            Tx_Email.Text = "";
            Tx_Address.Text = "";
            Tx_SName.Text = "";
            Tx_Job.Text = "";
            Dt_DOB.Value = DateTime.Now;
            Tx_PrayRq.Text = "";
            Tx_Invited_BY.Text = "";
            Tx_MemID.Text = "";
            Tx_Contact.Text = "";

            CB_Flyer.Checked = false;
            CB_Invited.Checked = false;
            CB_Billboard.Checked = false;
            CB_Social.Checked = false;
            CB_BR_Jesus.Checked = false;
            CB_Talk_P.Checked = false;
            CB_Get_Info_Keepers.Checked = false;
            CB_Get_Info_Baptism.Checked = false;
            CB_Church_Worker.Checked = false;
            RD_MWS.Checked = false;
            RD_MF.Checked = false;
            RD_SS.Checked = false;
            RD_BM.Checked = false;
            RD_RRA.Checked = false;
            RD_AOV.Checked = false;
        }


    }
}
