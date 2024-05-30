using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class PatientMedicine
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int MedicineId { get; set; }
        public int Count { get; set; }

        public virtual Medicine Medicine { get; set; } = null!;
        public virtual Patient Patient { get; set; } = null!;
    }
}
