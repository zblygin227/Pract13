using LibMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Pract13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Matrix<int> matrix;

        public void Выход_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Очистить_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            inputColumnCount.Clear();
            inputRowCount.Clear();
            result.Text = null;
        }

        public void О_Программе_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Дана целочисленная матрица размера M * N." +
                "Найти номер первого из ее столбцов,содержащих только нечетные числа." +
                "Если таких столбцов нет, то вывести 0.", "О программе");
        }

        public void Сохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = @".\matrix" + matrix.Extension;
                matrix.Save(path);
                MessageBox.Show("Сохранение прошло успешно", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Открыть_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = @".\matrix" + matrix.Extension;
                matrix.Open(path);
                dataGrid.ItemsSource = matrix.ToDataTable().DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Рассчитать_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                result.Text = null;
                int columnCount = int.Parse(inputColumnCount.Text);
                int rowCount = int.Parse(inputRowCount.Text);

                matrix = new Matrix<int>(rowCount, columnCount);
                Random rnd = new();
                int[,] matr = new int[rowCount, columnCount];

                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        matr[i, j] = rnd.Next(-100, 100);
                    }
                }
                matrix.Add(matr);
                dataGrid.ItemsSource = matrix.ToDataTable().DefaultView;

                int res = 0;
                for (int j = 0; j < columnCount; j++)
                {
                    int odd = 0;
                    for (int i = 0; i < rowCount; i++)
                    {
                        int item = matrix[i, j];
                        if (item % 2 != 0)
                        {
                            odd++;
                        }
                    }
                    if (odd == rowCount)
                    {
                        res = j + 1;
                    }
                }
                result.Text = res.ToString();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Данные введены неверно", "Ошибка");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
    }
}
