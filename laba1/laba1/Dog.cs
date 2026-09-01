namespace laba1;

internal class Dog
{
    public readonly string OwnerName;
    public readonly string Nickname;
    public readonly string Breed;

    public Dog(string ownerName, string nickname, string breed)
    {
        OwnerName = ownerName;
        Nickname = nickname;
        Breed = breed;
    }

    private static bool IsValidText(string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    public static bool TryCreate(string? ownerName, string? nickname, string? breed, out Dog? dog, out string error)
    {
        if (!IsValidText(ownerName))
        {
            dog = null;
            error = "Имя хозяина не может быть пустым.";
            return false;
        }

        if (!IsValidText(nickname))
        {
            dog = null;
            error = "Кличка не может быть пустой.";
            return false;
        }

        if (!IsValidText(breed))
        {
            dog = null;
            error = "Порода не может быть пустой.";
            return false;
        }

        dog = new Dog(ownerName!.Trim(), nickname!.Trim(), breed!.Trim());
        error = "";
        return true;
    }

    public static void Bark(int count)
    {
        if (count <= 0)
        {
            Console.WriteLine("Количество лаев должно быть больше 0.");
            return;
        }

        for (var i = 0; i < count; i++)
        {
            Console.WriteLine("Гав!");
        }
    }
}
