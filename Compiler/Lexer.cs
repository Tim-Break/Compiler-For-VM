public class Lexer
{
    static Token ERROR = new Token(TokenType.ERROR);

    Diagnostic diagnostic;

    string text = "";
    int line = 0;
    int symbol = 0;
    char? current = null;   // null means EOF
    int index = 0;

    public Lexer(Diagnostic diagnostic)
    {
        this.diagnostic = diagnostic;
    }

    public void Start(string text)
    {
        this.text = text;
        index = -1;
        line = 1;
        symbol = -1;
        Advance();
    }

    void Advance()
    {
        index++;
        if (index == text.Length)
            current = null;
        else
            current = text[index];
        
        symbol++;
        if (current == '\n') {
            line++;
            symbol=0;
        }
    }

    void SkipWhiteSpace()
    {
        while (current != null && char.IsWhiteSpace((char)current))
        {
            Advance();
        }
    }

    Token ReadIdentifer()
    {
        int line_ = line;
        int symbol_ = symbol;
        string identifer = "";
        while (current != null && (char.IsAsciiLetter((char)current) || char.IsAsciiDigit((char)current)))
        {
            identifer += current;
            Advance();
        }
        return Keywords.ParseIdent(identifer, line_, symbol_);
    }

    Token ReadNumber()
    {
        int line_ = line;
        int symbol_ = symbol;
        string number = "";
        bool dot = false;
        while (current != null && (current == '.' || char.IsAsciiDigit((char)current)))
        {
            if (current == '.' && dot)
            {
                diagnostic.ReportError(
                    new Report("SyntaxError", "Bad number", line, symbol)
                );
                return ERROR;
            }
            number += current;
            Advance();
        }
        return new Token(TokenType.Number, number, line_, symbol_);
    }

    public Token ReadToken()
    {
        while (current != null)
        {
            if (char.IsWhiteSpace((char)current))
            {
                SkipWhiteSpace();
                continue;
            }

            if (char.IsAsciiLetter((char)current))
                return ReadIdentifer();
            
            if (current == '.' || char.IsAsciiDigit((char)current))
                return ReadNumber();
            
            if (current == ';')
            {
                Advance();
                return new Token(TokenType.Colon);
            }

            if (current == ',')
            {
                Advance();
                return new Token(TokenType.Comma);
            }
            
            if (current == '+' ||
                current == '-')
            {
                char prev = (char)current;
                Advance();
                if (current == prev) {Advance(); return new Token(TokenType.ModifyOp, ""+prev+prev);}
                if (current == '=') {Advance(); return new Token(TokenType.ModifyOp, ""+prev+"=");}
                return new Token(TokenType.Operator, ""+prev);
            }
            if (current == '*')
            {
                Advance();
                if (current == '*') {Advance(); return new Token(TokenType.Operator, "**");}
                if (current == '=') {Advance(); return new Token(TokenType.ModifyOp, "*=");}
                return new Token(TokenType.Operator, "*");
            }
            if (current == '/' ||
                current == '%' ||
                current == '^' ||
                current == '~')
            {
                char prev = (char)current;
                Advance();
                if (current == '=') {Advance(); return new Token(TokenType.ModifyOp, ""+prev+"=");}
                return new Token(TokenType.Operator, ""+prev);
            }
            if (current == '&' ||
                current == '|')
            {
                char prev = (char)current;
                Advance();
                if (current == '=') {Advance(); return new Token(TokenType.ModifyOp, ""+prev+"=");}
                if (current == prev) {Advance(); return new Token(TokenType.Operator, ""+prev+prev);}
                return new Token(TokenType.Operator, ""+prev);
            }
            if (current == '>' ||
                current == '<')
            {
                char prev = (char)current;
                Advance();
                if (current == prev) {Advance(); return new Token(TokenType.Operator, ""+prev+prev);}
                if (current == '=') {Advance(); return new Token(TokenType.Operator, ""+prev+"=");}
                return new Token(TokenType.Operator, ""+prev);
            }
            if (current == '!')
            {
                Advance();
                if (current == '=') {Advance(); return new Token(TokenType.Operator, "!=");}
                return new Token(TokenType.Operator, "!");
            }
            if (current == '=')
            {
                Advance();
                if (current == '=') {Advance(); return new Token(TokenType.Operator, "==");}
                return new Token(TokenType.Assign);
            }
            
            if (current == '(')
            {
                Advance();
                return new Token(TokenType.LParen);
            }
            if (current == ')')
            {
                Advance();
                return new Token(TokenType.RParen);
            }

            if (current == '{')
            {
                Advance();
                return new Token(TokenType.LBrace);
            }
            if (current == '}')
            {
                Advance();
                return new Token(TokenType.RBrace);
            }

            diagnostic.ReportError(
                new Report("SyntaxError", $"Unknown symbol '{current}'", line, symbol)
            );
            return ERROR;
        }
        return new Token(TokenType.EOF);
    }

}