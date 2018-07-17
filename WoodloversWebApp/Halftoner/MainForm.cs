using System;

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

using System.Xml;

namespace Halftoner
{
	public class MainForm
	{
		private enum Style
		{
			Dots,
			Lines,
			Squares,
			Circles,
			Random
		}

		private enum FileMode
		{
			DXF,
			PNG
		}

		private const double PI = 3.1415926535897931;

		private const double TwoPI = 6.2831853071795862;

		private const double HalfPI = 1.5707963267948966;

		private const double InvPI = 0.31830988618379069;

		private Bitmap fileImage;

		private Bitmap previewImage;

		private Bitmap scaledImage;

		private Bitmap grayImage;

		private Bitmap filterImage;

		private int[] sourceLevels = new int[256];

		private int[] adjustedLevels = new int[256];

		private float brightness;

		private float contrast = 1f;

		private bool negateImage;

		private bool adjustmentsChanged = true;

		private MitchellFilter Filter = new MitchellFilter();

		private HTImage points = new HTImage();

		private float[] dotLookup;

		private CultureInfo us = new CultureInfo("en-US");

		private Style style;

		private FileMode fileMode;

		private double workWidth = 1.0;

		private double workHeight = 1.0;

		private double border = 0.25;

		private double spacing = 0.125;

		private double minSize;

		private double maxSize = 0.25;

		private double angle;

		private double wavelength;

		private double amplitude;

		private double centerOffsX;

		private double centerOffsY;

		private bool offsetOdd;

		private bool invert;

		private bool gammaCorrect;

		private bool imperial = true;

		private bool FixedSizes;

		private int dotCount = 1000;

		private bool InternalChange;

		private IContainer components;

		//private PictureBox pbPreview;

		//private RadioButton rbPreview;

		//private RadioButton rbOriginal;

		//private NumericUpDown udWidth;

		//private Label label1;

		//private Label label2;

		//private NumericUpDown udHeight;

		//private Label label3;

		//private NumericUpDown udSpacing;

		//private Label label4;

		//private NumericUpDown udBorder;

		//private Label label5;

		//private NumericUpDown udMaxSize;

		//private Label label6;

		//private NumericUpDown udMinSize;

		//private Label label7;

		//private NumericUpDown udAngle;

		//private Button btnLoadImage;

		//private Label lblStatus;

		//private TabControl tabControl1;

		//private TabPage tabPage1;

		//private TabPage tabPage2;

		//private Label label8;

		//private NumericUpDown udSafeZ;

		//private NumericUpDown udPointRetract;

		//private NumericUpDown udToolAngle;

		//private Label label10;

		//private TabPage tabPage3;

		//private Button btnWriteDXF;

		//private Button btnWriteGCode;

		//private NumericUpDown udFeedRate;

		//private Label lblFeedRate;

		//private NumericUpDown udOriginX;

		//private Label label12;

		//private NumericUpDown udOriginY;

		//private Label label13;

		//private RadioButton rbMillimeters;

		//private RadioButton rbInches;

		//private CheckBox cbInvert;

		//private CheckBox cbOffsetOdd;

		//private Label lblDirections;

		//private CheckBox cbPointRetract;

		//private NumericUpDown udSpindleSpeed;

		//private Label label14;

		//private GroupBox groupBox1;

		//private RadioButton rbLines;

		//private RadioButton rbHalftone;

		//private Label label9;

		//private NumericUpDown udAmplitude;

		//private Label label15;

		//private NumericUpDown udWavelength;

		//private Label lblWriteDXFHelp;

		//private CheckBox cbGammaCorrect;

		//private RadioButton rbSquares;

		//private RadioButton rbRandom;

		//private GroupBox groupBox2;

		//private ToolTip ttHelpTip;

		//private Button btnTest;

		//private CheckBox cbTwoPassCuts;

		//private Label label11;

		//private NumericUpDown udEngraveDepth;

		//private CheckBox cbFixedSize;

		//private CheckBox cbLockAspect;

		//private Label lblRandomDots;

		//private NumericUpDown udRandomDots;

		//private Label label16;

		//private NumericUpDown udZOffset;

		//private RadioButton rbCircles;

		//private Label label17;

		//private NumericUpDown udCenterOffsX;

		//private NumericUpDown udCenterOffsY;

		//private Label label18;

		//private TabPage tabPage4;

		//private TrackBar tbContrast;

		//private TrackBar tbBright;

		//private Label lblContrast;

		//private Label lblBright;

		//private Button btnAutoLevels;

		//private Label label20;

		//private Label label19;

		//private CheckBox cbIncludeLineNumbers;

		//private CheckBox cbNegateImage;

		//private CheckBox cbGrblCompat;

		//public MainForm()
		//{
		//	InitializeComponent();
		//	pbPreview.Visible = false;
		//	lblDirections.Visible = true;
		//	lblRandomDots.Visible = false;
		//	udRandomDots.Visible = false;
		//	rbRandom.Visible = false;
		//	btnTest.Visible = false;
		//	btnTest.Enabled = false;
		//	LoadSettings();
		//	if (style == Style.Squares || style == Style.Circles)
		//	{
		//		udCenterOffsX.Enabled = true;
		//		udCenterOffsY.Enabled = true;
		//	}
		//	else
		//	{
		//		udCenterOffsX.Enabled = false;
		//		udCenterOffsY.Enabled = false;
		//	}
		//}

		//private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		//{
		//	SaveSettings();
		//}

		//private void MainForm_DragOver(object sender, DragEventArgs e)
		//{
		//	if (e.Data.GetDataPresent("FileNameW"))
		//	{
		//		e.Effect = DragDropEffects.Copy;
		//	}
		//	else
		//	{
		//		e.Effect = DragDropEffects.None;
		//	}
		//}

		//private void MainForm_DragDrop(object sender, DragEventArgs e)
		//{
		//	string[] array = e.Data.GetData("FileNameW") as string[];
		//	LoadImage(array[0]);
		//	ImageChanged();
		//}

		//private void btnLoadImage_Click(object sender, EventArgs e)
		//{
		//	OpenFileDialog openFileDialog = new OpenFileDialog();
		//	openFileDialog.CheckFileExists = true;
		//	openFileDialog.Filter = "Image files|*.jpg;*.jpeg;*.gif;*.png;*.bmp";
		//	openFileDialog.FilterIndex = 1;
		//	openFileDialog.AddExtension = true;
		//	openFileDialog.DefaultExt = "jpg";
		//	if (openFileDialog.ShowDialog() == DialogResult.OK)
		//	{
		//		LoadImage(openFileDialog.FileName);
		//		ImageChanged();
		//	}
		//}

		public void LoadImage(string Filename)
		{
			try
			{
				fileImage = new Bitmap(Filename);
				grayImage = Filter.ToGrayscale(fileImage);
			}
			catch (Exception)
			{
			}
		}

		private void ImageChanged()
		{
			if (fileImage != null)
			{
				if (fileImage != null && fileImage.Width > 0)
				{
					InternalChange = true;
					double num = (double)fileImage.Height / (double)fileImage.Width;
					//udHeight.Value = udWidth.Value * (decimal)num;
					InternalChange = false;
				}
				//if (!pbPreview.Visible)
				//{
				//	pbPreview.Visible = true;
				//	lblDirections.Visible = false;
				//}
				filterImage = null;
				RedrawPreview(true, false, true);
				ComputeSourceLevels();
				ComputeAdjustedLevels();
				//rbOriginal.Checked = true;
				//pbPreview.Image = fileImage;
				//pbPreview_SizeChanged(this, new EventArgs());
			}
		}

		//private void pbPreview_SizeChanged(object sender, EventArgs e)
		//{
		//	if (fileImage != null)
		//	{
		//		Size size = pbPreview.Size;
		//		if (rbOriginal.Checked)
		//		{
		//			if (size.Width >= 1 && size.Height >= 1)
		//			{
		//				double num = (double)size.Width / (double)fileImage.Width;
		//				double num2 = (double)size.Height / (double)fileImage.Height;
		//				double num3 = (num < num2) ? num : num2;
		//				size.Width = (int)((double)fileImage.Width * num3);
		//				size.Height = (int)((double)fileImage.Height * num3);
		//				previewImage = new Bitmap(fileImage, size);
		//				pbPreview.Image = previewImage;
		//			}
		//		}
		//		else
		//		{
		//			RedrawPreview(true, false, false);
		//		}
		//	}
		//}

		private void RedrawPreview(bool SizeChanged, bool SettingsChanged, bool FilterChanged)
		{
			if (fileImage != null )
			{
				bool flag = false;
				if (SettingsChanged || adjustmentsChanged)
				{
					//UpdateSettings();
					if (FilterChanged || filterImage == null)
					{
						int width = (int)Math.Ceiling(2.5 * workWidth / spacing);
						int height = (int)Math.Ceiling(2.5 * workHeight / spacing);
						scaledImage = new Bitmap(fileImage, width, height);
						grayImage = Filter.ToGrayscale(scaledImage);
						filterImage = Filter.Filter2(grayImage);
						adjustmentsChanged = true;
					}
					ComputePoints();
					flag = true;
				}
				if (SizeChanged || previewImage == null)
				{
					//if (pbPreview.Size.Width == 0 || pbPreview.Size.Height == 0)
					//{
					//	return;
					//}
					//Size size = pbPreview.Size;
					//double num = (double)size.Width / workWidth;
					//double num2 = (double)size.Height / workHeight;
					//double num3 = (num < num2) ? num : num2;
					//size.Width = (int)(workWidth * num3);
					//size.Height = (int)(workHeight * num3);
					//if (previewImage == null || size.Width != previewImage.Width || size.Height != previewImage.Height)
					//{
					//	flag = true;
					//	if (previewImage != null)
					//	{
					//		previewImage.Dispose();
					//	}
						previewImage = new Bitmap(600, 600, PixelFormat.Format32bppRgb);
					//}
				}
				if (flag)
				{
					DrawPreviewCircles();
				}
				//pbPreview.Image = previewImage;
				//lblStatus.Text = $"{points.NumRows} rows, {points.NumLines} lines, {points.NumPoints} pts";
			}
		}

		private void DrawPreviewCircles()
		{
			Size size = previewImage.Size;
			Color color = invert ? Color.White : Color.Black;
			Brush brush = invert ? Brushes.Black : Brushes.White;
			double num = (double)(float)size.Width / workWidth;
			double num2 = (double)(float)size.Height / workHeight;
			double num3 = (num < num2) ? num : num2;
			Graphics graphics = Graphics.FromImage(previewImage);
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.Clear(color);
			foreach (HTRow row in points.Rows)
			{
				foreach (HTLine line in row.Lines)
				{
					for (int i = 0; i < line.Points.Count; i++)
					{
						HTPoint hTPoint = line.Points[i];
						float num4 = (float)((double)hTPoint.w * num3);
						float num5 = (float)((double)hTPoint.x * num3);
						float num6 = (float)((double)hTPoint.y * num3);
						graphics.FillEllipse(brush, num5 - num4 * 0.5f, num6 - num4 * 0.5f, num4, num4);
					}
				}
			}
			graphics.Flush();
			graphics.Dispose();
		}

		public unsafe void ComputeSourceLevels()
		{
			Rectangle rect = new Rectangle(0, 0, grayImage.Width, grayImage.Height);
			BitmapData bitmapData = grayImage.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
			for (int i = 0; i < 256; i++)
			{
				sourceLevels[i] = 0;
			}
			byte* ptr = (byte*)(void*)bitmapData.Scan0;
			for (int j = 0; j < grayImage.Height; j++)
			{
				byte* ptr2 = ptr;
				for (int k = 0; k < grayImage.Width; k++)
				{
					sourceLevels[ptr2[k]]++;
				}
				ptr += bitmapData.Stride;
			}
			grayImage.UnlockBits(bitmapData);
		}

		public void ComputeAdjustedLevels()
		{
			for (int i = 0; i < 256; i++)
			{
				adjustedLevels[i] = 0;
			}
			for (int j = 0; j < 256; j++)
			{
				float num = (float)j / 255f;
				num = (num - 0.5f + brightness) * contrast + 0.5f;
				num = ((num < 0f) ? 0f : num);
				num = ((num > 1f) ? 1f : num);
				int num2 = (int)(num * 255f + 0.5f);
				adjustedLevels[num2] += sourceLevels[j];
			}
		}

		//private void rbOriginal_CheckedChanged(object sender, EventArgs e)
		//{
		//	if (rbOriginal.Checked)
		//	{
		//		pbPreview_SizeChanged(sender, e);
		//	}
		//}

		//private void rbPreview_CheckedChanged(object sender, EventArgs e)
		//{
		//	if (rbPreview.Checked)
		//	{
		//		RedrawPreview(true, true, true);
		//	}
		//}

		//private void UpdateSettings()
		//{
		//	if (rbHalftone.Checked)
		//	{
		//		style = Style.Dots;
		//	}
		//	else if (rbLines.Checked)
		//	{
		//		style = Style.Lines;
		//	}
		//	else if (rbSquares.Checked)
		//	{
		//		style = Style.Squares;
		//	}
		//	else if (rbCircles.Checked)
		//	{
		//		style = Style.Circles;
		//	}
		//	else if (rbRandom.Checked)
		//	{
		//		style = Style.Random;
		//	}
		//	workWidth = (double)udWidth.Value;
		//	workHeight = (double)udHeight.Value;
		//	border = (double)udBorder.Value;
		//	spacing = (double)udSpacing.Value;
		//	minSize = (double)udMinSize.Value;
		//	maxSize = (double)udMaxSize.Value;
		//	angle = (double)udAngle.Value * 0.017453292519943295;
		//	dotCount = (int)udRandomDots.Value;
		//	wavelength = (double)udWavelength.Value;
		//	amplitude = (double)udAmplitude.Value;
		//	centerOffsX = (double)udCenterOffsX.Value;
		//	centerOffsY = 0.0 - (double)udCenterOffsY.Value;
		//	offsetOdd = (cbOffsetOdd.Checked && style == Style.Dots);
		//	invert = cbInvert.Checked;
		//	gammaCorrect = cbGammaCorrect.Checked;
		//	FixedSizes = cbFixedSize.Checked;
		//}

		private void ComputeDotLookup()
		{
			double num = maxSize * 0.5 * (maxSize * 0.5);
			double num2 = 3.1415926535897931 * num;
			dotLookup = new float[256];
			for (int i = 0; i < 256; i++)
			{
				double num3 = (double)i * 0.00392156862745098;
				if (gammaCorrect)
				{
					num3 = Math.Pow(num3, 0.66666666666666663);
				}
				num3 = (num3 - 0.5 + (double)brightness) * (double)contrast + 0.5;
				if (invert ^ negateImage)
				{
					num3 = 1.0 - num3;
				}
				if (style == Style.Dots || style == Style.Random)
				{
					double num4 = num3 * num2;
					double d = num4 * 0.31830988618379069;
					double num5 = Math.Sqrt(d);
					if (FixedSizes)
					{
						num5 = ((num5 < 2.0) ? 0.0 : ((num5 < 3.5) ? 3.0 : ((!(num5 < 4.5)) ? 5.0 : 4.0)));
					}
					dotLookup[i] = (float)(num5 * 2.0);
				}
				else
				{
					dotLookup[i] = (float)(num3 * maxSize);
				}
			}
		}

		private PointD Rotate(PointD pt, double ang)
		{
			double num = Math.Sin(ang);
			double num2 = Math.Cos(ang);
			PointD pointD = new PointD();
			pointD.X = num2 * pt.X + num * pt.Y;
			pointD.Y = (0.0 - num) * pt.X + num2 * pt.Y;
			return pointD;
		}

		private bool IsLineStyle()
		{
			if (style != Style.Lines && style != Style.Circles)
			{
				return style == Style.Squares;
			}
			return true;
		}

		private void ComputePoints()
		{
			ComputePoints(false);
		}

		private unsafe void ComputePoints(bool IsHighQuality)
		{
			points.Clear();
			double num = spacing;
			double num2 = spacing;
			if (IsLineStyle())
			{
				num2 *= 0.25;
				if (IsHighQuality)
				{
					num2 *= 0.25;
				}
			}
			if (style == Style.Dots && offsetOdd)
			{
				num2 *= 1.235;
			}
			double num3 = Math.Sqrt(workHeight * workHeight + workWidth * workWidth) * 0.5 + amplitude;
			PointD pt = new PointD(0.0 - num3, 0.0 - num3);
			PointD pointD = Rotate(pt, angle);
			PointD pointD2 = Rotate(new PointD(num, 0.0), angle);
			PointD pointD3 = Rotate(new PointD(0.0, num), angle);
			PointD pointD4 = Rotate(new PointD(0.0, 1.0), angle);
			PointD pointD5 = Rotate(new PointD(1.0, 0.0), angle);
			PointD pointD6 = Rotate(new PointD(num2, 0.0), angle);
			PointD pointD7 = Rotate(new PointD(0.0, num2), angle);
			pointD.X = workWidth * 0.5 + pointD.X;
			pointD.Y = workHeight * 0.5 + pointD.Y;
			double num4 = workWidth / (double)filterImage.Width;
			double num5 = workHeight / (double)filterImage.Height;
			double num6 = 1.0 / num4;
			double num7 = 1.0 / num5;
			double num8 = 6.2831853071795862 / (wavelength / num2);
			bool flag = false;
			RectangleF rectangleF = new RectangleF((float)border, (float)border, (float)(workWidth - border * 2.0), (float)(workHeight - border * 2.0));
			ComputeDotLookup();
			num3 += Math.Max(Math.Abs(centerOffsX), Math.Abs(centerOffsY)) * 1.42;
			BitmapData bitmapData = filterImage.LockBits(new Rectangle(0, 0, filterImage.Width, filterImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
			byte* ptr = (byte*)(void*)bitmapData.Scan0;
			if (style == Style.Circles)
			{
				pointD.X = workWidth * 0.5;
				pointD.Y = workHeight * 0.5;
				for (double num9 = 0.0; num9 < num3; num9 += num)
				{
					double num10 = pointD.X + centerOffsX;
					double num11 = pointD.Y + centerOffsY;
					bool flag2 = false;
					int num12 = -2;
					int num13 = (int)(num9 * 2.0 * 3.1415926535897931 / num2 + 0.5);
					double num14 = (double)num13 * num2 / num9 / (double)num13;
					int num15 = 0;
					double num16 = 0.0;
					while (num16 < 6.2831853071795862)
					{
						double num17 = num10 + Math.Sin(num16) * num9;
						double num18 = num11 + Math.Cos(num16) * num9;
						if (rectangleF.Contains((float)num17, (float)num18))
						{
							int num19 = (int)(num17 * num6 + 0.5);
							int num20 = (int)(num18 * num7 + 0.5);
							if (num19 < 0)
							{
								num19 = 0;
							}
							if (num20 < 0)
							{
								num20 = 0;
							}
							if (num19 >= filterImage.Width)
							{
								num19 = filterImage.Width - 1;
							}
							if (num20 >= filterImage.Height)
							{
								num20 = filterImage.Height - 1;
							}
							byte b = ptr[num19 + num20 * bitmapData.Stride];
							float num21 = dotLookup[b];
							if ((double)num21 > minSize)
							{
								if (!flag2)
								{
									points.NewRow();
									flag2 = true;
								}
								if (num12 != num15 - 1)
								{
									points.NewLine();
								}
								points.AddPoint(new HTPoint((float)num17, (float)num18, num21));
								num12 = num15;
							}
						}
						num16 += num14;
						num15++;
					}
				}
			}
			else if (style == Style.Squares)
			{
				pointD.X = workWidth * 0.5;
				pointD.Y = workHeight * 0.5;
				int num22 = 0;
				for (double num23 = 0.0; num23 < num3; num23 += num)
				{
					double num24 = pointD.X + centerOffsX;
					double num25 = pointD.Y + centerOffsY;
					bool flag3 = false;
					int num26 = num22 * 4;
					int num27 = num22 * 4 - 1;
					int num28 = num26 * 2 + num27 * 2;
					PointD pointD8 = pointD4;
					double num29 = (double)(-num27 / 2) * num8;
					int num30 = -2;
					int num31 = 0;
					for (int i = 0; i < num28; i++)
					{
						if (i >= num26)
						{
							if (i < num26 + num27)
							{
								if (num31 == 0)
								{
									num29 = (double)(-num27 / 2 + 1) * num8;
									num30 = -2;
								}
								num31 = 1;
							}
							else if (i < num26 * 2 + num27)
							{
								if (num31 == 1)
								{
									num29 = (double)(-num26 / 2) * num8;
									num30 = -2;
								}
								num31 = 2;
							}
							else
							{
								if (num31 == 2)
								{
									num29 = (double)(-num27 / 2 + 1) * num8;
									num30 = -2;
								}
								num31 = 3;
							}
						}
						double num32 = Math.Sin(num29) * amplitude;
						double num33 = num24 + pointD8.X * num32;
						double num34 = num25 + pointD8.Y * num32;
						if (rectangleF.Contains((float)num33, (float)num34))
						{
							int num35 = (int)(num33 * num6 + 0.5);
							int num36 = (int)(num34 * num7 + 0.5);
							if (num35 < 0)
							{
								num35 = 0;
							}
							if (num36 < 0)
							{
								num36 = 0;
							}
							if (num35 >= filterImage.Width)
							{
								num35 = filterImage.Width - 1;
							}
							if (num36 >= filterImage.Height)
							{
								num36 = filterImage.Height - 1;
							}
							byte b2 = ptr[num35 + num36 * bitmapData.Stride];
							float num37 = dotLookup[b2];
							if ((double)num37 > minSize)
							{
								if (!flag3)
								{
									points.NewRow();
									flag3 = true;
								}
								if (num30 != i - 1)
								{
									points.NewLine();
								}
								points.AddPoint(new HTPoint((float)num33, (float)num34, num37));
								num30 = i;
							}
						}
						if (i < num26)
						{
							num24 += pointD6.X;
							num25 += pointD6.Y;
							pointD8 = pointD4;
						}
						else if (i < num26 + num27)
						{
							num24 += pointD7.X;
							num25 += pointD7.Y;
							pointD8 = pointD5;
						}
						else if (i < num26 * 2 + num27)
						{
							num24 -= pointD6.X;
							num25 -= pointD6.Y;
							pointD8 = pointD4;
						}
						else
						{
							num24 -= pointD7.X;
							num25 -= pointD7.Y;
							pointD8 = pointD5;
						}
						num29 += num8;
					}
					num22 += 2;
					pointD.X -= pointD2.X + pointD3.X;
					pointD.Y -= pointD2.Y + pointD3.Y;
				}
			}
			else if (style == Style.Dots || style == Style.Lines)
			{
				for (double num38 = 0.0 - num3; num38 < num3; num38 += num)
				{
					double num39 = pointD.X;
					double num40 = pointD.Y;
					double num41 = 0.0;
					bool flag4 = false;
					if (offsetOdd & flag)
					{
						num39 += pointD6.X * 0.5;
						num40 += pointD6.Y * 0.5;
						num41 += num8 * 0.5;
					}
					int num42 = -2;
					int num43 = 0;
					double num44 = 0.0 - num3;
					while (num44 < num3)
					{
						double num45 = Math.Sin(num41) * amplitude;
						double num46 = num39 + pointD4.X * num45;
						double num47 = num40 + pointD4.Y * num45;
						if (rectangleF.Contains((float)num46, (float)num47))
						{
							int num48 = (int)(num46 * num6 + 0.5);
							int num49 = (int)(num47 * num7 + 0.5);
							if (num48 < 0)
							{
								num48 = 0;
							}
							if (num49 < 0)
							{
								num49 = 0;
							}
							if (num48 >= filterImage.Width)
							{
								num48 = filterImage.Width - 1;
							}
							if (num49 >= filterImage.Height)
							{
								num49 = filterImage.Height - 1;
							}
							byte b3 = ptr[num48 + num49 * bitmapData.Stride];
							float num50 = dotLookup[b3];
							if ((double)num50 > minSize)
							{
								if (!flag4)
								{
									points.NewRow();
									flag4 = true;
								}
								if (num42 != num43 - 1)
								{
									points.NewLine();
								}
								points.AddPoint(new HTPoint((float)num46, (float)num47, num50));
								num42 = num43;
							}
						}
						num39 += pointD6.X;
						num40 += pointD6.Y;
						num41 += num8;
						num44 += num2;
						num43++;
					}
					pointD.X += pointD3.X;
					pointD.Y += pointD3.Y;
					flag = !flag;
				}
			}
			else if (style == Style.Random)
			{
				Random random = new Random(0);
				for (int j = 0; j < dotCount; j++)
				{
					float num51 = rectangleF.Left + (float)(random.NextDouble() * (double)rectangleF.Width);
					float num52 = rectangleF.Top + (float)(random.NextDouble() * (double)rectangleF.Height);
					int num53 = (int)((double)num51 * num6 + 0.5);
					int num54 = (int)((double)num52 * num7 + 0.5);
					if (num53 < 0)
					{
						num53 = 0;
					}
					if (num54 < 0)
					{
						num54 = 0;
					}
					if (num53 >= filterImage.Width)
					{
						num53 = filterImage.Width - 1;
					}
					if (num54 >= filterImage.Height)
					{
						num54 = filterImage.Height - 1;
					}
					byte b4 = ptr[num53 + num54 * bitmapData.Stride];
					points.AddPoint(new HTPoint(num51, num52, dotLookup[b4]));
				}
			}
			filterImage.UnlockBits(bitmapData);
		}

		private void OptimizePoints()
		{
			if (style == Style.Lines || style == Style.Squares || style == Style.Circles)
			{
				float num = 0.0025f;
				float num2 = 0.0005f;
				if (!imperial)
				{
					num *= 25f;
					num2 *= 25f;
				}
				foreach (HTRow row in points.Rows)
				{
					foreach (HTLine line in row.Lines)
					{
						int num3 = 1;
						while (num3 < line.Points.Count - 1)
						{
							HTPoint hTPoint = line.Points[num3 - 1];
							HTPoint hTPoint2 = line.Points[num3];
							HTPoint a = line.Points[num3 + 1];
							HTPoint hTPoint3 = a - hTPoint;
							HTPoint hTPoint4 = hTPoint2 - hTPoint;
							float length = hTPoint3.Length;
							float length2 = hTPoint4.Length;
							float b = length2 / length;
							HTPoint a2 = hTPoint + hTPoint3 * b;
							HTPoint hTPoint5 = a2 - hTPoint2;
							if (Math.Abs(hTPoint5.x) < num && Math.Abs(hTPoint5.y) < num && Math.Abs(hTPoint5.w) < num2)
							{
								line.Points.RemoveAt(num3);
							}
							else
							{
								num3++;
							}
						}
					}
				}
			}
		}

		//private void udWidth_ValueChanged(object sender, EventArgs e)
		//{
		//	if (fileImage != null && fileImage.Width > 0 && !InternalChange)
		//	{
		//		if (cbLockAspect.Checked)
		//		{
		//			InternalChange = true;
		//			float num = (float)fileImage.Height / (float)fileImage.Width;
		//			udHeight.Value = udWidth.Value * (decimal)num;
		//			InternalChange = false;
		//			RedrawPreview(false, true, true);
		//		}
		//		else
		//		{
		//			RedrawPreview(true, true, true);
		//		}
		//	}
		//}

		//private void udHeight_ValueChanged(object sender, EventArgs e)
		//{
		//	if (fileImage != null && fileImage.Height > 0 && !InternalChange)
		//	{
		//		if (cbLockAspect.Checked)
		//		{
		//			InternalChange = true;
		//			float num = (float)fileImage.Width / (float)fileImage.Height;
		//			udWidth.Value = udHeight.Value * (decimal)num;
		//			InternalChange = false;
		//			RedrawPreview(false, true, true);
		//		}
		//		else
		//		{
		//			RedrawPreview(true, true, true);
		//		}
		//	}
		//}

		//private void cbLockAspect_CheckedChanged(object sender, EventArgs e)
		//{
		//	if (cbLockAspect.Checked && fileImage != null && fileImage.Width > 0 && !InternalChange)
		//	{
		//		InternalChange = true;
		//		float num = (float)fileImage.Height / (float)fileImage.Width;
		//		udHeight.Value = udWidth.Value * (decimal)num;
		//		InternalChange = false;
		//		RedrawPreview(true, true, true);
		//	}
		//}

		//private void Controls_ValueChanged(object sender, EventArgs e)
		//{
		//	if (!InternalChange)
		//	{
		//		cbOffsetOdd.Enabled = rbHalftone.Checked;
		//		cbTwoPassCuts.Enabled = !rbHalftone.Checked;
		//		if (rbCircles.Checked || rbSquares.Checked)
		//		{
		//			udCenterOffsX.Enabled = true;
		//			udCenterOffsY.Enabled = true;
		//		}
		//		else
		//		{
		//			udCenterOffsX.Enabled = false;
		//			udCenterOffsY.Enabled = false;
		//		}
		//		if (!rbPreview.Checked)
		//		{
		//			rbPreview.Checked = true;
		//		}
		//		else
		//		{
		//			RedrawPreview(false, true, false);
		//		}
		//	}
		//}

		//private void udSpacing_ValueChanged(object sender, EventArgs e)
		//{
		//	if (!InternalChange)
		//	{
		//		RedrawPreview(false, true, true);
		//	}
		//}

		//private void btnWriteDXF_Click(object sender, EventArgs e)
		//{
		//	WriteDXF();
		//}

		//private void btnWriteGCode_Click(object sender, EventArgs e)
		//{
		//	OptimizePoints();
		//	WriteGCode();
		//	ComputePoints();
		//}

		//private void WriteDXF()
		//{
		//	WriteDXF(null);
		//}

		private void WriteLine(StreamWriter file, double d)
		{
			file.WriteLine(d.ToString(us));
		}

		private void WriteLine(StreamWriter file, int d)
		{
			file.WriteLine(d.ToString(us));
		}

		private void Write(StreamWriter file, double d)
		{
			file.Write(d.ToString(us));
		}

		private void Write(StreamWriter file, int d)
		{
			file.Write(d.ToString(us));
		}

		//private void WriteDXF(string filename)
		//{
		//	SaveFileDialog saveFileDialog = new SaveFileDialog();
		//	saveFileDialog.Filter = "DXF files|*.dxf|PNG files|*.png|All files (*.*)|*.*";
		//	saveFileDialog.FilterIndex = 1;
		//	saveFileDialog.AddExtension = true;
		//	saveFileDialog.DefaultExt = "dxf";
		//	saveFileDialog.OverwritePrompt = true;
		//	saveFileDialog.FileName = filename;
		//	if (filename != null || saveFileDialog.ShowDialog() == DialogResult.OK)
		//	{
		//		fileMode = (saveFileDialog.FileName.ToLower().EndsWith(".png") ? FileMode.PNG : FileMode.DXF);
		//		if (fileMode == FileMode.PNG)
		//		{
		//			ComputePoints(true);
		//			int width;
		//			int height;
		//			if (imperial)
		//			{
		//				width = (int)(workWidth * 600.0);
		//				height = (int)(workHeight * 600.0);
		//			}
		//			else
		//			{
		//				width = (int)(workWidth * 600.0 / 25.4);
		//				height = (int)(workHeight * 600.0 / 25.4);
		//			}
		//			if (previewImage != null)
		//			{
		//				previewImage.Dispose();
		//			}
		//			previewImage = new Bitmap(width, height, PixelFormat.Format32bppRgb);
		//			previewImage.SetResolution(600f, 600f);
		//			DrawPreviewCircles();
		//			previewImage.Save(saveFileDialog.FileName, ImageFormat.Png);
		//			previewImage.Dispose();
		//			previewImage = null;
		//			RedrawPreview(false, true, false);
		//		}
		//		else
		//		{
		//			StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, false, Encoding.ASCII);
		//			streamWriter.WriteLine(0);
		//			streamWriter.WriteLine("SECTION");
		//			streamWriter.WriteLine(2);
		//			streamWriter.WriteLine("ENTITIES");
		//			if (style == Style.Dots)
		//			{
		//				foreach (HTRow row in points.Rows)
		//				{
		//					foreach (HTLine line in row.Lines)
		//					{
		//						foreach (HTPoint point in line.Points)
		//						{
		//							WriteCircle(streamWriter, point);
		//						}
		//					}
		//				}
		//			}
		//			else if (IsLineStyle())
		//			{
		//				foreach (HTRow row2 in points.Rows)
		//				{
		//					foreach (HTLine line2 in row2.Lines)
		//					{
		//						if (line2.Points.Count == 1)
		//						{
		//							WriteCircle(streamWriter, line2.Points[0]);
		//						}
		//						else
		//						{
		//							PointD pointD = new PointD(0.0, 0.0);
		//							PointD pointD2 = new PointD(0.0, 0.0);
		//							new List<string>();
		//							for (int i = 0; i < line2.Points.Count; i++)
		//							{
		//								HTPoint pt = line2.Points[i];
		//								double lineAngle = 0.0;
		//								double radAngle = 0.0;
		//								double lineAngle2 = 0.0;
		//								double radAngle2 = 0.0;
		//								if (i > 0)
		//								{
		//									ComputeAngleBetween(line2.Points[i - 1], line2.Points[i], out lineAngle, out radAngle);
		//								}
		//								if (i < line2.Points.Count - 1)
		//								{
		//									ComputeAngleBetween(line2.Points[i], line2.Points[i + 1], out lineAngle2, out radAngle2);
		//								}
		//								PointD pointD3;
		//								PointD pointD4;
		//								if (i == 0)
		//								{
		//									double startAngle = lineAngle2 + 1.5707963267948966 + radAngle2;
		//									double endAngle = lineAngle2 + 3.1415926535897931 + 1.5707963267948966 - radAngle2;
		//									pointD3 = ArcPos(pt, startAngle);
		//									pointD4 = ArcPos(pt, endAngle);
		//									WriteArc(streamWriter, pt, startAngle, endAngle);
		//								}
		//								else if (i == line2.Points.Count - 1)
		//								{
		//									double endAngle2 = lineAngle + 1.5707963267948966 + radAngle;
		//									double startAngle2 = lineAngle + 3.1415926535897931 + 1.5707963267948966 - radAngle;
		//									pointD3 = ArcPos(pt, endAngle2);
		//									pointD4 = ArcPos(pt, startAngle2);
		//									WriteLine(streamWriter, pointD, pointD3);
		//									WriteLine(streamWriter, pointD2, pointD4);
		//									WriteArc(streamWriter, pt, startAngle2, endAngle2);
		//								}
		//								else
		//								{
		//									double num = 1.5707963267948966 + lineAngle + radAngle;
		//									double num2 = 4.71238898038469 + lineAngle - radAngle;
		//									double num3 = lineAngle2 + 1.5707963267948966 + radAngle2;
		//									double num4 = lineAngle2 + 3.1415926535897931 + 1.5707963267948966 - radAngle2;
		//									pointD3 = ArcPos(pt, num);
		//									pointD4 = ArcPos(pt, num2);
		//									PointD pointD5 = ArcPos(pt, num3);
		//									PointD pointD6 = ArcPos(pt, num4);
		//									if (num3 > num)
		//									{
		//										PointD l = ArcPos(line2.Points[i + 1], num3);
		//										if (DoLinesIntersect(pointD, pointD3, pointD5, l, out PointD ptIntersection))
		//										{
		//											pointD3 = ptIntersection;
		//											pointD5 = new PointD(ptIntersection.X, ptIntersection.Y);
		//										}
		//									}
		//									else if (num - num3 >= 4.9999998736893758E-05)
		//									{
		//										WriteArc(streamWriter, pt, num3, num);
		//									}
		//									if (num2 > num4)
		//									{
		//										PointD l2 = ArcPos(line2.Points[i + 1], num4);
		//										if (DoLinesIntersect(pointD2, pointD4, pointD6, l2, out PointD ptIntersection2))
		//										{
		//											pointD4 = ptIntersection2;
		//											pointD6 = new PointD(ptIntersection2.X, ptIntersection2.Y);
		//										}
		//									}
		//									else if (num4 - num2 >= 4.9999998736893758E-05)
		//									{
		//										WriteArc(streamWriter, pt, num2, num4);
		//									}
		//									WriteLine(streamWriter, pointD, pointD3);
		//									WriteLine(streamWriter, pointD2, pointD4);
		//									pointD3 = pointD5;
		//									pointD4 = pointD6;
		//								}
		//								pointD = pointD3;
		//								pointD2 = pointD4;
		//							}
		//						}
		//					}
		//				}
		//			}
		//			streamWriter.WriteLine(0);
		//			streamWriter.WriteLine("ENDSEC");
		//			streamWriter.WriteLine(0);
		//			streamWriter.WriteLine("EOF");
		//			streamWriter.Flush();
		//			streamWriter.Close();
		//		}
		//	}
		//}

		private void ComputeAngleBetween(HTPoint a, HTPoint b, out double lineAngle, out double radAngle)
		{
			double num = (double)(b.x - a.x);
			double num2 = (double)(0f - (b.y - a.y));
			double num3 = Math.Sqrt(num * num + num2 * num2);
			lineAngle = Math.Atan2(num2, num);
			if (lineAngle < 0.0)
			{
				lineAngle += 6.2831853071795862;
			}
			double num4 = (double)a.w * 0.5;
			double num5 = (double)b.w * 0.5;
			radAngle = Math.Atan((num5 - num4) / num3);
		}

		private PointD ArcPos(HTPoint pt, double angle)
		{
			PointD pointD = new PointD();
			pointD.X = (double)pt.x + Math.Cos(angle) * (double)pt.w * 0.5;
			pointD.Y = (double)pt.y - Math.Sin(angle) * (double)pt.w * 0.5;
			return pointD;
		}

		private bool DoLinesIntersect(PointD L11, PointD L12, PointD L21, PointD L22, out PointD ptIntersection)
		{
			ptIntersection = new PointD(0.0, 0.0);
			double num = (L22.Y - L21.Y) * (L12.X - L11.X) - (L22.X - L21.X) * (L12.Y - L11.Y);
			double num2 = (L22.X - L21.X) * (L11.Y - L21.Y) - (L22.Y - L21.Y) * (L11.X - L21.X);
			double num3 = (L12.X - L11.X) * (L11.Y - L21.Y) - (L12.Y - L11.Y) * (L11.X - L21.X);
			if (num == 0.0)
			{
				return false;
			}
			double num4 = num2 / num;
			double num5 = num3 / num;
			if (num4 >= 0.0 && num4 <= 1.0 && num5 >= 0.0 && num5 <= 1.0)
			{
				ptIntersection.X = L11.X + num4 * (L12.X - L11.X);
				ptIntersection.Y = L11.Y + num4 * (L12.Y - L11.Y);
				return true;
			}
			return false;
		}

		private void WriteCircle(StreamWriter file, HTPoint pt)
		{
			WriteLine(file, 0);
			file.WriteLine("CIRCLE");
			WriteLine(file, 8);
			WriteLine(file, 0);
			WriteLine(file, 10);
			WriteLine(file, (double)pt.x);
			WriteLine(file, 20);
			WriteLine(file, workHeight - (double)pt.y);
			WriteLine(file, 30);
			WriteLine(file, 0.0);
			WriteLine(file, 40);
			WriteLine(file, (double)(pt.w * 0.5f));
		}

		private void WriteArc(StreamWriter file, HTPoint pt, double startAngle, double endAngle)
		{
			WriteLine(file, 0);
			file.WriteLine("ARC");
			WriteLine(file, 8);
			WriteLine(file, 0);
			WriteLine(file, 10);
			WriteLine(file, (double)pt.x);
			WriteLine(file, 20);
			WriteLine(file, workHeight - (double)pt.y);
			WriteLine(file, 30);
			WriteLine(file, 0.0);
			WriteLine(file, 40);
			WriteLine(file, (double)(pt.w * 0.5f));
			WriteLine(file, 50);
			WriteLine(file, startAngle * 57.295779513082323);
			WriteLine(file, 51);
			WriteLine(file, endAngle * 57.295779513082323);
		}

		private void WriteLine(StreamWriter file, PointD from, PointD to)
		{
			WriteLine(file, 0);
			file.WriteLine("LINE");
			WriteLine(file, 8);
			WriteLine(file, 0);
			WriteLine(file, 10);
			WriteLine(file, from.X);
			WriteLine(file, 20);
			WriteLine(file, workHeight - from.Y);
			WriteLine(file, 30);
			WriteLine(file, 0.0);
			WriteLine(file, 11);
			WriteLine(file, to.X);
			WriteLine(file, 21);
			WriteLine(file, workHeight - to.Y);
			WriteLine(file, 31);
			WriteLine(file, 0.0);
		}

		//private void WriteGCode()
		//{
		//	double num = (double)udSafeZ.Value;
		//	double num2 = (double)udPointRetract.Value;
		//	double a = (double)udToolAngle.Value * 0.017453292519943295 * 0.5;
		//	double num3 = Math.Tan(a);
		//	double num4 = (double)udFeedRate.Value;
		//	double num5 = (double)udSpindleSpeed.Value;
		//	double num6 = (double)udEngraveDepth.Value;
		//	double num7 = (double)udOriginX.Value;
		//	double num8 = (double)udOriginY.Value;
		//	double num9 = (double)udZOffset.Value;
		//	bool @checked = cbTwoPassCuts.Checked;
		//	bool flag = cbIncludeLineNumbers.Checked;
		//	bool checked2 = cbGrblCompat.Checked;
		//	if (checked2)
		//	{
		//		flag = false;
		//	}
		//	if (!cbPointRetract.Checked)
		//	{
		//		num2 = num;
		//	}
		//	SaveFileDialog saveFileDialog = new SaveFileDialog();
		//	saveFileDialog.Filter = "Text files|*.txt|All files (*.*)|*.*";
		//	saveFileDialog.FilterIndex = 1;
		//	saveFileDialog.AddExtension = true;
		//	saveFileDialog.DefaultExt = "txt";
		//	saveFileDialog.OverwritePrompt = true;
		//	if (saveFileDialog.ShowDialog() == DialogResult.OK)
		//	{
		//		StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, false, Encoding.ASCII);
		//		int num10 = 10;
		//		int num11 = (!rbInches.Checked) ? 1 : 0;
		//		if (flag)
		//		{
		//			streamWriter.Write("N{0}0 ", num10++);
		//		}
		//		if (checked2)
		//		{
		//			streamWriter.WriteLine("G00 G2{0} G17 G90 G40", num11);
		//		}
		//		else
		//		{
		//			streamWriter.WriteLine("G00 G2{0} G17 G90 G40 G49 G80", num11);
		//			if (flag)
		//			{
		//				streamWriter.Write("N{0}0 ", num10++);
		//			}
		//			streamWriter.WriteLine("T1 M06");
		//		}
		//		if (flag)
		//		{
		//			streamWriter.Write("N{0}0 ", num10++);
		//		}
		//		streamWriter.WriteLine("G00 Z{0}", num.ToString(us));
		//		if (!checked2)
		//		{
		//			if (flag)
		//			{
		//				streamWriter.Write("N{0}0 ", num10++);
		//			}
		//			streamWriter.WriteLine("S{0} M03", num5.ToString(us));
		//		}
		//		string format = (num11 == 1) ? "0.00" : "0.0000";
		//		string format2 = (num11 == 1) ? "0.000" : "0.0000";
		//		bool flag2 = false;
		//		if (style == Style.Dots)
		//		{
		//			foreach (HTRow row in points.Rows)
		//			{
		//				int num16;
		//				int num17;
		//				int num18;
		//				if (!flag2)
		//				{
		//					num16 = 0;
		//					num17 = row.Lines.Count;
		//					num18 = 1;
		//				}
		//				else
		//				{
		//					num16 = row.Lines.Count - 1;
		//					num17 = -1;
		//					num18 = -1;
		//				}
		//				for (int i = num16; i != num17; i += num18)
		//				{
		//					HTLine hTLine = row.Lines[i];
		//					int num19;
		//					int num20;
		//					int num21;
		//					if (!flag2)
		//					{
		//						num19 = 0;
		//						num20 = hTLine.Points.Count;
		//						num21 = 1;
		//					}
		//					else
		//					{
		//						num19 = hTLine.Points.Count - 1;
		//						num20 = -1;
		//						num21 = -1;
		//					}
		//					for (int j = num19; j != num20; j += num21)
		//					{
		//						HTPoint hTPoint = hTLine.Points[j];
		//						double num22 = (double)hTPoint.x + num7;
		//						double num23 = workHeight - (double)hTPoint.y + num8;
		//						if (flag)
		//						{
		//							streamWriter.Write("N{0}0 ", num10++);
		//						}
		//						streamWriter.WriteLine("G00 X{0} Y{1}", num22.ToString(format, us), num23.ToString(format, us));
		//						double num25 = (double)hTPoint.w * 0.5 / num3 + num6;
		//						if (flag)
		//						{
		//							streamWriter.Write("N{0}0 ", num10++);
		//						}
		//						streamWriter.WriteLine("G00 Z{0}", num9.ToString(format2, us));
		//						if (flag)
		//						{
		//							streamWriter.Write("N{0}0 ", num10++);
		//						}
		//						streamWriter.WriteLine("G01 Z{0} F{1}", (num9 - num25).ToString(format2, us), num4.ToString(us));
		//						if (flag)
		//						{
		//							streamWriter.Write("N{0}0 ", num10++);
		//						}
		//						streamWriter.WriteLine("G00 Z{0}", num2.ToString(us));
		//					}
		//				}
		//				flag2 = !flag2;
		//			}
		//		}
		//		else if (IsLineStyle())
		//		{
		//			int num29 = @checked ? 1 : 0;
		//			for (int k = 0; k < points.Rows.Count << num29; k++)
		//			{
		//				HTRow hTRow = points.Rows[k >> num29];
		//				int num30;
		//				int num31;
		//				int num32;
		//				if (!flag2)
		//				{
		//					num30 = 0;
		//					num31 = hTRow.Lines.Count;
		//					num32 = 1;
		//				}
		//				else
		//				{
		//					num30 = hTRow.Lines.Count - 1;
		//					num31 = -1;
		//					num32 = -1;
		//				}
		//				for (int l = num30; l != num31; l += num32)
		//				{
		//					HTLine hTLine2 = hTRow.Lines[l];
		//					if (hTLine2.Points.Count != 0)
		//					{
		//						int num33;
		//						int num34;
		//						int num35;
		//						if (!flag2)
		//						{
		//							num33 = 0;
		//							num34 = hTLine2.Points.Count;
		//							num35 = 1;
		//						}
		//						else
		//						{
		//							num33 = hTLine2.Points.Count - 1;
		//							num34 = -1;
		//							num35 = -1;
		//						}
		//						HTPoint hTPoint2 = hTLine2.Points[num33];
		//						double num36 = (double)hTPoint2.x + num7;
		//						double num37 = workHeight - (double)hTPoint2.y + num8;
		//						if (flag)
		//						{
		//							streamWriter.Write("N{0}0 ", num10++);
		//						}
		//						streamWriter.WriteLine("G00 X{0} Y{1}", num36.ToString(format, us), num37.ToString(format, us));
		//						if (flag)
		//						{
		//							streamWriter.Write("N{0}0 ", num10++);
		//						}
		//						streamWriter.WriteLine("G00 Z{0}", num9.ToString(format2, us));
		//						double num40 = (double)hTPoint2.w * 0.5 / num3 + num6;
		//						if (flag)
		//						{
		//							streamWriter.Write("N{0}0 ", num10++);
		//						}
		//						streamWriter.WriteLine("G1 X{0} Y{1} Z{2} F{3}", num36.ToString(format, us), num37.ToString(format, us), (num9 - num40).ToString(format2, us), num4.ToString(us));
		//						for (int m = num33; m != num34; m += num35)
		//						{
		//							HTPoint hTPoint3 = hTLine2.Points[m];
		//							num40 = (double)hTPoint3.w * 0.5 / num3 + num6;
		//							num36 = (double)hTPoint3.x + num7;
		//							num37 = workHeight - (double)hTPoint3.y + num8;
		//							if (flag)
		//							{
		//								streamWriter.Write("N{0}0 ", num10++);
		//							}
		//							streamWriter.WriteLine("G1 X{0} Y{1} Z{2}", num36.ToString(format, us), num37.ToString(format, us), (num9 - num40).ToString(format2, us));
		//						}
		//						if (!@checked || flag2 || l + num32 != num31)
		//						{
		//							if (flag)
		//							{
		//								streamWriter.Write("N{0}0 ", num10++);
		//							}
		//							streamWriter.WriteLine("G00 Z{0}", num2.ToString(us));
		//						}
		//					}
		//				}
		//				flag2 = !flag2;
		//			}
		//		}
		//		if (!checked2)
		//		{
		//			if (flag)
		//			{
		//				streamWriter.Write("N{0}0 ", num10++);
		//			}
		//			streamWriter.WriteLine("M05");
		//		}
		//		if (flag)
		//		{
		//			streamWriter.Write("N{0}0 ", num10++);
		//		}
		//		streamWriter.WriteLine("G00 Z{0}", num.ToString(us));
		//		if (flag)
		//		{
		//			streamWriter.Write("N{0}0 ", num10++);
		//		}
		//		streamWriter.WriteLine("M30");
		//		streamWriter.Flush();
		//		streamWriter.Close();
		//	}
		//}

		//private void cbPointRetract_CheckedChanged(object sender, EventArgs e)
		//{
		//	udPointRetract.Enabled = cbPointRetract.Checked;
		//}

		//private void rbInches_CheckedChanged(object sender, EventArgs e)
		//{
		//	ConfigureUnits(rbInches.Checked);
		//}

		//private void ConfigureUpDown(NumericUpDown udCtrl, decimal min, decimal max, int places, decimal step)
		//{
		//	decimal value = udCtrl.Value;
		//	udCtrl.BeginInit();
		//	udCtrl.Minimum = min;
		//	udCtrl.Maximum = max;
		//	udCtrl.DecimalPlaces = places;
		//	udCtrl.Increment = step;
		//	decimal num = imperial ? (value / 25.4m) : (value * 25.4m);
		//	if (num < min)
		//	{
		//		num = min;
		//	}
		//	if (num > max)
		//	{
		//		num = max;
		//	}
		//	udCtrl.Value = num;
		//	udCtrl.EndInit();
		//}

		//private void ConfigureUnits(bool UseImperial)
		//{
		//	InternalChange = true;
		//	imperial = UseImperial;
		//	if (imperial)
		//	{
		//		ConfigureUpDown(udWidth, 4m, 250m, 3, 1m);
		//		ConfigureUpDown(udHeight, 4m, 250m, 3, 1m);
		//		ConfigureUpDown(udCenterOffsX, -500m, 500m, 3, 1m);
		//		ConfigureUpDown(udCenterOffsY, -500m, 500m, 3, 1m);
		//		ConfigureUpDown(udBorder, 0m, 250m, 3, 0.125m);
		//		ConfigureUpDown(udSpacing, 0.001m, 10m, 4, 0.01m);
		//		ConfigureUpDown(udMinSize, 0m, 10m, 4, 0.01m);
		//		ConfigureUpDown(udMaxSize, 0.001m, 10m, 4, 0.01m);
		//		ConfigureUpDown(udWavelength, 0.1m, 20m, 3, 0.125m);
		//		ConfigureUpDown(udAmplitude, -20m, 20m, 3, 0.01m);
		//		ConfigureUpDown(udSafeZ, 0m, 10m, 4, 0.0625m);
		//		ConfigureUpDown(udPointRetract, 0m, 10m, 4, 0.0625m);
		//		lblFeedRate.Text = "Feed (Inch/min)";
		//		ConfigureUpDown(udFeedRate, 0.1m, 1000m, 1, 10m);
		//		ConfigureUpDown(udEngraveDepth, 0m, 1m, 4, 0.005m);
		//		ConfigureUpDown(udZOffset, 0m, 5m, 4, 0.0625m);
		//		ConfigureUpDown(udOriginX, -250m, 250m, 3, 1m);
		//		ConfigureUpDown(udOriginY, -250m, 250m, 3, 1m);
		//	}
		//	else
		//	{
		//		ConfigureUpDown(udWidth, 5m, 6000m, 0, 10m);
		//		ConfigureUpDown(udHeight, 5m, 6000m, 0, 10m);
		//		ConfigureUpDown(udCenterOffsX, -12000m, 12000m, 0, 10m);
		//		ConfigureUpDown(udCenterOffsY, -12000m, 12000m, 0, 10m);
		//		ConfigureUpDown(udBorder, 0m, 6000m, 3, 5m);
		//		ConfigureUpDown(udSpacing, 0.05m, 250m, 2, 0.25m);
		//		ConfigureUpDown(udMinSize, 0m, 250m, 2, 0.25m);
		//		ConfigureUpDown(udMaxSize, 0.05m, 250m, 2, 0.25m);
		//		ConfigureUpDown(udWavelength, 1m, 600m, 1, 2.5m);
		//		ConfigureUpDown(udAmplitude, -600m, 600m, 1, 0.25m);
		//		ConfigureUpDown(udSafeZ, 0m, 250m, 1, 2m);
		//		ConfigureUpDown(udPointRetract, 0m, 250m, 1, 2m);
		//		lblFeedRate.Text = "Feed (mm/min)";
		//		ConfigureUpDown(udFeedRate, 1m, 25000m, 1, 250m);
		//		ConfigureUpDown(udEngraveDepth, 0m, 25m, 2, 0.1m);
		//		ConfigureUpDown(udZOffset, 0m, 150m, 1, 2m);
		//		ConfigureUpDown(udOriginX, -6000m, 6000m, 0, 20m);
		//		ConfigureUpDown(udOriginY, -6000m, 6000m, 0, 20m);
		//	}
		//	InternalChange = false;
		//	RedrawPreview(false, true, false);
		//}

		private string GetConfigName()
		{
            //string executablePath = Application.ExecutablePath;
            //string directoryName = Path.GetDirectoryName(executablePath);
            //return directoryName + "\\Halftoner.cfg";
            return "";
		}

		private void WriteSetting(XmlWriter cfg, string Name, string Value)
		{
			cfg.WriteStartElement(Name);
			cfg.WriteAttributeString("Value", Value);
			cfg.WriteEndElement();
		}

		//private void SaveSettings()
		//{
		//	try
		//	{
		//		string configName = GetConfigName();
		//		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		//		xmlWriterSettings.NewLineChars = "\n";
		//		xmlWriterSettings.Indent = true;
		//		XmlWriter xmlWriter = XmlWriter.Create(configName, xmlWriterSettings);
		//		xmlWriter.WriteStartDocument();
		//		xmlWriter.WriteStartElement("HalftoneConfig");
		//		WriteSetting(xmlWriter, "Units", rbInches.Checked ? "inch" : "mm");
		//		WriteSetting(xmlWriter, "Width", udWidth.Value.ToString());
		//		WriteSetting(xmlWriter, "Height", udHeight.Value.ToString());
		//		WriteSetting(xmlWriter, "Border", udBorder.Value.ToString());
		//		WriteSetting(xmlWriter, "Spacing", udSpacing.Value.ToString());
		//		WriteSetting(xmlWriter, "MinSize", udMinSize.Value.ToString());
		//		WriteSetting(xmlWriter, "MaxSize", udMaxSize.Value.ToString());
		//		WriteSetting(xmlWriter, "Angle", udAngle.Value.ToString());
		//		WriteSetting(xmlWriter, "Wavelength", udWavelength.Value.ToString());
		//		WriteSetting(xmlWriter, "Amplitude", udAmplitude.Value.ToString());
		//		WriteSetting(xmlWriter, "CenterOffsX", udCenterOffsX.Value.ToString());
		//		WriteSetting(xmlWriter, "CenterOffsY", udCenterOffsY.Value.ToString());
		//		WriteSetting(xmlWriter, "DarkBoost", cbGammaCorrect.Checked.ToString());
		//		WriteSetting(xmlWriter, "OffsetOdd", cbOffsetOdd.Checked.ToString());
		//		WriteSetting(xmlWriter, "Invert", cbInvert.Checked.ToString());
		//		WriteSetting(xmlWriter, "Style", style.ToString());
		//		WriteSetting(xmlWriter, "RandomDots", udRandomDots.Value.ToString());
		//		WriteSetting(xmlWriter, "SafeZ", udSafeZ.Value.ToString());
		//		WriteSetting(xmlWriter, "UsePointRetract", cbPointRetract.Checked.ToString());
		//		WriteSetting(xmlWriter, "PointRetract", udPointRetract.Value.ToString());
		//		WriteSetting(xmlWriter, "FeedRate", udFeedRate.Value.ToString());
		//		WriteSetting(xmlWriter, "ToolAngle", udToolAngle.Value.ToString());
		//		WriteSetting(xmlWriter, "SpindleRPM", udSpindleSpeed.Value.ToString());
		//		WriteSetting(xmlWriter, "EngraveDepth", udEngraveDepth.Value.ToString());
		//		WriteSetting(xmlWriter, "TwoPass", cbTwoPassCuts.Checked.ToString());
		//		WriteSetting(xmlWriter, "LineNumbers", cbIncludeLineNumbers.Checked.ToString());
		//		WriteSetting(xmlWriter, "GrblCompat", cbGrblCompat.Checked.ToString());
		//		WriteSetting(xmlWriter, "ZOffset", udZOffset.Value.ToString());
		//		WriteSetting(xmlWriter, "OriginX", udOriginX.Value.ToString());
		//		WriteSetting(xmlWriter, "OriginY", udOriginY.Value.ToString());
		//		WriteSetting(xmlWriter, "LockAspect", cbLockAspect.Checked.ToString());
		//		xmlWriter.WriteEndElement();
		//		xmlWriter.Flush();
		//		xmlWriter.Close();
		//	}
		//	catch (Exception)
		//	{
		//	}
		//}

		private string GetStringSetting(XmlElement cfg, string Name)
		{
			return cfg[Name]?.GetAttribute("Value");
		}

		//private void GetBoolSetting(CheckBox cbControl, XmlElement cfg, string Name)
		//{
		//	string stringSetting = GetStringSetting(cfg, Name);
		//	if (stringSetting != null)
		//	{
		//		cbControl.Checked = bool.Parse(stringSetting);
		//	}
		//}

		//private void GetDecimalSetting(NumericUpDown udControl, XmlElement cfg, string Name)
		//{
		//	string stringSetting = GetStringSetting(cfg, Name);
		//	if (stringSetting != null)
		//	{
		//		udControl.Value = decimal.Parse(stringSetting);
		//	}
		//}

		private void LoadSettings()
		{
			//try
			//{
			//	XmlDocument xmlDocument = new XmlDocument();
			//	xmlDocument.Load(GetConfigName());
			//	XmlElement xmlElement = xmlDocument["HalftoneConfig"];
			//	if (xmlElement != null)
			//	{
			//		string stringSetting = GetStringSetting(xmlElement, "Units");
			//		//if (stringSetting == "inch")
			//		//{
			//		//	rbInches.Checked = true;
			//		//}
			//		//if (stringSetting == "mm")
			//		//{
			//		//	rbMillimeters.Checked = true;
			//		//}
			//		InternalChange = true;
			//		GetDecimalSetting(udWidth, xmlElement, "Width");
			//		GetDecimalSetting(udHeight, xmlElement, "Height");
			//		GetDecimalSetting(udBorder, xmlElement, "Border");
			//		GetDecimalSetting(udSpacing, xmlElement, "Spacing");
			//		GetDecimalSetting(udMinSize, xmlElement, "MinSize");
			//		GetDecimalSetting(udMaxSize, xmlElement, "MaxSize");
			//		GetDecimalSetting(udAngle, xmlElement, "Angle");
			//		GetDecimalSetting(udWavelength, xmlElement, "Wavelength");
			//		GetDecimalSetting(udAmplitude, xmlElement, "Amplitude");
			//		GetDecimalSetting(udCenterOffsX, xmlElement, "CenterOffsX");
			//		GetDecimalSetting(udCenterOffsY, xmlElement, "CenterOffsY");
			//		GetBoolSetting(cbGammaCorrect, xmlElement, "DarkBoost");
			//		GetBoolSetting(cbOffsetOdd, xmlElement, "OffsetOdd");
			//		GetBoolSetting(cbInvert, xmlElement, "Invert");
			//		GetDecimalSetting(udRandomDots, xmlElement, "RandomDots");
			//		string stringSetting2 = GetStringSetting(xmlElement, "Style");
			//		switch (stringSetting2)
			//		{
			//		case "Dots":
			//			rbHalftone.Checked = true;
			//			style = Style.Dots;
			//			break;
			//		case "Lines":
			//			rbLines.Checked = true;
						style = Style.Lines;
			//			break;
			//		case "Squares":
			//			rbSquares.Checked = true;
			//			style = Style.Squares;
			//			break;
			//		case "Circles":
			//			rbCircles.Checked = true;
			//			style = Style.Circles;
			//			break;
			//		}
			//		GetDecimalSetting(udSafeZ, xmlElement, "SafeZ");
			//		GetBoolSetting(cbPointRetract, xmlElement, "UsePointRetract");
			//		GetDecimalSetting(udPointRetract, xmlElement, "PointRetract");
			//		GetDecimalSetting(udFeedRate, xmlElement, "FeedRate");
			//		GetDecimalSetting(udToolAngle, xmlElement, "ToolAngle");
			//		GetDecimalSetting(udSpindleSpeed, xmlElement, "SpindleRPM");
			//		GetDecimalSetting(udEngraveDepth, xmlElement, "EngraveDepth");
			//		GetBoolSetting(cbTwoPassCuts, xmlElement, "TwoPass");
			//		GetBoolSetting(cbIncludeLineNumbers, xmlElement, "LineNumbers");
			//		GetBoolSetting(cbGrblCompat, xmlElement, "GrblCompat");
			//		GetDecimalSetting(udZOffset, xmlElement, "ZOffset");
			//		GetDecimalSetting(udOriginX, xmlElement, "OriginX");
			//		GetDecimalSetting(udOriginY, xmlElement, "OriginY");
			//		GetBoolSetting(cbLockAspect, xmlElement, "LockAspect");
			//		InternalChange = false;
			//	}
			//}
			//catch (Exception)
			//{
			//}
			//finally
			//{
			//	InternalChange = false;
			//}
		}

		//private void btnTest_Click(object sender, EventArgs e)
		//{
		//	WriteDXF("C:\\Users\\Jason\\Desktop\\TestDXF.dxf");
		//}

		//private void Controls_AdjustmentsChanged(object sender, EventArgs e)
		//{
		//	if (!InternalChange)
		//	{
		//		lblBright.Text = (tbBright.Value * 2).ToString();
		//		lblContrast.Text = (tbContrast.Value * 2).ToString();
		//		brightness = (float)tbBright.Value / 50f;
		//		contrast = (float)tbContrast.Value / 50f;
		//		contrast = 1.01f * (contrast + 1f) / (1.01f - contrast);
		//		adjustmentsChanged = true;
		//		ComputeAdjustedLevels();
		//		if (!rbPreview.Checked)
		//		{
		//			rbPreview.Checked = true;
		//		}
		//		else
		//		{
		//			RedrawPreview(false, true, false);
		//		}
		//	}
		//}

		//private void btnAutoLevels_Click(object sender, EventArgs e)
		//{
		//	float num = 1f;
		//	float num2 = 0f;
		//	for (int i = 0; i < 256; i++)
		//	{
		//		float num3 = (float)i / 255f;
		//		if (sourceLevels[i] > 0)
		//		{
		//			if (num > num3)
		//			{
		//				num = num3;
		//			}
		//			if (num2 < num3)
		//			{
		//				num2 = num3;
		//			}
		//		}
		//	}
		//	float num4 = (num + num2) * 0.5f;
		//	float num5 = 0.5f - num4;
		//	float num6 = num2 - num;
		//	float num7 = 1f / num6;
		//	num7 = 101f * (num7 - 1f) / (100f * num7 + 101f);
		//	InternalChange = true;
		//	tbBright.Value = (int)(num5 * 50f);
		//	tbContrast.Value = (int)(num7 * 50f);
		//	InternalChange = false;
		//	Controls_AdjustmentsChanged(null, null);
		//}

		//private void cbNegateImage_CheckedChanged(object sender, EventArgs e)
		//{
		//	negateImage = cbNegateImage.Checked;
		//	Controls_AdjustmentsChanged(null, null);
		//}

		//protected override void Dispose(bool disposing)
		//{
		//	if (disposing && components != null)
		//	{
		//		components.Dispose();
		//	}
		//	base.Dispose(disposing);
		//}

		//private void InitializeComponent()
		//{
		//	components = new System.ComponentModel.Container();
		//	System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(Halftoner.MainForm));
		//	pbPreview = new System.Windows.Forms.PictureBox();
		//	rbPreview = new System.Windows.Forms.RadioButton();
		//	rbOriginal = new System.Windows.Forms.RadioButton();
		//	udWidth = new System.Windows.Forms.NumericUpDown();
		//	label1 = new System.Windows.Forms.Label();
		//	label2 = new System.Windows.Forms.Label();
		//	udHeight = new System.Windows.Forms.NumericUpDown();
		//	label3 = new System.Windows.Forms.Label();
		//	udSpacing = new System.Windows.Forms.NumericUpDown();
		//	label4 = new System.Windows.Forms.Label();
		//	udBorder = new System.Windows.Forms.NumericUpDown();
		//	label5 = new System.Windows.Forms.Label();
		//	udMaxSize = new System.Windows.Forms.NumericUpDown();
		//	label6 = new System.Windows.Forms.Label();
		//	udMinSize = new System.Windows.Forms.NumericUpDown();
		//	label7 = new System.Windows.Forms.Label();
		//	udAngle = new System.Windows.Forms.NumericUpDown();
		//	btnLoadImage = new System.Windows.Forms.Button();
		//	lblStatus = new System.Windows.Forms.Label();
		//	tabControl1 = new System.Windows.Forms.TabControl();
		//	tabPage1 = new System.Windows.Forms.TabPage();
		//	label17 = new System.Windows.Forms.Label();
		//	udCenterOffsX = new System.Windows.Forms.NumericUpDown();
		//	udCenterOffsY = new System.Windows.Forms.NumericUpDown();
		//	label18 = new System.Windows.Forms.Label();
		//	lblRandomDots = new System.Windows.Forms.Label();
		//	udRandomDots = new System.Windows.Forms.NumericUpDown();
		//	cbLockAspect = new System.Windows.Forms.CheckBox();
		//	cbFixedSize = new System.Windows.Forms.CheckBox();
		//	cbGammaCorrect = new System.Windows.Forms.CheckBox();
		//	label9 = new System.Windows.Forms.Label();
		//	udAmplitude = new System.Windows.Forms.NumericUpDown();
		//	label15 = new System.Windows.Forms.Label();
		//	udWavelength = new System.Windows.Forms.NumericUpDown();
		//	cbOffsetOdd = new System.Windows.Forms.CheckBox();
		//	cbInvert = new System.Windows.Forms.CheckBox();
		//	tabPage2 = new System.Windows.Forms.TabPage();
		//	cbIncludeLineNumbers = new System.Windows.Forms.CheckBox();
		//	label16 = new System.Windows.Forms.Label();
		//	udZOffset = new System.Windows.Forms.NumericUpDown();
		//	label11 = new System.Windows.Forms.Label();
		//	udEngraveDepth = new System.Windows.Forms.NumericUpDown();
		//	cbTwoPassCuts = new System.Windows.Forms.CheckBox();
		//	cbPointRetract = new System.Windows.Forms.CheckBox();
		//	udSpindleSpeed = new System.Windows.Forms.NumericUpDown();
		//	label14 = new System.Windows.Forms.Label();
		//	udOriginX = new System.Windows.Forms.NumericUpDown();
		//	label12 = new System.Windows.Forms.Label();
		//	udOriginY = new System.Windows.Forms.NumericUpDown();
		//	label13 = new System.Windows.Forms.Label();
		//	udFeedRate = new System.Windows.Forms.NumericUpDown();
		//	lblFeedRate = new System.Windows.Forms.Label();
		//	btnWriteGCode = new System.Windows.Forms.Button();
		//	label8 = new System.Windows.Forms.Label();
		//	udSafeZ = new System.Windows.Forms.NumericUpDown();
		//	udPointRetract = new System.Windows.Forms.NumericUpDown();
		//	udToolAngle = new System.Windows.Forms.NumericUpDown();
		//	label10 = new System.Windows.Forms.Label();
		//	tabPage3 = new System.Windows.Forms.TabPage();
		//	lblWriteDXFHelp = new System.Windows.Forms.Label();
		//	btnWriteDXF = new System.Windows.Forms.Button();
		//	tabPage4 = new System.Windows.Forms.TabPage();
		//	cbNegateImage = new System.Windows.Forms.CheckBox();
		//	lblContrast = new System.Windows.Forms.Label();
		//	lblBright = new System.Windows.Forms.Label();
		//	btnAutoLevels = new System.Windows.Forms.Button();
		//	label20 = new System.Windows.Forms.Label();
		//	label19 = new System.Windows.Forms.Label();
		//	tbContrast = new System.Windows.Forms.TrackBar();
		//	tbBright = new System.Windows.Forms.TrackBar();
		//	rbMillimeters = new System.Windows.Forms.RadioButton();
		//	rbInches = new System.Windows.Forms.RadioButton();
		//	lblDirections = new System.Windows.Forms.Label();
		//	groupBox1 = new System.Windows.Forms.GroupBox();
		//	rbCircles = new System.Windows.Forms.RadioButton();
		//	rbRandom = new System.Windows.Forms.RadioButton();
		//	rbSquares = new System.Windows.Forms.RadioButton();
		//	rbLines = new System.Windows.Forms.RadioButton();
		//	rbHalftone = new System.Windows.Forms.RadioButton();
		//	groupBox2 = new System.Windows.Forms.GroupBox();
		//	ttHelpTip = new System.Windows.Forms.ToolTip(components);
		//	btnTest = new System.Windows.Forms.Button();
		//	cbGrblCompat = new System.Windows.Forms.CheckBox();
		//	((System.ComponentModel.ISupportInitialize)pbPreview).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udWidth).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udHeight).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udSpacing).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udBorder).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udMaxSize).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udMinSize).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udAngle).BeginInit();
		//	tabControl1.SuspendLayout();
		//	tabPage1.SuspendLayout();
		//	((System.ComponentModel.ISupportInitialize)udCenterOffsX).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udCenterOffsY).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udRandomDots).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udAmplitude).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udWavelength).BeginInit();
		//	tabPage2.SuspendLayout();
		//	((System.ComponentModel.ISupportInitialize)udZOffset).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udEngraveDepth).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udSpindleSpeed).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udOriginX).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udOriginY).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udFeedRate).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udSafeZ).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udPointRetract).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)udToolAngle).BeginInit();
		//	tabPage3.SuspendLayout();
		//	tabPage4.SuspendLayout();
		//	((System.ComponentModel.ISupportInitialize)tbContrast).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)tbBright).BeginInit();
		//	groupBox1.SuspendLayout();
		//	groupBox2.SuspendLayout();
		//	SuspendLayout();
		//	pbPreview.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
		//	pbPreview.Location = new System.Drawing.Point(215, 12);
		//	pbPreview.Name = "pbPreview";
		//	pbPreview.Size = new System.Drawing.Size(405, 434);
		//	pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		//	pbPreview.TabIndex = 0;
		//	pbPreview.TabStop = false;
		//	pbPreview.SizeChanged += new System.EventHandler(pbPreview_SizeChanged);
		//	rbPreview.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
		//	rbPreview.AutoSize = true;
		//	rbPreview.Location = new System.Drawing.Point(215, 453);
		//	rbPreview.Name = "rbPreview";
		//	rbPreview.Size = new System.Drawing.Size(63, 17);
		//	rbPreview.TabIndex = 0;
		//	rbPreview.TabStop = true;
		//	rbPreview.Text = "Preview";
		//	rbPreview.UseVisualStyleBackColor = true;
		//	rbPreview.CheckedChanged += new System.EventHandler(rbPreview_CheckedChanged);
		//	rbOriginal.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
		//	rbOriginal.AutoSize = true;
		//	rbOriginal.Checked = true;
		//	rbOriginal.Location = new System.Drawing.Point(284, 453);
		//	rbOriginal.Name = "rbOriginal";
		//	rbOriginal.Size = new System.Drawing.Size(92, 17);
		//	rbOriginal.TabIndex = 1;
		//	rbOriginal.TabStop = true;
		//	rbOriginal.Text = "Original Image";
		//	rbOriginal.UseVisualStyleBackColor = true;
		//	rbOriginal.CheckedChanged += new System.EventHandler(rbOriginal_CheckedChanged);
		//	udWidth.DecimalPlaces = 3;
		//	udWidth.Location = new System.Drawing.Point(11, 40);
		//	udWidth.Maximum = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		0
		//	});
		//	udWidth.Minimum = new decimal(new int[4]
		//	{
		//		5,
		//		0,
		//		0,
		//		65536
		//	});
		//	udWidth.Name = "udWidth";
		//	udWidth.Size = new System.Drawing.Size(71, 20);
		//	udWidth.TabIndex = 0;
		//	ttHelpTip.SetToolTip(udWidth, "Width of the work area");
		//	udWidth.Value = new decimal(new int[4]
		//	{
		//		12,
		//		0,
		//		0,
		//		0
		//	});
		//	udWidth.ValueChanged += new System.EventHandler(udWidth_ValueChanged);
		//	label1.AutoSize = true;
		//	label1.Location = new System.Drawing.Point(8, 25);
		//	label1.Name = "label1";
		//	label1.Size = new System.Drawing.Size(35, 13);
		//	label1.TabIndex = 4;
		//	label1.Text = "Width";
		//	label2.AutoSize = true;
		//	label2.Location = new System.Drawing.Point(109, 25);
		//	label2.Name = "label2";
		//	label2.Size = new System.Drawing.Size(38, 13);
		//	label2.TabIndex = 6;
		//	label2.Text = "Height";
		//	udHeight.DecimalPlaces = 3;
		//	udHeight.Location = new System.Drawing.Point(112, 40);
		//	udHeight.Maximum = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		0
		//	});
		//	udHeight.Minimum = new decimal(new int[4]
		//	{
		//		5,
		//		0,
		//		0,
		//		65536
		//	});
		//	udHeight.Name = "udHeight";
		//	udHeight.Size = new System.Drawing.Size(71, 20);
		//	udHeight.TabIndex = 1;
		//	ttHelpTip.SetToolTip(udHeight, "Height of the work area");
		//	udHeight.Value = new decimal(new int[4]
		//	{
		//		12,
		//		0,
		//		0,
		//		0
		//	});
		//	udHeight.ValueChanged += new System.EventHandler(udHeight_ValueChanged);
		//	label3.AutoSize = true;
		//	label3.Location = new System.Drawing.Point(109, 65);
		//	label3.Name = "label3";
		//	label3.Size = new System.Drawing.Size(46, 13);
		//	label3.TabIndex = 10;
		//	label3.Text = "Spacing";
		//	udSpacing.DecimalPlaces = 4;
		//	udSpacing.Increment = new decimal(new int[4]
		//	{
		//		1,
		//		0,
		//		0,
		//		131072
		//	});
		//	udSpacing.Location = new System.Drawing.Point(112, 80);
		//	udSpacing.Maximum = new decimal(new int[4]
		//	{
		//		10,
		//		0,
		//		0,
		//		0
		//	});
		//	udSpacing.Minimum = new decimal(new int[4]
		//	{
		//		1,
		//		0,
		//		0,
		//		196608
		//	});
		//	udSpacing.Name = "udSpacing";
		//	udSpacing.Size = new System.Drawing.Size(71, 20);
		//	udSpacing.TabIndex = 3;
		//	ttHelpTip.SetToolTip(udSpacing, "Distance between neighboring dots or lines");
		//	udSpacing.Value = new decimal(new int[4]
		//	{
		//		125,
		//		0,
		//		0,
		//		196608
		//	});
		//	udSpacing.ValueChanged += new System.EventHandler(udSpacing_ValueChanged);
		//	label4.AutoSize = true;
		//	label4.Location = new System.Drawing.Point(8, 65);
		//	label4.Name = "label4";
		//	label4.Size = new System.Drawing.Size(38, 13);
		//	label4.TabIndex = 8;
		//	label4.Text = "Border";
		//	udBorder.DecimalPlaces = 3;
		//	udBorder.Increment = new decimal(new int[4]
		//	{
		//		125,
		//		0,
		//		0,
		//		196608
		//	});
		//	udBorder.Location = new System.Drawing.Point(11, 80);
		//	udBorder.Maximum = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		0
		//	});
		//	udBorder.Name = "udBorder";
		//	udBorder.Size = new System.Drawing.Size(71, 20);
		//	udBorder.TabIndex = 2;
		//	ttHelpTip.SetToolTip(udBorder, "Amount of border to leave blank around the image (note that this excludes part of the image)");
		//	udBorder.Value = new decimal(new int[4]
		//	{
		//		25,
		//		0,
		//		0,
		//		131072
		//	});
		//	udBorder.ValueChanged += new System.EventHandler(Controls_ValueChanged);
		//	label5.AutoSize = true;
		//	label5.Location = new System.Drawing.Point(109, 106);
		//	label5.Name = "label5";
		//	label5.Size = new System.Drawing.Size(50, 13);
		//	label5.TabIndex = 14;
		//	label5.Text = "Max Size";
		//	udMaxSize.DecimalPlaces = 4;
		//	udMaxSize.Increment = new decimal(new int[4]
		//	{
		//		1,
		//		0,
		//		0,
		//		131072
		//	});
		//	udMaxSize.Location = new System.Drawing.Point(112, 121);
		//	udMaxSize.Maximum = new decimal(new int[4]
		//	{
		//		10,
		//		0,
		//		0,
		//		0
		//	});
		//	udMaxSize.Minimum = new decimal(new int[4]
		//	{
		//		1,
		//		0,
		//		0,
		//		196608
		//	});
		//	udMaxSize.Name = "udMaxSize";
		//	udMaxSize.Size = new System.Drawing.Size(71, 20);
		//	udMaxSize.TabIndex = 5;
		//	ttHelpTip.SetToolTip(udMaxSize, "Maximum width of a dot or line");
		//	udMaxSize.Value = new decimal(new int[4]
		//	{
		//		1250,
		//		0,
		//		0,
		//		262144
		//	});
		//	udMaxSize.ValueChanged += new System.EventHandler(Controls_ValueChanged);
		//	label6.AutoSize = true;
		//	label6.Location = new System.Drawing.Point(8, 106);
		//	label6.Name = "label6";
		//	label6.Size = new System.Drawing.Size(47, 13);
		//	label6.TabIndex = 12;
		//	label6.Text = "Min Size";
		//	udMinSize.DecimalPlaces = 4;
		//	udMinSize.Increment = new decimal(new int[4]
		//	{
		//		1,
		//		0,
		//		0,
		//		131072
		//	});
		//	udMinSize.Location = new System.Drawing.Point(11, 121);
		//	udMinSize.Maximum = new decimal(new int[4]
		//	{
		//		10,
		//		0,
		//		0,
		//		0
		//	});
		//	udMinSize.Name = "udMinSize";
		//	udMinSize.Size = new System.Drawing.Size(71, 20);
		//	udMinSize.TabIndex = 4;
		//	ttHelpTip.SetToolTip(udMinSize, "Dots smaller than this value will be discarded (0 = keep all)");
		//	udMinSize.ValueChanged += new System.EventHandler(Controls_ValueChanged);
		//	label7.AutoSize = true;
		//	label7.Location = new System.Drawing.Point(8, 147);
		//	label7.Name = "label7";
		//	label7.Size = new System.Drawing.Size(34, 13);
		//	label7.TabIndex = 16;
		//	label7.Text = "Angle";
		//	udAngle.DecimalPlaces = 1;
		//	udAngle.Increment = new decimal(new int[4]
		//	{
		//		5,
		//		0,
		//		0,
		//		0
		//	});
		//	udAngle.Location = new System.Drawing.Point(11, 162);
		//	udAngle.Maximum = new decimal(new int[4]
		//	{
		//		90,
		//		0,
		//		0,
		//		0
		//	});
		//	udAngle.Minimum = new decimal(new int[4]
		//	{
		//		90,
		//		0,
		//		0,
		//		-2147483648
		//	});
		//	udAngle.Name = "udAngle";
		//	udAngle.Size = new System.Drawing.Size(71, 20);
		//	udAngle.TabIndex = 6;
		//	ttHelpTip.SetToolTip(udAngle, "Tilt the dot or lines by this many degrees");
		//	udAngle.Value = new decimal(new int[4]
		//	{
		//		450,
		//		0,
		//		0,
		//		65536
		//	});
		//	udAngle.ValueChanged += new System.EventHandler(Controls_ValueChanged);
		//	btnLoadImage.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
		//	btnLoadImage.Location = new System.Drawing.Point(31, 319);
		//	btnLoadImage.Name = "btnLoadImage";
		//	btnLoadImage.Size = new System.Drawing.Size(128, 23);
		//	btnLoadImage.TabIndex = 12;
		//	btnLoadImage.Text = "Load Image";
		//	btnLoadImage.UseVisualStyleBackColor = true;
		//	btnLoadImage.Click += new System.EventHandler(btnLoadImage_Click);
		//	lblStatus.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
		//	lblStatus.Location = new System.Drawing.Point(382, 457);
		//	lblStatus.Name = "lblStatus";
		//	lblStatus.Size = new System.Drawing.Size(238, 20);
		//	lblStatus.TabIndex = 18;
		//	tabControl1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
		//	tabControl1.Controls.Add(tabPage1);
		//	tabControl1.Controls.Add(tabPage2);
		//	tabControl1.Controls.Add(tabPage3);
		//	tabControl1.Controls.Add(tabPage4);
		//	tabControl1.Location = new System.Drawing.Point(3, 3);
		//	tabControl1.Name = "tabControl1";
		//	tabControl1.SelectedIndex = 0;
		//	tabControl1.Size = new System.Drawing.Size(206, 374);
		//	tabControl1.TabIndex = 0;
		//	tabPage1.Controls.Add(label17);
		//	tabPage1.Controls.Add(udCenterOffsX);
		//	tabPage1.Controls.Add(udCenterOffsY);
		//	tabPage1.Controls.Add(label18);
		//	tabPage1.Controls.Add(lblRandomDots);
		//	tabPage1.Controls.Add(udRandomDots);
		//	tabPage1.Controls.Add(cbLockAspect);
		//	tabPage1.Controls.Add(cbFixedSize);
		//	tabPage1.Controls.Add(cbGammaCorrect);
		//	tabPage1.Controls.Add(label9);
		//	tabPage1.Controls.Add(udAmplitude);
		//	tabPage1.Controls.Add(label15);
		//	tabPage1.Controls.Add(udWavelength);
		//	tabPage1.Controls.Add(cbOffsetOdd);
		//	tabPage1.Controls.Add(cbInvert);
		//	tabPage1.Controls.Add(label1);
		//	tabPage1.Controls.Add(udWidth);
		//	tabPage1.Controls.Add(btnLoadImage);
		//	tabPage1.Controls.Add(udHeight);
		//	tabPage1.Controls.Add(label7);
		//	tabPage1.Controls.Add(label2);
		//	tabPage1.Controls.Add(udAngle);
		//	tabPage1.Controls.Add(udBorder);
		//	tabPage1.Controls.Add(label5);
		//	tabPage1.Controls.Add(label4);
		//	tabPage1.Controls.Add(udMaxSize);
		//	tabPage1.Controls.Add(udSpacing);
		//	tabPage1.Controls.Add(label6);
		//	tabPage1.Controls.Add(label3);
		//	tabPage1.Controls.Add(udMinSize);
		//	tabPage1.Location = new System.Drawing.Point(4, 22);
		//	tabPage1.Name = "tabPage1";
		//	tabPage1.Padding = new System.Windows.Forms.Padding(3);
		//	tabPage1.Size = new System.Drawing.Size(198, 348);
		//	tabPage1.TabIndex = 0;
		//	tabPage1.Text = "Generator";
		//	tabPage1.UseVisualStyleBackColor = true;
		//	label17.AutoSize = true;
		//	label17.Location = new System.Drawing.Point(8, 229);
		//	label17.Name = "label17";
		//	label17.Size = new System.Drawing.Size(77, 13);
		//	label17.TabIndex = 27;
		//	label17.Text = "Center offset X";
		//	udCenterOffsX.DecimalPlaces = 3;
		//	udCenterOffsX.Location = new System.Drawing.Point(11, 244);
		//	udCenterOffsX.Maximum = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		0
		//	});
		//	udCenterOffsX.Minimum = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		-2147483648
		//	});
		//	udCenterOffsX.Name = "udCenterOffsX";
		//	udCenterOffsX.Size = new System.Drawing.Size(71, 20);
		//	udCenterOffsX.TabIndex = 25;
		//	ttHelpTip.SetToolTip(udCenterOffsX, "Offset the center of circles / squares horizontally");
		//	udCenterOffsX.ValueChanged += new System.EventHandler(Controls_ValueChanged);
		//	udCenterOffsY.DecimalPlaces = 3;
		//	udCenterOffsY.Location = new System.Drawing.Point(112, 244);
		//	udCenterOffsY.Maximum = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		0
		//	});
		//	udCenterOffsY.Minimum = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		-2147483648
		//	});
		//	udCenterOffsY.Name = "udCenterOffsY";
		//	udCenterOffsY.Size = new System.Drawing.Size(71, 20);
		//	udCenterOffsY.TabIndex = 26;
		//	ttHelpTip.SetToolTip(udCenterOffsY, "Offset the center of circles / squares vertically");
		//	udCenterOffsY.ValueChanged += new System.EventHandler(Controls_ValueChanged);
		//	label18.AutoSize = true;
		//	label18.Location = new System.Drawing.Point(109, 229);
		//	label18.Name = "label18";
		//	label18.Size = new System.Drawing.Size(77, 13);
		//	label18.TabIndex = 28;
		//	label18.Text = "Center offset Y";
		//	lblRandomDots.AutoSize = true;
		//	lblRandomDots.Location = new System.Drawing.Point(109, 147);
		//	lblRandomDots.Name = "lblRandomDots";
		//	lblRandomDots.Size = new System.Drawing.Size(72, 13);
		//	lblRandomDots.TabIndex = 24;
		//	lblRandomDots.Text = "Random Dots";
		//	udRandomDots.Increment = new decimal(new int[4]
		//	{
		//		100,
		//		0,
		//		0,
		//		0
		//	});
		//	udRandomDots.Location = new System.Drawing.Point(112, 162);
		//	udRandomDots.Maximum = new decimal(new int[4]
		//	{
		//		1000000,
		//		0,
		//		0,
		//		0
		//	});
		//	udRandomDots.Minimum = new decimal(new int[4]
		//	{
		//		100,
		//		0,
		//		0,
		//		0
		//	});
		//	udRandomDots.Name = "udRandomDots";
		//	udRandomDots.Size = new System.Drawing.Size(71, 20);
		//	udRandomDots.TabIndex = 23;
		//	ttHelpTip.SetToolTip(udRandomDots, "Tilt the dot or lines by this many degrees");
		//	udRandomDots.Value = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		0
		//	});
		//	udRandomDots.ValueChanged += new System.EventHandler(Controls_ValueChanged);
		//	cbLockAspect.AutoSize = true;
		//	cbLockAspect.Checked = true;
		//	cbLockAspect.CheckState = System.Windows.Forms.CheckState.Checked;
		//	cbLockAspect.Location = new System.Drawing.Point(42, 6);
		//	cbLockAspect.Name = "cbLockAspect";
		//	cbLockAspect.Size = new System.Drawing.Size(114, 17);
		//	cbLockAspect.TabIndex = 22;
		//	cbLockAspect.Text = "Lock Aspect Ratio";
		//	ttHelpTip.SetToolTip(cbLockAspect, "Choose white on black, or black on white");
		//	cbLockAspect.UseVisualStyleBackColor = true;
		//	cbLockAspect.CheckedChanged += new System.EventHandler(cbLockAspect_CheckedChanged);
		//	cbFixedSize.AutoSize = true;
		//	cbFixedSize.Location = new System.Drawing.Point(112, 288);
		//	cbFixedSize.Name = "cbFixedSize";
		//	cbFixedSize.Size = new System.Drawing.Size(79, 17);
		//	cbFixedSize.TabIndex = 21;
		//	cbFixedSize.Text = "Fixed Sizes";
		//	ttHelpTip.SetToolTip(cbFixedSize, "Choose white on black, or black on white");
		//	cbFixedSize.UseVisualStyleBackColor = true;
		//	cbFixedSize.CheckedChanged += new System.EventHandler(Controls_ValueChanged);
		//	cbGammaCorrect.AutoSize = true;
		//	cbGammaCorrect.Location = new System.Drawing.Point(112, 269);
		//	cbGammaCorrect.Name = "cbGammaCorrect";
		//	cbGammaCorrect.Size = new System.Drawing.Size(79, 17);
		//	cbGammaCorrect.TabIndex = 7;
		//	cbGammaCorrect.Text = "Dark Boost";
		//	ttHelpTip.SetToolTip(cbGammaCorrect, "Brighten dark image areas");
		//	cbGammaCorrect.UseVisualStyleBackColor = true;
		//	cbGammaCorrect.CheckedChanged += new System.EventHandler(Controls_ValueChanged);
		//	label9.AutoSize = true;
		//	label9.Location = new System.Drawing.Point(109, 189);
		//	label9.Name = "label9";
		//	label9.Size = new System.Drawing.Size(53, 13);
		//	label9.TabIndex = 20;
		//	label9.Text = "Amplitude";
		//	udAmplitude.DecimalPlaces = 3;
		//	udAmplitude.Increment = new decimal(new int[4]
		//	{
		//		1,
		//		0,
		//		0,
		//		131072
		//	});
		//	udAmplitude.Location = new System.Drawing.Point(112, 204);
		//	udAmplitude.Maximum = new decimal(new int[4]
		//	{
		//		10,
		//		0,
		//		0,
		//		0
		//	});
		//	udAmplitude.Minimum = new decimal(new int[4]
		//	{
		//		10,
		//		0,
		//		0,
		//		-2147483648
		//	});
		//	udAmplitude.Name = "udAmplitude";
		//	udAmplitude.Size = new System.Drawing.Size(71, 20);
		//	udAmplitude.TabIndex = 9;
		//	ttHelpTip.SetToolTip(udAmplitude, "Maximum distance to move dots away from their line by a sine wave (0 = no wave)");
		//	udAmplitude.ValueChanged += new System.EventHandler(Controls_ValueChanged);
		//	label15.AutoSize = true;
		//	label15.Location = new System.Drawing.Point(8, 189);
		//	label15.Name = "label15";
		//	label15.Size = new System.Drawing.Size(65, 13);
		//	label15.TabIndex = 19;
		//	label15.Text = "Wavelength";
		//	udWavelength.DecimalPlaces = 3;
		//	udWavelength.Increment = new decimal(new int[4]
		//	{
		//		125,
		//		0,
		//		0,
		//		196608
		//	});
		//	udWavelength.Location = new System.Drawing.Point(11, 204);
		//	udWavelength.Maximum = new decimal(new int[4]
		//	{
		//		10,
		//		0,
		//		0,
		//		0
		//	});
		//	udWavelength.Minimum = new decimal(new int[4]
		//	{
		//		1,
		//		0,
		//		0,
		//		131072
		//	});
		//	udWavelength.Name = "udWavelength";
		//	udWavelength.Size = new System.Drawing.Size(71, 20);
		//	udWavelength.TabIndex = 8;
		//	ttHelpTip.SetToolTip(udWavelength, "Length of one full cycle of the wave pattern");
		//	udWavelength.Value = new decimal(new int[4]
		//	{
		//		20,
		//		0,
		//		0,
		//		65536
		//	});
		//	udWavelength.ValueChanged += new System.EventHandler(Controls_ValueChanged);
		//	cbOffsetOdd.AutoSize = true;
		//	cbOffsetOdd.Checked = true;
		//	cbOffsetOdd.CheckState = System.Windows.Forms.CheckState.Checked;
		//	cbOffsetOdd.Location = new System.Drawing.Point(11, 269);
		//	cbOffsetOdd.Name = "cbOffsetOdd";
		//	cbOffsetOdd.Size = new System.Drawing.Size(99, 17);
		//	cbOffsetOdd.TabIndex = 10;
		//	cbOffsetOdd.Text = "Offset odd lines";
		//	ttHelpTip.SetToolTip(cbOffsetOdd, "Check this to \"honeycomb\" the dots instead of using a grid");
		//	cbOffsetOdd.UseVisualStyleBackColor = true;
		//	cbOffsetOdd.CheckedChanged += new System.EventHandler(Controls_ValueChanged);
		//	cbInvert.AutoSize = true;
		//	cbInvert.Location = new System.Drawing.Point(11, 288);
		//	cbInvert.Name = "cbInvert";
		//	cbInvert.Size = new System.Drawing.Size(53, 17);
		//	cbInvert.TabIndex = 11;
		//	cbInvert.Text = "Invert";
		//	ttHelpTip.SetToolTip(cbInvert, "Choose white on black, or black on white");
		//	cbInvert.UseVisualStyleBackColor = true;
		//	cbInvert.CheckedChanged += new System.EventHandler(Controls_ValueChanged);
		//	tabPage2.Controls.Add(cbGrblCompat);
		//	tabPage2.Controls.Add(cbIncludeLineNumbers);
		//	tabPage2.Controls.Add(label16);
		//	tabPage2.Controls.Add(udZOffset);
		//	tabPage2.Controls.Add(label11);
		//	tabPage2.Controls.Add(udEngraveDepth);
		//	tabPage2.Controls.Add(cbTwoPassCuts);
		//	tabPage2.Controls.Add(cbPointRetract);
		//	tabPage2.Controls.Add(udSpindleSpeed);
		//	tabPage2.Controls.Add(label14);
		//	tabPage2.Controls.Add(udOriginX);
		//	tabPage2.Controls.Add(label12);
		//	tabPage2.Controls.Add(udOriginY);
		//	tabPage2.Controls.Add(label13);
		//	tabPage2.Controls.Add(udFeedRate);
		//	tabPage2.Controls.Add(lblFeedRate);
		//	tabPage2.Controls.Add(btnWriteGCode);
		//	tabPage2.Controls.Add(label8);
		//	tabPage2.Controls.Add(udSafeZ);
		//	tabPage2.Controls.Add(udPointRetract);
		//	tabPage2.Controls.Add(udToolAngle);
		//	tabPage2.Controls.Add(label10);
		//	tabPage2.Location = new System.Drawing.Point(4, 22);
		//	tabPage2.Name = "tabPage2";
		//	tabPage2.Padding = new System.Windows.Forms.Padding(3);
		//	tabPage2.Size = new System.Drawing.Size(198, 348);
		//	tabPage2.TabIndex = 1;
		//	tabPage2.Text = "Toolpath";
		//	tabPage2.UseVisualStyleBackColor = true;
		//	cbIncludeLineNumbers.AutoSize = true;
		//	cbIncludeLineNumbers.Checked = true;
		//	cbIncludeLineNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
		//	cbIncludeLineNumbers.Location = new System.Drawing.Point(15, 240);
		//	cbIncludeLineNumbers.Name = "cbIncludeLineNumbers";
		//	cbIncludeLineNumbers.Size = new System.Drawing.Size(123, 17);
		//	cbIncludeLineNumbers.TabIndex = 30;
		//	cbIncludeLineNumbers.Text = "Include line numbers";
		//	ttHelpTip.SetToolTip(cbIncludeLineNumbers, "Creates GCode with unique line numbers (some machines require this, but it makes bigger files)");
		//	cbIncludeLineNumbers.UseVisualStyleBackColor = true;
		//	label16.AutoSize = true;
		//	label16.Location = new System.Drawing.Point(12, 136);
		//	label16.Name = "label16";
		//	label16.Size = new System.Drawing.Size(45, 13);
		//	label16.TabIndex = 28;
		//	label16.Text = "Z Offset";
		//	udZOffset.DecimalPlaces = 4;
		//	udZOffset.Increment = new decimal(new int[4]
		//	{
		//		625,
		//		0,
		//		0,
		//		262144
		//	});
		//	udZOffset.Location = new System.Drawing.Point(15, 152);
		//	udZOffset.Maximum = new decimal(new int[4]
		//	{
		//		5,
		//		0,
		//		0,
		//		0
		//	});
		//	udZOffset.Name = "udZOffset";
		//	udZOffset.Size = new System.Drawing.Size(71, 20);
		//	udZOffset.TabIndex = 6;
		//	ttHelpTip.SetToolTip(udZOffset, "Value to offset all hole depths by.  Use zero if you zero your machine to the top of your work piece, or the height of your material if you zero to the bed of the machine.");
		//	label11.AutoSize = true;
		//	label11.Location = new System.Drawing.Point(109, 94);
		//	label11.Name = "label11";
		//	label11.Size = new System.Drawing.Size(87, 13);
		//	label11.TabIndex = 26;
		//	label11.Text = "Engraving Depth";
		//	udEngraveDepth.DecimalPlaces = 4;
		//	udEngraveDepth.Increment = new decimal(new int[4]
		//	{
		//		5,
		//		0,
		//		0,
		//		196608
		//	});
		//	udEngraveDepth.Location = new System.Drawing.Point(112, 110);
		//	udEngraveDepth.Maximum = new decimal(new int[4]
		//	{
		//		2,
		//		0,
		//		0,
		//		0
		//	});
		//	udEngraveDepth.Name = "udEngraveDepth";
		//	udEngraveDepth.Size = new System.Drawing.Size(71, 20);
		//	udEngraveDepth.TabIndex = 5;
		//	ttHelpTip.SetToolTip(udEngraveDepth, "If using a two-layer engraveable plastic, set this to the thickness of the surface layer");
		//	cbTwoPassCuts.AutoSize = true;
		//	cbTwoPassCuts.Location = new System.Drawing.Point(15, 223);
		//	cbTwoPassCuts.Name = "cbTwoPassCuts";
		//	cbTwoPassCuts.Size = new System.Drawing.Size(95, 17);
		//	cbTwoPassCuts.TabIndex = 9;
		//	cbTwoPassCuts.Text = "Two-pass cuts";
		//	ttHelpTip.SetToolTip(cbTwoPassCuts, "Cut lines in both directions - improves cut edges on some materials");
		//	cbTwoPassCuts.UseVisualStyleBackColor = true;
		//	cbPointRetract.AutoSize = true;
		//	cbPointRetract.Location = new System.Drawing.Point(92, 7);
		//	cbPointRetract.Name = "cbPointRetract";
		//	cbPointRetract.Size = new System.Drawing.Size(88, 17);
		//	cbPointRetract.TabIndex = 1;
		//	cbPointRetract.Text = "Point Retract";
		//	ttHelpTip.SetToolTip(cbPointRetract, "Use a different Safe-Z value when moving between dots");
		//	cbPointRetract.UseVisualStyleBackColor = true;
		//	cbPointRetract.CheckedChanged += new System.EventHandler(cbPointRetract_CheckedChanged);
		//	udSpindleSpeed.Increment = new decimal(new int[4]
		//	{
		//		500,
		//		0,
		//		0,
		//		0
		//	});
		//	udSpindleSpeed.Location = new System.Drawing.Point(15, 110);
		//	udSpindleSpeed.Maximum = new decimal(new int[4]
		//	{
		//		40000,
		//		0,
		//		0,
		//		0
		//	});
		//	udSpindleSpeed.Name = "udSpindleSpeed";
		//	udSpindleSpeed.Size = new System.Drawing.Size(71, 20);
		//	udSpindleSpeed.TabIndex = 4;
		//	udSpindleSpeed.ThousandsSeparator = true;
		//	udSpindleSpeed.Value = new decimal(new int[4]
		//	{
		//		30000,
		//		0,
		//		0,
		//		0
		//	});
		//	label14.AutoSize = true;
		//	label14.Location = new System.Drawing.Point(12, 94);
		//	label14.Name = "label14";
		//	label14.Size = new System.Drawing.Size(69, 13);
		//	label14.TabIndex = 23;
		//	label14.Text = "Spindle RPM";
		//	udOriginX.DecimalPlaces = 3;
		//	udOriginX.Location = new System.Drawing.Point(15, 196);
		//	udOriginX.Maximum = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		0
		//	});
		//	udOriginX.Minimum = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		-2147483648
		//	});
		//	udOriginX.Name = "udOriginX";
		//	udOriginX.Size = new System.Drawing.Size(71, 20);
		//	udOriginX.TabIndex = 7;
		//	ttHelpTip.SetToolTip(udOriginX, "Offset from zero to place the image");
		//	label12.AutoSize = true;
		//	label12.Location = new System.Drawing.Point(12, 181);
		//	label12.Name = "label12";
		//	label12.Size = new System.Drawing.Size(44, 13);
		//	label12.TabIndex = 21;
		//	label12.Text = "Origin X";
		//	udOriginY.DecimalPlaces = 3;
		//	udOriginY.Location = new System.Drawing.Point(112, 196);
		//	udOriginY.Maximum = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		0
		//	});
		//	udOriginY.Minimum = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		-2147483648
		//	});
		//	udOriginY.Name = "udOriginY";
		//	udOriginY.Size = new System.Drawing.Size(71, 20);
		//	udOriginY.TabIndex = 8;
		//	ttHelpTip.SetToolTip(udOriginY, "Offset from zero to place the image");
		//	label13.AutoSize = true;
		//	label13.Location = new System.Drawing.Point(109, 181);
		//	label13.Name = "label13";
		//	label13.Size = new System.Drawing.Size(44, 13);
		//	label13.TabIndex = 20;
		//	label13.Text = "Origin Y";
		//	udFeedRate.DecimalPlaces = 1;
		//	udFeedRate.Increment = new decimal(new int[4]
		//	{
		//		10,
		//		0,
		//		0,
		//		0
		//	});
		//	udFeedRate.Location = new System.Drawing.Point(15, 67);
		//	udFeedRate.Maximum = new decimal(new int[4]
		//	{
		//		1000,
		//		0,
		//		0,
		//		0
		//	});
		//	udFeedRate.Minimum = new decimal(new int[4]
		//	{
		//		1,
		//		0,
		//		0,
		//		0
		//	});
		//	udFeedRate.Name = "udFeedRate";
		//	udFeedRate.Size = new System.Drawing.Size(71, 20);
		//	udFeedRate.TabIndex = 2;
		//	udFeedRate.Value = new decimal(new int[4]
		//	{
		//		50,
		//		0,
		//		0,
		//		0
		//	});
		//	lblFeedRate.AutoSize = true;
		//	lblFeedRate.Location = new System.Drawing.Point(12, 51);
		//	lblFeedRate.Name = "lblFeedRate";
		//	lblFeedRate.Size = new System.Drawing.Size(82, 13);
		//	lblFeedRate.TabIndex = 17;
		//	lblFeedRate.Text = "Feed (Inch/min)";
		//	btnWriteGCode.Location = new System.Drawing.Point(5, 281);
		//	btnWriteGCode.Name = "btnWriteGCode";
		//	btnWriteGCode.Size = new System.Drawing.Size(185, 23);
		//	btnWriteGCode.TabIndex = 10;
		//	btnWriteGCode.Text = "Write GCode";
		//	btnWriteGCode.UseVisualStyleBackColor = true;
		//	btnWriteGCode.Click += new System.EventHandler(btnWriteGCode_Click);
		//	label8.AutoSize = true;
		//	label8.Location = new System.Drawing.Point(12, 8);
		//	label8.Name = "label8";
		//	label8.Size = new System.Drawing.Size(39, 13);
		//	label8.TabIndex = 10;
		//	label8.Text = "Safe Z";
		//	udSafeZ.DecimalPlaces = 4;
		//	udSafeZ.Increment = new decimal(new int[4]
		//	{
		//		625,
		//		0,
		//		0,
		//		262144
		//	});
		//	udSafeZ.Location = new System.Drawing.Point(15, 24);
		//	udSafeZ.Maximum = new decimal(new int[4]
		//	{
		//		10,
		//		0,
		//		0,
		//		0
		//	});
		//	udSafeZ.Name = "udSafeZ";
		//	udSafeZ.Size = new System.Drawing.Size(71, 20);
		//	udSafeZ.TabIndex = 0;
		//	ttHelpTip.SetToolTip(udSafeZ, "Height used for all rapid moves, unless Point Retract is enabled");
		//	udSafeZ.Value = new decimal(new int[4]
		//	{
		//		25,
		//		0,
		//		0,
		//		131072
		//	});
		//	udPointRetract.DecimalPlaces = 4;
		//	udPointRetract.Enabled = false;
		//	udPointRetract.Increment = new decimal(new int[4]
		//	{
		//		625,
		//		0,
		//		0,
		//		262144
		//	});
		//	udPointRetract.Location = new System.Drawing.Point(112, 24);
		//	udPointRetract.Maximum = new decimal(new int[4]
		//	{
		//		10,
		//		0,
		//		0,
		//		0
		//	});
		//	udPointRetract.Name = "udPointRetract";
		//	udPointRetract.Size = new System.Drawing.Size(71, 20);
		//	udPointRetract.TabIndex = 1;
		//	ttHelpTip.SetToolTip(udPointRetract, "Use a different Safe-Z value when moving between neighboring dots");
		//	udPointRetract.Value = new decimal(new int[4]
		//	{
		//		25,
		//		0,
		//		0,
		//		131072
		//	});
		//	udToolAngle.DecimalPlaces = 2;
		//	udToolAngle.Increment = new decimal(new int[4]
		//	{
		//		5,
		//		0,
		//		0,
		//		0
		//	});
		//	udToolAngle.Location = new System.Drawing.Point(112, 67);
		//	udToolAngle.Maximum = new decimal(new int[4]
		//	{
		//		170,
		//		0,
		//		0,
		//		0
		//	});
		//	udToolAngle.Minimum = new decimal(new int[4]
		//	{
		//		10,
		//		0,
		//		0,
		//		0
		//	});
		//	udToolAngle.Name = "udToolAngle";
		//	udToolAngle.Size = new System.Drawing.Size(71, 20);
		//	udToolAngle.TabIndex = 3;
		//	ttHelpTip.SetToolTip(udToolAngle, "Angle of the V-bit to use for carving");
		//	udToolAngle.Value = new decimal(new int[4]
		//	{
		//		90,
		//		0,
		//		0,
		//		0
		//	});
		//	label10.AutoSize = true;
		//	label10.Location = new System.Drawing.Point(109, 51);
		//	label10.Name = "label10";
		//	label10.Size = new System.Drawing.Size(58, 13);
		//	label10.TabIndex = 14;
		//	label10.Text = "Tool Angle";
		//	tabPage3.Controls.Add(lblWriteDXFHelp);
		//	tabPage3.Controls.Add(btnWriteDXF);
		//	tabPage3.Location = new System.Drawing.Point(4, 22);
		//	tabPage3.Name = "tabPage3";
		//	tabPage3.Padding = new System.Windows.Forms.Padding(3);
		//	tabPage3.Size = new System.Drawing.Size(198, 348);
		//	tabPage3.TabIndex = 2;
		//	tabPage3.Text = "DXF";
		//	tabPage3.UseVisualStyleBackColor = true;
		//	lblWriteDXFHelp.Location = new System.Drawing.Point(10, 20);
		//	lblWriteDXFHelp.Name = "lblWriteDXFHelp";
		//	lblWriteDXFHelp.Size = new System.Drawing.Size(182, 127);
		//	lblWriteDXFHelp.TabIndex = 1;
		//	lblWriteDXFHelp.Text = componentResourceManager.GetString("lblWriteDXFHelp.Text");
		//	btnWriteDXF.Location = new System.Drawing.Point(7, 174);
		//	btnWriteDXF.Name = "btnWriteDXF";
		//	btnWriteDXF.Size = new System.Drawing.Size(185, 23);
		//	btnWriteDXF.TabIndex = 0;
		//	btnWriteDXF.Text = "Write to DXF or PNG";
		//	btnWriteDXF.UseVisualStyleBackColor = true;
		//	btnWriteDXF.Click += new System.EventHandler(btnWriteDXF_Click);
		//	tabPage4.Controls.Add(cbNegateImage);
		//	tabPage4.Controls.Add(lblContrast);
		//	tabPage4.Controls.Add(lblBright);
		//	tabPage4.Controls.Add(btnAutoLevels);
		//	tabPage4.Controls.Add(label20);
		//	tabPage4.Controls.Add(label19);
		//	tabPage4.Controls.Add(tbContrast);
		//	tabPage4.Controls.Add(tbBright);
		//	tabPage4.Location = new System.Drawing.Point(4, 22);
		//	tabPage4.Name = "tabPage4";
		//	tabPage4.Size = new System.Drawing.Size(198, 348);
		//	tabPage4.TabIndex = 3;
		//	tabPage4.Text = "Adjust";
		//	tabPage4.UseVisualStyleBackColor = true;
		//	cbNegateImage.AutoSize = true;
		//	cbNegateImage.Location = new System.Drawing.Point(54, 214);
		//	cbNegateImage.Name = "cbNegateImage";
		//	cbNegateImage.Size = new System.Drawing.Size(93, 17);
		//	cbNegateImage.TabIndex = 7;
		//	cbNegateImage.Text = "Negate Image";
		//	cbNegateImage.UseVisualStyleBackColor = true;
		//	cbNegateImage.CheckedChanged += new System.EventHandler(cbNegateImage_CheckedChanged);
		//	lblContrast.Location = new System.Drawing.Point(144, 96);
		//	lblContrast.Name = "lblContrast";
		//	lblContrast.Size = new System.Drawing.Size(51, 13);
		//	lblContrast.TabIndex = 6;
		//	lblContrast.Text = "0";
		//	lblContrast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		//	lblBright.Location = new System.Drawing.Point(144, 17);
		//	lblBright.Name = "lblBright";
		//	lblBright.Size = new System.Drawing.Size(51, 13);
		//	lblBright.TabIndex = 5;
		//	lblBright.Text = "0";
		//	lblBright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		//	btnAutoLevels.Location = new System.Drawing.Point(9, 176);
		//	btnAutoLevels.Name = "btnAutoLevels";
		//	btnAutoLevels.Size = new System.Drawing.Size(177, 23);
		//	btnAutoLevels.TabIndex = 4;
		//	btnAutoLevels.Text = "Auto Adjust";
		//	ttHelpTip.SetToolTip(btnAutoLevels, "Automatically choose levels to maximize dynamic range");
		//	btnAutoLevels.UseVisualStyleBackColor = true;
		//	btnAutoLevels.Click += new System.EventHandler(btnAutoLevels_Click);
		//	label20.AutoSize = true;
		//	label20.Location = new System.Drawing.Point(6, 96);
		//	label20.Name = "label20";
		//	label20.Size = new System.Drawing.Size(46, 13);
		//	label20.TabIndex = 3;
		//	label20.Text = "Contrast";
		//	label19.AutoSize = true;
		//	label19.Location = new System.Drawing.Point(6, 17);
		//	label19.Name = "label19";
		//	label19.Size = new System.Drawing.Size(56, 13);
		//	label19.TabIndex = 2;
		//	label19.Text = "Brightness";
		//	tbContrast.Location = new System.Drawing.Point(3, 115);
		//	tbContrast.Maximum = 50;
		//	tbContrast.Minimum = -50;
		//	tbContrast.Name = "tbContrast";
		//	tbContrast.Size = new System.Drawing.Size(192, 45);
		//	tbContrast.TabIndex = 1;
		//	tbContrast.TickFrequency = 5;
		//	ttHelpTip.SetToolTip(tbContrast, "Adjust the contrast of the image");
		//	tbContrast.ValueChanged += new System.EventHandler(Controls_AdjustmentsChanged);
		//	tbBright.Location = new System.Drawing.Point(3, 36);
		//	tbBright.Maximum = 50;
		//	tbBright.Minimum = -50;
		//	tbBright.Name = "tbBright";
		//	tbBright.Size = new System.Drawing.Size(192, 45);
		//	tbBright.TabIndex = 0;
		//	tbBright.TickFrequency = 5;
		//	ttHelpTip.SetToolTip(tbBright, "Adjust the brightness of the image");
		//	tbBright.ValueChanged += new System.EventHandler(Controls_AdjustmentsChanged);
		//	rbMillimeters.AutoSize = true;
		//	rbMillimeters.Location = new System.Drawing.Point(9, 36);
		//	rbMillimeters.Name = "rbMillimeters";
		//	rbMillimeters.Size = new System.Drawing.Size(73, 17);
		//	rbMillimeters.TabIndex = 9;
		//	rbMillimeters.Text = "Millimeters";
		//	rbMillimeters.UseVisualStyleBackColor = true;
		//	rbInches.AutoSize = true;
		//	rbInches.Checked = true;
		//	rbInches.Location = new System.Drawing.Point(9, 19);
		//	rbInches.Name = "rbInches";
		//	rbInches.Size = new System.Drawing.Size(57, 17);
		//	rbInches.TabIndex = 8;
		//	rbInches.TabStop = true;
		//	rbInches.Text = "Inches";
		//	rbInches.UseVisualStyleBackColor = true;
		//	rbInches.CheckedChanged += new System.EventHandler(rbInches_CheckedChanged);
		//	lblDirections.AutoSize = true;
		//	lblDirections.Location = new System.Drawing.Point(363, 201);
		//	lblDirections.Name = "lblDirections";
		//	lblDirections.Size = new System.Drawing.Size(116, 13);
		//	lblDirections.TabIndex = 19;
		//	lblDirections.Text = "Drag an image file here";
		//	lblDirections.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		//	groupBox1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
		//	groupBox1.Controls.Add(rbCircles);
		//	groupBox1.Controls.Add(rbRandom);
		//	groupBox1.Controls.Add(rbSquares);
		//	groupBox1.Controls.Add(rbLines);
		//	groupBox1.Controls.Add(rbHalftone);
		//	groupBox1.Location = new System.Drawing.Point(7, 379);
		//	groupBox1.Name = "groupBox1";
		//	groupBox1.Size = new System.Drawing.Size(90, 98);
		//	groupBox1.TabIndex = 20;
		//	groupBox1.TabStop = false;
		//	groupBox1.Text = "Style";
		//	rbCircles.AutoSize = true;
		//	rbCircles.Location = new System.Drawing.Point(7, 62);
		//	rbCircles.Name = "rbCircles";
		//	rbCircles.Size = new System.Drawing.Size(56, 17);
		//	rbCircles.TabIndex = 3;
		//	rbCircles.Text = "Circles";
		//	rbCircles.UseVisualStyleBackColor = true;
		//	rbCircles.CheckedChanged += new System.EventHandler(Controls_ValueChanged);
		//	rbRandom.AutoSize = true;
		//	rbRandom.Location = new System.Drawing.Point(7, 78);
		//	rbRandom.Name = "rbRandom";
		//	rbRandom.Size = new System.Drawing.Size(65, 17);
		//	rbRandom.TabIndex = 4;
		//	rbRandom.Text = "Random";
		//	rbRandom.UseVisualStyleBackColor = true;
		//	rbRandom.CheckedChanged += new System.EventHandler(Controls_ValueChanged);
		//	rbSquares.AutoSize = true;
		//	rbSquares.Location = new System.Drawing.Point(7, 46);
		//	rbSquares.Name = "rbSquares";
		//	rbSquares.Size = new System.Drawing.Size(64, 17);
		//	rbSquares.TabIndex = 2;
		//	rbSquares.Text = "Squares";
		//	rbSquares.UseVisualStyleBackColor = true;
		//	rbSquares.CheckedChanged += new System.EventHandler(Controls_ValueChanged);
		//	rbLines.AutoSize = true;
		//	rbLines.Location = new System.Drawing.Point(7, 30);
		//	rbLines.Name = "rbLines";
		//	rbLines.Size = new System.Drawing.Size(50, 17);
		//	rbLines.TabIndex = 1;
		//	rbLines.Text = "Lines";
		//	rbLines.UseVisualStyleBackColor = true;
		//	rbLines.CheckedChanged += new System.EventHandler(Controls_ValueChanged);
		//	rbHalftone.AutoSize = true;
		//	rbHalftone.Checked = true;
		//	rbHalftone.Location = new System.Drawing.Point(7, 14);
		//	rbHalftone.Name = "rbHalftone";
		//	rbHalftone.Size = new System.Drawing.Size(47, 17);
		//	rbHalftone.TabIndex = 0;
		//	rbHalftone.TabStop = true;
		//	rbHalftone.Text = "Dots";
		//	rbHalftone.UseVisualStyleBackColor = true;
		//	rbHalftone.CheckedChanged += new System.EventHandler(Controls_ValueChanged);
		//	groupBox2.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
		//	groupBox2.Controls.Add(rbMillimeters);
		//	groupBox2.Controls.Add(rbInches);
		//	groupBox2.Location = new System.Drawing.Point(103, 383);
		//	groupBox2.Name = "groupBox2";
		//	groupBox2.Size = new System.Drawing.Size(90, 63);
		//	groupBox2.TabIndex = 21;
		//	groupBox2.TabStop = false;
		//	groupBox2.Text = "Units";
		//	btnTest.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
		//	btnTest.Location = new System.Drawing.Point(103, 453);
		//	btnTest.Name = "btnTest";
		//	btnTest.Size = new System.Drawing.Size(102, 24);
		//	btnTest.TabIndex = 22;
		//	btnTest.Text = "Test DXF";
		//	btnTest.UseVisualStyleBackColor = true;
		//	btnTest.Click += new System.EventHandler(btnTest_Click);
		//	cbGrblCompat.AutoSize = true;
		//	cbGrblCompat.Location = new System.Drawing.Point(15, 258);
		//	cbGrblCompat.Name = "cbGrblCompat";
		//	cbGrblCompat.Size = new System.Drawing.Size(145, 17);
		//	cbGrblCompat.TabIndex = 31;
		//	cbGrblCompat.Text = "GRBL compatible GCode";
		//	ttHelpTip.SetToolTip(cbGrblCompat, "Creates GCode that is directly compatible with GRBL / Easel");
		//	cbGrblCompat.UseVisualStyleBackColor = true;
		//	AllowDrop = true;
		//	base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		//	base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		//	base.ClientSize = new System.Drawing.Size(632, 482);
		//	base.Controls.Add(btnTest);
		//	base.Controls.Add(groupBox2);
		//	base.Controls.Add(groupBox1);
		//	base.Controls.Add(lblDirections);
		//	base.Controls.Add(tabControl1);
		//	base.Controls.Add(lblStatus);
		//	base.Controls.Add(rbOriginal);
		//	base.Controls.Add(rbPreview);
		//	base.Controls.Add(pbPreview);
		//	MinimumSize = new System.Drawing.Size(648, 518);
		//	base.Name = "MainForm";
		//	Text = "Halftoner V1.7 - by Jason Dorie";
		//	base.DragDrop += new System.Windows.Forms.DragEventHandler(MainForm_DragDrop);
		//	base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainForm_FormClosing);
		//	base.DragOver += new System.Windows.Forms.DragEventHandler(MainForm_DragOver);
		//	((System.ComponentModel.ISupportInitialize)pbPreview).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udWidth).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udHeight).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udSpacing).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udBorder).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udMaxSize).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udMinSize).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udAngle).EndInit();
		//	tabControl1.ResumeLayout(false);
		//	tabPage1.ResumeLayout(false);
		//	tabPage1.PerformLayout();
		//	((System.ComponentModel.ISupportInitialize)udCenterOffsX).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udCenterOffsY).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udRandomDots).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udAmplitude).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udWavelength).EndInit();
		//	tabPage2.ResumeLayout(false);
		//	tabPage2.PerformLayout();
		//	((System.ComponentModel.ISupportInitialize)udZOffset).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udEngraveDepth).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udSpindleSpeed).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udOriginX).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udOriginY).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udFeedRate).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udSafeZ).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udPointRetract).EndInit();
		//	((System.ComponentModel.ISupportInitialize)udToolAngle).EndInit();
		//	tabPage3.ResumeLayout(false);
		//	tabPage4.ResumeLayout(false);
		//	tabPage4.PerformLayout();
		//	((System.ComponentModel.ISupportInitialize)tbContrast).EndInit();
		//	((System.ComponentModel.ISupportInitialize)tbBright).EndInit();
		//	groupBox1.ResumeLayout(false);
		//	groupBox1.PerformLayout();
		//	groupBox2.ResumeLayout(false);
		//	groupBox2.PerformLayout();
		//	ResumeLayout(false);
		//	PerformLayout();
		//}
	}
}
