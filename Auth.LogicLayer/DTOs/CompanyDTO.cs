using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.LogicLayer.DTOs
{
    public class CompanyDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid PublicId { get; set; }
    }
}
