using laba1;

while (true)
{
    Console.WriteLine("1 — Проверка строки на палиндром");
    Console.WriteLine("2 — Класс Dog");
    Console.WriteLine("3 — Наследование и полиморфизм");
    Console.WriteLine("0 — Выход");
    Console.Write("Выберите задание: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            RunPalindromeCheck();
            break;
        case "2":
            RunDogDemo();
            break;
        case "3":
            RunPaymentDemo();
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Неверный выбор. Введите 0, 1, 2 или 3.");
            break;
    }
}

void RunPalindromeCheck()
{
    Console.Write("Введите строку: ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Строка не может быть пустой.");
        return;
    }

    var result = IsPalindrome(input);
    Console.WriteLine(result ? "Строка является палиндромом." : "Строка не является палиндромом.");
}

bool IsPalindrome(string text)
{
    var cleaned = text.ToLower().Replace(" ", "");

    for (var i = 0; i < cleaned.Length / 2; i++)
    {
        if (cleaned[i] != cleaned[cleaned.Length - 1 - i])
        {
            return false;
        }
    }

    return true;
}

void RunDogDemo()
{
    Console.Write("Имя хозяина: ");
    var ownerName = Console.ReadLine();

    Console.Write("Кличка собаки: ");
    var nickname = Console.ReadLine();

    Console.Write("Порода: ");
    var breed = Console.ReadLine();

    if (!Dog.TryCreate(ownerName, nickname, breed, out var dog, out var error))
    {
        Console.WriteLine(error);
        return;
    }

    Console.WriteLine($"Собака: {dog!.Nickname}, порода: {dog.Breed}, хозяин: {dog.OwnerName}");
    Console.Write("Сколько раз лаять: ");

    if (int.TryParse(Console.ReadLine(), out var count))
    {
        Dog.Bark(count);
    }
    else
    {
        Console.WriteLine("Введите целое число.");
    }
}

void RunPaymentDemo()
{
    var payments = new PaymentMethod[]
    {
        new CreditCard(1500, "**** 1234"),
        new PayPal(2500, "user@mail.com"),
        new CreditCard(500, "**** 5678"),
        new PayPal(100, "shop@paypal.com")
    };

    foreach (var payment in payments)
    {
        payment.ProcessPmnt();
        Console.WriteLine("-----------------------------");
    }
}
