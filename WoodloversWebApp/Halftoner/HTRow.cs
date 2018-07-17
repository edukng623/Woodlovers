using System.Collections.Generic;

namespace Halftoner
{
	public class HTRow
	{
		private HTLine curLine;

		public List<HTLine> Lines = new List<HTLine>();

		public int NumPoints
		{
			get
			{
				int num = 0;
				foreach (HTLine line in Lines)
				{
					num += line.NumPoints;
				}
				return num;
			}
		}

		public void NewLine()
		{
			curLine = new HTLine();
			Lines.Add(curLine);
		}

		public void AddPoint(HTPoint pt)
		{
			if (curLine == null)
			{
				NewLine();
			}
			curLine.Points.Add(pt);
		}
	}
}
