using System;

namespace TourAgency.Models
{
    /// <summary>
    /// Модель данных для туристического тура
    /// Содержит всю информацию о туре: название, направление, стоимость и длительность
    /// </summary>
    public class Tour
    {
        /// <summary>
        /// Уникальный идентификатор тура
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название тура (макс. 200 символов)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Страна назначения (макс. 100 символов)
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Город назначения (макс. 100 символов)
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Длительность тура в днях (1-365)
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Стоимость тура на одного человека (0.01 - 10,000,000)
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Название отеля (макс. 200 символов)
        /// </summary>
        public string Hotel { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// Инициализирует строковые поля пустыми строками для предотвращения NullReferenceException
        /// </summary>
        public Tour()
        {
            Name = string.Empty;
            Country = string.Empty;
            City = string.Empty;
            Hotel = string.Empty;
        }

        /// <summary>
        /// Переопределение ToString() для корректного отображения в ComboBox
        /// Возвращает название тура
        /// </summary>
        /// <returns>Название тура</returns>
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
