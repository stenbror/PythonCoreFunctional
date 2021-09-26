# PythonCoreFunctional

This is another project of mine to produce a Python parser that is written in a functional style for Python 3.10 grammar using Dot net 6 and ofcourse fsharp language. I am writing the unittests as i implement the code for the parser and each grammar rule can be used as a start rule if you really want to do that. This helps testing each components and does not impact the final products stability when only using start rules allowed in Python.

I am trying to build this parser a little like the Roslyn C# compiler from Microsoft with Nodes, Tokens and Trivias. The idea is to be able to refactor Python code in your own program by using this as a framework for Python.

I have written a lot of different Python Parsers through the times after my master thesis which was Python in C++. This will be the final version, i prommise and it will be in fsharp and dot net only, but i am developing on a Linux Server and Mac M1 notebook, so i am designing it to be multi platform.
