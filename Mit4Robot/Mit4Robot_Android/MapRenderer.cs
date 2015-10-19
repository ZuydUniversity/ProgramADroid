using System;
using Shared;
using Android.Graphics;
using Android.Content;
using Android.Views;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Shared.BusinessLayer;
using Shared.Enums;

namespace Mit4Robot_Android
{
	
	/// <summary>
	/// Map renderer.
	/// </summary>
	public class MapRenderer
	{
		public MapRenderer (){}

		private Bitmap mapBmp = null;

		/// Author:	Guy Spronck
		/// Date:	09-06-2015
		/// <summary>
		/// Render the specified map
		/// </summary>
		/// <param name="map">Map.</param>
		/// <param name="res">Resources.</param>
		/// <param name="robotX">Robot x.</param>
		/// <param name="robotY">Robot y.</param>
		public Bitmap Render(Map map, Android.Content.Res.Resources res,int robotX,int robotY)
		{
			int y = map.map.GetLength (0);
			int x = map.map.GetLength (1);
			const int imgHeight = 64;
			const int imgWidth = 64;

			Bitmap tempBitmap;

			if (mapBmp == null) {
				tempBitmap = Bitmap.CreateBitmap ( x * imgWidth,  y * imgHeight, Bitmap.Config.Rgb565);
			} else {
				tempBitmap = mapBmp.Copy(mapBmp.GetConfig(),true);
			}

			Canvas tempCanvas = new Canvas(tempBitmap);
			BitmapFactory.Options opt = new BitmapFactory.Options ();
			opt.InDensity = tempCanvas.Density;

			if (mapBmp == null) {
				for (int iX = 0; iX < map.map.GetLength (1); iX++) {
					for (int iY = 0; iY < map.map.GetLength (0); iY++) {
						DrawTile (map, iX, iY, tempCanvas, imgWidth, imgHeight, res, opt);
					}
				}
				mapBmp = tempBitmap.Copy (tempBitmap.GetConfig (), true);
			} 

			DrawSpawn (res, opt, Robot.Instance.level.spawn, tempCanvas, imgWidth, imgHeight);
			DrawRobot (tempCanvas, res, opt, robotX, robotY, imgHeight, imgWidth);
			try{
				return tempBitmap;
			}finally{
				tempCanvas = null;
				System.GC.Collect ();
			}
		}

		/// Author:	Guy Spronck
		/// Date:	09-06-2015
		/// <summary>
		/// Rotates the bitmap.
		/// </summary>
		/// <returns>The bitmap.</returns>
		/// <param name="source">Source.</param>
		/// <param name="angle">Angle.</param>
		public Bitmap RotateBitmap(Bitmap source, float angle)
		{
			Matrix matrix = new Matrix();
			matrix.PostRotate (angle);
			return Bitmap.CreateBitmap(source, 0, 0, source.Width, source.Height, matrix, true);
		}

		/// Author:	Guy Spronck
		/// Date:	23-06-2015
		/// <summary>
		/// Draws the spawn.
		/// </summary>
		/// <param name="res">Res.</param>
		/// <param name="opt">Opt.</param>
		/// <param name="spawn">Spawn.</param>
		/// <param name="tempCanvas">Temp canvas.</param>
		/// <param name="imgWidth">Image width.</param>
		/// <param name="imgHeight">Image height.</param>
		private void DrawSpawn(Android.Content.Res.Resources res, BitmapFactory.Options opt, Spawn spawn, Canvas tempCanvas, int imgWidth, int imgHeight)
		{
			tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.SpawnFlag01, opt), (float)spawn.x * imgWidth, (float)spawn.y * imgHeight, null);
		}

		/// Author:	Guy Spronck
		/// Date:	09-06-2015
		/// <summary>
		/// Draws the robot.
		/// </summary>
		/// <param name="tempCanvas">Temp canvas.</param>
		/// <param name="res">Res.</param>
		/// <param name="opt">Opt.</param>
		/// <param name="robotX">Robot x.</param>
		/// <param name="robotY">Robot y.</param>
		/// <param name="imgHeight">Image height.</param>
		/// <param name="imgWidth">Image width.</param>
		private void DrawRobot(Canvas tempCanvas, Android.Content.Res.Resources res, BitmapFactory.Options opt, int robotX, int robotY, int imgHeight, int imgWidth)
		{
			float rotation = 0f;
			switch (GlobalSupport.CurrentRobotOrientation) 
			{
			case EOrientation.North:
				rotation = 270f;
				break;
			case EOrientation.East:
				rotation = 0f;
				break;
			case EOrientation.South:
				rotation = 90f;
				break;
			case EOrientation.West:
				rotation = 180f;
				break;
			}

			tempCanvas.DrawBitmap (RotateBitmap(BitmapFactory.DecodeResource(res, Resource.Drawable.blue, opt), rotation), (float)robotX * imgHeight, (float)robotY * imgHeight, null);
		}

		/// Author:	Guy Spronck
		/// Date:	09-06-2015
		/// <summary>
		/// Draws the tile
		/// </summary>
		/// <param name="map">Map.</param>
		/// <param name="iX">x.</param>
		/// <param name="iY">y.</param>
		/// <param name="tempCanvas">Temp canvas.</param>
		/// <param name="imgWidth">Image width.</param>
		/// <param name="imgHeight">Image height.</param>
		/// <param name="res">Res.</param>
		/// <param name="opt">Opt.</param>
		private void DrawTile(Map map, int iX, int iY, Canvas tempCanvas, int imgWidth, int imgHeight, Android.Content.Res.Resources res, BitmapFactory.Options opt)
		{
			switch (map.map [iY, iX].TileId) {
			case 0:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile1, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 1:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile2, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 2:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile3, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 3:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile4, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 4:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile5, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 5:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile6, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 6:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile7, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 7:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile8, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 8:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile9, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 9:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile10, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 10:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile11, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 11:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile12, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 12:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile13, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 13:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile14, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 14:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile15, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 15:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile16, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 16:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile17, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 17:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile18, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 18:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile19, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 19:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile20, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 20:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile21, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 21:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile22, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 22:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile23, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 23:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile24, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 24:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile25, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 25:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile26, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 26:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.roadTile27, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 27:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.terrainTile1, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 28:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.terrainTile2, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 29:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.terrainTile3, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 30:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.terrainTile4, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 31:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.terrainTile5, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 32:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.terrainTile6, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 33:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile1, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 34:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile2, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 35:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile3, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 36:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile4, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 37:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile5, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 38:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile6, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 39:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile7, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 40:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile8, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 41:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile9, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 42:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile10, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 43:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile11, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 44:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile12, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 45:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile13, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 46:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile14, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 47:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile15, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 48:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile16, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 49:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile17, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 50:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile18, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 51:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile19, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 52:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile20, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 53:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile21, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 54:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile22, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 55:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile23, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 56:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile24, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 57:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile25, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 58:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile26, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 59:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile27, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 60:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile28, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 61:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile29, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 62:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile30, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 63:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile31, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 64:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile32, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 65:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile33, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 66:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile34, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 67:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile35, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 68:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile36, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 69:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile37, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 70:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile38, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 71:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile39, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 72:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile40, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 73:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile41, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 74:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile42, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 75:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile43, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 76:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile44, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 77:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile45, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 78:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.waterTile46, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 79:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.StoreApple, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			case 80:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.StoreSausage, opt), (float)iX * imgWidth, (float)iY * imgHeight, null);
				break;
			default:
				tempCanvas.DrawBitmap (BitmapFactory.DecodeResource (res, Resource.Drawable.terrainTile3, opt), (float)iX * 64, (float)iY * imgHeight, null);
				break;
			}
		}
	}
}

