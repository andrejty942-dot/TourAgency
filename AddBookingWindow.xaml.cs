using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TourAgency.Models;

namespace TourAgency
{
    public partial class AddBookingWindow : Window
    {
        public Booking NewBooking { get; private set; }
        private ObservableCollection<Tour> _tours;
        public AddBookingWindow(ObservableCollection<Tour> tours)
        {
            InitializeComponent();
            _tours = tours;
            CmbTour.ItemsSource = _tours;
            DpDepartureDate.SelectedDate = DateTime.Now.AddDays(7);
        }
        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtClientFio.Text))
            {
                MessageBox.Show("Введите ФИО клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (CmbTour.SelectedItem == null)
            {
                MessageBox.Show("Выберите тур", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(TxtPeopleCount.Text, out int peopleCount) || peopleCount <= 0)
            {
                MessageBox.Show("Введите корректное количество человек (целое число больше 0)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!DpDepartureDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите дату вылета", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DpDepartureDate.SelectedDate.Value < DateTime.Now.Date)
            {
                MessageBox.Show("Дата вылета не может быть в прошлом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var selectedTour = (Tour)CmbTour.SelectedItem;
            var statusItem = (ComboBoxItem)CmbStatus.SelectedItem;
            NewBooking = new Booking
            {
                ClientFio = TxtClientFio.Text.Trim(),
                TourId = selectedTour.Id,
                TourName = selectedTour.Name,
                PeopleCount = peopleCount,
                DepartureDate = DpDepartureDate.SelectedDate.Value,
                TotalPrice = selectedTour.Price * peopleCount,
                Status = statusItem.Content.ToString(),
                Phone = TxtPhone.Text.Trim(),
                Email = TxtEmail.Text.Trim()
            };
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
