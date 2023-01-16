using CommandLine.Text;

namespace LatexEquationGenerator;

public class LatexEquationArgs
{
    [CommandLine.Option('e', "equation", HelpText = "Simple Plaintext Equation" )]
    public string Equation { get; set; }
}