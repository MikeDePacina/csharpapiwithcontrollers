using Northwind.EntityModels; // To use Customer.
namespace Northwind.WebApi.Repositories;
public interface ICustomerRepository
{
  //task is async wrapper
  Task<Customer?> CreateAsync(Customer c);
  Task<Customer[]> RetrieveAllAsync();
  Task<Customer?> RetrieveAsync(int id);
  Task<Customer?> UpdateAsync(Customer c);
  Task<bool?> DeleteAsync(int id);
}