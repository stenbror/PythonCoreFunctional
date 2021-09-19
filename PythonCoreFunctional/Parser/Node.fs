
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
    