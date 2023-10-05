using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeAutoFind : MonoBehaviour
{
    List<Node> SameNode = new List<Node>();

    public void AutoFind()
    {
        GetSameNode();
        foreach (Node node1 in SameNode)
        {
            NodeManager.Ins.fristNode = node1;
            foreach (Node node2 in SameNode)
            {   
               if(node1.transform != node2.transform)
                {
                    NodeManager.Ins.lastNode = node2;
                    NodeManager.Ins.PathFinDing(node1, node2);
                    if (NodeManager.Ins.isFound)
                    {
                        SameNode.Clear();
                        return;
                    } 
                        
                }
            }
        }
    }
    public List<Node> GetSameNode()
    {
      
        foreach (Node node in NodeManager.Ins.arrayNodes)
        {
            if(node.pokemonType != PokemonType.None)
            {
                SameNode.Add(node);
            }
        }return SameNode;
    }

}
