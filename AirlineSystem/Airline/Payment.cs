using System;

namespace AirlineTicketSystem
{
    public abstract class Payment
    {
        public string PaymentId { get; set; }
        public double Amount { get; protected set; }
        public string Status { get; protected set; }

        protected Payment(double amount, string paymentId)
        {
            Amount = amount;
            PaymentId = "PM" + new Random().Next(100000, 999999);
            Status = "Pending";
        }

        // trả về true nếu thanh toán thành công
        public abstract bool Process();
        public abstract void Print();
    }

    public class CashPayment : Payment
    {
        public CashPayment(double amount, string paymentId = null) : base(amount, paymentId) { }

        public override bool Process()
        {
            Status = Amount > 0 ? "Success" : "Failed";
            return Status == "Success";
        }

        public override void Print()
        {
            Console.WriteLine("=== Cash Payment ===");
            Console.WriteLine($"ID: {PaymentId}");
            Console.WriteLine($"Amount: {Amount}");
            Console.WriteLine($"Status: {Status}");
        }
    }

    public class CreditCardPayment : Payment
    {
        public string CardNumber { get; set; }

        public CreditCardPayment(double amount, string cardNumber, string paymentId = null) : base(amount, paymentId)
        {
            CardNumber = cardNumber ?? "";
        }

        public override bool Process()
        {
            // giả lập: thẻ valid nếu amount>0 và không kết thúc bằng "0000"
            if (Amount > 0 && !CardNumber.EndsWith("0000"))
            {
                Status = "Success";
                return true;
            }
            Status = "Failed";
            return false;
        }

        public override void Print()
        {
            Console.WriteLine("=== Credit Card Payment ===");
            Console.WriteLine($"ID: {PaymentId}");
            Console.WriteLine($"Amount: {Amount}");
            string last4 = CardNumber.Length >= 4 ? CardNumber.Substring(CardNumber.Length - 4) : CardNumber;
            Console.WriteLine($"Card: **** **** **** {last4}");
            Console.WriteLine($"Status: {Status}");
        }
    }

    public class EwalletPayment : Payment
    {
        public string WalletId { get; set; }

        public EwalletPayment(double amount, string walletId, string paymentId = null) : base(amount, paymentId)
        {
            WalletId = walletId ?? "";
        }

        public override bool Process()
        {
            // giả lập: ewallet chỉ thành công nếu amount >= 10
            if (Amount >= 10)
            {
                Status = "Success";
                return true;
            }
            Status = "Failed";
            return false;
        }

        public override void Print()
        {
            Console.WriteLine("=== E-Wallet Payment ===");
            Console.WriteLine($"ID: {PaymentId}");
            Console.WriteLine($"Amount: {Amount}");
            Console.WriteLine($"Wallet: {WalletId}");
            Console.WriteLine($"Status: {Status}");
        }
    }
}
