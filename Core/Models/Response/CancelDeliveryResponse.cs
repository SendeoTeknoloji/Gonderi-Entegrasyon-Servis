namespace Core.Models.Response;

/// <summary>
/// Gönderi iptali uç noktasından dönen cevabın alanlarını belirtir.
/// </summary>
public class CancelDeliveryResponse
{
    /// <summary>
    /// İptal işleminin başarı durumunu belirten bayrak. Gönderi iptal edilirse <see langword="true"/>, edilemezse <see langword="false"/> döndürülür.
    /// </summary>
    public bool IsSuccess { get; set; }
}