namespace AirlineTicketSystem
{
    public interface IExportable
    {
        string ToCsvHeader();
        string ToCsvRow();
    }
}


