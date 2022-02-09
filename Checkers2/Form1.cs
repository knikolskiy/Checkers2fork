using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region global
        bool player = true;
        Image ch_black = Properties.Resources._4;
        Image ch_white = Properties.Resources._5;
        int[,] matrix = new int[8, 8];
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "White";
            int size = panel1.Width / 8;
            for (int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    PictureBox pic = new PictureBox
                    {
                        Name = $"{x}_{y}",
                        BorderStyle = BorderStyle.FixedSingle,
                        Width = size,
                        Height = size,
                        Location = new Point(x * size, y * size),
                        BackColor = Color.White,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                    };
                    if(x % 2 == 1 && y % 2 == 0 || x % 2 == 0 && y % 2 == 1)
                    {
                        pic.BackColor = Color.Gray;
                        if (y < 3)
                        {
                            matrix[x, y] = 3;
                        }
                        else if (y > 4)
                        {
                            matrix[x, y] = 1;
                        }
                    }
                    pic.MouseClick += Pic_MouseClick;
                    panel1.Controls.Add(pic);
                }
            }
            draw();
        }

        private void draw()
        {
            for(int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    if (x % 2 == 1 && y % 2 == 0 || x % 2 == 0 && y % 2 == 1)
                    {
                        PictureBox pic = panel1.Controls[$"{x}_{y}"] as PictureBox;
                        pic.BackColor = Color.Gray;
                        switch (matrix[x, y])
                        {
                            case 0:
                                pic.Image = null;
                                break;
                            case 1:
                                pic.Image = ch_white;
                                break;

                            case 3:
                                pic.Image = ch_black;
                                break;
                            case 5:
                                pic.BackColor = Color.Green;
                                break;
                        }
                        if (matrix[x, y] < 0)
                        {
                            pic.BackColor = Color.Yellow;
                        }
                    }
                }
            }
        }

        private void Pic_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            string[] s = pic.Name.Split('_');
            int x = int.Parse(s[0]);
            int y = int.Parse(s[1]);
            label1.Text = $"Click on {pic.Name}";

            if (matrix[x, y] > 4)
            {
                move(x,y);
                return;
            }
            //old_pic = pic;
            if (player && matrix[x, y] > 0 && matrix[x, y] < 3)
            {
                right_click(pic, x,y);
            }
            else if (!player && matrix[x, y] > 2 && matrix[x, y] < 5)
            {
                right_click(pic, x,y);
            }
        }
        private void right_click(PictureBox pic, int x, int y)
        {
            clear_matrix();
            matrix[x, y] *= -1;
            steps(x,y);
            draw();
        }

        private void clear_matrix()
        {
            for(int x = 0; x < 8; x++)
            {
                for(int y=0; y < 8; y++)
                {
                    if (x % 2 == 1 && y % 2 == 0 || x % 2 == 0 && y % 2 == 1)
                    {
                        matrix[x, y] = Math.Abs(matrix[x, y]);
                        if(matrix[x,y] == 5)
                        {
                            matrix[x, y] = 0;
                        }
                    }
                }
            }
        }

        private void delete_chess()
        {
            
        }

        private void steps(int x, int y)
        {

            if(matrix[x,y] == -1 || matrix[x,y] == -2)
            {
                make_green(x - 1, y - 1);
                make_green(x + 1, y - 1);
            }
            if (matrix[x, y] == -3 || matrix[x, y] == -4)
            {
                make_green(x - 1, y + 1);
                make_green(x + 1, y + 1);
            }
        }

        private void make_green(int x, int y)
        {
            if (x >= 0 && x < 8 && y >= 0 && y < 8)
            {
                
                if (matrix[x,y] == 0)
                {
                    matrix[x, y] = 5;
                }
                else
                {
                    if (player && (matrix[x,y] == 3 || matrix[x,y] == 4))
                    {
                        make_red(x,y);
                    }
                    if (!player && (matrix[x, y] == 1 || matrix[x, y] == 2))
                    {
                        make_red(x,y);
                    }
                }
            }
        }
        private void make_red(int r_x,int r_y)
        {
            int x = 0, y = 0;
            for (; x < 8; x++)
            {
                for (y = 0; y < 8; y++)
                {
                    if (matrix[x, y] < 0)
                    {
                        x += 8;
                        break;
                    }
                }
            }
            x -= 9;
            x = x - r_x;
            y = y - r_y;
            matrix[x, y] = 6;
            //int delta_x = blocks[blocks.Count - 1].x - blocks[0].x;
            //int delta_y = blocks[blocks.Count - 1].y - blocks[0].y;
            //int x = blocks[blocks.Count - 1].x + delta_x;
            //int y = blocks[blocks.Count - 1].y + delta_y;
            //PictureBox pic = panel1.Controls[$"{x}_{y}"] as PictureBox;
            //Block b = new Block(pic, x, y);
            //blocks.Add(b);
            //if(pic.Image == null)
            //{
            //    pic.BackColor = Color.Red;
            //}
        }

        private void move(int g_x, int g_y)
        {
            delete_chess();
            int x = 0, y = 0;
            for(; x < 8; x++)
            {
                for(y=0; y < 8; y++)
                {
                    if(matrix[x,y] < 0)
                    {
                        x += 8;
                        break;
                    }
                }
            }
            x -= 9;
            matrix[g_x, g_y] = matrix[x, y];
            matrix[x, y] = 0;
            clear_matrix();
            draw();
            if (player)
            {
                player = false;
                toolStripStatusLabel1.Text = "Black";
            }
            else
            {
                player = true;
                toolStripStatusLabel1.Text = "White";
            }
        }
    }
}
