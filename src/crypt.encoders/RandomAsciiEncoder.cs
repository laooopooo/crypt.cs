/// @file
/// Implementation of the `Crypt.Encoders.RandomAsciiEncoder` class.

namespace Crypt.Encoders {
  using System;
  using System.Globalization;
  using System.Linq;
  using System.Text;

  using Crypt.Encoders.Properties;
  
  /// Represents the RandomASCII encoding method.
  public class RandomAsciiEncoder: IStringEncoder {
    
    /// Initializes a new instance of the class.
    public RandomAsciiEncoder() {}
  
    /// @var random
    /// Instance used to generate a random number sequence.
    private Random random=new Random();

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.RandomAsciiDescription; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "RandomASCII"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      if(string.IsNullOrEmpty(text)) return text;

      var builder=new StringBuilder(text.Length);
      foreach(var item in text) {
        if(this.random.Next(3)==0) builder.Append(item);
        else builder.Append(string.Format(CultureInfo.InvariantCulture, "&#{0};", (int) item));
      }

      return builder.ToString();
    }
  }
}