namespace WebApi.Models.Errors
{
    /// <summary>
    /// Коды ошибок, которые возвращает API.
    /// </summary>
    public static class ErrorCodes
    {
        /// <summary>
        /// Запрошенная сущность не найдена.
        /// </summary>
        public const string EntityNotFound = "EntityNotFound";

        /// <summary>
        /// Переданные данные не прошли проверку.
        /// </summary>
        public const string ValidationError = "ValidationError";

        /// <summary>
        /// Операция противоречит текущему состоянию данных.
        /// </summary>
        public const string Conflict = "Conflict";

        /// <summary>
        /// Непредвиденная ошибка сервера.
        /// </summary>
        public const string InternalError = "InternalError";
    }
}
