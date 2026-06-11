using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TourAgency.Models;

namespace TourAgency
{
    /// <summary>
    /// Окно добавления нового бронирования
    /// Содержит форму с валидацией всех полей, включая regex для email и телефона
    /// Автоматически рассчитывает общую стоимость (цена тура × количество человек)
    /// </summary>
    public partial class AddBookingWindow : Window
    {
        /// <summary>
        /// Новое бронирование, создаваемое в этом окне
        /// Передается обратно в MainWindow при успешном добавлении
        /// </summary>
        public Booking NewBooking { get; private set; }

        /// <summary>
        /// Конструктор окна добавления бронирования
        /// Инициализирует список туров и устанавливает дату вылета по умолчанию (+7 дней)
        /// </summary>
        /// <param name="_tours">Коллекция доступных туров для выбора</param>
        public AddBookingWindow(ObservableCollection<Tour> _tours)
        {
            InitializeComponent();

            CmbTour.ItemsSource = _tours;
            DpDepartureDate.SelectedDate = DateTime.Now.AddDays(7);
        }

        /// <summary>
        /// Обработчик кнопки "Добавить"
        /// Выполняет полную валидацию всех полей и создает объект Booking
        /// Включает проверки: обязательные поля, regex email/телефона, диапазоны дат, максимальные значения
        /// Включает try-catch для обработки исключений
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void BtnAddClick(object sender, RoutedEventArgs e)
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
            NewBooking = new Booking
            {
                ClientFio = TxtClientFio.Text.Trim(),
                TourId = selectedTour.Id,
                TourName = selectedTour,
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
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик кнопки "Отмена"
        /// Закрывает окно без сохранения данных
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
        /// Вызывается при вводе текста в поле TxtClientFio
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
        /// Вызывается при вводе текста в поле TxtPhone
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
        /// Вызывается при вводе текста в поле TxtEmail
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
        /// Вызывается автоматически при изменении текста в любом поле
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
