using System.Reflection.Metadata.Ecma335;

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
                new Report("SyntaxError", $"Excepted '{Token.tokenStrings.GetValueOrDefault(type, "**ERROR**")}', got '{current.ToString()}'.", current.line, current.symbol)
            );
            // Need to do something to stop parsing(mb throw exception)
            // UPD: in diagnostic i'm already throwing exception
        }
    }

    ASTNode Expr_Atom()
    {
        Token token = current;
        switch (current.type)
        {
            case TokenType.Ident:
            {
                Eat(TokenType.Ident);
                return new Variable(token.value);
            }
            case TokenType.Bool:
            {
                Eat(TokenType.Bool);
                return new Boolean(token.value == "true");
            }
            case TokenType.Number:
            {
                Eat(TokenType.Number);
                return new Integer(int.Parse(token.value));
            }
            case TokenType.LParen:
            {
                Eat(TokenType.LParen);
                ASTNode node = Expression();
                Eat(TokenType.RParen);
                return node;
            }
            default:
                diagnostic.ReportError(
                    new Report("SyntaxError", $"Excepted expression, got '{token}'")
                );
                Console.WriteLine("!!!CAUTION 'null!' RUNNED!!!");
                return null!; // '!' is not very good here, but this code is never run
        }
    }

    ASTNode Expr_Pow()
    {
        ASTNode node = Expr_Atom();
        while (current.type == TokenType.Operator && current.value == "**")
        {
            Eat(TokenType.Operator);
            node = new BinOp(node, Expr_Pow(), "**");
        }
        return node;
    }

    ASTNode Expr_Unary()
    {
        if (current.type == TokenType.Operator &&
            (current.value == "-" || current.value == "~"))
        {
            string op = current.value;
            Eat(TokenType.Operator);
            return new UnaryOp(Expr_Unary(), op);
        }
        return Expr_Pow();
    }
    
    ASTNode Expr_MulDiv()
    {
        ASTNode node = Expr_Unary();
        while (current.type == TokenType.Operator &&
            (current.value == "*" || current.value == "/" || current.value == "%"))
        {
            string op = current.value;
            Eat(TokenType.Operator);
            node = new BinOp(node, Expr_Unary(), op);
        }
        return node;
    }

    ASTNode Expr_AddSub()
    {
        ASTNode node = Expr_MulDiv();
        while (current.type == TokenType.Operator &&
            (current.value == "+" || current.value == "-"))
        {
            string op = current.value;
            Eat(TokenType.Operator);
            node = new BinOp(node, Expr_MulDiv(), op);
        }
        return node;
    }

    ASTNode Expr_BShift()
    {
        ASTNode node = Expr_AddSub();
        while (current.type == TokenType.Operator &&
            (current.value == "<<" || current.value == ">>"))
        {
            string op = current.value;
            Eat(TokenType.Operator);
            node = new BinOp(node, Expr_AddSub(), op);
        }
        return node;
    }

    ASTNode Expr_BAnd()
    {
        ASTNode node = Expr_BShift();
        while (current.type == TokenType.Operator && current.value == "&")
        {
            Eat(TokenType.Operator);
            node = new BinOp(node, Expr_BShift(), "&");
        }
        return node;
    }

    ASTNode Expr_Xor()
    {
        ASTNode node = Expr_BAnd();
        while (current.type == TokenType.Operator && current.value == "^")
        {
            Eat(TokenType.Operator);
            node = new BinOp(node, Expr_BAnd(), "^");
        }
        return node;
    }

    ASTNode Expr_BOr()
    {
        ASTNode node = Expr_Xor();
        while (current.type == TokenType.Operator && current.value == "|")
        {
            Eat(TokenType.Operator);
            node = new BinOp(node, Expr_Xor(), "|");
        }
        return node;
    }

    ASTNode Expr_Cmp()
    {
        ASTNode node = Expr_BOr();
        while (current.type == TokenType.Operator &&
            (current.value == "==" ||
            current.value == "<" ||
            current.value == ">" ||
            current.value == "<=" ||
            current.value == ">=" ||
            current.value == "!="))
        {
            string op = current.value;
            Eat(TokenType.Operator);
            node = new BinOp(node, Expr_BOr(), op);
        }
        return node;
    }

    ASTNode Expr_Not()
    {
        if (current.type == TokenType.Operator && current.value == "!")
        {
            Eat(TokenType.Operator);
            return new UnaryOp(Expr_Not(), "!");
        }
        return Expr_Cmp();
    }

    ASTNode Expr_And()
    {
        ASTNode node = Expr_Not();
        while (current.type == TokenType.Operator && current.value == "&&")
        {
            Eat(TokenType.Operator);
            node = new BinOp(node, Expr_Not(), "&&");
        }
        return node;
    }

    ASTNode Expression()
    {
        ASTNode node = Expr_And();
        while (current.type == TokenType.Operator && current.value == "||")
        {
            Eat(TokenType.Operator);
            node = new BinOp(node, Expr_And(), "||");
        }
        return node;
    }

    public ASTTree Parse()
    {
        ASTTree tree = new ASTTree();

        // Parsing
        tree.statements.Add(Expression());

        return tree;
    }
}