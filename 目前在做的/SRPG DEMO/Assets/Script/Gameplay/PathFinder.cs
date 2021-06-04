using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{
    #region 成员变量
    [SerializeField] Dictionary<Vector3Int,GameObject> moveCursors;
    [SerializeField] GameObject CursorObject;
    [SerializeField] GameObject mapObjectBase;
    private int[][] moveDelta = new int[4][] { new int[]{ 1, 0 }, new int[]{ -1, 0 }, new int[]{ 0, 1 }, new int[]{ 0, -1 } };

    #endregion
    public Dictionary<Vector3Int, GameObject> MoveCursors
    {
        get { return moveCursors; }
    }

    #region 方法

    #region 清除目前所有的移动光标
    //输入：无
    //效果：删除moveCursors中的光标物体
    //输出：无
    public void ClearMoveCursors()
    {
        if(moveCursors != null)
        {
            foreach (var kvp in moveCursors)
            {
                DestroyImmediate(kvp.Value.gameObject);
            }
            moveCursors.Clear();
            
        }


    }
    #endregion

    #region 创建AI移动范围
    //输入:需要移动的角色Unit
    //输出:创建好的移动范围
    public Dictionary<Vector3Int,int> CreatAIMoveRenge(SrpgClassUnit moveClass)
    {
        Dictionary<Vector3Int,int> aIMoveCursors = new Dictionary<Vector3Int, int>();
        Vector3Int startPos = moveClass.m_Position;
        int moveCost = moveClass.moveCost;
        List<Vector3Int> visited = new List<Vector3Int>();
        var startTile = MapManager.instance.tilemaps[1].GetTile<SrpgTile>(startPos);
        var startCellData = CreatTileData(startPos, MapManager.instance.GetSrpgTilemap(), moveClass.moveCost);
        aIMoveCursors.Add(new Vector3Int((int)startPos.x, (int)startPos.y, 0), 0);
        visited.Add(startPos);
        Queue<CellData> queue = new Queue<CellData>();
        queue.Enqueue(startCellData);
        while (queue.Count > 0)
        {

            //queue里还有数据的时候，继续遍历
            var curStartPosData = queue.Dequeue();
            foreach (var mD in moveDelta)
            {
                var newPosition = new Vector3Int(curStartPosData.m_Position.x + mD[0], curStartPosData.m_Position.y + mD[1], 0);
                if (!visited.Contains(newPosition))
                {
                    if (MapManager.instance.GetSrpgTilemapData(newPosition) != null)
                    {
                        var newCellData = CreatTileData(newPosition, MapManager.instance.GetSrpgTilemap(), curStartPosData.curCost - MapManager.instance.GetSrpgTilemapData(newPosition).moveCost);
                        if (newCellData.curCost >= 0)
                        {
                            aIMoveCursors.Add(new Vector3Int((int)newPosition.x, (int)newPosition.y, 0),0);
                            queue.Enqueue(newCellData);
                            visited.Add(newPosition);
                        }
                    }
                }
            }
        }

        //TO DO：清除moveCursor中被其他mapObject占用的格子
        DeleteAlreadyUseAIMoveCursors(moveClass.m_Position,aIMoveCursors);
        return aIMoveCursors;
    }
    #endregion

    #region 创建移动范围
    //输入：需要移动的角色，以角色为中心开始创建移动范围
    //输出：一个装有所有移动位置的字典，Key为坐标，Value为显示出来的Cursor
    public Dictionary<Vector3Int,GameObject> CreatMoveRenge(SrpgClassUnit moveClass)
    {
        /*
        moveCursors = new Dictionary<Vector3Int, GameObject>();
        Vector3Int startPos = moveClass.m_Position;
        int moveCost = moveClass.moveCost;
        List<Vector3Int> visited = new List<Vector3Int>();
        var startTile = MapManager.instance.tilemaps[1].GetTile<SrpgTile>(startPos);
        var startCellData = CreatTileData(startPos, MapManager.instance.GetSrpgTilemap(), moveClass.moveCost);
        var startCursorObject = Instantiate(CursorObject, mapObjectBase.transform, true);
        startCursorObject.transform.position = startPos;
        moveCursors.Add(new Vector3Int((int)startCursorObject.transform.position.x,(int)startCursorObject.transform.position.y,0),startCursorObject);
        visited.Add(startPos);
        Queue<CellData> queue = new Queue<CellData>();
        queue.Enqueue(startCellData);
        while(queue.Count > 0)
        {
            
            //queue里还有数据的时候，继续遍历
            var curStartPosData = queue.Dequeue();
            foreach (var mD in moveDelta)
            {
                var newPosition = new Vector3Int(curStartPosData.m_Position.x + mD[0], curStartPosData.m_Position.y + mD[1], 0);
                if (!visited.Contains(newPosition))
                {
                    if (MapManager.instance.GetSrpgTilemapData(newPosition) != null)
                    {
                        var newCellData = CreatTileData(newPosition, MapManager.instance.GetSrpgTilemap(), curStartPosData.curCost - MapManager.instance.GetSrpgTilemapData(newPosition).moveCost);
                        if (newCellData.curCost >= 0)
                        {
                            var newCursorObject = Instantiate(CursorObject, mapObjectBase.transform, true);
                            newCursorObject.transform.position = newPosition;
                            moveCursors.Add(new Vector3Int((int)newCursorObject.transform.position.x, (int)newCursorObject.transform.position.y, 0), newCursorObject);
                            queue.Enqueue(newCellData);
                            visited.Add(newPosition);
                        }
                    }
                }
            }
        }

        //TO DO：清除moveCursor中被其他mapObject占用的格子
        */
        moveCursors = new Dictionary<Vector3Int, GameObject>();
        Vector3Int startPos = moveClass.m_Position;
        int movePoint = moveClass.moveCost;
        var startTile = MapManager.instance.tilemaps[1].GetTile<SrpgTile>(startPos);
        var startCellData = CreatTileData(startPos, MapManager.instance.GetSrpgTilemap(), movePoint);
        var startCursorObject = Instantiate(CursorObject, mapObjectBase.transform, true);
        startCursorObject.transform.position = startPos;
        moveCursors.Add(new Vector3Int((int)startCursorObject.transform.position.x, (int)startCursorObject.transform.position.y, 0), startCursorObject);
        PriorityQueue<CellData> openList = new PriorityQueue<CellData>(new SortByF());
        List<CellData> closeList = new List<CellData>();
        openList.Enqueue(startCellData);
        while(openList.Count > 0)
        {
            var curCellData = openList.Dequeue();
            foreach(var mD in moveDelta)
            {
                var newPosition = new Vector3Int(curCellData.m_Position.x + mD[0], curCellData.m_Position.y + mD[1], 0);
                if (MapManager.instance.GetSrpgTilemapData(newPosition) != null)
                {

                    var newCellData = CreatTileData(newPosition, MapManager.instance.GetSrpgTilemap(),curCellData.curCost,curCellData);

                    if (!IsContainsCellData(openList, newCellData) && !IsContainsCellData(closeList, newCellData) && newCellData.curCost >= 0)
                    {
                        if (newCellData.curCost >= 0 && !moveCursors.ContainsKey(newPosition))
                        {
                            var newCursorObject = Instantiate(CursorObject, mapObjectBase.transform, true);
                            newCursorObject.transform.position = newPosition;
                            moveCursors.Add(new Vector3Int((int)newCursorObject.transform.position.x, (int)newCursorObject.transform.position.y, 0), newCursorObject);

                        }
                        openList.Enqueue(newCellData);
                    }
                }
            }
        }

        return moveCursors;
    }
    #endregion

    #region 删除被占用的移动光标
    public void DeleteAlreadyUseMoveCursors(Vector3Int curPosition)
    {
        if(ScenceManager.instance.mapObjectGameObjects != null)
        {
            List<Vector3Int> waitForDelete = new List<Vector3Int>();
            foreach(var moveCursor in moveCursors)
            {
                if (ScenceManager.instance.mapObjectGameObjects.ContainsKey(moveCursor.Key) && moveCursor.Key != curPosition )
                {
                    if(ScenceManager.instance.mapObjectGameObjects[moveCursor.Key].activeSelf != false)
                    {
                        Destroy(moveCursor.Value);
                        waitForDelete.Add(moveCursor.Key);
                    }

                }
            }

            foreach(var vec in waitForDelete)
            {
                moveCursors.Remove(vec);
            }
        }
    }
    #endregion

    #region 删除AI的移动范围
    //输入，删除AI角色的移动范围
    private void DeleteAlreadyUseAIMoveCursors(Vector3Int curPosition,Dictionary<Vector3Int,int> aiMoveCursor)
    {
        if (ScenceManager.instance.mapObjectGameObjects != null)
        {
            List<Vector3Int> waitForDelete = new List<Vector3Int>();
            foreach (var moveCursor in aiMoveCursor)
            {
                if (ScenceManager.instance.mapObjectGameObjects.ContainsKey(moveCursor.Key) && moveCursor.Key != curPosition)
                {
                    waitForDelete.Add(moveCursor.Key);
                }
            }

            foreach (var vec in waitForDelete)
            {
                aiMoveCursor.Remove(vec);
            }
        }
    }
    #endregion

    #region 创建移动范围的TileData
    
    private CellData CreatTileData(Vector3Int startPos, Tilemap srpgTile,int m_CurCost,CellData parent = null)
    {
        var cellData = new CellData(startPos);
        var posTile = srpgTile.GetTile<SrpgTile>(startPos);
        cellData.G = posTile.moveCost;
        if (parent != null)
        {
            cellData.parent = parent;
            cellData.G = (parent.G + Distance(parent.m_Position, startPos)) * posTile.moveCost;
            cellData.curCost = parent.curCost - posTile.moveCost;
        }
        else
        {
            cellData.curCost = m_CurCost;
        }
        cellData.F = cellData.G;
        return cellData;
    }
    #endregion

    #region 创建A星寻路的TileData
    //输入:起始位置，目标位置，TileMap用于获取SRPGTile信息，Parent为Null时是第一次创建，后面的则不是null
    //输出:创建好的CellData
    private CellData CreatAstarTileData(Vector3Int startPos,Vector3Int targetPos,Tilemap srpgTile,CellData parent = null)
    {

        var cellData = new CellData(startPos);
        var posTile = srpgTile.GetTile<SrpgTile>(startPos);
        cellData.G = posTile.moveCost;
        var startTeleporter = ScenceManager.instance.TryGetNearestTeleporter(startPos);
        var endTeleporter = ScenceManager.instance.TryGetNearestTeleporter(targetPos);
        if (startTeleporter != null && endTeleporter != null)
        {
            //如果场上存在传送器，看看能不能抄近道
            //D(x,y)为x点到y点的距离，T(x)为离x最近的传送器的坐标
            //H(x,y) = min(D(x,y), D(x,T(x)) + D(T(y),y) )
            cellData.H = Mathf.Min(Distance(startPos, targetPos), Distance(startPos, startTeleporter.pos) + Distance(endTeleporter.pos, targetPos));
        }
        else
        {
            //没有传送器，按部就班寻路
            cellData.H = Distance(startPos, targetPos);
        }


        if (parent != null)
        {
            cellData.parent = parent;
            cellData.G = parent.G + posTile.moveCost;
        }
        cellData.F = cellData.G + cellData.H;
        return cellData;
    }
    #endregion

    #region A星寻路
    //输入：开始点和结束点
    //效果：通过A星算法算出俩点间的最短路径
    //输出：创建好的最短路径链表
    public List<CellData> AstarCreatMovePath(Vector3Int startPos, Vector3Int targetPos)
    {
        if (startPos == targetPos)
            return new List<CellData>();
        PriorityQueue<CellData> openList = new PriorityQueue<CellData>(new SortByF());
        HashSet<CellData> closeList = new HashSet<CellData>();
        
        openList.Enqueue(CreatAstarTileData(startPos, targetPos, MapManager.instance.tilemaps[1]));


        while (openList.Count > 0)
        {

            var curCellData = openList.Dequeue();

            closeList.Add(curCellData);

            foreach(var mD in moveDelta)
            {
                var newPostion = new Vector3Int(curCellData.m_Position.x + mD[0], curCellData.m_Position.y + mD[1], 0);
                if(MapManager.instance.GetSrpgTilemapData(newPostion) != null)
                {
                    
                    var newCellData = CreatAstarTileData(newPostion, targetPos, MapManager.instance.GetSrpgTilemap(), curCellData);
                    if (newCellData.m_Position == targetPos)
                    {
                        return CreatAstarResultPath(newCellData);//A*寻路的找到结果后需要从最终的结果点回溯parent来构建路径，将这部分的逻辑分离到CreatAstarResultPath函数中
                    }


                    if (!IsContainsCellData(openList, newCellData) && !closeList.Contains(newCellData))
                    {
                        openList.Enqueue(newCellData);
                    }
                }
            }
            //抄传送门近道
            if (ScenceManager.instance.interactiveObjectGameObjects.ContainsKey(curCellData.m_Position))
            {
                var teleporter = ScenceManager.instance.interactiveObjectGameObjects[curCellData.m_Position].gameObject.GetComponent<Teleporter>();
                if (teleporter != null)
                {
                    var newCellData = CreatAstarTileData(teleporter.targetPos, targetPos, MapManager.instance.GetSrpgTilemap(), curCellData);
                    if (newCellData.m_Position == targetPos)
                    {
                        return CreatAstarResultPath(newCellData);//A*寻路的找到结果后需要从最终的结果点回溯parent来构建路径，将这部分的逻辑分离到CreatAstarResultPath函数中
                    }
                    if (!IsContainsCellData(openList, newCellData) && !closeList.Contains(newCellData) && !ScenceManager.instance.mapObjectGameObjects.ContainsKey(newCellData.m_Position))
                    {
                        openList.Enqueue(newCellData);
                    }
                }
            }
        }

        return null;
    }
    #endregion

    #region 创建A星寻路最终路径
    //输入：目标节点信息
    //效果：从目标节点回溯到起点，创建路径
    //输出：创建好的A星寻路的最终路径
    private List<CellData> CreatAstarResultPath(CellData resultData)
    {
        List<CellData> aStarResultCellData = new List<CellData>();
        var targetCellData = resultData;
        while (targetCellData.parent != null)
        {
            var resultCellData = new CellData(targetCellData.m_Position);
            aStarResultCellData.Add(resultCellData);
            targetCellData = targetCellData.parent;
        }
        aStarResultCellData.Reverse();//因为是从目标点开始创建路径，所以需要翻转一下
        return aStarResultCellData;
    }
    #endregion

    #region 判断列表中是否包含重复节点
    private bool IsContainsCellData(List<CellData> cellDatas,CellData m_cellData)
    {
        foreach(var cD in cellDatas)
        {
            if(cD.m_Position.x == m_cellData.m_Position.x && cD.m_Position.y == m_cellData.m_Position.y)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsContainsCellData(PriorityQueue<CellData> cellDatas, CellData m_cellData)
    {

        return cellDatas.Contains(m_cellData);
    }

    #endregion

    #region 曼哈顿距离
    //输入：开始与结束点
    //效果：计算俩点的距离
    //输出：距离
    private int Distance(Vector3Int x,Vector3Int y)
    {
        return Mathf.Abs(x.x - y.x) + Mathf.Abs(x.y - y.y);
    }
    #endregion

    #region dijkstra算法
    public SrpgClassUnit DijsktraFindPlayerClass(Vector3Int startPos)
    {

        PriorityQueue<CellData> openList = new PriorityQueue<CellData>(new SortByF());
        List<CellData> closeList = new List<CellData>();

        openList.Enqueue(CreatDijkstraTileData(startPos,MapManager.instance.tilemaps[1]));


        while (openList.Count > 0)
        {

            var curCellData = openList.Dequeue();
            closeList.Add(curCellData);

            foreach (var mD in moveDelta)
            {
                var newPostion = new Vector3Int(curCellData.m_Position.x + mD[0], curCellData.m_Position.y + mD[1], 0);
                if (MapManager.instance.GetSrpgTilemapData(newPostion) != null)
                {

                    var newCellData = CreatDijkstraTileData(newPostion,MapManager.instance.GetSrpgTilemap(), curCellData);
                    if (ScenceManager.instance.mapObjectGameObjects.ContainsKey(newCellData.m_Position))
                    {
                        var srpgClass = ScenceManager.instance.mapObjectGameObjects[newCellData.m_Position].GetComponent<SrpgClassUnit>();
                        if(srpgClass != null)
                        {
                            if(srpgClass.classCamp == ClassCamp.ally || srpgClass.classCamp == ClassCamp.player)
                            {
                                return srpgClass;
                            }
                        }
                    }



                    if (!IsContainsCellData(openList, newCellData) && !IsContainsCellData(closeList, newCellData))
                    {
                        openList.Enqueue(newCellData);
                    }
                }

            }
            //抄传送门近道
            if (ScenceManager.instance.interactiveObjectGameObjects.ContainsKey(curCellData.m_Position))
            {
                var teleporter = ScenceManager.instance.interactiveObjectGameObjects[curCellData.m_Position].gameObject.GetComponent<Teleporter>();
                if (teleporter != null)
                {
                    var newCellData = CreatDijkstraTileData(teleporter.targetPos, MapManager.instance.GetSrpgTilemap(), curCellData);
                    if (ScenceManager.instance.mapObjectGameObjects.ContainsKey(newCellData.m_Position))
                    {
                        var srpgClass = ScenceManager.instance.mapObjectGameObjects[newCellData.m_Position].GetComponent<SrpgClassUnit>();
                        if (srpgClass != null)
                        {
                            if (srpgClass.classCamp == ClassCamp.ally || srpgClass.classCamp == ClassCamp.player)
                            {
                                return srpgClass;
                            }
                        }
                    }
                    if (!IsContainsCellData(openList, newCellData) && !IsContainsCellData(closeList, newCellData))
                    {
                        openList.Enqueue(newCellData);
                    }
                }
            }
        }

        return null;
    }

    private CellData CreatDijkstraTileData(Vector3Int startPos, Tilemap srpgTile, CellData parent = null)
    {
        var cellData = new CellData(startPos);
        var posTile = srpgTile.GetTile<SrpgTile>(startPos);
        cellData.G = posTile.moveCost;
        cellData.H = 0;


        if (parent != null)
        {
            cellData.parent = parent;
            cellData.G = (parent.G + Distance(parent.m_Position, startPos)) * posTile.moveCost;
        }
        cellData.F = cellData.G + cellData.H;
        return cellData;
    }
    #endregion

    #endregion

}

public class SortByF : IComparer<CellData>
{
    public int Compare(CellData x, CellData y)
    {
        return y.F.CompareTo(x.F);
    }
}

public class SortByG : IComparer<CellData>
{
    public int Compare(CellData x, CellData y)
    {
        return x.G.CompareTo(y.G);
    }


}