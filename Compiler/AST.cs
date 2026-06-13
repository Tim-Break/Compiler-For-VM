using System.Formats.Asn1;
using System.Text;
static class StringUtil
{
    public static string Repeat(this string input, int count)
    {
        var result = new StringBuilder();
        for (int i = 0; i < count; i++)
        {
            result.Append(input);
        }
        return result.ToString();
    }
}

// ACTUAL CODE

public class ASTTree
{
    public List<ASTNode> statements = new();

    public void Print()
    {
        for (int i=0; i<statements.Count; i++)
        {
            statements[i].Print(1, i==statements.Count-1);
        }
    }
}

// AST nodes
public class ASTNode
{
    public virtual void Print(int ident, bool last) {}
}

public class AssemblyNode : ASTNode // TODO
{
}

public class IntegerNode : ASTNode
{
    public int value;

    public IntegerNode(int value)
    {
        this.value = value;
    }

    public override void Print(int ident, bool last)
    {
        Console.WriteLine("|   ".Repeat(ident-1)+(last ? "└── " : "├── ")+value.ToString());
    }
}

public class BooleanNode : ASTNode
{
    public bool value;

    public BooleanNode(bool value)
    {
        this.value = value;
    }

    public override void Print(int ident, bool last)
    {
        Console.WriteLine("|   ".Repeat(ident-1)+(last ? "└── " : "├── ")+value.ToString());
    }
}

public class VariableNode : ASTNode
{
    public string name;

    public VariableNode(string name)
    {
        this.name = name;
    }
}

public class DeclareNode : ASTNode
{
    public string typename;
    public string name;
    public ASTNode value;

    public DeclareNode(string typename, string name, ASTNode value)
    {
        this.typename = typename;
        this.name = name;
        this.value = value;
    }
}

public class AssignNode : ASTNode
{
    public string name;
    public ASTNode value;

    public AssignNode(string name, ASTNode value)
    {
        this.name = name;
        this.value = value;
    }
}

public class BinOpNode : ASTNode
{
    public ASTNode left;
    public ASTNode right;
    public string op;

    public BinOpNode(ASTNode left, ASTNode right, string op)
    {
        this.left = left;
        this.right = right;
        this.op = op;
    }

    public override void Print(int ident, bool last)
    {
        Console.WriteLine("|   ".Repeat(ident-1)+(last ? "└── " : "├── ")+op);
        left.Print(ident+1,false);
        right.Print(ident+1,true);
    }
}

public class UnaryOpNode : ASTNode
{
    public ASTNode value;
    public string op;

    public UnaryOpNode(ASTNode value, string op)
    {
        this.value = value;
        this.op = op;
    }

    public override void Print(int ident, bool last)
    {
        Console.WriteLine("|   ".Repeat(ident-1)+(last ? "└── " : "├── ")+op);
        value.Print(ident+1,false);
    }
}

public class BlockNode : ASTNode
{
    public ASTNode[] statements;

    public BlockNode(ASTNode[] statements)
    {
        this.statements = statements;
    }
}

public class IfNode : ASTNode
{
    public ASTNode condition;
    public ASTNode then;
    public ASTNode? _else;

    public IfNode(ASTNode condition, ASTNode then, ASTNode? _else = null)
    {
        this.condition = condition;
        this.then = then;
        this._else = _else;
    }
}

public class WhileNode : ASTNode
{
    public ASTNode condition;
    public ASTNode body;
    public ASTNode? _else;

    public WhileNode(ASTNode condition, ASTNode body, ASTNode? _else = null)
    {
        this.condition = condition;
        this.body = body;
        this._else = _else;
    }
}

public class BreakNode : ASTNode
{
    
}

public class ContinueNode : ASTNode
{
    
}