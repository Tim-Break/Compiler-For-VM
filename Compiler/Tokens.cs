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

    Operator,   // +, -, *, /, %, &, |, ^, ~, &&, ||, !, ==, <=, >=, <, >, !=
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

public class Token
{
    public readonly TokenType type;
    public readonly string value = "";

    public Token(TokenType type, string value="")
    {
        this.type = type;
        this.value = value;
    }

    public string ToPrint()
    {
        return type.ToString() + ": {" + value + "}";
    }
}