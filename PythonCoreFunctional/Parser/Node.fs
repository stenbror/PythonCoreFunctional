
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
    
    