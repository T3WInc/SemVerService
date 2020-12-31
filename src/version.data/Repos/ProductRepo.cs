using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t3winc.version.common.Interfaces;
using _model = t3winc.version.common.Models;
using t3winc.version.domain;

namespace t3winc.version.data.Repos
{
    public class ProductRepo : IProductRepo
    {
        private VersionContext _context;

        public ProductRepo(VersionContext context)
        {
            _context = context;
        }

        public bool ProductExist(int versionId, string product)
        {
            var version = _context.Version.Where(e => e.Id == versionId).FirstOrDefault();
            return _context.Product.Any(e => e.Name == product && e.Version == version);
        }

        public string NewProduct(int versionId, string name)
        {
            var version = _context.Version.Where(e => e.Id == versionId).FirstOrDefault();
            if(ProductExist(version.Id, name))
            {
                return "Sorry, this product already exists";
            }
            Product product = new Product();
            product.Version = version;
            product.Name = name;
            product.Major = 0;
            product.Minor = 0;
            product.Patch = 0;
            product.Revision = 0;
            product.Master = "0.0.0.0";
            _context.Product.Add(product);
            _context.SaveChanges();

            return product.Master;
        }

        public _model.Product GetProduct(int versionId, string name)
        {
            var version = _context.Version.Where(e => e.Id == versionId).FirstOrDefault();
            var result = _context.Product.Where(e => e.Name == name && e.Version == version).FirstOrDefault();
            _model.Product product = new _model.Product();
            
            product.Id = result.Id;
            product.Major = result.Major;
            product.Minor = result.Minor;
            product.Name = result.Name;
            product.Patch = result.Patch;
            product.Revision = result.Revision;
            product.Master = result.Master;

            return product;
        }

        public List<_model.Products> GetAllProducts(int versionId)
        {
            var version = _context.Version.Where(e => e.Id == versionId).FirstOrDefault();
            var result = _context.Product.Where(e => e.Version == version).ToList();
            List<_model.Products> products = new List<_model.Products>();
            foreach(Product product in result)
            {
                _model.Products _products = new _model.Products();
                _products.Name = product.Name;
                _products.Master = product.Master;
                products.Add(_products);
            }
            
            return products;
        }

        public Product GetProductDomain(int versionId, string name)
        {
            var version = _context.Version.Where(e => e.Id == versionId).FirstOrDefault();
            return _context.Product.Where(e => e.Name == name && e.Version == version).FirstOrDefault();
        }

        public void IncrementMajor(int versionId, string product)
        {
            var version = _context.Version.Where(e => e.Id == versionId);
            var result = _context.Product.Where(e => e.Name == product && e.Version == version).FirstOrDefault();
            result.Major++;
            result.Master = $"{result.Major}.{result.Minor}.{result.Patch}.{result.Revision}";
            _context.SaveChanges();
        }

        public void IncrementMinor(int versionId, string product)
        {
            var version = _context.Version.Where(e => e.Id == versionId);
            var result = _context.Product.Where(e => e.Name == product && e.Version == version).FirstOrDefault();
            result.Minor++;
            result.Master = $"{result.Major}.{result.Minor}.{result.Patch}.{result.Revision}";
            _context.SaveChanges();
        }

        public void IncrementPatch(int versionId, string product)
        {
            var version = _context.Version.Where(e => e.Id == versionId);
            var result = _context.Product.Where(e => e.Name == product && e.Version == version).FirstOrDefault();
            result.Patch++;
            result.Master = $"{result.Major}.{result.Minor}.{result.Patch}.{result.Revision}";
            _context.SaveChanges();
        }

    }
}
