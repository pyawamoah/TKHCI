using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TKHCI.TKHCI_ForgotPass_Username_SecondPage;

namespace TKHCI
{
    public partial class TKHCI_ForgotPass_SQ_Answers_ThirdPage : Form
    {
        KeeperDBEntities DBS = new KeeperDBEntities();
        public TKHCI_ForgotPass_SQ_Answers_ThirdPage()
        {
            InitializeComponent();
        }

        private void BT_Next_Click(object sender, EventArgs e)
        {
            var user1 = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(CheckUsername.Mainusername));
            if (user1.Secu_quest_answer1 == txtAnswerOne.Text && user1.Secu_quest_answer2 == txtAnswerTwo.Text && user1.Secu_quest_answer3 == txtAnswerThree.Text)
            {
                TKHCI_ForgotPass_Set_Password chgPass = new TKHCI_ForgotPass_Set_Password();
                this.Hide();
                chgPass.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("check security question answer");
            }
        }

        private void TKHCI_ForgotPass_SQ_Answers_ThirdPage_Load(object sender, EventArgs e)
        {
            var user1 = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(CheckUsername.Mainusername));
            if (!String.IsNullOrEmpty(TKHCI_ForgotPass_Username_SecondPage.CheckUsername.Mainusername))
            {

                if (user1 != null)
                {
                    txtQuestion1.Text = user1.Security_question1;
                    txtQuestion2.Text = user1.Security_question2;
                    txtQuestion3.Text = user1.Security_question3;
                }
            }
        }
    }
}
