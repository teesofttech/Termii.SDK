using System.Text.Json.Serialization;

namespace Termii;

public sealed class SendWhatsAppTemplateMediaRequest : SendWhatsAppTemplateRequest
{
    [JsonPropertyName("media")]
    public WhatsAppTemplateMedia Media { get; set; } = new();

    internal override void Validate()
    {
        base.Validate();

        if (Media is null)
        {
            throw new ArgumentNullException(nameof(Media));
        }

        Media.Validate();
    }
}
