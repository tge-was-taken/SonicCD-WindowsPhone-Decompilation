using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Devices.Sensors
{
	public class Accelerometer
	{
		public EventHandler<AccelerometerReadingEventArgs> ReadingChanged { get; set; }

		public void Start()
		{
			Stub.Log( typeof( Accelerometer ), nameof( Start ) );
		}
	}

	public class AccelerometerReadingEventArgs : EventArgs
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
	}

	public class AccelerometerFailedException : Exception
	{

	}
}
