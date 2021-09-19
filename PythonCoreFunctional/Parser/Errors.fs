
namespace PythonCore.Runtime.Parser


exception LexicalError of string * uint32

exception SyntaxError of Token * string * uint32
