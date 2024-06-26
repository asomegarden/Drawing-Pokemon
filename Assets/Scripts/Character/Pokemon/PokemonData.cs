using UnityEngine;

[CreateAssetMenu(fileName = "NewPokemonData", menuName = "Pokemon/Create New Pokemon Data")]
public class PokemonData : ScriptableObject
{
    public string id;
    public string pokemonName;
    public Type type;
    public Sprite frontPortrait;
    public Sprite behindPortrait;
}
