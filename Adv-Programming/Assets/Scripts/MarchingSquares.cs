using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingSquares : MonoBehaviour
{
    public SquareGrid squareGrid;

    private List<Vector3> vertices;
    private List<int> triangles;

    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = new SquareGrid(map, squareSize);

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquares(squareGrid.squares[x, y]);
            }
        }

        Mesh mesh = new Mesh();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void TriangulateSquares(Square square)
    {
        switch (square.squareConfiguration)
        {
            case 0:
                break;
            //One point configurations
            case 1:
                CreateMeshFromPoints(square.centreBot, square.bottomLeft, square.centreLeft);
                break;
            case 2:
                CreateMeshFromPoints(square.centreRight, square.bottomRight, square.centreBot);
                break;
            case 4:
                CreateMeshFromPoints(square.centreTop, square.topRight, square.centreRight);
                break;
            case 8:
                CreateMeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
                break;

            //two point configurations
            case 3:
                CreateMeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                break;
            case 6:
                CreateMeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBot);
                break;
            case 9:
                CreateMeshFromPoints(square.topLeft, square.centreTop, square.centreBot, square.bottomLeft);
                break;
            case 12:
                CreateMeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
                break;
            case 5:
                CreateMeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBot, square.bottomLeft, square.centreLeft);
                break;
            case 10:
                CreateMeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBot, square.centreLeft );
                break;

            //three point configurations
            case 7:
                CreateMeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                break;
            case 11:
                CreateMeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
                break;
            case 13:
                CreateMeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBot, square.bottomLeft);
                break;
            case 14:
                CreateMeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBot, square.centreLeft);
                break;

            //four point configurations
            case 15:
                CreateMeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
                break;
        }
    }

    void CreateMeshFromPoints(params Node[] points)
    {
        AssignVertices(points);

        if (points.Length >= 3)
        {
            CreateTriangle(points[0], points[1], points[2]);
        }
        if (points.Length >= 4)
        {
            CreateTriangle(points[0], points[2], points[3]);
        }
        if (points.Length >= 5)
        {
            CreateTriangle(points[0], points[3], points[4]);
        }
        if (points.Length >= 6)
        {
            CreateTriangle(points[0], points[4], points[5]);
        }
    }

    void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].vertexIndex == -1)
            {
                Debug.Log("Running");
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }

    void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);
    }

    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 _position)
        {
            position = _position;
        }
    }

    public class ControlNode : Node
    {
        public bool active;
        public Node above, right;

        public ControlNode(Vector3 _position, bool _active, float squareSize) : base(_position)
        {
            active = _active;

            above = new Node(position + Vector3.forward * squareSize / 2);
            right = new Node(position + Vector3.right * squareSize / 2);
        }
    }

    public class Square
    {
        public ControlNode topLeft, topRight, bottomRight, bottomLeft;
        public Node centreTop, centreRight, centreBot, centreLeft;

        public int squareConfiguration;

        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
        {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomRight = _bottomRight;
            bottomLeft = _bottomLeft;

            centreTop = topLeft.right;
            centreRight = bottomRight.above;
            centreBot = bottomLeft.right;
            centreLeft = bottomLeft.above;

            if (topLeft.active)
            {
                squareConfiguration += 8;
            }
            if (topRight.active)
            {
                squareConfiguration += 4;
            }
            if (bottomRight.active)
            {
                squareConfiguration += 2;
            }
            if (bottomLeft.active)
            {
                squareConfiguration += 1;
            }
        }
    }

    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            float mapWidth = map.GetLength(0) * squareSize;
            float mapHeight = map.GetLength(1) * squareSize;

            ControlNode[,] controlNodes = new ControlNode[map.GetLength(0), map.GetLength(1)];

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Vector3 nodePosition = new Vector3(-mapWidth/2 + x * squareSize + squareSize / 2, 0, -mapHeight/2 + y * squareSize + squareSize / 2);

                    bool active = false;
                    if (map[x, y] == 0)
                    {
                        active = true;
                    }
                    controlNodes[x, y] = new ControlNode(nodePosition, active, squareSize);


                }
            }

            squares = new Square[map.GetLength(0) - 1, map.GetLength(1) - 1]; // there is always one less square than there is node
            for (int x = 0; x < map.GetLength(0)-1; x++)
            {
                for (int y = 0; y < map.GetLength(1) - 1; y++)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }
        }
    }
}
