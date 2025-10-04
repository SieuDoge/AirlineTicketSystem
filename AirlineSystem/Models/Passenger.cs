namespace AirlineTicketSystem;
//thấy chưa
public class Passenger : IPrintable, ISearchable, IValidatable, IExportable
{
    private string name;
    private string email;
    private string phoneNumber;
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
    public Passenger(string name, string email, char gender, int age, string phoneNumber)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Gender = gender;
        Age = age;
    }
    
    public void Print() {
        Console.WriteLine($"Name: {Name} - " +
                          $"Email: {Email} - " +
                          $"Phone: {PhoneNumber} - " +
                          $"Gender: {Gender} - " +
                          $"Age: {Age} - ");
    }

    public string ToCsvHeader()
    {
        return "Name,PhoneNumber,Email,Age,Gender";
    }

    public string ToCsvRow()
    {
        return $"{Name},{PhoneNumber},{Email},{Age},{Gender}";
    }

    /// <summary>
    /// Check xem có match ko
    /// || (OR) 
    /// </summary>
    /// <param name="term"></param>
    /// <returns></returns>
    public bool Matches(string term)
    {
        if (string.IsNullOrWhiteSpace(term)) return true;
        term = term.Trim().ToLower();
        return (Name?.ToLower().Contains(term) == true)
               || (Email?.ToLower().Contains(term) == true)
               || (PhoneNumber?.Contains(term) == true)
               || Age.ToString().Contains(term)
               || Gender.ToString().ToLower().Contains(term);
    }

    public bool IsValid(out string errorMessage)
    {
        try
        {
            // trigger property validations
            Name = Name;
            Email = Email;
            PhoneNumber = PhoneNumber;
            Gender = Gender;
            Age = Age;
            errorMessage = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            return false;
        }
    }

    // Other methods and properties can be added as needed
}
