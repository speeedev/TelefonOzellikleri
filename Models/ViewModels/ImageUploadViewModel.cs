namespace TelefonOzellikleri.Models.ViewModels
{
    /// <summary>
    /// View model for the reusable image upload partial component.
    /// </summary>
    public class ImageUploadViewModel
    {
        /// <summary>Label text (e.g. "Main Image URL", "Logo URL").</summary>
        public string LabelText { get; set; } = "Image URL";

        /// <summary>HTML id for the URL input (must be unique on the page).</summary>
        public string UrlInputId { get; set; } = "imageUrlInput";

        /// <summary>HTML id for the hidden file input (must be unique on the page).</summary>
        public string FileInputId { get; set; } = "imageFileInput";

        /// <summary>HTML id for the status message element (must be unique on the page).</summary>
        public string StatusId { get; set; } = "uploadStatus";

        /// <summary>Form field name for model binding (e.g. "MainImageUrl", "LogoUrl").</summary>
        public string InputName { get; set; } = "ImageUrl";

        /// <summary>Upload subfolder: "phones", "brands", or "pages".</summary>
        public string UploadFolder { get; set; } = "phones";

        /// <summary>Initial value for the URL input (when editing).</summary>
        public string? CurrentValue { get; set; }
    }
}
