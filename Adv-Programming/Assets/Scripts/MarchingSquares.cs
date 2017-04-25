using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingSquares : MonoBehaviour
{
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
        }
    }

    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            float mapWidth = map.GetLength(0) * squareSize;
            float mapHeight = map.GetLength(1) * squareSize;
        }
    }
}
