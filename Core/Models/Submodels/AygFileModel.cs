namespace Core.Models.Submodels;

/// <summary>
/// Gönderi oluşturma modelinde gönderilebilecek dosyalar hakkında gerekli bilgileri barındıran alanları tanımlar.
/// </summary>
public class AygFileModel
{
    /// <summary>
    /// Dosyanın Base64 değeri.
    /// </summary>
    public string? Base64File { get; set; }

    /// <summary>
    /// Dosyanın uzantısı.
    /// </summary>
    public string? Extension { get; set; }

    /// <summary>
    /// Dosyanın adı.
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Dosyanın bayt cinsinden uzunluğu.
    /// </summary>
    public long FileLength { get; set; }
}