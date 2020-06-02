using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Drawing.Imaging;
using System.Linq;

public class Aplikacja
{
    public struct Rgb
    {
        public byte b, g, r;
        public Rgb Add(int parametr)
        {
            Rgb rob;

            int temp = this.r + parametr;
            if (temp > 255) temp = 255;
            else if (temp < 0) temp = 0;
            rob.r = (byte)(temp);

            temp = this.g + parametr;
            if (temp > 255) temp = 255;
            else if (temp < 0) temp = 0;
            rob.g = (byte)(temp);

            temp = this.b + parametr;
            if (temp > 255) temp = 255;
            else if (temp < 0) temp = 0;
            rob.b = (byte)(temp);

            return rob;
        }
    }

    public static Bitmap Binarization(Bitmap btmp)
    {
        int width = btmp.Width;
        int height = btmp.Height;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (btmp.GetPixel(i, j).R >= 0 && btmp.GetPixel(i, j).R <= 100)
                {
                    btmp.SetPixel(i, j, Color.White);
                }
                else
                {
                    btmp.SetPixel(i, j, Color.Black);
                }
            }
        }

        return btmp;
    }

    public static int Weight(Bitmap btmp, int i, int j)
    {
        int weight = 0;
        if (btmp.GetPixel(i - 1, j).R > 0) weight += 1;
        if (btmp.GetPixel(i - 1, j + 1).R > 0) weight += 2;
        if (btmp.GetPixel(i, j + 1).R > 0) weight += 4;
        if (btmp.GetPixel(i + 1, j + 1).R > 0) weight += 8;
        if (btmp.GetPixel(i + 1, j).R > 0) weight += 16;
        if (btmp.GetPixel(i + 1, j - 1).R > 0) weight += 32;
        if (btmp.GetPixel(i, j - 1).R > 0) weight += 64;
        if (btmp.GetPixel(i - 1, j - 1).R > 0) weight += 128;

        return weight;
    }

    public static Bitmap Thinning(Bitmap btmp)
    {
        bool changed = false;

        int[] A0 = new int[] { 3, 6, 7, 12, 14, 15, 24, 28, 30,
            31, 48, 56, 60, 62, 63, 96, 112, 120,
            124, 126, 127, 129, 131, 135, 143, 159,
            191, 192, 193, 195, 199, 207, 223, 224,
            225, 227, 231, 239, 240, 241, 243, 247,
            248, 249, 251, 252, 253, 254 };

        int[] A1 = new int[] { 7, 14, 28, 56, 112, 131, 193, 224 };

        int[] A2 = new int[] { 7, 14, 15, 28, 30, 56, 60, 112, 120, 131, 135, 193, 195, 224, 225, 240 };

        int[] A3 = new int[] { 7, 14, 15, 28, 30, 31, 56, 60, 62, 112, 120, 124, 131, 135, 143, 193, 195, 199, 224, 225, 227, 240, 241, 248 };

        int[] A4 = new int[] {7, 14, 15, 28, 30, 31, 56, 60, 62,
            63, 112, 120, 124, 126, 131, 135, 143,
            159, 193, 195, 199, 207, 224, 225, 227,
            231, 240, 241, 243, 248, 249, 252};

        int[] A5 = new int[] {7, 14, 15, 28, 30, 31, 56, 60,
            62, 63, 112, 120, 124, 126, 131, 135,
            143, 159, 191, 193, 195, 199, 207, 224,
            225, 227, 231, 239, 240, 241, 243, 248, 249, 251, 252, 254};

        int[] Apxl = new int[] {3, 6, 7, 12, 14, 15, 24, 28, 30,
            31, 48, 56, 60, 62, 63, 96, 112, 120,
            124, 126, 127, 129, 131, 135, 143, 159,
            191, 192, 193, 195, 199, 207, 223, 224,
            225, 227, 231, 239, 240, 241, 243, 247,
            248, 249, 251, 252, 253, 254};

        int wysokosc = btmp.Height;
        int szerokosc = btmp.Width;

        Bitmap OutBtmp = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

        BitmapData DataBtmp = btmp.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        BitmapData DataOutBtmp = OutBtmp.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

        int strideWe = DataBtmp.Stride;
        int strideWy = DataOutBtmp.Stride;

        IntPtr scanWe = DataBtmp.Scan0;
        IntPtr scanWy = DataOutBtmp.Scan0;

        int r = 1;

        Rgb black = new Rgb() { r = 0, g = 0, b = 0 };
        Rgb white = new Rgb() { r = 255, g = 255, b = 255 };
        Rgb red = new Rgb() { r = 255, g = 0, b = 0 };

        unsafe
        {
            while(changed == true)
            {
                for (int y = r; y < wysokosc - r; y++)              //Faza 0 - oznaczanie granic
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                    for (int x = r; x < szerokosc - r; x++)
                    {
                        Rgb pikselAnalizowany = ((Rgb*)pWe)[x];

                        if (pikselAnalizowany.r == 0 && pikselAnalizowany.g == 0 && pikselAnalizowany.b == 0)       //Sprawdzic czy to musi byc spelnione
                        {
                            for (int y0 = y - r; y0 <= y + r; y0++)
                            {
                                byte* pOtoczenie = (byte*)(void*)scanWe + y0 * strideWe;
                                for (int x0 = x - r; x0 <= x + r; x0++)
                                {
                                    Rgb pikselOtoczenia = ((Rgb*)pOtoczenie)[x0];
                                    if (A0.Contains(Weight(btmp, x0, y0)))
                                    {
                                        pikselAnalizowany = red;
                                    }

                                }
                            }
                        }
                    }
                }
                /*
                for (int y = r; y < wysokosc - r; y++)                  //Faza 1-5 sprawdzanie granicznych pikseli
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;    //SPRAWDZIĆ
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;    //SPRAWDZIĆ

                    for (int x = r; x < szerokosc - r; x++)
                    {
                        Rgb pikselAnalizowany = ((Rgb*)pWe)[x];                                             //SPRAAWDZIĆ

                        if (pikselAnalizowany.r == 0 && pikselAnalizowany.g == 0 && pikselAnalizowany.b == 0)   //SPRAWDZIĆ
                        {
                            for (int y0 = y - r; y0 <= y + r; y0++)
                            {
                                byte* pOtoczenie = (byte*)(void*)scanWe + y0 * strideWe;        //SPRAAWDZIĆ
                                for (int x0 = x - r; x0 <= x + r; x0++)
                                {
                                    if (btmp.GetPixel(x0, y0) == Color.Red)
                                    {
                                        switch (Weight(btmp, x0, y0))
                                        {
                                            case 1:
                                                if (A1.Contains(Weight(btmp, x0, y0)))
                                                {
                                                    btmp.SetPixel(x0, y0, Color.White);
                                                    changed = true;
                                                }
                                                break;
                                            case 2:
                                                if (A2.Contains(Weight(btmp, x0, y0)))
                                                {
                                                    btmp.SetPixel(x0, y0, Color.White);
                                                    changed = true;
                                                }
                                                break;
                                            case 3:
                                                if (A3.Contains(Weight(btmp, x0, y0)))
                                                {
                                                    btmp.SetPixel(x0, y0, Color.White);
                                                    changed = true;
                                                }
                                                break;
                                            case 4:
                                                if (A4.Contains(Weight(btmp, x0, y0)))
                                                {
                                                    btmp.SetPixel(x0, y0, Color.White);
                                                    changed = true;
                                                }
                                                break;
                                            case 5:
                                                if (A5.Contains(Weight(btmp, x0, y0)))
                                                {
                                                    btmp.SetPixel(x0, y0, Color.White);
                                                    changed = true;
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                */ /*
                for (int y = r; y < wysokosc - r; y++)              //Faza 6 - same punkty
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;    //SPRAAWDZIĆ
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;    //SPRAAWDZIĆ

                    for (int x = r; x < szerokosc - r; x++)
                    {
                        Rgb pikselAnalizowany = ((Rgb*)pWe)[x];         //SPRAAWDZIĆ

                        if (pikselAnalizowany.r == 0 && pikselAnalizowany.g == 0 && pikselAnalizowany.b == 0)       //Sprawdzic czy to musi byc spelnione
                        {
                            for (int y0 = y - r; y0 <= y + r; y0++)
                            {
                                byte* pOtoczenie = (byte*)(void*)scanWe + y0 * strideWe;        //SPRAAWDZIĆ
                                for (int x0 = x - r; x0 <= x + r; x0++)
                                {
                                    if (Apxl.Contains(Weight(btmp, x0, y0)))
                                    {
                                        btmp.SetPixel(x0, y0, Color.White);
                                    }
                                }
                            }
                        }
                    }
                }*/
            }
        }

        return btmp;
    }
}
