public class Lexer
{
    private static Token ERROR = new Token(TokenType.ERROR);

    private Diagnostic diagnostic;

    private string text = "";
    private int line = 0;
    private int symbol = 0;
    private char? current = null;   // null means EOF
    private int index = 0;

    public Lexer(Diagnostic diagnostic)
    {
        this.diagnostic = diagnostic;
    }

    public void Start(string text)
    {
        this.text = text;
        index = -1;
        line = 0;
        symbol = -1;
        Advance();
    }

    private void Advance()
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

    private void SkipWhiteSpace()
    {
        while (current != null && char.IsWhiteSpace((char)current))
        {
            Advance();
        }
    }

    private Token ReadIdentifer()
    {
        string identifer = "";
        while (current != null && (char.IsAsciiLetter((char)current) || char.IsAsciiDigit((char)current)))
        {
            identifer += current;
            Advance();
        }
        return Keywords.ParseIdent(identifer);
    }

    private Token ReadNumber()
    {
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
        }
        return new Token(TokenType.Number, number);
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
            
            if (current == '+' ||
                current == '-')
            {
                char prev = (char)current;
                Advance();
                if (current == prev && current != null) return new Token(TokenType.ModifyOp, ""+prev+prev);
                if (current == '=' && current != null) return new Token(TokenType.ModifyOp, ""+prev);
                return new Token(TokenType.Operator, ""+prev);
            }
            if (current == '*' ||
                current == '/' ||
                current == '%' ||
                current == '^' ||
                current == '~')
            {
                char prev = (char)current;
                Advance();
                if (current == '=' && current != null) return new Token(TokenType.ModifyOp, ""+prev);
                return new Token(TokenType.Operator, ""+prev);
            }
            if (current == '&' ||
                current == '|')
            {
                char prev = (char)current;
                Advance();
                if (current == '=' && current != null) return new Token(TokenType.ModifyOp, ""+prev);
                if (current == prev && current != null) return new Token(TokenType.Operator, ""+prev+prev);
                return new Token(TokenType.Operator, ""+prev);
            }
            if (current == '!' ||
                current == '>' ||
                current == '<')
            {
                char prev = (char)current;
                Advance();
                if (current == '=' && current != null) return new Token(TokenType.Operator, ""+prev+"=");
                return new Token(TokenType.Operator, ""+prev);
            }
            if (current == '=')
            {
                Advance();
                if (current == '=' && current != null) return new Token(TokenType.Operator, "==");
                return new Token(TokenType.Assign);
            }

            diagnostic.ReportError(
                new Report("SyntaxError", "Unknown symbol", line, symbol)
            );
            return ERROR;
        }
        return new Token(TokenType.EOF);
    }

}