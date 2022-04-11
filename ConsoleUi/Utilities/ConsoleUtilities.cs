namespace ConsoleUi.Utilities
{
    /// <summary>
    /// Kullanıcıdan alınan girdiler üzerinde işlemler yapan yöntemler barındırır.
    /// </summary>
    public static class ConsoleUtilities
    {
        /// <summary>
        /// Kullanıcıdan bir girdi alır.
        /// </summary>
        /// <param name="inputName"> Alınacak girdinin adı. </param>
        /// <param name="allowEmptyInput"> <see langword="null"/> veya beyaz boşluk karakterlerinden oluşan girdiye izin verilip verilmeyeceğini belirten bayrak. </param>
        /// <param name="isOptional"> Girdinin isteğe bağlı olup olmadığını belirten bayrak. </param>
        /// <returns> Kullanıcıdan alınan girdiyi döndürür. </returns>
        public static string? ObtainInput(string inputName, bool allowEmptyInput = true, bool isOptional = false)
        {
            Console.Write($"{inputName}");
            if (isOptional)
            {
                Console.Write(" (isteğe bağlı)");
            }
            Console.Write(": ");

            var result = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(result) && !allowEmptyInput)
            {
                Console.WriteLine($"{inputName} boş olamaz. Lütfen bir değer girin.");
                
                Console.Write($"{inputName}");
                if (isOptional)
                {
                    Console.Write(" (isteğe bağlı)");
                }
                Console.Write(": ");

                result = Console.ReadLine();
            }

            return result;
        }

        /// <summary>
        /// Kullanıcıdan boş olmayan bir girdi alır.
        /// </summary>
        /// <param name="inputName"> Alınacak girdinin adı. </param>
        /// <returns> Kullanıcıdan alınan girdiyi döndürür. </returns>
        public static string? ObtainNonEmptyInput(string inputName)
        {
            return ObtainInput(inputName, false);
        }

        /// <summary>
        /// Kullanıcıdan bir girdi alır ve tam sayıya dönüştürür.
        /// </summary>
        /// <param name="inputName"> Alınacak girdinin adı. </param>
        /// <param name="allowEmptyInput"> <see langword="null"/> olan veya beyaz boşluk karakterlerinden oluşan girdiye izin verilip verilmeyeceğini belirten bayrak. </param>
        /// <param name="isOptional"> Girdinin isteğe bağlı olup olmadığını belirten bayrak. </param>
        /// <returns> Kullanıcıdan alınan girdinin tam sayıya dönüştürülmüş hâlini döndürür. </returns>
        public static int ObtainInputAsInteger(string inputName, bool allowEmptyInput = true, bool isOptional = false)
        {
            var conversion = int.TryParse(ObtainInput(inputName, allowEmptyInput, isOptional), out var result);

            while (!conversion)
            {
                Console.WriteLine("Geçersiz bir değer girdiniz. Lütfen yeniden deneyin.");
                conversion = int.TryParse(ObtainNonEmptyInput(inputName), out result);
            }

            return result;
        }

        /// <summary>
        /// Kullanıcıdan boş olmayan bir girdi alır ve tam sayıya dönüştürür.
        /// </summary>
        /// <param name="inputName"> Alınacak girdinin adı. </param>
        /// <returns> Kullanıcıdan alınan girdinin tam sayıya dönüştürülmüş hâlini döndürür. </returns>
        public static int ObtainNonEmptyInputAsInteger(string inputName)
        {
            return ObtainInputAsInteger(inputName, false);
        }

        /// <summary>
        /// Kullanıcıdan bir girdi alır ve ondalıklı sayıya dönüştürür.
        /// </summary>
        /// <param name="inputName"> Alınacak girdinin adı. </param>
        /// <param name="allowEmptyInput"> <see langword="null"/> olan veya beyaz boşluk karakterlerinden oluşan girdiye izin verilip verilmeyeceğini belirten bayrak. </param>
        /// <param name="isOptional"> Girdinin isteğe bağlı olup olmadığını belirten bayrak. </param>
        /// <returns> Kullanıcıdan alınan girdinin ondalıklı sayıya dönüştürülmüş hâlini döndürür. </returns>
        public static decimal ObtainInputAsDecimal(string inputName, bool allowEmptyInput = true, bool isOptional = false)
        {
            var conversion = decimal.TryParse(ObtainInput(inputName, allowEmptyInput, isOptional), out var result);

            while (!conversion)
            {
                Console.WriteLine("Geçersiz bir değer girdiniz. Lütfen yeniden deneyin.");
                conversion = decimal.TryParse(ObtainNonEmptyInput(inputName), out result);
            }

            return result;
        }

        /// <summary>
        /// <see cref="value"/> dizesini, <see langword="null"/> değilse veya beyaz boşluk karakterlerinden oluşmuyorsa, "<see cref="header"/>: <see cref="value"/>" biçiminde konsola yazdırır.
        /// </summary>
        /// <param name="header"> Başlık. </param>
        /// <param name="value"> Yazdırılacak değer. </param>
        /// <param name="newLine"> Değerden sonra yeni satır karakterlerinin koyulup koyulmayacağını belirten bayrak. </param>
        public static void WriteTupleIfNotNullOrWhiteSpace(string header, string? value, bool newLine = false)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            Console.Write($"{header}: {value}");

            if (newLine)
                Console.WriteLine();
        }
    }
}
