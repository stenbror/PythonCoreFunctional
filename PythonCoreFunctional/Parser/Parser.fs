
namespace PythonCore.Runtime.Parser

[<Struct>]
type ParseState =
    {
        mutable isInteractive : bool
        mutable FlowLevel : uint32
        mutable FuncLevel : uint32
    }
    with
        static member Init =
            {
                isInteractive = false
                FlowLevel = 0u
                FuncLevel = 0u
            }
