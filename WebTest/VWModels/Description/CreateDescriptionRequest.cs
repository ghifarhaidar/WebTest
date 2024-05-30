namespace WebTest.VWModels.Medicine
{
    public class CreateDescriptionRequest
    {
        public int PatientId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description1 { get; set; }

    }
}
