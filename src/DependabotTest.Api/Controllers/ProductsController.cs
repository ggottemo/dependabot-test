using AutoMapper;
using DependabotTest.Library;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DependabotTest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IValidator<Product> _validator;
    private readonly ILogger<ProductsController> _logger;

    // In-memory storage for demo purposes
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 999.99m, Description = "High-performance laptop" },
        new Product { Id = 2, Name = "Mouse", Price = 29.99m, Description = "Wireless mouse" },
        new Product { Id = 3, Name = "Keyboard", Price = 79.99m, Description = "Mechanical keyboard" }
    };

    public ProductsController(IMapper mapper, IValidator<Product> validator, ILogger<ProductsController> logger)
    {
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDto>> GetProducts()
    {
        _logger.LogInformation("Getting all products");
        var productDtos = _mapper.Map<List<ProductDto>>(_products);
        return Ok(productDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<ProductDto> GetProduct(int id)
    {
        _logger.LogInformation("Getting product with ID: {ProductId}", id);
        var product = _products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", id);
            return NotFound();
        }

        var productDto = _mapper.Map<ProductDto>(product);
        return Ok(productDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] Product product)
    {
        _logger.LogInformation("Creating new product: {ProductJson}", JsonConvert.SerializeObject(product));

        var validationResult = await _validator.ValidateAsync(product);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Product validation failed: {Errors}", JsonConvert.SerializeObject(validationResult.Errors));
            return BadRequest(validationResult.Errors);
        }

        product.Id = _products.Max(p => p.Id) + 1;
        _products.Add(product);

        var productDto = _mapper.Map<ProductDto>(product);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        _logger.LogInformation("Updating product with ID: {ProductId}", id);

        var existingProduct = _products.FirstOrDefault(p => p.Id == id);
        if (existingProduct == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", id);
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(product);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Product validation failed: {Errors}", JsonConvert.SerializeObject(validationResult.Errors));
            return BadRequest(validationResult.Errors);
        }

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        existingProduct.Description = product.Description;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", id);

        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", id);
            return NotFound();
        }

        _products.Remove(product);
        return NoContent();
    }
}
