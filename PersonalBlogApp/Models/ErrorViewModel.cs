namespace PersonalBlogApp.Models
{
    /// <summary>
    /// View model used for showing application-wide error information.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the HTTP request identifier.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the request identifier is present and should be shown.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
