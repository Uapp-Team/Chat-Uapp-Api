using System;
using System.Numerics;
using System.Text;

namespace ChatUapp.Core.Extensions;

public static class GuidExtensions
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; // Base62 alphabet
    public static string ToBotName(this Guid guid)
    {
        return "B" + ToBase62(guid.ToByteArray());
    }

    public static string ToTrainSourceTitle(this Guid guid)
    {
        return "T" + ToBase62(guid.ToByteArray());
    }

    public static string ToSessionTitle(this Guid guid)
    {
        return "S" + ToBase62(guid.ToByteArray());
    }

    private static string ToBase62(byte[] bytes)
    {
        // Convert byte array to a big integer
        BigInteger bigInt = new BigInteger(bytes, isUnsigned: true, isBigEndian: true);

        StringBuilder result = new StringBuilder();
        while (bigInt > 0)
        {
            bigInt = BigInteger.DivRem(bigInt, 62, out BigInteger remainder);
            result.Insert(0, Alphabet[(int)remainder]);
        }

        return result.ToString();
    }
}
