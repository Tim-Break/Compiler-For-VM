string code = """100*(10+1)""";

Diagnostic diagnostic = new Diagnostic();
Lexer lexer = new Lexer(diagnostic);

lexer.Start(code);

Parser parser = new Parser(diagnostic, lexer);
parser.Parse().Print();