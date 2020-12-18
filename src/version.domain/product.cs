using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t3winc.version.domain
{
    public class Product
    {
        // Identity fields
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        
        // The latest version from the master branch
        public string Master { get; set; }

        // Values to work from
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public int Revision { get; set; }

        // A Product can have many branches but we do not accept master branches.
        // The master branch is held in the product table as there is only one.
        public ICollection<Branch> Branches { get; set; }

        // Each Product only has one Version record which represents the Organization.
        public MyVersion Version { get; set; }
    }
}
