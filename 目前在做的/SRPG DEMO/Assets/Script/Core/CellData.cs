using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CellType
{
    none,
    Terrain,
    StaticObject
}

public class CellData: IEquatable<CellData>
{

    public Vector3Int m_Position;
    public MapObject m_MapObject;
    public int moveCost;
    public int curCost = 0;
    public int G = 0;
    public int H = 0;
    public int F = 0;
    public CellData parent;
    public Vector3Int connectPoint;

    private CellType m_CellType;

    public bool hasMapObject
    {
        get { return m_MapObject != null; }
    }

    public MapObject mapObject
    {
        get { return m_MapObject; }
    }

    public CellData(Vector3Int pos)
    {
        m_Position = new Vector3Int(pos.x,pos.y,0);
    }

    public CellData(int x,int y)
    {
        m_Position = new Vector3Int(x, y, 0);
    }

    public void SetCellType(CellType cellType)
    {
        m_CellType = cellType;
    }

    public bool Equals(CellData celldata)
    {
        return celldata.m_Position.x == this.m_Position.x && celldata.m_Position.y == this.m_Position.y;
    }

    public override bool Equals(object obj)
    {
        return ((CellData)obj).Equals(this);
    }

}
