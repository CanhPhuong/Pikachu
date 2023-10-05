using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnAutoFind : MonoBehaviour
{
    // Start is called before the first frame update
    public void HindNode()
    {
        NodeManager.Ins.nodeAutoFind.AutoFind();
        /*NodeManager.Ins.AutoFind();*/
        Debug.Log("đã tìm kiếm tự động được");

    }
}
