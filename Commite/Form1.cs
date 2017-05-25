using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Commite
{
    public enum Mode
    {
        Insert,
        Update,
        Delete
    }
    public partial class Form1 : Form
    {
        private Mode myMode;

        List<Model1> MyList = new List<Model1>();
        int LastId = 1;
        int SelectedId = 0;

        public Mode MyMode
        {
            get
            {
                return myMode;
            }
            set
            {
                if (SelectedId != 0)
                {
                    LastId = MyList.Max(x => x.Id) + 1;
                    var item = MyList.Where(x => x.Id == SelectedId).FirstOrDefault();
                    textBox1.Text = item.نام;
                    textBox2.Text = item.نام_خانوادگی;
                    comboBox4.Text = item.نام_کمیته;
                    comboBox1.Text = item.مقطع_تحصیلی;
                    comboBox2.Text = item.رشته_ی_تحصیلی;
                    comboBox3.Text = item.محل_خدمت;
                    textBox3.Text = item.شماره_پرسنلی;
                    comboBox5.Text = item.نتیجه;
                    textBox4.Text = item.اعضای_کمیته;
                    numericUpDown1.Value = int.Parse(item.نمره);
                    textBox5.Text = item.تاریخ;
                    dateTimePicker2.Text = item.زمان;
                }
                switch (value)
                {
                    case Mode.Insert:
                        button1.Text = "جدید";
                        SelectedId = 0;
                        dataGridView1.DataSource = MyList.ToList();
                        textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text =
                            comboBox1.Text = comboBox2.Text = comboBox3.Text = comboBox4.Text = comboBox5.Text =
                            string.Empty;
                        numericUpDown1.Value = 0;
                        break;
                    case Mode.Update:
                        button1.Text = "ویرایش";
                        break;
                    case Mode.Delete:
                        button1.Text = "حذف";
                        break;
                }
                myMode = value;
            }
        }

        public Form1()
        {
            InitializeComponent();

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "hh:mm";

            textBox5.Text = DateTime.Now.ToString("yyyy/MM/dd");
            dateTimePicker2.Value = DateTime.Now;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            foreach (var item in File.ReadAllText("List1.csv").Split(new string[] { "\r\n" }, StringSplitOptions.None))
            {
                var row = item.Split(',');
                if (row.Length > 1)
                {
                    var thisItem = new Model1();
                    thisItem.Id = int.Parse(row[0]);
                    thisItem.نام = row[1];
                    thisItem.نام_خانوادگی = row[2];
                    thisItem.شماره_پرسنلی = row[3];
                    thisItem.مقطع_تحصیلی = row[4];
                    thisItem.رشته_ی_تحصیلی = row[5];
                    thisItem.محل_خدمت = row[6];
                    thisItem.نام_کمیته = row[7];
                    thisItem.اعضای_کمیته = row[8].Replace(" - ", "\r\n");
                    thisItem.نتیجه = row[9];
                    thisItem.نمره = row[10];
                    thisItem.تاریخ = row[11];
                    thisItem.زمان = row[12];

                    MyList.Add(thisItem);
                }

                MyMode = Mode.Insert;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyMode = Mode.Insert;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MyMode != Mode.Delete)
                if (MyList.Where(x => x.شماره_پرسنلی == textBox3.Text).Count() > 0 || textBox3.Text.Length != 6)
                    throw new Exception("کد پرسنلی باید منحصر به فرد و شش رقم باشد");
            switch (MyMode)
            {
                case Mode.Insert:
                    MyList.Add(
                        new Model1
                        {
                            Id = LastId,
                            نام = textBox1.Text,
                            نام_خانوادگی = textBox2.Text,
                            نام_کمیته = comboBox4.Text,
                            مقطع_تحصیلی = comboBox1.Text,
                            رشته_ی_تحصیلی = comboBox2.Text,
                            محل_خدمت = comboBox3.Text,
                            شماره_پرسنلی = textBox3.Text,
                            نتیجه = comboBox5.Text,
                            اعضای_کمیته = textBox4.Text.Replace("\r\n", " - "),
                            نمره = numericUpDown1.Value.ToString(),
                            تاریخ = textBox5.Text,
                            زمان = dateTimePicker2.Text
                        }
                        );
                    break;
                case Mode.Update:
                    var item_update = MyList.Where(x => x.Id == SelectedId).FirstOrDefault();
                    item_update.نام = textBox1.Text;
                    item_update.نام_خانوادگی = textBox2.Text;
                    item_update.نام_کمیته = comboBox4.Text;
                    item_update.مقطع_تحصیلی = comboBox1.Text;
                    item_update.رشته_ی_تحصیلی = comboBox2.Text;
                    item_update.محل_خدمت = comboBox3.Text;
                    item_update.شماره_پرسنلی = textBox3.Text;
                    item_update.نتیجه = comboBox5.Text;
                    item_update.اعضای_کمیته = textBox4.Text.Replace("\r\n", " - ");
                    item_update.نمره = numericUpDown1.Value.ToString();
                    item_update.تاریخ = textBox5.Text;
                    item_update.زمان = dateTimePicker2.Text;
                    break;
                case Mode.Delete:
                    var item_delete = MyList.Where(x => x.Id == SelectedId).FirstOrDefault();
                    SelectedId = 0;
                    MyList.Remove(item_delete);
                    break;
            }
            MyMode = Mode.Insert;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in MyList)
            {
                sb.Append(item.Id + "," + item.نام + "," + item.نام_خانوادگی + "," + item.شماره_پرسنلی + "," + item.مقطع_تحصیلی + "," + item.رشته_ی_تحصیلی + "," + item.محل_خدمت + "," + item.نام_کمیته + "," + item.اعضای_کمیته + "," + item.نتیجه + "," + item.نمره + "," + item.تاریخ + "," + item.زمان + "\r\n");
            }
            string output = sb.ToString();
            File.WriteAllText("List1.csv", output, Encoding.UTF8);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedId = MyList[e.RowIndex].Id;
            switch (e.ColumnIndex)
            {
                case 0:
                    MyMode = Mode.Update;
                    break;

                case 1:
                    MyMode = Mode.Delete;
                    break;
            }
        }
    }
}
