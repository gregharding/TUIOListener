# TUIOListener

**Simple C# TUIO v1.1 / OSC v1.1 network listener.**

Listen for [TUIO](http://www.tuio.org/) or [OSC](http://opensoundcontrol.org/) network traffic and output it to the console. Useful for quickly checking/debugging data sent from TUIO server apps.

Defaults to listening for TUIO on port 3333. Output radians/degrees values in TUIO data using the rads/degs option. Invert X/Y axis values in TUIO data using the invertx/y/xy option.

Usage (Mono/OS X):

    > mono TUIOListener [port] [tuio|osc] [rads|degs] [invertx|inverty|invertxy]
    > mono TUIOListener -help

Output examples:

	> mono TUIOListener
	TUIO listening on port 3333... (Press escape to quit)
	...
	198 Object Added 10/2:0.5108514,0.4567669 0.000
	199 Object Moved 10/2:0.5203468,0.4452846 0.045
	200 Object Moved 10/2:0.5203468,0.4452846 0.123
	...
	223 Object Moved 10/2:0.9283744,0.8942347 2.236
	224 Object Removed 10/2
	Bye!

	> mono TUIOListener 3334 osc
	OSC listening on port 3334... (Press escape to quit)
	...
	/tuio/2Dobj,si alive 10
	/tuio/2Dobj,siiffffffff set 10 2 0.5108514 0.4567669 0 0 0 0 0 0
	/tuio/2Dobj,si fseq 198
	/tuio/2Dobj,si alive 10 11
	/tuio/2Dobj,siiffffffff set 10 2 0.5203468 0.4452846 0 0 0 0 0 0
	/tuio/2Dobj,siiffffffff set 11 5 0.2395874 0.8796411 0 0 0 0 0 0
	/tuio/2Dobj,si fseq 199
	...
	Bye!

Libraries / Assemblies:
* [https://github.com/gregharding/TUIOsharp](https://github.com/gregharding/TUIOsharp)
* [https://github.com/gregharding/OSCsharp](https://github.com/gregharding/OSCsharp)

Currently fixed versions of;
* [https://github.com/valyard/TUIOsharp](https://github.com/valyard/TUIOsharp)
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
