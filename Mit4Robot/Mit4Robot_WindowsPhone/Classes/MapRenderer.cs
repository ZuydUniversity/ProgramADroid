using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.Media.Render;
using Windows.Media;
using Windows.Media.Editing;
using Windows.Media.Core;
using Windows.Foundation;
using Windows.UI;

namespace Mit4Robot_WindowsPhone.Classes
{
    class MapRenderer
    {
        private BitmapImage mapBmp = null;

        public Windows.UI.Xaml.UIElement Render(/*Map map,*/ int robotX, int robotY)
        {
            /*int x = map.map.GetLength(0);
            int y = map.map.GetLength(1);

            const int imgHeight = 64;
            const int imgWidth = 64;

            var tempBmp = new WriteableBitmap(x * imgWidth, y * imgHeight);

            for (int iX = 0; iX < map.map.GetLength(1); iX++)
            {
                for (int iY = 0; iY < map.map.GetLength(0); iY++)
                {
                    
                    BitmapImage bmp = new BitmapImage(new Uri("..\\Assets\\tiles\\roadTile1.png", UriKind.Relative));
                    tempBmp.Blit(new Point(iX * imgWidth, iY * imgHeight), bmp, new Rect(0, 0, imgWidth, imgHeight), Color.White, WriteableBitmapExtensions.BlendMode.Additive);
                }
            }

            Image img = new Image();
            img.Source = tempBmp;

            return img;*/
            return null;
        }
    }
}
