public class PotentialField 
{
    public int Width;
    public int Height;

    public float[,] Values;

    public PotentialField(int width = 0, int height = 0)
    {
        Width = width;
        Height = height;

        Values = new float[height, width];
    }

    public static PotentialField operator +(PotentialField pf1, PotentialField pf2)
    {
        PotentialField composite = new PotentialField(pf1.Width, pf1.Height);
        for (int r = 0; r < pf1.Height; r++)
        {
            for (int c = 0; c < pf1.Width; c++)
            {
                if (pf1.Values[r, c] < 0 || pf2.Values[r,c] < 0)
                    composite.Values[r, c] = -1;
                else
                    composite.Values[r, c] = pf1.Values[r, c] + pf2.Values[r, c];
            }
        }
        return composite;
    }
}
