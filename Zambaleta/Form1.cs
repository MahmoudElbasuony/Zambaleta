using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zambaleta
{
    public partial class Form1 : Form
    {
        List<Thread> workers = new List<Thread>();
        Graphics g;
        Random r = new Random();
        object lock_object = new object();

        public Form1()
        {
            InitializeComponent();
            g = CreateGraphics();
            BackColor = Color.LimeGreen;
            TransparencyKey = BackColor;
        }


        void DrawCircles()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                int pen_width = r.Next(Width / 15);
                int red = ((int)(r.NextDouble() * 255));
                int green = ((int)(r.NextDouble() * 255));
                int blue = ((int)(r.NextDouble() * 255));
                int x = ((int)(r.NextDouble() * Screen.PrimaryScreen.Bounds.Width));
                int y = ((int)(r.NextDouble() * Screen.PrimaryScreen.Bounds.Height));
                int circle_radius = r.Next(Width / 15);
                Color color = Color.FromArgb(red, green, blue);
                lock (lock_object)
                    g.DrawEllipse(new Pen(color, pen_width), new Rectangle(x, y, circle_radius, circle_radius));
                Thread.Sleep(100);
            }
        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread worker = new Thread(DrawCircles);
                worker.IsBackground = false;
                worker.Start();
                workers.Add(worker);
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            workers.ForEach(w => { w.Abort(); });
            workers.Clear();
            g.Dispose();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
