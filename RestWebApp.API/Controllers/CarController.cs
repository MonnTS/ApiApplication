using Microsoft.AspNetCore.Mvc;
using RestWebApp.Contracts;
using RestWebApp.Entities;
using RestWebApp.Entities.Models;

namespace RestWebApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private readonly IRepositoryWrapper _repoWrapper;
    private readonly ILoggerManager _logger;

    public CarController(IRepositoryWrapper repoWrapper, RepositoryContext context,
        ILoggerManager logger)
    {
        _repoWrapper = repoWrapper;
        _logger = logger;
        context.Database.EnsureCreated();
    }
    
    [HttpGet]
    public IActionResult GetAllCars()
    {
        var cars = _repoWrapper.Cars.GetAll();
        _logger.LogInfo("Returned all cars from database.");
        return Ok(cars);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetCarById(int id)
    {
        var car = _repoWrapper.Cars.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if(car == null)
        {
            _logger.LogWarn("Car with id: " + id + " was not found in the database.");
            return NotFound($"Car with this id {car.Id} not found");
        }
        
        _logger.LogInfo("Returned car with id: " + id + " from database.");
        return Ok(car);
    }
    
    [HttpPost]
    public IActionResult CreateCar([FromBody] Car car)
    {
        if(car == null)
        {
            _logger.LogError("Car object sent from client is null.");
            return BadRequest($"Cannot create an empty object of type {typeof(Car)}");
        }
        
        _repoWrapper.Cars.Create(car);
        _repoWrapper.Save();
        _logger.LogInfo("User with id: " + car.Id + " was created in the database.");
        return Ok(car);
    }
    
    [HttpPut("{id:int}")]
    public IActionResult UpdateCar(int id, [FromBody] Car car)
    {
        if(car == null || car.Id != id)
        {
            _logger.LogError("Car object sent from client is null.");
            return BadRequest("Object car is null or id is not equal to id in url");
        }
        
        var carToUpdate = _repoWrapper.Cars.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if(carToUpdate == null)
        {
            _logger.LogWarn("Car with id: " + id + " was not found in the database.");
            return NotFound($"Car with id {id} not found");
        }
            
        carToUpdate.Description = car.Description;
        carToUpdate.Brand = car.Brand;
        carToUpdate.Date = car.Date;
        
        _repoWrapper.Cars.Update(carToUpdate);
        _repoWrapper.Save();
        return Ok(carToUpdate);
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult DeleteCar(int id)
    {
        var car = _repoWrapper.Cars.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if (car == null)
        {
            _logger.LogWarn("Car with id: " + id + " was not found in the database.");
            return NotFound($"Car with id {id} not found");
        }
        
        _repoWrapper.Cars.Delete(car);
        _repoWrapper.Save();
        _logger.LogInfo("Car with id: " + id + " was deleted from the database.");
        return NoContent();
    }
}