using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pokemon
{
    public string name;
    public int level;
    public int maxHp;
    public int currentHp;
    public int power = 5;
    public Type type;
    public Sprite frontPortrait;
    public Sprite behindPortrait;

    public Pokemon(PokemonData data)
    {
        this.name = data.name;
        this.type = data.type;
        this.frontPortrait = data.frontPortrait;
        this.behindPortrait = data.behindPortrait;
        this.level = 0;
        this.maxHp = 0;
        this.currentHp = 0;
        this.power = 0;
    }

    public Pokemon(Pokemon pokemon)
    {
        this.name = pokemon.name;
        this.type = pokemon.type;
        this.frontPortrait = pokemon.frontPortrait;
        this.behindPortrait = pokemon.behindPortrait;
        this.level = pokemon.level;
        this.maxHp = pokemon.maxHp;
        this.currentHp = pokemon.currentHp;
        this.power = pokemon.power;
    }

    public Pokemon() { }
}

public enum Type
{
    Fire,
    Water,
    Grass,
    Normal,
    Flying,
    Poison,
    Rock,
    Electric,
    Bug,
    Fighting
}