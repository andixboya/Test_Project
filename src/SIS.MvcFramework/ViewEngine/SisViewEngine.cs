using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SIS.MvcFramework.ViewEngine
{
    public class SisViewEngine : IViewEngine
    {
        public string GetHtml<T>(string viewContent, T model)
        {
            string csharpHtmlCode = GetCSharpCode(viewContent);
            string code = $@"
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework.ViewEngine;
namespace AppViewCodeNamespace
{{
    public class AppViewCode : IView
    {{
        public string GetHtml(object model)
        {{
            var Model = model as {model.GetType().FullName};
	        var html = new StringBuilder();

            {csharpHtmlCode}
            
	        return html.ToString();
        }}
    }}
}}";
            var view = CompileAndInstance(code, model.GetType().Assembly);
            var htmlResult = view?.GetHtml(model);
            return htmlResult;
        }

        private string GetCSharpCode(string viewContent)
        {
            // TODO: { var a = "Niki"; }
            var lines = viewContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var cSharpCode = new StringBuilder();
            var supportedOpperators = new[] { "for", "if", "else" };
            foreach (var line in lines)
            {
                if (line.TrimStart().StartsWith("{") || line.TrimStart().StartsWith("}"))
                {
                    // { / }
                    cSharpCode.AppendLine(line);
                }
                //step: in case of @ AND! operator
                else if (supportedOpperators.Any(x => line.TrimStart().StartsWith("@" + x)))
                {
                    //step: 
                    // @C#
                    //get the location of the "@";
                    var atSignLocation = line.IndexOf("@");
                    //remove it 
                    var csharpLine = line.Remove(atSignLocation, 1);
                    //append the rest 
                    cSharpCode.AppendLine(csharpLine);
                }
                else
                {
                    // HTML
                    //step: 
                    //if there is JUST "@" and no operator, it SHOULD count as param!
                    if (!line.Contains("@"))
                    {
                        //step: 
                        //if there are no params, you just have to add htmlAppendLine + escape all of the inv. commas
                        var csharpLine = $"html.AppendLine(@\"{line.Replace("\"", "\"\"")}\");";
                        cSharpCode.AppendLine(csharpLine);
                    }
                    else
                    {
                        //step: In case there is one "@" (we won`t be covering for now cases with more than one)

                        var csharpStringToAppend = "html.AppendLine(@\"";
                        var restOfLine = line;
                        while (restOfLine.Contains("@"))
                        {
                            //first we get the @ location and separate the FIRST PART until we reach it. (BEFORE @ part)
                            var atSignLocation = restOfLine.IndexOf("@");
                            var plainText = restOfLine.Substring(0, atSignLocation);


                            //add regex, which will take everything AFTER @ (if there is more than one we are a bit screwed (only for now)
                            var csharpCodeRegex = new Regex(@"[^\s<""]+", RegexOptions.Compiled);

                            //second => here we take everything that comes after @  (AFTER @ part)
                            var csharpExpression = csharpCodeRegex.Match(restOfLine.Substring(atSignLocation + 1))?.Value;

                            // we add "\" , because we have to escape the @? 
                            csharpStringToAppend += plainText + "\" + " + csharpExpression + " + @\"";

                            //here i should debug, to check what is going on, because i`m  not sure...
                            if (restOfLine.Length <= atSignLocation + csharpExpression.Length + 1)
                            {
                                restOfLine = string.Empty;
                            }
                            else
                            {
                                restOfLine = restOfLine.Substring(atSignLocation + csharpExpression.Length + 1);
                            }
                        }

                        csharpStringToAppend += $"{restOfLine}\");";
                        cSharpCode.AppendLine(csharpStringToAppend);
                    }
                }
            }

            return cSharpCode.ToString();
        }

        private IView CompileAndInstance(string code, Assembly modelAssembly)
        {
            // we load the assemblies, so the usings above , from our code will compile i think ? 
            var compilation = CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(modelAssembly.Location));

            var netStandardAssembly = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();
            foreach (var assembly in netStandardAssembly)
            {
                //here we load more assemblies, which are standard for each app. (necessary, just in case?) 
                compilation = compilation.AddReferences(
                    MetadataReference.CreateFromFile(Assembly.Load(assembly).Location));
            }

            //here we LOAD the code, which is received from the getC#Code AS STRING and create a code from it (in-memory)
            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));

            using (var memoryStream = new MemoryStream())
            {
                //here... we use a stream to RE-CREATE the loaded code with the help of the given assemblies.
                var compilationResult = compilation.Emit(memoryStream);
                if (!compilationResult.Success)
                {
                    //here we add  a method which displays if we have any errors in our code!
                    foreach (var error in compilationResult.Diagnostics) // .Where(x => x.Severity == DiagnosticSeverity.Error)
                    {
                        Console.WriteLine(error.GetMessage());
                    }

                    return null;
                }

                //because of the stream, which is first filled with the code
                //we have to tell it manually to start from 0 , so it can begin WRITING what is contained within it?
                //tricky... , and good to know.
                memoryStream.Seek(0, SeekOrigin.Begin);

                //here it writes the bytes, which are read within it
                var assemblyBytes = memoryStream.ToArray();
                //here i guess we create an assembly out of the loaded bytes?
                var assembly = Assembly.Load(assemblyBytes);

                //we add... what was this again? 
                var type = assembly.GetType("AppViewCodeNamespace.AppViewCode");

                // and.. type check of the assembly, just in case it was null and not created from above "Load method"
                if (type == null)
                {
                    Console.WriteLine("AppViewCode not found.");
                    return null;
                }

                //instance null-check ( just in case sth.went wrong with the type)
                var instance = Activator.CreateInstance(type);
                if (instance == null)
                {
                    Console.WriteLine("AppViewCode cannot be instanciated.");
                    return null;
                }
                return instance as IView;
            }
        }
    }
}
