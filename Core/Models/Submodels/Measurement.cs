namespace Core.Models.Submodels;

/// <summary>
/// Bir gönderinin sayısını, boyutlarını, desisini ve ağırlığını belirtir.
/// </summary>
public class Measurement
{
    /// <summary>
    /// Gönderideki ürün sayısı.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gönderinin genişliği.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gönderinin uzunluğu.
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// Gönderinin yüksekliği.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Gönderinin desisi.
    /// </summary>
    public decimal Deci { get; set; }

    /// <summary>
    /// Gönderinin ağırlığı.
    /// </summary>
    public decimal Weight { get; set; }
}