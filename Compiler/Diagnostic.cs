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
}

public class Diagnostic
{
    public readonly List<Report> errors = new();
    public readonly List<Report> warnings = new();

    public void ReportError(Report error)
    {
        errors.Add(error);
    }

    public void ReportWarning(Report warn)
    {
        warnings.Add(warn);
    }
}