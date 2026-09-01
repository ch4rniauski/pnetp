namespace laba1;

internal abstract class PaymentMethod
{
    public readonly double Amount;

    protected PaymentMethod(double amount)
    {
        Amount = amount;
    }

    public abstract void ProcessPmnt();
}

internal class CreditCard : PaymentMethod
{
    private readonly string _cardNumber;

    public CreditCard(double amount, string cardNumber) : base(amount)
    {
        _cardNumber = cardNumber;
    }

    public override void ProcessPmnt()
    {
        Console.WriteLine($"Оплата {Amount} руб. банковской картой {_cardNumber}");
    }
}

internal class PayPal : PaymentMethod
{
    private readonly string _email;

    public PayPal(double amount, string email) : base(amount)
    {
        _email = email;
    }

    public override void ProcessPmnt()
    {
        Console.WriteLine($"Оплата {Amount} руб. через PayPal ({_email})");
    }
}
