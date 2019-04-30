using System;
using System.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Krestiki_noliki_ver._3._1
{
    public partial class Form1 : Form
    {
        Random Rand = new Random();

        // Поле из 9 картинок
        PictureBox[] GamePole = new PictureBox[9];
        
        int    player, computer,                       // Выбор игрока или компьютера
               k = 0;                                  // Счётчик для кнопки с музыкой
        SoundPlayer sp = new SoundPlayer("Меню.wav"),
                    sm = new SoundPlayer("Игра.wav");                

        // Игровое поле для просчёта выигрыша 
        int[] GamePoleMas =
                        {
                            0,0,0,
                            0,0,0,
                            0,0,0
                        };

        // Имена картинок, используемых в игре
        string[] ImgName =
        {
           "Пустое поле.jpg", "Крестик.jpg", "Нолик.jpg"
        };

        public Form1()
        {
            // Верхнее меню
            InitializeComponent();
            ToolStripMenuItem startItem = new ToolStripMenuItem("Игра") { Checked = true, CheckOnClick = true };
            startItem.Click += startItem_Click;
            menuStrip1.Items.Add(startItem);

            ToolStripMenuItem backItem = new ToolStripMenuItem("Назад");
            backItem.Click += backItem_Click;
            menuStrip1.Items.Add(backItem);
  
            ToolStripMenuItem halpItem = new ToolStripMenuItem("Помощь") { Checked = true, CheckOnClick = true };
            halpItem.Click += halpItem_Click;
            menuStrip1.Items.Add(halpItem);

            ToolStripMenuItem aboutItem = new ToolStripMenuItem("Об игре") { Checked = true, CheckOnClick = true };
            aboutItem.Click += aboutItem_Click;           
            menuStrip1.Items.Add(aboutItem);
        }

        void startItem_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
        }

        void backItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
        }

        void halpItem_Click(object sender, EventArgs e)
        {
            Process.Start("Помощь.txt");
        }

        void aboutItem_Click(object sender, EventArgs e)
        {
            Process.Start("Об игре.txt");
        }

        // Функция рисует поле игрока
        void MainPole()
        {
            // Начало построения поля
            int
            ox = 0,
            oy = 0;

            //Размеры картинки
            int
            HeightP = 95,       // Высота
            WidthP = 100,       // Ширина
            indexPicture = 0;   // Счётчик количества картинок

            // Имя в ячейке начинается с "Р_"
            string Name = "P_";

            // цикл формирования игрального поля по высоте
            for (int yy = 0; yy < 3; yy++)
            { 
                    // цикл формирования игрального поля по ширине
                    for (int xx = 0; xx < 3; xx++)
                    {
                        GamePole[indexPicture] = new PictureBox()
                        {
                            Name = Name + indexPicture,                 // Имя картинки
                            Height = HeightP,                           // Высота
                            Width = WidthP,                             // Ширина
                            Image = Image.FromFile("Пустое поле.jpg"),  // Загрузка пустого поля
                            SizeMode = PictureBoxSizeMode.StretchImage, // Подводим размер картинки к размеру поля
                            Location = new Point(ox, oy)                  // Размещение картинки
                        };

                    GamePole[indexPicture].Click += Picture_Click;

                        panel4.Controls.Add(GamePole[indexPicture]);               // Размещение картинки на панели управления
                        indexPicture++;                                 // Изменение имени после каждого цикла
                        ox += WidthP;                                   // Рассчитывание координаты следующей картинки по ширине
                    }
                oy += HeightP;                                   // Рассчитывание координаты следующей картинки по высоте
                ox = 0;                                          // Обнуление значения Х
            }
          }

        // Функция блокировки поля
        void LookPole()
        {
            foreach (PictureBox P in GamePole)
                P.Enabled = false;
        }

        // Функция разблокировки поля
        void UnLookPole()
        {
            int indexx = 0;
            foreach (PictureBox P in GamePole)
            {
                if (GamePoleMas[indexx++] == 0)
                    P.Enabled = true;
            }
        }

        // Функция проверки возможности хода в игре
        bool FreePole()
        {
            foreach (int s in GamePoleMas)
                if (s == 0) return true;

                   if (TestWin(player))
                    {
                        label3.Text = "Вы выиграли!";
                        LookPole();
                        panel3.Visible = true;
                        panel4.Visible = false;
                        return false;
                    }

                    if (TestWin(computer))
                    {
                        label3.Text = "Вы проиграли!";
                        LookPole();
                        panel3.Visible = true;
                        panel4.Visible = false;
                        return false;
                    } 

                label3.Text = "Ничья!";
                LookPole();
                panel3.Visible = true;
                panel4.Visible = false;

                return false;
            }

        // Функция проверки победной комбинации
        bool TestWin(int Win)
        {
            // варианты выигрышных комбинаций
            int[,] WinVariant =
            {      {    
                    1,1,1,  
                    0,0,0,  
                    0,0,0   
                }, 
                {    
                    0,0,0, 
                    1,1,1,  
                    0,0,0   
                }, 
                {    
                    0,0,0,  
                    0,0,0,  
                    1,1,1   
                }, 
                {    
                    1,0,0,  
                    1,0,0,  
                    1,0,0   
                }, 
                {    
                    0,1,0,  
                    0,1,0,  
                    0,1,0   
                },
                {    
                    0,0,1,  
                    0,0,1,  
                    0,0,1   
                }, 
                {    
                    1,0,0,  
                    0,1,0,  
                    0,0,1   
                }, 
                {    
                    0,0,1,   
                    0,1,0,   
                    1,0,0    
                }
            };

            // Просчёт поля
            int[] TestMas = new int[GamePoleMas.Length];
            for (int I = 0; I < GamePoleMas.Length; I++)
                if (GamePoleMas[I] == Win) TestMas[I] = 1;

            // вариант для сравнения 
            for (int Variant_Index = 0; Variant_Index < WinVariant.GetLength(0); Variant_Index++)
            {
                //счетчик для подсчета соотвествий
                int WinX = 0;
                for (int TestIndex = 0; TestIndex < TestMas.Length; TestIndex++)
                {
                    if (WinVariant[Variant_Index, TestIndex] == 1)
                    {
                        if (WinVariant[Variant_Index, TestIndex] == TestMas[TestIndex]) WinX++;
                    }
                    if (WinX == 3) return true;
                }
            }
            return false;
        }

        // Ход Компьютера
        void Computer_Step()
        {
            if (FreePole())
            {
                int temp0 = 0;
                if (GamePoleMas[0] == 0 && GamePoleMas[1] == 0 && GamePoleMas[2] == 0)
                {
                    temp0 = Rand.Next(0, 2);
                    if (GamePoleMas[temp0] == 0)
                    {
                        GamePole[temp0].Image = Image.FromFile(ImgName[computer]);
                        GamePoleMas[temp0] = computer;
                    }
                }
                else if (GamePoleMas[3] == 0 && GamePoleMas[4] == 0 && GamePoleMas[5] == 0)
                {

                    temp0 = Rand.Next(3, 5);
                    if (GamePoleMas[temp0] == 0)
                    {
                        GamePole[temp0].Image = Image.FromFile(ImgName[computer]);
                        GamePoleMas[temp0] = computer;
                    }
                }
                else if (GamePoleMas[6] == 0 && GamePoleMas[7] == 0 && GamePoleMas[8] == 0)
                {
                    temp0 = Rand.Next(6, 8);
                    if (GamePoleMas[temp0] == 0)
                    {
                        GamePole[temp0].Image = Image.FromFile(ImgName[computer]);
                        GamePoleMas[temp0] = computer;
                    }
                }
                else if (GamePoleMas[0] == 0 && GamePoleMas[3] == 0 && GamePoleMas[6] == 0)
                {
                    temp0 = 0;
                    if (GamePoleMas[temp0] == 0)
                    {
                        GamePole[temp0].Image = Image.FromFile(ImgName[computer]);
                        GamePoleMas[temp0] = computer;
                    }
                }
                else if (GamePoleMas[1] == 0 && GamePoleMas[4] == 0 && GamePoleMas[7] == 0)
                {
                    temp0 = 4;
                    if (GamePoleMas[temp0] == 0)
                    {
                        GamePole[temp0].Image = Image.FromFile(ImgName[computer]);
                        GamePoleMas[temp0] = computer;
                    }
                }
                else if (GamePoleMas[2] == 0 && GamePoleMas[5] == 0 && GamePoleMas[8] == 0)
                {
                    temp0 = 8;
                    if (GamePoleMas[temp0] == 0)
                    {
                        GamePole[temp0].Image = Image.FromFile(ImgName[computer]);
                        GamePoleMas[temp0] = computer;
                    }
                }
                else
                {
                GENER:
                    temp0 = Rand.Next(0, 8);
                    if (GamePoleMas[temp0] == 0)
                    {
                        GamePole[temp0].Image = Image.FromFile(ImgName[computer]);
                        GamePoleMas[temp0] = computer;
                    }
                    else goto GENER;
                }

                if (TestWin(computer))
                {
                    label3.Text = "Вы проиграли!";
                    LookPole();
                    panel3.Visible = true;
                    panel4.Visible = false;
                }
            }
        }


        private void Picture_Click(object sender, EventArgs e)
        {
            if (FreePole())
            {
                PictureBox ClickImage = sender as PictureBox;
                string[] PartName = ClickImage.Name.Split('_');

                int IndexSelectImage = Convert.ToInt32(PartName[1]);
                GamePole[IndexSelectImage].Image = Image.FromFile(ImgName[player]);
                GamePoleMas[IndexSelectImage] = player;

                if (!TestWin(player))
                {
                    LookPole();
                    Computer_Step();
                    UnLookPole();
                }
                else
                {
                    label3.Text = "Вы выиграли!";
                    LookPole();
                    panel3.Visible = true;
                    panel4.Visible = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Отключение кнопок свернуть-развернуть окно
            MaximizeBox = false;
            MinimizeBox = false;
            this.MaximumSize = new System.Drawing.Size(550, 460);

            sp.Play();
            MainPole();
            int x_ = panel1.Location.X,
                y2 = panel2.Location.Y,
                y3 = panel3.Location.Y,
                y4 = panel4.Location.Y;

            panel2.Location = new Point(69, 64);
            panel3.Location = new Point(69, 64);
            panel4.Location = new Point(69, 64);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            player = 1;
            computer = 2;
            panel2.Visible = false;
            panel4.Visible = true;
            if  (button4.Text == "Музыка: " + "Вкл" ^ button4.Text == "Музыка")
                 sm.Play();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            player = 2;
            computer = 1;
            panel2.Visible = false;
            panel4.Visible = true;
            if (button4.Text == "Музыка: " + "Вкл" ^ button4.Text == "Музыка")
                sm.Play();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel4.Visible = false;

            // Обнуление карты игры
            GamePoleMas = new int[] { 0, 0, 0,
                                      0, 0, 0,
                                      0, 0, 0
                                    };

            // Обнуление изображения поля
            foreach (PictureBox P in GamePole) P.Image = Image.FromFile(ImgName[0]);

            player = 0;
            computer = 0;
            UnLookPole();
            panel1.Visible = true;
            if (button4.Text == "Музыка: " + "Вкл" ^ button4.Text == "Музыка")
                sm.Play();
        }

        public void button4_Click(object sender, EventArgs e)
        {
            // Загрузка фоновой музыки Меню
            k++;
            if (panel4.Visible == false)
            {
                if (k % 2 > 0)
                {
                    sp.Stop();
                    button4.Text = "Музыка: " + "Выкл";
                }
                if (k % 2 == 0)
                {
                    sp.Play();
                    button4.Text = "Музыка: " + "Вкл";
                }
            }

            if (panel4.Visible == true)
                {
                    if (k % 2 > 0)
                    {
                        sm.Stop();
                        button4.Text = "Музыка: " + "Выкл";
                    }
                    if (k % 2 == 0)
                    {
                        sm.Play();
                        button4.Text = "Музыка: " + "Вкл";
                    }
                }
            }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = false;
        }
    }
}
