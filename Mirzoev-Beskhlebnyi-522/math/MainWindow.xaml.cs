using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private const string formulaUrl = "https://i.imgur.com/PFJyc4Q.png";

        public MainWindow()
        {
            InitializeComponent();
            LoadFormulaImage();
        }

        private void LoadFormulaImage()
        {
            try
            {
                Uri uri = new Uri(formulaUrl, UriKind.Absolute);

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = uri;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                ImageControl.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения (формулы): {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            InputX.Text = string.Empty;
            InputP.Text = string.Empty;
            SelectFValue.SelectedIndex = -1;
            OutputResult.Text = "Результат: ";
        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            double x, p;

            if (!double.TryParse(InputX.Text, out x) || !double.TryParse(InputP.Text, out p))
            {
                MessageBox.Show("Введите корректные числовые значения для x и p.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ComboBoxItem selectedItem = SelectFValue.SelectedItem as ComboBoxItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Выберите функцию f(x).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double fVal = CalculateF(x, selectedItem.Content.ToString());
            if (double.IsNaN(fVal))
            {
                MessageBox.Show("Ошибка в расчете f(x).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double absP = Math.Abs(p);
            double result;

            if (x > absP)
            {
                result = 2.0 * Math.Pow(fVal, 3) + 3.0 * p * p;
            }
            else if (Math.Abs(x - absP) < 1e-14)
            {
                result = Math.Pow(fVal - p, 2);
            }
            else if (x > 3.0 && x < absP)
            {
                result = Math.Abs(fVal - p);
            }
            else
            {
                MessageBox.Show("Значение x не соответствует условиям.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OutputResult.Text = $"Результат: {result:F4}";
        }

        private double CalculateF(double x, string functionName)
        {
            if (functionName == "sh(x)")
            {
                return Math.Sinh(x);
            }
            else if (functionName == "x^2")
            {
                return x * x;
            }
            else if (functionName == "e^x")
            {
                return Math.Exp(x);
            }
            return double.NaN;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}