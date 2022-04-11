using Core.Models.Submodels;

namespace Core.Models.Request;

/// <summary>
/// Bir gönderinin ölçülerinin güncelleştirilebilmesi için gerekli alanları tanımlar.
/// </summary>
public class CargoMeasurementUpdateRequest
{
    /// <summary>
    /// Müşteri referans numarası.
    /// </summary>
    public string? CustomerReferenceNo { get; set; }

    /// <summary>
    /// Gönderideki ürünlerin ölçülerini belirten nesnelerin listesi.
    /// </summary>
    public List<Measurement>? Measurements { get; set; }
}