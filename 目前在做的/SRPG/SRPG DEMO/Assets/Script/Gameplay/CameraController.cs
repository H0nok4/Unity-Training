using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera m_Camera;
    [SerializeField] int panSpeed = 5;

    [SerializeField] int[] clampX = new int[2];
    [SerializeField] int[] clampY = new int[2];
    [SerializeField] Vector3 moveInput;
    private void Start()
    {

    }
    private void Update()
    {
        if (PlayerInputManager.instance.GetMouseWheelDown())
        {
            m_Camera.orthographicSize -= 20 * Time.deltaTime;
            m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize, 4f, 9f);
        }else if (PlayerInputManager.instance.GetMouseWheelUp())
        {
            m_Camera.orthographicSize += 20 * Time.deltaTime;
            m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize, 4f, 9f);
        }
        MouseControlCameraMove();
        LimitCamera();
        
    }

    private void MouseControlCameraMove()
    {
        if (GameManager.instance.gameState == GameState.PlayScenario || (BattleManager.instance.battleState != BattleStat.PlayerCharacterSelect && BattleManager.instance.battleState != BattleStat.EnemyTurn))
            return;

        Vector3 pos = m_Camera.transform.position;

        Vector3 mousePos = Input.mousePosition;

        moveInput = Vector3.zero;

        if(mousePos.x > Screen.width * 0.8f && mousePos.x < Screen.width)
        {
            moveInput.x = 1;
        }
        if (mousePos.x < Screen.width * 0.2f && mousePos.x > 0)
        {
            moveInput.x = -1;
        }
        if (mousePos.y > Screen.height * 0.8f && mousePos.y < Screen.height)
        {
            moveInput.y = 1;
        }
        if (mousePos.y < Screen.height * 0.2f && mousePos.x > 0)
        {
            moveInput.y = -1;
        }

        
        pos += moveInput.normalized * panSpeed * Time.deltaTime;
        m_Camera.transform.position = new Vector3(pos.x, pos.y, -10);
    }

    public void LimitCamera()
    {

        Vector2 StartPos = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));//左下
        Vector2 EndPos = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));//右上

        if (StartPos.x < clampX[0])
        {
            var offsetX = clampX[0] - StartPos.x;
            m_Camera.transform.position = new Vector3(m_Camera.transform.position.x + offsetX, m_Camera.transform.position.y, -10);
        }
        if (StartPos.y < clampY[0])
        {
            var offsetY = clampY[0] - StartPos.y;
            m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y + offsetY, -10);
        }
        if (EndPos.x > clampX[1])
        {
            var offsetX = EndPos.x - clampX[1];
            m_Camera.transform.position = new Vector3(m_Camera.transform.position.x - offsetX, m_Camera.transform.position.y, -10);
        }
        if (EndPos.y > clampY[1])
        {
            var offsetY = EndPos.y - clampY[1];
            m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y - offsetY, -10);
        }



    }
}
