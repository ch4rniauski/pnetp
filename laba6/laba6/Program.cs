while (true)
{
    Console.WriteLine("1 — stackalloc и Span<byte>");
    Console.WriteLine("2 — Частота сборки Gen 0");
    Console.WriteLine("3 — Потокобезопасный ресурс");
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
    
    Span<byte> buffer = stackalloc byte[256];
    buffer.Fill(2);

    var sum = 0;
    foreach (var t in buffer)
    {
        sum += t;
    }

    Console.WriteLine($"Размер буфера на стеке: {buffer.Length} байт");
    Console.WriteLine($"Первый байт: {buffer[0]}");
    Console.WriteLine($"Последний байт: {buffer[^1]}");
    Console.WriteLine($"Сумма байтов: {sum}");
}

void Task2()
{
    Console.WriteLine();

    const int objectCount = 1_000_000;
    var collectionsBefore = GC.CollectionCount(0);

    for (var i = 0; i < objectCount; i++)
    {
        _ = new object();
    }

    var collectionsAfter = GC.CollectionCount(0);
    var collections = collectionsAfter - collectionsBefore;

    Console.WriteLine($"Создано мелких объектов: {objectCount}");
    Console.WriteLine($"Сборок Gen 0 до: {collectionsBefore}");
    Console.WriteLine($"Сборок Gen 0 после: {collectionsAfter}");
    Console.WriteLine($"Произошло сборок Gen 0: {collections}");
}

void Task3()
{
    Console.WriteLine();

    using var resource = new ThreadSafeResource("data.txt");

    var threads = new Thread[5];
    for (var i = 0; i < threads.Length; i++)
    {
        var threadId = i + 1;
        threads[i] = new Thread(() =>
        {
            for (var j = 0; j < 5; j++)
            {
                resource.WriteLine($"Поток {threadId}, запись {j + 1}");
            }
        });
        threads[i].Start();
    }

    foreach (var t in threads)
    {
        t.Join();
    }

    Console.WriteLine($"Всего записей: {resource.WriteCount}");
    Console.WriteLine("Ресурс использован в нескольких потоках");
}

internal class ThreadSafeResource : IDisposable
{
    private readonly object _sync = new();
    private StreamWriter? _writer;
    private bool _disposed;

    public int WriteCount { get; private set; }

    public ThreadSafeResource(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));
        }

        _writer = new StreamWriter(filePath, append: false);
    }

    public void WriteLine(string text)
    {
        lock (_sync)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (_writer is null)
            {
                throw new InvalidOperationException("Ресурс недоступен");
            }

            _writer.WriteLine(text);
            WriteCount++;
        }
    }

    ~ThreadSafeResource()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        lock (_sync)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _writer?.Dispose();
                _writer = null;
            }

            _disposed = true;
        }
    }
}
