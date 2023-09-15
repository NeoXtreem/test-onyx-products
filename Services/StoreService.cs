using System.Drawing;
using Onyx.Products.Data;
using Onyx.Products.Models;

namespace Onyx.Products.Services;

public class StoreService
{
    private readonly StoreContext _storeDb;

    public StoreService(StoreContext storeContext)
    {
        _storeDb = storeContext;
    }

    public IReadOnlyCollection<Product> GetProducts(Color? colour)
    {
        return (colour is null ? _storeDb.Products : _storeDb.Products.Where(p => p.Colour == colour)).ToList().AsReadOnly();
    }
}