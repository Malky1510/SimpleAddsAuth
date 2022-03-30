using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAddsAuth.data
{
    public class Add
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool IsOwner { get; set;}
        public int LoginId { get; set; }
    }
}
