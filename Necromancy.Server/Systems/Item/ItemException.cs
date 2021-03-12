using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public class ItemException :  Exception
    {
        public ItemException(ItemExceptionType exceptionType) { ExceptionType = exceptionType; }
        public ItemExceptionType ExceptionType {get; private set;}
    }
}
