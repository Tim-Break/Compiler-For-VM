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
                Advance();
                return new VariableNode(token.value);
            }
            case TokenType.Bool:
            {
                Advance();
                return new BooleanNode(token.value == "true");
            }
            case TokenType.Number:
            {
                Advance();
                return new IntegerNode(int.Parse(token.value));
            }
            case TokenType.LParen:
            {
                Advance();
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
            Advance();
            node = new BinOpNode(node, Expr_Pow(), "**");
        }
        return node;
    }

    ASTNode Expr_Unary()
    {
        if (current.type == TokenType.Operator &&
            (current.value == "-" || current.value == "~"))
        {
            string op = current.value;
            Advance();
            return new UnaryOpNode(Expr_Unary(), op);
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
            Advance();
            node = new BinOpNode(node, Expr_Unary(), op);
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
            Advance();
            node = new BinOpNode(node, Expr_MulDiv(), op);
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
            Advance();
            node = new BinOpNode(node, Expr_AddSub(), op);
        }
        return node;
    }

    ASTNode Expr_BAnd()
    {
        ASTNode node = Expr_BShift();
        while (current.type == TokenType.Operator && current.value == "&")
        {
            Advance();
            node = new BinOpNode(node, Expr_BShift(), "&");
        }
        return node;
    }

    ASTNode Expr_Xor()
    {
        ASTNode node = Expr_BAnd();
        while (current.type == TokenType.Operator && current.value == "^")
        {
            Advance();
            node = new BinOpNode(node, Expr_BAnd(), "^");
        }
        return node;
    }

    ASTNode Expr_BOr()
    {
        ASTNode node = Expr_Xor();
        while (current.type == TokenType.Operator && current.value == "|")
        {
            Advance();
            node = new BinOpNode(node, Expr_Xor(), "|");
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
            Advance();
            node = new BinOpNode(node, Expr_BOr(), op);
        }
        return node;
    }

    ASTNode Expr_Not()
    {
        if (current.type == TokenType.Operator && current.value == "!")
        {
            Advance();
            return new UnaryOpNode(Expr_Not(), "!");
        }
        return Expr_Cmp();
    }

    ASTNode Expr_And()
    {
        ASTNode node = Expr_Not();
        while (current.type == TokenType.Operator && current.value == "&&")
        {
            Advance();
            node = new BinOpNode(node, Expr_Not(), "&&");
        }
        return node;
    }

    ASTNode Expression()
    {
        ASTNode node = Expr_And();
        while (current.type == TokenType.Operator && current.value == "||")
        {
            Advance();
            node = new BinOpNode(node, Expr_And(), "||");
        }
        return node;
    }

    ASTNode Block()
    {
        Eat(TokenType.LBrace);
        List<ASTNode> statements = new();
        while (current.type != TokenType.RBrace)
        {
            statements.Add(Statement());
        }
        Advance();
        return new BlockNode(statements.ToArray());
    }

    ASTNode If()
    {
        Advance();

        Eat(TokenType.LParen);
        ASTNode condition = Expression();
        Eat(TokenType.RParen);

        ASTNode then = Statement();

        ASTNode? _else = null;
        if (current.type == TokenType.CKeyword && current.value == "else")
            _else = Block();
        
        return new IfNode(condition, then, _else);
    }

    ASTNode While()
    {
        Advance();

        Eat(TokenType.LParen);
        ASTNode condition = Expression();
        Eat(TokenType.RParen);

        ASTNode body = Statement();

        ASTNode? _else = null;
        if (current.type == TokenType.CKeyword && current.value == "else")
            _else = Block();
        
        return new WhileNode(condition, body, _else);
    }

    ASTNode Statement()
    {
        if (current.type == TokenType.CKeyword)
        {
            if (current.value == "if") return If();
            if (current.value == "while") return While();
            throw new Exception("Unknown construction keyword");
        }
        if (current.type == TokenType.LBrace)
            return Block();
        if (current.type == TokenType.Ident)
        {
            if (next.type == TokenType.Ident)
            {
                string typename = current.value;
                Advance();
                string name = current.value;
                Advance();
                Eat(TokenType.Assign);
                ASTNode value = Expression();
                Eat(TokenType.Colon);
                return new DeclareNode(typename, name, value);
            } else if (next.type == TokenType.Assign ||
                       next.type == TokenType.ModifyOp)
            {
                string name = current.value;
                Advance();
                if (current.type == TokenType.ModifyOp && (
                    current.value == "++" ||
                    current.value == "--"
                ))
                {
                    return new AssignNode(name,
                                          new BinOpNode(new VariableNode(name),
                                                        new IntegerNode(1), current.value[1..]));
                } else
                {
                    string op = current.value;
                    Advance();
                    return new AssignNode(name,
                                          new BinOpNode(new VariableNode(name),
                                                        Expression(), op));
                }
            }
        }
        diagnostic.ReportError(
            new Report("SyntaxError", $"Excepted statement, got {current}", current.line, current.symbol)
        );
        Console.WriteLine("!!!CAUTION 'null!' RUNNED!!!");
        return null!;
    }

    public ASTTree Parse()
    {
        ASTTree tree = new ASTTree();

        // Parsing
        tree.statements.Add(Expression());

        return tree;
    }
}