using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWebApp.Contracts;
using RestWebApp.Entities;
using RestWebApp.Entities.Models;

namespace RestWebApp.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private readonly IRepositoryWrapper _repoWrapper;
    private readonly ILogger<CarController> _logger;

    public CarController(IRepositoryWrapper repoWrapper, RepositoryContext context,
        ILogger<CarController> logger)
    {
        _repoWrapper = repoWrapper;
        _logger = logger;
        context.Database.EnsureCreated();
    }
    
    [HttpGet]
    [Authorize(Roles = "User, Admin")]
    public IActionResult GetAllCars()
    {
        var cars = _repoWrapper.Cars.GetAll();
        _logger.LogInformation("Returned all cars from database");
        return Ok(cars);
    }
    
    [HttpGet("{id:int}")]
    [Authorize(Roles = "User, Admin")]
    public IActionResult GetCarById(int id)
    {
        var car = _repoWrapper.Cars.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if(car == null)
        {
            _logger.LogWarning("Car with id: " + id + " was not found in the database.");
            return NotFound($"Car with this id {car.Id} not found");
        }
        
        _logger.LogInformation("Returned car with id: " + id + " from database.");
        return Ok(car);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateCar([FromBody] Car car)
    {
        _repoWrapper.Cars.Create(car);
        _repoWrapper.Save();
        _logger.LogInformation("User with id: " + car.Id + " was created in the database.");
        return Ok(car);
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateCar(int id, [FromBody] Car car)
    {
        if(car.Id != id)
        {
            _logger.LogError($"Wrong Id provided in the request. Expected Id: {id} and provided Id: {car.Id}");
            return BadRequest($"Id of object {typeof(Car)} is not equal to id in url.");
        }
        
        var carToUpdate = _repoWrapper.Cars.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if(carToUpdate == null)
        {
            _logger.LogWarning("Car with id: " + id + " was not found in the database.");
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
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteCar(int id)
    {
        var car = _repoWrapper.Cars.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if (car == null)
        {
            _logger.LogWarning("Car with id: " + id + " was not found in the database.");
            return NotFound($"Car with id {id} not found");
        }
        
        _repoWrapper.Cars.Delete(car);
        _repoWrapper.Save();
        _logger.LogInformation("Car with id: " + id + " was deleted from the database.");
        return NoContent();
    }
}