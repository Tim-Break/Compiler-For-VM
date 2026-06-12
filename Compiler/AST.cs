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

public class Assembly : ASTNode // TODO
{
}

public class Integer : ASTNode
{
    public int value;

    public Integer(int value)
    {
        this.value = value;
    }

    public override void Print(int ident, bool last)
    {
        Console.WriteLine("|   ".Repeat(ident-1)+(last ? "└── " : "├── ")+value.ToString());
    }
}

public class Boolean : ASTNode
{
    public bool value;

    public Boolean(bool value)
    {
        this.value = value;
    }

    public override void Print(int ident, bool last)
    {
        Console.WriteLine("|   ".Repeat(ident-1)+(last ? "└── " : "├── ")+value.ToString());
    }
}

public class Variable : ASTNode
{
    public string name;

    public Variable(string name)
    {
        this.name = name;
    }
}

public class Declare : ASTNode
{
    public string typename;
    public string name;
    public ASTNode value;

    public Declare(string typename, string name, ASTNode value)
    {
        this.typename = typename;
        this.name = name;
        this.value = value;
    }
}

public class Assign : ASTNode
{
    public string name;
    public ASTNode value;

    public Assign(string name, ASTNode value)
    {
        this.name = name;
        this.value = value;
    }
}

public class BinOp : ASTNode
{
    public ASTNode left;
    public ASTNode right;
    public string op;

    public BinOp(ASTNode left, ASTNode right, string op)
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

public class UnaryOp : ASTNode
{
    public ASTNode value;
    public string op;

    public UnaryOp(ASTNode value, string op)
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

public class Block : ASTNode
{
    public List<ASTNode> statements;

    public Block(List<ASTNode> statements)
    {
        this.statements = statements;
    }
}