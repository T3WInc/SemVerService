using System;
using System.Collections.Generic;
using System.Text;

namespace t3winc.version.common.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // The latest version from the master branch
        public string Master { get; set; }

        // Values to work from
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public int Revision { get; set; }

    }
}
