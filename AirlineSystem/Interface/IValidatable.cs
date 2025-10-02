namespace AirlineTicketSystem
{
    public interface IValidatable
    {
        bool IsValid(out string errorMessage);
    }
}


