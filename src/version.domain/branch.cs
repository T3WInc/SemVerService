using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t3winc.version.domain
{
    public class Branch
    {
        // Identity fields
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        // Values to work from
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public int Revision { get; set; }
        public string Suffix { get; set; }

        // Last calculated version number
        public string Version { get; set; }

        // Each branch has only one product
        public Product Product { get; set; }
    }
}
