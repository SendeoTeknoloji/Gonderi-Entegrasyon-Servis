using System.Globalization;
using System.Text;
using ConsoleUi.Utilities;
using Core;
using Core.Enums;
using Core.Models.Request;
using Core.Models.Response;
using Core.Models.Submodels;

namespace ConsoleUi;

/// <summary>
/// Uygulamayı ve Sendeo Entegrasyon ile iletişim kuran istemciyi yöneten yöntemler içerir.
/// </summary>
public class Program
{
    /// <summary>
    /// Sendeo Entegrasyon ile iletişim kurmak için kullanılacak istemci.
    /// </summary>
    public Client Client { get; set; }

    /// <summary>
    /// Giriş yapmış kullanıcının adı.
    /// </summary>
    public string? User { get; set; }

    /// <summary>
    /// Sendeo Entegrasyon'dan alınmış yetkilendirme bileti.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// <see cref="Program"/> sınıfının bir örneğini ve Sendeo Entegrasyon ile iletişim kurmak için bir istemci oluşturur.
    /// </summary>
    public Program()
    {
        Client = new Client();
    }

    /// <summary>
    /// Uygulamanın başlangıç noktasıdır.
    /// </summary>
    private static async Task Main()
    {
        // Türkçe karakterlerin doğru gösterilmesi için.
        Console.OutputEncoding = Encoding.Default;

        var program = new Program();

        await program.Start();
    }

    /// <summary>
    /// Uygulamanın hayat döngüsünü başlatır.
    /// </summary>
    /// <returns></returns>
    public async Task Start()
    {
        Console.WriteLine("Sendeo Entegrasyon İstemcisi'ne Hoş Geldiniz!");

        while (true)
        {
            if (!await LoginAsync())
                break;

            if (!await MainMenuAsync())
                break;
        }

        Console.WriteLine("Sendeo Entegrasyon İstemcisi'ni kullandığınız için teşekkürler!");
    }

    /// <summary>
    /// Giriş yapma sürecini başlatır.
    /// </summary>
    /// <returns> Eğer giriş başarılı olursa <see langword="true"/>, olmazsa <see langword="false"/> döndürür. </returns>
    private async Task<bool> LoginAsync()
    {
        ResponseDataOfTokenResponseData? loginResponse = null;

        while (loginResponse is not {StatusCode: 200})
        {
            var userName = ConsoleUtilities.ObtainNonEmptyInput("Kullanıcı adı");
            var password = ConsoleUtilities.ObtainNonEmptyInput("Parola");

            var credentials = new LoginRequest
            {
                UserName = userName, 
                Password = password
            };

            Console.WriteLine("Giriş yapılıyor, lütfen bekleyin...");

            loginResponse = await Client.LoginAsync(credentials);

            if (loginResponse.result != null)
            {
                if (loginResponse.StatusCode == 200
                    && loginResponse.result != null
                    && !string.IsNullOrWhiteSpace(loginResponse.result.Token))
                {
                    Console.WriteLine("Başarıyla giriş yapıldı.");
                    User = credentials.UserName;
                    Token = loginResponse.result.Token;
                    return true;
                }
            }

            switch (loginResponse.StatusCode)
            {
                case -1:
                {
                    Console.Write("Sendeo Entegrasyon ile iletişim kurulurken bir hata oluştu.");

                    Console.WriteLine(!string.IsNullOrEmpty(loginResponse.ExceptionMessage)
                        ? $" Ek bilgi: {loginResponse.ExceptionMessage}"
                        : " Lütfen daha sonra yeniden deneyin.");

                    break;
                }

                case >= 400 and < 500:
                {
                    Console.WriteLine("Giriş bilgileri doğru değil.");
                    break;
                }

                case >= 500 and < 600:
                {
                    Console.WriteLine("Giriş uç noktasından sunucu kaynaklı bir hata olduğu bildirildi.");
                    break;
                }

                default:
                {
                    Console.WriteLine("Giriş uç noktasından beklenmeyen bir durum kodu bildirildi.");
                    break;
                }
            }

            Console.WriteLine("Yeniden denemek için herhangi bir tuşa, çıkmak için 'Esc' tuşuna basın.");

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape) return false;
        }

        return true;
    }

    /// <summary>
    /// Ana menüyü gösterir ve kullanıcının seçimlerini işler.
    /// </summary>
    /// <returns> Kullanıcı çıkış yapma (logout) seçeneğini seçerse <see langword="true"/>, uygulamayı kapatma seçeneğini seçerse <see langword="false"/> döndürür. </returns>
    private async Task<bool> MainMenuAsync()
    {
        Console.WriteLine();
        Console.WriteLine($"Hoş geldiniz {User}! Ne yapmak istersiniz?");

        ConsoleKeyInfo? menuChoice = null;

        while (menuChoice == null)
        {
            Console.WriteLine("1: Gönderi oluştur");
            Console.WriteLine("2: Gönderi takibi yap");
            Console.WriteLine("3: Gönderi iptal et");
            Console.WriteLine("4: Gönderi ölçülerini güncelleştir");
            Console.WriteLine("5: Çıkış yap");
            Console.WriteLine("Esc: Uygulamayı kapat");

            menuChoice = Console.ReadKey(true);

            switch (menuChoice.Value.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                {
                    Console.WriteLine("Gönderi oluşturma seçildi.");
                    Console.WriteLine();
                    await SetDeliveryAsync();
                    break;
                }

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                {
                    Console.WriteLine("Gönderi takibi seçildi.");
                    Console.WriteLine();
                    await TrackDeliveryAsync();
                    break;
                }

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                {
                    Console.WriteLine("Gönderi iptali seçildi.");
                    Console.WriteLine();
                    await CancelDeliveryAsync();
                    break;
                }

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                {
                    Console.WriteLine("Gönderi ölçülerini güncelleştirme seçildi.");
                    Console.WriteLine();
                    await UpdateMeasurementsAsync();
                    break;
                }

                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                {
                    Console.WriteLine("Başarıyla çıkış yapıldı.");
                    Console.WriteLine();
                    User = null;
                    return true;
                }

                case ConsoleKey.Escape:
                {
                    return false;
                }

                default:
                {
                    Console.WriteLine("Geçersiz seçim yaptınız.");
                    break;
                }
            }

            menuChoice = null;
            Console.WriteLine();
        }

        return false;
    }

    /// <summary>
    /// Bir gönderi oluşturmak için kullanıcıdan gerekli bilgileri alır, istek gönderir ve sonucu bildirir.
    /// </summary>
    /// <returns></returns>
    private async Task SetDeliveryAsync()
    {
        var request = new SetDeliveryRequest();

        // Gönderi türü ve gönderi türüne göre belirlenen alanlar

        var deliveryType = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Gönderi türü");

        while (deliveryType is < 1 or > 6)
        {
            Console.WriteLine("Geçersiz gönderici türü girdiniz. Lütfen 1 ile 6 arasında (ikisi de dâhil) bir değer girin.");
            deliveryType = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Gönderi türü");
        }

        request.DeliveryType = (DeliveryType)deliveryType;

        switch (deliveryType)
        {
            case 1:
            {
                request.Receiver = ConsoleUtilities.ObtainNonEmptyInput("Alıcı");
                request.ReceiverPhone = ConsoleUtilities.ObtainInput("Alıcı telefon no");
                request.ReceiverGSM = ConsoleUtilities.ObtainInput("Alıcı GSM no");
                request.ReceiverId = ConsoleUtilities.ObtainInput("Alıcı no");

                //while (string.IsNullOrWhiteSpace(request.ReceiverPhone)
                //       && string.IsNullOrWhiteSpace(request.ReceiverGSM)
                //       && string.IsNullOrWhiteSpace(request.ReceiverId))
                //{
                //    Console.WriteLine("Lütfen alıcı telefonu, GSM numarası veya alıcı no alanlarından en az bir tanesini girin.");

                //    request.ReceiverPhone = InputUtilities.ObtainInput("Alıcı telefon no");
                //    request.ReceiverGSM = InputUtilities.ObtainInput("Alıcı GSM no");
                //    request.ReceiverId = InputUtilities.ObtainInput("Alıcı no");
                //}

                request.ReceiverAddress = ConsoleUtilities.ObtainNonEmptyInput("Alıcı adresi");
                request.ReceiverDistrictId = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Alıcı mahalle no");
                request.ReceiverCityId = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Alıcı şehir no");
                request.ReceiverEmail = ConsoleUtilities.ObtainInput("Alıcı e-posta adresi");
                break;
            }

            case 2:
            {
                request.Sender = ConsoleUtilities.ObtainNonEmptyInput("Gönderici");
                request.SenderPhone = ConsoleUtilities.ObtainInput("Gönderici telefon no");
                request.SenderGSM = ConsoleUtilities.ObtainInput("Gönderici GSM no");
                request.SenderId = ConsoleUtilities.ObtainInput("Gönderici no");

                //while (string.IsNullOrWhiteSpace(request.SenderPhone)
                //       && string.IsNullOrWhiteSpace(request.SenderGSM)
                //       && string.IsNullOrWhiteSpace(request.SenderId))
                //{
                //    Console.WriteLine("Lütfen gönderici telefonu, GSM numarası veya gönderici no alanlarından en az bir tanesini girin.");

                //    request.SenderPhone = InputUtilities.ObtainInput("Gönderici telefon no");
                //    request.SenderGSM = InputUtilities.ObtainInput("Gönderici GSM no");
                //    request.SenderId = InputUtilities.ObtainInput("Gönderici no");
                //}

                request.SenderAddress = ConsoleUtilities.ObtainNonEmptyInput("Gönderici adresi");
                request.SenderDistrictId = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Gönderici mahalle no");
                request.SenderCityId = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Gönderici şehir no");
                request.ReceiverEmail = ConsoleUtilities.ObtainInput("Gönderici e-posta adresi");
                break;
            }

            case 3:
            {
                request.Receiver = ConsoleUtilities.ObtainNonEmptyInput("Alıcı");
                request.ReceiverPhone = ConsoleUtilities.ObtainInput("Alıcı telefon no");
                request.ReceiverGSM = ConsoleUtilities.ObtainInput("Alıcı GSM no");
                request.ReceiverId = ConsoleUtilities.ObtainInput("Alıcı no");

                //while (string.IsNullOrWhiteSpace(request.ReceiverPhone)
                //       && string.IsNullOrWhiteSpace(request.ReceiverGSM)
                //       && string.IsNullOrWhiteSpace(request.ReceiverId))
                //{
                //    Console.WriteLine("Lütfen alıcı telefonu, GSM numarası veya alıcı no alanlarından en az bir tanesini girin.");

                //    request.ReceiverPhone = InputUtilities.ObtainInput("Alıcı telefon no");
                //    request.ReceiverGSM = InputUtilities.ObtainInput("Alıcı GSM no");
                //    request.ReceiverId = InputUtilities.ObtainInput("Alıcı no");
                //}

                request.ReceiverAddress = ConsoleUtilities.ObtainNonEmptyInput("Alıcı adresi");
                request.ReceiverDistrictId = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Alıcı mahalle no");
                request.ReceiverCityId = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Alıcı şehir no");
                request.ReceiverEmail = ConsoleUtilities.ObtainInput("Alıcı e-posta adresi");

                request.Sender = ConsoleUtilities.ObtainNonEmptyInput("Gönderici");
                request.SenderPhone = ConsoleUtilities.ObtainInput("Gönderici telefon no");
                request.SenderGSM = ConsoleUtilities.ObtainInput("Gönderici GSM no");
                request.SenderId = ConsoleUtilities.ObtainInput("Gönderici no");

                //while (string.IsNullOrWhiteSpace(request.SenderPhone)
                //       && string.IsNullOrWhiteSpace(request.SenderGSM)
                //       && string.IsNullOrWhiteSpace(request.SenderId))
                //{
                //    Console.WriteLine("Lütfen gönderici telefonu, GSM numarası veya gönderici no alanlarından en az bir tanesini girin.");

                //    request.SenderPhone = InputUtilities.ObtainInput("Gönderici telefon no");
                //    request.SenderGSM = InputUtilities.ObtainInput("Gönderici GSM no");
                //    request.SenderId = InputUtilities.ObtainInput("Gönderici no");
                //}

                request.SenderAddress = ConsoleUtilities.ObtainNonEmptyInput("Gönderici adresi");
                request.SenderDistrictId = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Gönderici mahalle no");
                request.SenderCityId = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Gönderici şehir no");
                request.ReceiverEmail = ConsoleUtilities.ObtainInput("Gönderici e-posta adresi");
                break;
            }

            case 4:
            {
                request.ReferenceNo = ConsoleUtilities.ObtainNonEmptyInput("Referans no");
                break;
            }

            case 5:
            {
                request.ReceiverBranchCode = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Alıcı dal kodu");
                break;
            }

            case 6:
            {
                request.SenderBranchCode = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Gönderici dal kodu");
                break;
            }
        }

        // Ürün sayısı
        var productCount = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Ürün sayısı");

        while (productCount < 1)
        {
            Console.WriteLine("Geçersiz ürün sayısı girdiniz. Lütfen ürün sayısını en az 1 olacak şekilde girin.");
            productCount = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Ürün sayısı");
        }

        var products = new List<Product>(productCount);
        for (var i = 0; i < productCount; ++i)
        {
            var product = new Product
            {
                ProductCode = ConsoleUtilities.ObtainNonEmptyInput("Ürün kodu"),
                Count = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Sayı"),
                Price = ConsoleUtilities.ObtainInputAsDecimal("Fiyat"),
                Deci = ConsoleUtilities.ObtainInputAsInteger("Desi"),
                Weigth = ConsoleUtilities.ObtainInputAsInteger("Ağırlık"),
                Description = ConsoleUtilities.ObtainInput("Açıklama:")
            };

            products.Add(product);
        }

        request.Products = products;

        request.PaymentType = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Ödeme türü");
        request.CollectionType = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Koleksiyon türü");

        if (request.CollectionType > 0)
        {
            request.CollectionPrice = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Koleksiyon fiyatı");
        }

        request.Description = ConsoleUtilities.ObtainInput("Açıklama");
        request.ServiceType = ConsoleUtilities.ObtainInputAsInteger("Hizmet türü");
        request.BarcodeLabelType = ConsoleUtilities.ObtainInputAsInteger("Barkod etiket türü");

        Console.WriteLine("Gönderi oluşturma isteği gönderiliyor...");
        var response = await Client.SetDeliveryAsync(request, Token);

        if (response.HttpStatusCode == 200)
        {
            Console.Write("Gönderi başarıyla oluşturuldu.");

            if (response.Data?.result == null)
            {
                Console.WriteLine("Gönderi bilgileri alınamadı.");
                return;
            }
            
            Console.WriteLine("Gönderi bilgileri:");

            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Kargo takip no", response.Data.result.TrackingNumber);
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Kargo takip bağlantısı", response.Data.result.TrackingUrl);
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Barkod", response.Data.result.Barcode);
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Barkod (ZPL)", response.Data.result.BarcodeZpl);
            
            if (response.Data.result.BarcodeNumbers?.Count > 1)
            {
                Console.WriteLine("Barkodlar:");
                foreach (var barcode in response.Data.result.BarcodeNumbers)
                {
                    Console.WriteLine(barcode);
                }
            }

            if (response.Data.result.BarcodeZpls?.Count > 1)
            {
                Console.WriteLine("Barkodlar (ZPL):");
                foreach (var barcode in response.Data.result.BarcodeZpls)
                {
                    Console.WriteLine(barcode);
                }
            }
        }
        else
        {
            Console.Write("Gönderi oluşturulamadı");
            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                Console.Write($": {response.Message}");
            }
            else
            {
                Console.WriteLine(".");
            }
        }
    }

    /// <summary>
    /// Bir gönderiyi takip etmek için kullanıcıdan gerekli bilgileri alır, istek gönderir ve sonucu bildirir.
    /// </summary>
    /// <returns></returns>
    private async Task TrackDeliveryAsync()
    {
        var request = new TrackDeliveryRequest
        {
            TrackingNo = ConsoleUtilities.ObtainInputAsInteger("Kargo takip no"),
            ReferenceNo = ConsoleUtilities.ObtainInput("Müşteri referans no")
        };

        Console.WriteLine("Gönderi takibi isteği gönderiliyor...");
        var response = await Client.TrackDeliveryAsync(request, Token);

        if (response.HttpStatusCode == 200)
        {
            Console.Write("Gönderi takibi başarıyla yapıldı.");

            if (response.Data?.result == null)
            {
                Console.WriteLine("Gönderi takip bilgileri alınamadı.");
                return;
            }

            Console.WriteLine("Gönderi takip bilgileri:");

            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Kargo takip no", response.Data.result.TrackingNo.ToString());
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Müşteri referans no", response.Data.result.ReferenceNo);

            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Gönderici", response.Data.result.Sender);
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Gönderici şube", response.Data.result.DepartureBranchName);
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Alıcı", response.Data.result.Receiver);
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Alıcı şube", response.Data.result.ArrivalBranchName);
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Dağıtıcı no", response.Data.result.DealerId.ToString());

            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Gönderi açıklaması", response.Data.result.DeliveryDescription);
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Gönderi tarihi", response.Data.result.SendDate);
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Tahminî teslim tarihi", response.Data.result.DeliveryPlannedDate?.ToString("dd/MM/yyyy HH.mm.ss"));

            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Kargo sayısı", response.Data.result.Quantity.ToString());
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Desi/Ağırlık", response.Data.result.DeciWeight.ToString());
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Kargo ücreti", response.Data.result.CargoAmount.ToString(CultureInfo.CurrentCulture));
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Toplam fiyat", response.Data.result.TotalPrice.ToString(CultureInfo.CurrentCulture));

            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Durum kodu", response.Data.result.State?.ToString());
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Durum", response.Data.result.StateText);
            ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Güncelleştirilme tarihi", response.Data.result.UpdateDate);

            Console.WriteLine();

            if (response.Data.result.StatusHistories?.Count > 0)
            {
                Console.WriteLine("Gönderi hareketleri:");
                foreach (var entry in response.Data.result.StatusHistories)
                {
                    ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Durum kodu", entry.StatusId.ToString());
                    ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Durum", entry.Status);
                    ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Tarih", entry.StatusDate?.ToString("dd/MM/yyyy HH.mm.ss"));
                    ConsoleUtilities.WriteTupleIfNotNullOrWhiteSpace("Açıklama", entry.Description);
                    Console.WriteLine();
                }
            }
        }
        else if (response.HttpStatusCode is >= 400 and < 500)
        {
            Console.WriteLine("Gönderi takibi yapılamadı. Lütfen kargo takip numaranızı ve/veya referans numaranızı kontrol edip yeniden deneyin.");
        }
        else
        {
            Console.Write("Gönderi takibi yapılamadı");
            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                Console.Write($": {response.Message}");
            }
            else
            {
                Console.WriteLine(".");
            }
        }
    }

    /// <summary>
    /// Bir gönderiyi iptal etmek için kullanıcıdan gerekli bilgileri alır, istek gönderir ve sonucu bildirir.
    /// </summary>
    /// <returns></returns>
    private async Task CancelDeliveryAsync()
    {
        var request = new CancelDeliveryRequest
        {
            TrackingNo = ConsoleUtilities.ObtainInputAsInteger("Kargo takip no"),
            CustomerReferenceNo = ConsoleUtilities.ObtainInput("Müşteri referans no")
        };

        Console.WriteLine("Gönderi iptal isteği gönderiliyor...");
        var response = await Client.CancelDeliveryAsync(request, Token);

        if (response.HttpStatusCode == 200)
        {
            if (response.Data == null)
            {
                Console.WriteLine("Gönderi iptal isteği başarıyla iletildi, ancak işlemin sonucu bilinmiyor.");
                return;
            }

            if (response.Data?.result == null)
            {
                Console.Write("Gönderi başarıyla iptal edildi.");
            }
            else
            {
                Console.WriteLine("Gönderi iptal edilemedi. Lütfen kargo takip numaranızı ve/veya referans numaranızı kontrol edip yeniden deneyin.");
            }
        }
        else
        {
            Console.Write("Gönderi iptali yapılamadı");
            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                Console.Write($": {response.Message}");
            }
            else
            {
                Console.WriteLine(".");
            }
        }
    }

    /// <summary>
    /// Bir gönderinin ölçülerini güncelleştirmek için kullanıcıdan gerekli bilgileri alır, istek gönderir ve sonucu bildirir.
    /// </summary>
    /// <returns></returns>
    private async Task UpdateMeasurementsAsync()
    {
        var request = new CargoMeasurementUpdateRequest
        {
            CustomerReferenceNo = ConsoleUtilities.ObtainNonEmptyInput("Müşteri referans no")
        };

        var cargoCount = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Kargo sayısı");

        while (cargoCount <= 0)
        {
            Console.WriteLine("Geçersiz kargo sayısı girdiniz. Lütfen 0'dan büyük bir değer girin.");
            cargoCount = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Kargo sayısı");
        }

        request.Measurements = new List<Measurement>(cargoCount);
        for (var i = 0; i < cargoCount; ++i)
        {
            var measurement = new Measurement
            {
                Quantity = ConsoleUtilities.ObtainNonEmptyInputAsInteger("Sayı"),
                Width = ConsoleUtilities.ObtainInputAsInteger("Genişlik"),
                Length = ConsoleUtilities.ObtainInputAsInteger("Uzunluk"),
                Height = ConsoleUtilities.ObtainInputAsInteger("Yükseklik"),
                Deci = ConsoleUtilities.ObtainInputAsInteger("Desi"),
                Weight = ConsoleUtilities.ObtainInputAsInteger("Ağırlık")
            };

            request.Measurements.Add(measurement);
        }

        Console.WriteLine("Gönderi ölçüleri güncelleştirme isteği gönderiliyor...");
        var response = await Client.CargoMeasurementUpdateAsync(request, Token);

        if (response.HttpStatusCode == 200)
        {
            if (response.Data?.result == null)
            {
                Console.WriteLine("Gönderi ölçüleri güncelleştirme isteği başarıyla iletildi, ancak işlemin sonucu bilinmiyor.");
                return;
            }

            if (response.Data.result.IsSuccess)
            {
                Console.Write("Gönderi ölçüleri başarıyla güncelleştirildi.");
            }
            else
            {
                Console.WriteLine("Gönderi ölçüleri güncelleştirilemedi.");
            }
        }
        else
        {
            Console.Write("Gönderi iptali yapılamadı");
            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                Console.Write($": {response.Message}");
            }
            else
            {
                Console.WriteLine(".");
            }
        }
    }
}