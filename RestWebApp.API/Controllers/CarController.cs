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

    public CarController(IRepositoryWrapper repoWrapper, RepositoryContext context)
    {
        _repoWrapper = repoWrapper;
        context.Database.EnsureCreated();
    }
    
    [HttpGet]
    public IActionResult GetAllCars()
    {
        var cars = _repoWrapper.Cars.GetAll();
        return Ok(cars);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetCarById(int id)
    {
        var car = _repoWrapper.Cars.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if(car == null)
        {
            return NotFound($"Car with this id {car.Id} not found");
        }
        
        return Ok(car);
    }
    
    [HttpPost]
    public IActionResult CreateCar([FromBody] Car car)
    {
        if(car == null)
        {
            return BadRequest($"Car with id {car.Id} was not found");
        }
        
        _repoWrapper.Cars.Create(car);
        _repoWrapper.Save();
        return Ok(car);
    }
    
    [HttpPut("{id:int}")]
    public IActionResult UpdateCar(int id, [FromBody] Car car)
    {
        if(car == null || car.Id != id)
        {
            return BadRequest("Object car is null or id is not equal to id in url");
        }
        
        var carToUpdate = _repoWrapper.Cars.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if(carToUpdate == null)
        {
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
            return NotFound($"Car with id {id} not found");
        }
        
        _repoWrapper.Cars.Delete(car);
        _repoWrapper.Save();
        return NoContent();
    }
}