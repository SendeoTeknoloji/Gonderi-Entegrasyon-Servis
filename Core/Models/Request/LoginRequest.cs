using Newtonsoft.Json;

namespace Core.Models.Request;

/// <summary>
/// Uç noktaları kullanabilmek için gerekli olan kimlik bilgilerini tanımlar.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Kullanıcı adı.
    /// </summary>
    [JsonProperty("musteri")]
    public string? UserName { get; set; }

    /// <summary>
    /// Parola.
    /// </summary>
    [JsonProperty("sifre")]
    public string? Password { get; set; }
}