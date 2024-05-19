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
    public Type type;
    public List<Skill> skills;

    public Sprite portrait;
}

public enum Type
{
    Fire,
    Water,
    Grass,
    Normal
}