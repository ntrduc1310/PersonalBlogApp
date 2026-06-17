namespace PersonalBlogApp.Models
{
    /// View model used for showing application-wide error information.
    public class ErrorViewModel
    {
        /// Gets or sets the HTTP request identifier.
        public string? RequestId { get; set; }

        /// Gets a value indicating whether the request identifier is present and should be shown.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
