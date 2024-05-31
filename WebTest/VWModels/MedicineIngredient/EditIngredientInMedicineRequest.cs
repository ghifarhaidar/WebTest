namespace WebTest.VWModels.MedicineIngredient
{
    public class EditIngredientInMedicineRequest
    {
        public int MedicineIngredientId { get; set; }
        public int MedicineId { get; set; }
        public IngredientInfo Ingredient { get; set; }
    }
}
