public class Keywords
{
    private static Dictionary<string, Token> keywords = new(100)
    {
        ["if"] = new Token(TokenType.If),
        ["elif"] = new Token(TokenType.Elif),
        ["else"] = new Token(TokenType.Else),

        ["while"] = new Token(TokenType.While),
        ["for"] = new Token(TokenType.While),
        ["break"] = new Token(TokenType.Break),
        ["continue"] = new Token(TokenType.Continue),

        ["asm"] = new Token(TokenType.Asm),

        // Boolean values
        ["true"] = new Token(TokenType.Bool, "true"),
        ["false"] = new Token(TokenType.Bool, "false")
    };

    public static Token ParseIdent(string ident)
    {
        if (keywords.TryGetValue(ident, out Token? keyword))
            return keyword;
        else
            return new Token(TokenType.Ident, ident);
    }
}