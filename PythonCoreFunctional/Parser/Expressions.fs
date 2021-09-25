
namespace PythonCore.Runtime.Parser

module Expressions =

    let TryToken (stream: TokenStream) =
        match stream with
        |   symbol :: rest ->   Some( symbol, rest )
        |   [] ->   Option.None

    let rec ParseAtom (stream: TokenStream, state: byref<ParseState>) : Node = 
        Node.Empty
