namespace AirlineTicketSystem;
//thấy chưa
public class Passenger
{
    private string name ="";
    private string email="";
    private string phoneNumber="";
    private char gender;
    private int age;

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public string Email
    {
        get
        {
            return email;
        }

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
        get
        {
            return phoneNumber;
        }

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
        get
        {
            return gender;
        }

        set
        {
            if (value != 'm' && value != 'f' && value != 'u')
            {
                throw new ArgumentException("Invalid gender, type 'f', 'm' or 'u'.");
            }
            gender = value;
        }
    }

    public int Age
    {
        get
        {
            return age;
        }

        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Age cannot be negative.");
            }
            age = value;
        }
    }
    public Passenger(string name, string email, char gender, int age)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Gender = gender;
        Age = age;
    }
    public string inforstudent()
    {
        return $"Name: {Name}, Email: {Email}, Phone Number: {PhoneNumber}";
    }
    // Other methods and properties can be added as needed
}
