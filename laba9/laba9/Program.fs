// ========== Задание 1. Вариант 11: удвоенное значение числа ==========
let doubleValue x = x * 2

// ========== Задание 2. Вариант 11: все ли элементы больше 0 ==========
let allPositive list =
    List.forall (fun x -> x > 0) list

// ========== Задание 3. Вариант 11: удалить все вхождения элемента ==========
let rec removeAll elem lst =
    match lst with
    | [] -> []
    | head :: tail when head = elem -> removeAll elem tail
    | head :: tail -> head :: removeAll elem tail

// ========== Задание 4. Вариант 11: проверить, пуст ли список ==========
let isEmptyList lst =
    match lst with
    | [] -> true
    | _ -> false

// ========== Задание 5. Вариант 11: тип Account и баланс ==========
type Account =
    | Savings of float
    | Checking of float

let getBalance account =
    match account with
    | Savings balance -> balance
    | Checking balance -> balance

// ========== Задание 6. Вариант 11: парсинг → квадрат → деление ==========
let parseInt (str: string) =
    match System.Int32.TryParse(str) with
    | true, n -> Some n
    | _ -> None

let square x = Some (x * x)

let divideBy divisor x =
    if divisor = 0.0 then None
    else Some (float x / divisor)

let parseSquareDivide str divisor =
    parseInt str
    |> Option.bind square
    |> Option.bind (divideBy divisor)

// ========== Демонстрация работы программы ==========
[<EntryPoint>]
let main _ =
    printfn "=== Задание 1: удвоенное значение ==="
    printfn "doubleValue 7 = %d" (doubleValue 7)

    printfn "\n=== Задание 2: все элементы > 0 ==="
    printfn "allPositive [1; 2; 3] = %b" (allPositive [1; 2; 3])
    printfn "allPositive [1; -2; 3] = %b" (allPositive [1; -2; 3])

    printfn "\n=== Задание 3: удалить все вхождения ==="
    printfn "removeAll 2 [1; 2; 3; 2; 4; 2] = %A" (removeAll 2 [1; 2; 3; 2; 4; 2])

    printfn "\n=== Задание 4: пустой список ==="
    printfn "isEmptyList [] = %b" (isEmptyList [])
    printfn "isEmptyList [1] = %b" (isEmptyList [1])

    printfn "\n=== Задание 5: баланс счёта ==="
    let savings = Savings 1500.0
    let checking = Checking 320.5
    printfn "Savings 1500.0 -> баланс = %.1f" (getBalance savings)
    printfn "Checking 320.5 -> баланс = %.1f" (getBalance checking)

    printfn "\n=== Задание 6: парсинг → квадрат → деление ==="
    printfn "parseSquareDivide \"4\" 2.0 = %A" (parseSquareDivide "4" 2.0)   // 4*4/2 = 8.0
    printfn "parseSquareDivide \"abc\" 2.0 = %A" (parseSquareDivide "abc" 2.0)
    printfn "parseSquareDivide \"5\" 0.0 = %A" (parseSquareDivide "5" 0.0)

    0
