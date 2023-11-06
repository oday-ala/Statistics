using System;
using System.Drawing;
using System.Windows.Forms;

namespace app
{
    class Program
    {
        static Bitmap bitmap;
        static Graphics graphics;

        static void Main(string[] args)
        {
            // Create a new form
            Form form = new Form();
            form.Text = "Homework 1";
            form.ClientSize = new Size(400, 300);

            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            form.Controls.Add(pictureBox);

            Program.bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);

            Program.graphics = Graphics.FromImage(Program.bitmap);

            Program.graphics.Clear(Color.White);

            Program.graphics.FillRectangle(Brushes.Black, 25, 25, 2, 2);

            Program.graphics.DrawLine(Pens.Red, 50, 50, 140, 140);

            Program.graphics.DrawEllipse(Pens.Blue, form.ClientSize.Width / 2 - 50, form.ClientSize.Height / 2 - 50, 100, 100);

            Program.graphics.DrawRectangle(Pens.Green, form.ClientSize.Width - 100 - 10, form.ClientSize.Height - 50 - 10, 100, 50);

            pictureBox.Image = Program.bitmap;

            Application.Run(form);
        }
    }
}
