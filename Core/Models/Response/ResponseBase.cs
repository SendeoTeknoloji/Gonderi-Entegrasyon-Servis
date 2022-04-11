namespace Core.Models.Response;

/// <summary>
/// Sendeo Entegrasyon'un döndüğü cevabın biçimini tanımlar.
/// </summary>
/// <typeparam name="T"> Sendeo Entegrasyon'ndan dönen asıl verinin türü. </typeparam>
public class ResponseBase<T>
{
    /// <summary>
    /// İsteğin GUID değeri.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Hata mesajı.
    /// </summary>
    /// <remarks> Eğer hata oluşmadıysa bu alanın dikkate alınmalıdır. </remarks>
    public string? exceptionMessage { get; set; }

    /// <summary>
    /// İç hata açıklaması.
    /// </summary>
    /// <remarks> Eğer hata oluşmadıysa bu alanın dikkate alınmalıdır. </remarks>
    public string? innerExceptionMessage { get; set; }

    /// <summary>
    /// Hata açıklaması.
    /// </summary>
    /// <remarks> Eğer hata oluşmadıysa bu alanın dikkate alınmalıdır. </remarks>
    public string? exceptionDescription { get; set; }
    
    /// <summary>
    /// Döndürülen veri.
    /// </summary>
    public T? result { get; set; }

    /// <summary>
    /// HTTP durum kodu.
    /// </summary>
    public int StatusCode { get; set; }
}