
namespace PythonCore.Runtime.Parser

type Node =
    |   Empty
    |   NamedExpr of uint32 * uint32 * Node * Token * Node
    |   Test of uint32 * uint32 * Node * Token * Node * Token * Node
    |   Lambda of uint32 * uint32 * Token * Node * Token * Node * bool
    |   OrTest of uint32 * uint32 * Node * Token * Node 
    