using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public enum TerrainType
{
    Plain,
    Road,
    Woods,
    Decoration,
    Barrier
}
[Serializable]
[CreateAssetMenu(fileName = "New Srpg Tile.asset",menuName = "SRPG/Tile")]
public class SrpgTile : Tile
{
    [SerializeField] TerrainType m_TerrainType = TerrainType.Plain;
    [SerializeField] int m_AvoidChange = 0;
    [SerializeField] int m_MoveCost = 1;
    [SerializeField] bool m_Walkable;
    public TerrainType terrainType
    {
        get { return m_TerrainType; }
        set { m_TerrainType = value; }
    }
    public int avoidChange
    {
        get { return m_AvoidChange; }
        set { m_AvoidChange = value; }
    }

    public int moveCost

    {
        get { return m_MoveCost; }
        set { m_MoveCost = value; }
    }

    public bool walkable
    {
        get { return m_Walkable; }
    }


}
