using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BlockClickable : MonoBehaviour
{
    [Header("Block Clickable")]
    public BoxCollider _collider;
   [SerializeField] private Node nodeCtl;
    private void Start()
    {
        LoadComponents();
        
    }
    //private void Update()
    //{
    //    OnMouseUp();
    //}
    protected  void LoadComponents()
    {
       
        this.LoadColider();
        LoadNodeCtrl();
    }

    protected virtual void LoadColider()
    {
        if (this._collider != null) return;
        this._collider = GetComponent<BoxCollider>();
        this._collider.isTrigger = true;
        this._collider.size = new Vector3(0.386155605f, 0.530452847f, 0.5f);
        
    }
    protected virtual void LoadNodeCtrl()
    {
        if (this.nodeCtl != null) return;
        this.nodeCtl = transform.parent.GetComponent<Node>();
       
    }

    protected void OnMouseUp()
    {
        
        NodeManager.Ins.SetNode(nodeCtl);
    }
    public void DeleteNdode()
    {   // To DO;
        NodeManager.Ins.DeleteNode(nodeCtl);
    }
    
}
