using System.Collections.Generic;
using System.Linq;

namespace Halftoner
{
	public class HTLine
	{
		public List<HTPoint> Points = new List<HTPoint>();

		public int NumPoints => Points.Count();
	}
}
