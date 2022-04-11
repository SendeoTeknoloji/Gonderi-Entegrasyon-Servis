using Core.Models.Response;

namespace Core.Models;

/// <summary>
/// Sendeo Entegrasyon'dan gelen cevapların yönetimini kolaylaştırmak için kullanılır.
/// </summary>
/// <typeparam name="T"> Cevap nesnesi. </typeparam>
public class ResponseWrapper<T>
{
    /// <summary>
    /// HTTP durum kodu.
    /// </summary>
    public int? HttpStatusCode { get; set; }

    /// <summary>
    /// İstek ve cevapla ilgili verilen ek bilgi.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Cevap nesnesi.
    /// </summary>
    public ResponseBase<T>? Data { get; set; }
}