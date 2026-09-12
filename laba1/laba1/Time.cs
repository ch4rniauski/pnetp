namespace laba1;

internal class Time
{
    private readonly int _hours;
    private readonly int _minutes;
    private readonly string _timeOfDay;

    public Time(int hours, int minutes)
    {
        _hours = hours;
        _minutes = minutes;
        _timeOfDay = GetTimeOfDayByHours(hours);
    }

    private static string GetTimeOfDayByHours(int hours)
    {
        return hours switch
        {
            >= 0 and <= 5 => "ночь",
            >= 6 and <= 11 => "утро",
            >= 12 and <= 17 => "день",
            _ => "вечер"
        };
    }

    public static bool TryCreate(string? hoursText, string? minutesText, out Time? time, out string error)
    {
        if (!int.TryParse(hoursText, out var hours) || hours < 0 || hours > 23)
        {
            time = null;
            error = "Часы должны быть целым числом от 0 до 23";
            return false;
        }

        if (!int.TryParse(minutesText, out var minutes) || minutes < 0 || minutes > 59)
        {
            time = null;
            error = "Минуты должны быть целым числом от 0 до 59";
            return false;
        }

        time = new Time(hours, minutes);
        error = "";
        
        return true;
    }

    public Time AddMinutes(int minutes)
    {
        var totalMinutes = _hours * 60 + _minutes + minutes;

        while (totalMinutes < 0)
        {
            totalMinutes += 24 * 60;
        }

        totalMinutes %= 24 * 60;

        var newHours = totalMinutes / 60;
        var newMinutes = totalMinutes % 60;

        return new Time(newHours, newMinutes);
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Время: {_hours:D2}:{_minutes:D2}, время суток: {_timeOfDay}");
    }
}
