
namespace PythonCore.Runtime.Parser

type Token =
    |   Empty
    |   Eof of uint32 * Trivia
    |   Newline of uint32 * uint32 * char * char * Trivia
    |   Indent of uint32
    |   Dedent of uint32
    |   False of uint32 * uint32 * Trivia
    |   None of uint32 * uint32 * Trivia
    |   True of uint32 * uint32 * Trivia
    |   And of uint32 * uint32 * Trivia
    |   As of uint32 * uint32 * Trivia
    |   Assert of uint32 * uint32 * Trivia
    |   Async of uint32 * uint32 * Trivia
    |   Await of uint32 * uint32 * Trivia
    |   Break of uint32 * uint32 * Trivia
    |   Class of uint32 * uint32 * Trivia
    |   Continue of uint32 * uint32 * Trivia
    |   Def of uint32 * uint32 * Trivia
    |   Del of uint32 * uint32 * Trivia
    |   Elif of uint32 * uint32 * Trivia
    |   Else of uint32 * uint32 * Trivia
    |   Except of uint32 * uint32 * Trivia
    |   Finally of uint32 * uint32 * Trivia
    |   For of uint32 * uint32 * Trivia
    |   From of uint32 * uint32 * Trivia
    |   Global of uint32 * uint32 * Trivia
    |   If of uint32 * uint32 * Trivia
    |   Import of uint32 * uint32 * Trivia
    |   In of uint32 * uint32 * Trivia
    |   Is of uint32 * uint32 * Trivia
    |   Lambda of uint32 * uint32 * Trivia
    |   NonLocal of uint32 * uint32 * Trivia
    |   Not of uint32 * uint32 * Trivia
    |   Or of uint32 * uint32 * Trivia
    |   Pass of uint32 * uint32 * Trivia
    |   Raise of uint32 * uint32 * Trivia
    |   Return of uint32 * uint32 * Trivia
    |   Try of uint32 * uint32 * Trivia
    |   While of uint32 * uint32 * Trivia
    |   With of uint32 * uint32 * Trivia
    |   Yield of uint32 * uint32 * Trivia


    |   Name of uint32 * uint32 * Trivia * string
    |   Number of uint32 * uint32 * Trivia * string
    |   String of uint32 * uint32 * Trivia * string array

type TokenStream = Token list
