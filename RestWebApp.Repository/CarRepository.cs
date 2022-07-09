using RestWebApp.Contracts;
using RestWebApp.Entities.Models;
using RestWebApp.Entities;

namespace RestWebApp.Repository;

public class CarRepository : RepositoryBase<Car>, ICarRepository
{
    public CarRepository(RepositoryContext context) : base(context)
    { }
}