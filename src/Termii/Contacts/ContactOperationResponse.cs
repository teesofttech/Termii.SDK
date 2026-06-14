namespace Termii;

public sealed class ContactOperationResponse
{
    public int? Code { get; set; }

    public ContactOperationData? Data { get; set; }

    public string? Message { get; set; }

    public string? Status { get; set; }
}

public sealed class ContactOperationData
{
    public string? Message { get; set; }
}
