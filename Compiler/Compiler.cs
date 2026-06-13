public class Compiler
{
    string CompileNode(ASTNode node)
    {
        {if (node is IntegerNode n)
            return $"mov r1, {n.value}\n";}
        {if (node is BooleanNode n)
            return $"mov r1, {(n.value ? 1 : 0)}\n";}
        {if (node is BinOpNode n)
            {
                string left = CompileNode(n.left);
                string right = CompileNode(n.right);
                string op = n.op switch
                {
                    "+" => "add r1, r2",
                    "-" => "sub r1, r2",
                    "*" => "mul r1, r2",
                    "/" => "div r1, r2",
                    "%" => "mov r3, r1\ndiv r3, r2\nmul r3, r2\nsub r1, r3",
                    // TODO: Add POW operator compilation
                    "<<"=> "shl r1, r2",
                    ">>"=> "shr r1, r2",
                    "&" => "and r1, r2",
                    "|" => "or r1, r2",
                    "^" => "xor r1, r2",
                    "&&"=> "and r1, r2",
                    "||"=> "or r1, r2",
                    // "=="=> "xor r1, r2",
                    _ => throw new Exception($"Unknown operator '{n.op}'!")
                };
                return right + "mov r2, r1\n" + left + op + "\n";
            }}
        {if (node is UnaryOpNode n)
            {
                string value = CompileNode(n.value);
                string op = n.op switch
                {
                    "-" => "mul r1, -1",
                    "~" => "not r1",
                    //"!" => "mul r1, r2",
                    _ => throw new Exception($"Unknown operator '{n.op}'!")
                };
                return value + op + "\n";
            }}
        throw new Exception("Unknown node!");
    }

    public string Compile(ASTTree ast)
    {
        string result = "";
        foreach (ASTNode node in ast.statements)
        {
            result += CompileNode(node);
        }
        return result;
    }
}