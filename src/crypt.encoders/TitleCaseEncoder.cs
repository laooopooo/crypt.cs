/// @file
/// Implementation of the `Crypt.Encoders.TitleCaseEncoder` class.

namespace Crypt.Encoders {
  using System;
  using System.Globalization;
  using System.Text;

  using Crypt.Encoders.Properties;

  /// Represents the TitleCase encoding method.
  public class TitleCaseEncoder: IStringEncoder {

    /// @var whiteSpaces
    /// The list of white spaces.
    private static readonly char[] whiteSpaces = {
      '\t', // '\u0009'
      '\f', // '\u000A'
      '\v', // '\u000B'
      '\n', // '\u000C'
      '\r', // '\u000D'
      ' ', // '\u0020'
      '\u00A0',
      '\u2000',
      '\u2001',
      '\u2002',
      '\u2003',
      '\u2004',
      '\u2005',
      '\u2006',
      '\u2007',
      '\u2008',
      '\u2009',
      '\u200A',
      '\u200B',
      '\u3000',
      '\uFEFF'
    };

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.TitleCaseDescription; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "TitleCase"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    /// @exception System.ArgumentNullException The specified string is `null`.
    public string Encode(string text) {
      if(text == null) throw new ArgumentNullException("text");
      return CapitalizeWords(text, null);
    }

    /// Changes to uppercase the first letter of the specified string.
    /// @param value The string to process.
    /// @returns The processed string.
    private static string Capitalize(string value) {
      if(string.IsNullOrEmpty(value)) return value;
      var firstChar = Char.ToUpper(value[0], CultureInfo.CurrentCulture);
      return value.Length > 1 ? firstChar + value.Substring(1) : firstChar;
    }

    /// Changes to uppercase the first letter of all words of the specified string.
    /// @param value The string to process.
    /// @param delimiters The character set used to determine the words.
    /// @returns The processed string.
    private static string CapitalizeWords(this string value, params char[] delimiters) {
      return ProcessWords(Capitalize, value, delimiters);
    }

    /// Formats all words of the specified string.
    /// @param handler The delegate used to format words.
    /// @param value The string to format.
    /// @param delimiters The character set used to determine the words.
    /// @returns The processed string.
    private static string ProcessWords(Func<string, string> handler, string value, char[] delimiters) {
      if(string.IsNullOrEmpty(value)) return value;
      if(delimiters == null || delimiters.Length == 0) delimiters = whiteSpaces;

      var builder = new StringBuilder();
      int lastIndex = value.Length - 1, startIndex = 0, endIndex;

      do {
        endIndex = value.IndexOfAny(delimiters, startIndex);
        if(endIndex < 0) endIndex = lastIndex;

        builder.Append(handler(value.Substring(startIndex, (endIndex - startIndex) + 1)));
        startIndex = endIndex + 1;
      }
      while(startIndex <= lastIndex);

      return builder.ToString();
    }
  }
}