using System;

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
