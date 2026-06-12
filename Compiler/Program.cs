string code = """ """;

Diagnostic diagnostic = new Diagnostic();
Lexer lexer = new Lexer(diagnostic);

lexer.Start(code);

Parser parser = new Parser(diagnostic, lexer);
parser.Parse().Print();