
namespace PythonCore.Runtime.Parser


exception LexicalError of uint32 * string

exception SyntaxError of uint32 * Token * string
