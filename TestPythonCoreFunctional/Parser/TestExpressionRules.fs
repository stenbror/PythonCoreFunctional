
namespace PythonCore.Tests

open PythonCore.Runtime.Parser
open Xunit

module TestParserExpressionRules =

    [<Fact>]
    let ``Test Atom rule with name literal``() = 
        let mutable state = ParseState.Init
        let stream = "__init__".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseAtom(stream, &state)
        Assert.Equal([| Token.Eof(8ul, [| |]) |], rest)
        Assert.Equal(Node.Name(0ul, 8ul, Token.Name(0ul, 8ul, [| |], "__init__")), node)

    [<Fact>]
    let ``Test Atom rule with number literal``() = 
        let mutable state = ParseState.Init
        let stream = "0.34J".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseAtom(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal(Node.Number(0ul, 5ul, Token.Number(0ul, 5ul, [| |], "0.34J")), node)

