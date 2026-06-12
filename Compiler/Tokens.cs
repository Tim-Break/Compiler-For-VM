public enum TokenType
{
    If,         // if
    Elif,       // elif
    Else,       // else

    While,      // while
    For,        // for
    Break,      // break
    Continue,   // continue

    Asm,        // asm

    Ident,      // any identifer

    Number,     // 1, 2.5 e.t.c.
    Bool,       // true, false

    Operator,   // +, -, *, /, %, **, <<, >>, &, |, ^, ~, &&, ||, !, ==, <=, >=, <, >, !=
    Assign,     // =
    ModifyOp,   // ++, --, +=, -=, *=, /=, %=, &=, |=, ^=, ~=

    LParen,     // (
    RParen,     // )

    LBrace,     // {
    RBrace,     // }

    Comma,      // ,
    Colon,      // ;

    EOF,        // End of file

    ERROR       // Bad input error. Lexer throws it when gets unknown symbol.
}

public struct Token
{
    public static Dictionary<TokenType, string> tokenStrings = new()
    {
        [TokenType.Ident] = "identifer",
        [TokenType.Number] = "number",
        [TokenType.Bool] = "boolean",
        [TokenType.Operator] = "operator",
        [TokenType.ModifyOp] = "operator",

        [TokenType.If] = "if",
        [TokenType.Elif] = "elif",
        [TokenType.Else] = "else",
        
        [TokenType.While] = "while",
        [TokenType.For] = "for",
        [TokenType.Break] = "break",
        [TokenType.Continue] = "continue",

        [TokenType.Asm] = "asm",

        [TokenType.Assign] = "=",

        [TokenType.LParen] = "(",
        [TokenType.RParen] = ")",

        [TokenType.LBrace] = "{",
        [TokenType.RBrace] = "}",

        [TokenType.Comma] = ",",
        [TokenType.Colon] = ";",

        [TokenType.EOF] = "*EOF*",

        [TokenType.ERROR] = "**SYNTAX ERROR**"
    };

    public readonly TokenType type;
    public readonly string value = "";
    public readonly int line;
    public readonly int symbol;

    public Token(TokenType type, string value="", int line=-1, int symbol=-1)
    {
        this.type = type;
        this.value = value;
        this.line = line;
        this.symbol = symbol;
    }

    public string ToPrint()
    {
        if (type == TokenType.Ident ||
            type == TokenType.Number ||
            type == TokenType.Bool ||
            type == TokenType.Operator ||
            type == TokenType.ModifyOp)
            return value;
        else
            return tokenStrings.GetValueOrDefault(type, "***ERROR***");
    }
}