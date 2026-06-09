using System;
using System.Windows;
using TourAgency.Models;

namespace TourAgency
{
    public partial class EditTourWindow : Window
    {
        public Tour EditedTour { get; private set; }
        public EditTourWindow(Tour tour)
        {
            InitializeComponent();
            EditedTour = tour;
            LoadTourData();
        }
        private void LoadTourData()
        {
            TxtName.Text = EditedTour.Name;
            TxtCountry.Text = EditedTour.Country;
            TxtCity.Text = EditedTour.City;
            TxtDuration.Text = EditedTour.Duration.ToString();
            TxtPrice.Text = EditedTour.Price.ToString();
            TxtHotel.Text = EditedTour.Hotel;
        }
        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
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
            EditedTour.Name = TxtName.Text.Trim();
            EditedTour.Country = TxtCountry.Text.Trim();
            EditedTour.City = TxtCity.Text.Trim();
            EditedTour.Duration = duration;
            EditedTour.Price = price;
            EditedTour.Hotel = TxtHotel.Text.Trim();
            DialogResult = true;
            Close();
        }
        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
