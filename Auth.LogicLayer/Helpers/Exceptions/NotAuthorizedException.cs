using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.LogicLayer.Helpers.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public string Message { get; set; }
        public NotAuthorizedException(string messsage)
        {
            this.Message = messsage;
        }
    }
}
