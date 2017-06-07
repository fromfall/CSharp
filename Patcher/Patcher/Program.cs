using System.Diagnostics;
using System.IO;

namespace Patcher
{
    class Program
    {
        static void Main(string[] args)
        {
            string assemblyLoc = args[0];
            string ildasmLoc = args[1];
            string ilasmLoc = args[2];
            
            FileInfo assembly = new FileInfo(assemblyLoc);
            Directory.SetCurrentDirectory(assembly.Directory.FullName);
            string code = "code.il";
            string unpackPaths = string.Format("/out={0} {1}", code, assembly.Name);
            string packPaths = string.Format("/out={0} {1}", assembly.Name, code);
    
            ProcessStartInfo unpack = new ProcessStartInfo(ildasmLoc, unpackPaths);
            unpack.WindowStyle = ProcessWindowStyle.Hidden;
            ProcessStartInfo pack = new ProcessStartInfo(ilasmLoc, packPaths);
            pack.WindowStyle = ProcessWindowStyle.Hidden;
            
            Process x;
            x = Process.Start(unpack);
            x.WaitForExit();

            string codeText = File.ReadAllText(code);
            string modified = codeText.Replace("Hello, World!", "Goodbye, World!");
            File.WriteAllText(code, modified);
            
            x = Process.Start(pack);
            x.WaitForExit();
            
            File.Delete(code);
            File.Delete("code.res");
        }
    }
}