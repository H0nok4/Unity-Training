using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeftController : MonoBehaviour
{
    Paimon paimon;
    public GameObject leftMoveButton;
    public GameObject rightMoveButton;
    public GameObject jumpButton;



    public void Start()
    {

        paimon = GetComponent<Paimon>();

    }

    public void HandleFixedUpdate()
    {
        
        //PC处理玩家移动
        if (Input.GetKey(KeyCode.D))
        {
            paimon.MoveHorizantal(1);
        }else if (Input.GetKey(KeyCode.A))
        {
            paimon.MoveHorizantal(-1);
        }

        //跳跃
        if (Input.GetKey(KeyCode.W))
        {
            paimon.Jump();
            
        }
        //拍击
        if (Input.GetKey(KeyCode.Space))
        {
            paimon.PowerSmash();
        }
        

        /*
        //手机处理玩家移动
        for(int i = 0; i < Input.touchCount; i++)
        {
            //遍历所有接触的点,看看有没有在左按钮
            var touch = Input.touches[i];
            var leftButtonScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main,leftMoveButton.transform.position);
            Vector2 leftButtonVec2Pos = new Vector2(leftButtonScreenPos.x,leftButtonScreenPos.y);
            Debug.Log("magnitude = " + (leftButtonVec2Pos - touch.position).magnitude);
            if ((leftButtonVec2Pos - touch.position).magnitude <= 100)
            {
                //点击了左键，向左走
                paimon.MoveHorizantal(-1);
            }
            //看看有没有在右按钮
            var rightButtonScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main,rightMoveButton.transform.position);
            Vector2 rightButtonVec2Pos = new Vector2(rightButtonScreenPos.x, rightButtonScreenPos.y);

            if((rightButtonVec2Pos - touch.position).magnitude <= 100)
            {
                paimon.MoveHorizantal(1);
            }
            //看看有没有在跳跃按钮
            var jumpButtonScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, jumpButton.transform.position);
            Vector2 jumpButtonVec2Pos = new Vector2(jumpButtonScreenPos.x, jumpButtonScreenPos.y);
            if((jumpButtonVec2Pos - touch.position).magnitude <= 100)
            {
                paimon.Jump();
            }

        }
        */

    }

}
