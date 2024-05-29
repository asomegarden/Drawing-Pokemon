using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Pokemon
{
    public string id;
    public string name;
    public int level;
    public int maxHp;
    private int currentHp;
    public int CurrentHp { 
        get { return currentHp; } 
        set { currentHp = value; if (currentHp < 0) currentHp = 0; } 
    }
    public int power = 5;
    public Type type;
    public Sprite frontPortrait;
    public Sprite behindPortrait;

    public float GetDamageCoefficient(Type targetType)
    {
        var strongAgainst = new Dictionary<Type, Type[]>
        {
            { Type.Fire, new Type[] { Type.Grass, Type.Bug } },
            { Type.Water, new Type[] { Type.Fire, Type.Rock } },
            { Type.Grass, new Type[] { Type.Water, Type.Rock } },
            { Type.Flying, new Type[] { Type.Fighting, Type.Bug, Type.Grass} },
            { Type.Poison, new Type[] { Type.Grass } },
            { Type.Rock, new Type[] { Type.Flying, Type.Bug, Type.Fire } },
            { Type.Electric, new Type[] { Type.Flying, Type.Water } },
            { Type.Bug, new Type[] { Type.Grass } },
            { Type.Fighting, new Type[] { Type.Normal, Type.Rock } }
        };

        var weakAgainst = new Dictionary<Type, Type[]>
        {
            { Type.Fire, new Type[] { Type.Rock, Type.Fire, Type.Water } },
            { Type.Water, new Type[] { Type.Water, Type.Grass } },
            { Type.Grass, new Type[] { Type.Flying, Type.Bug, Type.Fire, Type.Grass } },
            { Type.Normal, new Type[] { Type.Rock } },
            { Type.Flying, new Type[] { Type.Rock, Type.Electric } },
            { Type.Poison, new Type[] { Type.Rock } },
            { Type.Rock, new Type[] { Type.Fighting } },
            { Type.Electric, new Type[] { Type.Rock, Type.Grass, Type.Electric } },
            { Type.Bug, new Type[] { Type.Fighting, Type.Flying, Type.Poison, Type.Fire } },
            { Type.Fighting, new Type[] { Type.Flying, Type.Poison, Type.Bug } }
        };

        if (strongAgainst.ContainsKey(type) && strongAgainst[type].Contains(targetType))
        {
            return 2f;
        }
        else if (weakAgainst.ContainsKey(type) && weakAgainst[type].Contains(targetType))
        {
            return 0.5f;
        }
        else
        {
            return 1f;
        }
    }

    public Pokemon(PokemonData data)
    {
        this.id = data.id;
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
        this.id = pokemon.id;
        this.name = pokemon.name;
        this.type = pokemon.type;
        this.frontPortrait = pokemon.frontPortrait;
        this.behindPortrait = pokemon.behindPortrait;
        this.level = pokemon.level;
        this.maxHp = pokemon.maxHp;
        this.currentHp = pokemon.CurrentHp;
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