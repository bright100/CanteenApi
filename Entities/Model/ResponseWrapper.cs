namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents a generic response wrapper.
    /// </summary>
    /// <typeparam name="T">The type of data being wrapped.</typeparam>
    public class ResponseWrapper<T>
    {
        /// <summary>
        /// Message accompanying the response.
        /// </summary>
        public string ResponseMessage { get; set; } = string.Empty;

        /// <summary>
        /// HTTP status code of the response.
        /// </summary>
        public int ResponseCode { get; set; } = 200;

        /// <summary>
        /// The actual data being wrapped.
        /// </summary>
        public T? Data { get; set; }
    }
}
