using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PokemonType
{
    None = 0,    
    Pokemon_1 = 1,
    Pokemon_2 = 2,
    Pokemon_3 = 3,
    Pokemon_4 = 4,
    Pokemon_5 = 5,
    Pokemon_6 = 6,
    Pokemon_7 = 7,
    Pokemon_8 = 8,
    Pokemon_9 = 9,
    Pokemon_10 = 10,
    Pokemon_11 = 11,
    Pokemon_12 = 12,
    Pokemon_13 = 13,
    Pokemon_14 = 14,
    Pokemon_15 = 15,
    Pokemon_16 = 16,
    Pokemon_17 = 17,
    Pokemon_18 = 18,
    Pokemon_19 = 19,
    Pokemon_20 = 20,
    Pokemon_21 = 21,
    Pokemon_22 = 22,
    Pokemon_23 = 23,
    Pokemon_24 = 24,
    Pokemon_25 = 25,
    Pokemon_26 = 26,
    Pokemon_27 = 27,
    Pokemon_28 = 28,
    Pokemon_29 = 29,
    Pokemon_30 = 30,
    Pokemon_31 = 31,
    Pokemon_32 = 32,
    Pokemon_33 = 33,
    Pokemon_34 = 34,
    Pokemon_35 = 35,
    Pokemon_36 = 36,

    

}

public class NodeManager : MonoBehaviour
{
   
    public Node fristNode;
    public Node lastNode;
    [SerializeField] private Transform hodel;
    public Vector2 space;
    public NodeAutoFind nodeAutoFind;
    public bool isFound;
    public static NodeManager Ins;

    public int width = 18;
    public int height = 11;
    [SerializeField] GameObject pokemonPrefab;
    [SerializeField] float maxCurren = 3;

    public PokemonData pokemonData;
    public Dictionary<PokemonType, int> imageDictionary; // Dictionary để ánh xạ enum đến hình ảnh.
    [SerializeField] private List<PokemonType> PokemonTypes = new List<PokemonType>();
    public Node[,] arrayNodes = new Node[18, 11];
    public List<Node> freeNode = new List<Node>();
    public List<PokemonType> freePokemonType = new List<PokemonType>();


    private void Awake()
    {
        Ins = this;

    }
    private void Update()
    {

    }

    private void Start()
    {   
        LoadHodel();
        InitDic();
        InitGrid();
        InitType();
        FindNodesNeighbors();
        nodeAutoFind = FindObjectOfType<NodeAutoFind>();

    }
    private void InitDic()
    {
        imageDictionary = new Dictionary<PokemonType, int>();
        foreach (var key in imageDictionary.Keys)
        {
            imageDictionary[key] = 0;
        }
    }
    public void InitGrid()
    {

        InitListNode();

        //tao danh sach node
        //cho node vao mot collection
        //set sprite cho node
    }
    public void InitListNode()
    {
        Vector3 startPoint = new Vector3(space.x * this.width / 2, space.y * this.height / 2);

        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                Node node = Instantiate(pokemonPrefab, new Vector3(x * space.x, y * space.y, 0) - startPoint, Quaternion.identity).GetComponent<Node>();
                node.OnInit(x, y);
                arrayNodes[x, y] = node;
                arrayNodes[x, y].gameObject.transform.SetParent(hodel);
            }
        }
    }
    public void LoadHodel()
    {
        if (this.hodel != null) return;
        this.hodel = transform.Find("Hodel");
    }


    public void InitType()
    {
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                if (x == 0 || y == 0 || x == (this.width - 1) || y == (this.height - 1))
                {
                    arrayNodes[x, y].SetPokemonType(PokemonType.None);
                    arrayNodes[x, y].curren = false;
                }
                else
                {
                    PokemonType randomType;
                    do
                    {
                        randomType = (PokemonType)Random.Range(1, 37);
                        if (!imageDictionary.ContainsKey(randomType))
                        {
                            imageDictionary.Add(randomType, 0);
                        }
                    } while (imageDictionary.ContainsKey(randomType) && imageDictionary[randomType] > maxCurren);

                    if (imageDictionary.ContainsKey(randomType))
                    {
                        imageDictionary[randomType]++;
                    }
                    arrayNodes[x, y].SetPokemonType(randomType);
                    arrayNodes[x, y].pokemonType = randomType;
                    arrayNodes[x, y].curren = true;

                }
            }
        }
    }




    public Node GetNodeFromIndex(int x, int y)
    {
        return arrayNodes[x, y];
    }

    public (int, int) GetIndexFromNode(Node node)
    {
        return (node.x, node.y);
    }
    public void SetNode(Node node)
    {
        if (node.x != 0 && node.x != this.width - 1 && node.y != 0 && node.y != this.height - 1)
        {
            
           
            if (this.fristNode == null)
            {
                this.fristNode = node;
                
                return;
            }
           
                this.lastNode = node;
            if (this.fristNode == this.lastNode)
            {
                this.lastNode = null;
                return;
            }
            


            (fristNode, lastNode) = SwapNodeX(fristNode, lastNode);//tham trị chứ k phải tham chiếu
            PathFinDing(fristNode, lastNode);
            this.fristNode = null;
            this.lastNode = null;
           
                return;
            
        }


    }
    protected virtual void FindNodesNeighbors()
    {

        int x, y;

        foreach (Node node in this.arrayNodes)

        {

            if (node.pokemonType == PokemonType.None)
            {

            }
            else
            {
                x = node.x;
                y = node.y;
                node.up = this.GetNodeByXY(x, y + 1);
                node.right = this.GetNodeByXY(x + 1, y);
                node.down = this.GetNodeByXY(x, y - 1);
                node.left = this.GetNodeByXY(x - 1, y);
            }
        }

    }
    protected virtual Node GetNodeByXY(int x, int y)
    {

        return arrayNodes[x, y];
    }
    public void HideNode()
    {
        AddPokemonType(fristNode, lastNode);
        AddNode(fristNode, lastNode);
        fristNode.gameObject.SetActive(false);
        lastNode.gameObject.SetActive(false);
        fristNode.curren = false;
        lastNode.curren = false;
        fristNode.pokemonType = PokemonType.None;
        lastNode.pokemonType = PokemonType.None;
        isFound = true;
        Debug.Log(arrayNodes.Length);

    }
    
    public void AddPokemonType()
    {
        foreach (Node node in arrayNodes)
        {
            
            if (node.pokemonType != PokemonType.None)
            {
                PokemonTypes.Add(node.pokemonType);
            }
        }
    }
    public void ReverseNode()
    {
        AddPokemonType();
        foreach (Node node in arrayNodes)
        {
            if(node.pokemonType != PokemonType.None)
            {
                PokemonType randomType;
                int randomIndex = Random.Range(0, PokemonTypes.Count);
                randomType = PokemonTypes[randomIndex];
                node.pokemonType = randomType;
                node.SetPokemonType(randomType);
                PokemonTypes.RemoveAt(randomIndex);
                Debug.LogError("PokemonTypes.count" + " = " + PokemonTypes.Count);
            }
        }
    }
    //free node chứa node đã được ăn
    public void PutOutNode()
    {
        int lastIndexNode = freeNode.Count - 1;
        if (lastIndexNode < 0) return;
        int lastIndexPokemonType = freePokemonType.Count - 1;
        Node lastNode = freeNode[lastIndexNode];
        lastNode.gameObject.SetActive(true);
        lastNode.pokemonType = freePokemonType[lastIndexPokemonType];
        lastNode.curren = true;
        freeNode.RemoveAt(lastIndexNode);    
        freePokemonType.RemoveAt(lastIndexPokemonType);
    }
    public void DeleteNode(Node node)
    {
        
            //TO DO;
            node.gameObject.SetActive(false);
            
        
    }

    public void AddNode(Node node1, Node node2)
    {
        freeNode.Add(node1);
        freeNode.Add(node2);
    }
    public void AddPokemonType(Node node1, Node node2)
    {
        freePokemonType.Add(node1.pokemonType);
        freePokemonType.Add(node2.pokemonType);
    }

    public void PathFinDing(Node fristNode, Node lastNode)
    {
        Debug.Log("đã chạy vào PathFinDing");
        isFound = false;
        if (fristNode.pokemonType == lastNode.pokemonType)
        {
            Debug.Log(fristNode.x +"/"+ fristNode.y + " ----------------------" + lastNode.x + "/" + lastNode.y);
            if ((Mathf.Abs(lastNode.x - fristNode.x) == 1 && lastNode.y == fristNode.y) 
                || (Mathf.Abs(lastNode.y - fristNode.y) == 1 && lastNode.x == fristNode.x))
            {
                Debug.Log("2 node cạnh nhau");
               /* AddPokemonType(fristNode, lastNode);
                AddNode(fristNode, lastNode);*/
                HideNode();

                return;
            }

            

            if (fristNode.right.curren == false && lastNode.up.curren == false)
            {
                int x = lastNode.x;
                int y = fristNode.y;
                Node temp = this.GetNodeByXY(x, y);
                if (IsCheckHorizontal(fristNode, temp) && IsCheckVertical(temp, lastNode) && temp.pokemonType == PokemonType.None)
                {
                    /*lineRenderer.SetPosition(0, fristNode.position);
                    lineRenderer.SetPosition(1, temp.position);*/

                    HideNode();
                    return;
                }
               
            }
            if (fristNode.down.curren == false && lastNode.left.curren == false)
            {
                int x = fristNode.x;
                int y = lastNode.y;
                Node temp = this.GetNodeByXY(x, y);
                if (IsCheckVertical(fristNode, temp) && IsCheckHorizontal(temp, lastNode) && temp.pokemonType == PokemonType.None)
                {
                    HideNode();
                    return;
                }
                
            }
            if (fristNode.up.curren == false && lastNode.left.curren == false)

            {
                int x = fristNode.x;
                int y = lastNode.y;
                Node temp = this.GetNodeByXY(x, y);
                if (IsCheckVertical(fristNode, temp) && IsCheckHorizontal(temp, lastNode) &&temp.pokemonType == PokemonType.None)
                {
                    HideNode();
                    return;
                }
                
            }
            if (fristNode.right.curren == false && lastNode.down.curren == false)

            {
                int x = lastNode.x;
                int y = fristNode.y;
                Node temp = this.GetNodeByXY(x, y);
                if (IsCheckHorizontal(fristNode, temp) && IsCheckVertical(temp, lastNode) && temp.pokemonType == PokemonType.None)
                {
                    AddPokemonType(fristNode, lastNode);
                    AddNode(fristNode, lastNode);
                    HideNode();
                    return;
                }
               
            }

            /* if (fristNode.up.curren == false && lastNode.up.curren == false ||
                 fristNode.up.curren == false && lastNode.down.curren == false ||
                 fristNode.down.curren == false && lastNode.down.curren == false ||
                 fristNode.down.curren == false && lastNode.up.curren == false)
             {*/
            if (fristNode.up.curren == false && lastNode.up.curren == false ||
            fristNode.up.curren == false && lastNode.down.curren == false ||
            fristNode.down.curren == false && lastNode.down.curren == false)//xóa bỏ điều kiện thứ 4
            {
                Debug.Log(" cung ho dọc");
                for (int i = 0; i < 11; i++)
                {
                    if (IsCheckVertical(fristNode, GetNodeByXY(fristNode.x, i)) &&
                        IsCheckHorizontal(GetNodeByXY(fristNode.x, i), GetNodeByXY(lastNode.x, i))
                        && IsCheckVertical(GetNodeByXY(lastNode.x, i), lastNode) 
                        && GetNodeByXY(fristNode.x, i).pokemonType == PokemonType.None 
                        && GetNodeByXY(lastNode.x, i).pokemonType == PokemonType.None)
                    {
                        AddPokemonType(fristNode, lastNode);
                        AddNode(fristNode, lastNode);
                        HideNode();
                        return;
                    }
                }

            }

            /*if (fristNode.right.curren == false && lastNode.left.curren == false ||
            fristNode.right.curren == false && lastNode.right.curren == false ||
            fristNode.left.curren == false && lastNode.left.curren == false ||
            fristNode.left.curren == false && lastNode.right.curren == false)
            {*/
            if (fristNode.right.curren == false && lastNode.left.curren == false ||
         fristNode.right.curren == false && lastNode.right.curren == false ||
         fristNode.left.curren == false && lastNode.left.curren == false)//xóa bỏ điều kiện thứ 4
            {
                
                Debug.Log(" cung ho ngang");
                for (int i = 0; i < 18; i++)
                {
                    if (IsCheckHorizontal(fristNode, GetNodeByXY(i, fristNode.y))
                        
                        && IsCheckVertical(GetNodeByXY(i, fristNode.y), GetNodeByXY(i, lastNode.y)) 
                        && IsCheckHorizontal(GetNodeByXY(i, lastNode.y), lastNode)
                        && GetNodeByXY(i, fristNode.y).pokemonType == PokemonType.None
                        && GetNodeByXY(i, lastNode.y).pokemonType == PokemonType.None)
                    {   
                        AddPokemonType(fristNode, lastNode);
                        AddNode(fristNode, lastNode);
                        HideNode();
                        return;
                    }
                }
               
            }
        }
        
    }
    public bool IsCheckHorizontal(Node node1, Node node2)
    {
        Debug.Log(node1.x + " --" + node1.y);
        Debug.Log(node2.x + " --" + node2.y);
        for (int i = node1.x + 1; i <= node2.x - 1; i++)
        {   
             if (GetNodeByXY(i, node1.y).pokemonType != PokemonType.None)
                {
                Debug.Log(GetNodeByXY(i, node1.y).pokemonType);
                Debug.Log(GetNodeByXY(i, node1.y).x + " __" + GetNodeByXY(i, node1.y).y);
                return false;
                }
            
        }
        Debug.Log("check hàng ngang thấy k có node ngăn cản");
        return true;
    }
/*    public bool CheckHorizontalRangeExclusive(Node node1, Node node2)
    {
        Debug.Log(node1.x + " --" + node1.y);
        Debug.Log(node2.x + " --" + node2.y);
        for (int i = node1.x + 1; i <= node2.x - 1; i++)
        {
            if (GetNodeByXY(i, node1.y).pokemonType != PokemonType.None)
            {
                Debug.Log(GetNodeByXY(i, node1.y).pokemonType);
                Debug.Log(GetNodeByXY(i, node1.y).x + " __" + GetNodeByXY(i, node1.y).y);
                return false;
            }

        }
        Debug.Log("check hàng ngang thấy k có node ngăn cản");
        return true;
    }*/



    public bool IsCheckVertical(Node node1, Node node2)
    {
        Debug.Log(node1.x + " --" + node1.y);
        Debug.Log(node2.x + " --" + node2.y);
        (node1, node2) = SwapNodeY(node1, node2);

        for (int i = node1.y + 1; i <= node2.y - 1; i++)
        {
           
            if (GetNodeByXY(node1.x, i).pokemonType != PokemonType.None)
            {
                Debug.Log(GetNodeByXY(node1.x, i).pokemonType);
                Debug.Log(GetNodeByXY(node1.x, i).x + " __" + GetNodeByXY(node1.x, i).y);
                return false;
            }
        }
        Debug.Log("check hàng dọc thấy k có node ngăn cản");
        return true;
    }
/*    public bool CheckVerticallRangeExclusive(Node node1, Node node2)
    {   
        Debug.Log(node1.x + " --" + node1.y);
        Debug.Log(node2.x + " --" + node2.y);
        (node1, node2) = SwapNodeY(node1, node2);

        for (int i = node1.y + 1; i <= node2.y -1; i++)
        {

            if (GetNodeByXY(node1.x, i).pokemonType != PokemonType.None)
            {
                

                Debug.Log("check hàng dọc và thấy có node ngăn cản");
                Debug.Log(GetNodeByXY(node1.x, i).x + " __" + GetNodeByXY(node1.x, i).y);
                Debug.Log(GetNodeByXY(node1.x, i).pokemonType);
                return false;
            }
        }
        Debug.Log("check hàng dọc thấy k có node ngăn cản");
        return true;
    }*/



    public (Node, Node) SwapNodeX(Node node1, Node node2)
    {
        return node1.x < node2.x ? (node1, node2) : (node2, node1);
    }  
    
    public (Node, Node) SwapNodeY(Node node1, Node node2)
    {
        return node1.y < node2.y ? (node1, node2) : (node2, node1);
    }
/*   public List<Node> SameNode = new List<Node>();

    public void AutoFind()
    {
        GetSameNode();
        Debug.Log("đã chạy vào Autofind");
        foreach (Node node1 in SameNode)
        {
            foreach (Node node2 in SameNode)
            {
                if (node1.transform != node2.transform)
                {
                    PathFinDing(node1, node2);
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

        foreach (Node node in nodes)
        {
            if (node.pokemonType != PokemonType.None)
            {
                SameNode.Add(node);
            }
        }
        return SameNode;
    }*/
}
