namespace TelefonOzellikleri.Models.ViewModels;

public class SmartphoneDetailViewModel
{
    public Smartphone Phone { get; set; } = null!;
    public Brand Brand { get; set; } = null!;
    public Series? Series { get; set; }
    public List<LatestPhoneItem> SameBrandPhones { get; set; } = new();
}
