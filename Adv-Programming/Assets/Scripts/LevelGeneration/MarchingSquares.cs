using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Using the 2d array generated from the Cellular Automata
 * 1.create a new square grid taking in the cellular automata grid
 * 2. intialise the vertices and triangles list
 * 3. loop through the square grid and trianglulate the squares, creating a mesh out of the active points on the square
 * 4. calculate the vertices and assign then to the vertices list
 * 5. then create and assign the triangles
 * 6. feed the calculated vertices and triangles into the mesh render and create a mesh
 * 
 */

public class MarchingSquares : MonoBehaviour
{
    public SquareGrid squareGrid;
    public bool lava;

    private List<Vector3> vertices; // A list will hold the vertices as we do not know how many vertices there may be
    private List<int> triangles; // a list is used to hold the triangles as we dont know how many triangles there will be a run time.

    //generates the mesh
    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = new SquareGrid(map, squareSize, lava); // creates a new square grid

        vertices = new List<Vector3>(); // creates a new list of vertices
        triangles = new List<int>(); // creates a new list of triangles

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++) // loop through all the squares in the square grid x
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++) // loop through all the squares in the square grid y
            {
                TriangulateSquares(squareGrid.squares[x, y]); // triangle the square for the square at position x and y
            }
        }

        Mesh mesh = new Mesh(); // creates a new mesh
        MeshFilter meshFilter = GetComponent<MeshFilter>(); // gets the mesh filter on the object
        meshFilter.mesh = mesh; // sets the meshfilters mesh to the mesh that was just created
        mesh.vertices = vertices.ToArray(); // feeds the vertices positions into the mesh
        mesh.triangles = triangles.ToArray(); // feeds the calculated triangles to the mesh
        mesh.RecalculateNormals();

        MeshCollider meshCollider = GetComponent<MeshCollider>(); // Gets the mesh collider component
        meshCollider.sharedMesh = mesh; // assigns the created mesh to the mesh collider, this allows us to collider with the generated mesh
    }

    // sets the squares tri congiurations, check documentation for marching square configurations
    void TriangulateSquares(Square square) 
    {
        switch (square.squareConfiguration)
        {
            case 0:
                break;
            //One point configurations
            case 1:
                CreateMeshFromPoints(square.centreBot, square.bottomLeft, square.centreLeft); // all cases create a mesh from the the supplied points.
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

    // create a mesh out of the points supplied
    void CreateMeshFromPoints(params Node[] points) // params is used here as we do not know how many points are going to be supplied to the function.
    {
        AssignVertices(points); // assign all the vertices

        if (points.Length >= 3) // if 3 points are supplied
        {
            CreateTriangle(points[0], points[1], points[2]); // creates a mesh from points 0 1 and 2
        }
        if (points.Length >= 4) // if 4 points are supplied also do this
        {
            CreateTriangle(points[0], points[2], points[3]); // create points from 0,2 and 3
        }
        if (points.Length >= 5) // if 5 points are supplied also do this
        {
            CreateTriangle(points[0], points[3], points[4]); // create points 0,3,4
        }
        if (points.Length >= 6) // if 6 points are supplied also do this
        {
            CreateTriangle(points[0], points[4], points[5]); // create mesh from 0,4,5
        }
    }

    // assign the vertices to the vertice list
    void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++) // loop through all of the nodes that have been supplied
        {
            if (points[i].vertexIndex == -1) // if this nodes vertext index has not been set (equals -1)
            {
                points[i].vertexIndex = vertices.Count; // make the vertex index equal the amount of vertices
                vertices.Add(points[i].position); // then add this vertex to the vertices list
            }
        }
    }
    
    // creates a triangle out of the supplied nodes and adds them to the triangles list
    void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);
    }

    // These nodes will be the intermediate nodes between the Control Nodes
    public class Node
    {
        public Vector3 position; // holds the position of the node
        public int vertexIndex = -1; // the vertex index is initialize to -1 so that it can easily be set later.

        // the node constructor
        public Node(Vector3 _position)
        {
            position = _position; // sets this nodes position
        }
    }

    // the control node inherits from the node class
    public class ControlNode : Node
    {
        public bool active; // is this node active (is it a floor tile?)
        public Node above, right; // holds the above node and the node to the right

        // Control node constructor
        public ControlNode(Vector3 _position, bool _active, float squareSize) : base(_position) // the position is set through the base Node constructor
        {
            active = _active; //sets the nodes active state

            above = new Node(position + Vector3.forward * squareSize / 2); // sets the above node, as this is a 2D representation, vector3.forward is used to get the above node.
            right = new Node(position + Vector3.right * squareSize / 2); // set the node to the right
        }
    }

    // the square class, Control nodes and the nodes between them
    public class Square
    {
        public ControlNode topLeft, topRight, bottomRight, bottomLeft; // create the control nodes
        public Node centreTop, centreRight, centreBot, centreLeft; // create the nodes

        public int squareConfiguration; // this value will equal this squares marching squares configuration, please see documentation to see marching square configurations.

        // the square constructor
        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
        {
            // set the control Nodes
            topLeft = _topLeft; 
            topRight = _topRight;
            bottomRight = _bottomRight;
            bottomLeft = _bottomLeft;

            //set the nodes depending on the control nodes
            centreTop = topLeft.right;
            centreRight = bottomRight.above;
            centreBot = bottomLeft.right;
            centreLeft = bottomLeft.above;

            if (topLeft.active)// if the top left node is active
            {
                squareConfiguration += 8; // add 8 to the square configuration
            }
            if (topRight.active) // if the top right node is active
            {
                squareConfiguration += 4; // add 4 to the square configuration
            }
            if (bottomRight.active) // if the bottom right node is active
            {
                squareConfiguration += 2; // add 2 to the square configuration
            }
            if (bottomLeft.active) // if the bottom left node is active
            {
                squareConfiguration += 1; // add one to the square configuration
            }
            // the configuration is ued for the marching squares  formula.
        }
    }

    // The square grid class, used to create the grid of squares
    public class SquareGrid
    {
        public Square[,] squares; // creates a two dimensial array of squares

        // the constructor for the square grid
        public SquareGrid(int[,] map, float squareSize, bool lava) // takes the map created from the cellular automata, a float for the size of the square and if we are generating lava or not.
        {
            float mapWidth = map.GetLength(0) * squareSize; // sets the width of the entire map
            float mapHeight = map.GetLength(1) * squareSize; // sets the height of the entire map

            ControlNode[,] controlNodes = new ControlNode[map.GetLength(0), map.GetLength(1)]; // make a two dimenisal array of control nodes the same size as the cellular automata map.

            for (int x = 0; x < map.GetLength(0); x++) // loop through the maps x
            {
                for (int y = 0; y < map.GetLength(1); y++) // loop through the maps y
                {   //TODO USE A LAMBDA EXPRESSION HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    float yPos = Random.Range(0f, 0.25f); // create a random number for the nodes y position
                    Vector3 nodePosition = new Vector3(-mapWidth/2 + x * squareSize + squareSize / 2, yPos, -mapHeight/2 + y * squareSize + squareSize / 2); // sets the position of this node

                    bool active = false; // create a bool to set if this node is a wall or lava tile
                    if (!lava && map[x, y] == 0)
                    {
                        active = true; // set true if we are generating the ground tiles
                    }
                    else if (lava && map[x, y] == 1)
                    {
                        active = true; // set true if we are generating the lava tiles
                    }
                    controlNodes[x, y] = new ControlNode(nodePosition, active, squareSize); // creates a new control node at this position of the array


                }
            }

            squares = new Square[map.GetLength(0) - 1, map.GetLength(1) - 1]; // there is always one less square than there is node
            for (int x = 0; x < map.GetLength(0)-1; x++) // loop through the map x
            {
                for (int y = 0; y < map.GetLength(1) - 1; y++) // loop through the map y
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]); // makes a new square out of the follow control node positions
                }
            }
        }
    }
}
