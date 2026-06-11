using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TourAgency
{
    /// <summary>
    /// Окно авторизации пользователя в системе
    /// Предоставляет форму входа с логином и паролем
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// <summary>
        /// Конструктор окна авторизации
        /// Инициализирует компоненты интерфейса
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Войти"
        /// Выполняет валидацию учетных данных и авторизацию пользователя
        /// </summary>
        /// <param name="sender">Источник события (кнопка)</param>
        /// <param name="e">Аргументы события</param>
        private void BtnLogInClick(object sender, RoutedEventArgs e)
        {
            // Получаем введенные данные
            string login = Name.Text;
            string password = Password.Password;
            
            // Проверка на пустые поля (добавлено в v1.1 для исправления BUG-001)
            // Защита от попытки входа без ввода логина или пароля
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }
            
            // Проверка учетных данных (hardcoded для демонстрации)
            // В production-версии заменить на проверку в базе данных
            if (login == "agent" && password == "agent")
            {
                // Успешная авторизация - открываем главное окно
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                // Неверные учетные данные - очищаем поля и показываем ошибку
                Name.Text = string.Empty; Password.Password = string.Empty;
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Выход"
        /// Завершает работу приложения
        /// </summary>
        /// <param name="sender">Источник события (кнопка)</param>
        /// <param name="e">Аргументы события</param>
        private void BtnExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }  
    }
}
