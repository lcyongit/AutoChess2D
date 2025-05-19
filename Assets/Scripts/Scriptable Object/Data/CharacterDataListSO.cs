using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataListSO", menuName = "Scriptable Objects/Data/CharacterDataListSO")]
public class CharacterDataListSO : ScriptableObject
{
    // ®§¶‚πœ≈≤ 
    public List<CharacterDetails> characterDetailsList;

    public CharacterDetails GetCharacterDetails(CharacterName characterName)
    {
        return characterDetailsList.Find(x => x.name == characterName);

    }
}


[Serializable]
public class CharacterDetails
{
    [Header("Character Name")]
    public CharacterName name;

    [Header("Character Prefab")]
    public GameObject prefab;

    [Header("Sprite")]
    public Sprite sprite;

    [Header("Stats")]
    public float maxHp;
    public float maxSkillGauge;

    public float moveSpeed;

    [Header("Attack 01")]
    public float attackDamage01;
    public float attackInterval01;
    public float attackRange01;

    [Header("Attack 02")]
    public float attackDamage02;
    public float attackInterval02;
    public float attackRange02;

    [Header("Attack 03")]
    public float attackDamage03;
    public float attackInterval03;
    public float attackRange03;

    [Header("Defense")]
    public float defensePower;


}
