using Microsoft.AspNetCore.Mvc;
using RestWebApp.Contracts;
using RestWebApp.Entities;
using RestWebApp.Entities.Models;

namespace RestWebApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IRepositoryWrapper _repoWrapper;
    private readonly ILoggerManager _logger;
    
    public UserController(IRepositoryWrapper repoWrapper, RepositoryContext context,
        ILoggerManager logger)
    {
        _repoWrapper = repoWrapper;
        _logger = logger;
        context.Database.EnsureCreated();
    }
    
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _repoWrapper.Users.GetAll();
        _logger.LogInfo("Returned all users from database.");
        return Ok(users);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetUserById(int id)
    {
        var user = _repoWrapper.Users.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if(user == null)
        {
            _logger.LogWarn("User with id: " + id + " was not found in the database.");
            return NotFound($"User with this id {user.Id} not found");
        }
        
        _logger.LogInfo("Returned user with id: " + id + " from database.");
        return Ok(user);
    }
    
    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        if(user == null)
        {
            _logger.LogError("User object sent from client is null.");
            return BadRequest($"Cannot create an empty object of type {typeof(User)}");
        }
        
        _repoWrapper.Users.Create(user);
        _repoWrapper.Save();
        _logger.LogInfo("User with id: " + user.Id + " was created in the database.");
        return Ok(user);
    }
    
    [HttpPut("{id:int}")]
    public IActionResult UpdateUser(int id, [FromBody] User user)
    {
        if(user == null || user.Id != id)
        {
            _logger.LogError("User object sent from client is null.");
            return BadRequest("Object user is null or id is not equal to id in url");
        }
        
        var userToUpdate = _repoWrapper.Users.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if(userToUpdate == null)
        {
            _logger.LogWarn("User with id: " + id + " was not found in the database.");
            return NotFound($"User with id {id} not found");
        }
        
        userToUpdate.Name = user.Name;
        userToUpdate.Password = user.Password;
        userToUpdate.Role = user.Role;
        
        _repoWrapper.Users.Update(userToUpdate);
        _repoWrapper.Save();
        return Ok(userToUpdate);
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult UserCar(int id)
    {
        var user = _repoWrapper.Users.GetByCondition(x => x.Id == id).FirstOrDefault();
        
        if (user == null)
        {
            _logger.LogWarn("User with id: " + id + " was not found in the database.");
            return NotFound($"User with id {id} not found");
        }
        
        _repoWrapper.Users.Delete(user);
        _repoWrapper.Save();
        _logger.LogInfo("User with id: " + id + " was deleted from the database.");
        return NoContent();
    }
}