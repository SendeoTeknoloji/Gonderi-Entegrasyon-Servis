using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Core.Models;
using Core.Models.Request;
using Core.Models.Response;
using Core.Utilities;
using Newtonsoft.Json;

namespace Core;

/// <summary>
/// Sendeo Entegrasyon uygulamasının sunduğu genel entegrasyon uç noktalarına erişimleri tanımlar.
/// </summary>
public class Client
{
    /// <summary>
    /// Sendeo Entegrasyon ile iletişimi sağlayacak HTTP istemcisi.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Sendeo Entegrasyon uygulamasına ulaşılan taban URL.
    /// </summary>
    /// <remarks> Bu bir taban URL olduğu için yalnızca kök adres [ve varsa kapı (port)] kullanılır, örneğin "https://www.site.com".</remarks>
    public string BaseUrl { get; set; } = "https://localhost:5001";

    /// <summary>
    /// Sendeo Entegrasyon uygulamasıyla iletişime geçecek bir istemci oluşturur.
    /// </summary>
    public Client()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    /// <summary>
    /// Uç noktaları kullanabilmek için gereken yetkilendirme biletinin alındığı uç noktadır.
    /// </summary>
    /// <param name="request"> Giriş isteği. </param>
    /// <returns> Giriş işleminin sonucunu döndürür. </returns>
    public async Task<ResponseDataOfTokenResponseData> LoginAsync(LoginRequest request)
    {
        var result = new ResponseDataOfTokenResponseData();

        try
        {
            var requestAsJson = JsonConvert.SerializeObject(request);

            _httpClient.DefaultRequestHeaders.Clear();

            var response = await _httpClient.PostAsync($"{BaseUrl}/api/Token/Login",
                new StringContent(requestAsJson, Encoding.UTF8, MediaTypeNames.Application.Json)
                );
            var responseStream = await response.Content.ReadAsStreamAsync();

            if (responseStream == null)
            {
                throw new ApiException("Giriş yapma adresinden boş cevap döndürüldü.");
            }

            var responseData = response.Content.Headers.ContentType switch
            {
                {MediaType: MediaTypeNames.Application.Xml} => XmlDeserializer.DeserializeObject<ResponseDataOfTokenResponseData>(responseStream),
                _ => throw new ApiException("Giriş yapma adresinden dönen cevap XML biçiminde değil.")
            };

            result = responseData ?? throw new ApiException("Giriş yapma adresinden dönen cevap, XML biçimine dönüştürülemedi.");
        }
        catch (Exception? e)
        {
            result.StatusCode = -1;
            result.ExceptionMessage = $"{e.Message}{result.ExceptionMessage}";
        }

        return result;
    }

    /// <summary>
    /// Bir gönderinin oluşturulduğu uç noktadır.
    /// </summary>
    /// <param name="request"> Gönderi oluşturma isteği. </param>
    /// <param name="token"> Yetkilendirme bileti. </param>
    /// <returns> Gönderi oluşturma işleminin sonucunu döndürür. </returns>
    public async Task<ResponseWrapper<SetDeliveryResponse>> SetDeliveryAsync(SetDeliveryRequest request, string? token)
    {
        return await SendPostRequestAsync<SetDeliveryResponse>("api/Cargo/SETDELIVERY", request, token);
    }

    /// <summary>
    /// Bir gönderinin durumunun değiştirildiği uç noktadır.
    /// </summary>
    /// <param name="request"> Gönderi takibi isteği. </param>
    /// <param name="token"> Yetkilendirme bileti. </param>
    /// <returns> Gönderi takibi işleminin sonucunu döndürür. </returns>
    public async Task<ResponseWrapper<TrackDeliveryResponse>> TrackDeliveryAsync(TrackDeliveryRequest request, string? token)
    {
        var queryValues = new Dictionary<string, string>();

        if (request.TrackingNo.HasValue)
        {
            queryValues.Add("trackingNo", request.TrackingNo.Value.ToString());
        }

        if (!string.IsNullOrWhiteSpace(request.ReferenceNo))
        {
            queryValues.Add("referenceNo", request.ReferenceNo);
        }

        return await SendGetRequestAsync<TrackDeliveryResponse>("api/Cargo/TRACKDELIVERY", queryValues, token);
    }

    /// <summary>
    /// Bir gönderinin iptal edildiği uç noktadır.
    /// </summary>
    /// <param name="request"> Gönderi iptali isteği. </param>
    /// <param name="token"> Yetkilendirme bileti. </param>
    /// <returns> Gönderi iptali işleminin sonucunu döndürür. </returns>
    public async Task<ResponseWrapper<CancelDeliveryResponse>> CancelDeliveryAsync(CancelDeliveryRequest request, string? token)
    {
        return await SendPostRequestAsync<CancelDeliveryResponse>("api/Cargo/CANCELDELIVERY", request, token);
    }

    /// <summary>
    /// Gönderi ölçülerinin güncelleştirildiği uç noktadır.
    /// </summary>
    /// <param name="request"> Gönderi ölçüleri güncelleştirme isteği. </param>
    /// <param name="token"> Yetkilendirme bileti. </param>
    /// <returns> Gönderi ölçüleri güncelleştirme isteği işleminin sonucunu döndürür. </returns>
    public async Task<ResponseWrapper<CargoMeasurementUpdateResponse>> CargoMeasurementUpdateAsync(CargoMeasurementUpdateRequest request, string? token)
    {
        return await SendPostRequestAsync<CargoMeasurementUpdateResponse>("api/Cargo/CARGOMEASUREMENTUPDATE", request, token);
    }

    /// <summary>
    /// Sendeo Entegrasyon'a bir GET isteği gönderir.
    /// </summary>
    /// <typeparam name="T"> Uç noktadan beklenen cevabın dönüştürüleceği nesne. </typeparam>
    /// <param name="uri"> Uç noktanın adresi. İstek "<see cref="BaseUrl"/>/<paramref name="uri"/>" adresine gönderilir. Eğer "/" karakteriyle başlıyorsa kaldırılır.</param>
    /// <param name="arguments"> Gönderilecek anahtar-değer ikililerini içeren sözlük. </param>
    /// <param name="token"> Yetkilendirme bileti. Kimlik doğrulama (giriş yapma) istekleri için boş bırakılır. </param>
    /// <returns> Sendeo Entegrasyon'dan dönen cevabın <typeparamref name="T"/> nesnesine dönüştürülmüş biçimini döndürür. </returns>
    /// <exception cref="ApiException"> API'den bir hata gelirse fırlatılır. </exception>
    private async Task<ResponseWrapper<T>> SendGetRequestAsync<T>(string uri, IDictionary<string, string> arguments, string? token = null)
    {
        var result = new ResponseWrapper<T>();

        try
        {
            if (uri.StartsWith("/")) uri = uri[1..];

            _httpClient.DefaultRequestHeaders.Clear();

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var requestAddress = $"{BaseUrl}/{uri}";
            if (arguments.Count > 0)
            {
                var queryString = "";
                foreach (var (key,value) in arguments)
                {
                    queryString += $"{key}={value}&";
                }

                requestAddress = $"{requestAddress}?{queryString[..^2]}";
            }

            var response = await _httpClient.GetAsync(requestAddress);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseStream = await response.Content.ReadAsStreamAsync();

            if (string.IsNullOrWhiteSpace(responseContent) || responseStream == null)
            {
                throw new ApiException($"{uri} adresinden boş cevap döndürüldü.");
            }

            var resultData = response.Content.Headers.ContentType switch
            {
                {MediaType: MediaTypeNames.Application.Xml} => XmlDeserializer.DeserializeObject<ResponseBase<T>>(responseStream),
                {MediaType: MediaTypeNames.Application.Json} => JsonConvert.DeserializeObject<ResponseBase<T>>(responseContent),
                _ => throw new ApiException($"{uri} adresinden dönen cevap XML veya JSON biçiminde değil.")
            };

            if (resultData == null)
            {
                throw new ApiException($"{uri} adresinden dönen cevap, XML veya JSON biçimine dönüştürülemedi.");
            }

            result.HttpStatusCode = (int)response.StatusCode;
            result.Data = resultData;

            return result;
        }
        catch (Exception? e)
        {
            result.HttpStatusCode = null;
            result.Message = e.Message;
            e = e.InnerException;

            while (e != null)
            {
                result.Message += $" -> {e.Message}";
                e = e.InnerException;
            }
        }

        return result;
    }

    /// <summary>
    /// Sendeo Entegrasyon'a bir POST isteği gönderir.
    /// </summary>
    /// <typeparam name="T"> Uç noktadan beklenen cevabın dönüştürüleceği nesne. </typeparam>
    /// <param name="uri"> Uç noktanın adresi. İstek "<see cref="BaseUrl"/>/<paramref name="uri"/>" adresine gönderilir. Eğer "/" karakteriyle başlıyorsa kaldırılır.</param>
    /// <param name="request"> Sendeo Entegrasyon'a gönderilen isteğin içeriğinde bulunan nesne. </param>
    /// <param name="token"> Yetkilendirme bileti. Kimlik doğrulama (giriş yapma) istekleri için boş bırakılır. </param>
    /// <returns> Sendeo Entegrasyon'dan dönen cevabın <typeparamref name="T"/> nesnesine dönüştürülmüş biçimini döndürür. </returns>
    /// <exception cref="ApiException"> API'den bir hata gelirse fırlatılır. </exception>
    private async Task<ResponseWrapper<T>> SendPostRequestAsync<T>(string uri, object request, string? token = null)
    {
        var result = new ResponseWrapper<T>();

        try
        {
            if (uri.StartsWith("/")) uri = uri[1..];

            var requestAsJson = JsonConvert.SerializeObject(request);

            _httpClient.DefaultRequestHeaders.Clear();

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsync(
                $"{BaseUrl}/{uri}",
                new StringContent(requestAsJson, Encoding.UTF8, MediaTypeNames.Application.Json)
                );
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseStream = await response.Content.ReadAsStreamAsync();

            if (string.IsNullOrWhiteSpace(responseContent) || responseStream == null)
            {
                throw new ApiException($"{uri} adresinden boş cevap döndürüldü.");
            }

            var resultData = response.Content.Headers.ContentType switch
            {
                {MediaType: MediaTypeNames.Application.Xml} => XmlDeserializer.DeserializeObject<ResponseBase<T>>(responseStream),
                {MediaType: MediaTypeNames.Application.Json} => JsonConvert.DeserializeObject<ResponseBase<T>>(responseContent),
                _ => throw new ApiException($"{uri} adresinden dönen cevap XML veya JSON biçiminde değil.")
            };

            if (resultData == null)
            {
                throw new ApiException($"{uri} adresinden dönen cevap, XML veya JSON biçimine dönüştürülemedi.");
            }

            result.HttpStatusCode = (int?)response.StatusCode;
            result.Data = resultData;
            result.Message = "";
        }
        catch (Exception? e)
        {
            result.HttpStatusCode = null;
            result.Message = e.Message;
            e = e.InnerException;

            while (e != null)
            {
                result.Message += $" -> {e.Message}";
                e = e.InnerException;
            }
        }

        return result;
    }
}