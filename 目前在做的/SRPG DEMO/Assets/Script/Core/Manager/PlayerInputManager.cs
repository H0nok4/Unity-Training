using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
        }
    }

    public bool ClickAnyButton()
    {
        if (CanProcessInput())
        {
            if (Input.anyKeyDown)
            {
                return true;
            }
        }
        return false;
    }
    public bool CanProcessInput()
    {
        return !GameManager.instance.isGameStop;
    }

    public Vector3Int GetMouseInTilemapPosition(Tilemap tilemap)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3Int vector = tilemap.WorldToCell(ray.GetPoint(-ray.origin.z / ray.direction.z));
        return vector;
    }

    public int GetArrowKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return 1;
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return 2;
        }else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return 3;
        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return 4;
        }

        return 0;
    }

    public bool GetEscapeButton()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    public bool GetMouseWheelDown()
    {
        if (CanProcessInput() && GameManager.instance.gameState != GameState.PlayScenario)
        {
            return Input.GetAxis("Mouse ScrollWheel") < 0;
        }

        return false;
    }

    public bool GetMouseWheelUp()
    {
        if (CanProcessInput() && GameManager.instance.gameState != GameState.PlayScenario)
        {
            return Input.GetAxis("Mouse ScrollWheel") > 0;
        }

        return false;
    }

    public bool GetBagButton()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.B);
        }

        return false;
    }

    public bool GetPlayerLeftMouseKeyDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.Mouse0);
        }

        return false;
    }

    public bool GetPlayerRightMouseKeyDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.Mouse1);
        }

        return false;
    }
}
