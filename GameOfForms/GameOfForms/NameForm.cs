using System;
using System.Windows.Forms;

namespace GameOfForms
{
    public partial class NameForm : Form
    {
        // The players name
        public static string PlayerName;

        /// <summary>
        /// The forms ctor
        /// </summary>
        public NameForm()
        {
            InitializeComponent();

            // Chaning form's title
            this.Text = "Choose Name";

            // Getting old values from registry
            object objNameReg = Application.UserAppDataRegistry.GetValue("Name");
            object objIpReg = Application.UserAppDataRegistry.GetValue("IP");

            // Checking that registry is not empty
            if (objNameReg != null && objIpReg != null)
            {
                // Changing player name to registry value
                PlayerName = objNameReg.ToString();

                // Writing the name from registry to the text box
                this.NameTB.Text = PlayerName;

                // Making the text not selected
                this.NameTB.Select(PlayerName.Length, 0);

                // Sending old ip adress we used and saved in reg to the ip form
                IpForm.IPAdress = objIpReg.ToString();
            }
        }

        /// <summary>
        /// Clicking the connect button sends you to the next form
        /// </summary>
        private void ConnectNameB_Click(object sender, EventArgs e)
        {
            // Checking if text box is empty
            if (NameTB.Text.Length == 0)
            {
                // Error message
                MessageBox.Show("You must enter a name", "Stupid", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (NameTB.Text.Length > 10)
            {
                // Error message
                MessageBox.Show("Your name must be 10 or less characters", "Silly",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (NameTB.Text.Contains(" "))
            {
                // Error message
                MessageBox.Show("You cant have spaces in your name", "Silly",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Getting name from text box
                PlayerName = NameTB.Text;

                // Hiding this form
                this.Hide();

                // Showing next form
                (new StartingItemsForm()).Show();
            }
        }
    }
}
