using static System.Console;
using System.CodeDom.Compiler;

namespace InputUtil.Old;

//
// this code is just for performance comparison
//
#nullable disable

/// <summary>
/// 標準入力の高機能な読み取りを提供します。
/// </summary>
public static class MyInputUtil
{
    private const char separator = ' ';

    /// <summary>
    /// 標準入力から入力を1つ指定の型に変換して読み取ります。
    /// </summary>
    /// <typeparam name="T">変換先として指定する型。</typeparam>
    /// <param name="converter">変換に使用する関数。<c>null</c>の場合は自動決定を試みます。</param>
    /// <returns></returns>
    public static T ReadInput<T>(Func<string, T> converter = null)
    {
        if (converter is null)
        {
            converter = GetConverter<T>();
        }
        return converter.Invoke(ReadLine());
    }

    #region 一定個数の入力の読み取り
    // 用法は ReadInput<T> と共通です。
    [GeneratedCode(null, "2.1")]
    public static ValueTuple<T, T> ReadInput2<T>(Func<string, T> converter = null)
    {
        if (converter is null) { converter = GetConverter<T>(); }
        var input = Console.ReadLine().Split(separator);
        return (converter.Invoke(input[0]), converter.Invoke(input[1]));
    }
    [GeneratedCode(null, "2.1")]
    public static ValueTuple<T, T, T> ReadInput3<T>(Func<string, T> converter = null)
    {
        if (converter is null) { converter = GetConverter<T>(); }
        var input = Console.ReadLine().Split(separator);
        return (converter.Invoke(input[0]), converter.Invoke(input[1]), converter.Invoke(input[2]));
    }
    [GeneratedCode(null, "2.1")]
    public static ValueTuple<T, T, T, T> ReadInput4<T>(Func<string, T> converter = null)
    {
        if (converter is null) { converter = GetConverter<T>(); }
        var input = Console.ReadLine().Split(separator);
        return (converter.Invoke(input[0]), converter.Invoke(input[1]), converter.Invoke(input[2]), converter.Invoke(input[3]));
    }
    [GeneratedCode(null, "2.1")]
    public static ValueTuple<T, T, T, T, T> ReadInput5<T>(Func<string, T> converter = null)
    {
        if (converter is null) { converter = GetConverter<T>(); }
        var input = Console.ReadLine().Split(separator);
        return (converter.Invoke(input[0]), converter.Invoke(input[1]), converter.Invoke(input[2]), converter.Invoke(input[3]), converter.Invoke(input[4]));
    }
    [GeneratedCode(null, "2.1")]
    public static ValueTuple<T, T, T, T, T, T> ReadInput6<T>(Func<string, T> converter = null)
    {
        if (converter is null) { converter = GetConverter<T>(); }
        var input = Console.ReadLine().Split(separator);
        return (converter.Invoke(input[0]), converter.Invoke(input[1]), converter.Invoke(input[2]), converter.Invoke(input[3]), converter.Invoke(input[4]), converter.Invoke(input[5]));
    }
    [GeneratedCode(null, "2.1")]
    public static ValueTuple<T, T, T, T, T, T, T> ReadInput7<T>(Func<string, T> converter = null)
    {
        if (converter is null) { converter = GetConverter<T>(); }
        var input = Console.ReadLine().Split(separator);
        return (converter.Invoke(input[0]), converter.Invoke(input[1]), converter.Invoke(input[2]), converter.Invoke(input[3]), converter.Invoke(input[4]), converter.Invoke(input[5]), converter.Invoke(input[6]));
    }
    #endregion

    /// <summary>
    /// 標準入力から配列を読み取ります。
    /// </summary>
    /// <returns></returns>
    public static T[] ReadArray<T>(Func<string, T> converter = null)
    {
        var str = ReadLine().Split(separator);
        if (converter is null)
        {
            converter = GetConverter<T>();
        }
        return str.Select(s => converter.Invoke(s)).ToArray();
    }

    #region 変換用メソッド
    /// <summary>
    /// 文字列を指定の型に変換します。
    /// </summary>
    /// <typeparam name="T">変換先の型。すべての組み込み型がサポートされます。</typeparam>
    /// <param name="str">変換元の文字列</param>
    /// <exception cref="NotSupportedException">指定の型引数がサポートされない場合にスローされる例外。</exception>
    /// <remarks>大量の値を処理する場合、このメソッドは低速です。<see cref="ConvertTo{T}(string[])"/> の使用を検討してください。</remarks>
    public static T ConvertTo<T>(string str)
    {
        return GetConverter<T>().Invoke(str);
    }

    /// <summary>
    /// 文字列配列を指定の型の配列に変換します。
    /// </summary>
    /// <typeparam name="T">変換先の型。すべての組み込み型がサポートされます。</typeparam>
    /// <param name="str">変換元の文字列配列</param>
    /// <exception cref="NotSupportedException">指定の型引数がサポートされない場合にスローされる例外。</exception>
    public static T[] ConvertTo<T>(string[] str)
    {
        var conveter = GetConverter<T>();
        var array = new T[str.Length];
        for (var i = 0; i < str.Length; i++)
        {
            array[i] = conveter.Invoke(str[i]);
        }
        return array;
    }

    private static Func<string, T> GetConverter<T>() => GetConverterOrDefault<T>() ?? throw new NotSupportedException("この型に対する変換関数の取得はサポートされていません");
    private static bool TryGetConverter<T>(out Func<string, T> func)
    {
        func = GetConverterOrDefault<T>();
        return !(func is null);
    }

    private static Func<string, T> GetConverterOrDefault<T>()
    {
        Type type = typeof(T);
        Func<string, T> conveter;

        // 出現頻度が高そうなものを上
        if (type == typeof(int))
        {
            conveter = str => (T)(object)int.Parse(str);
        }
        else if (type == typeof(long))
        {
            conveter = str => (T)(object)long.Parse(str);
        }
        else if (type == typeof(double))
        {
            conveter = str => (T)(object)double.Parse(str);
        }
        else if (type == typeof(decimal))
        {
            conveter = str => (T)(object)decimal.Parse(str);
        }
        else if (type == typeof(float))
        {
            conveter = str => (T)(object)float.Parse(str);
        }
        else if (type == typeof(char))
        {
            conveter = str => (T)(object)str[0];
        }
        else if (type == typeof(short))
        {
            conveter = str => (T)(object)short.Parse(str);
        }
        else if (type == typeof(byte))
        {
            conveter = str => (T)(object)byte.Parse(str);
        }
        else if (type == typeof(bool))
        {
            conveter = str => (T)(object)bool.Parse(str);
        }
        else if (type == typeof(ulong))
        {
            conveter = str => (T)(object)ulong.Parse(str);
        }
        else if (type == typeof(uint))
        {
            conveter = str => (T)(object)uint.Parse(str);
        }
        else if (type == typeof(ushort))
        {
            conveter = str => (T)(object)ushort.Parse(str);
        }
        else if (type == typeof(sbyte))
        {
            conveter = str => (T)(object)sbyte.Parse(str);
        }
        else if (type == typeof(string))
        {
            conveter = str => (T)(object)str;
        }
        else if (type == typeof(object))
        {
            conveter = str => (T)(object)str;
        }
        else
        {
            conveter = null;
        }

        return conveter;
    }
    #endregion
}

#nullable restore