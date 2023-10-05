using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int x;
    public int y;
  public  bool curren;
    public Node up;
    public Node left;
    public Node down;
    public Node right;

    public PokemonType pokemonType;

    [SerializeField] PokemonData pokemonData;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private List<Node> NodeNeighbour;
    [SerializeField] private LineRenderer lineRenderer;

    internal void OnInit(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    internal void SetPokemonType(PokemonType type)
    {
        spriteRenderer.sprite = pokemonData.GetSprite(type);
    }
    
}
