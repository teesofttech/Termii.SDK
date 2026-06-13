namespace Termii;

internal static class TermiiMessagingEnumExtensions
{
    public static string ToWireValue(this TermiiMessageChannel channel)
    {
        return channel switch
        {
            TermiiMessageChannel.Generic => "generic",
            TermiiMessageChannel.Dnd => "dnd",
            TermiiMessageChannel.WhatsApp => "whatsapp",
            TermiiMessageChannel.Voice => "voice",
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, "Unsupported Termii message channel."),
        };
    }

    public static string ToWireValue(this TermiiMessageType type)
    {
        return type switch
        {
            TermiiMessageType.Plain => "plain",
            TermiiMessageType.Unicode => "unicode",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported Termii message type."),
        };
    }
}
