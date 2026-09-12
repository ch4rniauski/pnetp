using laba1;

while (true)
{
    Console.WriteLine("1 — Перевод секунд в часы и минуты");
    Console.WriteLine("2 — Класс Time");
    Console.WriteLine("3 — Наследование и полиморфизм");
    Console.WriteLine("0 — Выход");
    Console.Write("Выберите задание: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            RunSecondsConvert();
            break;
        case "2":
            RunTimeDemo();
            break;
        case "3":
            RunFigureDemo();
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Неверный выбор. Введите 0, 1, 2 или 3");
            break;
    }
}

void RunSecondsConvert()
{
    Console.Write("Введите количество секунд: ");
    var input = Console.ReadLine();

    if (!int.TryParse(input, out var totalSeconds) || totalSeconds < 0)
    {
        Console.WriteLine("Введите целое число секунд (0 или больше)");
        return;
    }

    var hours = totalSeconds / 3600;
    var minutes = (totalSeconds % 3600) / 60;
    var seconds = totalSeconds % 60;

    Console.WriteLine($"{totalSeconds} сек. = {hours} ч. {minutes} мин. {seconds} сек.");
}

void RunTimeDemo()
{
    Console.Write("Часы (0-23): ");
    var hoursText = Console.ReadLine();

    Console.Write("Минуты (0-59): ");
    var minutesText = Console.ReadLine();

    if (!Time.TryCreate(hoursText, minutesText, out var time, out var error))
    {
        Console.WriteLine(error);
        return;
    }

    time!.PrintInfo();

    Console.Write("Сколько минут добавить: ");
    if (!int.TryParse(Console.ReadLine(), out var minutesToAdd))
    {
        Console.WriteLine("Введите целое число");
        return;
    }

    var newTime = time.AddMinutes(minutesToAdd);
    Console.Write("После добавления минут: ");
    newTime.PrintInfo();
}

void RunFigureDemo()
{
    var figures = new Figure[]
    {
        new Triangle(),
        new Rectangle(),
        new Triangle(),
        new Rectangle()
    };

    foreach (var figure in figures)
    {
        figure.Draw();
        Console.WriteLine("-----------------------------");
    }
}
