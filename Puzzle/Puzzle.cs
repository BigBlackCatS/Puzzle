using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle
{
	/// <summary>
	/// Пазл
	/// </summary>
	public class Puzzle
	{
		private Piece[,] pieces;
		private int width;
		private int height;

		public Puzzle(int width, int height)
		{
			pieces = new Piece[width, height];
			this.width = width;
			this.height = height;
		}

		public Piece this[int i, int j]
		{
			get
			{
				if (pieces[i, j] == null)
				{
					pieces[i, j] = new Piece();
				}

				return pieces[i, j];
			}
			set
			{
				pieces[i, j] = value;
			}
		}

		public int Width
		{
			get
			{
				return width;
			}
			set 
			{
				width = value <= 0 ? 0 : value;
			}
		}

		public int Height
		{
			get
			{
				return height;
			}
			set
			{
				height = value <= 0 ? 0 : value;
			}
		}

		public void GeneratePieceSides()
		{
			Random random = new Random();

			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					this[i, j].Up = getPieceSideType(i == 0, previousPieceSideType: i == 0 ? 0 : this[i - 1, j].Down);
					this[i, j].Down = getPieceSideType(i == width - 1, random);
					this[i, j].Left = getPieceSideType(j == 0, previousPieceSideType: j == 0 ? 0 : this[i, j - 1].Right);
					this[i, j].Right = getPieceSideType(j == height - 1, random);
				}
			}
		}

		private PieceSideType getPieceSideType(bool isBorder, Random random = null, PieceSideType previousPieceSideType = 0)
		{
			if (isBorder)
			{
				return PieceSideType.Border;
			}

			if (random != null)
			{
				return (PieceSideType)random.Next(1, 3);
			}

			return previousPieceSideType == PieceSideType.Hole ? PieceSideType.Hump : PieceSideType.Hole;
		}
	}

	/// <summary>
	/// Кусочек пазла
	/// </summary>
	public class Piece
	{
		public PieceSideType Up { get; set; }

		public PieceSideType Down { get; set; }

		public PieceSideType Left { get; set; }

		public PieceSideType Right { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public Point Location { get; set; }
	}

	/// <summary>
	/// Тип стороны кусочка пазла
	/// </summary>
	public enum PieceSideType
	{
		/// <summary>
		/// Крайняя строна
		/// </summary>
		Border,

		/// <summary>
		/// Впадина
		/// </summary>
		Hole,

		/// <summary>
		/// Выступ
		/// </summary>
		Hump
	}
}
