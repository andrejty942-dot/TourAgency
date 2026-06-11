using System;

namespace TourAgency.Models
{
    /// <summary>
    /// Модель данных для бронирования тура
    /// Содержит информацию о клиенте, выбранном туре, количестве человек и статусе бронирования
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// Уникальный идентификатор бронирования
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ФИО клиента (макс. 100 символов)
        /// </summary>
        public string ClientFio { get; set; }

        /// <summary>
        /// ID выбранного тура (внешний ключ к Tour.Id)
        /// </summary>
        public int TourId { get; set; }

        /// <summary>
        /// Объект тура для отображения в DataGrid
        /// Используется для связи с коллекцией туров
        /// </summary>
        public Tour TourName { get; set; }

        /// <summary>
        /// Количество человек в бронировании (1-100)
        /// </summary>
        public int PeopleCount { get; set; }

        /// <summary>
        /// Дата вылета (от сегодня до +2 лет)
        /// </summary>
        public DateTime DepartureDate { get; set; }

        /// <summary>
        /// Общая стоимость бронирования (Price * PeopleCount)
        /// Рассчитывается автоматически при создании/редактировании
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Статус бронирования: "Новое", "Подтверждено", "Оплачено", "Отменено"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Телефон клиента (макс. 20 символов, формат: цифры, +, -, (), пробелы)
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Email клиента (макс. 100 символов, проверяется regex)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// Инициализирует поля значениями по умолчанию
        /// Дата вылета устанавливается на +7 дней от текущей даты
        /// </summary>
        public Booking()
        {
            ClientFio = string.Empty;
            Status = "Новое";
            Phone = string.Empty;
            Email = string.Empty;
            DepartureDate = DateTime.Now.AddDays(7);
        }
    }
}
