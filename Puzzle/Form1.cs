using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Puzzle
{
	public partial class Form1 : Form
	{
		private PictureBox _pb = new PictureBox();

		public Form1()
		{
			InitializeComponent();

			Width = 1000;
			Height = 700;
			var puzzle = new Puzzle(2, 4);
			puzzle.GeneratePieceSides();
			_pb.MouseMove += _pb_MouseMove;
			_pb.MouseDown += _pb_MouseDown;
		}

		private void OpenFileClick(object sender, EventArgs e)
		{
			var openFileDialog = new OpenFileDialog()
			{
				Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png",
			};

			var dialogResult = openFileDialog.ShowDialog();

			if (dialogResult == DialogResult.OK)
			{
				GetImages(openFileDialog.FileName);
			}
		}

		private void GetImages(string fileName)
		{
			var locX = 100;
			var locY = 100;

			var endX = 130;
			var graphicsPath = new GraphicsPath();
			graphicsPath.AddLine(0 + locX, 0 + locY, 40 + locX, 0 + locY);
			graphicsPath.AddBezier(40 + locX, 0 + locY, 44 + locX, 2 + locY, 44 + locX, 4 + locY, 44 + locX, 6 + locY);
			graphicsPath.AddBezier(44 + locX, 6 + locY, 15 + locX, 45 + locY, 115 + locX, 45 + locY, 86 + locX, 6 + locY);
			graphicsPath.AddBezier(86 + locX, 6 + locY, 86 + locX, 4 + locY, 86 + locX, 2 + locY, 90 + locX, 0 + locY);
			graphicsPath.AddLine(90 + locX, 0 + locY, endX + locX, 0 + locY);
			graphicsPath.AddPath(getGraphicsPath(graphicsPath, new PointF(65 + locX, 65 + locY), Math.PI), true);
			//graphicsPath.AddLine(endX, 0, endX, 40);
			//graphicsPath.AddBezier(endX, 40, endX + 2, 44, endX + 4, 44, endX + 6, 44);
			//graphicsPath.AddBezier(endX + 6, 44, endX + 45, 15, endX + 45, 115, endX + 6, 86);
			//graphicsPath.AddBezier(endX + 6, 86, endX + 4, 86, endX + 2, 86, endX, 90);
			//graphicsPath.AddLine(endX, 90, endX, endX);
			graphicsPath.AddLine(endX + locX, endX + locY, 0 + locX, endX + locY);
			graphicsPath.AddLine(0 + locX, endX + locY, 0 + locX, 0 + locY);
			graphicsPath.AddLine(0 + locX, 0 + locY, 50 + locX, 0 + locY);
			_pb.Region = new Region(graphicsPath);
			var bmp = new Bitmap(fileName);
			_pb.Image = (Image)new Bitmap(bmp, 600, 400); /*Image.FromFile(fileName);*/
			_pb.Location = new Point(10, 30);
			_pb.Size = new Size(600, 500);
			_pb.BorderStyle = BorderStyle.None;
			this.Controls.Add(_pb);
		}

		private GraphicsPath getGraphicsPath(GraphicsPath path, PointF center, double angle)
		{
			var cos = Math.Cos(angle);
			var sin = Math.Sin(angle);

			var newPoints = new List<PointF>();

			foreach (var point in path.PathData.Points)
			{
				newPoints.Add(new PointF((float)(center.X + ((point.X - center.X) * cos - (point.Y - center.Y) * sin)),
					(float)(center.Y + (point.Y - center.Y) * cos + (point.X - center.X) * sin)));
			}

			return new GraphicsPath(newPoints.ToArray(), path.PathData.Types);
		}

		void _pb_MouseMove(object sender, MouseEventArgs e)
		{
			PictureBox pbox = (PictureBox)sender;
			if (e.Button == MouseButtons.Left)
			{
				pbox.Left = e.X + pbox.Left - MouseDownLocation.X;
				pbox.Top = e.Y + pbox.Top - MouseDownLocation.Y;
			}
		}

		private Point MouseDownLocation;

		void _pb_MouseDown(object sender, MouseEventArgs e)
		{
			PictureBox pbox = (PictureBox)sender;
			pbox.BringToFront();
			if (e.Button == MouseButtons.Left)
				MouseDownLocation = e.Location;
		}
	}

	public class PuzzledPictureBox : PictureBox
	{
		public PuzzledPictureBox(Piece piece)
			: base()
		{
			var graphicsPath = new GraphicsPath();

			var firstRate = piece.Location.X + 0.3F * piece.Width;
			var secondRate = piece.Location.X + 0.7F * piece.Width;
			var thirdRate = piece.Location.X + 0.33F * piece.Width;
			var fourthRate = piece.Location.X + 0.66F * piece.Width;

			graphicsPath.AddLine(piece.Location.X, piece.Location.Y, firstRate, piece.Location.Y);

			switch (piece.Up)
			{
				case PieceSideType.Border:
					graphicsPath.AddLine(firstRate, piece.Location.Y, secondRate, piece.Location.Y);
					break;
				case PieceSideType.Hole:
					graphicsPath.AddBezier(firstRate, piece.Location.Y, thirdRate, piece.Location.Y + 0.015F * piece.Height,
						thirdRate, piece.Location.Y + 0.03F * piece.Height, thirdRate, piece.Location.Y + 0.045F * piece.Height);
					graphicsPath.AddBezier(thirdRate, piece.Location.Y + 0.045F * piece.Height,
						piece.Location.X + 0.115F * piece.Width, piece.Location.Y + 0.33F * piece.Height,
						piece.Location.X + 0.885F * piece.Width, piece.Location.Y + 0.33F * piece.Height,
						fourthRate, piece.Location.Y + 0.045F * piece.Height);
					graphicsPath.AddBezier(fourthRate, piece.Location.Y + 0.045F * piece.Height,
						fourthRate, piece.Location.Y + 0.03F * piece.Height,
						fourthRate, piece.Location.Y + 0.015F * piece.Height,
						secondRate, piece.Location.Y);
					break;
				case PieceSideType.Hump:
					break;
			}

			//var endX = 130;
			////graphicsPath.AddLine(0, 0, 40, 0);
			//graphicsPath.AddBezier(40, 0, 44, 2, 44, 4, 44, 6);
			//graphicsPath.AddBezier(44, 6, 15, 45, 115, 45, 86, 6);
			//graphicsPath.AddBezier(86, 6, 86, 4, 86, 2, 90, 0);
			//graphicsPath.AddLine(90, 0, endX, 0);
			//graphicsPath.AddLine(endX, 0, endX, 40);
			//graphicsPath.AddBezier(endX, 40, endX + 2, 44, endX + 4, 44, endX + 6, 44);
			//graphicsPath.AddBezier(endX + 6, 44, endX + 45, 15, endX + 45, 115, endX + 6, 86);
			//graphicsPath.AddBezier(endX + 6, 86, endX + 4, 86, endX + 2, 86, endX, 90);
			//graphicsPath.AddLine(endX, 90, endX, endX);
			//graphicsPath.AddLine(endX, endX, 0, endX);
			//graphicsPath.AddLine(0, endX, 0, 0);
			//graphicsPath.AddLine(0, 0, 50, 0);
		}
	}
}
