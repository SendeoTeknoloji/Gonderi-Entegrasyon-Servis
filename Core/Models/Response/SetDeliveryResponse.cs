namespace Core.Models.Response;

/// <summary>
/// Gönderi oluşturma uç noktasından dönen cevabın alanlarını belirtir.
/// </summary>
public class SetDeliveryResponse
{
    /// <summary>
    /// Gönderi takip numarası.
    /// </summary>
    public string? TrackingNumber { get; set; }

    /// <summary>
    /// Gönderi takibinin yapılabileceği URL adresi.
    /// </summary>
    public string? TrackingUrl { get; set; }

    /// <summary>
    /// Gönderinin bilgilerini içeren barkod.
    /// </summary>
    public string? Barcode { get; set; }

    /// <summary>
    /// Gönderinin bilgilerinin içeren, ZPL türündeki barkod.
    /// </summary>
    public string? BarcodeZpl { get; set; }

    /// <summary>
    /// Gönderideki kargoların barkodlarının numaraları.
    /// </summary>
    public List<string>? BarcodeNumbers { get; set; }

    /// <summary>
    /// Gönderideki kargoların ZPL türündeki barkodların numaraları.
    /// </summary>
    public List<string>? BarcodeZpls { get; set; }
}