using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.JScript;
using Microsoft.JScript.Vsa;

namespace lnE
{
    public static class AssemblyHelper
    {
        public static Assembly GetAssembly(string[] referencedAssemblies, string path)
        {
            var provider = CodeDomProvider.CreateProvider("cs");
            var cp = new CompilerParameters();
            cp.IncludeDebugInformation = true;
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
            cp.ReferencedAssemblies.AddRange(new[]{ 
                "System.dll", 
                "System.Core.dll", 
                "Microsoft.CSharp.dll",
                "Microsoft.JScript.dll",
                "System.Configuration.dll",
                "System.Data.dll", 
                "System.Data.DataSetExtensions.dll",
                "System.Xml.dll",
                "System.Xml.Linq.dll",
                "System.Windows.Forms.dll",
            });

            cp.ReferencedAssemblies.AddRange(referencedAssemblies);

            var sources = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);

            var result = provider.CompileAssemblyFromFile(cp, sources);
            if (result.Errors.HasErrors)
            {
                Trace.WriteLine(result.Output.Cast<string>().Aggregate((a, b) => a + Environment.NewLine + b));
                throw new Exception("Compile Failed");
            }

            return result.CompiledAssembly;
        }

        public static IList<T1> GetObjects<T1, T2>(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsDefined(typeof(T2))).Select(t => (T1)Activator.CreateInstance(t)).ToList();
        }

        public static IList<Type> GetTypes<T>(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsDefined(typeof(T))).ToList();
        }

        public static T GetObject<T>(Type t)
        {
            return (T)Activator.CreateInstance(t);
        }

        private static string GetJsSource(string source, string varName)
        {
            if (!source.TrimEnd().EndsWith(";"))
                source += ";";
            if (!String.IsNullOrWhiteSpace(varName))
                source += varName + ";";

            return source;
        }

        public static dynamic EvalJs(string source, string varName = null)
        {
#pragma warning disable 618
            VsaEngine ve = VsaEngine.CreateEngine();
#pragma warning restore

            source = GetJsSource(source, varName);

            return Eval.JScriptEvaluate(source, "unsafe", ve);
        }

        public static T EvalJs<T>(string source, string varName = null)
        {
            return (T)EvalJs(source, varName);
        }

        public static dynamic EvalJs2(string source, string varName = null)
        {
            source = GetJsSource(source, varName);

            var script = new MSScriptControl.ScriptControl();
            script.Language = "javascript";
            var result = script.Eval(source);
            return result;
        }

        public static T EvalJs2<T>(string source, string varName = null)
        {
            return (T)EvalJs2(source, varName);
        }

        public static string ParseString(string pattern, string str)
        {
            var ex = new Regex(pattern);
            var match = ex.Match(str);
            if (match.Groups.Count < 2)
                return null;

            return match.Groups[1].Value;
        }
    }
}
