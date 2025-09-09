using System;


namespace AirlineTicketSystem
{
    
    
    
    abstract class Payment
    {
        public string paymentid { get; set; }
        public double amount { get; set; }
        public string status { get; set; }

        public Payment(string paymentid, double amount)
        {
            this.paymentid = paymentid;
            this.amount = amount;
            this.status = "Pending";
        }

        public abstract bool Process();  
        public abstract void Print();    
    }

   
    class CashPayment : Payment
    {
        public CashPayment(string paymentid, double amount) : base(paymentid, amount) { }

        public override bool Process()
        {
            this.  status = amount > 0 ? "Success" : "Failed";
            return status == "Success";
        }

        public override void Print()
        {
            Console.WriteLine("Cash Payment");
            Console.WriteLine($"ID: {paymentid}");
            Console.WriteLine($"amount: {amount}");
            Console.WriteLine($"status: {status}");
           
        }
    }

    class CreditCardPayment : Payment
    {
        public string CardNumber { get; set; }

        public CreditCardPayment(string paymentid, double amount, string cardNumber)
            : base(paymentid, amount)
        {
            this.CardNumber = cardNumber;
        }

        public override bool Process()
        {
            
            if (amount > 0 && !CardNumber.EndsWith("0000"))
            {
                this.status = "Success";
                return true;
            }
                    this.   status = "Failed";
            return false;
        }

        public override void Print()
        {
            Console.WriteLine("Credit Card Payment ");
            Console.WriteLine($"ID: {paymentid}");
            Console.WriteLine($"amount: {amount}");
            Console.WriteLine($"Card: **** **** **** {CardNumber.Substring(CardNumber.Length - 4)}");
            Console.WriteLine($"status: {status}");
            
        }
    }

   
    class EwalletPayment : Payment
    {
        public string WalletId { get; set; }

        public EwalletPayment(string paymentid, double amount, string walletId)
            : base(paymentid, amount)
        {
            this.   WalletId = walletId;
        }

        public override bool Process()
        {
            
            if (amount >= 10)
            {
                this.status = "Success";
                return true;
            }
            this.status = "Failed";
            return false;
        }

        public override void Print()
        {
            Console.WriteLine("EWallet Payment ");
            Console.WriteLine($"ID: {paymentid}");
            Console.WriteLine($"amount: {amount}");
            Console.WriteLine($"Wallet: {WalletId}");
            Console.WriteLine($"status: {status}");
            
        }
    }

   
}
