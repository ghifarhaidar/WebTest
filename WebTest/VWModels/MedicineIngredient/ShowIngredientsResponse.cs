namespace WebTest.VWModels.MedicineIngredient
{
    public class ShowIngredientsResponse
    {
        public int MedicineId { get; set; }
        public List<IngredientInfo> Ingredients { get; set; } = new();
    }
}
