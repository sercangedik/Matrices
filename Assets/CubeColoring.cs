using UnityEngine;
using System;
using System.Collections;

public class CubeColoring : MonoBehaviour {

	private Matrix processMatrix = new Matrix(4, 4);
	private Matrix objectMatrix = new Matrix(4, 1);
	private Matrix result = new Matrix (4, 1);
	private Vector3 vector = new Vector3 (0, 0, 0);
	private Vector3 move = new Vector3 (0, 0 ,0);
	private Mesh mesh;
	private Vector3[] vertices;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter> ().mesh;
		vertices = mesh.vertices;
		Color[] colors = new Color[vertices.Length];
		Array.ForEach<Color> (colors, c => new Color (1, 1, 1, 1));
		mesh.colors = colors;
		objectMatrix [0, 0] = transform.position.x;
		objectMatrix [1, 0] = transform.position.y;
		objectMatrix [2, 0] = transform.position.z;
		objectMatrix [3, 0] = 1;
	}

	// Update is called once per frame
	void Update () {
		//rotateX ();
		//rotateY ();
		//rotateZ ();
		//moveCube ();
		//scaleCube ();
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

	void rotateX() {
		float degree = Time.time / 200;

		for(int i = 0; i < vertices.Length; ++i) {
			objectMatrix [0, 0] = vertices [i].x;
			objectMatrix [1, 0] = vertices [i].y;
			objectMatrix [2, 0] = vertices [i].z;
			objectMatrix [3, 0] = 1;

			getRotatingMatrixForX (degree);
			result = NaiveMultiplication (processMatrix, objectMatrix);

			vertices [i].x = (float)result [0, 0];
			vertices [i].y = (float)result [1, 0];
			vertices [i].z = (float)result [2, 0];

		}

		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}

	void rotateY() {
		float degree = Time.time / 200;

		for(int i = 0; i < vertices.Length; ++i) {
			objectMatrix [0, 0] = vertices [i].x;
			objectMatrix [1, 0] = vertices [i].y;
			objectMatrix [2, 0] = vertices [i].z;
			objectMatrix [3, 0] = 1;

			getRotatingMatrixForY (degree);
			result = NaiveMultiplication (processMatrix, objectMatrix);

			vertices [i].x = (float)result [0, 0];
			vertices [i].y = (float)result [1, 0];
			vertices [i].z = (float)result [2, 0];

		}

		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}

	void rotateZ() {
		float degree = Time.time / 200;

		for(int i = 0; i < vertices.Length; ++i) {
			objectMatrix [0, 0] = vertices [i].x;
			objectMatrix [1, 0] = vertices [i].y;
			objectMatrix [2, 0] = vertices [i].z;
			objectMatrix [3, 0] = 1;

			getRotatingMatrixForZ (degree);
			result = NaiveMultiplication (processMatrix, objectMatrix);

			vertices [i].x = (float)result [0, 0];
			vertices [i].y = (float)result [1, 0];
			vertices [i].z = (float)result [2, 0];

		}

		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}

	void moveCube(){ 
		float degree = Time.time / 200;

		vector.x = 0;//degree;
		vector.y  = 0;//degree;
		vector.z  = 0;//degree;

		for(int i = 0; i < vertices.Length; ++i) {
			objectMatrix [0, 0] = vertices [i].x;
			objectMatrix [1, 0] = vertices [i].y;
			objectMatrix [2, 0] = vertices [i].z;
			objectMatrix [3, 0] = 1;

			getMovingMatrix (vector);
			result = NaiveMultiplication (processMatrix, objectMatrix);

			vertices [i].x = (float)result [0, 0];
			vertices [i].y = (float)result [1, 0];
			vertices [i].z = (float)result [2, 0];

		}

		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	} 

	void scaleCube() {
		float degree = Time.time / 200;

		vector.x  = 1 + degree;
		vector.y = 1;// + degree;
		vector.z  = 1;// + degree;

		for(int i = 0; i < vertices.Length; ++i) {
			objectMatrix [0, 0] = vertices [i].x;
			objectMatrix [1, 0] = vertices [i].y;
			objectMatrix [2, 0] = vertices [i].z;
			objectMatrix [3, 0] = 1;
			getScalingMatrix (vector);
			result = NaiveMultiplication (processMatrix, objectMatrix);

			vertices [i].x = (float)result [0, 0];
			vertices [i].y = (float)result [1, 0];
			vertices [i].z = (float)result [2, 0];

		}
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}
}