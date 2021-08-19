using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity
{
    public float x;
    public float y;
    public Velocity()
    {
        x = 0;
        y = 0;
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        this.x = newVelocity.x;
        this.y = newVelocity.y;
    }

}

public class Paimon : MonoBehaviour
{
    //派蒙动画
    PaimonAnimator paimonAnimator;
    //物理相关
    public Rigidbody2D paimonRigbody2D;

    [SerializeField] int moveSpeed = 4;
    [SerializeField] int jumpVelocity = 500;
    //位置相关，用于计算Velocity
    Vector3 prePos;
    Vector3 curPos;
    public Velocity velocity;

    float preSmashTime;

    public bool isJumping;
    public bool isSmash;

    //左边的派蒙为0，右边的派蒙为1
    public int isLeftPaimon;

    public bool isComputer;

    public int computerBoldness;
    //用来控制电脑站位，1代表站中间，0代表站靠近网的地方
    public int computerShouldStandPos;

    private void Start()
    {
        paimonAnimator = GetComponent<PaimonAnimator>();
        paimonRigbody2D = GetComponent<Rigidbody2D>();
        curPos = this.transform.position;
        velocity = new Velocity();
    }

    public void HandleUpdate()
    {
        //更新位置，加速度
        paimonAnimator.isJumping = isJumping;
        prePos = curPos;
        curPos = transform.position;
        float deltaX = curPos.x - prePos.x;
        velocity.x = deltaX;
        float deltaY = curPos.y - prePos.y;
        velocity.y = deltaY;

        if(Time.time - preSmashTime > 0.25f)
        {
            isSmash = false;
        }

        paimonAnimator.isSmashing = isSmash;

    }

    public void MoveHorizantal(int horizantal)
    {
        //水平移动
        if(horizantal == 1)
        {
            transform.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime);
        }else if(horizantal == -1)
        {
            transform.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime);
        }
    }
    public void Jump()
    {
        //跳跃
        if (!isJumping)
        {
            paimonRigbody2D.velocity = new Vector3(paimonRigbody2D.velocity.x, jumpVelocity * Time.deltaTime);
            isJumping = true;
        }
        else
        {
            PowerSmash();
        }
    }

    public void PowerSmash()
    {
        //击球
        if(Time.time - (preSmashTime + 0.5f) > 0 && isJumping)
        {
            preSmashTime = Time.time;
            isSmash = true;
        }

    }
}
