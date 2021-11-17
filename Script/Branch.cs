using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
	public Material material;

	public float radiusUp;
	public float radiusBottom;
	public float height;
	public int nbMeridians;
	public int nbParallels;

	private Vector3[] vertices;
	private int[] triangles;
	private int nbTriangles;
	private int nbVertices;

	public Tree tree;


	void Start()
	{
	}

	public void GenerateBranch(float radBottom)
    {
		radiusBottom = radBottom;
		// Création d'un composant MeshFilter qui peut ensuite être visualisé

		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();

		nbTriangles = (nbMeridians * 6) * (nbParallels + 1) + (nbMeridians * 6);
		nbVertices = (nbMeridians + 1) * (nbParallels + 2) + 2;

		vertices = new Vector3[nbVertices];
		triangles = new int[nbTriangles];

		// Vertices
		float x, y, radius;

		radiusUp = radiusBottom - (radiusBottom * 0.2f);
		for (int i = 0; i <= nbMeridians; i++)
			for (int j = 0; j <= nbParallels + 1; j++)
			{
				radius = radiusBottom - (radiusBottom - radiusUp) * ((float)j / (nbParallels + 1));
				x = radius * Mathf.Cos(2 * Mathf.PI * i / nbMeridians);
				y = radius * Mathf.Sin(2 * Mathf.PI * i / nbMeridians);
				vertices[i + ((nbMeridians + 1) * j)] = new Vector3(x, height / (nbParallels + 1) * j, y);
			}

		vertices[nbVertices - 2] = new Vector3(0, 0, 0);
		vertices[nbVertices - 1] = new Vector3(0, height, 0);

		// Triangles
		int nbTrianglesCorps = nbTriangles - (nbMeridians * 6);

		for (int i = 0, k = 0, p = 0; i <= nbTrianglesCorps; i += 6, k++, p++)
		{
			if (p == nbMeridians) { p = 0; k++; }
			triangles[i] = triangles[i + 3] = k;
			triangles[i + 1] = k + nbMeridians + 1;
			triangles[i + 2] = triangles[i + 4] = k + nbMeridians + 2;
			triangles[i + 5] = k + 1;
		}

		for (int i = 0, k = 0; k < nbMeridians; i += 6, k++)
		{
			triangles[i + nbTrianglesCorps] = k;
			triangles[i + nbTrianglesCorps + 1] = k + 1;
			triangles[i + nbTrianglesCorps + 2] = nbVertices - 2;
			triangles[i + nbTrianglesCorps + 3] = k + (nbMeridians + 1) * (nbParallels + 1) + 1;
			triangles[i + nbTrianglesCorps + 4] = k + (nbMeridians + 1) * (nbParallels + 1);
			triangles[i + nbTrianglesCorps + 5] = nbVertices - 1;
		}

		// Création et remplissage du Mesh

		Mesh mesh = new Mesh();

		mesh.vertices = vertices;
		mesh.triangles = triangles;

		// Remplissage du Mesh et ajout du material
		gameObject.GetComponent<MeshFilter>().mesh = mesh;
		gameObject.GetComponent<MeshRenderer>().material = material;
		gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
	}
}