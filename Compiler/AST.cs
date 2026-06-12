public class ASTTree
{
    public List<ASTNode> statements = new();
}

// AST nodes
public class ASTNode
{
    
}

public class Assembly : ASTNode {}

public class Int : ASTNode
{
    public int value;

    public Int(int value)
    {
        this.value = value;
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
}

public class Block : ASTNode
{
    public List<ASTNode> statements;

    public Block(List<ASTNode> statements)
    {
        this.statements = statements;
    }
}