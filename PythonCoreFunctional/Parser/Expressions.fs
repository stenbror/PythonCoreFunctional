
namespace PythonCore.Runtime.Parser

module Expressions =

        let TryToken (stream: TokenStream) =
                match stream with
                |   symbol :: rest ->   Some( symbol, rest )
                |   [] ->   Option.None

        let GetStartPosition (stream: TokenStream) : uint32 =
                match stream with
                |   symbol :: rest ->
                        match symbol with
                        |   Token.Empty ->  0ul
                        |   Token.Newline(a, _ , _ , _ , _ ) -> a
                        |   Token.Indent -> 0u
                        |   Token.Dedent ->  0u
                        |   Token.Eof(a, _) -> a
                        |   Token.Name(a, _ , _ , _) -> a
                        |   Token.Number(a, _ , _ , _) -> a
                        |   Token.String(a, _, _ , _) -> a
                        |   Token.TypeComment(a, _ , _ , _)   ->  a
                        |   Token.False(a, _ , _) -> a 
                        |   Token.True(a, _ , _) -> a
                        |   Token.None(a, _ , _) -> a  
                        |   Token.And(a, _ , _) -> a
                        |   Token.As(a, _ , _) -> a  
                        |   Token.Assert(a, _ , _) -> a
                        |   Token.Async(a, _ , _) -> a
                        |   Token.Await(a, _ , _) -> a
                        |   Token.Break(a, _ , _) -> a
                        |   Token.Class(a, _ , _) -> a
                        |   Token.Continue(a, _ , _) -> a
                        |   Token.Def(a, _ , _) -> a
                        |   Token.Del(a, _ , _) -> a
                        |   Token.Elif(a, _ , _) -> a
                        |   Token.Else(a, _ , _) -> a
                        |   Token.Except(a, _ , _) -> a
                        |   Token.Finally(a, _ , _) -> a
                        |   Token.For(a, _ , _) -> a
                        |   Token.From(a, _ , _) -> a
                        |   Token.Global(a, _ , _) -> a
                        |   Token.If(a, _ , _) -> a
                        |   Token.Import(a, _ , _) -> a
                        |   Token.In(a, _ , _) -> a
                        |   Token.Is(a, _ , _) -> a
                        |   Token.Lambda(a, _ , _) -> a
                        |   Token.NonLocal(a, _ , _) -> a
                        |   Token.Not(a, _ , _) -> a
                        |   Token.Or(a, _ , _) -> a
                        |   Token.Pass(a, _ , _) -> a
                        |   Token.Raise(a, _ , _) -> a
                        |   Token.Return(a, _ , _) -> a
                        |   Token.Try(a, _ , _) -> a
                        |   Token.While(a, _ , _) -> a
                        |   Token.With(a, _ , _) -> a
                        |   Token.Yield(a, _ , _) -> a
                        |   Token.Plus(a, _ , _) -> a
                        |   Token.Minus(a, _ , _) -> a
                        |   Token.Mul(a, _ , _) -> a
                        |   Token.Power(a, _ , _) -> a
                        |   Token.Div(a, _ , _) -> a
                        |   Token.FloorDiv(a, _ , _) -> a
                        |   Token.Modulo(a, _ , _) -> a
                        |   Token.Matrice(a, _ , _) -> a
                        |   Token.ShiftLeft(a, _ , _) -> a
                        |   Token.ShiftRight(a, _ , _) -> a
                        |   Token.BitAnd(a, _ , _) -> a
                        |   Token.BitOr(a, _ , _) -> a
                        |   Token.BitXor(a, _ , _) -> a
                        |   Token.BitInvert(a, _ , _) -> a
                        |   Token.Less(a, _ , _) -> a
                        |   Token.Greater(a, _ , _) -> a
                        |   Token.LessEqual(a, _ , _) -> a
                        |   Token.GreaterEqual(a, _ , _) -> a
                        |   Token.Equal(a, _ , _) -> a
                        |   Token.NotEqual(a, _ , _) -> a
                        |   Token.LeftParen(a, _ , _) -> a
                        |   Token.RightParen(a, _ , _) -> a
                        |   Token.LeftBracket(a, _ , _) -> a
                        |   Token.RightBracket(a, _ , _) -> a
                        |   Token.LeftCurly(a, _ , _) -> a
                        |   Token.RightCurly(a, _ , _) -> a
                        |   Token.Comma(a, _ , _) -> a
                        |   Token.Colon(a, _ , _) -> a
                        |   Token.ColonAssign(a, _ , _) -> a
                        |   Token.Dot(a, _ , _) -> a
                        |   Token.Elipsis(a, _ , _) -> a
                        |   Token.SemiColon(a, _ , _) -> a   
                        |   Token.Assign(a, _ , _) -> a
                        |   Token.Arrow(a, _ , _) -> a
                        |   Token.PlusAssign(a, _ , _) -> a
                        |   Token.MinusAssign(a, _ , _) -> a
                        |   Token.MulAssign(a, _ , _) -> a
                        |   Token.PowerAssign(a, _ , _) -> a
                        |   Token.DivAssign(a, _ , _) -> a
                        |   Token.FloorDivAssign(a, _ , _) -> a
                        |   Token.ModuloAssign(a, _ , _) -> a
                        |   Token.BitAndAssign(a, _ , _) -> a
                        |   Token.BitOrAssign(a, _ , _) -> a
                        |   Token.BitXorAssign(a, _ , _) -> a
                        |   Token.MatriceAssign(a, _ , _) -> a
                        |   Token.ShiftLeftAssign(a, _ , _) -> a
                        |   Token.ShiftRightAssign(a, _ , _) -> a
                        |   Token.Match(a, _ , _) -> a
                        |   Token.Case(a, _ , _) -> a
                        |   Token.MatchAllPattern(a, _ , _) -> a        
                |   _ ->    0u


        let rec ParseAtom (stream: TokenStream, state: byref<ParseState>) : (Node * TokenStream) =
                let spanStart = GetStartPosition stream
                match TryToken stream with
                |   Some(Token.Name(_ , _ , _ , _), rest)  ->
                        let op = List.head stream
                        Node.Name(spanStart, GetStartPosition rest, op), rest
                |   Some(Token.Number(_ , _ , _ , _), rest)  ->
                        let op = List.head stream
                        Node.Number(spanStart, GetStartPosition rest, op), rest
                |   Some(Token.String(_ , _ , _ , _), rest)    ->
                        let mutable restAgain = rest
                        let mutable nodes : Token list = []
                        nodes <- List.head stream :: nodes
                        while   match TryToken restAgain with
                                |    Some(Token.String(_ , _ , _ , _), restNow) ->
                                        nodes <- List.head restAgain :: nodes
                                        restAgain <- restNow
                                        true
                                |   _ ->    false
                                do ()
                        Node.String(spanStart, GetStartPosition restAgain, List.toArray(List.rev nodes)), restAgain
                |   Some(Token.None(_ , _ , _), rest)   ->
                        let op = List.head stream
                        Node.None(spanStart, GetStartPosition rest, op), rest
                |   Some(Token.True(_ , _ , _), rest)   ->
                        let op = List.head stream
                        Node.True(spanStart, GetStartPosition rest, op), rest
                |   Some(Token.False(_ , _ , _), rest)   ->
                        let op = List.head stream
                        Node.False(spanStart, GetStartPosition rest, op), rest
                |   Some(Token.Elipsis(_ , _ , _), rest)   ->
                        let op = List.head stream
                        Node.Elipsis(spanStart, GetStartPosition rest, op), rest
                |   _ -> 
                        raise (SyntaxError(List.head stream, "Illegal literal!", spanStart))

        and ParseAtomExpr (stream: TokenStream, state: byref<ParseState>) : (Node * TokenStream) =
                let spanStart = GetStartPosition stream
                let op, rest =  match TryToken stream with
                                |       Some(Token.Await(_ , _ , _), rest) ->
                                                let op = List.head stream
                                                op, rest
                                |       _ ->
                                                Token.Empty, stream
                let left, rest2 = ParseAtom (rest, &state)
                match TryToken rest2 with
                |       _ ->
                                left, rest2

        and ParsePower (stream: TokenStream, state: byref<ParseState>) : (Node * TokenStream) =
                let spanStart = GetStartPosition stream
                let left, rest = ParseAtomExpr (stream, &state)
                match TryToken rest with
                |       Some(Token.Power(_ , _ , _), rest2) ->
                                let op = List.head rest
                                let right, rest3 = ParseFactor(rest2, &state)
                                Node.Power(spanStart, GetStartPosition rest3, left, op, right), rest3
                |       _ ->
                                left, rest


        and ParseFactor (stream: TokenStream, state: byref<ParseState>) : (Node * TokenStream) =
                let spanStart = GetStartPosition stream
                match TryToken stream with
                |       Some(Token.Plus(_ , _ , _), rest) ->
                                let op = List.head stream
                                let right, rest2 = ParseFactor(rest, &state)
                                Node.UnaryPlus(spanStart, GetStartPosition rest2, op, right), rest2
                |       Some(Token.Minus(_ , _ , _), rest) ->
                                let op = List.head stream
                                let right, rest2 = ParseFactor(rest, &state)
                                Node.UnaryMinus(spanStart, GetStartPosition rest2, op, right), rest2
                |       Some(Token.BitInvert(_ , _ , _), rest) ->
                                let op = List.head stream
                                let right, rest2 = ParseFactor(rest, &state)
                                Node.BitInvert(spanStart, GetStartPosition rest2, op, right), rest2
                |       _ ->
                               ParsePower(stream, &state) 