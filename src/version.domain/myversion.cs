using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t3winc.version.domain
{
    public class MyVersion
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Organization { get; set; }

        // An organization can have many products
        public ICollection<Product> Products { get; set; }
    }
}
