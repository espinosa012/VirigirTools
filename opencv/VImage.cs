using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

public class VImage : Image<Bgr, byte>
{
    public VImage(int sizeX, int sizeY)
    {
        Data = (new Mat(new Size(sizeX, sizeY), DepthType.Cv8U, 1)).ToImage<Bgr, byte>().Data;
    }

    public VImage(Image<Bgr, byte> img)
    {
        Data = img.Data;
    }

    // values
    public float GetValueAt(int x, int y) => Data[x, y, 0] / 255f;


    // aux
    public void Save(string path) =>
        Flip(FlipType.Horizontal).Rotate(90, new Bgr(0, 0, 0)).Save(path); //TODO:addextesion
}