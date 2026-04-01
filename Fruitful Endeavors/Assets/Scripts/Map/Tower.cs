using System;
using UnityEngine;

[Serializable]
public class Tower
{
    public string name;
    public int cost;
    public GameObject prefab;
    public GameObject towerPreview;

    public Tower(string name, int cost, GameObject prefab, GameObject towerPreview)
    {
        this.name = name;
        this.cost = cost;
        this.prefab = prefab;
        this.towerPreview = towerPreview;
    }
}
