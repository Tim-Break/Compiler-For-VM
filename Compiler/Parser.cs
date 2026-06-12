public class Parser
{
    Diagnostic diagnostic;

    Lexer lexer;
    
    Token current;
    Token next;

    public Parser(Diagnostic diagnostic, Lexer lexer)
    {
        this.diagnostic = diagnostic;
        this.lexer = lexer;
        current = lexer.ReadToken();
        next = lexer.ReadToken();
    }

    void Advance()
    {
        current = next;
        next = lexer.ReadToken();
    }

    void Eat(TokenType type)
    {
        if (current.type == type) Advance();
        else
        {
            diagnostic.ReportError(
                new Report("SyntaxError", $"Excepted '{Token.tokenStrings.GetValueOrDefault(type, "**ERROR**")}', got '{current.ToPrint()}'.", current.line, current.symbol)
            );
            // Need to do something to stop parsing(mb throw exception)
        }
    }

    

    public ASTTree Parse()
    {
        ASTTree tree = new ASTTree();

        // Parsing

        return tree;
    }
}