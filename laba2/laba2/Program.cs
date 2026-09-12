while (true)
{
    Console.WriteLine("1 — Обработка UriFormatException");
    Console.WriteLine("2 — Проверка значения на null");
    Console.WriteLine("3 — Проверка номера банковской карты");
    Console.WriteLine("0 — Выход");
    Console.Write("Выберите задание: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Task1();
            break;
        case "2":
            Task2();
            break;
        case "3":
            Task3();
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Неверный выбор. Введите 0, 1, 2 или 3");
            break;
    }

    Console.WriteLine();
}

void Task1()
{
    Console.WriteLine();

    try
    {
        Console.Write("Введите URL: ");
        var input = Console.ReadLine() ?? string.Empty;
        var uri = new Uri(input);
        Console.WriteLine($"URI корректен: {uri}");
    }
    catch (UriFormatException ex)
    {
        Console.WriteLine($"UriFormatException: неверный формат URI — {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
    }
    finally
    {
        Console.WriteLine("Блок finally выполнен");
    }
}

void Task2()
{
    Console.WriteLine();

    try
    {
        Console.Write("Введите значение (или оставьте пустым для null): ");
        var input = Console.ReadLine();

        var value = string.IsNullOrWhiteSpace(input) ? null : input;
        ValidateNotNull(value);
        Console.WriteLine($"Значение принято: {value}");
    }
    catch (NullReferenceException ex)
    {
        Console.WriteLine($"NullReferenceException: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
}

void Task3()
{
    Console.WriteLine();

    try
    {
        Console.Write("Введите номер банковской карты: ");
        var cardNumber = Console.ReadLine() ?? string.Empty;

        ValidateCardNumber(cardNumber);
        Console.WriteLine($"Номер карты корректен: {cardNumber}");
    }
    catch (InvalidCardNumberException ex)
    {
        Console.WriteLine($"InvalidCardNumberException: {ex.Message}");
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

void ValidateNotNull(string? value)
{
    if (value is null)
    {
        throw new NullReferenceException("Значение переменной не может быть null");
    }
}

void ValidateCardNumber(string cardNumber)
{
    if (string.IsNullOrWhiteSpace(cardNumber))
    {
        throw new InvalidCardNumberException("Номер карты не может быть пустым");
    }

    if (cardNumber.Length != 16)
    {
        throw new InvalidCardNumberException("Номер карты должен содержать 16 цифр");
    }

    if (cardNumber[0] == '0')
    {
        throw new InvalidCardNumberException("Номер карты не должен начинаться на 0");
    }

    if (cardNumber.Any(digit => !char.IsDigit(digit)))
    {
        throw new InvalidCardNumberException("Номер карты должен содержать только цифры");
    }
}

internal class InvalidCardNumberException : Exception
{
    public InvalidCardNumberException(string message) : base(message)
    {
    }
}
