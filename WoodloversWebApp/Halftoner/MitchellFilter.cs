using System.Drawing;
using System.Drawing.Imaging;

namespace Halftoner
{
	public class MitchellFilter
	{
		private int KernelRadius = 3;

		private int[] FilterKernel;

		private int FilterSum = 1;

		private int FilterMult = 1;

		public int Size => KernelRadius;

		private int FilterValue(double x)
		{
			if (x < 0.0)
			{
				x = 0.0 - x;
			}
			double num = (x >= 2.0) ? 0.0 : ((!(x >= 1.0)) ? (7.0 * x * x * x - 12.0 * x * x + 5.333333333333333) : (-2.3333333333333335 * x * x * x + 12.0 * x * x + -20.0 * x + 10.666666666666666));
			return (int)(num * 0.16666666666666666 * 1024.0);
		}

		private void BuildKernel(int size)
		{
			KernelRadius = size;
			FilterKernel = new int[size + 1];
			FilterSum = 0;
			for (int i = 0; i <= size; i++)
			{
				FilterKernel[i] = FilterValue((double)i / (double)KernelRadius);
				FilterSum += FilterKernel[i] * 2;
			}
			FilterSum -= FilterKernel[0];
			FilterMult = 131072 / FilterSum;
		}

		public unsafe Bitmap ToGrayscale(Bitmap map)
		{
			Rectangle rect = new Rectangle(0, 0, map.Width, map.Height);
			BitmapData bitmapData = map.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			Bitmap bitmap = new Bitmap(map.Width, map.Height, PixelFormat.Format8bppIndexed);
			BitmapData bitmapData2 = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
			byte* ptr = (byte*)(void*)bitmapData.Scan0;
			byte* ptr2 = (byte*)(void*)bitmapData2.Scan0;
			for (int i = 0; i < map.Height; i++)
			{
				byte* ptr3 = ptr;
				int num = 0;
				while (num < map.Width)
				{
					int num2 = ptr3[2] * 306 + ptr3[1] * 600 + *ptr3 * 117 + 511 >> 10;
					ptr2[num] = (byte)num2;
					num++;
					ptr3 += 4;
				}
				ptr += bitmapData.Stride;
				ptr2 += bitmapData2.Stride;
			}
			map.UnlockBits(bitmapData);
			bitmap.UnlockBits(bitmapData2);
			for (int j = 0; j < 256; j++)
			{
				bitmap.Palette.Entries[j] = Color.FromArgb(255, j, j, j);
			}
			return bitmap;
		}

		public unsafe Bitmap Filter(Bitmap map, int size)
		{
			BuildKernel(size);
			BitmapData bitmapData = map.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
			Bitmap bitmap = new Bitmap(map.Width, map.Height, PixelFormat.Format8bppIndexed);
			BitmapData bitmapData2 = bitmap.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
			byte* ptr = (byte*)(void*)bitmapData.Scan0;
			byte* ptr2 = (byte*)(void*)bitmapData2.Scan0;
			byte[] array = new byte[map.Width + size * 2];
			for (int i = 0; i < map.Height; i++)
			{
				byte* ptr3 = ptr;
				byte* ptr4 = ptr2;
				for (int j = 1; j < size; j++)
				{
					array[size - j] = *ptr3;
					array[map.Width - 1 + j] = ptr3[map.Width - 1];
				}
				for (int k = 0; k < map.Width; k++)
				{
					array[k + size] = ptr3[k];
				}
				for (int l = size; l < map.Width + size; l++)
				{
					int num = array[l] * FilterKernel[0];
					for (int m = 1; m <= size; m++)
					{
						num += (array[l - m] + array[l + m]) * FilterKernel[m];
					}
					num = num * FilterMult + 32767 >> 17;
					num = ((num >= 0) ? num : 0);
					num = ((num > 255) ? 255 : num);
					ptr4[l - size] = (byte)num;
				}
				ptr += bitmapData.Stride;
				ptr2 += bitmapData2.Stride;
			}
			map.UnlockBits(bitmapData);
			bitmap.UnlockBits(bitmapData2);
			int[] array2 = new int[size * 2 + 1];
			for (int n = -size; n <= 0; n++)
			{
				array2[size + n] = 0;
			}
			for (int num2 = 1; num2 <= size; num2++)
			{
				array2[size + num2] = num2 * bitmapData.Stride;
			}
			bitmapData = bitmap.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
			Bitmap bitmap2 = new Bitmap(map.Width, map.Height, PixelFormat.Format8bppIndexed);
			bitmapData2 = bitmap2.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
			ptr = (byte*)(void*)bitmapData.Scan0;
			ptr2 = (byte*)(void*)bitmapData2.Scan0;
			for (int num3 = 0; num3 < map.Height; num3++)
			{
				if (num3 != 0)
				{
					for (int num4 = 1; num4 <= size * 2; num4++)
					{
						array2[num4 - 1] = array2[num4];
					}
					int num5 = num3 + size;
					if (num5 >= map.Height)
					{
						num5 = map.Height - 1;
					}
					array2[size * 2] = num5 * bitmapData.Stride;
				}
				byte* ptr5 = ptr;
				byte* ptr6 = ptr2;
				for (int num6 = 0; num6 < map.Width; num6++)
				{
					int num7 = ptr5[array2[size]] * FilterKernel[0];
					for (int num8 = 1; num8 <= size; num8++)
					{
						num7 += (ptr5[array2[size - num8]] + ptr5[array2[size + num8]]) * FilterKernel[num8];
					}
					num7 = num7 * FilterMult + 32767 >> 17;
					ptr6[num6] = (byte)num7;
					ptr5++;
				}
				ptr2 += bitmapData2.Stride;
			}
			bitmap.UnlockBits(bitmapData);
			bitmap2.UnlockBits(bitmapData2);
			return bitmap2;
		}

		public unsafe Bitmap Filter2(Bitmap map)
		{
			BuildKernel(2);
			BitmapData bitmapData = map.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
			Bitmap bitmap = new Bitmap(map.Width, map.Height, PixelFormat.Format8bppIndexed);
			BitmapData bitmapData2 = bitmap.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
			byte* ptr = (byte*)(void*)bitmapData.Scan0;
			byte* ptr2 = (byte*)(void*)bitmapData2.Scan0;
			byte[] array = new byte[map.Width + 4];
			for (int i = 0; i < map.Height; i++)
			{
				byte* ptr3 = ptr;
				byte* ptr4 = ptr2;
				for (int j = 1; j < 2; j++)
				{
					array[2 - j] = *ptr3;
					array[map.Width - 1 + j] = ptr3[map.Width - 1];
				}
				for (int k = 0; k < map.Width; k++)
				{
					array[k + 2] = ptr3[k];
				}
				for (int l = 2; l < map.Width + 2; l++)
				{
					int num = array[l] * FilterKernel[0];
					num += (array[l - 1] + array[l + 1]) * FilterKernel[1];
					num += (array[l - 2] + array[l + 2]) * FilterKernel[2];
					num = num * FilterMult + 32767 >> 17;
					ptr4[l - 2] = (byte)num;
				}
				ptr += bitmapData.Stride;
				ptr2 += bitmapData2.Stride;
			}
			map.UnlockBits(bitmapData);
			bitmap.UnlockBits(bitmapData2);
			int[] array2 = new int[5];
			for (int m = -2; m <= 0; m++)
			{
				array2[2 + m] = 0;
			}
			for (int n = 1; n <= 2; n++)
			{
				array2[2 + n] = n * bitmapData.Stride;
			}
			bitmapData = bitmap.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
			Bitmap bitmap2 = new Bitmap(map.Width, map.Height, PixelFormat.Format8bppIndexed);
			bitmapData2 = bitmap2.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
			ptr = (byte*)(void*)bitmapData.Scan0;
			ptr2 = (byte*)(void*)bitmapData2.Scan0;
			for (int num2 = 0; num2 < map.Height; num2++)
			{
				if (num2 != 0)
				{
					for (int num3 = 1; num3 <= 4; num3++)
					{
						array2[num3 - 1] = array2[num3];
					}
					int num4 = num2 + 2;
					if (num4 >= map.Height)
					{
						num4 = map.Height - 1;
					}
					array2[4] = num4 * bitmapData.Stride;
				}
				byte* ptr5 = ptr;
				byte* ptr6 = ptr2;
				for (int num5 = 0; num5 < map.Width; num5++)
				{
					int num6 = ptr5[array2[2]] * FilterKernel[0];
					num6 += (ptr5[array2[1]] + ptr5[array2[3]]) * FilterKernel[1];
					num6 += (ptr5[array2[0]] + ptr5[array2[4]]) * FilterKernel[2];
					num6 = num6 * FilterMult + 32767 >> 17;
					ptr6[num5] = (byte)num6;
					ptr5++;
				}
				ptr2 += bitmapData2.Stride;
			}
			bitmap.UnlockBits(bitmapData);
			bitmap2.UnlockBits(bitmapData2);
			return bitmap2;
		}
	}
}
