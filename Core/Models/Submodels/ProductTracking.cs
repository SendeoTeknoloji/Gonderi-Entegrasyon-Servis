namespace Core.Models.Submodels;

/// <summary>
/// TODO Bu sınıf ne amaçla kullanılıyor?
/// </summary>
public class ProductTracking
{
    /// <summary>
    /// Ürün sayısı.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Ürün kodu.
    /// </summary>
    public int ProductCode { get; set; }

    /// <summary>
    /// Açıklama.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Fiyat.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Stoktaki ürün sayısı.
    /// </summary>
    public int StockCount { get; set; }

    /// <summary>
    /// TODO MinStockAmount nedir?
    /// </summary>
    public int MinStockAmount { get; set; }
}