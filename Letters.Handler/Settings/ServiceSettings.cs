namespace Letters.Handler.Settings
{
    public class ServiceSettings
    {
        /// <summary>
        /// Идентификатор профиля через который выполняется запрос
        /// </summary>
        public Guid ProfileId { get; set; }

        /// <summary>
        /// Идентификатор организации которая принимает обращения
        /// </summary>
        public Guid RegistratorDepartmentId { get; set; }

        /// <summary>
        /// Заявитель по умолчанию
        /// </summary>
        public Guid ApplicantDefaultId { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public int DocumentSeatchParameterNumber { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public string DocumentKind { get; set; }

        /// <summary>
        /// Имя организации получателя
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Время паузы
        /// </summary>
        public int TimeOutSend { get; set; }

        /// <summary>
        /// Интервал логирования ошибок
        /// </summary>
        public int IntervalException { get; set; }
    }
}
