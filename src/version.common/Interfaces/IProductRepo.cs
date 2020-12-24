using System;
using System.Collections.Generic;
using System.Text;
using t3winc.version.common.Models;

namespace t3winc.version.common.Interfaces
{
    public interface IProductRepo
    {
        string NewProduct(int versionId, string name);
        Product GetProduct(string name);
        void IncrementMajor(string name);
        void IncrementMinor(string name);
        void IncrementPatch(string name);

    }
}
