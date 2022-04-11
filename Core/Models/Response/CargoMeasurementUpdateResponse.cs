namespace Core.Models.Response;

/// <summary>
/// Gönderi ölçüleri güncelleştirme uç noktasından dönen cevabın alanlarını belirtir.
/// </summary>
public class CargoMeasurementUpdateResponse
{
    /// <summary>
    /// İşlemin başarı durumunu belirten bayrak.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// İşlem hakkında verilen bilgi.
    /// </summary>
    public string? Message { get; set; }
}