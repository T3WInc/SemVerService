using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _model = t3winc.version.common.Models;
using t3winc.version.domain;

namespace t3winc.version.common.Interfaces
{
    public interface IProductRepo
    {
        bool ProductExist(int versionId, string product);
        string NewProduct(int versionId, string product);
        _model.Product GetProduct(int versionId, string name);
        void IncrementMajor(int versionId, string name);
        void IncrementMinor(int versionId, string name);
        void IncrementPatch(int versionId, string name);
        List<_model.Products> GetAllProducts(int version);
    }
}
