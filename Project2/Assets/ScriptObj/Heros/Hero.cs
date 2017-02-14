using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Hero : ScriptableObject
{

    public string emname;
    public int health, atk, def, agi;
    public float atkTime;
    public bool isMagic;
    public int manaPool;
    public Sprite mySprite;
}

