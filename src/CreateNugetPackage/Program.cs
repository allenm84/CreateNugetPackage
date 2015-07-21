using System;
using System.Collections.Generic;
using System.Common.References;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateNugetPackage
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      string path = Application.StartupPath;
      string dropbox = Dropbox.Location;

      var nuget = Path.Combine(dropbox, @"Apps\nuget\nuget.exe");

      var repositories = Directory.EnumerateDirectories(path);
      foreach (var repo in repositories)
      {
        var bin = Path.Combine(repo, "bin");
        if (!Directory.Exists(bin))
        {
          Console.WriteLine("{0} does not contain a bin directory", repo);
          continue;
        }

        var spec = Directory.EnumerateFiles(repo, "*.nuspec", SearchOption.TopDirectoryOnly).SingleOrDefault();
        if (string.IsNullOrWhiteSpace(spec))
        {
          Console.WriteLine("{0} does not contain a nuspec", repo);
          continue;
        }

        var dir = new DirectoryInfo(repo);
        var output = Path.Combine(dropbox, "[nuget]", dir.Name);
        var info = new ProcessStartInfo(nuget, string.Format("pack -OutputDirectory \"{0}\"", output));
        info.WorkingDirectory = repo;
        Process.Start(info).WaitForExit();
      }
    }
  }
}
