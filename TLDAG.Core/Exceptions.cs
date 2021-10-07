using System;

namespace TLDAG.Core
{
    public static class Exceptions
    {
        public static NotImplementedException NotYetImplemented(string method)
        {
            return new NotImplementedException(method);
        }
    }
}
