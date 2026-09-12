namespace laba1;

internal abstract class Figure
{
    protected readonly string Name;

    protected Figure(string name)
    {
        Name = name;
    }

    public abstract void Draw();
}

internal class Triangle : Figure
{
    public Triangle() : base("Треугольник")
    {
    }

    public override void Draw()
    {
        Console.WriteLine($"Рисую {Name}: соединяю три вершины линиями");
    }
}

internal class Rectangle : Figure
{
    public Rectangle() : base("Прямоугольник")
    {
    }

    public override void Draw()
    {
        Console.WriteLine($"Рисую {Name}: соединяю четыре вершины под прямыми углами");
    }
}
