namespace WebTest.VWModels.Description
{
    public class EditDescriptionRequest
    {
        public string Name { get; set; } = null!;
        public int PatientId { get; set; }
        public string? Description1 { get; set; }

    }
}
