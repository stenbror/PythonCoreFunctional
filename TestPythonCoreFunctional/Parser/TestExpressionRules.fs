
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






    [<Fact>]
    let ``Test power operator with only left side``() = 
        let mutable state = ParseState.Init
        let stream = "True".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParsePower(stream, &state)
        Assert.Equal([| Token.Eof(4ul, [| |]) |], rest)
        Assert.Equal(Node.True(0ul, 4ul, Token.True(0ul, 4ul, [| |] )), node)

    [<Fact>]
    let ``Test power operator``() = 
        let mutable state = ParseState.Init
        let stream = "3 ** 5".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParsePower(stream, &state)
        Assert.Equal([| Token.Eof(6ul, [| |]) |], rest)
        Assert.Equal(Node.Power(0ul, 6ul, 
                            Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [||], "3")),
                            Token.Power(2ul, 4ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                            Node.Number(5ul, 6ul, Token.Number(5ul, 6ul, [| Trivia.WhiteSpace(4ul, 5ul) |], "5" ))
                        ), node)

    [<Fact>]
    let ``Test factor rule without operators``() = 
        let mutable state = ParseState.Init
        let stream = "True".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseFactor(stream, &state)
        Assert.Equal([| Token.Eof(4ul, [| |]) |], rest)
        Assert.Equal(Node.True(0ul, 4ul, Token.True(0ul, 4ul, [| |] )), node)

    [<Fact>]
    let ``Test Factor operator, single plus operator``() = 
        let mutable state = ParseState.Init
        let stream = "+a".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseFactor(stream, &state)
        Assert.Equal([| Token.Eof(2ul, [| |]) |], rest)
        Assert.Equal( Node.UnaryPlus(0ul, 2ul, Token.Plus(0ul, 1ul, [| |]), Node.Name(1ul, 2ul, Token.Name(1ul, 2ul, [| |], "a"))), node)

    [<Fact>]
    let ``Test Factor operator, multiple plus operator``() = 
        let mutable state = ParseState.Init
        let stream = "+ + a".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseFactor(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal( Node.UnaryPlus(0ul, 5ul, Token.Plus(0ul, 1ul, [| |]), 
                            Node.UnaryPlus(2ul, 5ul, Token.Plus(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ), 
                                Node.Name(4ul, 5ul, Token.Name(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "a") ))
                        ), node)

    [<Fact>]
    let ``Test Factor operator, single minus operator``() = 
        let mutable state = ParseState.Init
        let stream = "-a".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseFactor(stream, &state)
        Assert.Equal([| Token.Eof(2ul, [| |]) |], rest)
        Assert.Equal( Node.UnaryMinus(0ul, 2ul, Token.Minus(0ul, 1ul, [| |]), Node.Name(1ul, 2ul, Token.Name(1ul, 2ul, [| |], "a"))), node)

    [<Fact>]
    let ``Test Factor operator, multiple minus operator``() = 
        let mutable state = ParseState.Init
        let stream = "- - a".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseFactor(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal( Node.UnaryMinus(0ul, 5ul, Token.Minus(0ul, 1ul, [| |]), 
                            Node.UnaryMinus(2ul, 5ul, Token.Minus(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ), 
                                Node.Name(4ul, 5ul, Token.Name(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "a") ))
                        ), node)

    [<Fact>]
    let ``Test Factor operator, single invert operator``() = 
        let mutable state = ParseState.Init
        let stream = "~a".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseFactor(stream, &state)
        Assert.Equal([| Token.Eof(2ul, [| |]) |], rest)
        Assert.Equal( Node.BitInvert(0ul, 2ul, Token.BitInvert(0ul, 1ul, [| |]), Node.Name(1ul, 2ul, Token.Name(1ul, 2ul, [| |], "a"))), node)

    [<Fact>]
    let ``Test Factor operator, multiple invert operator``() = 
        let mutable state = ParseState.Init
        let stream = "~ ~ a".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseFactor(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal( Node.BitInvert(0ul, 5ul, Token.BitInvert(0ul, 1ul, [| |]), 
                            Node.BitInvert(2ul, 5ul, Token.BitInvert(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ), 
                                Node.Name(4ul, 5ul, Token.Name(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "a") ))
                        ), node)

    [<Fact>]
    let ``Test term rule without operators``() = 
        let mutable state = ParseState.Init
        let stream = "True".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseTerm(stream, &state)
        Assert.Equal([| Token.Eof(4ul, [| |]) |], rest)
        Assert.Equal(Node.True(0ul, 4ul, Token.True(0ul, 4ul, [| |] )), node)

    [<Fact>]
    let ``Test term rule with single mul operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 * 5".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseTerm(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal(Node.Mul(0ul, 5ul, 
                            Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                            Token.Mul(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                            Node.Number(4ul, 5ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" ))
                        ), node)

    [<Fact>]
    let ``Test term rule with multiple mul operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 * 5 * 6".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseTerm(stream, &state)
        Assert.Equal([| Token.Eof(9ul, [| |]) |], rest)
        Assert.Equal(Node.Mul(0ul, 9ul,
                            Node.Mul(0ul, 6ul, 
                                Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                                Token.Mul(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                                Node.Number(4ul, 6ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" )) ),
                            Token.Mul(6ul, 7ul, [| Trivia.WhiteSpace(5ul, 6ul) |] ),
                            Node.Number(8ul, 9ul, Token.Number(8ul, 9ul, [| Trivia.WhiteSpace(7ul, 8ul) |], "6" ))
                        ), node)

    [<Fact>]
    let ``Test term rule with single div operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 / 5".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseTerm(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal(Node.Div(0ul, 5ul, 
                            Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                            Token.Div(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                            Node.Number(4ul, 5ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" ))
                        ), node)

    [<Fact>]
    let ``Test term rule with multiple div operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 / 5 / 6".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseTerm(stream, &state)
        Assert.Equal([| Token.Eof(9ul, [| |]) |], rest)
        Assert.Equal(Node.Div(0ul, 9ul,
                            Node.Div(0ul, 6ul, 
                                Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                                Token.Div(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                                Node.Number(4ul, 6ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" )) ),
                            Token.Div(6ul, 7ul, [| Trivia.WhiteSpace(5ul, 6ul) |] ),
                            Node.Number(8ul, 9ul, Token.Number(8ul, 9ul, [| Trivia.WhiteSpace(7ul, 8ul) |], "6" ))
                        ), node)

    [<Fact>]
    let ``Test term rule with single modulo operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 % 5".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseTerm(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal(Node.Modulo(0ul, 5ul, 
                            Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                            Token.Modulo(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                            Node.Number(4ul, 5ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" ))
                        ), node)

    [<Fact>]
    let ``Test term rule with multiple modulo operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 % 5 % 6".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseTerm(stream, &state)
        Assert.Equal([| Token.Eof(9ul, [| |]) |], rest)
        Assert.Equal(Node.Modulo(0ul, 9ul,
                            Node.Modulo(0ul, 6ul, 
                                Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                                Token.Modulo(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                                Node.Number(4ul, 6ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" )) ),
                            Token.Modulo(6ul, 7ul, [| Trivia.WhiteSpace(5ul, 6ul) |] ),
                            Node.Number(8ul, 9ul, Token.Number(8ul, 9ul, [| Trivia.WhiteSpace(7ul, 8ul) |], "6" ))
                        ), node)

    [<Fact>]
    let ``Test term rule with single matrice operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 @ 5".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseTerm(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal(Node.Matrice(0ul, 5ul, 
                            Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                            Token.Matrice(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                            Node.Number(4ul, 5ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" ))
                        ), node)

    [<Fact>]
    let ``Test term rule with multiple matrice operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 @ 5 @ 6".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseTerm(stream, &state)
        Assert.Equal([| Token.Eof(9ul, [| |]) |], rest)
        Assert.Equal(Node.Matrice(0ul, 9ul,
                            Node.Matrice(0ul, 6ul, 
                                Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                                Token.Matrice(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                                Node.Number(4ul, 6ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" )) ),
                            Token.Matrice(6ul, 7ul, [| Trivia.WhiteSpace(5ul, 6ul) |] ),
                            Node.Number(8ul, 9ul, Token.Number(8ul, 9ul, [| Trivia.WhiteSpace(7ul, 8ul) |], "6" ))
                        ), node)

    [<Fact>]
    let ``Test term rule with single floor div operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 // 5".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseTerm(stream, &state)
        Assert.Equal([| Token.Eof(6ul, [| |]) |], rest)
        Assert.Equal(Node.FloorDiv(0ul, 6ul, 
                            Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                            Token.FloorDiv(2ul, 4ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                            Node.Number(5ul, 6ul, Token.Number(5ul, 6ul, [| Trivia.WhiteSpace(4ul, 5ul) |], "5" ))
                        ), node)

    [<Fact>]
    let ``Test term rule with multiple floor div operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 // 5 // 6".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseTerm(stream, &state)
        Assert.Equal([| Token.Eof(11ul, [| |]) |], rest)
        Assert.Equal(Node.FloorDiv(0ul, 11ul,
                            Node.FloorDiv(0ul, 7ul, 
                                Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                                Token.FloorDiv(2ul, 4ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                                Node.Number(5ul, 7ul, Token.Number(5ul, 6ul, [| Trivia.WhiteSpace(4ul, 5ul) |], "5" )) ),
                            Token.FloorDiv(7ul, 9ul, [| Trivia.WhiteSpace(6ul, 7ul) |] ),
                            Node.Number(10ul, 11ul, Token.Number(10ul, 11ul, [| Trivia.WhiteSpace(9ul, 10ul) |], "6" ))
                        ), node)

    [<Fact>]
    let ``Test arith rule without operators``() = 
        let mutable state = ParseState.Init
        let stream = "True".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseArith(stream, &state)
        Assert.Equal([| Token.Eof(4ul, [| |]) |], rest)
        Assert.Equal(Node.True(0ul, 4ul, Token.True(0ul, 4ul, [| |] )), node)

    [<Fact>]
    let ``Test arith rule with single plus operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 + 5".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseArith(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal(Node.Plus(0ul, 5ul, 
                            Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                            Token.Plus(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                            Node.Number(4ul, 5ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" ))
                        ), node)

    [<Fact>]
    let ``Test arith rule with multiple plus operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 + 5 + 6".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseArith(stream, &state)
        Assert.Equal([| Token.Eof(9ul, [| |]) |], rest)
        Assert.Equal(Node.Plus(0ul, 9ul,
                            Node.Plus(0ul, 6ul, 
                                Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                                Token.Plus(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                                Node.Number(4ul, 6ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" )) ),
                            Token.Plus(6ul, 7ul, [| Trivia.WhiteSpace(5ul, 6ul) |] ),
                            Node.Number(8ul, 9ul, Token.Number(8ul, 9ul, [| Trivia.WhiteSpace(7ul, 8ul) |], "6" ))
                        ), node)

    [<Fact>]
    let ``Test arith rule with single minus operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 - 5".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseArith(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal(Node.Minus(0ul, 5ul, 
                            Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                            Token.Minus(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                            Node.Number(4ul, 5ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" ))
                        ), node)

    [<Fact>]
    let ``Test arith rule with multiple minus operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 - 5 - 6".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseArith(stream, &state)
        Assert.Equal([| Token.Eof(9ul, [| |]) |], rest)
        Assert.Equal(Node.Minus(0ul, 9ul,
                            Node.Minus(0ul, 6ul, 
                                Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                                Token.Minus(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                                Node.Number(4ul, 6ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" )) ),
                            Token.Minus(6ul, 7ul, [| Trivia.WhiteSpace(5ul, 6ul) |] ),
                            Node.Number(8ul, 9ul, Token.Number(8ul, 9ul, [| Trivia.WhiteSpace(7ul, 8ul) |], "6" ))
                        ), node)

    [<Fact>]
    let ``Test shift rule without operators``() = 
        let mutable state = ParseState.Init
        let stream = "True".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseShift(stream, &state)
        Assert.Equal([| Token.Eof(4ul, [| |]) |], rest)
        Assert.Equal(Node.True(0ul, 4ul, Token.True(0ul, 4ul, [| |] )), node)

    [<Fact>]
    let ``Test shift rule with single shift left operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 << 5".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseShift(stream, &state)
        Assert.Equal([| Token.Eof(6ul, [| |]) |], rest)
        Assert.Equal(Node.ShiftLeft(0ul, 6ul, 
                            Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                            Token.ShiftLeft(2ul, 4ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                            Node.Number(5ul, 6ul, Token.Number(5ul, 6ul, [| Trivia.WhiteSpace(4ul, 5ul) |], "5" ))
                        ), node)

    [<Fact>]
    let ``Test shift rule with multiple shift left operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 << 5 << 6".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseShift(stream, &state)
        Assert.Equal([| Token.Eof(11ul, [| |]) |], rest)
        Assert.Equal(Node.ShiftLeft(0ul, 11ul,
                            Node.ShiftLeft(0ul, 7ul, 
                                Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                                Token.ShiftLeft(2ul, 4ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                                Node.Number(5ul, 7ul, Token.Number(5ul, 6ul, [| Trivia.WhiteSpace(4ul, 5ul) |], "5" )) ),
                            Token.ShiftLeft(7ul, 9ul, [| Trivia.WhiteSpace(6ul, 7ul) |] ),
                            Node.Number(10ul, 11ul, Token.Number(10ul, 11ul, [| Trivia.WhiteSpace(9ul, 10ul) |], "6" ))
                        ), node)

    [<Fact>]
    let ``Test shift rule with single shift right operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 >> 5".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseShift(stream, &state)
        Assert.Equal([| Token.Eof(6ul, [| |]) |], rest)
        Assert.Equal(Node.ShiftRight(0ul, 6ul, 
                            Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                            Token.ShiftRight(2ul, 4ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                            Node.Number(5ul, 6ul, Token.Number(5ul, 6ul, [| Trivia.WhiteSpace(4ul, 5ul) |], "5" ))
                        ), node)

    [<Fact>]
    let ``Test shift rule with multiple shift right operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 >> 5 >> 6".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseShift(stream, &state)
        Assert.Equal([| Token.Eof(11ul, [| |]) |], rest)
        Assert.Equal(Node.ShiftRight(0ul, 11ul,
                            Node.ShiftRight(0ul, 7ul, 
                                Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                                Token.ShiftRight(2ul, 4ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                                Node.Number(5ul, 7ul, Token.Number(5ul, 6ul, [| Trivia.WhiteSpace(4ul, 5ul) |], "5" )) ),
                            Token.ShiftRight(7ul, 9ul, [| Trivia.WhiteSpace(6ul, 7ul) |] ),
                            Node.Number(10ul, 11ul, Token.Number(10ul, 11ul, [| Trivia.WhiteSpace(9ul, 10ul) |], "6" ))
                        ), node)

    [<Fact>]
    let ``Test and rule without operators``() = 
        let mutable state = ParseState.Init
        let stream = "True".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseAnd(stream, &state)
        Assert.Equal([| Token.Eof(4ul, [| |]) |], rest)
        Assert.Equal(Node.True(0ul, 4ul, Token.True(0ul, 4ul, [| |] )), node)

    [<Fact>]
    let ``Test and rule with single & operator``() = 
        let mutable state = ParseState.Init
        let stream = "4 & 5".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseAnd(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal(Node.AndExpr(0ul, 5ul, 
                            Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                            Token.BitAnd(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                            Node.Number(4ul, 5ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" ))
                        ), node)

    [<Fact>]
    let ``Test and rule with multiple & operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 & 5 & 6".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseAnd(stream, &state)
        Assert.Equal([| Token.Eof(9ul, [| |]) |], rest)
        Assert.Equal(Node.AndExpr(0ul, 9ul,
                            Node.AndExpr(0ul, 6ul, 
                                Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                                Token.BitAnd(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                                Node.Number(4ul, 6ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" )) ),
                            Token.BitAnd(6ul, 7ul, [| Trivia.WhiteSpace(5ul, 6ul) |] ),
                            Node.Number(8ul, 9ul, Token.Number(8ul, 9ul, [| Trivia.WhiteSpace(7ul, 8ul) |], "6" ))
                        ), node)




    [<Fact>]
    let ``Test xor rule without operators``() = 
        let mutable state = ParseState.Init
        let stream = "True".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseXor(stream, &state)
        Assert.Equal([| Token.Eof(4ul, [| |]) |], rest)
        Assert.Equal(Node.True(0ul, 4ul, Token.True(0ul, 4ul, [| |] )), node)

    [<Fact>]
    let ``Test xor rule with single ^ operator``() = 
        let mutable state = ParseState.Init
        let stream = "4 ^ 5".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseXor(stream, &state)
        Assert.Equal([| Token.Eof(5ul, [| |]) |], rest)
        Assert.Equal(Node.XorExpr(0ul, 5ul, 
                            Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                            Token.BitXor(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                            Node.Number(4ul, 5ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" ))
                        ), node)

    [<Fact>]
    let ``Test xor rule with multiple ^ operators``() = 
        let mutable state = ParseState.Init
        let stream = "4 ^ 5 ^ 6".ToCharArray() |> Tokenizer.TokenizeFromCharArray
        let node, rest = Expressions.ParseXor(stream, &state)
        Assert.Equal([| Token.Eof(9ul, [| |]) |], rest)
        Assert.Equal(Node.XorExpr(0ul, 9ul,
                            Node.XorExpr(0ul, 6ul, 
                                Node.Number(0ul, 2ul, Token.Number(0ul, 1ul, [| |], "4" )),
                                Token.BitXor(2ul, 3ul, [| Trivia.WhiteSpace(1ul, 2ul) |] ),
                                Node.Number(4ul, 6ul, Token.Number(4ul, 5ul, [| Trivia.WhiteSpace(3ul, 4ul) |], "5" )) ),
                            Token.BitXor(6ul, 7ul, [| Trivia.WhiteSpace(5ul, 6ul) |] ),
                            Node.Number(8ul, 9ul, Token.Number(8ul, 9ul, [| Trivia.WhiteSpace(7ul, 8ul) |], "6" ))
                        ), node)