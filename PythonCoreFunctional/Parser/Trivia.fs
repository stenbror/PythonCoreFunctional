
namespace PythonCore.Runtime.Parser

type Trivia = 
    |   Empty
    |   Whitespace of int * int
    |   Tabulator of int * int
    |   Newline of int * int * char * char
    |   LineContinuation of int * int
    |   Comment of int * int * string
    