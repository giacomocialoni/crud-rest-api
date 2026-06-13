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

    public UsersController(IRepository<User> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAll()
    {
        var users = _repository.GetAll()
            .Select(u => new UserDto { Id = u.Id, Name = u.Name, Email = u.Email });
        return Ok(users);
    }

    [HttpGet("{id}")]
    public ActionResult<UserDto> GetById(int id)
    {
        var user = _repository.GetById(id);
        if (user == null) return NotFound();
        return Ok(new UserDto { Id = user.Id, Name = user.Name, Email = user.Email });
    }

    [HttpPost]
    public ActionResult<UserDto> Create(CreateUserDto dto)
    {
        var user = new User { Name = dto.Name, Email = dto.Email };
        _repository.Add(user);
        var result = new UserDto { Id = user.Id, Name = user.Name, Email = user.Email };
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, result);
    }

    [HttpPut("{id}")]
    public ActionResult<UserDto> Update(int id, UpdateUserDto dto)
    {
        var user = new User { Name = dto.Name, Email = dto.Email };
        var updated = _repository.Update(id, user);
        if (updated == null) return NotFound();
        return Ok(new UserDto { Id = updated.Id, Name = updated.Name, Email = updated.Email });
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var deleted = _repository.Delete(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}