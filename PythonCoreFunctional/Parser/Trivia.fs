
namespace PythonCore.Runtime.Parser

type Trivia = 
    |   Empty
    |   WhiteSpace of uint32 * uint32
    |   Tabulator of uint32 * uint32
    |   Newline of uint32 * uint32 * char * char
    |   LineContinuation of uint32 * uint32 * char * char * char
    |   Comment of uint32 * uint32 * string
