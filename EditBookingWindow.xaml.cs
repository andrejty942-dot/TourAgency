using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TourAgency.Models;

namespace TourAgency
{
    /// <summary>
    /// Окно редактирования существующего бронирования
    /// Загружает данные выбранного бронирования и позволяет изменить их
    /// Использует ту же валидацию, что и AddBookingWindow (email, телефон, даты, количество)
    /// Поддерживает автоматический пересчёт общей стоимости при изменении тура или количества человек
    /// </summary>
    public partial class EditBookingWindow : Window
    {
        /// <summary>
        /// Редактируемое бронирование (ссылка на объект из коллекции)
        /// Изменения применяются непосредственно к этому объекту
        /// </summary>
        public Booking EditedBooking { get; private set; }

        /// <summary>
        /// Список доступных туров для выбора
        /// </summary>
        private List<Tour> _tours;

        /// <summary>
        /// Конструктор окна редактирования бронирования
        /// Принимает бронирование для редактирования и список туров
        /// </summary>
        /// <param name="booking">Бронирование для редактирования</param>
        /// <param name="tours">Список доступных туров</param>
        public EditBookingWindow(Booking booking, List<Tour> tours)
        {
            InitializeComponent();
            EditedBooking = booking;
            _tours = tours;
            CmbTour.ItemsSource = _tours;
            LoadBookingData();
        }

        /// <summary>
        /// Загрузка данных бронирования в поля формы
        /// Заполняет все поля значениями из EditedBooking
        /// Устанавливает выбранный тур и статус в ComboBox
        /// </summary>
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

        /// <summary>
        /// Обработчик изменения выбранного тура
        /// Пересчитывает общую стоимость при выборе другого тура
        /// </summary>
        private void CmbTour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecalculateTotalPrice();
        }

        /// <summary>
        /// Обработчик изменения количества человек
        /// Пересчитывает общую стоимость при изменении количества
        /// </summary>
        private void TxtPeopleCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            RecalculateTotalPrice();
        }

        /// <summary>
        /// Пересчёт общей стоимости бронирования
        /// Формула: TotalPrice = selectedTour.Price × peopleCount
        /// Вызывается автоматически при изменении тура или количества человек
        /// </summary>
        private void RecalculateTotalPrice()
        {
            if (CmbTour.SelectedItem is Tour selectedTour && 
                int.TryParse(TxtPeopleCount.Text, out int peopleCount) && 
                peopleCount > 0)
            {
                EditedBooking.TotalPrice = selectedTour.Price * peopleCount;
            }
        }

        /// <summary>
        /// Обработчик кнопки "Сохранить"
        /// Выполняет валидацию и применяет изменения к объекту бронирования
        /// Включает try-catch для обработки исключений
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bool hasErrors = false;
                string errorMessage = "";

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

            // Проверка статуса
            if (CmbStatus.SelectedItem == null)
            {
                errorMessage += "• Выберите статус бронирования\n";
                hasErrors = true;
            }

            // Проверка телефона (если заполнен)
            if (!string.IsNullOrWhiteSpace(TxtPhone.Text))
            {
                string phonePattern = @"^[\d\s\+\-\(\)]+$";
                if (!Regex.IsMatch(TxtPhone.Text.Trim(), phonePattern))
                {
                    TxtPhone.Background = new SolidColorBrush(Colors.IndianRed);
                    TxtPhone.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                    errorMessage += "• Введите корректный номер телефона (только цифры, +, -, (), пробелы)\n";
                    hasErrors = true;
                }
            }

            // Проверка email (если заполнен)
            if (!string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(TxtEmail.Text.Trim(), emailPattern))
                {
                    TxtEmail.Background = new SolidColorBrush(Colors.IndianRed);
                    TxtEmail.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                    errorMessage += "• Введите корректный email адрес\n";
                    hasErrors = true;
                }
            }

            // Проверка максимального количества человек
            if (int.TryParse(TxtPeopleCount.Text, out int maxPeopleCheck) && maxPeopleCheck > 100)
            {
                TxtPeopleCount.Background = new SolidColorBrush(Colors.IndianRed);
                TxtPeopleCount.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                errorMessage += "• Количество человек не может превышать 100\n";
                hasErrors = true;
            }

            // Проверка даты вылета (не более 2 лет вперёд)
            if (DpDepartureDate.SelectedDate.HasValue && DpDepartureDate.SelectedDate.Value > DateTime.Now.AddYears(2))
            {
                errorMessage += "• Дата вылета не может быть более чем через 2 года\n";
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
            EditedBooking.ClientFio = TxtClientFio.Text.Trim();
            EditedBooking.TourId = selectedTour.Id;
            EditedBooking.TourName = selectedTour;
            EditedBooking.PeopleCount = peopleCount;
            EditedBooking.DepartureDate = DpDepartureDate.SelectedDate.Value;
            EditedBooking.TotalPrice = selectedTour.Price * peopleCount;
            EditedBooking.Status = statusItem.Content.ToString();
            EditedBooking.Phone = TxtPhone.Text.Trim();
            EditedBooking.Email = TxtEmail.Text.Trim();
            DialogResult = true;
            Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик кнопки "Отмена"
        /// Закрывает окно без сохранения изменений
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Проверка ограничения длины ФИО клиента (макс. 100 символов)
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события ввода текста</param>
        private void Fioleght(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtClientFio.Text.Length >= 100)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина ФИО - 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Проверка ограничения длины телефона (макс. 20 символов)
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события ввода текста</param>
        private void PhoneLength(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtPhone.Text.Length >= 20)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина телефона - 20 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Проверка ограничения длины email (макс. 100 символов)
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события ввода текста</param>
        private void EmailLength(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtEmail.Text.Length >= 100)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина email - 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Сброс красной подсветки поля при начале редактирования
        /// Восстанавливает стандартный цвет фона и рамки
        /// </summary>
        /// <param name="sender">Источник события (TextBox)</param>
        /// <param name="e">Аргументы события изменения текста</param>
        private void ResetFieldColor(object sender, TextChangedEventArgs e)
        {
            var selectedTour = (Tour)CmbTour.SelectedItem;
            var textBox = (TextBox)sender;
            textBox.Background = new SolidColorBrush(Colors.White);
            textBox.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));

        }
    }
}
