using CrudRestApi.DTOs;
using CrudRestApi.Models;
using CrudRestApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CrudRestApi.Controllers;

[ApiController]
[Route("/users")]
public class UsersController : ControllerBase
{
    private readonly IRepository<User> _repository;

    // Repository is injected by .NET dependency injection
    public UsersController(IRepository<User> repository)
    {
        _repository = repository;
    }

    // GET /users?page=1&pageSize=10 — returns a paginated list of users
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) return BadRequest("Page must be greater than 0");
        if (pageSize < 1 || pageSize > 100) return BadRequest("PageSize must be between 1 and 100");

        var users = _repository.GetAll()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto { Id = u.Id, Name = u.Name, Email = u.Email });

        return Ok(users);
    }

    // GET /users/{id} — returns a single user or 404 if not found
    [HttpGet("{id}")]
    public ActionResult<UserDto> GetById(int id)
    {
        var user = _repository.GetById(id);
        if (user == null) return NotFound($"User with id {id} not found");
        return Ok(new UserDto { Id = user.Id, Name = user.Name, Email = user.Email });
    }

    // POST /users — creates a new user and returns 201 with the created resource
    [HttpPost]
    public ActionResult<UserDto> Create(CreateUserDto dto)
    {
        var user = new User { Name = dto.Name, Email = dto.Email };
        _repository.Add(user);
        var result = new UserDto { Id = user.Id, Name = user.Name, Email = user.Email };
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, result);
    }

    // PUT /users/{id} — updates an existing user or 404 if not found
    [HttpPut("{id}")]
    public ActionResult<UserDto> Update(int id, UpdateUserDto dto)
    {
        var user = new User { Name = dto.Name, Email = dto.Email };
        var updated = _repository.Update(id, user);
        if (updated == null) return NotFound($"User with id {id} not found");
        return Ok(new UserDto { Id = updated.Id, Name = updated.Name, Email = updated.Email });
    }

    // DELETE /users/{id} — deletes a user or 404 if not found
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var deleted = _repository.Delete(id);
        if (!deleted) return NotFound($"User with id {id} not found");
        return NoContent();
    }
}