namespace Termii;

/// <summary>
/// Main entry point for the Termii SDK.
/// </summary>
public sealed class TermiiClient
{
    public TermiiClient(TermiiOptions options)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        Options.Validate();
    }

    public TermiiOptions Options { get; }
}
