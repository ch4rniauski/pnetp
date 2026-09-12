while (true)
{
    Console.WriteLine("1 — Демонстрация ArraySegment<T>");
    Console.WriteLine("2 — Вычисление факториала 5!");
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
        case "0":
            return;
        default:
            Console.WriteLine("Неверный выбор. Введите 0, 1 или 2");
            break;
    }

    Console.WriteLine();
}

void Task1()
{
    Console.WriteLine();
    
    var array = new[] { 10, 20, 30, 40, 50, 60, 70 };

    Console.Write("Исходный массив: ");
    foreach (var x in array)
    {
        Console.Write($"{x} ");
    }
    Console.WriteLine();

    var segment = new ArraySegment<int>(array, 2, 3);

    Console.WriteLine($"Смещение сегмента: {segment.Offset}");
    Console.WriteLine($"Количество элементов: {segment.Count}");

    Console.Write("Элементы сегмента: ");
    foreach (var t in segment)
    {
        Console.Write($"{t} ");
    }
    Console.WriteLine();

    segment[0] = 99;
    segment[1] = 88;
    segment[2] = 77;

    Console.Write("Массив после изменения сегмента: ");
    foreach (var t in array)
    {
        Console.Write($"{t} ");
    }
    Console.WriteLine();
}

void Task2()
{
    Console.WriteLine();

    const int n = 5;
    var factorial = 1.0;

    for (var i = 1; i <= n; i++)
    {
        factorial = Math.Round(factorial * i);
    }

    Console.WriteLine($"{n}! = {factorial}");
}
