using CRN_Technical_Assessment.Domain.Common;
using CRN_Technical_Assessment.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Domain.Entities
{
    public class Products:BaseEntities
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }

        public Categories Category { get; set; } = null!;

        public ProductStatus Status { get; set; }
            = ProductStatus.Active;

        public ProductCondition Condition { get; set; }
            = ProductCondition.New;

        public RecordStatus RecordStatus { get; set; }
            = RecordStatus.Active;
    }
}
