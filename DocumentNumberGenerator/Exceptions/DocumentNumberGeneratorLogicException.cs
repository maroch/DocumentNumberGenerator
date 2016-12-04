using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentNumberGenerator.Exceptions
{
    public class DocumentNumberGeneratorLogicException : Exception
    {
        public DocumentNumberGeneratorLogicException()
        {
        }

        public DocumentNumberGeneratorLogicException(string message)
            : base(message)
        {
        }

        public DocumentNumberGeneratorLogicException(string message, Exception inner)
        : base(message, inner)
    {
        }
    }
}
