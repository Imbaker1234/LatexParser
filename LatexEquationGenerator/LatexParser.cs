using System.Text;
using System.Text.RegularExpressions;

namespace LatexEquationGenerator;

public class LatexParser
{
    
public StringBuilder TranslateMarkdownEquationToLatex(string equation)
{
    equation = equation.Trim();

    equation = ReplaceForwardSlashesWithDivisionToken(equation);

    var lines = Regex.Split(equation, @"\r\n").Select(x => x.Trim());

    var outputBuilder = new StringBuilder().AppendLine(@"\begin{align*}");

    outputBuilder = lines.Aggregate(outputBuilder, (current, line) => AddTranslatedLineAsLatexToBuilder(current, line));

    outputBuilder.AppendLine(@"\end{align*}");
    return outputBuilder;
}

public StringBuilder AddTranslatedLineAsLatexToBuilder(StringBuilder builder, string line)
{
    var add = line;

    add = ReplaceDashesWithDivider(line, add);

    var textGrpsWithCount = GetTextGrpsWithCount(add);

    add = TokenizeTextGroups(textGrpsWithCount, add);
    
    add = ReplaceDoubleSpacesWithQuad(add);
    
    add = DeTokenizeTextGroups(textGrpsWithCount, add);

    return PrefixAndAppendLine(add, builder);
}

public string ReplaceDashesWithDivider(string s, string add1)
{
    var dashRgx = Regex.Match(s, "-+");
    if (dashRgx.Success && dashRgx.ToString() == s)
    {
        add1 = @"\underline{\hspace{ @@@int@@@ in}}"
            .Replace("@@@int@@@", s.Length.ToString());
    }

    return add1;
}

private Tuple<string, int>[] GetTextGrpsWithCount(string s1)
{
    var enumerable = new Regex(@"\(.*?\)").Matches(s1)
        .Select((match, index) => new Tuple<string, int>(match.ToString(), index));
    return enumerable.ToArray();
}

public string TokenizeTextGroups(Tuple<string, int>[] tuples, string add2)
{
    foreach (var (textGrp, index) in tuples)
    {
        add2 = add2.Replace(textGrp, $"@@@textGrp{index}@@@");
    }

    return add2;
}

public string DeTokenizeTextGroups(Tuple<string, int>[] textGrpsWithCount1, string s2)
{
    foreach (var (textGrp, index) in textGrpsWithCount1)
    {
        s2 = s2.Replace($"@@@textGrp{index}@@@", @"\text {" + textGrp + '}');
    }

    return s2;
}

public string ReplaceDoubleSpacesWithQuad(string add3)
{
    add3 = new Regex(@"\s\s").Replace(add3, @"\quad");
    return add3;
}

public StringBuilder PrefixAndAppendLine(string s3, StringBuilder stringBuilder)
{
    var prefix = @"\quad";
    if (s3.StartsWith('*') || s3.StartsWith('+') || s3.StartsWith("\\div") || s3.StartsWith('-'))
    {
        prefix = s3.Substring(0, 1);
        s3 = s3.Substring(1);
    }

    return stringBuilder.AppendLine($@"& {prefix} {s3} \\");
}

public string ReplaceForwardSlashesWithDivisionToken(string submission1)
{
    submission1 = new Regex(@"\s\/\s")
        .Replace(submission1, @" \div ");
    return submission1;
}
}