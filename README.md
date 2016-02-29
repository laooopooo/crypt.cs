# Crypt.cs
Simple string encoder (Base64, CRC32, DES, MD, SHA...), implemented in [C#](https://www.microsoft.com/net).

![Screenshot](http://www.belin.io/crypt.cs/img/screenshot.png)

## Requirements
The latest [.NET Framework](https://www.microsoft.com/net) version.
If you plan to play with the sources, you will also need the latest versions of the following products:

- [Doxygen](http://www.doxygen.org)
- [Inno Setup](http://www.jrsoftware.org/isinfo.php)
- [NAnt](http://nant.sourceforge.net)
- [SonarQube Runner](http://docs.sonarqube.org/display/SONAR/Installing+and+Configuring+SonarQube+Runner)

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
[Crypt.cs](http://dev.belin.io/crypt.cs) is distributed under the Apache License, version 2.0.
