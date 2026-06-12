public struct Report
{
    public readonly string name;
    public readonly string message;
    public readonly int line;
    public readonly int symbol;

    public Report(string name, string message, int line=-1, int symbol=-1)
    {
        this.name = name;
        this.message = message;
        this.line = line;
        this.symbol = symbol;
    }

    public override readonly string ToString()
    {
        return $"{name} at Ln {line}, Col {symbol} : {message}";
    }
}

public class Diagnostic
{
    public readonly List<Report> errors = new();
    public readonly List<Report> warnings = new();

    public void ReportError(Report error)
    {
        errors.Add(error);
        // Just throw exception
        throw new Exception(error.ToString());
    }

    public void ReportWarning(Report warn)
    {
        warnings.Add(warn);
    }

    public void PrintErrors()
    {
        for (int i=0; i<errors.Count; i++)
        {
            Console.WriteLine("Error:\n\t"+errors[i].ToString());
        }
    }

    public void PrintWarnings()
    {
        for (int i=0; i<warnings.Count; i++)
        {
            Console.WriteLine("Warning:\n\t"+warnings[i].ToString());
        }
    }
}