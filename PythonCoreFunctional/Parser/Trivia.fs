
namespace PythonCore.Runtime.Parser

type Trivia = 
    |   Empty
    |   Whitespace of uint32 * uint32
    |   Tabulator of uint32 * uint32
    |   Newline of uint32 * uint32 * char * char
    |   LineContinuation of uint32 * uint32
    |   Comment of uint32 * uint32 * string
