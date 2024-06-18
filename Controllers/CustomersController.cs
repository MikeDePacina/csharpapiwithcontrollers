using Microsoft.AspNetCore.Mvc;
using Northwind.WebApi.Repositories;
using Northwind.EntityModels;

namespace Northwind.WebApi.Controllers;

// Base address: api/customers
[Route("api/customers")]
[ApiController]
public class CustomersController : ControllerBase
{
  private readonly ICustomerRepository _repo;
  // Constructor injects repository registered in Program.cs.
  public CustomersController(ICustomerRepository repo)
  {
    _repo = repo;
  }

  // GET: api/customers

[HttpGet]
[ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
public async Task<IEnumerable<Customer>> GetCustomers()
{
  
    return await _repo.RetrieveAllAsync();
  
}

// GET: api/customers/[id]
[HttpGet("{id}", Name = nameof(GetCustomer))] // Named route.
[ProducesResponseType(200, Type = typeof(Customer))]
[ProducesResponseType(404)]
public async Task<IActionResult> GetCustomer(string id)
{
  int idnum = int.Parse(id);
  Customer? c = await _repo.RetrieveAsync(idnum);
  if (c == null)
  {
    return NotFound(); // 404 Resource not found.
  }
  return Ok(c); // 200 OK with customer in body
}

// POST: api/customers
// BODY: Customer (JSON, XML)
[HttpPost]
[ProducesResponseType(201, Type = typeof(Customer))]
[ProducesResponseType(400)]
public async Task<IActionResult> Create([FromBody] Customer c)
{
  if (c == null)
  {
    return BadRequest(); // 400 Bad request.
  }
  Customer? addedCustomer = await _repo.CreateAsync(c);
  if (addedCustomer == null)
  {
    return BadRequest("Repository failed to create customer.");
  }
  else
  {
    return CreatedAtRoute( // 201 Created.
      routeName: nameof(GetCustomer),
      routeValues: new { id = addedCustomer.CustomerId },
      value: addedCustomer);
  }
}

// PUT: api/customers/[id]
// BODY: Customer (JSON, XML)
[HttpPut("{id}")]
[ProducesResponseType(204)]
[ProducesResponseType(400)]
[ProducesResponseType(404)]
public async Task<IActionResult> Update(
  string id, [FromBody] Customer c)
{
  int idnum = int.Parse(id);
  if (c == null || c.CustomerId != idnum)
  {
    return BadRequest(); // 400 Bad request.
  }
  Customer? existing = await _repo.RetrieveAsync(idnum);
  if (existing == null)
  {
    return NotFound(); // 404 Resource not found.
  }
  await _repo.UpdateAsync(c);
  return new NoContentResult(); // 204 No content.
}

// DELETE: api/customers/[id]
[HttpDelete("{id}")]
[ProducesResponseType(204)]
[ProducesResponseType(400)]
[ProducesResponseType(404)]
public async Task<IActionResult> Delete(string id)
{
  int idnum = int.Parse(id);
  Customer? existing = await _repo.RetrieveAsync(idnum);
  if (existing == null)
  {
    return NotFound(); // 404 Resource not found.
  }
  bool? deleted = await _repo.DeleteAsync(idnum);
  if (deleted.HasValue && deleted.Value) // Short circuit AND.
  {
    return new NoContentResult(); // 204 No content.
  }
  else
  {
    
    return BadRequest( // 400 Bad request.
      $"Customer {idnum} was found but failed to delete.");
  }
}
}