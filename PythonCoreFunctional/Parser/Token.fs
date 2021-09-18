
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
    |   Plus of uint32 * uint32 * Trivia
    |   Minus of uint32 * uint32 * Trivia
    |   Mul of uint32 * uint32 * Trivia
    |   Power of uint32 * uint32 * Trivia
    |   Div of uint32 * uint32 * Trivia
    |   FloorDiv of uint32 * uint32 * Trivia
    |   Modulo of uint32 * uint32 * Trivia
    |   Matrice of uint32 * uint32 * Trivia
    |   ShiftLeft of uint32 * uint32 * Trivia
    |   ShiftRight of uint32 * uint32 * Trivia
    |   BitAnd of uint32 * uint32 * Trivia
    |   BitOr of uint32 * uint32 * Trivia
    |   BitXor of uint32 * uint32 * Trivia
    |   BitInvert of uint32 * uint32 * Trivia
    |   Less of uint32 * uint32 * Trivia
    |   Greater of uint32 * uint32 * Trivia
    |   LessEqual of uint32 * uint32 * Trivia
    |   GreaterEqual of uint32 * uint32 * Trivia
    |   Equal of uint32 * uint32 * Trivia
    |   NotEqual of uint32 * uint32 * Trivia
    |   LeftParen of uint32 * uint32 * Trivia
    |   RightParen of uint32 * uint32 * Trivia
    |   LeftBracket of uint32 * uint32 * Trivia
    |   RightBracket of uint32 * uint32 * Trivia
    |   LeftCurly of uint32 * uint32 * Trivia
    |   RightCurly of uint32 * uint32 * Trivia
    |   Comma of uint32 * uint32 * Trivia
    |   Colon of uint32 * uint32 * Trivia
    |   ColonAssign of uint32 * uint32 * Trivia
    |   Dot of uint32 * uint32 * Trivia
    |   Elipsis of uint32 * uint32 * Trivia
    |   SemiColon of uint32 * uint32 * Trivia
    |   Assign of uint32 * uint32 * Trivia
    |   Arrow of uint32 * uint32 * Trivia
    |   PlusAssign of uint32 * uint32 * Trivia
    |   MinusAssign of uint32 * uint32 * Trivia
    |   MulAssign of uint32 * uint32 * Trivia
    |   PowerAssign of uint32 * uint32 * Trivia
    |   DivAssign of uint32 * uint32 * Trivia
    |   FloorDivAssign of uint32 * uint32 * Trivia
    |   ModuloAssign of uint32 * uint32 * Trivia
    |   BitAndAssign of uint32 * uint32 * Trivia
    |   BitOrAssign of uint32 * uint32 * Trivia
    |   BitXorAssign of uint32 * uint32 * Trivia
    |   MatriceAssign of uint32 * uint32 * Trivia
    |   ShiftLeftAssign of uint32 * uint32 * Trivia
    |   ShiftRightAssign of uint32 * uint32 * Trivia
    |   Name of uint32 * uint32 * Trivia * string
    |   Number of uint32 * uint32 * Trivia * string
    |   String of uint32 * uint32 * Trivia * string array

type TokenStream = Token list
