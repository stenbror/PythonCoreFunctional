
namespace PythonCore.Runtime.Parser

module Tokenizer =

    open System

    let Keywords =  
            [ 
                ( "and",        Token.And );
                ( "as",         Token.As );
                ( "assert",     Token.Assert );
                ( "async",      Token.Async );
                ( "await",      Token.Await );
                ( "break",      Token.Break );
                ( "class",      Token.Class );
                ( "continue",   Token.Continue );
                ( "def",        Token.Def );
                ( "del",        Token.Del );
                ( "elif",       Token.Elif );
                ( "else",       Token.Else );
                ( "except",     Token.Except );
                ( "finally",    Token.Finally );
                ( "for",        Token.For );
                ( "from",       Token.From );
                ( "global",     Token.Global );
                ( "if",         Token.If );
                ( "import",     Token.Import );
                ( "in",         Token.In );
                ( "is",         Token.Is );
                ( "lambda",     Token.Lambda );
                ( "nonlocal",   Token.NonLocal );
                ( "not",        Token.Not );
                ( "or",         Token.Or );
                ( "pass",       Token.Pass );
                ( "raise",      Token.Raise );
                ( "return",     Token.Return );
                ( "try",        Token.Try );
                ( "while",      Token.While );
                ( "with",       Token.With );
                ( "yield",      Token.Yield );
                ( "False",      Token.False );
                ( "None",       Token.None );
                ( "True",       Token.True );
            ] |> Map.ofList


    type IndentationOrLevelStack<'T> =
        | Empty 
        | Stack of 'T * IndentationOrLevelStack<'T>

        member s.Push x = Stack(x, s)

        member s.Pop() = 
          match s with
          | Empty -> failwith "Underflow"
          | Stack(t,_) -> t
          
        member s.Rest() =
          match s with
          | Empty -> Empty
          | Stack(_, r) -> r

        member s.IEmpty = 
          match s with
          | Empty -> true
          | _ -> false


    [<Struct>]
    type TokenizerState =
        {
            mutable IndentStack : uint32 array
            mutable IndentLevel : uint32
            mutable LevelStack : IndentationOrLevelStack<char>
            mutable Pending : int32
            mutable Index : int32
            mutable TokenStartPos : uint32
            mutable AtBOL : bool
            mutable TabSize : uint32
            mutable IsInteractiveMode : bool
            mutable SourceCode : char array
            mutable TriviaList : Trivia list
            mutable IsBlankLine : bool
            mutable IsDone : bool
        }
        with
            static member Init source =
                {
                    SourceCode = source
                    TabSize = 8ul
                    IsInteractiveMode = false
                    Pending = 0l
                    Index = 0l
                    TokenStartPos = 0ul
                    AtBOL = true
                    LevelStack = IndentationOrLevelStack.Empty
                    IndentStack = Array.zeroCreate 100
                    IndentLevel = 0ul
                    TriviaList = List.Empty
                    IsBlankLine = false
                    IsDone = false
                }


    let GetChar (state : byref<TokenizerState>) : Option<char> =
        if state.Index >= state.SourceCode.Length then
            Option.None
        else
            let current = state.Index
            state.Index <- state.Index + 1l
            Some(state.SourceCode.[current])
            
    let PeekChar (state : byref<TokenizerState>, step : int32) : Option<char> =
        if (state.Index + step) >= state.SourceCode.Length then
            Option.None
        else
            Some(state.SourceCode.[state.Index + step])
    
    let IsHexDigit (ch : char) : bool =
        match ch with
        | '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9'
        | 'a' | 'b' | 'c' | 'd' | 'e' | 'f'
        | 'A' | 'B' | 'C' | 'D' | 'E' | 'F' ->    true
        | _ -> false
        
    let IsOctetDigit (ch : char) : bool =
        match ch with
        | '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' ->    true
        | _ ->    false
        
    let IsBoolDigit (ch : char) : bool =
        match ch with
        | '0' | '1' ->    true
        | _ ->    false
                        
    let IsStartLetter (ch: char) : bool =
         Char.IsLetter (ch) || ch = '_'
         
    let IsLetterOrDigit (ch: char) : bool =
         Char.IsLetterOrDigit (ch) || ch = '_'

    let TriviaKeeping(state : byref<TokenizerState>, steps : int32) : Trivia array =
            state.Index <- state.Index + steps
            let trivia = List.toArray(List.rev state.TriviaList)
            state.TriviaList <- List.Empty
            state.TokenStartPos <- (uint32)state.Index
            trivia

    
    let rec OperatorOrDelimiters (state : byref<TokenizerState>) : Token option =
        let ch1 = match GetChar(&state) with | Some(a) -> a | Option.None -> (char)0x00 
        let ch2 = match PeekChar(&state, 0) with | Some(a) -> a | Option.None -> (char)0x00
        let ch3 = match PeekChar(&state, 1) with | Some(a) -> a | Option.None -> (char)0x00
        let _TokenStartPos = state.TokenStartPos
        
        match ch1, ch2, ch3 with
        |    '*', '*', '=' ->
                let trivia = TriviaKeeping(&state, 2)
                Some(Token.PowerAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '*', '*', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.Power(_TokenStartPos, (uint32)state.Index, trivia))
        |    '*', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.MulAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '*', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Mul(_TokenStartPos, (uint32)state.Index, trivia))
        |    '/', '/', '=' ->
                let trivia = TriviaKeeping(&state, 2)
                Some(Token.FloorDivAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '/', '/', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.FloorDiv(_TokenStartPos, (uint32)state.Index, trivia))
        |    '/', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.DivAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '/', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Div(_TokenStartPos, (uint32)state.Index, trivia))
        |    '<', '<', '=' ->
                let trivia = TriviaKeeping(&state, 2)
                Some(Token.ShiftLeftAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '<', '<', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.ShiftLeft(_TokenStartPos, (uint32)state.Index, trivia))
        |    '<', '>', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.NotEqual(_TokenStartPos, (uint32)state.Index, trivia))
        |    '<', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.LessEqual(_TokenStartPos, (uint32)state.Index, trivia))
        |    '<', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Less(_TokenStartPos, (uint32)state.Index, trivia))
        |    '>', '>', '=' ->
                let trivia = TriviaKeeping(&state, 2)
                Some(Token.ShiftRightAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '>', '>', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.ShiftRight(_TokenStartPos, (uint32)state.Index, trivia))
        |    '>', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.GreaterEqual(_TokenStartPos, (uint32)state.Index, trivia))
        |    '>', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Greater(_TokenStartPos, (uint32)state.Index, trivia))
        |    '.', '.', '.'  ->
                let trivia = TriviaKeeping(&state, 2)
                Some(Token.Elipsis(_TokenStartPos, (uint32)state.Index, trivia))
        |    '.', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Dot(_TokenStartPos, (uint32)state.Index, trivia))
        |    '+', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.PlusAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '+', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Plus(_TokenStartPos, (uint32)state.Index, trivia))
        |    '-', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.MinusAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '-', '>', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.Arrow(_TokenStartPos, (uint32)state.Index, trivia))
        |    '-', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Minus(_TokenStartPos, (uint32)state.Index, trivia))
        |    '%', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.ModuloAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '%', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Modulo(_TokenStartPos, (uint32)state.Index, trivia))
        |    '@', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.MatriceAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '@', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Matrice(_TokenStartPos, (uint32)state.Index, trivia))
        |    '=', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.Equal(_TokenStartPos, (uint32)state.Index, trivia))
        |    '=', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Assign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '&', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.BitAndAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '&', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.BitAnd(_TokenStartPos, (uint32)state.Index, trivia))
        |    '|', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.BitOrAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '|', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.BitOr(_TokenStartPos, (uint32)state.Index, trivia))
        |    '^', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.BitXorAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    '^', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.BitXor(_TokenStartPos, (uint32)state.Index, trivia))
        |    ':', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.ColonAssign(_TokenStartPos, (uint32)state.Index, trivia))
        |    ':', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Colon(_TokenStartPos, (uint32)state.Index, trivia))
        |    '!', '=', _ ->
                let trivia = TriviaKeeping(&state, 1)
                Some(Token.NotEqual(_TokenStartPos, (uint32)state.Index, trivia))
        |    '~', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.BitInvert(_TokenStartPos, (uint32)state.Index, trivia))
        |    ',', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Comma(_TokenStartPos, (uint32)state.Index, trivia))
        |    ';', _ , _ ->
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.SemiColon(_TokenStartPos, (uint32)state.Index, trivia))
        |    '(', _ , _ ->
                state.LevelStack <- state.LevelStack.Push (ch1)
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.LeftParen(_TokenStartPos, (uint32)state.Index, trivia))
        |    '[', _ , _ ->
                state.LevelStack <- state.LevelStack.Push (ch1)
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.LeftBracket(_TokenStartPos, (uint32)state.Index, trivia))
        |    '{', _ , _ ->
                state.LevelStack <- state.LevelStack.Push (ch1)
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.LeftCurly(_TokenStartPos, (uint32)state.Index, trivia))
        |    ')', _ , _ ->
                if state.LevelStack.IEmpty = false && state.LevelStack.Pop() = '(' then
                    let trivia = TriviaKeeping(&state, 0)
                    state.LevelStack <- state.LevelStack.Rest()
                    Some(Token.RightParen(_TokenStartPos, (uint32)state.Index, trivia))
                else
                    Option.None
        |    ']', _ , _ ->
                if state.LevelStack.IEmpty = false && state.LevelStack.Pop() = '[' then
                    let trivia = TriviaKeeping(&state, 0)
                    state.LevelStack <- state.LevelStack.Rest()
                    Some(Token.RightBracket(_TokenStartPos, (uint32)state.Index, trivia))
                else
                    Option.None
        |    '}', _ , _ ->
                if state.LevelStack.IEmpty = false && state.LevelStack.Pop() = '{' then
                    let trivia = TriviaKeeping(&state, 0)
                    state.LevelStack <- state.LevelStack.Rest()
                    Some(Token.RightCurly(_TokenStartPos, (uint32)state.Index, trivia))
                else
                    Option.None
        |    _ , _ , _ ->
                state.Index <- state.Index - 1
                Option.None

    and NumberLiteral (state : byref<TokenizerState>) : Token option =
        let _TokenStartPos = state.TokenStartPos
        match PeekChar(&state, 0), PeekChar(&state, 1) with
        |  Some(x), Some(y) when x = '.' && Char.IsDigit(y) = false ->
                Option.None
        |  Some(x), _ when x = '0' ->        // Number starting with zero
                state.Index <- state.Index + 1
                
                match PeekChar(&state, 0) with
                | Some(x) when x = 'x' || x = 'X' ->
                        state.Index <- state.Index + 1
                        
                        while match PeekChar(&state, 0) with
                              | Some(x) when IsHexDigit(x) ->
                                   state.Index <- state.Index + 1
                                   match PeekChar(&state, 0) with
                                   |  Some(x) when x = '_' ->
                                           state.Index <- state.Index + 1
                                           match PeekChar(&state, 0) with | Some(x) when IsHexDigit(x) = false -> raise (LexicalError("Expecting digit after '_' in Number!", (uint32)state.Index)) | _ -> ()
                                   |  _ -> ()
                                   true
                              | Some(x) when IsBoolDigit(x) = false -> raise (LexicalError("Expecting hex digit in hex decimal Number!", (uint32)state.Index))
                              | Some(_) -> false
                              | _ -> false
                            do ()    
                | Some(x) when x = 'o' || x = 'O' ->
                        state.Index <- state.Index + 1
                        
                        while match PeekChar(&state, 0) with
                              | Some(x) when IsOctetDigit(x) ->
                                   state.Index <- state.Index + 1
                                   match PeekChar(&state, 0) with
                                   |  Some(x) when x = '_' ->
                                           state.Index <- state.Index + 1
                                           match PeekChar(&state, 0) with | Some(x) when IsOctetDigit(x) = false -> raise (LexicalError("Expecting digit after '_' in Number!", (uint32)state.Index)) | _ -> ()
                                   |  _ -> ()
                                   true
                              | Some(x) when IsBoolDigit(x) = false -> raise (LexicalError("Expecting between '0' and '7' digit in octet Number!", (uint32)state.Index))
                              | Some(_) -> false
                              | _ -> false
                            do ()    
                | Some(x) when x = 'b' || x = 'B' ->
                        state.Index <- state.Index + 1
                        
                        while match PeekChar(&state, 0) with
                              | Some(x) when IsBoolDigit(x) ->
                                   state.Index <- state.Index + 1
                                   match PeekChar(&state, 0) with
                                   |  Some(x) when x = '_' ->
                                           state.Index <- state.Index + 1
                                           match PeekChar(&state, 0) with | Some(x) when IsBoolDigit(x) = false -> raise (LexicalError("Expecting digit after '_' in Number!", (uint32)state.Index)) | _ -> ()
                                   |  _ -> ()
                                   true
                              | Some(x) when IsBoolDigit(x) = false -> raise (LexicalError("Expecting '0' or '1' digit in binary Number!", (uint32)state.Index))
                              | Some(_) -> false
                              | _ -> false
                            do ()       
                | Some(_) ->        // Decimal number
                        let mutable nonZero = false
                        
                        match PeekChar(&state, 0) with
                        |  Some(x) when x <> '.' ->
                                while   match PeekChar(&state, 0) with
                                        | Some(x) when x = '0' ->
                                                state.Index <- state.Index + 1
                                                true
                                        | Some(x) when Char.IsDigit(x) ->
                                                nonZero <- true
                                                state.Index <- state.Index + 1
                                                true
                                        | _ ->  false
                                     do ()
                        |  _ -> ()

                        match PeekChar(&state, 0) with
                        | Some(x) when x = '.' ->
                              state.Index <- state.Index + 1
                              while match PeekChar(&state, 0) with
                                    | Some(x) when Char.IsDigit(x) ->
                                           state.Index <- state.Index + 1
                                           match PeekChar(&state, 0) with
                                           |  Some(x) when x = '_' ->
                                                   state.Index <- state.Index + 1
                                                   match PeekChar(&state, 0) with | Some(x) when Char.IsDigit(x) = false -> raise (LexicalError("Expecting digit after '_' in Number!", (uint32)state.Index)) | _ -> ()
                                           |  _ -> ()
                                           true
                                    | Some(_) -> false
                                    | _ -> false
                                do ()
                        | _ -> ()
                        
                        match PeekChar(&state, 0) with
                        | Some(x) when x = 'e' || x = 'E' ->
                                state.Index <- state.Index + 1
                                
                                match PeekChar(&state, 0) with
                                | Some(x) when x = '+' || x = '-' ->
                                       state.Index <- state.Index + 1
                                       match PeekChar(&state, 0) with | Some(x) when Char.IsDigit(x) = false -> raise (LexicalError("Expecting digit after '+' or '-' in exponent part of Number!", (uint32)state.Index)) | _ -> ()
                                | _ -> ()
                                
                                while match PeekChar(&state, 0) with
                                      | Some(x) when Char.IsDigit(x) ->
                                           state.Index <- state.Index + 1
                                           match PeekChar(&state, 0) with
                                           |  Some(x) when x = '_' ->
                                                   state.Index <- state.Index + 1
                                                   match PeekChar(&state, 0) with | Some(x) when Char.IsDigit(x) = false -> raise (LexicalError("Expecting digit after '_' in Number!", (uint32)state.Index)) | _ -> ()
                                           |  _ -> ()
                                           true
                                      | Some(_) -> false
                                      | _ -> false
                                    do ()
                        | _ -> ()
                        
                        match PeekChar(&state, 0) with
                        | Some(x) when x = 'j' || x = 'J' ->
                                state.Index <- state.Index + 1
                        | _ -> ()
                        
                | Option.None -> ()
                
                let text = System.String(state.SourceCode.[(int32)_TokenStartPos..state.Index])
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Number(_TokenStartPos, (uint32)state.Index, trivia, text))
        |  Some(x), _  when (x >= '1' && x <= '9') || x = '.' ->                     // Number starting with digit other than zero. Possibly '.'
                if x <> '.' then
                     while match PeekChar(&state, 0) with
                           | Some(x) when Char.IsDigit(x) ->
                                   state.Index <- state.Index + 1
                                   match PeekChar(&state, 0) with
                                   |  Some(x) when x = '_' ->
                                           state.Index <- state.Index + 1
                                           match PeekChar(&state, 0) with | Some(x) when Char.IsDigit(x) = false -> raise (LexicalError("Expecting digit after '_' in Number!", (uint32)state.Index)) | _ -> ()
                                   |  _ -> ()
                                   true
                           | Some(_) -> false
                           | _ -> false
                        do ()
                        
                match PeekChar(&state, 0) with
                | Some(x) when x = '.' ->
                      state.Index <- state.Index + 1
                      while match PeekChar(&state, 0) with
                            | Some(x) when Char.IsDigit(x) ->
                                   state.Index <- state.Index + 1
                                   match PeekChar(&state, 0) with
                                   |  Some(x) when x = '_' ->
                                           state.Index <- state.Index + 1
                                           match PeekChar(&state, 0) with | Some(x) when Char.IsDigit(x) = false -> raise (LexicalError("Expecting digit after '_' in Number!", (uint32)state.Index)) | _ -> ()
                                   |  _ -> ()
                                   true
                            | Some(_) -> false
                            | _ -> false
                        do ()
                | _ -> ()
                
                match PeekChar(&state, 0) with
                | Some(x) when x = 'e' || x = 'E' ->
                        state.Index <- state.Index + 1
                        
                        match PeekChar(&state, 0) with
                        | Some(x) when x = '+' || x = '-' ->
                               state.Index <- state.Index + 1
                               match PeekChar(&state, 0) with | Some(x) when Char.IsDigit(x) = false -> raise (LexicalError("Expecting digit after '+' or '-' in exponent part of Number!", (uint32)state.Index)) | _ -> ()
                        | _ -> ()
                        
                        while match PeekChar(&state, 0) with
                              | Some(x) when Char.IsDigit(x) ->
                                   state.Index <- state.Index + 1
                                   match PeekChar(&state, 0) with
                                   |  Some(x) when x = '_' ->
                                           state.Index <- state.Index + 1
                                           match PeekChar(&state, 0) with | Some(x) when Char.IsDigit(x) = false -> raise (LexicalError("Expecting digit after '_' in Number!", (uint32)state.Index)) | _ -> ()
                                   |  _ -> ()
                                   true
                              | Some(_) -> false
                              | _ -> false
                            do ()
                | _ -> ()
                
                match PeekChar(&state, 0) with
                | Some(x) when x = 'j' || x = 'J' ->
                        state.Index <- state.Index + 1
                | _ -> ()
                
                let text = System.String(state.SourceCode.[(int32)_TokenStartPos..state.Index - 1])
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.Number(_TokenStartPos, (uint32)state.Index, trivia, text))
        |  _ ->
                Option.None

    and StringLiteral (state : byref<TokenizerState>, tokenStart : uint32) : Token option =
        let _TokenStartPos = tokenStart
        match PeekChar(&state, 0) with
        | Some(x1) when x1 = '\'' || x1 = '"' ->
                let x2 = match PeekChar(&state, 1) with | Some(x2) -> x2 | _ -> (char)0x00
                let x3 = match PeekChar(&state, 2) with | Some(x2) -> x2 | _ -> (char)0x00
                let isTriple = (x1 = x2) && (x2 = x3)
                let isEmpty = (x1 = x2) && not isTriple
                
                if isEmpty then
                     state.Index <- state.Index + 2
                else
                     if isTriple then
                          state.Index <- state.Index + 3
                     else
                          state.Index <- state.Index + 1
                     
                     while  match PeekChar(&state, 0) with
                            | Some(x4) when x4 <> x1 ->
                                    state.Index <- state.Index + 1
                                    true
                            | Some(x4) ->
                                    if isTriple then
                                        let x5 = match PeekChar(&state, 1) with | Some(x2) -> x2 | _ -> (char)0x00
                                        let x6 = match PeekChar(&state, 2) with | Some(x2) -> x2 | _ -> (char)0x00
                                        if (x4 = x5) && (x5 = x6) then
                                             state.Index <- state.Index + 3
                                             false
                                        else
                                             state.Index <- state.Index + 1
                                             true
                                    else
                                        state.Index <- state.Index + 1
                                        false 
                            | Option.None ->  false
                        do ()
                     
                let text = System.String(state.SourceCode.[(int32)_TokenStartPos..state.Index - 1])
                let trivia = TriviaKeeping(&state, 0)
                Some(Token.String(_TokenStartPos, (uint32)state.Index, trivia, text))
        | _ ->
                Option.None

    and KeywordOrNameLiteral (state : byref<TokenizerState>) : Token option =
        let _TokenStartPos = state.TokenStartPos
        
        match   IsStartLetter (match PeekChar(&state, 0) with | Some(x) -> x | _ -> ' ') with
        | true ->
        
                while IsLetterOrDigit (match PeekChar(&state, 0) with | Some(x) -> x | _ -> ' ') do
                        state.Index <- state.Index + 1
                        
                let text = System.String(state.SourceCode.[(int32)_TokenStartPos..state.Index - 1])
                        
                if Keywords.ContainsKey text then
                        
                        let trivia = TriviaKeeping(&state, 0)
                        Some(Keywords.[text](_TokenStartPos, (uint32)state.Index, trivia))
                else
                        match PeekChar(&state, 0) with
                        | Some(x) when x = '\'' || x = '"' ->
                                match text with
                                | "r"
                                | "u"
                                | "R"
                                | "U"
                                | "f"
                                | "F"
                                | "fr"
                                | "Fr"
                                | "fR"
                                | "FR"
                                | "rf"
                                | "rF"
                                | "Rf"
                                | "RF" ->
                                        StringLiteral(&state, _TokenStartPos)
                                | _ ->
                                      let trivia = TriviaKeeping(&state, 0)
                                      Some(Token.Name(_TokenStartPos, (uint32)state.Index, trivia, text))
                        | _ ->    
                                let trivia = TriviaKeeping(&state, 0)
                                Some(Token.Name(_TokenStartPos, (uint32)state.Index, trivia, text))
        | _ ->
                Option.None

    and TypeCommentOrComment (state : byref<TokenizerState>) : Token option =
        let _TokenStartPos = state.TokenStartPos
        match PeekChar(&state, 0) with
        |    Some(x) ->
                match x with
                | '#' ->
                        state.Index <- state.Index + 1
                        while   match PeekChar(&state, 0) with
                                | Some( x ) when x <> '\r' && x <> '\n'  ->
                                        state.Index <- state.Index + 1
                                        true
                                | Some(_) -> false
                                | Option.None -> false
                                do ()
                        let text = System.String(state.SourceCode.[(int32)_TokenStartPos..state.Index - 1])
                        if text.StartsWith("# type: ") then
                             let trivia = TriviaKeeping(&state, 0)
                             Some(Token.TypeComment(_TokenStartPos, (uint32)state.Index, trivia, text))
                        else
                             state.TriviaList <- Trivia.Comment(_TokenStartPos, (uint32)state.Index, text) :: state.TriviaList
                             Some(InnerLoop(&state)) // Recursive call innerloop again!
                |   _ -> Option.None
        |   _ ->
                Option.None

    and NewlineHandling (state : byref<TokenizerState>) : Token option =
         let _TokenStartPos = state.TokenStartPos
         match PeekChar(&state, 0) with
         | Some(x) when x = '\r' || x = '\n' ->
                 match PeekChar(&state, 0), PeekChar(&state, 1) with
                 |  Some(x), Some(y) ->
                        match x, y with
                        | '\r', '\n' ->     state.Index <- state.Index + 2l
                        | '\r', _ ->        state.Index <- state.Index + 1l
                        | '\n', _ ->        state.Index <- state.Index + 1l
                        | _ -> ()
                        state.AtBOL <- true
                        if state.IsBlankLine || state.LevelStack.IEmpty = false then
                              state.TriviaList <- Trivia.Newline(_TokenStartPos, (uint32)state.Index, x, y) :: state.TriviaList
                              Some(OuterLoop(&state)) // Recursive call outerloop again!
                        else
                              let trivia = TriviaKeeping(&state, 0)
                              Some(Token.Newline(_TokenStartPos, (uint32)state.Index, x, y, trivia))
                 |  Some(x), _ ->
                         match x with
                         | '\r' | '\n' ->
                                 state.Index <- state.Index + 1l
                                 if state.IsBlankLine || not state.LevelStack.IEmpty then
                                      state.TriviaList <- Trivia.Newline(_TokenStartPos, (uint32)state.Index, x, ' ') :: state.TriviaList
                                      Some(OuterLoop(&state)) // Recursive call outerloop again!
                                 else
                                      let trivia = TriviaKeeping(&state, 0)
                                      Some(Token.Newline(_TokenStartPos, (uint32)state.Index, x, ' ', trivia))
                         | _ ->  Option.None
                 | _ ->
                        Option.None
         | _ ->
                 Option.None
                
    and EndOfFileHandling (state : byref<TokenizerState>) : Token option =
         match PeekChar(&state, 0) with
         | Option.None ->
                 if state.IsInteractiveMode && not state.IsDone then raise (InteractiveNeedMoreInput)
                 let trivia = TriviaKeeping(&state, 0)
                 Some(Token.Eof((uint32)state.Index, trivia))
         | _ -> Option.None
         
    and WhiteSpaceHandling (state : byref<TokenizerState>) : Token option =
         let _TokenStartPos = state.TokenStartPos
         match PeekChar(&state, 0) with
         | Some(x) when x = ' ' || x = '\t' ->
                 while match PeekChar(&state, 0) with
                       | Some(x) when x = ' ' ->
                               state.Index <- state.Index + 1
                               state.TriviaList <- Trivia.WhiteSpace(_TokenStartPos, (uint32)state.Index) :: state.TriviaList
                               true
                        | Some(x) when x = '\t' ->
                                state.Index <- state.Index + 1
                                state.TriviaList <- Trivia.Tabulator(_TokenStartPos, (uint32)state.Index) :: state.TriviaList
                                true
                       | Some(_) -> false
                       | _ -> false
                       do ()
                 
                 state.TokenStartPos <- (uint32)state.Index
                 Some(InnerLoop(&state)) // Recursive call innerloop again!
         | _ ->
                 Option.None
         
    and LineContinuationHandling (state : byref<TokenizerState>) : Token Option =
         let _TokenStartPos = state.TokenStartPos
         match PeekChar(&state, 0) with
         | Some(x) ->
                 match x with
                 | '\\' ->
                      state.Index <- state.Index + 1
                      match PeekChar(&state, 0), PeekChar(&state, 1) with
                      | Some(y), Some(z) ->
                            match y, z with
                            | '\r', '\n'
                            | '\r', _
                            | '\n', _ ->
                                  if z = '\n' then state.Index <- state.Index + 1
                                  state.Index <- state.Index + 1
                                  state.TriviaList <- Trivia.LineContinuation(_TokenStartPos, (uint32)state.Index, '\\', y, z) :: state.TriviaList
                                  state.TokenStartPos <- (uint32)state.Index
                                  Some(InnerLoop(&state)) // Recursive call innerloop again!
                            | _ ->
                                  raise (LexicalError("Expecting newline after line continuation character '\\'!", (uint32)state.Index ))
                      | Some(x), Option.None ->
                            match x with
                            | '\r' | '\n' ->
                                  state.Index <- state.Index + 1
                                  state.TriviaList <- Trivia.LineContinuation(_TokenStartPos, (uint32)state.Index, '\\', x, ' ') :: state.TriviaList  
                                  state.TokenStartPos <- (uint32)state.Index
                                  Some(InnerLoop(&state)) // Recursive call innerloop again!
                            | _ ->
                                  raise (LexicalError("Expecting newline after line continuation character '\\'!", (uint32)state.Index ))
                      | _ ->
                            Option.None
                 | _ ->
                      Option.None
         | _ ->
                Option.None
               
    and PendingIndentationLevel (state : byref<TokenizerState>) : Token option =
         state.TokenStartPos <- (uint32)state.Index
         match state.Pending with
         |  _ when state.Pending < 0 ->
                state.Pending <- state.Pending + 1
                Some(Token.Dedent)
         |  _ when state.Pending > 0 ->
                state.Pending <- state.Pending - 1
                Some(Token.Indent)
         |  _ ->
                Option.None
                
    and AnalyzeIndentationLevel (state : byref<TokenizerState>) =
          match state.AtBOL with
          | true ->
                 state.AtBOL <- false
                 let mutable col = 0ul
                 
                 while  match PeekChar(&state, 0) with
                        | Some(' ') ->
                                col <- col + 1ul
                                true
                        | Some('\t') ->
                                col <- (col / state.TabSize + 1ul) *state.TabSize
                                true
                        | Some('\f') ->
                                col <- 0ul
                                true
                        | _ -> false
                        do
                                GetChar(&state) |> ignore
                 
                 match PeekChar(&state, 0) with
                 | Some('#')
                 | Some('\r')
                 | Some('\n')
                 | Some('\\') ->
                         if col = 0ul && state.IsInteractiveMode then
                              match PeekChar(&state, 0) with
                              | Some('\r')
                              | Some('\n') ->
                                   state.IsBlankLine <- false
                              | _ ->
                                   ()
                         elif state.IsInteractiveMode then
                              state.IsBlankLine <- false
                              col <- 0ul
                         else
                              state.IsBlankLine <- true
                 | _ ->  ()
                 
                 if state.IsBlankLine = false && state.LevelStack.IEmpty then
                      if col = state.IndentStack.[(int32)state.IndentLevel] then
                          () // No change in indentation level
                      elif col > state.IndentStack.[(int32)state.IndentLevel] then
                          if state.IndentLevel >= 99ul then
                               raise (LexicalError("Maximum 100 level of indentation allowed!", (uint32)state.Index))
                          state.IndentLevel <- state.IndentLevel + 1ul
                          state.IndentStack.[(int32)state.IndentLevel] <- col
                          state.Pending <- state.Pending + 1l
                      else
                           while state.IndentLevel > 0ul && col < state.IndentStack.[(int32)state.IndentLevel] do
                                state.IndentLevel <- state.IndentLevel - 1ul
                                state.Pending <- state.Pending - 1l
                           if col <> state.IndentStack.[(int32)state.IndentLevel] then
                                raise (LexicalError("Inconsistant indenation level!", (uint32)state.Index))
          | _ ->
                ()
                        
    and InnerLoop (state : byref<TokenizerState>) =
        match WhiteSpaceHandling(&state) with
        | Some(x) -> x
        | _ ->
             match TypeCommentOrComment(&state) with
             | Some(x) -> x
             | _ ->
                 match EndOfFileHandling(&state) with
                 | Some(x) -> x
                 | _ ->
                        match KeywordOrNameLiteral(&state) with
                        | Some(x) -> x
                        | _ ->
                                match NewlineHandling(&state) with
                                | Some(x) -> x
                                | _ ->
                                        match NumberLiteral(&state) with
                                        | Some(x) -> x
                                        | _ ->
                                                match StringLiteral(&state, (uint32)state.Index) with
                                                | Some(x) -> x
                                                | _ ->
                                                        match LineContinuationHandling(&state) with
                                                        | Some(x) -> x
                                                        | _ ->
                                                                match OperatorOrDelimiters(&state) with
                                                                | Some(x) -> x
                                                                | _ ->
                                                                        raise (LexicalError("Expected a valid Token!", (uint32)state.Index))
    
    and OuterLoop (state : byref<TokenizerState>) =
        AnalyzeIndentationLevel(&state)
        match PendingIndentationLevel(&state) with
        | Some(x) -> x
        | _ ->
                InnerLoop(&state)
                
    and TokenizeFromCharArray (text : char array) : TokenStream =
       let mutable state = TokenizerState.Init text
       let mutable tokenStream : Token list = []
       
       while    match OuterLoop(&state) with
                | Token.Eof(_) as x ->
                        tokenStream <- x :: tokenStream
                        false
                | x ->
                        tokenStream <- x :: tokenStream
                        true
           do ()
       
       List.rev tokenStream
