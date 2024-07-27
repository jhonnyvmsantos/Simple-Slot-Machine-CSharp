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
		
		Label wallet = new Label();
		Label bonus = new Label();
		Label cost = new Label();
		Label reward = new Label();
		
		int count = 0, bcount = 0, money = 500, payoff = 250, price = 10, attempts = 0, multiplier = 1;
		
		int[] results = new int[3];
		
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
			lever.Left = (this.Width / 2) + 115;
			lever.Top = (this.Height / 2) - 135;
			lever.Click += LeverClick;
			lever.Parent = this;
			
			wallet.AutoSize = true; LabelTextChanges("wallet");
			wallet.Font = new Font(FontFamily.GenericSerif, 21f);
			wallet.Left = 15; wallet.Top = 25;
			wallet.BorderStyle = BorderStyle.Fixed3D;
			wallet.Parent = this;
			
			bonus.AutoSize = true; LabelTextChanges("bonus");
			bonus.Font = new Font(FontFamily.GenericSerif, 14f);
			bonus.Left = 15; bonus.Top = this.Height - 75;
			bonus.Parent = this;
			
			cost.AutoSize = true; LabelTextChanges("cost");
			cost.Font = new Font(FontFamily.GenericSerif, 17f, FontStyle.Bold);
			cost.Left = 180; cost.Top = (this.Height / 2) + 50;
			cost.Parent = this;
			
			reward.AutoSize = true; LabelTextChanges("reward");
			reward.Font = new Font(FontFamily.GenericSerif, 14f);
			reward.Left = this.Width - 200; reward.Top = 15;
			reward.Parent = this;
			
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
			if (loading.Enabled == false)
			{
				lever.BackgroundImage = null;
				rolling.Play();
				lever.Load("lever.gif");
				loading.Enabled = true;
				
				await Task.Delay(1500);
				lever.BackgroundImage = Image.FromFile("lever.gif");
				lever.Image = null;
			}
		}
		
		void TimerTickLoading(object sender, EventArgs e)
		{
			count++;
			
			if (count < 15)
			{
				RollingDices();
			}
			else
			{
				loading.Enabled = false;
				count = 0;
				
				if (results[0] == results[1] && results[0] == results[2])
				{
					MessageBox.Show("Parabéns!!! Você conseguiu 3 iguais.\nVocê ganhou R$ " + (payoff).ToString() + ",00.");
					money += payoff;
				}
				else
				{
					money -= price;
					
					if (money <= 0)
					{
						MessageBox.Show("Que pena... Você zerou sua conta.\nBoa sorte na próxima tentativa.");
						Application.Restart();
					}					
				}
				
				if (money >= 1000 && money < 3000 && bcount == 0)
				{
					MessageBox.Show("Parabéns!!! Você já conseguiu o dobro do valor inicial!\nGaroto(a) de sorte, hein.");
					bcount++;
				}
				else if (money >= 3000)
				{
					MessageBox.Show("Okay... Você é bem ganancioso! Está causando nossa falência.\nDito isso, volte para a estaca zero. XD");
					Application.Restart();
				}
				
				if (attempts == 10 || attempts == 20 || attempts == 30 || attempts == 40)
				{
					MessageBox.Show("Ao que parece, você é bem obstinado. Enfim, o multiplcador de recompensa e preço, por tentativa, aumentou em 1!");
					
					multiplier += 1;
					price = 10 * multiplier;
					payoff = 250 * multiplier;
					
					string[] elements = new string[3]
					{
						"bonus", "cost", "reward"
					};
					
					foreach (string element in elements)
					{
						LabelTextChanges(element);
					}
				}
				
				attempts++;
				LabelTextChanges("wallet");
			}
		}
		
		void LabelTextChanges(string type)
		{
			switch (type)
			{
				case "wallet":
					wallet.Text = "Carteira (R$): " + money.ToString() + ",00";
					break;
				case "bonus":
					bonus.Text = "Multiplicador: " + multiplier.ToString() + "x";
					break;
				case "cost":
					cost.Text = "$ " + price.ToString();
					break;
				case "reward":
					reward.Text = "Recompensa: " + payoff.ToString() + ",00";
					break;
			}
		}
		
		void RollingDices()
		{
			foreach (Control control in this.Controls)
		    {
				if (control is PictureBox && control.Name.Contains("dice") == true)
		        {
		            PictureBox dice = control as PictureBox;
		            int number = randomizer.Next(1,7);
		            int tag = Convert.ToInt32(dice.Tag);
		            
		            dice.Load("dice" + number.ToString() + ".png");
		            results[tag] = number;
		            
		            if (tag == 2)
		            {
		            	break;
		            }
		        }
		    }
		}
	}
}
