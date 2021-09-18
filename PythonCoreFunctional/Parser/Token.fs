
namespace PythonCore.Runtime.Parser

type Token =
    |   Empty
    |   Eof of uint32 * Trivia
    |   Newline of uint32 * uint32 * char * char * Trivia
    |   Indent of uint32
    |   Dedent of uint32
    |   False of uint32 * uint32 * Trivia
    |   None of uint32 * uint32 * Trivia

type TokenStream = Token list
