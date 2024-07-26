using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace simple_slot_machine
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}
		
		SoundPlayer rolling = new SoundPlayer("lever.wav");
		PictureBox lever = new PictureBox();
		Timer loading = new Timer();
		Random randomizer = new Random();
		
		int count = 0;
		
		void MainFormLoad(object sender, EventArgs e)
		{
			this.BackColor = Color.Green;
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			
			loading.Interval = 100;
			loading.Tick += TimerTickLoading;
			
			lever.Name = "lever"; 
			lever.Tag = "available";
			lever.SizeMode = PictureBoxSizeMode.StretchImage;
			lever.BackgroundImageLayout = ImageLayout.Stretch;
			lever.Width = 200; lever.Height = 200;
			lever.BackgroundImage = Image.FromFile("lever.gif");
			lever.Left = (this.Height / 2) + 225;
			lever.Top = (this.Height / 2) - 135;
			lever.Click += LeverClick;
			lever.Parent = this;
			
			for (int i = 0; i < 3; i++)
			{
				PictureBox dice = new PictureBox();
				dice.Name = "dice" + i.ToString(); 
				dice.Tag = i;
				dice.SizeMode = PictureBoxSizeMode.StretchImage;
				dice.Width = 100; dice.Height = 100;
				dice.Load("interrogation.png");
				dice.Left = 110 * i + 1 + 50;
				dice.Top = (this.Height / 2) - 70;
				dice.Parent = this;
			}
		}
		
		async void LeverClick(object sender, EventArgs e)
		{
			lever.BackgroundImage = null;
			rolling.Play();
			lever.Load("lever.gif");
			loading.Enabled = true;
			
			await Task.Delay(3500);
			lever.BackgroundImage = Image.FromFile("lever.gif");
			lever.Image = null;
		}
		
		void TimerTickLoading(object sender, EventArgs e)
		{
			count++;
			
			if (count < 35)
			{
				RollingDices();
			}
			else
			{
				loading.Enabled = false;
				count = 0;
			}
		}
		
		void RollingDices()
		{
			foreach (Control control in this.Controls)
		    {
				if (control is PictureBox && control.Name.Contains("dice") == true)
		        {
		            PictureBox dice = control as PictureBox;
		            dice.Load("dice" + randomizer.Next(1,7).ToString() + ".png");
		            
		            if (Convert.ToInt32(dice.Tag) == 2)
		            {
		            	break;
		            }
		        }
		    }
		}
	}
}
