# Crypt.cs
[![Release](http://img.shields.io/github/release/cedx/crypt.cs.svg)](https://bitbucket.org/cedx/crypt.cs/downloads) [![License](http://img.shields.io/badge/license-MIT-blue.svg)](https://bitbucket.org/cedx/crypt.cs/src/master/LICENSE.txt) [![Dependencies](http://img.shields.io/david/dev/cedx/crypt.cs.svg)](https://david-dm.org/cedx/crypt.cs)

Simple string encoder (Base64, CRC32, DES, MD, SHA...), implemented in [C#](https://www.microsoft.com/net).  

![Screenshot](http://dev.belin.io/crypt.cs/img/screenshot.png)

This program uses the [MiniFramework.cs](https://bitbucket.org/cedx/miniframework.cs) library.

## Command Line Interface
In addition to the graphical user interface, you can also use the application from the command prompt:

```
> .\crypt.console.exe -h

Encode a message by applying a hash algorithm.

Usage: crypt.console <algorithm> <message>

Options:
  -?, -h, --help             Show this help.
  -l, --list                 Show the supported hash algorithms.
  -v, --version              Show the program version.
```

## Documentation
- [API Reference](http://dev.belin.io/crypt.cs/api)

## License
[Crypt.cs](https://bitbucket.org/cedx/crypt.cs) is distributed under the MIT License.
