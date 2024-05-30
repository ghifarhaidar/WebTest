using System;
using System.Collections.Generic;

namespace WebTest.Models
{
    public partial class Factory
    {
        public Factory()
        {
            Medicines = new HashSet<Medicine>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
