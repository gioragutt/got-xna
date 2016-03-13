using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GameOfForms
{
    public partial class IpForm : Form
    {
        // The ip adress were connecting to
        public string strIPAdress;

        private string strName;

        private string strTeamName;

        /// <summary>
        /// The forms ctor
        /// </summary>
        public IpForm()
        {
            InitializeComponent();

            // Changing the forms title
            this.Text = "Game of Throws: Login";

            // Getting old values from registry
            object objNameReg = Application.UserAppDataRegistry.GetValue("Name");
            object objIpReg = Application.UserAppDataRegistry.GetValue("IP");

            // Checking that registry is not empty
            if (objNameReg != null && objIpReg != null)
            {
                // Changing player name to registry value
                strName = objNameReg.ToString();

                // Writing the name from registry to the text box
                this.txtName.Text = strName;

                // Making the text not selected
                this.txtName.Select(strName.Length, 0);

                // Sending old ip adress we used and saved in reg to the ip form
                strIPAdress = objIpReg.ToString();

                this.txtIp.Text = strIPAdress;
            }

            // Checking if already have an ip
            if (strIPAdress != null)
            {
                // Writing the ip to the text box
                this.txtIp.Text = strIPAdress;
                
                // Changing selecting of text to nothing is selected
                this.txtIp.Select(strIPAdress.Length, 0);
            }
        }

        /// <summary>
        /// Clicking the connect button connects you to the game
        /// </summary>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            // Checking if text box is empty
            if (txtIp.Text.Length == 0 || txtName.Text.Length == 0 || ((!this.radGOAT.Checked) && (!this.radHolyCow.Checked)))
            {
                // Showing message to user
                MessageBox.Show("Incorrect input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Getting ip adress from the text box
                strIPAdress = txtIp.Text;

                strName = txtName.Text;

                if (this.radGOAT.Checked)
                {
                    strTeamName = "1";
                }
                else
                {
                    strTeamName = "0";
                }

                // Trying to connect to the game
                try
                {
                    // Getting current process
                    Process CurrPros = Process.GetCurrentProcess();

                    // Creating game process
                    Process NewPros = new Process();

                    // Initializing the process with arguments and file name
                    NewPros.StartInfo.FileName = "Run.exe";
                    NewPros.StartInfo.Arguments = strName + " " + strIPAdress + " " + strTeamName;
                    NewPros.StartInfo.UseShellExecute = false;

                    // Writing current values to registry
                    Application.UserAppDataRegistry.SetValue("Name", strName);

                    // Starting game process
                    NewPros.Start();

                    Application.UserAppDataRegistry.SetValue("IP", strIPAdress);

                    // Killing this process
                    CurrPros.Kill();
                }
                // Catching exception from client
                catch
                {
                    MessageBox.Show("Error occurred while trying to connect to that ip",
                                    "Error :[");
                }
                // Telling user goot bye (wont happen if game starting fine => 
                // because then this process will be killed before the code gets here
                finally
                {
                    // Messagin
                    MessageBox.Show("faka u, tnx for playing", "faka u");
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtIp_TextChanged(object sender, EventArgs e)
        {

        }

        private void IpLabel_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process pro = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo("cmd.exe");
            start.Arguments = "/K ipconfig";
            pro.StartInfo = start;
            pro.Start();
        }
    }
}
