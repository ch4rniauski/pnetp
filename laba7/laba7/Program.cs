using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml.Serialization;

var dataDir = Path.Combine(AppContext.BaseDirectory, "data");

EncryptFileWithAes();
AddElementToXml();

void EncryptFileWithAes()
{
    var inputPath = Path.Combine(dataDir, "input.txt");
    var outputPath = Path.Combine(dataDir, "input_encrypted.txt");

    if (!File.Exists(inputPath))
    {
        Console.WriteLine("Исходный файл не найден: " + inputPath);
        return;
    }

    using var aes = Aes.Create();
    aes.GenerateKey();
    aes.GenerateIV();

    using var inputStream = File.OpenRead(inputPath);
    using var outputStream = File.Create(outputPath);

    outputStream.Write(aes.Key);
    outputStream.Write(aes.IV);

    using (var cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
    {
        inputStream.CopyTo(cryptoStream);
    }

    Console.WriteLine("Файл зашифрован: " + outputPath);
}

void AddElementToXml()
{
    var xmlPath = Path.Combine(dataDir, "books.xml");

    if (!File.Exists(xmlPath))
    {
        Console.WriteLine("XML-файл не найден: " + xmlPath);
        return;
    }

    AddElementToXmlWithLinq(xmlPath);
    AddElementToXmlWithClass(xmlPath);
}

void AddElementToXmlWithLinq(string xmlPath)
{
    var doc = XDocument.Load(xmlPath);
    var root = doc.Root;

    if (root is null)
    {
        Console.WriteLine("Корневой элемент XML не найден");
        return;
    }

    var existingIds = root.Elements("Book")
        .Select(b => (int?)b.Attribute("id"))
        .Where(id => id.HasValue)
        .Select(id => id!.Value)
        .ToList();

    var newId = existingIds.Count != 0
        ? existingIds.Max() + 1
        : 1;

    var newBook = new XElement("Book",
        new XAttribute("id", newId),
        new XElement("Title", "Мастер и Маргарита"),
        new XElement("Author", "Михаил Булгаков"),
        new XElement("Year", 1967),
        new XElement("Price",
            new XAttribute("currency", "RUB"),
            650));

    root.Add(newBook);
    doc.Save(xmlPath);

    Console.WriteLine("Новый элемент добавлен через LINQ to XML: " + xmlPath);
}

void AddElementToXmlWithClass(string xmlPath)
{
    BooksCatalog catalog;

    using (var stream = File.OpenRead(xmlPath))
    {
        var serializer = new XmlSerializer(typeof(BooksCatalog));
        catalog = (BooksCatalog?)serializer.Deserialize(stream)
                  ?? new BooksCatalog();
    }

    var newId = catalog.Books.Count != 0
        ? catalog.Books.Max(b => b.Id) + 1
        : 1;

    catalog.Books.Add(new Book
    {
        Id = newId,
        Title = "Преступление и наказание",
        Author = "Фёдор Достоевский",
        Year = 1866,
        Price = new PriceInfo
        {
            Currency = "RUB",
            Value = 720
        }
    });

    using (var stream = File.Create(xmlPath))
    {
        var serializer = new XmlSerializer(typeof(BooksCatalog));
        serializer.Serialize(stream, catalog);
    }

    Console.WriteLine("Новый элемент добавлен через класс: " + xmlPath);
}

[XmlRoot("Books")]
public class BooksCatalog
{
    [XmlElement("Book")]
    public List<Book> Books { get; set; } = [];
}

public class Book
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public PriceInfo Price { get; set; } = new();
}

public class PriceInfo
{
    [XmlAttribute("currency")]
    public string Currency { get; set; } = string.Empty;
    
    [XmlText]
    public int Value { get; set; }
}
