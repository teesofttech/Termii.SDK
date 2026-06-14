namespace Termii;

public sealed class UploadContactsRequest
{
    public string PhonebookId { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public Stream File { get; set; } = Stream.Null;

    public string FileName { get; set; } = "contacts.csv";

    public string ContentType { get; set; } = "text/csv";

    internal void Validate()
    {
        TermiiRequestValidation.Required(PhonebookId, nameof(PhonebookId));
        TermiiRequestValidation.Required(CountryCode, nameof(CountryCode));
        TermiiRequestValidation.Required(FileName, nameof(FileName));
        TermiiRequestValidation.Required(ContentType, nameof(ContentType));

        if (File is null || File == Stream.Null)
        {
            throw new ArgumentException("A contacts CSV file stream is required.", nameof(File));
        }
    }
}
