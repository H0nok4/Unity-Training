using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleTileUI : MonoBehaviour
{
    public GameObject TileUIPanel;
    public TMP_Text tileName;
    public TMP_Text tileDEF;
    public TMP_Text tileAVO;

    public void UpdateTileUI(SrpgTile tile)
    {

        if(tile != null)
        {
            if (TileUIPanel.activeSelf == false)
                TileUIPanel.SetActive(true);
            tileName.text = tile.terrainType.ToString();
            tileAVO.text = tile.avoidChange.ToString();
        }
        else
        {
            TileUIPanel.SetActive(false);
        }

    }
}
