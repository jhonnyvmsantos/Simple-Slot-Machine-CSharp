using System;
using System.Collections.Generic;
using System.Drawing;
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
		
		PictureBox lever = new PictureBox();
		Timer loading = new Timer();
		
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

		}
		
		void TimerTickLoading(object sender, EventArgs e)
		{
			
		}
	}
}
