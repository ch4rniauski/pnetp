Task1();
Task2();

void Task1()
{
    Console.WriteLine($"ОС: {Environment.OSVersion}");
    Console.WriteLine($"Компьютер: {Environment.MachineName}");
    Console.WriteLine($"Пользователь: {Environment.UserName}");
    Console.WriteLine($"Процессоров: {Environment.ProcessorCount}");
    Console.WriteLine($"Текущая директория: {Environment.CurrentDirectory}");
    Console.WriteLine($"Версия .NET: {Environment.Version}");
    Console.WriteLine($"64-разрядная ОС: {Environment.Is64BitOperatingSystem}");
    Console.WriteLine($"64-разрядный процесс: {Environment.Is64BitProcess}");
}

void Task2()
{
    var morning = DateTime.Today.AddHours(6);
    var now = DateTime.Now;

    if (now < morning)
    {
        Console.WriteLine("С 6:00 утра ещё не прошло ни одной минуты");
        return;
    }

    var elapsed = now - morning;
    var minutes = (int)elapsed.TotalMinutes;
    Console.WriteLine($"С 6:00 утра прошло минут: {minutes}");
}
