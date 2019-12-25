using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Shi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StratGame();
        }
        private string sentance="";
        private int error = 0;
        private IEnumerable<char> chosenChars = new List<char>();
        private Random random = new Random();
        int n;
        List<Image> obrazky = new List<Image>();
        private void StratGame()
        {
            n = sentance.Count();
            sentance = GenSentence();
            chosenChars = new List<char>();
            error = 0;
            lblVeta.Text = GetMAsk();
            LoadPicture(error);
            
            ShowBtns();

        }
        private string GenSentence()
        {
            string fn = GetAppDir + @"\\sen.txt";
            if (File.Exists(fn))
            {
                string[] snt = File.ReadAllLines(fn,Encoding.UTF8);
                n = snt.Count();
                if(n>0)
                {
                    int i = 0;
                    i = random.Next(1, n);
                    return snt[i].ToUpper();
                }
               
            }
            else MessageBox.Show(String.Format("File{0} not found", fn));
            return "";
        }
        private void ShowBtns()
        {
            foreach(Control cr in pnlButtons.Controls)
            {
                if (cr is Button) (cr as Button).Visible = true;
            }
        }
        private string GetMAsk()
        {
            string mask = "";
            foreach(char z in sentance )
            {
                if(z == ' ' || chosenChars.Contains(z))
                {
                    mask += z;
                }
                else 
                {
                    mask += '*';
                }
            }
            return mask;
        }
        private bool Win()
        {
            foreach (char z in sentance)
            {
                if (z != ' ' && !chosenChars.Contains(z))
                {
                    return false;
                }
            }    
          return true;  
        }
        private IList<char> GetCharList(char c)
        {
            List<char> list = new List<char>();
            list.Add(c);
            switch(c)
            {
                case 'I':
                {
                    list.Add('Í');
                        break;
                }
                case 'E':
                {
                     list.Add('É');
                     list.Add('Ě');
                     break;
                }
                case 'Y':
                {
                     list.Add('Ý');
                     break;
                }
                case 'A':
                {
                    list.Add('Á');
                    break;
                }
                case 'O':
                {
                    list.Add('Ó');
                    break;
                }
                case 'U':
                {
                    list.Add('Ú');
                    list.Add('Ů');
                    break;
                }
            }
            return list;
        }
        private bool Hit(IList<char> list)
        {
            foreach(char a in sentance)
            {
                if (list.Contains(a)) return true;
            }
             return false;   
        }
        private string GetAppDir
        {
            get
            {
                FileInfo fi = new FileInfo(Application.ExecutablePath);
                return fi.DirectoryName;
            }
        }
        private bool LoadPicture(int i)
        {
            string dir = GetAppDir + @"\pics\";
            string fn = dir + i.ToString().PadLeft(2, '0') + "bmp";
            if (File.Exists(fn))
            {
                pcbPicture.Image = new Bitmap(fn);
                return true;
            }
            return false;
        }
        private void GameOver()
        {
            MessageBox.Show("Game Over!!!\nSentence was: "+sentance);
            StratGame();
        }
        private void Miss()
        {
            ++error; //index
            if(error<=11)
            {
                foreach(Image obr in imageList1.Images)
                {
                    obrazky.Add(obr);
                }
                pcbPicture.Image = obrazky[error];
                if (error == 11) //index == 11 hra skončila
                {
                    MessageBox.Show("You LOST!");
                    GameOver();
                }

            }
        }
        private void GameWiner()
        {
            MessageBox.Show("Win!!");
            StratGame();
        }
        private void BtnA_Click(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            if(btn!=null)
            {
                char c = btn.Text[0];
                IList<char> list = GetCharList(c);
                bool trefa = Hit(list);
                chosenChars = chosenChars.Concat(list);
                if (trefa)
                {
                    lblVeta.Text = GetMAsk();
                    bool vitez = Win();
                    if (vitez) GameWiner();
                }
                else
                {
                    Miss();
                }
                    
            }
        }
    }
}
