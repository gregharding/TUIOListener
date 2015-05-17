/*

	Simple TUIO v1.1 / OSC v1.1 network listener.
	
	Listen for TUIO or OSC network traffic and output it to the console. Useful for quickly checking/debugging data sent from TUIO server apps.
	
	Defaults to listening for TUIO on port 3333. Output radians/degrees using rads/degs options. Invert X/Y axis values in TUIO data using the invertx/y/xy options.
	
	Usage:
		> mono TUIOListener [port] [tuio|osc] [rads|degs] [invertx|inverty|invertxy]
		> mono TUIOListener -help

	Libraries:
		https://github.com/valyard/TUIOsharp (v1.1 development branch)
		https://github.com/valyard/OSCsharp

	
	Author:
		Greg Harding greg@flightless.co.nz
		www.flightless.co.nz
	
	Copyright 2015 Flightless Ltd.
	

	The MIT License (MIT)

	Copyright (c) 2015 Flightless Ltd

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.

*/

using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

using TUIOsharp;
using TUIOsharp.DataProcessors;
using TUIOsharp.Entities;

using OSCsharp;
using OSCsharp.Data;
using OSCsharp.Net;
using OSCsharp.Utils;


namespace TUIOListener {

	class Program {

		public enum MessageType {
			TUIO,
			OSC
		};

		public static int port = 3333;
		public static MessageType messageType = MessageType.TUIO;
		public static bool degs = false;
		public static bool invertX = false;
		public static bool invertY = false;


		public static void Main(string[] args) {
			if (ProcessCommandLineArgs(args)) {
				if (messageType == MessageType.TUIO) {
					ListenForTUIO();
				} else {
					ListenForOSC();
				}
			}
		}

		private static bool ProcessCommandLineArgs(string[] args) {
			if (args == null) return true;

			// help
			if (args.Length == 1 && (args[0] == "-h" || args[0] == "-help")) {
				Console.WriteLine("Usage: mono TUIOListener [port] [tuio|osc] [rads|degs] [invertx|inverty|invertxy]");
				return false;
			}

			bool success = true;

			// port
			if (args.Length >= 1) {
				int _port;
				if (int.TryParse(args[0], out _port)) {
					if (_port > 1024 && _port <= 65535) {
						port = _port;
					} else {
						Console.WriteLine(string.Format("Warning: should be listening on port 1025..65535! Defaulting to port {0}", port));
					}
				} else {
					success = false;
				}
			}

			// message type
			if (args.Length >= 2) {
				string arg = args[1].ToLower();
				if (arg == "tuio") {
					messageType = MessageType.TUIO;
				} else if (arg == "osc") {
					messageType = MessageType.OSC;
				} else {
					Console.WriteLine(string.Format("Warning: should be listening for tuio or osc! Defaulting to {0}", messageType.ToString()));
				}
			}

			// rads/degs
			if (args.Length >= 3) {
				string arg = args[2].ToLower();
				if (messageType == MessageType.TUIO) {
					if (arg == "rads") {
						degs = false;
					} else if (arg == "degs") {
						degs = true;
					} else {
						Console.WriteLine(string.Format("Warning: use rads/degs for angle option! Defaulting to {0}", (degs ? "degs" : "rads")));
					}
				} else {
					Console.WriteLine("Warning: rads/degs only used when listening to tuio!");
				}
			}

			// invert y
			if (args.Length >= 4) {
				string arg = args[3].ToLower();
				if (messageType == MessageType.TUIO) {
					if (arg == "invertx") {
						invertX = true;
					} else if (arg == "inverty") {
						invertY = true;
					} else if (arg == "invertxy") {
						invertX = true;
						invertY = true;
					} else {
						Console.WriteLine("Warning: not using invert x/y!");
					}
				} else {
					Console.WriteLine("Warning: invert x/y only used when listening to tuio!");
				}
			}

			return success;
		}


		//
		// tuio listener
		//

		private static TuioServer tuioServer;

		private static void ListenForTUIO() {
			Console.WriteLine(string.Format("TUIO listening on port {0}... (Press escape to quit)", port));

			// tuio
			tuioServer = new TuioServer(port);

			CursorProcessor cursorProcessor = new CursorProcessor();
			cursorProcessor.CursorAdded += OnCursorAdded;
			cursorProcessor.CursorUpdated += OnCursorUpdated;
			cursorProcessor.CursorRemoved += OnCursorRemoved;

			BlobProcessor blobProcessor = new BlobProcessor();
			blobProcessor.BlobAdded += OnBlobAdded;
			blobProcessor.BlobUpdated += OnBlobUpdated;
			blobProcessor.BlobRemoved += OnBlobRemoved;

			ObjectProcessor objectProcessor = new ObjectProcessor();
			objectProcessor.ObjectAdded += OnObjectAdded;
			objectProcessor.ObjectUpdated += OnObjectUpdated;
			objectProcessor.ObjectRemoved += OnObjectRemoved;

			// listen...
			tuioServer.Connect();

			tuioServer.AddDataProcessor(cursorProcessor);
			tuioServer.AddDataProcessor(blobProcessor);
			tuioServer.AddDataProcessor(objectProcessor);

			do {
				while (!Console.KeyAvailable) {
					Thread.Sleep(100);
				}
			} while (Console.ReadKey(true).Key != ConsoleKey.Escape);

			// done
			tuioServer.Disconnect();
			tuioServer = null;

			Console.WriteLine("Bye!");
		}

		private static void OnCursorAdded(object sender, TuioCursorEventArgs e) {
			var entity = e.Cursor;
			lock (tuioServer) {
				var x = invertX ? (1 - entity.X) : entity.X ;
				var y = invertY ? (1 - entity.Y) : entity.Y ;
				Console.WriteLine(string.Format("{0} Cursor Added {1}:{2},{3}", ((CursorProcessor)sender).FrameNumber, entity.Id, x, y));
			}
		}

		private static void OnCursorUpdated(object sender, TuioCursorEventArgs e) {
			var entity = e.Cursor;
			lock (tuioServer) {
				var x = invertX ? (1 - entity.X) : entity.X ;
				var y = invertY ? (1 - entity.Y) : entity.Y ;
				Console.WriteLine(string.Format("{0} Cursor Moved {1}:{2},{3}", ((CursorProcessor)sender).FrameNumber, entity.Id, x, y));
			}
		}

		private static void OnCursorRemoved(object sender, TuioCursorEventArgs e) {
			var entity = e.Cursor;
			lock (tuioServer) {
				Console.WriteLine(string.Format("{0} Cursor Removed {1}", ((CursorProcessor)sender).FrameNumber, entity.Id));
			}
		}

		private static void OnBlobAdded(object sender, TuioBlobEventArgs e) {
			var entity = e.Blob;
			lock (tuioServer) {
				var x = invertX ? (1 - entity.X) : entity.X ;
				var y = invertY ? (1 - entity.Y) : entity.Y ;
				var angle = degs ? (entity.Angle * (180f / Math.PI)) : entity.Angle ;
				Console.WriteLine(string.Format("{0} Blob Added {1}:{2},{3} {4:F1}", ((BlobProcessor)sender).FrameNumber, entity.Id, x, y, angle));
			}
		}

		private static void OnBlobUpdated(object sender, TuioBlobEventArgs e) {
			var entity = e.Blob;
			lock (tuioServer) {
				var x = invertX ? (1 - entity.X) : entity.X ;
				var y = invertY ? (1 - entity.Y) : entity.Y ;
				var angle = degs ? (entity.Angle * (180f / Math.PI)) : entity.Angle ;
				Console.WriteLine(string.Format("{0} Blob Moved {1}:{2},{3} {4:F1}", ((BlobProcessor)sender).FrameNumber, entity.Id, x, y, angle));
			}
		}

		private static void OnBlobRemoved(object sender, TuioBlobEventArgs e) {
			var entity = e.Blob;
			lock (tuioServer) {
				Console.WriteLine(string.Format("{0} Blob Removed {1}", ((BlobProcessor)sender).FrameNumber, entity.Id));
			}
		}

		private static void OnObjectAdded(object sender, TuioObjectEventArgs e) {
			var entity = e.Object;
			lock (tuioServer) {
				var x = invertX ? (1 - entity.X) : entity.X ;
				var y = invertY ? (1 - entity.Y) : entity.Y ;
				var angle = degs ? (entity.Angle * (180f / Math.PI)) : entity.Angle ;
				Console.WriteLine(string.Format("{0} Object Added {1}/{2}:{3},{4} {5:F1}", ((ObjectProcessor)sender).FrameNumber, entity.ClassId, entity.Id, x, y, angle));
			}
		}

		private static void OnObjectUpdated(object sender, TuioObjectEventArgs e) {
			var entity = e.Object;
			lock (tuioServer) {
				var x = invertX ? (1 - entity.X) : entity.X ;
				var y = invertY ? (1 - entity.Y) : entity.Y ;
				var angle = degs ? (entity.Angle * (180f / Math.PI)) : entity.Angle ;
				Console.WriteLine(string.Format("{0} Object Moved {1}/{2}:{3},{4} {5:F1}", ((ObjectProcessor)sender).FrameNumber, entity.ClassId, entity.Id, x, y, angle));
			}
		}

		private static void OnObjectRemoved(object sender, TuioObjectEventArgs e) {
			var entity = e.Object;
			lock (tuioServer) {
				Console.WriteLine(string.Format("{0} Object Removed {1}/{2}", ((ObjectProcessor)sender).FrameNumber, entity.ClassId, entity.Id));
			}
		}


		//
		// osc listener
		//

		private static void ListenForOSC() {
			Console.WriteLine(string.Format("OSC listening on port {0}... (Press escape to quit)", port));

			// osc
			UDPReceiver udpReceiver = new UDPReceiver(port, false);
			udpReceiver.MessageReceived += OscMessageReceivedHandler;
			udpReceiver.ErrorOccured += OscErrorOccuredHandler;

			// listen...
			udpReceiver.Start();

			do {
				while (!Console.KeyAvailable) {
					Thread.Sleep(100);
				}
			} while (Console.ReadKey(true).Key != ConsoleKey.Escape);

			// done
			udpReceiver.Stop();
			udpReceiver = null;

			Console.WriteLine("Bye!");
		}

		private static void OscErrorOccuredHandler(object sender, ExceptionEventArgs exceptionEventArgs) {
			Console.WriteLine(string.Format("Error {0}", exceptionEventArgs.ToString()));
		}

		private static void OscMessageReceivedHandler(object sender, OscMessageReceivedEventArgs oscMessageReceivedEventArgs) {
			OscMessage msg = oscMessageReceivedEventArgs.Message;

			StringBuilder data = new StringBuilder();
			for (int i=0; i<msg.Data.Count; i++) {
				data.AppendFormat(" {0}", msg.Data[i]);
			}

			Console.WriteLine(string.Format("{0}{1}{2}", msg.Address, msg.TypeTag, data.ToString()));
		}
	}
}
