using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            MedicineIngredients = new HashSet<MedicineIngredient>();
            Medicines = new HashSet<Medicine>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<MedicineIngredient> MedicineIngredients { get; set; }
        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
