Task1();
Console.WriteLine();
Task2();
Console.WriteLine();
Task3();

void Task1()
{
    var readOnlyFilePath = Path.Combine(Path.GetTempPath(), $"laba2_readonly_{Guid.NewGuid():N}.txt");
    File.WriteAllText(readOnlyFilePath, "test");
    File.SetAttributes(readOnlyFilePath, FileAttributes.ReadOnly);

    try
    {
        File.WriteAllText(readOnlyFilePath, "новые данные");
        Console.WriteLine("Файл успешно записан.");
    }
    catch (UnauthorizedAccessException ex)
    {
        Console.WriteLine($"UnauthorizedAccessException: нет доступа — {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
    }
    finally
    {
        Console.WriteLine("Блок finally выполнен (задание 1).");

        if (File.Exists(readOnlyFilePath))
        {
            File.SetAttributes(readOnlyFilePath, FileAttributes.Normal);
            File.Delete(readOnlyFilePath);
        }
    }
}

void Task2()
{
    try
    {
        Console.Write("Введите строку: ");
        var input = Console.ReadLine() ?? string.Empty;

        Console.Write("Минимальная длина: ");
        var minLength = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Максимальная длина: ");
        var maxLength = int.Parse(Console.ReadLine() ?? "0");

        ValidateStringLength(input, minLength, maxLength);
        Console.WriteLine($"Строка принята. Длина: {input.Length}");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"ArgumentOutOfRangeException: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
}

void Task3()
{
    try
    {
        Console.Write("Введите URL: ");
        var url = Console.ReadLine() ?? string.Empty;

        ValidateUrl(url);
        Console.WriteLine($"URL корректен: {url}");
    }
    catch (InvalidUrlException ex)
    {
        Console.WriteLine($"InvalidUrlException: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
    finally
    {
        Console.WriteLine("Блок finally выполнен");
    }
}

void ValidateStringLength(string text, int minLength, int maxLength)
{
    if (minLength < 0 || maxLength < minLength)
    {
        throw new ArgumentException("Некорректный диапазон длины");
    }

    if (text.Length < minLength || text.Length > maxLength)
    {
        throw new ArgumentOutOfRangeException($"Длина строки должна быть от {minLength} до {maxLength} символов. Текущая длина: {text.Length}");
    }
}

void ValidateUrl(string url)
{
    if (string.IsNullOrWhiteSpace(url))
    {
        throw new InvalidUrlException("URL не может быть пустым");
    }

    if (!url.StartsWith("http://") && !url.StartsWith("https://"))
    {
        throw new InvalidUrlException("URL должен начинаться с http:// или https://");
    }
}

internal class InvalidUrlException : Exception
{
    public InvalidUrlException(string message) : base(message)
    {
    }
}
