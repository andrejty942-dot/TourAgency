using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TourAgency.Models;

namespace TourAgency
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Tour> _tours;
        private ObservableCollection<Booking> _bookings;
        private int _nextTourId = 1;
        private int _nextBookingId = 1;
        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
        }
        private void InitializeData()
        {
            _tours = new ObservableCollection<Tour>();
            _bookings = new ObservableCollection<Booking>();
            AddSampleTours();
            DgTours.ItemsSource = _tours;
            DgBookings.ItemsSource = _bookings;
        }
        private void AddSampleTours()
        {
            _tours.Add(new Tour
            {
                Id = _nextTourId++,
                Name = "Пляжный отдых в Турции",
                Country = "Турция",
                City = "Анталия",
                Duration = 7,
                Price = 35000,
                Hotel = "Rixos Premium Belek"
            });
            _tours.Add(new Tour
            {
                Id = _nextTourId++,
                Name = "Экскурсионный тур в Италию",
                Country = "Италия",
                City = "Рим",
                Duration = 5,
                Price = 55000,
                Hotel = "Hotel Colosseum"
            });
            _tours.Add(new Tour
            {
                Id = _nextTourId++,
                Name = "Горнолыжный курорт",
                Country = "Австрия",
                City = "Инсбрук",
                Duration = 10,
                Price = 75000,
                Hotel = "Alpine Resort"
            });
        }
        private void BtnAddTourClick(object sender, RoutedEventArgs e)
        {
            AddTourWindow addWindow = new AddTourWindow();
            addWindow.NewTour = new Tour();
            if (addWindow.ShowDialog() == true)
            {
                var newTour = addWindow.NewTour;
                newTour.Id = _nextTourId++;
                _tours.Add(newTour);
                MessageBox.Show($"Тур \"{newTour.Name}\" успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BtnEditTourClick(object sender, RoutedEventArgs e)
        {
            var selectedTour = DgTours.SelectedItem as Tour;
            if (selectedTour == null)
            {
                MessageBox.Show("Выберите тур для редактирования", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var editWindow = new EditTourWindow(selectedTour);
            if (editWindow.ShowDialog() == true)
            {
                DgTours.Items.Refresh();
                MessageBox.Show($"Тур \"{selectedTour.Name}\" успешно обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BtnDeleteTourClick(object sender, RoutedEventArgs e)
        {
            var selectedTour = DgTours.SelectedItem as Tour;
            if (selectedTour == null)
            {
                MessageBox.Show("Выберите тур для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var hasBookings = _bookings.Any(b => b.TourId == selectedTour.Id);
            if (hasBookings)
            {
                MessageBox.Show("Невозможно удалить тур, на который есть бронирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var result = MessageBox.Show($"Вы уверены, что хотите удалить тур \"{selectedTour.Name}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _tours.Remove(selectedTour);
                MessageBox.Show("Тур удалён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                _nextTourId = _nextTourId-1;
            }
        }
        private void BtnRefreshToursClick(object sender, RoutedEventArgs e)
        {
            DgTours.Items.Refresh();
            MessageBox.Show($"Список туров обновлён. Всего туров: {_tours.Count}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void BtnAddBookingClick(object sender, RoutedEventArgs e)
        {
            if (_tours.Count == 0)
            {
                MessageBox.Show("Сначала добавьте хотя бы один тур", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var addWindow = new AddBookingWindow(_tours);
            if (addWindow.ShowDialog() == true)
            {
                var newBooking = addWindow.NewBooking;
                newBooking.Id = _nextBookingId++;
                _bookings.Add(newBooking);
                MessageBox.Show($"Бронирование для \"{newBooking.ClientFio}\" успешно создано!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BtnEditBookingClick(object sender, RoutedEventArgs e)
        {
            var selectedBooking = DgBookings.SelectedItem as Booking;
            if (selectedBooking == null)
            {
                MessageBox.Show("Выберите бронирование для редактирования", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var editWindow = new EditBookingWindow(selectedBooking, _tours.ToList());
            if (editWindow.ShowDialog() == true)
            {
                DgBookings.Items.Refresh();
                MessageBox.Show($"Бронирование для \"{selectedBooking.ClientFio}\" успешно обновлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BtnDeleteBookingClick(object sender, RoutedEventArgs e)
        {
            var selectedBooking = DgBookings.SelectedItem as Booking;
            if (selectedBooking == null)
            {
                MessageBox.Show("Выберите бронирование для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show($"Вы уверены, что хотите удалить бронирование для \"{selectedBooking.ClientFio}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _bookings.Remove(selectedBooking);
                MessageBox.Show("Бронирование удалено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BtnRefreshBookingsClick(object sender, RoutedEventArgs e)
        {
            DgBookings.Items.Refresh();
            MessageBox.Show($"Список бронирований обновлён. Всего бронирований: {_bookings.Count}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
