# Crypt.cs
Simple string encoder (Base64, CRC32, DES, MD, SHA...), implemented in [C#](https://www.microsoft.com/net).

![Screenshot](http://dev.belin.io/crypt.cs/img/screenshot.png)

## Command Line Interface
In addition to the graphical user interface, you can also use the application from the command prompt:

```
> crypt.console.exe --help

Encode a message by applying a hash algorithm.

Usage: crypt.console <algorithm> <message>

Options:
  -?, -h, --help             Show this help.
  -l, --list                 Show the supported hash algorithms.
  -v, --version              Show the program version.
```

## License
[Crypt.cs](http://dev.belin.io/crypt.cs) is distributed under the MIT License.
