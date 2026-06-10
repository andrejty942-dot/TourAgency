using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            bool hasErrors = false;
            string errorMessage = "";

            // Сброс цветов всех полей
            TxtClientFio.Background = new SolidColorBrush(Colors.White);
            TxtClientFio.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            TxtPeopleCount.Background = new SolidColorBrush(Colors.White);
            TxtPeopleCount.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));

            // Проверка ФИО
            if (string.IsNullOrWhiteSpace(TxtClientFio.Text))
            {
                TxtClientFio.Background = new SolidColorBrush(Colors.IndianRed);
                TxtClientFio.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                errorMessage += "• Введите ФИО клиента\n";
                hasErrors = true;
            }

            // Проверка тура
            if (CmbTour.SelectedItem == null)
            {
                errorMessage += "• Выберите тур\n";
                hasErrors = true;
            }

            // Проверка количества человек
            if (!int.TryParse(TxtPeopleCount.Text, out int peopleCount) || peopleCount <= 0)
            {
                TxtPeopleCount.Background = new SolidColorBrush(Colors.IndianRed);
                TxtPeopleCount.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                errorMessage += "• Введите корректное количество человек (целое число больше 0)\n";
                hasErrors = true;
            }

            // Проверка даты вылета
            if (!DpDepartureDate.SelectedDate.HasValue)
            {
                errorMessage += "• Выберите дату вылета\n";
                hasErrors = true;
            }
            else if (DpDepartureDate.SelectedDate.Value < DateTime.Now.Date)
            {
                errorMessage += "• Дата вылета не может быть в прошлом\n";
                hasErrors = true;
            }

            // Если есть ошибки, показываем сообщение и выходим
            if (hasErrors)
            {
                MessageBox.Show(errorMessage.TrimEnd('\n'), "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void Fioleght(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtClientFio.Text.Length >= 100)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина ФИО - 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void PhoneLength(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtPhone.Text.Length >= 20)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина телефона - 20 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EmailLength(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtEmail.Text.Length >= 100)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина email - 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
