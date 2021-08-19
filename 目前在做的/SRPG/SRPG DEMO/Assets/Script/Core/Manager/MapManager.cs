using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] public Tilemap[] tilemaps;
    public GameObject playerCursor;
    public Vector3Int CursorPosition;
    public GameObject playerCursorPrefab;
    public ScenceManager scenceManager;
    public MapObject playerCursorMapObject;
    public PlayerInputManager playerInputManager;
    public PathFinder pathFinder;

    private void Start()
    {
        scenceManager = ScenceManager.instance;
        playerInputManager = GameObject.Find("BattleManager").GetComponent<PlayerInputManager>();
        pathFinder = GameObject.Find("BattleManager").GetComponent<PathFinder>();
    }

    public static MapManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }

    }

    public void HandleUpdate()
    {
        SetPlayerCursorPositionToMousePosition();

        CheckMapObjectOnPlayerCursorPosition();
    }

    #region 获得鼠标光标位置的SrpgTile
    public SrpgTile GetCursorPositionSrpgTile()
    {
        if(playerCursor != null)
        {
            return tilemaps[1].GetTile<SrpgTile>(new Vector3Int((int)playerCursor.transform.position.x, (int)playerCursor.transform.position.y, 0));
        }

        return null;
    }
    #endregion

    #region 获得鼠标光标的位置
    public Vector3Int GetCursorPosition()
    {
        if (playerCursor != null)
        {
            return new Vector3Int((int)playerCursor.transform.position.x, (int)playerCursor.transform.position.y, 0);
        }
        return new Vector3Int(int.MinValue, int.MinValue, 0);

    }
    #endregion

    #region 设置玩家鼠标光标的位置
    public void SetPlayerCursorPositionToMousePosition()
    {
        Vector3Int mousePosition = playerInputManager.GetMouseInTilemapPosition(tilemaps[1]);
        var mouseTile = tilemaps[1].GetTile<SrpgTile>(mousePosition);
        if (mouseTile != null)
        {
            if(playerCursor != null)
            {
                CursorPosition = mousePosition;
                playerCursor.transform.position = CursorPosition;
            }
            else
            {
                var Cursor = GameObject.FindGameObjectWithTag("playerCursor");
                if(Cursor == null)
                {
                    playerCursor = Instantiate(playerCursorPrefab);
                    CursorPosition = mousePosition;
                    playerCursor.transform.position = CursorPosition;
                }
                else
                {
                    playerCursor = Cursor;
                    CursorPosition = mousePosition;
                    playerCursor.transform.position = CursorPosition;
                }
            }
        }
        else
        {
            if(playerCursor != null)
            {
                Destroy(playerCursor);
            }
            playerCursorMapObject = null;
        }
    }
    #endregion

    #region 获得鼠标光标位置的MapObject信息
    public MapObject CheckMapObjectOnPlayerCursorPosition()
    {

        if (ScenceManager.instance.mapObjectGameObjects.ContainsKey(GetCursorPosition()))
        {
            return ScenceManager.instance.mapObjectGameObjects[GetCursorPosition()].GetComponent<MapObject>();
        }

        playerCursorMapObject = null;
        return null;
    }
    #endregion

    #region 获得目标位置的SrpgTile
    public SrpgTile GetSrpgTilemapData(Vector3Int tilePos)
    {
        if(tilemaps[1].GetTile<SrpgTile>(tilePos) != null)
        {
            return tilemaps[1].GetTile<SrpgTile>(tilePos);
        }

        return null;
    }
    #endregion

    public Tilemap GetSrpgTilemap()
    {
        return tilemaps[1];
    }
}
 