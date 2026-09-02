Task1();
Task2();
await Task3();

void Task1()
{
    var counter = 0;
    var threads = new Thread[10];

    for (var i = 0; i < threads.Length; i++)
    {
        threads[i] = new Thread(() =>
        {
            for (var j = 0; j < 1000; j++)
            {
                Interlocked.Increment(ref counter);
            }
        });
        threads[i].Start();
    }

    foreach (var thread in threads)
    {
        thread.Join();
    }

    const int expected = 10_000;
    Console.WriteLine($"Итоговое значение счетчика: {counter}");

    if (counter != expected)
    {
        Console.WriteLine($"Ошибка: ожидалось {expected}, получено {counter}");
    }
    else
    {
        Console.WriteLine("Значение счетчика корректно");
    }
}

void Task2()
{
    using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));
    var token = cts.Token;
    var processed = 0;

    try
    {
        Parallel.For(0, 1_000_000, new ParallelOptions { CancellationToken = token }, _ =>
        {
            Interlocked.Increment(ref processed);
            Thread.SpinWait(1000);
        });
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Параллельная операция отменена");
    }

    if (processed == 0)
    {
        Console.WriteLine("Ошибка: не обработано ни одного элемента");
    }
    else
    {
        Console.WriteLine($"Обработано элементов до отмены: {processed}");
    }
}

async Task Task3()
{
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

    try
    {
        await LongRunningOperationAsync(cts.Token);
        Console.WriteLine("Асинхронная операция завершена");
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Асинхронная операция отменена");
    }
}

async Task LongRunningOperationAsync(CancellationToken token)
{
    for (var step = 1; step <= 10; step++)
    {
        token.ThrowIfCancellationRequested();
        
        await Task.Delay(500, token);
        
        Console.WriteLine($"Выполнен шаг {step}");
    }
}
