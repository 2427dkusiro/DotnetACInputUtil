# DotnetACInputUtil

atcoder などのオンラインジャッジ用のC#入力ライブラリ

## 概要

(デフォルトで)半角スペース区切りの形式で標準入力が渡される場合に、簡単に入力を受け取るためのユーティリティです。  
具体的に、以下のような入力形式に対応します。  

```plaintext
1 2 3
```

ライブラリの使用例を示します。

```cs
double x = ReadLine<double>();
var (m, n) = ReadLine<int>();
var (a, b, c, d, e) = ReadLine<int>();

string str = ReadLine();
var (s, t, u) = ReadLine();
var strs = ReadLine().ToArray();

foreach (var item in ReadLine<int>())
{
    // do something
}
var array = ReadLine<decimal>().ToArray();
var max = ReadLine<long>().Select(x => x * 2).Where(x => x != 0).Max();
```

使用例の全文は [使用例コード](./AtCoderTemplateNet7/Program.cs) を参照してください。

## 導入方法

オンラインジャッジで使用するには、ライブラリ部のコードをコピペします。  
[テスト済みの最新コード](./InputUtil/Current/InputUtil.cs) をコピペするのが安全です。  
コピペする場合、必要に応じて名前空間の宣言は変更するか削除してください。  
また、`InputUtil.` の記述が煩わしい場合、`using static` を追加します。

## 使い方

`InputUtil.ReadLine()` メソッドがほぼすべての入力ケースに対応します。  
使用例では `using static` を通したと仮定して、`InputUtil.` を省略します。

### 1入力の場合

```cs
int x = ReadLine<int>();
```

と、欲しい型をジェネリック型引数として指定します。  
変数は明示的な型で指定する必要があります。( `var` は使用できません。 )

```cs
string s = ReadLine();
```

`string` が欲しい場合は、ジェネリック型引数`<>` は指定しません。

### 複数入力(定数個)の場合

```cs
var (m, n) = ReadLine<int>();
var (a, b, c, d, e) = ReadLine<int>();
```

入力の個数がわかっている場合は、`var` の後に括弧でくくって入力の数だけ変数名を書きます。  
名前は好きな名前をつけることができるため、問題文と同じか、あるいは自分がわかりやすい名前がおすすめです。

### 個数がわからない場合

```cs
foreach (var item in ReadLine<int>())
{
    // do something
}
```

個数がわからないため、ループで処理する場合は `foreach` ループの処理対象として直接このライブラリの `ReadLine` を使うのがおすすめです。  
入力個数 `N` が前の行で与えられる場合もありますが、このライブラリでは不要なので適当な変数に読み出して捨てます。

### 配列、LINQ

```cs
var array = ReadLine<decimal>().ToArray();
```

入力を配列として欲しい場合は、`ToArray` メソッドを使用すると便利です。

```cs
var max = ReadLine<long>().Select(x => x * 2).Where(x => x != 0).Max();
```

LINQ で処理したい場合は、単に `ReadLine` の後にクエリを続けることができます。

## 実装

内部実装を解説します。  
`ReadLine` メソッドは `InputToken` 構造体を返します。この構造体は1入力の場合に暗黙的キャスト、複数入力の場合に分解パターン、可変入力の場合に `GetEnumerator` と異なる言語仕様・構文で対応するため、見かけ上統一的な構文で扱えます。  

`ReadLine` メソッドの型引数は `ISpanParsable` インターフェイスを実装する必要があります。`string` はそれを実装しないため、非ジェネリックのオーバーロードで対応しています。  

## テスト

このプロジェクトには単体テスト/CIパイプラインが存在します。これらはジャッジ環境と合わせるため、.NET7 に固定されています。  
コードの変更は単体テストに合格する必要があり、また新機能を実装する場合その単体テストも実装することが必要です。

## ライセンス

このライブラリでは一部、MITライセンスで提供されている dotnet 標準ライブラリのコードを使用しています。  
本ライブラリ自体は MITライセンスです。