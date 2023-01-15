namespace LogisticApi.DTO
{
    public class UpdateDocumentRequest
    {
        public bool? ActionType { get; set; }

        public string? ContragentName { get; set; }

        public string Storage { get; set; } = null!;
    }
}
