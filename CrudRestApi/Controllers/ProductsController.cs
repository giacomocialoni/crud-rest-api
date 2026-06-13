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

    // Repository is injected by .NET dependency injection
    public ProductsController(IRepository<Product> repository)
    {
        _repository = repository;
    }

    // GET /products?page=1&pageSize=10 — returns a paginated list of products
    [HttpGet]
    public ActionResult<IEnumerable<ProductDto>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) return BadRequest("Page must be greater than 0");
        if (pageSize < 1 || pageSize > 100) return BadRequest("PageSize must be between 1 and 100");

        var products = _repository.GetAll()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto { Id = p.Id, Name = p.Name, Price = p.Price });

        return Ok(products);
    }

    // GET /products/{id} — returns a single product or 404 if not found
    [HttpGet("{id}")]
    public ActionResult<ProductDto> GetById(int id)
    {
        var product = _repository.GetById(id);
        if (product == null) return NotFound($"Product with id {id} not found");
        return Ok(new ProductDto { Id = product.Id, Name = product.Name, Price = product.Price });
    }

    // POST /products — creates a new product and returns 201 with the created resource
    [HttpPost]
    public ActionResult<ProductDto> Create(CreateProductDto dto)
    {
        var product = new Product { Name = dto.Name, Price = dto.Price };
        _repository.Add(product);
        var result = new ProductDto { Id = product.Id, Name = product.Name, Price = product.Price };
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, result);
    }

    // PUT /products/{id} — updates an existing product or 404 if not found
    [HttpPut("{id}")]
    public ActionResult<ProductDto> Update(int id, UpdateProductDto dto)
    {
        var product = new Product { Name = dto.Name, Price = dto.Price };
        var updated = _repository.Update(id, product);
        if (updated == null) return NotFound($"Product with id {id} not found");
        return Ok(new ProductDto { Id = updated.Id, Name = updated.Name, Price = updated.Price });
    }

    // DELETE /products/{id} — deletes a product or 404 if not found
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var deleted = _repository.Delete(id);
        if (!deleted) return NotFound($"Product with id {id} not found");
        return NoContent();
    }
}