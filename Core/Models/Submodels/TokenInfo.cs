namespace Core.Models.Submodels;

/// <summary>
/// Sende Entegrasyon'da kullanılacak yetkilendirme biletinin bilgileri.
/// </summary>
public class TokenInfo
{
    /// <summary>
    /// Yetkilendirme bileti.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Giriş yapan müşterinin numarası.
    /// </summary>
    public int CustomerId { get; set; }
}