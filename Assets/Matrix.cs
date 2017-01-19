using System;

class Matrix
{
	private readonly float[,] _matrix;
	public Matrix(int dim1, int dim2)
	{
		_matrix = new float[dim1, dim2];
	}

	public int Height { get { return _matrix.GetLength(0); } }
	public int Width { get { return _matrix.GetLength(1); } }

	public float this[int x, int y]
	{
		get { return _matrix[x, y]; }
		set { _matrix[x, y] = value; }
	}
}