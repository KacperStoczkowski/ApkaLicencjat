using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Drawing.Imaging;
using System.Linq;



public class Aplikacja
{
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

    bool changed = true;

    Bitmap dupa;
    Bitmap dupa2;
    Bitmap dupa3;

    /*public struct Rgb
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
    }*/

    public Bitmap Binarization(Bitmap btmp)
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

    public int Weight(Bitmap btmp, int i, int j)
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

    public Bitmap Faz0(Bitmap btmp)
    {
        int height = btmp.Height;
        int width = btmp.Width;

        for (int x = 1; x < height - 1; x++)                  //Faza 0
        {
            for (int y = 1; y < width - 1; y++)
            {
                Color c = btmp.GetPixel(x, y);
                if (c.R > 0)
                {
                    if (this.A0.Contains(this.Weight(btmp, x, y)))
                    {
                        btmp.SetPixel(x, y, Color.FromArgb(c.A, 2, 2, 2));
                    }
                }
            }
        }

        return btmp;
    }

    public Bitmap Faz1(Bitmap btmp, int ph)
    {
        int height = btmp.Height;
        int width = btmp.Width;

        /*for (int x = 1; x < width - 1; x++)                  //Faza 1-5
        {
            for (int y = 1; y < height - 1; y++)
            {
                Color c = btmp.GetPixel(x, y);
                if (c.R == 2)
                {
                    int waga = this.Weight(btmp, x, y);
                    if(this.A1.Contains(waga))
                    {
                        btmp.SetPixel(x, y, Color.White);
                        this.changed = true;
                    }

                    if (this.A2.Contains(waga))
                    {
                        btmp.SetPixel(x, y, Color.White);
                        this.changed = true;
                    }

                    if (this.A3.Contains(waga))
                    {
                        btmp.SetPixel(x, y, Color.White);
                        this.changed = true;
                    }

                    if (this.A4.Contains(waga))
                    {
                        btmp.SetPixel(x, y, Color.White);
                        this.changed = true;
                    }

                    if (this.A5.Contains(waga))
                    {
                        btmp.SetPixel(x, y, Color.White);
                        this.changed = true;
                    }
                }
            }
        }*/

        for (int x = 1; x < height - 1; x++)
        {
            for (int y = 1; y < width - 1; y++)
            {
                Color c = btmp.GetPixel(x, y);

                if(c.R == 2)
                {
                    switch(ph)
                    {
                        case 1:
                                    if (this.A1.Contains(Weight(btmp, x, y)))
                        {
                            btmp.SetPixel(x, y, Color.FromArgb(c.A, 0, 0, 0));
                            this.changed = true;
                        }
                        break;
                                case 2:
                                    if (this.A2.Contains(Weight(btmp, x, y)))
                        {
                            btmp.SetPixel(x, y, Color.FromArgb(c.A, 0, 0, 0));
                            this.changed = true;
                        }
                        break;
                                case 3:
                                    if (this.A3.Contains(Weight(btmp, x, y)))
                        {
                            btmp.SetPixel(x, y, Color.FromArgb(c.A, 0, 0, 0));
                            this.changed = true;
                        }
                        break;
                                case 4:
                                    if (this.A4.Contains(Weight(btmp, x, y)))
                        {
                            btmp.SetPixel(x, y, Color.FromArgb(c.A, 0, 0, 0));
                            this.changed = true;
                        }
                        break;
                                case 5:
                                    if (this.A5.Contains(Weight(btmp, x, y)))
                        {
                            btmp.SetPixel(x, y, Color.FromArgb(c.A, 0, 0, 0));
                            this.changed = true;
                        }
                        break;
                    }
                }
            }
        }
        return btmp;
    }

    public Bitmap Thinning(Bitmap btmp)
    {
        //this.changed = false;
        bool changed = true;

        this.dupa = this.Faz0(btmp);
        //this.dupa2 = this.Faz1(this.dupa);

        /*while (changed)
        {
            changed = false;
            btmp = Faz0(btmp);

            for (int i = 1; i < 6; i++)
            {

                btmp = Faz1(btmp, i);
            }
        }*/
            return this.dupa;
        }
}
