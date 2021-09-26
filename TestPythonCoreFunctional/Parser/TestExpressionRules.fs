
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

    [<Fact>]
    let ``Test Atom rule with None literal``() = 
        let mutable state = ParseState.Init
        let stream = "None".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseAtom(stream, &state)
        Assert.Equal([| Token.Eof(4ul, [| |]) |], rest)
        Assert.Equal(Node.None(0ul, 4ul, Token.None(0ul, 4ul, [| |] )), node)

    [<Fact>]
    let ``Test Atom rule with True literal``() = 
        let mutable state = ParseState.Init
        let stream = "True".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseAtom(stream, &state)
        Assert.Equal([| Token.Eof(4ul, [| |]) |], rest)
        Assert.Equal(Node.True(0ul, 4ul, Token.True(0ul, 4ul, [| |] )), node)

    [<Fact>]
    let ``Test Atom rule with False literal``() = 
        let mutable state = ParseState.Init
        let stream = "False".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseAtom(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal(Node.False(0ul, 5ul, Token.False(0ul, 5ul, [| |] )), node)

    [<Fact>]
    let ``Test Atom rule with Elipsis literal``() = 
        let mutable state = ParseState.Init
        let stream = "...".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseAtom(stream, &state)
        Assert.Equal([| Token.Eof(3ul, [| |]) |], rest)
        Assert.Equal(Node.Elipsis(0ul, 3ul, Token.Elipsis(0ul, 3ul, [| |] )), node)

    [<Fact>]
    let ``Test Atom rule with single string literal``() = 
        let mutable state = ParseState.Init
        let stream = "'Hello, World!'".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseAtom(stream, &state)
        Assert.Equal([| Token.Eof(15ul, [| |]) |], rest)
        Assert.Equal(Node.String(0ul, 15ul, [| Token.String(0ul, 15ul, [| |], "'Hello, World!'" ) |] ), node)

    [<Fact>]
    let ``Test Atom rule with multiple string literal``() = 
        let mutable state = ParseState.Init
        let stream = "'Hello, World!' '.txt'".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseAtom(stream, &state)
        Assert.Equal([| Token.Eof(22ul, [| |]) |], rest)
        Assert.Equal(Node.String(0ul, 22ul, 
                        [| 
                            Token.String(0ul, 15ul, [| |], "'Hello, World!'" ) 
                            Token.String(16ul, 22ul, [| Trivia.WhiteSpace(15ul, 16ul) |], "'.txt'" ) 
                        |] ), node)