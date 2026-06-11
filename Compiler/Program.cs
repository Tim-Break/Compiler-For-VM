string code = """
a = 1.5;
a += 10;
a++;
if (a == 11 || a <= 0 && true && !false) {
    a = 10000;
    print(a);
}
asm {
mov r0, r1
load r2, r1
}
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