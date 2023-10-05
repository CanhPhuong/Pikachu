using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFingding : MonoBehaviour
{
    public Queue<Node> queue = new Queue<Node>();
    public  void FindPath(Node startNode, Node endNode)
    {
        this.queue.Enqueue(startNode);
        while(this.queue.Count > 0)
        {
            Node currenqueue = this.queue.Dequeue();
            if (currenqueue == endNode) { break; }
           /* foreach(Node node in )*/

        }
    }

}
