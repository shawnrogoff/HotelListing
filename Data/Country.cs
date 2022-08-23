namespace HotelListing.Data;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; } // Abbreviation

    // When getting information from db about Country, if requested
    // You can also grab information on that country's hotels
    // Using the .Include option
    public virtual IList<Hotel> Hotels { get; set; }
}
