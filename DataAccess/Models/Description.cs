using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Description
    {
        public Description()
        {
            DescriptionMedicines = new HashSet<DescriptionMedicine>();
        }

        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description1 { get; set; }

        public virtual Patient Patient { get; set; } = null!;
        public virtual ICollection<DescriptionMedicine> DescriptionMedicines { get; set; }
    }
}
