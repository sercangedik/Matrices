using UnityEngine;
using System.Collections;

public class CubeColoring : MonoBehaviour {

	private Matrix processMatrix = new Matrix(4, 4);
	private Matrix objectMatrix = new Matrix(4, 1);
	private Matrix result = new Matrix (4, 1);
	private Vector3 vector = new Vector3 (0, 0, 0);
	private Vector3 move = new Vector3 (0, 0 ,0);

	// Use this for initialization
	void Start () {
		objectMatrix [0, 0] = transform.position.x;
		objectMatrix [1, 0] = transform.position.y;
		objectMatrix [2, 0] = transform.position.z;
		objectMatrix [3, 0] = 1;
	}

	// Update is called once per frame
	void Update () {

		//vector.x  = vector.x + Time.time / 2000;
		vector.y  = vector.y + Time.time / 2000;
		//vector.z  = vector.z + Time.time / 2000;
		getMovingMatrix (vector);

		result = NaiveMultiplication (processMatrix, objectMatrix);
		move.x = (float)result [0, 0]; 
		move.y = (float)result [1, 0];
		move.z = (float)result [2, 0];

		transform.position = move;
	}

	void getMovingMatrix(Vector3 vector) {
		for(int i=0; i < processMatrix.Height; ++i){
			for(int j=0; j < processMatrix.Width; ++j){
				if (i == j) {
					processMatrix [i, j] = 1;
				} else if (i == 0 && j == 3) {
					processMatrix [i, j] = vector.x;
				} else if (i == 1 && j == 3) {
					processMatrix [i, j] = vector.y;
				} else if (i == 2 && j == 3) {
					processMatrix [i, j] = vector.z;
				} else {
					processMatrix [i, j] = 0;
				}
			}
		}
	}

	void getScalingMatrix(Vector3 vector) {
		for(int i=0; i < processMatrix.Height; ++i){
			for(int j=0; j < processMatrix.Width; ++j){
				if (i == 0 & j == 0) {
					processMatrix [i, j] = vector.x;
				} else if (i == 1 & j == 1) {
					processMatrix [i, j] = vector.y;
				} else if (i == 2 & j == 2) {
					processMatrix [i, j] = vector.z;
				} else if (i == 3 && j == 3) {
					processMatrix [i, j] = 1;
				} else {
					processMatrix [i, j] = 0;
				}
			}
		}
	}

	void getRotatingMatrixForY(float degree) {
		for(int i=0; i < processMatrix.Height; ++i){
			for(int j=0; j < processMatrix.Width; ++j){
				if (i == 0 && j == 0) {
					processMatrix [i, j] = Mathf.Cos (degree);
				} else if (i == 0 && j == 2) {
					processMatrix [i, j] = Mathf.Sin (degree);
				} else if (i == 2 && j == 0) {
					processMatrix [i, j] = -Mathf.Sin (degree);
				} else if (i == 2 && j == 2) {
					processMatrix [i, j] = Mathf.Cos (degree);
				} else if (i == j) {
					processMatrix [i, j] = 1;
				} else {
					processMatrix [i, j] = 0;
				}
			}
		}
	}

	void getRotatingMatrixForX(float degree) {
		for(int i=0; i < processMatrix.Height; ++i){
			for(int j=0; j < processMatrix.Width; ++j){
				if (i == 1 && j == 1) {
					processMatrix [i, j] = Mathf.Cos (degree);
				} else if (i == 1 && j == 2) {
					processMatrix [i, j] = -Mathf.Sin (degree);
				} else if (i == 2 && j == 1) {
					processMatrix [i, j] = Mathf.Sin (degree);
				} else if (i == 2 && j == 2) {
					processMatrix [i, j] = Mathf.Cos (degree);
				} else if (i == j) {
					processMatrix [i, j] = 1;
				} else {
					processMatrix [i, j] = 0;
				}
			}
		}
	}

	void getRotatingMatrixForZ(float degree) {
		for(int i=0; i < processMatrix.Height; ++i){
			for(int j=0; j < processMatrix.Width; ++j){
				if (i == 0 && j == 0) {
					processMatrix [i, j] = Mathf.Cos (degree);
				} else if (i == 0 && j == 1) {
					processMatrix [i, j] = -Mathf.Sin (degree);
				} else if (i == 1 && j == 0) {
					processMatrix [i, j] = Mathf.Sin (degree);
				} else if (i == 1 && j == 1) {
					processMatrix [i, j] = Mathf.Cos  (degree);
				} else if (i == j) {
					processMatrix [i, j] = 1;
				} else {
					processMatrix [i, j] = 0;
				}
			}
		}
	}

	Matrix NaiveMultiplication(Matrix m1, Matrix m2)
	{
		Matrix resultMatrix = new Matrix(m1.Height, m2.Width);
		for (int i = 0; i < resultMatrix.Height; i++)
		{
			for (int j = 0; j < resultMatrix.Width; j++)
			{
				resultMatrix[i, j] = 0;
				for (int k = 0; k < m1.Width; k++)
				{
					resultMatrix[i, j] += m1[i, k] * m2[k, j];
				}
			}
		}
		return resultMatrix;
	}

	Matrix NaiveAddition(Matrix m1, Matrix m2)
	{
		Matrix resultMatrix = new Matrix(m1.Height, m2.Width);
		for (int i = 0; i < resultMatrix.Height; i++)
		{
			for (int j = 0; j < resultMatrix.Width; j++)
			{
				resultMatrix[i, j] = 0;
				for (int k = 0; k < m1.Width; k++)
				{
					resultMatrix[i, j] += m1[i, k] + m2[k, j];
				}
			}
		}
		return resultMatrix;
	}
}
