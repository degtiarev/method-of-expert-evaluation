using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Experts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1_Click(new object(), new EventArgs());

            double[,] matrix = new double[,]{ {4, 7, 7, 4, 2},
                                              {5, 8, 5, 6, 5}, 
                                              {9, 6, 8, 9, 9},
                                              {2, 4, 4, 5, 3},
                                              {7, 2, 6, 8, 4},
                                              {6, 3, 3, 7, 6},
                                              {8, 10,9,12, 8},
                                              {1, 1, 2, 1, 1},
                                              {3, 5, 1, 2, 7},
                                              {11,9, 12,3, 10},
                                              {10,12,11,10,12},
                                              {12,11,10,11,11}};

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 12; j++)
                    dataGridView1[i, j].Value = matrix[j, i].ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            dataGridView1.ColumnCount = 0;
            for (int i = 0; i < numericUpDown1.Value; i++)
                dataGridView1.Columns.Add("Expert" + (i + 1), "Эксперт " + (i + 1));

            dataGridView1.RowCount = Convert.ToInt32(numericUpDown2.Value);

            dataGridView1.TopLeftHeaderCell.Value = "Объект";

            for (int i = 0; (i <= (dataGridView1.Rows.Count - 1)); i++)
                dataGridView1.Rows[i].HeaderCell.Value = string.Format("Объект " + (i + 1));

            dataGridView1.RowHeadersWidth = 5;
            dataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            double[,] matrix = new double[dataGridView1.RowCount, dataGridView1.ColumnCount];
            double number;


            for (int i = 0; i < dataGridView1.RowCount; i++)

                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    if ((dataGridView1[j, i].Value == null) || double.TryParse(dataGridView1[j, i].Value.ToString(), out number) == false ||
                        Convert.ToDouble(dataGridView1[j, i].Value.ToString()) > 12 || Convert.ToDouble(dataGridView1[j, i].Value.ToString()) < 0)
                    {
                        MessageBox.Show("Ошибка, проверьте, во всех ли ячейках числовые значения и находятся в отрезке (1;10)");
                        return;
                    }
                    else matrix[i, j] = Convert.ToDouble(dataGridView1[j, i].Value.ToString());
                }

            List<string> allNames = new List<string>();

            for (int i = 0; i < dataGridView1.RowCount; i++)
                allNames.Add(dataGridView1.Rows[i].HeaderCell.Value.ToString());

            EvalutionTable myTable = new EvalutionTable(matrix, allNames);

            List<Tuple<string, double>> mu = myTable.GetResult();

            dataGridView2.DataSource = myTable.GetResult();

            dataGridView2.Columns[0].HeaderText = "Меры адаптации";
            dataGridView2.Columns[1].HeaderText = "Экспертная оценка";

            label4.Text = myTable.GetConcordanceCoefficient().ToString();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Random rand = new Random();

            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                for (int j = 0; j < dataGridView1.RowCount; j++)
                    dataGridView1[i, j].Value = rand.Next(0, 10);

        }


    }



}
