using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Validation
{
    public class DomainExceptionValidation : Exception
    {
        public DomainExceptionValidation(string error) : base(error) { }
    }
}
