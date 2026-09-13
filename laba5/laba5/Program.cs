using System.Diagnostics;
using System.Reflection;
using System.Text;

while (true)
{
    Console.WriteLine("1 — LINQ: проверка All");
    Console.WriteLine("2 — PLINQ: сравнение с LINQ");
    Console.WriteLine("3 — Рефлексия: сериализация в JSON");
    Console.WriteLine("4 — DLR: паттерн Посетитель");
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
        case "4":
            Task4();
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Неверный выбор. Введите 0, 1, 2, 3 или 4");
            break;
    }

    Console.WriteLine();
}

void Task1()
{
    Console.WriteLine();
    
    var numbers = new List<int> { 2, 4, 6, 8, 10 };

    Console.WriteLine($"Исходный список: {string.Join(", ", numbers)}");

    var allEven = numbers.All(n => n % 2 == 0);
    var allPositive = numbers.All(n => n > 0);
    var allGreaterThanFive = numbers.All(n => n > 5);

    Console.WriteLine($"Все числа чётные: {allEven}");
    Console.WriteLine($"Все числа положительные: {allPositive}");
    Console.WriteLine($"Все числа больше 5: {allGreaterThanFive}");
}

void Task2()
{
    Console.WriteLine();
    
    var numbers = Enumerable.Range(1, 50_000_000).ToArray();

    var stopwatch = Stopwatch.StartNew();
    
    var linqResult = numbers
        .Where(HeavyIsEven)
        .Count();
    
    stopwatch.Stop();
    var linqMs = stopwatch.ElapsedMilliseconds;

    stopwatch.Restart();
    
    var plinqResult = numbers
        .AsParallel()
        .Where(HeavyIsEven)
        .Count();
    
    stopwatch.Stop();
    var plinqMs = stopwatch.ElapsedMilliseconds;

    Console.WriteLine($"LINQ: результат = {linqResult}, время = {linqMs} мс");
    Console.WriteLine($"PLINQ: результат = {plinqResult}, время = {plinqMs} мс");

    if (linqMs < plinqMs)
    {
        Console.WriteLine("LINQ выполнен быстрее");
    }
    else if (plinqMs < linqMs)
    {
        Console.WriteLine("PLINQ выполнен быстрее");
    }
    else
    {
        Console.WriteLine("Время выполнения одинаковое");
    }
}

void Task3()
{
    Console.WriteLine();
    
    var person = new Person("Евгений", 45, "Варшава");

    try
    {
        var json = SerializeToJson(person);

        if (string.IsNullOrEmpty(json))
        {
            Console.WriteLine("Ошибка: JSON пуст");
            return;
        }

        Console.WriteLine(json);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
}

void Task4()
{
    Console.WriteLine();

    var shapes = new List<IShape>
    {
        new Circle { Radius = 5 },
        new Rectangle { Width = 4, Height = 6 },
        new Triangle { A = 3, B = 4, C = 5 }
    };

    var visitor = new ShapeVisitor();

    foreach (var shape in shapes)
    {
        try
        {
            shape.Accept(visitor);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}

bool HeavyIsEven(int number)
{
    var value = Math.Sqrt(number) * Math.Log(number + 1);
    return number % 2 == 0 && value >= 0;
}

string SerializeToJson(object? obj)
{
    if (obj is null)
    {
        return "null";
    }

    var type = obj.GetType();
    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    if (properties.Length == 0)
    {
        return "{}";
    }

    var builder = new StringBuilder();
    builder.Append('{');

    for (var i = 0; i < properties.Length; i++)
    {
        var property = properties[i];
        var value = property.GetValue(obj);
        var formattedValue = FormatJsonValue(value);

        builder.Append('"');
        builder.Append(property.Name);
        builder.Append("\": ");
        builder.Append(formattedValue);

        if (i < properties.Length - 1)
        {
            builder.Append(", ");
        }
    }

    builder.Append('}');
    return builder.ToString();
}

string FormatJsonValue(object? value)
{
    return value switch
    {
        null => "null",
        string text => $"\"{text}\"",
        bool flag => flag ? "true" : "false",
        _ => value.ToString() ?? "null"
    };
}

internal interface IShapeVisitor
{
    void Visit(Circle circle);
    void Visit(Rectangle rectangle);
    void Visit(Triangle triangle);
}

internal interface IShape
{
    public void Accept(dynamic visitor);
}

internal class Circle : IShape
{
    public double Radius { get; init; }

    public void Accept(dynamic visitor)
    {
        visitor.Visit(this);
    }
}

internal class Rectangle : IShape
{
    public double Width { get; init; }
    public double Height { get; init; }

    public void Accept(dynamic visitor)
    {
        visitor.Visit(this);
    }
}

internal class Triangle : IShape
{
    public double A { get; init; }
    public double B { get; init; }
    public double C { get; init; }

    public void Accept(dynamic visitor)
    {
        visitor.Visit(this);
    }
}

internal class ShapeVisitor : IShapeVisitor
{
    public void Visit(Circle circle)
    {
        var area = Math.PI * circle.Radius * circle.Radius;
        Console.WriteLine($"Круг: радиус = {circle.Radius}, площадь = {area:F2}");
    }

    public void Visit(Rectangle rectangle)
    {
        var area = rectangle.Width * rectangle.Height;
        Console.WriteLine($"Прямоугольник: {rectangle.Width}x{rectangle.Height}, площадь = {area:F2}");
    }

    public void Visit(Triangle triangle)
    {
        var perimeter = triangle.A + triangle.B + triangle.C;
        Console.WriteLine($"Треугольник: стороны = {triangle.A}, {triangle.B}, {triangle.C}, периметр = {perimeter:F2}");
    }
}

internal record Person(string Name, int Age, string City);
