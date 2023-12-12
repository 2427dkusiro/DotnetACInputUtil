using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace InputUtil.Simple;
public static class InputUtil
{
    public static InputToken<T> ReadLine<T>() where T : ISpanParsable<T>
    {
        return new InputToken<T>(Console.ReadLine().AsSpan());
    }
}

public ref struct InputToken<TResult> where TResult : ISpanParsable<TResult>
{
    private readonly IFormatProvider _formatProvider;

    public ReadOnlySpan<char> _input;

    const char separator = ' ';

#pragma warning disable CS8618 // Property Current is initialized in MoveNext
    public InputToken(ReadOnlySpan<char> input)
    {
        _formatProvider = System.Globalization.CultureInfo.InvariantCulture;
        _input = input;
    }

    public InputToken(IFormatProvider formatProvider, ReadOnlySpan<char> input)
    {
        _formatProvider = formatProvider;
        _input = input;
    }
#pragma warning restore CS8618

    public static implicit operator TResult(InputToken<TResult> token)
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
        var index = _input.IndexOf(separator);
        var target = index == -1 ? _input : _input[..index];
        if (target.Length == 0)
        {
            ThrowSeparatorNotFound();
        }
        var value = TResult.Parse(target, _formatProvider);
        _input = index != -1 ? _input[(index + 1)..] : Array.Empty<char>();
        return value;
    }

    private static void ThrowSeparatorNotFound()
    {
        throw new ArgumentException($"Separator not found");
    }

    public readonly InputToken<TResult> GetEnumerator()
    {
        return this;
    }

    public TResult Current { get; private set; }

    public bool MoveNext()
    {
        var index = _input.IndexOf(separator);
        var target = index == -1 ? _input : _input[..index];
        if (target.Length == 0)
        {
            return false;
        }
        Current = TResult.Parse(target, _formatProvider);
        _input = index != -1 ? _input[(index + 1)..] : Array.Empty<char>();
        return true;
    }

    public TResult[] ToArray()
    {
        ref ushort begin = ref Unsafe.As<char, ushort>(ref MemoryMarshal.GetReference(_input));
        var count = SpanHelpers.CountValueType(ref begin, separator, _input.Length) + 1;
        var array = new TResult[count];
        for (int i = 0; i < array.Length; i++)
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
        int count = 0;
        ref T end = ref Unsafe.Add(ref current, length);

        if (Vector128.IsHardwareAccelerated && length >= Vector128<T>.Count)
        {
            // Vector512 is not supported on .NET 7.0 so Vector512 code is not included.
            if (Vector256.IsHardwareAccelerated && length >= Vector256<T>.Count)
            {
                Vector256<T> targetVector = Vector256.Create(value);
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
                uint remaining = (uint)Unsafe.ByteOffset(ref current, ref end) / (uint)Unsafe.SizeOf<T>();
                if (remaining > Vector256<T>.Count / 2)
                {
                    uint mask = Vector256.Equals(Vector256.LoadUnsafe(ref oneVectorAwayFromEnd), targetVector).ExtractMostSignificantBits();

                    // The mask contains some elements that may be double-checked, so shift them away in order to get the correct pop-count.
                    uint overlaps = (uint)Vector256<T>.Count - remaining;
                    mask >>= (int)overlaps;
                    count += BitOperations.PopCount(mask);

                    return count;
                }
            }
            else
            {
                Vector128<T> targetVector = Vector128.Create(value);
                ref T oneVectorAwayFromEnd = ref Unsafe.Subtract(ref end, Vector128<T>.Count);
                do
                {
                    count += BitOperations.PopCount(Vector128.Equals(Vector128.LoadUnsafe(ref current), targetVector).ExtractMostSignificantBits());
                    current = ref Unsafe.Add(ref current, Vector128<T>.Count);
                }
                while (!Unsafe.IsAddressGreaterThan(ref current, ref oneVectorAwayFromEnd));

                uint remaining = (uint)Unsafe.ByteOffset(ref current, ref end) / (uint)Unsafe.SizeOf<T>();
                if (remaining > Vector128<T>.Count / 2)
                {
                    uint mask = Vector128.Equals(Vector128.LoadUnsafe(ref oneVectorAwayFromEnd), targetVector).ExtractMostSignificantBits();

                    // The mask contains some elements that may be double-checked, so shift them away in order to get the correct pop-count.
                    uint overlaps = (uint)Vector128<T>.Count - remaining;
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
