using Newtonsoft.Json;

namespace Core.Models.Submodels;

/// <summary>
/// Bir gönderide bulunan bir ürünün bilgilerini barındıran alanları tanımlar.
/// </summary>
public class Product
{
    /// <summary>
    /// Ürünün sayısı.
    /// </summary>
    [JsonProperty("count")]
    public int Count { get; set; }

    /// <summary>
    /// Ürünün kodu.
    /// </summary>
    [JsonProperty("productCode")]
    public string? ProductCode { get; set; }

    /// <summary>
    /// Ürün hakkında açıklama.
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Ürünün desisi.
    /// </summary>
    [JsonProperty("deci")]
    public int Deci { get; set; }

    /// <summary>
    /// Ürünün ağırlığı.
    /// </summary>
    [JsonProperty("weigth")]
    public int Weigth { get; set; }

    /// <summary>
    /// Ürünün desisi veya ağırlığı.
    /// </summary>
    /// <remarks> Bu değer, ürün veya desiden hangisi büyükse ona eşit olmalıdır. </remarks>
    public int DeciWeight { get; set; }

    /// <summary>
    /// Ürünün fiyatı.
    /// </summary>
    public decimal Price { get; set; }
}