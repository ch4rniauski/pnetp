while (true)
{
    Console.WriteLine("1 — CountdownEvent");
    Console.WriteLine("2 — ParallelOptions");
    Console.WriteLine("3 — ValueTask");
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
            Console.WriteLine(await Task3());
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

    const int threadCount = 5;
    using var countdown = new CountdownEvent(threadCount);

    for (var i = 1; i <= threadCount; i++)
    {
        var id = i;
        var thread = new Thread(() =>
        {
            Console.WriteLine($"Поток {id} начал работу");
            Thread.Sleep(500 * id);
            Console.WriteLine($"Поток {id} завершил работу");
            countdown.Signal();
        });
        thread.Start();
    }

    Console.WriteLine("Ожидание завершения всех потоков");
    countdown.Wait();
    Console.WriteLine("Все потоки завершены");
}

void Task2()
{
    Console.WriteLine();

    Console.Write("Введите максимальную степень параллелизма (положительное число или -1 без ограничений): ");
    var input = Console.ReadLine();

    if (!int.TryParse(input, out var maxDegree) || (maxDegree < 1 && maxDegree != -1))
    {
        Console.WriteLine("Ошибка: введите положительное целое число или -1");
        return;
    }

    var options = new ParallelOptions
    {
        MaxDegreeOfParallelism = maxDegree
    };

    var current = 0;
    var peak = 0;
    var lockObj = new object();

    Parallel.For(0, 20, options, i =>
    {
        int running;
        lock (lockObj)
        {
            current++;
            running = current;
            if (running > peak)
            {
                peak = running;
            }
        }

        Console.WriteLine($"Итерация {i}, поток {Environment.CurrentManagedThreadId}, одновременно: {running}");
        Thread.Sleep(100);

        lock (lockObj)
        {
            current--;
        }
    });

    Console.WriteLine($"Максимальная степень параллелизма: {maxDegree}");
    Console.WriteLine($"Пиковое число одновременных потоков: {peak}");
}

async ValueTask<string> Task3()
{
    Console.WriteLine();

    Console.Write("Введите ключ для получения данных: ");
    var key = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(key))
    {
        return "Ошибка: ключ не может быть пустым";
    }

    var useAsync = false;

    if (useAsync)
    {
        var result = await Task.Run(async () =>
        {
            await Task.Delay(1000);
            return $"Результат для ключа '{key}'";
        });
        
        return result;
    }

    var value = $"Результат для ключа '{key}'";
    Console.WriteLine("Данные получены синхронно");
    
    return value;
}
