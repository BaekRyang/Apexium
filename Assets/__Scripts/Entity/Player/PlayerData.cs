using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("Name")]
    public string characterName;

    [Space] [Header("Stats")]
    public PlayerStats stats;

    [Space] [Header("Skills")]
    public string skillPassive;
    public string skillPrimary;
    public string skillSecondary;
    public string skillUtility;
    public string skillUltimate; 
    
    [Space] [Header("Sprites")]
    public Sprite sprite;
    public AnimatorController animatorController;
}
    