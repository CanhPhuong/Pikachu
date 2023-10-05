using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PokemonData")]
public class PokemonData : ScriptableObject
{
    public Sprite[] sprites;

    public Sprite GetSprite(PokemonType type)
    {
        return sprites[(int)type];
    }
}
