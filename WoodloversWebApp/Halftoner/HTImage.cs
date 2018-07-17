using System.Collections.Generic;

namespace Halftoner
{
	public class HTImage
	{
		private HTRow curRow;

		public List<HTRow> Rows = new List<HTRow>();

		public int NumRows => Rows.Count;

		public int NumLines
		{
			get
			{
				int num = 0;
				foreach (HTRow row in Rows)
				{
					num += row.Lines.Count;
				}
				return num;
			}
		}

		public int NumPoints
		{
			get
			{
				int num = 0;
				foreach (HTRow row in Rows)
				{
					num += row.NumPoints;
				}
				return num;
			}
		}

		public void Clear()
		{
			Rows.Clear();
			curRow = null;
		}

		public void NewRow()
		{
			curRow = new HTRow();
			Rows.Add(curRow);
		}

		public void NewLine()
		{
			if (curRow == null)
			{
				NewRow();
			}
			curRow.NewLine();
		}

		public void AddPoint(HTPoint pt)
		{
			if (curRow == null)
			{
				NewRow();
			}
			curRow.AddPoint(pt);
		}
	}
}
