using System.Text;
using System.Xml.Serialization;

namespace Core.Utilities;

/// <summary>
/// XML içeren bir dizeyi istenen türde bir nesneye dönüştürecek bir yöntemi barındırır.
/// </summary>
public static class XmlDeserializer
{
    /// <summary>
    /// XML verisi içeren bir dizeyi bir nesneye dönüştürür.
    /// </summary>
    /// <typeparam name="T"> XML verisinin dönüştürüleceği nesne. </typeparam>
    /// <param name="input"> Dönüştürülecek XML verisini içeren dize. </param>
    /// <returns> XML verisinin <typeparamref name="T"/> nesnesine dönüştürülmüş hâlini döndürür. </returns>
    /// <exception cref="ArgumentException"> <paramref name="input"/>, <typeparamref name="T"/> nesnesine dönüştürülemezse fırlatılır. </exception>
    public static T DeserializeObject<T>(string input)
    {
        var xmlSerializer = new XmlSerializer(typeof(T));
        T? intermediateObject;

        using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(input)))
        {
            intermediateObject = (T?)xmlSerializer.Deserialize(stream);
        }

        return intermediateObject ?? throw new ArgumentException("Verilen dize XML'den istenen nesneye dönüştürülemedi.", nameof(input));
    }

    /// <summary>
    /// XML verisi içeren bir akışı bir nesneye dönüştürür.
    /// </summary>
    /// <typeparam name="T"> XML verisinin dönüştürüleceği nesne. </typeparam>
    /// <param name="input"> Dönüştürülecek XML verisini içeren akış. </param>
    /// <returns> XML verisinin <typeparamref name="T"/> nesnesine dönüştürülmüş hâlini döndürür. </returns>
    /// <exception cref="ArgumentException"> <paramref name="input"/>, <typeparamref name="T"/> nesnesine dönüştürülemezse fırlatılır. </exception>
    public static T? DeserializeObject<T>(Stream input)
    {
        try
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var intermediateObject = (T?) xmlSerializer.Deserialize(input);
            return intermediateObject;
        }
        catch (InvalidOperationException e)
        {
            throw new ArgumentException("Verilen akış, XML'den istenen nesneye dönüştürülemedi.", nameof(input), e);
        }
    }
}