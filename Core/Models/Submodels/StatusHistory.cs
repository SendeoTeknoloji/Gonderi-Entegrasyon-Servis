namespace Core.Models.Submodels;

/// <summary>
/// Bir gönderi üzerinde yapılmış bir işlemin detaylarını belirtir.
/// </summary>
public class StatusHistory
{
    /// <summary>
    /// Durum bilgisinin oluşturulduğu tarih.
    /// </summary>
    public DateTime? StatusDate { get; set; }

    /// <summary>
    /// Durum numarası.
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Durumun okunabilir açıklaması.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Durum hakkında detay açıklama.
    /// </summary>
    public string? Description { get; set; }
}