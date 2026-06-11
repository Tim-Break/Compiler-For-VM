string code = """
a = 1.5
a += 10
a++
if a == 11
""";

Diagnostic diagnostic = new Diagnostic();
Lexer lexer = new Lexer(diagnostic);

lexer.Start(code);

while (true)
{
    Token token = lexer.ReadToken();
    Console.WriteLine(token.ToPrint());
    if (token.type == TokenType.EOF) break;
}