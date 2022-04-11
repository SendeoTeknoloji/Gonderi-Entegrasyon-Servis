namespace Core.Models.Request;

/// <summary>
/// Bir gönderinin iptal edilmesi isteğinde kullanılacak alanları tanımlar.
/// </summary>
public class CancelDeliveryRequest
{
    /// <summary>
    /// Gönderinin takip numarası.
    /// </summary>
    /// <remarks> Takip numarası ve müşteri referans numarasından bir tanesinin verilmesi gerekli ve yeterlidir. </remarks>
    public int? TrackingNo { get; set; }

    /// <summary>
    /// Gönderinin müşteri referans numarası.
    /// </summary>
    /// <remarks> Takip numarası ve müşteri referans numarasından bir tanesinin verilmesi gerekli ve yeterlidir. </remarks>
    public string? CustomerReferenceNo { get; set; }
}