using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TourAgency.Models;

namespace TourAgency
{
    public partial class AddTourWindow : Window
    {
        public Tour NewTour { get; set; }
        
        public AddTourWindow()
        {
            InitializeComponent();
        }
        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bool hasErrors = false;
                string errorMessage = "";

                // Сброс цветов всех полей
            TxtName.Background = new SolidColorBrush(Colors.White);
            TxtName.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            TxtCountry.Background = new SolidColorBrush(Colors.White);
            TxtCountry.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            TxtCity.Background = new SolidColorBrush(Colors.White);
            TxtCity.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            TxtDuration.Background = new SolidColorBrush(Colors.White);
            TxtDuration.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            TxtPrice.Background = new SolidColorBrush(Colors.White);
            TxtPrice.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            TxtHotel.Background = new SolidColorBrush(Colors.White);
            TxtHotel.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));

            // Проверка названия
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                TxtName.Background = new SolidColorBrush(Colors.IndianRed);
                TxtName.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                errorMessage += "• Введите название тура\n";
                hasErrors = true;
            }

            // Проверка страны
            if (string.IsNullOrWhiteSpace(TxtCountry.Text))
            {
                TxtCountry.Background = new SolidColorBrush(Colors.IndianRed);
                TxtCountry.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                errorMessage += "• Введите страну\n";
                hasErrors = true;
            }

            // Проверка города
            if (string.IsNullOrWhiteSpace(TxtCity.Text))
            {
                TxtCity.Background = new SolidColorBrush(Colors.IndianRed);
                TxtCity.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                errorMessage += "• Введите город\n";
                hasErrors = true;
            }

            // Проверка длительности
            if (!int.TryParse(TxtDuration.Text, out int duration) || duration <= 0)
            {
                TxtDuration.Background = new SolidColorBrush(Colors.IndianRed);
                TxtDuration.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                errorMessage += "• Введите корректную длительность (целое число больше 0)\n";
                hasErrors = true;
            }

            // Проверка цены
            if (!decimal.TryParse(TxtPrice.Text, out decimal price) || price <= 0)
            {
                TxtPrice.Background = new SolidColorBrush(Colors.IndianRed);
                TxtPrice.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                errorMessage += "• Введите корректную цену (число больше 0)\n";
                hasErrors = true;
            }

            // Проверка гостиницы
            if (string.IsNullOrWhiteSpace(TxtHotel.Text))
            {
                TxtHotel.Background = new SolidColorBrush(Colors.IndianRed);
                TxtHotel.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                errorMessage += "• Введите название гостиницы\n";
                hasErrors = true;
            }

            // Проверка максимальной длительности
            if (int.TryParse(TxtDuration.Text, out int maxDuration) && maxDuration > 365)
            {
                TxtDuration.Background = new SolidColorBrush(Colors.IndianRed);
                TxtDuration.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                errorMessage += "• Длительность тура не может превышать 365 дней\n";
                hasErrors = true;
            }

            // Проверка максимальной цены
            if (decimal.TryParse(TxtPrice.Text, out decimal maxPrice) && maxPrice > 10000000)
            {
                TxtPrice.Background = new SolidColorBrush(Colors.IndianRed);
                TxtPrice.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                errorMessage += "• Цена не может превышать 10 000 000\n";
                hasErrors = true;
            }

            // Если есть ошибки, показываем сообщение и выходим
            if (hasErrors)
            {
                MessageBox.Show(errorMessage.TrimEnd('\n'), "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewTour = new Tour
            {
                Name = TxtName.Text.Trim(),
                Country = TxtCountry.Text.Trim(),
                City = TxtCity.Text.Trim(),
                Duration = duration,
                Price = price,
                Hotel = TxtHotel.Text.Trim()
            };
            DialogResult = true;
            Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void Loadedwin(object sender, RoutedEventArgs e)
        {
            if (NewTour != null)
            {
                TxtHotel.Text = NewTour.Hotel;
                TxtCity.Text = NewTour.City;
                TxtCountry.Text = NewTour.Country;
                TxtName.Text = NewTour.Name;
            }
        }

        private void NameLength(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtName.Text.Length >= 200)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина названия тура - 200 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CountryLength(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtCountry.Text.Length >= 100)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина названия страны - 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CityLength(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtCity.Text.Length >= 100)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина названия города - 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void HotelLength(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtHotel.Text.Length >= 200)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина названия гостиницы - 200 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ResetFieldColor(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Background = new SolidColorBrush(Colors.White);
            textBox.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
        }
    }
}
