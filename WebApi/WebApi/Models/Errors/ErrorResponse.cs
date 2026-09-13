namespace WebApi.Models.Errors
{
    /// <summary>
    /// Описание ошибки, возникшей при обработке запроса.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Код ошибки, см. <see cref="ErrorCodes"/>.
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;

        /// <summary>
        /// Человекочитаемое описание того, что пошло не так.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
