using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TourAgency.Models;

namespace TourAgency
{
    public partial class EditBookingWindow : Window
    {
        public Booking EditedBooking { get; private set; }
        private List<Tour> _tours;
        public EditBookingWindow(Booking booking, List<Tour> tours)
        {
            InitializeComponent();
            EditedBooking = booking;
            _tours = tours;
            CmbTour.ItemsSource = _tours;
            LoadBookingData();
        }
        private void LoadBookingData()
        {
            TxtClientFio.Text = EditedBooking.ClientFio;
            var selectedTour = _tours.FirstOrDefault(t => t.Id == EditedBooking.TourId);
            if (selectedTour != null)
            {
                CmbTour.SelectedItem = selectedTour;
            }
            TxtPeopleCount.Text = EditedBooking.PeopleCount.ToString();
            DpDepartureDate.SelectedDate = EditedBooking.DepartureDate;
            TxtPhone.Text = EditedBooking.Phone;
            TxtEmail.Text = EditedBooking.Email;
            foreach (ComboBoxItem item in CmbStatus.Items)
            {
                if (item.Content.ToString() == EditedBooking.Status)
                {
                    CmbStatus.SelectedItem = item;
                    break;
                }
            }
        }
        private void CmbTour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecalculateTotalPrice();
        }
        private void TxtPeopleCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            RecalculateTotalPrice();
        }
        private void RecalculateTotalPrice()
        {
            if (CmbTour.SelectedItem is Tour selectedTour && 
                int.TryParse(TxtPeopleCount.Text, out int peopleCount) && 
                peopleCount > 0)
            {
                EditedBooking.TotalPrice = selectedTour.Price * peopleCount;
            }
        }
        private void BtnSaveClick(object sender, RoutedEventArgs e)
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
            EditedBooking.ClientFio = TxtClientFio.Text.Trim();
            EditedBooking.TourId = selectedTour.Id;
            EditedBooking.TourName = selectedTour.Name;
            EditedBooking.PeopleCount = peopleCount;
            EditedBooking.DepartureDate = DpDepartureDate.SelectedDate.Value;
            EditedBooking.TotalPrice = selectedTour.Price * peopleCount;
            EditedBooking.Status = statusItem.Content.ToString();
            EditedBooking.Phone = TxtPhone.Text.Trim();
            EditedBooking.Email = TxtEmail.Text.Trim();
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
