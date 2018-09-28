using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pool Type")]
public class PoolType : ScriptableObject
{
    public AllPoolTypes type;
    public string groupName;
    public GameObject prefab;
    public int amount;
}
