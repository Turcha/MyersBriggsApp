using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyersBriggsApp
{
    public partial class Form1 : Form
    {
        string name = "";
        string lastname = "";
        string otch = "";

        int A1, A2;
        int i = 0;
        StreamReader read;
        int ch;
        string s, Res;
        string s_o = "";
        string s_b = "";
        string s_v = "";
        byte block;
        byte otv1 = 0;
        byte otv2 = 0;
        byte vo = 0;
        byte ot = 0;
        const char met = '#';

        string[] Results = {"ISTJ - ответственный, организатор",
                                "ISFJ - лояльный, исполнитель",
                                "INFJ - вдохновляющий созерцатель",
                                "INTJ - независимый, мыслитель",
                                "ISTP - прагматичный, мастер на все руки",
                                "ISFP - некичливый, хороший член команды",
                                "INFP - благородный, идеалист",
                                "INTP - концептуальный, мечтатель",
                                "ESTP - спонтанный, реалист",
                                "ESFP - великодушный, весельчак",
                                "ENFP - оптимист, для него люди важнее всего",
                                "ENTP - исследователь, изобретательная личность",
                                "ESTJ - администратор, требовательный человек",
                                "ESFJ - гармоничная личность, друг для всех",
                                "ENFJ - переговорщик, умеющий убеждать",
                                "ENTJ - Командующий, лидер"};

        public Form1()
        {
            InitializeComponent();
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
        }


        //Кнопка "Начать тест
        private void button1_Click(object sender, EventArgs e)
        {
            if (DataVerification())
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                label4.Text = String.Format(" {0} {1} {2}", name, lastname, otch);
                Res = "";
                A1 = 0;
                A2 = 0;
                block = 0;
                read = new StreamReader("data.txt");
                RunningTest();
            }
        }

        //Проверка входных данных
        private bool DataVerification()
        {
           
            //Имя
            name = textBox1.Text;
            //Фамилия
            lastname = textBox2.Text;
            //Отчество
            otch = textBox3.Text;

            if (String.IsNullOrEmpty(name))
            {
                MessageBox.Show("Некорректно указано поле Имя!!!");
                return false;
            }

            if (String.IsNullOrEmpty(lastname))
            {
                MessageBox.Show("Некорректно указано поле Фамилия!!!");
                return false;
            }

            if (String.IsNullOrEmpty(otch))
            {
                MessageBox.Show("Некорректно указано поле Отчество!!!");
                return false;
            }

            return true;
        }
        private void Show(string tit, string vorp, string var1, string var2, byte nom)
        {
            label7.Text = String.Format("Часть: {0}", tit);
            label8.Text = String.Format("Вопрос: {0}: {1}", nom, vorp);
            label9.Text = String.Format("1 - {0}", var1);
            label10.Text = String.Format("2 - {0}", var2);
        }

        //Кнопка номер 1
        private void button2_Click(object sender, EventArgs e)
        {
            ch = 1;
            RunningTest();
        }
        //Кнопка номер 2
        private void button3_Click(object sender, EventArgs e)
        {
            ch = 2;
            RunningTest();
        }

        private void VAL(string str, ref byte num, ref int err)
        {
            string result = "";

            for (int i = 0; i < str.Length; i++)
            {
                //Проверяем на число
                if (char.IsDigit(str[i]))
                {
                    result += str[i];
                }
                else
                {
                    err = i;
                    break;
                }
            }
            num = Convert.ToByte(result);
        }

        //Главный тест программы
        private void RunningTest()
        {

            do {
    
                s = read.ReadLine();

                if (s == null)
                {
                    read.Close();
                    groupBox2.Enabled = false;
                    for (int j = 0; j < 16; j++)
                    {
                        if (Results[j].IndexOf(Res) > -1)
                        {
                            s_o = Results[j];
                        }
                    }

                    label5.Text = s_o;


                    StreamWriter swWrite = new StreamWriter(String.Format("{0}_{1}.txt",name,lastname));
                    swWrite.WriteLine(s_o);
                    swWrite.Close();
                    break;
                }

                if (s[0] == met)
                    {
                        if ((A1 > 0) && (A2 > 0))
                        {
                            switch (block)
                            {
                                case 1:
                                    if (A1 > A2)
                                        Res += 'E';
                                    else
                                        Res += 'I';
                                    break;
                                case 2:
                                    if (A1 > A2)
                                        Res += 'S';
                                    else
                                        Res += 'N';
                                    break;
                                case 3:
                                    if (A1 > A2)
                                        Res += 'T';
                                    else
                                        Res += 'F';
                                    break;
                                case 4:
                                    if (A1 > A2)
                                        Res += 'J';
                                    else
                                        Res += 'P';
                                    break;
                            }
                            A1 = 0;
                            A2 = 0;
                        }
                        block++;
                        vo = 0;
                        s_b = s.Substring(1);
                        s = read.ReadLine();
                        s_b = String.Format("{0} - {1}", s_b, s.Substring(1));
                    }
                    else if (s.IndexOf(met) == -1)
                    {
                        s_v = s;
                        ot = 0;
                    }
                    else if (s.IndexOf(met) > 1)
                    {
                        ot++;
                        vo++;

                        VAL(s.Substring(s.IndexOf(met) + 1), ref otv1, ref i);
                        s_o = s.Substring(0, s.IndexOf(met));
                        s = read.ReadLine();
                        VAL(s.Substring(s.IndexOf(met) + 1), ref otv2, ref i);
                        Show(s_b, s_v, s_o, s.Substring(0, s.IndexOf(met)), vo);

                        label11.Text = "Нажмите 1 или 2 для ввода ответа. ";

                        switch (ch)
                        {
                            case 1:
                                A1 += otv1;
                                break;
                            case 2:
                                A2 += otv2;
                                break;
                        }
                    }

            } while ((s[0] == met) || (s.IndexOf(met) == -1));

        
        }
    }
}
