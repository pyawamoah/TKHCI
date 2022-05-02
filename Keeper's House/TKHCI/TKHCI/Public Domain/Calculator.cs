using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TKHCI.Public_Domain
{
    public partial class Calculator : Form
    {
        public Calculator()
        {
            InitializeComponent();
            new Guna2ShadowForm(this);
        }
        public double LastAns { get; private set; }
        private int i = 0;
        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    if (keyData == Keys.Numpad0)
        //    {
        //        Numpad0.PerformClick();
        //        return true;
        //    }

        //    return base.ProcessCmdKey(ref msg, keyData);
        //}
        public static double Evaluate(string expression)
        {
            DataTable table = new DataTable();
            table.Columns.Add("expression", typeof(string), expression);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            return double.Parse((string) row["expression"]);
        }
        private string Expression = "";
        string GetRandomNums(double Ans)
        {
            this.LastAns = Ans;
            string temp = "";
            Random ran = new Random();
            for (int i = 0; i < Ans.ToString().Length; i++)
            {
                temp += ran.Next();
            }

            return temp;
        }

        private void TimerAnimation_Tick(object sender, EventArgs e)
        {
            if (i < 2)
            {
                displayAnswer.Text = GetRandomNums(this.LastAns);
                i++;
            }
            else
            {
                displayAnswer.Text = LastAns.ToString();
                i = 0;
                TimerAnimation.Stop();
            }
        }

        private void Bu_Equal_Click(object sender, EventArgs e)
        {
            if (displayExpression.Text != null)
            {
                LastAns = Evaluate(Expression);
                TimerAnimation.Start();
            }
            else return;
        }

        private void Bu_1_Click(object sender, EventArgs e)
        {
            displayExpression.Text = (Expression += ((Control)sender).Tag.ToString());
        }

        private void Bu_ShutDown_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Bu_Del_Click(object sender, EventArgs e)
        {
            try
            {
                displayExpression.Text = Expression = Expression.Substring(0, Expression.Length - 1);
            }
            catch (Exception)
            {

            }
        }
        public void ButtomOn()
        {
            Bu_ShutDown.Enabled = true;
            Bu_0.Enabled = true;
            Bu_1.Enabled = true;
            Bu_2.Enabled = true;
            Bu_3.Enabled = true;
            Bu_4.Enabled = true;
            Bu_5.Enabled = true;
            Bu_6.Enabled = true;
            Bu_7.Enabled = true;
            Bu_8.Enabled = true;
            Bu_9.Enabled = true;
            Bu_Dot.Enabled = true;
            Bu_Mod.Enabled = true;
            Bu_Del.Enabled = true;
            Bu_Clear.Enabled = true;
            Bu_BOpen.Enabled = true;
            Bu_BClose.Enabled = true;
            Bu_Div.Enabled = true;
            Bu_Mul.Enabled = true;
            Bu_Minus.Enabled = true;
            Bu_Plus.Enabled = true;
            Bu_Equal.Enabled = true;

        }
        public void ButtomOff()
        {
            Bu_ShutDown.Enabled = true;
            Bu_0.Enabled = false;
            Bu_1.Enabled = false;
            Bu_2.Enabled = false;
            Bu_3.Enabled = false;
            Bu_4.Enabled = false;
            Bu_5.Enabled = false;
            Bu_6.Enabled = false;
            Bu_7.Enabled = false;
            Bu_8.Enabled = false;
            Bu_9.Enabled = false;
            Bu_Dot.Enabled = false;
            Bu_Mod.Enabled = false;
            Bu_Del.Enabled = false;
            Bu_Clear.Enabled = false;
            Bu_BOpen.Enabled = false;
            Bu_BClose.Enabled = false;
            Bu_Div.Enabled = false;
            Bu_Mul.Enabled = false;
            Bu_Minus.Enabled = false;
            Bu_Plus.Enabled = false;
            Bu_Equal.Enabled = false;
        }
        private void Bu_Clear_Click(object sender, EventArgs e)
        {
            displayAnswer.Text = Expression = 0.ToString();
            displayExpression.Text = Expression = "";
        }

        private void Bu_ScreenOn_Click(object sender, EventArgs e)
        {
            if (Pn_RowOff.Visible == false)
            {
                Pn_RowOff.Visible = true;
                ButtomOff();

            }
            else if (Pn_RowOff.Visible == true)
            {
                Pn_RowOff.Visible = false;
                ButtomOn();
            }
            else
            {
                Pn_RowOff.Visible = false;
                ButtomOff();
            }
        }

        private void Calculator_Load(object sender, EventArgs e)
        {
            Pn_RowOff.Visible = true;
            ButtomOff();
        }
    }
}
