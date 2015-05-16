# TUIOListener

**Simple C# TUIO v1.1 / OSC v1.1 network listener.**

Listen for [http://www.tuio.org/](TUIO) or [http://opensoundcontrol.org/](OSC) network traffic and print to the console. Defaults to listening for TUIO on port 3333. Useful for quickly checking/debugging data sent from TUIO server apps.

Usage (Mono/OS X):

    > mono TUIOListener [port] [tuio|osc]
    > mono TUIOListener -help

Libraries / Assemblies:
* [https://github.com/valyard/TUIOsharp](https://github.com/valyard/TUIOsharp) (v1.1 development branch)
* [https://github.com/valyard/OSCsharp](https://github.com/valyard/OSCsharp)

**Author**

Greg Harding [http://www.flightless.co.nz](http://www.flightless.co.nz)

Copyright 2015 Flightless Ltd

**License**

> The MIT License (MIT)
> 
> Copyright (c) 2015 Flightless Ltd
> 
> Permission is hereby granted, free of charge, to any person obtaining
> a copy 	of this software and associated documentation files (the
> "Software"), to deal 	in the Software without restriction, including
> without limitation the rights 	to use, copy, modify, merge, publish,
> distribute, sublicense, and/or sell 	copies of the Software, and to
> permit persons to whom the Software is 	furnished to do so, subject to
> the following conditions:
> 
> The above copyright notice and this permission notice shall be
> included in all 	copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
> EXPRESS OR 	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
> MERCHANTABILITY, 	FITNESS FOR A PARTICULAR PURPOSE AND
> NONINFRINGEMENT. IN NO EVENT SHALL THE 	AUTHORS OR COPYRIGHT HOLDERS
> BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 	LIABILITY, WHETHER IN AN
> ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 	OUT OF OR IN
> CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
> SOFTWARE.
