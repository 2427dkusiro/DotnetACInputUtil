using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace InputUtil.GenericRef;

#region library
#pragma warning disable CA1050
public static class InputUtil
{
    public static InputToken<T, SpanParsableImpl<T>> ReadLine<T>() where T : ISpanParsable<T>
    {
        return new InputToken<T, SpanParsableImpl<T>>(Console.ReadLine().AsSpan());
    }

    public static InputToken<string, StringImpl> ReadLineString()
    {
        return new InputToken<string, StringImpl>(Console.ReadLine().AsSpan());
    }
}

public interface IReadNextValueImpl<TResult>
{
    public static abstract TResult ReadNextValue(ref ReadOnlySpan<char> _input, char separator, IFormatProvider _formatProvider);

    public static abstract bool TryReadNextValue(ref ReadOnlySpan<char> _input, char separator, IFormatProvider _formatProvider, out TResult value);
}

public readonly struct SpanParsableImpl<T> : IReadNextValueImpl<T> where T : ISpanParsable<T>
{
    public static T ReadNextValue(ref ReadOnlySpan<char> _input, char separator, IFormatProvider _formatProvider)
    {
        var index = _input.IndexOf(separator);
        ReadOnlySpan<char> target = index == -1 ? _input : _input[..index];
        if (target.Length == 0)
        {
            ThrowSeparatorNotFound();
        }
        var value = T.Parse(target, _formatProvider);
        _input = index != -1 ? _input[(index + 1)..] : Array.Empty<char>();
        return value;
    }

    private static void ThrowSeparatorNotFound()
    {
        throw new ArgumentException($"Separator not found");
    }

    public static bool TryReadNextValue(ref ReadOnlySpan<char> _input, char separator, IFormatProvider _formatProvider, out T value)
    {
        var index = _input.IndexOf(separator);
        ReadOnlySpan<char> target = index == -1 ? _input : _input[..index];
        if (target.Length == 0)
        {
            value = default!;
            return false;
        }
        value = T.Parse(target, _formatProvider);
        _input = index != -1 ? _input[(index + 1)..] : Array.Empty<char>();
        return true;
    }
}

public readonly struct StringImpl : IReadNextValueImpl<string>
{
    public static string ReadNextValue(ref ReadOnlySpan<char> _input, char separator, IFormatProvider _formatProvider)
    {
        var index = _input.IndexOf(separator);
        ReadOnlySpan<char> target = index == -1 ? _input : _input[..index];
        if (target.Length == 0)
        {
            ThrowSeparatorNotFound();
        }
        var value = target.ToString();
        _input = index != -1 ? _input[(index + 1)..] : Array.Empty<char>();
        return value;
    }

    private static void ThrowSeparatorNotFound()
    {
        throw new ArgumentException($"Separator not found");
    }

    public static bool TryReadNextValue(ref ReadOnlySpan<char> _input, char separator, IFormatProvider _formatProvider, out string value)
    {
        var index = _input.IndexOf(separator);
        ReadOnlySpan<char> target = index == -1 ? _input : _input[..index];
        if (target.Length == 0)
        {
            value = default!;
            return false;
        }
        value = target.ToString();
        _input = index != -1 ? _input[(index + 1)..] : Array.Empty<char>();
        return true;
    }
}

public ref struct InputToken<TResult, TImpl> where TImpl : IReadNextValueImpl<TResult>
{
    public ReadOnlySpan<char> _input;

    private readonly IFormatProvider _formatProvider;

    private readonly char _separator;

#pragma warning disable CS8618 // Property Current is initialized in MoveNext
    public InputToken(ReadOnlySpan<char> input)
    {
        _input = input;
        _formatProvider = System.Globalization.CultureInfo.InvariantCulture;
        _separator = ' ';
    }

    public InputToken(ReadOnlySpan<char> input, IFormatProvider formatProvider, char separator)
    {
        _input = input;
        _formatProvider = formatProvider;
        _separator = separator;
    }
#pragma warning restore CS8618

    public static implicit operator TResult(InputToken<TResult, TImpl> token)
    {
        return token.ReadNextValue();
    }

    public void Deconstruct(out TResult val1, out TResult val2)
    {
        val1 = ReadNextValue();
        val2 = ReadNextValue();
    }

    public void Deconstruct(out TResult val1, out TResult val2, out TResult val3)
    {
        val1 = ReadNextValue();
        val2 = ReadNextValue();
        val3 = ReadNextValue();
    }

    public void Deconstruct(out TResult val1, out TResult val2, out TResult val3, out TResult val4)
    {
        val1 = ReadNextValue();
        val2 = ReadNextValue();
        val3 = ReadNextValue();
        val4 = ReadNextValue();
    }

    public void Deconstruct(out TResult val1, out TResult val2, out TResult val3, out TResult val4, out TResult val5)
    {
        val1 = ReadNextValue();
        val2 = ReadNextValue();
        val3 = ReadNextValue();
        val4 = ReadNextValue();
        val5 = ReadNextValue();
    }

    public void Deconstruct(out TResult val1, out TResult val2, out TResult val3, out TResult val4, out TResult val5, out TResult val6)
    {
        val1 = ReadNextValue();
        val2 = ReadNextValue();
        val3 = ReadNextValue();
        val4 = ReadNextValue();
        val5 = ReadNextValue();
        val6 = ReadNextValue();
    }

    public void Deconstruct(out TResult val1, out TResult val2, out TResult val3, out TResult val4, out TResult val5, out TResult val6, out TResult val7)
    {
        val1 = ReadNextValue();
        val2 = ReadNextValue();
        val3 = ReadNextValue();
        val4 = ReadNextValue();
        val5 = ReadNextValue();
        val6 = ReadNextValue();
        val7 = ReadNextValue();
    }

    public void Deconstruct(out TResult val1, out TResult val2, out TResult val3, out TResult val4, out TResult val5, out TResult val6, out TResult val7, out TResult val8)
    {
        val1 = ReadNextValue();
        val2 = ReadNextValue();
        val3 = ReadNextValue();
        val4 = ReadNextValue();
        val5 = ReadNextValue();
        val6 = ReadNextValue();
        val7 = ReadNextValue();
        val8 = ReadNextValue();
    }

    private TResult ReadNextValue()
    {
        return TImpl.ReadNextValue(ref _input, _separator, _formatProvider);
    }

    public readonly InputToken<TResult, TImpl> GetEnumerator()
    {
        return this;
    }

    public TResult Current { get; private set; }

    public bool MoveNext()
    {
        var val = TImpl.TryReadNextValue(ref _input, _separator, _formatProvider, out TResult? current);
        Current = current;
        return val;
    }

    public TResult[] ToArray()
    {
        ref var begin = ref Unsafe.As<char, ushort>(ref MemoryMarshal.GetReference(_input));
        // non-ASCII separator is not supported(surrogate pair may break the logic)
        var count = SpanHelpers.CountValueType(ref begin, _separator, _input.Length) + 1;
        var array = new TResult[count];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = ReadNextValue();
        }
        return array;
    }
}

// Source: [System.Private.CoreLib]System.SpanHelpers
internal static class SpanHelpers
{
    public static int CountValueType<T>(ref T current, T value, int length) where T : struct, IEquatable<T>?
    {
        var count = 0;
        ref T end = ref Unsafe.Add(ref current, length);

        if (Vector128.IsHardwareAccelerated && length >= Vector128<T>.Count)
        {
            // Vector512 is not supported on .NET 7.0 so Vector512 code is not included.
            if (Vector256.IsHardwareAccelerated && length >= Vector256<T>.Count)
            {
                var targetVector = Vector256.Create(value);
                ref T oneVectorAwayFromEnd = ref Unsafe.Subtract(ref end, Vector256<T>.Count);
                do
                {
                    count += BitOperations.PopCount(Vector256.Equals(Vector256.LoadUnsafe(ref current), targetVector).ExtractMostSignificantBits());
                    current = ref Unsafe.Add(ref current, Vector256<T>.Count);
                }
                while (!Unsafe.IsAddressGreaterThan(ref current, ref oneVectorAwayFromEnd));

                // If there are just a few elements remaining, then processing these elements by the scalar loop
                // is cheaper than doing bitmask + popcount on the full last vector. To avoid complicated type
                // based checks, other remainder-count based logic to determine the correct cut-off, for simplicity
                // a half-vector size is chosen (based on benchmarks).
                var remaining = (uint)Unsafe.ByteOffset(ref current, ref end) / (uint)Unsafe.SizeOf<T>();
                if (remaining > Vector256<T>.Count / 2)
                {
                    var mask = Vector256.Equals(Vector256.LoadUnsafe(ref oneVectorAwayFromEnd), targetVector).ExtractMostSignificantBits();

                    // The mask contains some elements that may be double-checked, so shift them away in order to get the correct pop-count.
                    var overlaps = (uint)Vector256<T>.Count - remaining;
                    mask >>= (int)overlaps;
                    count += BitOperations.PopCount(mask);

                    return count;
                }
            }
            else
            {
                var targetVector = Vector128.Create(value);
                ref T oneVectorAwayFromEnd = ref Unsafe.Subtract(ref end, Vector128<T>.Count);
                do
                {
                    count += BitOperations.PopCount(Vector128.Equals(Vector128.LoadUnsafe(ref current), targetVector).ExtractMostSignificantBits());
                    current = ref Unsafe.Add(ref current, Vector128<T>.Count);
                }
                while (!Unsafe.IsAddressGreaterThan(ref current, ref oneVectorAwayFromEnd));

                var remaining = (uint)Unsafe.ByteOffset(ref current, ref end) / (uint)Unsafe.SizeOf<T>();
                if (remaining > Vector128<T>.Count / 2)
                {
                    var mask = Vector128.Equals(Vector128.LoadUnsafe(ref oneVectorAwayFromEnd), targetVector).ExtractMostSignificantBits();

                    // The mask contains some elements that may be double-checked, so shift them away in order to get the correct pop-count.
                    var overlaps = (uint)Vector128<T>.Count - remaining;
                    mask >>= (int)overlaps;
                    count += BitOperations.PopCount(mask);

                    return count;
                }
            }
        }

        while (Unsafe.IsAddressLessThan(ref current, ref end))
        {
            if (current.Equals(value))
            {
                count++;
            }

            current = ref Unsafe.Add(ref current, 1);
        }

        return count;
    }
}

#endregion