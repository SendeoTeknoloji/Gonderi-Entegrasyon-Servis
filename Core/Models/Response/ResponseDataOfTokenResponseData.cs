using Core.Models.Submodels;

namespace Core.Models.Response;

/// <summary>
/// Giriş uç noktasından dönen cevabın alanlarını belirtir.
/// </summary>
public class ResponseDataOfTokenResponseData
{
    /// <summary>
    /// İsteğin GUID değeri.
    /// </summary>
    public Guid requestId { get; set; }

    /// <summary>
    /// Hata mesajı.
    /// </summary>
    /// <remarks> Eğer hata oluşmadıysa bu alanın dikkate alınmalıdır. </remarks>
    public string? ExceptionMessage { get; set; }

    /// <summary>
    /// İç hata açıklaması.
    /// </summary>
    /// <remarks> Eğer hata oluşmadıysa bu alanın dikkate alınmalıdır. </remarks>
    public string? InnerExceptionMessage { get; set; }

    /// <summary>
    /// Hata açıklaması.
    /// </summary>
    /// <remarks> Eğer hata oluşmadıysa bu alanın dikkate alınmalıdır. </remarks>
    public string? ExceptionDescription { get; set; }

    /// <summary>
    /// Yetkilendirme bileti bilgisi.
    /// </summary>
    public TokenInfo? result { get; set; }

    /// <summary>
    /// HTTP durum kodu.
    /// </summary>
    public int StatusCode { get; set; }
}