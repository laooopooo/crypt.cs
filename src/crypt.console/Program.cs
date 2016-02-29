/// @file
/// Implementation of the `Crypt.Console.Program` class.

namespace Crypt.Console {
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;

  using MiniFramework.Reflection;
  using Mono.Options;
  using Properties;

  /// The console application.
  internal static class Program {
  
    /// @var appInfo
    /// Informations about the application assembly.
    private static AssemblyInfo appInfo = new AssemblyInfo(typeof(Program).Assembly);
    
    /// @var options
    /// The command line options.
    private static OptionSet options;

    /// The application entry point.
    /// @param args The command line arguments.
    private static void Main(string[] args) {
      CheckSetup();

      // Set default command line options.
      bool printHelp = false;
      bool printList = false;
      bool printVersion = false;

      options = new OptionSet {
        { "?|h|help", Resources.HelpOption, x => printHelp = (x != null) },
        { "l|list", Resources.ListOption, x => printList = (x != null) },
        { "v|version", Resources.VersionOption, x => printVersion = (x != null) }
      };

      // Parse command line arguments.
      List<string> parameters = null;
      try { parameters = options.Parse(args); }

      catch(OptionException e) {
        Console.WriteLine();
        Console.WriteLine(e.Message);
        PrintUsage();
        return;
      }

      // Print the detailed help.
      if(printHelp) {
        PrintHelp();
        return;
      }

      // Prints the list of hash algorithms.
      if(printList) {
        PrintEncoderList();
        return;
      }

      // Prints the version number.
      if(printVersion) {
        PrintVersion();
        return;
      }

      // Encode the specified string.
      if(parameters == null || parameters.Count != 2) {
        Console.WriteLine();
        Console.WriteLine(Resources.SyntaxError);
        PrintUsage();
        return;
      }

      PrintEncodedString(parameters[0], parameters[1]);
    }

    /// Checks that all conditions are met for application startup.
    /// If no string encoder can be found, a message is displayed to the user and the application terminated.
    private static void CheckSetup() {
      if(EncoderManager.Encoders.Count == 0) {
        Console.WriteLine();
        Console.WriteLine(Resources.AddInsNotFoundError);
        Console.WriteLine();

        Environment.Exit(1);
      }
    }

    /// Encodes and prints a given string using the specified encoding method.
    /// @param encodingMethod The name of the encoding method to use.
    /// @param stringToEncode The string to be encoded.
    private static void PrintEncodedString(string encodingMethod, string stringToEncode) {
      Console.WriteLine();

      var encoder = EncoderManager.Encoders.FirstOrDefault(x => x.Name.ToUpperInvariant() == encodingMethod.ToUpperInvariant());
      if(encoder == null) {
        Console.WriteLine(Resources.UnknownEncoderError);
        PrintEncoderList();
        return;
      }

      var encodedString = encoder.Encode(stringToEncode);
      Console.WriteLine(encodedString.Length > 0 ? encodedString : "\"\"");
      Console.WriteLine();
    }

    /// Prints the list of supported string encoders.
    private static void PrintEncoderList() {
      Console.WriteLine();
      
      foreach(var item in EncoderManager.Encoders) {
        Console.Write(item.Name.PadRight(25));
        Console.WriteLine(item.Description);
      }

      Console.WriteLine();
    }

    /// Prints the detailed help.
    private static void PrintHelp() {
      Console.WriteLine();
      Console.WriteLine(Resources.Description);
      PrintUsage();
    }

    /// Prints the program usage.
    private static void PrintUsage() {
      Console.WriteLine();
      Console.WriteLine(Resources.Usage, appInfo.Title);
      Console.WriteLine();
      Console.WriteLine(Resources.Options);
      options.WriteOptionDescriptions(Console.Out);
      Console.WriteLine();
    }

    /// Prints informations about the program version.
    private static void PrintVersion() {
      Console.WriteLine();
      Console.WriteLine("{0} {1}", appInfo.Product, appInfo.Version);
      Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Resources.Copyright, appInfo.Copyright));
      Console.WriteLine();
    }
  }
}