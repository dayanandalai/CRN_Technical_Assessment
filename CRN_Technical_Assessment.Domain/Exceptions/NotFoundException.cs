using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Domain.Exceptions
{
    public class NotFoundException: DomainException
    {
        public NotFoundException(string entityName, object key)
        : base($"{entityName} with Id '{key}' was not found.")
        {

        }
    }
}
