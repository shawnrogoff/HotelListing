using HotelListing.Data;

namespace HotelListing.IRepository;

public interface IUnitOfWork : IDisposable
{
    // The idea here is to have this Save outside of the repository so all tasks can be
    // saved with one database call rather than after each individual task.

    IGenericRepository<Country> Countries { get; }
    IGenericRepository<Hotel> Hotels { get; }
    Task Save();
}
