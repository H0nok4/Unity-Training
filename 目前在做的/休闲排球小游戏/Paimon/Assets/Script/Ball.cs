using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //public Rigidbody2D ballRig;
    public Velocity velocity;

    public float curSecond;
    public Vector3 prePos;
    public Vector3 prePrePos;
    public Vector3 curPos;
    public float preTriggerTime;
    public int preTriggerPaimon;//0为左，1为右派蒙


    public float guessLandPosX;//球将会在哪个点落地

    [SerializeField] GameObject ghostBall1;
    [SerializeField] GameObject ghostBall2;

    public bool isPowerHit;
    private void Start()
    {
        //ballRig = GetComponent<Rigidbody2D>();
        velocity = new Velocity();
    }

    public void HandleUpdate()
    {
        //大力击球模式的俩个贴图更新位置
        if(Time.time - curSecond > 0.05f)
        {
            curSecond = Time.time;
            prePrePos = prePos;
            prePos = curPos;
            curPos = transform.position;

        }
        //如果是击球状态，显示后面的虚影
        if (isPowerHit)
        {
            GhostBall();
        }
        else
        {
            //反之隐藏起来
            HideGhostBall();
        }

        velocity.y -= 9.8f * Time.deltaTime;

        //模仿移动
        transform.position = new Vector3(transform.position.x + (velocity.x * Time.deltaTime), transform.position.y + (velocity.y * Time.deltaTime));


    }

    public void HideGhostBall()
    {
        //隐藏虚影
        if (ghostBall1.activeSelf)
        {
            ghostBall1.SetActive(false);
            ghostBall2.SetActive(false);
        }
    }

    public void GhostBall()
    {

        //显示虚影
        if (!ghostBall1.activeSelf)
        {
            ghostBall1.SetActive(true);
            ghostBall2.SetActive(true);
        }
        ghostBall1.transform.position = prePos;
        ghostBall2.transform.position = prePrePos;
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //onTriggerEnter效果不理想，还是需要在Update里面自己写碰撞检测
        
        if (collision.CompareTag("Floor"))
        {
            //接触地板
            //向上弹起
            physics.ProccesBallCollsionWithFloor(this,collision.transform);
            Debug.Log("Toch Floor");
        }
        if (collision.CompareTag("Wall"))
        {
            physics.ProccesBallCollsionWithWall(this, collision.transform);
        }
        if (collision.CompareTag("Ceiling"))
        {
            physics.ProccesBallCollsionWithCelling(this, collision.transform);
        }
        if (collision.CompareTag("column"))
        {
            physics.ProccesBallCollsionWithColumn(this, collision.transform);
        }
        if (collision.CompareTag("PaimonBody"))
        {
            physics.ProccesBallCollsionWithPaimonBody(this,collision.GetComponentInParent<Paimon>());
        }
            
    }
    */
}
