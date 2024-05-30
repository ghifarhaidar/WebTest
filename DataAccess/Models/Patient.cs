using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Descriptions = new HashSet<Description>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal PhoneNumber { get; set; }

        public virtual ICollection<Description> Descriptions { get; set; }
    }
}
