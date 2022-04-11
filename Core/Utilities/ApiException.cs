namespace Core.Utilities;

/// <summary>
/// Bir API'den beklenmeyen bir cevap gelmesi durumunda oluşan hataları temsil eder.
/// </summary>
public class ApiException : Exception
{
    /// <summary>
    /// Bir <see cref="ApiException"/> istisnası oluşturur.
    /// </summary>
    /// <param name="message"> Hata ile ilgili bilgi. </param>
    public ApiException(string message) : base(message) { }
}