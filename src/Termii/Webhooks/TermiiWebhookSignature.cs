using System.Security.Cryptography;
using System.Text;

namespace Termii;

public static class TermiiWebhookSignature
{
    private const string Sha512Prefix = "sha512=";

    public static bool Verify(
        string payload,
        string? signature,
        string secretKey)
    {
        if (payload is null)
        {
            throw new ArgumentNullException(nameof(payload));
        }

        return Verify(Encoding.UTF8.GetBytes(payload), signature, secretKey);
    }

    public static bool Verify(
        byte[] payload,
        string? signature,
        string secretKey)
    {
        if (payload is null)
        {
            throw new ArgumentNullException(nameof(payload));
        }

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new ArgumentException("A Termii webhook secret key is required.", nameof(secretKey));
        }

        if (!TryDecodeSignature(signature, out var receivedSignature))
        {
            return false;
        }

        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey));
        var expectedSignature = hmac.ComputeHash(payload);

        return FixedTimeEquals(expectedSignature, receivedSignature);
    }

    private static bool TryDecodeSignature(string? signature, out byte[] value)
    {
        value = Array.Empty<byte>();

        if (string.IsNullOrWhiteSpace(signature))
        {
            return false;
        }

        var normalized = signature!.Trim();

        if (normalized.StartsWith(Sha512Prefix, StringComparison.OrdinalIgnoreCase))
        {
            normalized = normalized.Substring(Sha512Prefix.Length);
        }

        if (normalized.Length != 128)
        {
            return false;
        }

        var bytes = new byte[64];

        for (var i = 0; i < bytes.Length; i++)
        {
            var high = FromHex(normalized[i * 2]);
            var low = FromHex(normalized[(i * 2) + 1]);

            if (high < 0 || low < 0)
            {
                return false;
            }

            bytes[i] = (byte)((high << 4) | low);
        }

        value = bytes;
        return true;
    }

    private static int FromHex(char value)
    {
        if (value >= '0' && value <= '9')
        {
            return value - '0';
        }

        if (value >= 'a' && value <= 'f')
        {
            return value - 'a' + 10;
        }

        if (value >= 'A' && value <= 'F')
        {
            return value - 'A' + 10;
        }

        return -1;
    }

    private static bool FixedTimeEquals(byte[] left, byte[] right)
    {
        if (left.Length != right.Length)
        {
            return false;
        }

        var difference = 0;

        for (var i = 0; i < left.Length; i++)
        {
            difference |= left[i] ^ right[i];
        }

        return difference == 0;
    }
}
