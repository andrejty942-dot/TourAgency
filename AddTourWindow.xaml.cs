using System;
using System.Windows;
using TourAgency.Models;

namespace TourAgency
{
    /// <summary>
    /// Логика взаимодействия для AddTourWindow.xaml
    /// </summary>
    public partial class AddTourWindow : Window
    {
        public Tour NewTour { get; set; }
        
        public AddTourWindow()
        {
            InitializeComponent();
        }

        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            // Валидация полей
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                MessageBox.Show("Введите название тура", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtCountry.Text))
            {
                MessageBox.Show("Введите страну", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtCity.Text))
            {
                MessageBox.Show("Введите город", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtDuration.Text, out int duration) || duration <= 0)
            {
                MessageBox.Show("Введите корректную длительность (целое число больше 0)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену (число больше 0)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtHotel.Text))
            {
                MessageBox.Show("Введите название гостиницы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Создание нового тура
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

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TxtHotel_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void Loadedwin(object sender, RoutedEventArgs e)
        {
            TxtHotel.Text = NewTour.Hotel;
            
            
            TxtCity.Text = NewTour.City;
            TxtCountry.Text = NewTour.Country;
            TxtName.Text = NewTour.Name;
        }
    }
}
