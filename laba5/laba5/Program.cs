using System.Reflection;
using System.Reflection.Emit;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

Task1();
Task2();
Task3();
await Task4();

void Task1()
{
    var employees = new List<Employee>
    {
        new("IT", 3500),
        new("HR", 2800),
        new("IT", 4200),
        new("Финансы", 3100),
        new("HR", 2600),
        new("Финансы", 3300)
    };

    var averageByDepartment = employees
        .GroupBy(e => e.Department)
        .Select(g => new { Department = g.Key, AverageSalary = g.Average(e => e.Salary) })
        .OrderBy(x => x.Department);

    foreach (var item in averageByDepartment)
    {
        Console.WriteLine($"{item.Department}: средняя зарплата = {item.AverageSalary:F2}");
    }
}

void Task2()
{
    var sales = Enumerable.Range(1, 500_000)
        .Select(i => new Sale(i % 5, i * 1.5m))
        .ToList();

    var grouped = sales
        .AsParallel()
        .GroupBy(s => s.RegionId)
        .Select(g => new
        {
            RegionId = g.Key,
            Count = g.Count(),
            TotalAmount = g.Sum(s => s.Amount)
        })
        .OrderBy(g => g.RegionId);

    foreach (var group in grouped)
    {
        Console.WriteLine($"Регион {group.RegionId}: продаж = {group.Count}, сумма = {group.TotalAmount:F2}");
    }
}

void Task3()
{
    var assemblyName = new AssemblyName("DynamicAssembly");
    var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
    var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
    var typeBuilder = moduleBuilder.DefineType("Greeter", TypeAttributes.Public | TypeAttributes.Class);

    var nameField = typeBuilder.DefineField("_name", typeof(string), FieldAttributes.Private);

    var ctorBuilder = typeBuilder.DefineConstructor(
        MethodAttributes.Public,
        CallingConventions.Standard,
        [typeof(string)]);

    var ctorIl = ctorBuilder.GetILGenerator(); // получить IL-генератор для конструктора
    ctorIl.Emit(OpCodes.Ldarg_0); // загрузить аргумент 0 (this) на стек
    ctorIl.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes)!); // вызвать конструктор object (base) (this передается как аргумент)
    ctorIl.Emit(OpCodes.Ldarg_0); // загрузить аргумент 0 (this) на стек
    ctorIl.Emit(OpCodes.Ldarg_1); // положить аргумент 1 (name) на стек
    ctorIl.Emit(OpCodes.Stfld, nameField); // присвоить верхнее значение из стека (name) в nameField предпоследнего объекта (this)
    ctorIl.Emit(OpCodes.Ret); // завершить выполнение конструктора

    var greetMethod = typeBuilder.DefineMethod(
        "Greet",
        MethodAttributes.Public,
        typeof(string),
        Type.EmptyTypes);

    var greetIl = greetMethod.GetILGenerator(); // получить IL-генератор для метода
    greetIl.Emit(OpCodes.Ldstr, "Привет, "); // загрузить строку "Привет, " на стек
    greetIl.Emit(OpCodes.Ldarg_0); // загрузить аргумент 0 (this) на стек
    greetIl.Emit(OpCodes.Ldfld, nameField); // взять верхнее значение из стека (this),
                                            // попытаться прочитать у него nameField,
                                            // загрузить значение nameField на стек
    greetIl.Emit(
        OpCodes.Call,
        typeof(string).GetMethod("Concat", [typeof(string), typeof(string)])!); // вызвать метод Concat, взяв со стека (nameField + "Привет, ")
                                                                                // (берется со стека сверху вниз, но в параметры передаются справа налево).
                                                                                // Результат вызова метода загрузить на стек
    greetIl.Emit(OpCodes.Ret); // завершить выполнение метода и вернуть результат

    try
    {
        var type = typeBuilder.CreateType();
        
        var instance = Activator.CreateInstance(type, "Алексей");
        
        var method = type.GetMethod("Greet") 
            ?? throw new InvalidOperationException("Метод Greet не найден");
            
        var greeting = (string?)method.Invoke(instance, []);
        
        if (string.IsNullOrEmpty(greeting))
        {
            throw new InvalidOperationException("Метод Greet вернул пустой результат");
        }
        
        Console.WriteLine(greeting);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
}

async Task Task4()
{
    const string script = "int x = 5;";
    const string continuation = "x + 1";

    try
    {
        var state = await CSharpScript.RunAsync(script);
        var resultState = await state.ContinueWithAsync<int>(continuation);

        Console.WriteLine($"Результат выполнения скрипта: {resultState.ReturnValue}");
    }
    catch (CompilationErrorException ex)
    {
        Console.WriteLine($"Ошибка компиляции скрипта: {string.Join(", ", ex.Diagnostics)}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка выполнения скрипта: {ex.Message}");
    }

    var scriptPath = Path.Combine(AppContext.BaseDirectory, "script.csx");

    if (!File.Exists(scriptPath))
    {
        Console.WriteLine("Ошибка: файл скрипта не найден");
        return;
    }

    var fileScript = await File.ReadAllTextAsync(scriptPath);

    if (string.IsNullOrWhiteSpace(fileScript))
    {
        Console.WriteLine("Ошибка: скрипт из файла пуст");
        return;
    }

    try
    {
        var options = ScriptOptions.Default
            .WithImports("System.Linq")
            .WithReferences(typeof(Enumerable).Assembly);

        dynamic result = await CSharpScript.EvaluateAsync(fileScript, options);

        Console.WriteLine($"Результат выполнения скрипта из файла: {result}");
    }
    catch (CompilationErrorException ex)
    {
        Console.WriteLine($"Ошибка компиляции скрипта из файла: {string.Join(", ", ex.Diagnostics)}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка выполнения скрипта из файла: {ex.Message}");
    }
}

internal record Employee(string Department, decimal Salary);

internal record Sale(int RegionId, decimal Amount);
