// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using CommandLine;
using LatexEquationGenerator;


var submission = string.Join(Environment.NewLine, args)
    .Trim('\'', '\"');

var outputBuilder = new LatexParser().TranslateMarkdownEquationToLatex(submission);

var output = outputBuilder.ToString().Trim();
Console.WriteLine(output);

// var submission = @"
// 21     (Hours Remaining)
// +4  (Tasks Per Person) = 16  (Tasks To Do) / 4    (People To Help)
// -----
// 5.25  (Hours Per Task)";