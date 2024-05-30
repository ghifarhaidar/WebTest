using System;
using System.Collections.Generic;

namespace WebTest.Models
{
    public partial class DescriptionMedicine
    {
        public int Id { get; set; }
        public int DescriptionId { get; set; }
        public int MedicineId { get; set; }
        public int Count { get; set; }

        public virtual Description Description { get; set; } = null!;
        public virtual Medicine Medicine { get; set; } = null!;
    }
}
