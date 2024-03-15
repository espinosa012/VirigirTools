using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

public class VMatrix
{
    // Constructors
    private Matrix<float> _data;

    public VMatrix()
    {
    }

    public VMatrix(int sizeX, int sizeY, float initVal) => _data = BuildMatrix(sizeX, sizeY, initVal);
    public VMatrix(Matrix<float> data) => _data = data;

    // Matrix mathematical operations
    public void SumMatrix(VMatrix matrixToSum) => _data += matrixToSum.GetMatrix(); 
    public void SumFactor(float factor) => _data += factor; 
    public void SubtractMatrix(VMatrix matrixToSum) => _data -= matrixToSum.GetMatrix();
    public void SubtractFactor(float factor) => _data -= factor;
    public void ScaleMatrix(float factor) => _data *= factor;

    // getters & setters
    public Matrix<float> GetMatrix() => _data;
    public float GetValue(int x, int y) => _data[x, y];
    public void SetValue(int x, int y, float value) => _data[x, y] = value;
    public int GetRowCount() => _data.RowCount;
    public int GetColumnCount() => _data.ColumnCount;
    
    
    // operations 
    public void Transpose() => _data = _data.Transpose();
    
    public float GetSubMatrixAverage(int centroidX, int centroidY, int sizeX, int sizeY)
    {
        var subMatrix = _data.SubMatrix(centroidX - sizeX / 2,
            sizeX, centroidY - sizeY / 2, sizeY);
        return Enumerable.Sum(subMatrix.ColumnSums()) / (sizeY * sizeX);
    }

    public void ThresholdMatrixByTier(int minTier, int nTiers)
    {
        for (var i = 0; i < _data.RowCount; i++)
        for (var j = 0; j < _data.ColumnCount; j++)
            _data[i, j] =
                (VMathHelper.GetNormalizedValueTier(_data[i, j], nTiers) <= minTier) ? 0.0f : _data[i, j];
    }

    public void InverseThresholdMatrixByTier(int maxTier, int nTiers)
    {
        for (var i = 0; i < _data.RowCount; i++)
        for (var j = 0; j < _data.ColumnCount; j++)
            _data[i, j] =
                (VMathHelper.GetNormalizedValueTier(_data[i, j], nTiers) > maxTier) ? 0.0f : _data[i, j];
    }

    public void RangeThresholdMatrixByTier(int minTier, int maxTier, int nTiers)
    {
        ThresholdMatrixByTier(minTier, nTiers);
        InverseThresholdMatrixByTier(maxTier, nTiers);
    }


    // aux
    public VMatrix Clone() => new(_data.Clone());

    private static Matrix<float> BuildMatrix(int sizeX, int sizeY, float initVal) =>
        DenseMatrix.Build.Dense(sizeX, sizeY, initVal);
}