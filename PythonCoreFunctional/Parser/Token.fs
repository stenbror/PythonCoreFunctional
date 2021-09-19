
namespace PythonCore.Runtime.Parser

type Token =
    |   Empty
    |   Eof of uint32 * Trivia array
    |   Newline of uint32 * uint32 * char * char * Trivia array
    |   Indent of uint32
    |   Dedent of uint32
    |   False of uint32 * uint32 * Trivia array
    |   None of uint32 * uint32 * Trivia array
    |   True of uint32 * uint32 * Trivia array
    |   And of uint32 * uint32 * Trivia array
    |   As of uint32 * uint32 * Trivia array
    |   Assert of uint32 * uint32 * Trivia array
    |   Async of uint32 * uint32 * Trivia array
    |   Await of uint32 * uint32 * Trivia array
    |   Break of uint32 * uint32 * Trivia array
    |   Class of uint32 * uint32 * Trivia array
    |   Continue of uint32 * uint32 * Trivia array
    |   Def of uint32 * uint32 * Trivia array
    |   Del of uint32 * uint32 * Trivia array
    |   Elif of uint32 * uint32 * Trivia array
    |   Else of uint32 * uint32 * Trivia array
    |   Except of uint32 * uint32 * Trivia array
    |   Finally of uint32 * uint32 * Trivia array
    |   For of uint32 * uint32 * Trivia array
    |   From of uint32 * uint32 * Trivia array
    |   Global of uint32 * uint32 * Trivia array
    |   If of uint32 * uint32 * Trivia array
    |   Import of uint32 * uint32 * Trivia array
    |   In of uint32 * uint32 * Trivia array
    |   Is of uint32 * uint32 * Trivia array
    |   Lambda of uint32 * uint32 * Trivia array
    |   NonLocal of uint32 * uint32 * Trivia array
    |   Not of uint32 * uint32 * Trivia array
    |   Or of uint32 * uint32 * Trivia array
    |   Pass of uint32 * uint32 * Trivia array
    |   Raise of uint32 * uint32 * Trivia array
    |   Return of uint32 * uint32 * Trivia array
    |   Try of uint32 * uint32 * Trivia array
    |   While of uint32 * uint32 * Trivia array
    |   With of uint32 * uint32 * Trivia array
    |   Yield of uint32 * uint32 * Trivia array
    |   Plus of uint32 * uint32 * Trivia array
    |   Minus of uint32 * uint32 * Trivia array
    |   Mul of uint32 * uint32 * Trivia array
    |   Power of uint32 * uint32 * Trivia array
    |   Div of uint32 * uint32 * Trivia array
    |   FloorDiv of uint32 * uint32 * Trivia array
    |   Modulo of uint32 * uint32 * Trivia array
    |   Matrice of uint32 * uint32 * Trivia array
    |   ShiftLeft of uint32 * uint32 * Trivia array
    |   ShiftRight of uint32 * uint32 * Trivia array
    |   BitAnd of uint32 * uint32 * Trivia array
    |   BitOr of uint32 * uint32 * Trivia array
    |   BitXor of uint32 * uint32 * Trivia array
    |   BitInvert of uint32 * uint32 * Trivia array
    |   Less of uint32 * uint32 * Trivia array
    |   Greater of uint32 * uint32 * Trivia array
    |   LessEqual of uint32 * uint32 * Trivia array
    |   GreaterEqual of uint32 * uint32 * Trivia array
    |   Equal of uint32 * uint32 * Trivia array
    |   NotEqual of uint32 * uint32 * Trivia array
    |   LeftParen of uint32 * uint32 * Trivia array
    |   RightParen of uint32 * uint32 * Trivia array
    |   LeftBracket of uint32 * uint32 * Trivia array
    |   RightBracket of uint32 * uint32 * Trivia array
    |   LeftCurly of uint32 * uint32 * Trivia array
    |   RightCurly of uint32 * uint32 * Trivia array
    |   Comma of uint32 * uint32 * Trivia array
    |   Colon of uint32 * uint32 * Trivia array
    |   ColonAssign of uint32 * uint32 * Trivia array
    |   Dot of uint32 * uint32 * Trivia array
    |   Elipsis of uint32 * uint32 * Trivia array
    |   SemiColon of uint32 * uint32 * Trivia array
    |   Assign of uint32 * uint32 * Trivia array
    |   Arrow of uint32 * uint32 * Trivia array
    |   PlusAssign of uint32 * uint32 * Trivia array
    |   MinusAssign of uint32 * uint32 * Trivia array
    |   MulAssign of uint32 * uint32 * Trivia array
    |   PowerAssign of uint32 * uint32 * Trivia array
    |   DivAssign of uint32 * uint32 * Trivia array
    |   FloorDivAssign of uint32 * uint32 * Trivia array
    |   ModuloAssign of uint32 * uint32 * Trivia array
    |   BitAndAssign of uint32 * uint32 * Trivia array
    |   BitOrAssign of uint32 * uint32 * Trivia array
    |   BitXorAssign of uint32 * uint32 * Trivia array
    |   MatriceAssign of uint32 * uint32 * Trivia array
    |   ShiftLeftAssign of uint32 * uint32 * Trivia array
    |   ShiftRightAssign of uint32 * uint32 * Trivia array
    |   Name of uint32 * uint32 * Trivia array * string array
    |   Number of uint32 * uint32 * Trivia array * string
    |   String of uint32 * uint32 * Trivia array * string array
    |   Match of uint32 * uint32 * Trivia array // Positional keyword / Tokenizer gives a Name token, but parser replaces it with this if rule matches.
    |   Case of uint32 * uint32 * Trivia array // Positional keyword / Tokenizer gives a Name token, but parser replaces it with this if rule matches.
    |   MatchAllPattern of uint32 * uint32 * Trivia array // Positional keyword / Tokenizer gives a Name token, but parser replaces it with this if rule matches.

type TokenStream = Token list
