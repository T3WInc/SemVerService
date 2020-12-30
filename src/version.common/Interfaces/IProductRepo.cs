using System;
using System.Collections.Generic;
using System.Text;
using t3winc.version.common.Models;

namespace t3winc.version.common.Interfaces
{
    public interface IProductRepo
    {
        bool ProductExist(int versionId, string product);
        string NewProduct(int versionId, string product);
        Product GetProduct(int versionId, string name);
        void IncrementMajor(int versionId, string name);
        void IncrementMinor(int versionId, string name);
        void IncrementPatch(int versionId, string name);

    }
}
