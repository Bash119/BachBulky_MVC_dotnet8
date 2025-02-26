using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Data.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    
    private readonly ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db):base(db)
    {
        _db = db;
    }
    public void Update(Product product)
    {
        _db.Products.Update(product);
    }
}
