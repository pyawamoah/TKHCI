using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TKHCI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TKHCI_Main_Login());
            //Application.Run(new Cell_Module.Dashboard.Cell_Dashboard());
            //Application.Run(new Public_Domain.First_Timmers.Connection_Card());
            //Application.Run(new Admin_Dashbaord());
            //Application.Run(new Public_Domain. userSettings());
            //Application.Run(new Public_Domain.User_Information_DOC());
            //Application.Run(new Admin_Module.Member_CRM.Member_CRM_MAIN());
            //Application.Run(new Admin_SystemUser_AddNew());
            //Application.Run(new Finance.GeneralPayment());
            ////Application.Run(new VisualFinance.Finance_Graph());
            //Application.Run(new Finance.Dashboard());
            //Application.Run(new Reports.Reports());


        }
    }
}
