
namespace PythonCore.Runtime.Parser

type Node =
    |   Empty
    |   NamedExpr of uint32 * uint32 * Node * Token * Node
    |   Test of uint32 * uint32 * Node * Token * Node * Token * Node
    |   Lambda of uint32 * uint32 * Token * Node * Token * Node * bool
    |   OrTest of uint32 * uint32 * Node * Token * Node 
    |   AndTest of uint32 * uint32 * Node * Token * Node 
    |   NotTest of uint32 * uint32 * Token * Node 
    |   Less of uint32 * uint32 * Node * Token * Node 
    |   Greater of uint32 * uint32 * Node * Token * Node 
    |   Equal of uint32 * uint32 * Node * Token * Node 
    |   GreaterEqual of uint32 * uint32 * Node * Token * Node 
    |   LessEqual of uint32 * uint32 * Node * Token * Node 
    |   NotEqual of uint32 * uint32 * Node * Token * Node 
    |   In of uint32 * uint32 * Node * Token * Node 
    |   NotIn of uint32 * uint32 * Node * Token * Node 
    |   Is of uint32 * uint32 * Node * Token * Node 
    |   IsNot of uint32 * uint32 * Node * Token * Node 
    |   StarExpr of uint32 * uint32 * Token * Node
    |   OrExpr of uint32 * uint32 * Node * Token * Node 
    |   XorExpr of uint32 * uint32 * Node * Token * Node 
    |   AndExpr of uint32 * uint32 * Node * Token * Node 
    |   ShiftLeft of uint32 * uint32 * Node * Token * Node 
    |   ShiftRight of uint32 * uint32 * Node * Token * Node 
    |   Plus of uint32 * uint32 * Node * Token * Node 
    |   Minus of uint32 * uint32 * Node * Token * Node 
    |   Mul of uint32 * uint32 * Node * Token * Node 
    |   Matrice of uint32 * uint32 * Node * Token * Node 
    |   Modulo of uint32 * uint32 * Node * Token * Node 
    |   Div of uint32 * uint32 * Node * Token * Node 
    |   FloorDiv of uint32 * uint32 * Node * Token * Node 
    |   UnaryPlus of uint32 * uint32 * Token * Node
    |   UnaryMinus of uint32 * uint32 * Token * Node
    |   BitInvert of uint32 * uint32 * Token * Node
    |   Power of uint32 * uint32 * Node * Token * Node 
    |   AtomExpr of uint32 * uint32 * Token * Node * Node array
    |   Tuple of uint32 * uint32 * Token * Node * Token
    |   List of uint32 * uint32 * Token * Node * Token
    |   Dictionary of uint32 * uint32 * Token * Node array * Token
    |   Set of uint32 * uint32 * Token * Node array * Token
    |   Name of uint32 * uint32 * Token
    |   Number of uint32 * uint32 * Token
    |   String of uint32 * uint32 * Token array
    |   Elipsis of uint32 * uint32 * Token
    |   None of uint32 * uint32 * Token
    |   True of uint32 * uint32 * Token
    |   False of uint32 * uint32 * Token
    |   TestListComp of uint32 * uint32 * Node array * Token array
    |   Call of uint32 * uint32 * Token * Node * Token
    |   Index of uint32 * uint32 * Token * Node * Token
    |   DotName of uint32 * uint32 * Token * Token
    |   SubscriptList of uint32 * uint32 * Node array * Token array
    |   Subscript of uint32 * uint32 * Node * Token * Node * Token * Node
    |   ExprList of uint32 * uint32 * Node array * Token array
    |   TestList of uint32 * uint32 * Node array * Token array
    |   DictionaryItem of uint32 * uint32 * Node * Token * Node
    |   SetItem of uint32 * uint32 * Node
    |   ArgList of uint32 * uint32 * Node array * Token array
    |   Argument of uint32 * uint32 * Node * Token * Node
    |   SyncCompFor of uint32 * uint32 * Token * Node * Token * Node * Node
    |   CompFor of uint32 * uint32 * Token * Node
    |   CompIf of uint32 * uint32 * Token * Node * Node
    |   Yield of uint32 * uint32 * Token * Node
    |   YieldFrom of uint32 * uint32 * Token * Token * Node
    |   SimpleStmt of uint32 * uint32 * Node array * Token array * Token
    |   PlusAssign of uint32 * uint32 * Node * Token * Node
    |   MinusAssign of uint32 * uint32 * Node * Token * Node
    |   MulAssign of uint32 * uint32 * Node * Token * Node
    |   MatriceAssign of uint32 * uint32 * Node * Token * Node
    |   DivAssign of uint32 * uint32 * Node * Token * Node
    |   ModuloAssign of uint32 * uint32 * Node * Token * Node
    |   BitAndAssign of uint32 * uint32 * Node * Token * Node
    |   BitOrAssign of uint32 * uint32 * Node * Token * Node
    |   BitXorAssign of uint32 * uint32 * Node * Token * Node
    |   ShiftLeftAssign of uint32 * uint32 * Node * Token * Node
    |   ShiftRightAssign of uint32 * uint32 * Node * Token * Node
    |   PowerAssign of uint32 * uint32 * Node * Token * Node
    |   FloorDivAssign of uint32 * uint32 * Node * Token * Node
    |   AnnAssign of uint32 * uint32 * Node * Token * Node * Token * Node
    |   ExprStmt of uint32 * uint32 * Node * Token array * Node array * Token
    |   TestListStarExpr of uint32 * uint32 * Node array * Token array
    |   Del of uint32 * uint32 * Token * Node
    |   Pass of uint32 * uint32 * Token
    |   Break of uint32 * uint32 * Token
    |   Continue of uint32 * uint32 * Token
    |   Return of uint32 * uint32 * Token * Node
    |   Raise of uint32 * uint32 * Token * Node * Token * Node
    |   ImportName of uint32 * uint32 * Token * Node
    |   ImportFrom of uint32 * uint32 * Token * Token array * Node * Token * Token * Node * Token
    |   ImportAsName of uint32 * uint32 * Token * Token * Token
    |   DottedAsName of uint32 * uint32 * Node * Token * Token
    |   ImportAsNames of uint32 * uint32 * Node array * Token array
    |   DottedAsNames of uint32 * uint32 * Node array * Token array
    |   DottedName of uint32 * uint32 * Token array * Token array
    |   Global of uint32 * uint32 * Token * Token array * Token array
    |   NonLocal of uint32 * uint32 * Token * Token array * Token array
    |   Assert of uint32 * uint32 * Token * Token * Token * Token
    |   Async of uint32 * uint32 * Token * Node
    |   If of uint32 * uint32 * Token * Node * Token * Node * Node array * Node
    |   Elif of uint32 * uint32 * Token * Node * Token * Node
    |   Else of uint32 * uint32 * Token * Token * Node
    |   While of uint32 * uint32 * Token * Node * Token * Node * Node
    |   Try of uint32 * uint32 * Token * Token * Node * Node array * Node * Token * Token * Node
    |   Except of uint32 * uint32 * Token * Node * Token * Token * Token * Node
    |   WithItem of uint32 * uint32 * Node * Token * Node
    |   With of uint32 * uint32 * Node array * Token array * Token * Token * Node
    |   Suite of uint32 * uint32 * Token * Token * Node array * Token
    |   Class of uint32 * uint32 * Token * Token * Token * Node * Token * Token * Node
    |   SingleInput of uint32 * uint32 * Node * Token
    |   FileInput of uint32 * uint32 * Token array * Node array * Token
    |   EvalInput of uint32 * uint32 * Node * Token array * Token
    |   Decorator of uint32 * uint32 * Token * Node * Token * Node * Token * Token
    |   Decorators of uint32 * uint32 * Node array
    |   Decorated of uint32 * uint32 * Node * Node
    |   AsyncFuncDef of uint32 * uint32 * Token * Node
    |   FuncDef of uint32 * uint32 * Token * Token * Node * Token * Token * Token * Token * Node
    |   Parameters of uint32 * uint32 * Token * Node * Token
    |   TypedArgsList of uint32 * uint32 * Node array * Token array * Token array * Token * Node * Token * Node
    |   TfpDefAssign of uint32 * uint32 * Node * Token * Node
    |   TfpDef of uint32 * uint32 * Token *Token * Node
    |   VarArgsList of uint32 * uint32 * Node array * Token array * Token * Token * Token * Token
    |   VfpDefAssign of uint32 * uint32 * Token * Token * Node
    |   FuncBodySuite of uint32 * uint32 * Token * Token * Token * Token * Node array * Token
    |   FuncTypeInput of uint32 * uint32 * Node * Token array * Token
    |   FuncType of uint32 * uint32 * Token * Node * Token * Token * Node
    |   TypeList of uint32 * uint32 * Node array * Token array * Token * Node * Token * Node
