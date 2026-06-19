using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Domain.Exceptions
{
    public class DuplicateEntityException:DomainException
    {
        public DuplicateEntityException(string message)
        : base(message)
        {
        }
    }
}
