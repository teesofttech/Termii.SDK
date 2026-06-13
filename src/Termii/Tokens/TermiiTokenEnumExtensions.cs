namespace Termii;

internal static class TermiiTokenEnumExtensions
{
    public static string ToWireValue(this TermiiTokenPinType pinType)
    {
        return pinType switch
        {
            TermiiTokenPinType.Numeric => "NUMERIC",
            TermiiTokenPinType.Alphanumeric => "ALPHANUMERIC",
            _ => throw new ArgumentOutOfRangeException(nameof(pinType), pinType, "Unsupported Termii token PIN type."),
        };
    }
}
