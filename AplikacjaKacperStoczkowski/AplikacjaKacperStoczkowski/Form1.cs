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
using System.Drawing.Imaging;
using static Aplikacja;

namespace AplikacjaKacperStoczkowski
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void plikToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();

            if (opf.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(opf.FileName);
                pictureBox1.Image = bmp;
            }
        }

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //pictureBox2.Image.Save(Application.StartupPath + "\\Image\\picture1.jpg", ImageFormat.Jpeg);
        }

        private void binaryzacjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Bitmap NewImage = Aplikacja.Binarization((Bitmap)pictureBox1.Image);
            //pictureBox2.Image = NewImage;
        }

        private void szkieletyzacjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Aplikacja obj = new Aplikacja();
            Bitmap NewImage = obj.Binarization((Bitmap)pictureBox1.Image);
            Bitmap poSzkieletyzacji = obj.Thinning(NewImage);
            pictureBox2.Image = poSzkieletyzacji;
        }
    }
}
