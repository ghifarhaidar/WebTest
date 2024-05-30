using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Medicine
    {
        public Medicine()
        {
            DescriptionMedicines = new HashSet<DescriptionMedicine>();
            MedicineIngredients = new HashSet<MedicineIngredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Dose { get; set; }
        public int ActiveSubstanceId { get; set; }
        public int InStock { get; set; }
        public int FactoryId { get; set; }
        public string TradeName { get; set; } = null!;

        public virtual Ingredient ActiveSubstance { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual Factory Factory { get; set; } = null!;
        public virtual ICollection<DescriptionMedicine> DescriptionMedicines { get; set; }
        public virtual ICollection<MedicineIngredient> MedicineIngredients { get; set; }
    }
}
