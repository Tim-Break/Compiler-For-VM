public class Keywords
{
    static Dictionary<string, Token> keywords = new(100)
    {
        /*["if"] = new Token(TokenType.If),
        ["else"] = new Token(TokenType.Else),

        ["while"] = new Token(TokenType.While),
        ["for"] = new Token(TokenType.While),
        ["break"] = new Token(TokenType.Break),
        ["continue"] = new Token(TokenType.Continue),

        ["asm"] = new Token(TokenType.Asm),*/
        ["if"] = new Token(TokenType.CKeyword, "if"),
        ["else"] = new Token(TokenType.CKeyword, "else"),

        ["while"] = new Token(TokenType.CKeyword, "while"),
        ["for"] = new Token(TokenType.CKeyword, "for"),
        ["break"] = new Token(TokenType.TKeyword, "break"),
        ["continue"] = new Token(TokenType.TKeyword, "continue"),

        ["asm"] = new Token(TokenType.CKeyword, "asm"),

        // Boolean values
        ["true"] = new Token(TokenType.Bool, "true"),
        ["false"] = new Token(TokenType.Bool, "false")
    };

    public static Token ParseIdent(string ident, int line, int symbol)
    {
        if (keywords.TryGetValue(ident, out Token keyword))
            return new Token(keyword.type, keyword.value, line, symbol);
        else
            return new Token(TokenType.Ident, ident, line, symbol);
    }
}