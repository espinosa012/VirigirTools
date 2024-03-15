using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using VirigirTools.math;

namespace VirigirTools.opencv;

public static class VOpenCvHelper
{
    
    public static VMatrix ImageToMatrix(VImage input)
    {
        var matrix = new VMatrix(input.Rows, input.Cols, 0.0f);
        for (var i = 0; i < input.Rows; i++)
        for (var j = 0; j < input.Rows; j++)
            matrix.SetValue(i, j,input.GetValueAt(i, j));
        return matrix;
    }
    
    public static VImage MatrixToImage(VMatrix matrix)// TODO: devolver nuestra entidad imagen
    {
        var image = new VImage(matrix.GetRowCount(), matrix.GetColumnCount());
        for (var i = 0; i < matrix.GetRowCount(); i++)
        for (var j = 0; j < matrix.GetColumnCount(); j++)
            image.Draw(new Rectangle(j, i, 1, 1),
                VPaletteHelper.NormValueToBgr(matrix.GetValue(i, j)), -1);
        return image;
    }
    
    public static VImage GetGaussianFiltered(VImage input, int kernelSize)
    {
        var output = new Mat(new Size(input.Size.Width, input.Size.Height), DepthType.Cv8U, 1);
        CvInvoke.GaussianBlur(input, output, new Size(kernelSize, kernelSize), 0);  // cuanto mayor sea sigma, más borrosa será la imagen
        return new VImage(output.ToImage<Bgr, byte>());
    }
    
    public static VImage GetGaussianFilteredIterating(VImage input, int kernelSize, int nIterations)
    {
        // KernelSize tiene que ser impar, nIterations >= 1
        var it1 = GetGaussianFiltered(input, kernelSize);
        for (int i = 0; i < nIterations - 1; i++)
            it1 = GetGaussianFiltered(it1, kernelSize);
        return it1;
    }

    public static VImage ScaleImage(VImage input, float scaleFactor) => new (scaleFactor * input);

    public static void EqualizeHistogram(VImage input) => input._EqualizeHist();

    public static VImage ThresholdBinaryImage(VImage input, int threshold) =>
        new (input.ThresholdBinary(new Bgr(threshold, threshold, threshold),
            new Bgr(255, 255, 255)));
}