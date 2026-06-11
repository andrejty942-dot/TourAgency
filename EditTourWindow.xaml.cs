using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TourAgency.Models;

namespace TourAgency
{
    /// <summary>
    /// Окно редактирования существующего тура
    /// Загружает данные выбранного тура и позволяет изменить их
    /// Использует ту же валидацию, что и AddTourWindow
    /// </summary>
    public partial class EditTourWindow : Window
    {
        /// <summary>
        /// Редактируемый тур (ссылка на объект из коллекции)
        /// Изменения применяются непосредственно к этому объекту
        /// </summary>
        public Tour EditedTour { get; private set; }

        /// <summary>
        /// Конструктор окна редактирования тура
        /// Принимает тур для редактирования и загружает его данные в поля
        /// </summary>
        /// <param name="tour">Тур для редактирования</param>
        public EditTourWindow(Tour tour)
        {
            InitializeComponent();
            EditedTour = tour;
            LoadTourData();
        }

        /// <summary>
        /// Загрузка данных тура в поля формы
        /// Заполняет все текстовые поля значениями из EditedTour
        /// </summary>
        private void LoadTourData()
        {
            TxtName.Text = EditedTour.Name;
            TxtCountry.Text = EditedTour.Country;
            TxtCity.Text = EditedTour.City;
            TxtDuration.Text = EditedTour.Duration.ToString();
            TxtPrice.Text = EditedTour.Price.ToString();
            TxtHotel.Text = EditedTour.Hotel;
        }

        /// <summary>
        /// Обработчик кнопки "Сохранить"
        /// Выполняет валидацию и применяет изменения к объекту тура
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

            EditedTour.Name = TxtName.Text.Trim();
            EditedTour.Country = TxtCountry.Text.Trim();
            EditedTour.City = TxtCity.Text.Trim();
            EditedTour.Duration = duration;
            EditedTour.Price = price;
            EditedTour.Hotel = TxtHotel.Text.Trim();
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
        /// Проверка ограничения длины названия тура (макс. 200 символов)
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события ввода текста</param>
        private void NameLength(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtName.Text.Length >= 200)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина названия тура - 200 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Проверка ограничения длины названия страны (макс. 100 символов)
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события ввода текста</param>
        private void CountryLength(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtCountry.Text.Length >= 100)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина названия страны - 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Проверка ограничения длины названия города (макс. 100 символов)
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события ввода текста</param>
        private void CityLength(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtCity.Text.Length >= 100)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина названия города - 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Проверка ограничения длины названия гостиницы (макс. 200 символов)
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события ввода текста</param>
        private void HotelLength(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (TxtHotel.Text.Length >= 200)
            {
                e.Handled = true;
                MessageBox.Show("Максимальная длина названия гостиницы - 200 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            var textBox = (TextBox)sender;
            textBox.Background = new SolidColorBrush(Colors.White);
            textBox.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
        }
    }
}
