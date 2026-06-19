using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Domain.Exceptions
{
    public class ValidationException:DomainException
    {
        public IReadOnlyCollection<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors)
            : base("One or more validation failures occurred.")
        {
            Errors = errors.ToList().AsReadOnly();
        }
    }
}
