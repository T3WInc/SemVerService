using System;
using System.Linq;
using t3winc.version.common.Interfaces;
using t3winc.version.common.Models;
using t3winc.version.data.Helper;
using t3winc.version.domain;

namespace t3winc.version.data.Repos
{
    public class BranchRepo : IBranchRepo
    {
        private VersionContext _context;

        public BranchRepo(VersionContext context)
        {
            _context = context;
        }

        internal bool BranchExists(int id, string product, string branch)
        {
            ProductRepo repo = new ProductRepo(_context);
            var myProduct = repo.GetProductDomain(id, product);
            var results = _context.Branch.Any(e => e.Name == branch && e.Product == myProduct);
            return results;
        }

        internal Branch GetBranch(int version, string product, string branch)
        {
            ProductRepo repo = new ProductRepo(_context);
            var myProduct = repo.GetProductDomain(version, product);
            return _context.Branch.Where(e => e.Name == branch && e.Product == myProduct).FirstOrDefault();
        }

        internal Branch NewBranch(int versionId, string productName, string newbranch)
        {
            var version = _context.Version.Where(e => e.Id == versionId).FirstOrDefault();
            var product = _context.Product.Where(e => e.Version == version && e.Name == productName).FirstOrDefault();
            var suffix = VersionHelper.GetSuffix(newbranch);
            Branch branch = new Branch();
            switch (suffix)
            {
                case "alpha":
                    branch.Major = product.Major;
                    branch.Minor = product.Minor;
                    branch.Minor++;
                    branch.Patch = 0;
                    branch.Revision = 0;
                    branch.Suffix = "alpha";
                    break;
                case "beta":
                    branch.Major = product.Major;
                    branch.Minor = product.Minor;
                    branch.Patch = product.Patch;
                    branch.Patch++;
                    branch.Revision = 0;
                    branch.Suffix = "beta";
                    break;
                case "torn":
                    branch.Major = product.Major;
                    branch.Major++;
                    branch.Minor = 0;
                    branch.Patch = 0;
                    branch.Revision = 0;
                    branch.Suffix = "torn";
                    break;
            }
            branch.Version = $"{branch.Major}.{branch.Minor}.{branch.Patch}-{suffix}.{branch.Revision}";
            branch.Name = newbranch;
            branch.Product = product;
            branch.Status = "Active";
            _context.Branch.Add(branch);
            _context.SaveChanges();
            return branch;
        }
    }
}