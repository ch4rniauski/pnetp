Task1();
Console.WriteLine();
Task2();
Console.WriteLine();
Task3();

void Task1()
{
    const int iterations = 10_000;
    const int poolSize = 100;
    var pool = new PersonPool(poolSize);
    var memoryBefore = GC.GetTotalMemory(true);

    for (var i = 0; i < iterations; i++)
    {
        var person = pool.Get();
        person.Name = $"User{i}";
        person.Age = i % 100;
        pool.Return(person);
    }

    var memoryAfter = GC.GetTotalMemory(false);

    Console.WriteLine($"Память до: {memoryBefore} байт");
    Console.WriteLine($"Память после: {memoryAfter} байт");
    Console.WriteLine($"Размер пула: {poolSize}");
    Console.WriteLine($"Создано новых объектов: {pool.CreatedCount}");
    Console.WriteLine($"Повторно использовано из пула: {pool.ReusedCount}");
    Console.WriteLine($"Всего операций: {iterations}");
}

void Task2()
{
    var gen2Obj = new object();
    GC.Collect(0);
    GC.WaitForPendingFinalizers();
    GC.Collect(1);
    GC.WaitForPendingFinalizers();

    var gen1Obj = new object();
    GC.Collect(0);
    GC.WaitForPendingFinalizers();

    var tracked = new List<object>();
    for (var i = 0; i < 500; i++)
    {
        tracked.Add(new object());
    }

    tracked.Add(gen1Obj);
    tracked.Add(gen2Obj);

    PrintGenerationCounts(tracked, "До GC.Collect()");
    GC.Collect();
    GC.WaitForPendingFinalizers();
    PrintGenerationCounts(tracked, "После GC.Collect()");
}

void PrintGenerationCounts(List<object> objects, string label)
{
    var gen0 = 0;
    var gen1 = 0;
    var gen2 = 0;

    foreach (var obj in objects)
    {
        var generation = GC.GetGeneration(obj);
        switch (generation)
        {
            case 0:
                gen0++;
                break;
            case 1:
                gen1++;
                break;
            default:
                gen2++;
                break;
        }
    }

    Console.WriteLine($"{label}: Gen 0 = {gen0}, Gen 1 = {gen1}, Gen 2 = {gen2}");
}

void Task3()
{
    ResourceHolder.ResetFinalizerFlag();

    using (new ResourceHolder())
    {
    }

    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();

    Console.WriteLine(ResourceHolder.FinalizerCalled 
        ? "Финализатор был вызван после Dispose"
        : "Финализатор не вызван после явного Dispose");
}

internal class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }

    public void Reset()
    {
        Name = string.Empty;
        Age = 0;
    }
}

internal class PersonPool
{
    private readonly Stack<Person> _pool = new();
    private readonly int _maxSize;

    public int CreatedCount { get; private set; }
    public int ReusedCount { get; private set; }

    public PersonPool(int maxSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxSize);

        _maxSize = maxSize;
    }

    public Person Get()
    {
        if (_pool.Count > 0)
        {
            ReusedCount++;
            return _pool.Pop();
        }

        CreatedCount++;
        return new Person();
    }

    public void Return(Person? person)
    {
        if (person is null)
        {
            return;
        }

        person.Reset();
        if (_pool.Count < _maxSize)
        {
            _pool.Push(person);
        }
    }
}

internal class ResourceHolder : IDisposable
{
    private bool _disposed;

    public static bool FinalizerCalled { get; private set; }

    public static void ResetFinalizerFlag()
    {
        FinalizerCalled = false;
    }

    ~ResourceHolder()
    {
        Console.WriteLine("Финализация объекта ResourceHolder");
        
        FinalizerCalled = true;
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            Console.WriteLine("Освобождение ресурсов ResourceHolder с помощью Dispose");
        }

        _disposed = true;
    }
}
