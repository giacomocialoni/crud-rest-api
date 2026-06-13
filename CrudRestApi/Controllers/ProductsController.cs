using CrudRestApi.DTOs;
using CrudRestApi.Models;
using CrudRestApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CrudRestApi.Controllers;

[ApiController]
[Route("/products")]
public class ProductsController : ControllerBase
{
    private readonly IRepository<Product> _repository;

    public ProductsController(IRepository<Product> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDto>> GetAll()
    {
        var products = _repository.GetAll()
            .Select(p => new ProductDto { Id = p.Id, Name = p.Name, Price = p.Price });
        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<ProductDto> GetById(int id)
    {
        var product = _repository.GetById(id);
        if (product == null) return NotFound();
        return Ok(new ProductDto { Id = product.Id, Name = product.Name, Price = product.Price });
    }

    [HttpPost]
    public ActionResult<ProductDto> Create(CreateProductDto dto)
    {
        var product = new Product { Name = dto.Name, Price = dto.Price };
        _repository.Add(product);
        var result = new ProductDto { Id = product.Id, Name = product.Name, Price = product.Price };
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, result);
    }

    [HttpPut("{id}")]
    public ActionResult<ProductDto> Update(int id, UpdateProductDto dto)
    {
        var product = new Product { Name = dto.Name, Price = dto.Price };
        var updated = _repository.Update(id, product);
        if (updated == null) return NotFound();
        return Ok(new ProductDto { Id = updated.Id, Name = updated.Name, Price = updated.Price });
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var deleted = _repository.Delete(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}