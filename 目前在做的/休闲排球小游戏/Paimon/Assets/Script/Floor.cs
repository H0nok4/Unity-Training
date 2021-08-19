using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Floor : MonoBehaviour
{
    //地板类
    //用来判断派蒙是否处于跳跃状态，和球落地的判断
    public bool isPlayer2P;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Paimon"))
        {
            collision.GetComponent<Paimon>().isJumping = false;
        }else if (collision.CompareTag("Ball"))
        {
            if (isPlayer2P)
            {
                //球落到2P的地面上，1P加分
                GameManager.instance.PlayerGetScore(true);
            }
            else
            {
                //球落到1P的地面上，2P加分
                GameManager.instance.PlayerGetScore(false);
            }
        }
    }

}
