using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfForms
{
    public partial class StartingItemsForm : Form
    {
        // Right hand item
        public static string RightHandItem;

        // Left hand item
        public static string LeftHandItem;

        private Image imgClub = Image.FromFile(Application.StartupPath +
                                               @"/../../ClubOfClubing.png");

        private Image imgRock = Image.FromFile(Application.StartupPath +
                                               @"/../../RockOn.png");

        public StartingItemsForm()
        {
            InitializeComponent();

            // Changing form's top text
            this.Text = "Choose Starting Inventory";
        }

        /// <summary>
        /// Checking which item is choosed and displaying it
        /// </summary>
        private void RightHandListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (RightHandListBox.SelectedItem.ToString())
            {
                case ("Stick"):
                {
                    RightHandPictureBox.Image = imgClub;
                    
                    // WTF THIS DOESNT WORKKKKKKK
                    RightHandPictureBox.BackgroundImageLayout = ImageLayout.Stretch;

                    RightHandItemDesc.Text = "Club of clubbing is a \n" +
                                            "club you go clubbing \nwith";
                    break;
                }
                case ("Rock"):
                {
                    RightHandPictureBox.Image = imgRock;

                    // WTF THIS DOESNT WORKKKKKKK
                    RightHandPictureBox.BackgroundImageLayout = ImageLayout.Stretch;

                    RightHandItemDesc.Text = "Rock the fuck on B]";

                    break;
                }
                default:
                {
                    RightHandItemDesc.Text = "";

                    break;
                }
            }
        }

        /// <summary>
        /// Checking which item is choosed and displaying it
        /// </summary>
        private void LeftHandListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (LeftHandListBox.SelectedItem.ToString())
            {
                case ("Stick"):
                {
                    LeftHandPictureBox.Image = imgClub;

                    LeftHandPictureBox.BackgroundImageLayout = ImageLayout.Stretch;

                    LeftHandItemDesc.Text = "Club of clubbing is a \n" +
                                            "club you go clubbing \nwith";

                    break;
                }
                case ("Rock"):
                {
                    LeftHandPictureBox.Image = imgRock;

                    LeftHandPictureBox.BackgroundImageLayout = ImageLayout.Stretch;

                    LeftHandItemDesc.Text = "Rock the fuck on B]";

                    break;
                }
                default:
                {
                    LeftHandItemDesc.Text = "";

                    break;
                }
            }
        }

        /// <summary>
        /// Starting next form if inputs are valid
        /// </summary>
        private void AccpetButton_Click(object sender, EventArgs e)
        {
            if ((LeftHandListBox.SelectedItem == null) &&
                (RightHandListBox.SelectedItem == null))
            {
                MessageBox.Show("You must choose starting items!", 
                                "You left both items unmarked", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
            else if (LeftHandListBox.SelectedItem == null)
            {
                MessageBox.Show("You must choose a left hand item!", 
                                "Left hand item is unmarked",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (RightHandListBox.SelectedItem == null)
            {
                MessageBox.Show("You must choose a right hand item!",
                               "Right hand item is unmarked",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Getting the item
                RightHandItem = RightHandListBox.Text;

                // Getting the item
                LeftHandItem = LeftHandListBox.Text;

                // Closing this form
                this.Hide();

                // Showing next form
                (new IpForm()).Show();
            }
        }

        private void RightHandPictureBox_Click(object sender, EventArgs e)
        {

        }
    }
}
