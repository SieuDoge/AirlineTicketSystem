namespace AirlineTicketSystem
{
    public abstract class Person
    {
        // Fields
        private string name;
        private string email;
        private string phoneNumber;
        private char gender;
        private int age;
        protected Person(string name, string email, string phoneNumber, char gender, int age)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Gender = gender;
            Age = age;
        }

        // Properties
        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Email
        {
            get => email;
            set
            {
                if (string.IsNullOrEmpty(value) || !value.Contains("@"))
                {
                    throw new ArgumentException("Invalid email address.");
                }
                email = value;
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length != 10 || !value.All(char.IsDigit))
                {
                    throw new ArgumentException("Invalid phone number.");
                }
                phoneNumber = value;
            }
        }

        public char Gender
        {
            get => gender;
            set
            {
                if (value != 'm' && value != 'f' && value != 'u')
                {
                    throw new ArgumentException("Invalid gender, use 'm', 'f' or 'u'.");
                }
                gender = value;
            }
        }

        public int Age
        {
            get => age;
            set
            {
                if (value < 0) throw new ArgumentException("Age cannot be negative.");
                age = value;
            }
        }

    

        public abstract void Print();
    }
}
