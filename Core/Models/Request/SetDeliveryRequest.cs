using Core.Enums;
using Core.Models.Submodels;

namespace Core.Models.Request;

/// <summary>
/// Bir gönderi oluşturmak için gereken alanları tanımlar.
/// </summary>
public class SetDeliveryRequest
{
    /// <summary>
    /// Gönderi türü. 
    /// </summary>
    public DeliveryType DeliveryType { get; set; }

    /// <summary>
    /// Gönderinin referans numarası.
    /// </summary>
    public string? ReferenceNo { get; set; }

    /// <summary>
    /// Gönderi hakkınd açıklama.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gönderiyi gönderen tarafın adı.
    /// </summary>
    /// <remarks> Kişi veya şirketin adı yazılır. </remarks>
    public string? Sender { get; set; }

    /// <summary>
    /// Gönderici dal kodu.
    /// </summary>
    public int SenderBranchCode { get; set; }

    /// <summary>
    /// Göndericinin kimliği.
    /// </summary>
    public string? SenderId { get; set; }

    /// <summary>
    /// TODO Bu nedir?
    /// </summary>
    public string? SenderAuthority { get; set; }

    /// <summary>
    /// Göndericinin adresi.
    /// </summary>
    public string? SenderAddress { get; set; }

    /// <summary>
    /// Gönderinin alındığı şehrin kimliği.
    /// </summary>
    public int SenderCityId { get; set; }

    /// <summary>
    /// Gönderinin alındığı mahallenin kimliği.
    /// </summary>
    public int SenderDistrictId { get; set; }

    /// <summary>
    /// Göndericinin telefon numarası.
    /// </summary>
    public string? SenderPhone { get; set; }

    /// <summary>
    /// Göndericinin cep telefonu numarası.
    /// </summary>
    public string? SenderGSM { get; set; }

    /// <summary>
    /// Göndericinin e-posta adresi.
    /// </summary>
    public string? SenderEmail { get; set; }

    /// <summary>
    /// Gönderiyi alan tarafın adı.
    /// </summary>
    /// <remarks> Kişi veya şirketin adı yazılır. </remarks>
    public string? Receiver { get; set; }

    /// <summary>
    /// Alıcı dal kodu.
    /// </summary>
    public int ReceiverBranchCode { get; set; }

    /// <summary>
    /// Alıcının kimliği.
    /// </summary>
        
    public string? ReceiverId { get; set; }

    /// <summary>
    /// TODO Bu nedir?
    /// </summary>
    public string? ReceiverAuthority { get; set; }

    /// <summary>
    /// Alıcının adresi.
    /// </summary>
    public string? ReceiverAddress { get; set; }

    /// <summary>
    /// Gönderinin teslim edildiği şehrin kimliği.
    /// </summary>
    public int ReceiverCityId { get; set; }

    /// <summary>
    /// Gönderinin teslim edildiği mahallenin kimliği.
    /// </summary>
    public int ReceiverDistrictId { get; set; }

    /// <summary>
    /// Alıcının telefon numarası.
    /// </summary>
    public string? ReceiverPhone { get; set; }

    /// <summary>
    /// Alıcının cep telefonu numarası.
    /// </summary>
    public string? ReceiverGSM { get; set; }

    /// <summary>
    /// Alıcının e-posta adresi.
    /// </summary>
    public string? ReceiverEmail { get; set; }

    /// <summary>
    /// Ödeme türü.
    /// </summary>
    public int PaymentType { get; set; }

    /// <summary>
    /// TODO Bu nedir?
    /// </summary>
    public int CollectionType { get; set; }

    /// <summary>
    /// TODO Bu nedir?
    /// </summary>
    public decimal CollectionPrice { get; set; }

    /// <summary>
    /// TODO Bu nedir?
    /// </summary>
    public string? DispatchNoteNumber { get; set; }

    /// <summary>
    /// Hizmet türü.
    /// </summary>
    public int ServiceType { get; set; }

    /// <summary>
    /// Barkod etiketinin türü.
    /// </summary>
    public int? BarcodeLabelType { get; set; }

    /// <summary>
    /// Gönderilecek ürünlerin bilgisi.
    /// </summary>
    public List<Product>? Products { get; set; }

    /// <summary>
    /// TODO Bu nedir?
    /// </summary>
    public ICollection<AygFileModel>? FileDatas { get; set; }

    /// <summary>
    /// Müşteri referans türü.
    /// </summary>
    public string? CustomerReferenceType { get; set; }
}