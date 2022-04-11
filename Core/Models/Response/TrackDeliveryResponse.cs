using Core.Models.Submodels;

namespace Core.Models.Response;

/// <summary>
/// Gönderi durumu uç noktasından dönen cevabın alanlarını belirtir.
/// </summary>
public class TrackDeliveryResponse
{
    /// <summary>
    /// Gönderi takip numarası.
    /// </summary>
    public int TrackingNo { get; set; }
        
    /// <summary>
    /// Gönderi referans numarası.
    /// </summary>
    public string? ReferenceNo { get; set; }

    /// <summary>
    /// Gönderim tarihi.
    /// </summary>
    public string? SendDate { get; set; }

    /// <summary>
    /// Alıcı bilgisi.
    /// </summary>
    public string? Receiver { get; set; }

    /// <summary>
    /// TODO CargoAmount nedir?
    /// </summary>
    public decimal CargoAmount { get; set; }

    /// <summary>
    /// Gönderici bilgisi.
    /// </summary>
    public string? Sender { get; set; }

    /// <summary>
    /// Gönderinin durumunun numarası.
    /// </summary>
    public int? State { get; set; }

    /// <summary>
    /// Gönderinin durumunun okunabilir açıklaması.
    /// </summary>
    public string? StateText { get; set; }

    /// <summary>
    /// Durum bilgisinin güncelleştirilme tarihi.
    /// </summary>
    public string? UpdateDate { get; set; }

    /// <summary>
    /// Gönderi açıklaması.
    /// </summary>
    public string? DeliveryDescription { get; set; }

    /// <summary>
    /// Gönderideki ürünler hakkında bilgi.
    /// </summary>
    public List<ProductTracking>? Products { get; set; }

    /// <summary>
    /// TODO DealerId nedir?
    /// </summary>
    public int DealerId { get; set; }

    /// <summary>
    /// Gönderinin desi veya ağırlığı.
    /// </summary>
    public int DeciWeight { get; set; }

    /// <summary>
    /// Gönderideki kargo sayısı.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Toplam taşıma bedeli.
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Gönderinin çıktığı şubenin adı.
    /// </summary>
    public string? DepartureBranchName { get; set; }

    /// <summary>
    /// Gönderinin ulaştığı şubenin adı.
    /// </summary>
    public string? ArrivalBranchName { get; set; }

    /// <summary>
    /// Gönderinin planlanan dağıtılma tarihi.
    /// </summary>
    public DateTime? DeliveryPlannedDate { get; set; }

    /// <summary>
    /// Gönderi durum bilgileri.
    /// </summary>
    public List<StatusHistory>? StatusHistories { get; set; }
}