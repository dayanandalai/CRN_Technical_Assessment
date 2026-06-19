using CRN_Technical_Assessment.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Domain.Entities
{
    public class Categories:BaseEntities
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<Products> Products { get; set; }
            = new List<Products>();
    }
}
