public class Compiler
{
    int ifCount = 0;

    string CompileNode(ASTNode node)
    {
        {if (node is IntegerNode n)
            return $"mov r1, {n.value}\n";}
        {if (node is BooleanNode n)
            return $"mov r1, {(n.value ? 1 : 0)}\n";}
        {if (node is VariableNode n)
            return $"lea r1, _var_{n.name}\nload r1, r1\n";}
        {if (node is DeclareNode n)
            return $"_var_{n.name}:\ndb 0\n{CompileNode(n.value)}lea r2, _var_{n.name}\nstore r2, r1\n";}
        {if (node is AssignNode n)
            return $"{CompileNode(n.value)}lea r2, _var_{n.name}\nstore r2, r1\n";}
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
                return $"{right}mov r2, r1\n{left}{op}\n";
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
                return $"{value}{op}\n";
            }}
        {if (node is IfNode n)
            {
                string condition = CompileNode(n.condition);
                string elseTarget = "_if-" + (n._else==null ? "after" : "else") + $"_{ifCount}";

                string then = CompileNode(n.then);
                string thenPostfix = n._else==null ? "" : $"jmp _if-after_{ifCount}\n";

                string _else = "";
                if (n._else != null)
                    _else += $"_if-else_{ifCount}:\n" + CompileNode(n._else);

                ifCount++;
                return $"{condition}cmp r1, 1\njnz {elseTarget}\n{then}{thenPostfix}{_else}_if-after_{ifCount}:\n";
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