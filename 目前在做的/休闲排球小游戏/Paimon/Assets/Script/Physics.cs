using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics : MonoBehaviour
{
    public Ball ball;
    [SerializeField] Paimon leftPaimon;
    [SerializeField] Paimon rightPaimon;


    //球的位置
    float ballLeftMaxPos;
    float ballTopPos;
    float ballRightMaxPos;
    float ballBottomPos;
    float ballCenterX;
    float ballCenterY;

    //墙壁
    const float leftWallPos = -5.3f;
    const float rightWallPos = 5.3f;
    const float ceilingYPos = 3.5f;
    const float floorYpos = -2.19f;
    //柱子
    const float columnLeftPos = -0.05f;
    const float columRightPos = 0.065f;
    const float columTopPos = -1f;
    const float columCenterXPos = 0;
    const float columCenterYpos = -1.67f;
    const float columWidth = 0.1f;
    const float columHighth = 0.6f;
    //派蒙属性
    const float paimonHalfWidth = 0.2f;
    const float paimonHalfHighth = 0.6f;
    //球的半径
    const float ballRadius = 0.35f;
    //用来控制猜测球的最大循环量，防止无限循环
    const int loopMax = 10000;

    public void HandleUpdate()
    {
        ProccesBallCollisionWithWorld();

        if (ball.transform.position.x < columRightPos)
        {
            ProccesBallCollsionWithPaimonBody(ball, leftPaimon);
        }
        else
        {
            ProccesBallCollsionWithPaimonBody(ball, rightPaimon);
        }



    }

    public void HandleFixedUpdate()
    {
        //定时更新
        GuessBallWillLandWhere(ball);
        if (leftPaimon.isComputer)
        {
            var input = ComputerDecideControl(leftPaimon, rightPaimon, ball);

            leftPaimon.MoveHorizantal(input.xInput);
            if (input.jumpInput == 1)
            {
                leftPaimon.Jump();
            }

            if (input.powerHit == 1)
            {
                leftPaimon.PowerSmash();
            }
        }

        if (rightPaimon.isComputer)
        {
            var input = ComputerDecideControl(rightPaimon, leftPaimon, ball);

            rightPaimon.MoveHorizantal(input.xInput);
            if (input.jumpInput == 1)
            {
                rightPaimon.Jump();
            }

            if (input.powerHit == 1)
            {
                rightPaimon.PowerSmash();
            }
        }
    }


    private void ProccesBallCollisionWithWorld()
    {
        //更新球坐标和各个方向的坐标
        ballLeftMaxPos = ball.transform.position.x - ballRadius;
        ballRightMaxPos = ball.transform.position.x + ballRadius;
        ballTopPos = ball.transform.position.y + ballRadius;
        ballBottomPos = ball.transform.position.y - ballRadius;
        ballCenterX = ball.transform.position.x;
        ballCenterY = ball.transform.position.y;
        //处理与墙壁碰撞
        if (ballLeftMaxPos < leftWallPos)
        {
            //与左墙壁碰撞,球向右移动
            float ballXVelocity = Mathf.Abs(ball.velocity.x);
            ball.velocity.SetVelocity(new Vector3(ballXVelocity, ball.velocity.y));

        }
        if (ballRightMaxPos > rightWallPos)
        {
            //与右墙壁碰撞,球向左移动
            float ballXVelocity = -Mathf.Abs(ball.velocity.x);
            ball.velocity.SetVelocity(new Vector3(ballXVelocity, ball.velocity.y));
        }

        //处理与地板碰撞
        if (ballBottomPos < floorYpos)
        {
            //球向上移动
            float ballYVelocity = Mathf.Abs(ball.velocity.y);
            ball.velocity.SetVelocity(new Vector3(ball.velocity.x, ballYVelocity));
            if (ballCenterX < columCenterXPos)
            {
                //球在1P位置落地，2P得分
                GameManager.instance.PlayerGetScore(false);
            }
            else
            {
                //球在2P位置落地，1P得分
                GameManager.instance.PlayerGetScore(true);

            }
        }

        //处理与天花板碰撞
        if (ballTopPos > ceilingYPos)
        {
            //球向下运动
            float ballYVelocity = -Mathf.Abs(ball.velocity.y);
            if (ballYVelocity >= 10)
            {
                ballYVelocity = 10;
            }
            ball.velocity.SetVelocity(new Vector3(ball.velocity.x, ballYVelocity));
        }

        //处理与中央柱子的碰撞

        bool isBallCollisionWithColumn = IsBallCollisionWithColumn(ball);
        if (isBallCollisionWithColumn)
        {
            if ((ballBottomPos < columTopPos && ballCenterY > columTopPos))
            {
                //与柱子上部碰撞，方向向上
                float ballYVelocity = Mathf.Abs(ball.velocity.y);
                ball.velocity.SetVelocity(new Vector3(ball.velocity.x, ballYVelocity));
            }
            else
            {
                if (ballCenterX < columCenterXPos)
                {
                    //在右边碰撞，方向向右
                    float ballXVelocity = -Mathf.Abs(ball.velocity.x);
                    ball.velocity.SetVelocity(new Vector3(ballXVelocity, ball.velocity.y));
                }
                else
                {
                    //在左边碰撞，方向向左
                    float ballXVelocity = Mathf.Abs(ball.velocity.x);
                    ball.velocity.SetVelocity(new Vector3(ballXVelocity, ball.velocity.y));
                }
            }
        }
    }



    public void ProccesBallCollsionWithPaimonBody(Ball ball, Paimon paimon)
    {
        if(Time.time - ball.preTriggerTime >= 0.3f)
        {
            //处理球和派蒙碰撞]
            bool hadCollision = IsPaimonCollisionWithBall(ball, paimon);
            if (hadCollision)
            {
                //确定Y方向动量,向上，动量不足则补足
                float ballYVelocity = 7f - (ballCenterY * 2);
                Debug.Log($"Yv = {ballYVelocity}");

                //确定碰撞，判断X方向动量
                if (!paimon.isSmash)
                {
                    if (ball.transform.position.x < paimon.transform.position.x)
                    {
                        //碰撞在左边，球方向为左
                        float ballXVelocity = -((Mathf.Abs(ball.transform.position.x - paimon.transform.position.x) * 12) + (Mathf.Abs(paimon.velocity.x)));//-Mathf.Abs(ball.ballRig.velocity.x) - (Mathf.Abs(paimon.paimonRigbody2D.velocity.x) * 2);
                        ball.velocity.SetVelocity(new Vector3(ballXVelocity, ballYVelocity));
                    }
                    else if (ball.transform.position.x > paimon.transform.position.x)
                    {
                        //碰撞在右边，球方向为右
                        float ballXvelocity = ((Mathf.Abs(ball.transform.position.x - paimon.transform.position.x) * 12) + (Mathf.Abs(paimon.velocity.x)));
                        ball.velocity.SetVelocity(new Vector3(ballXvelocity, ballYVelocity));
                    }
                    else
                    {
                        //碰撞为正中间，随机选择一个方向
                        int randomDir = Random.Range(-1, 2);
                        float ballXVelocity = Mathf.Abs(ball.velocity.x) * randomDir;
                        ball.velocity.SetVelocity(new Vector3(ballXVelocity, ballYVelocity));
                    }
                    ball.isPowerHit = false;
                }
                else //强力一击
                {
                    if (ball.transform.position.x < columCenterXPos)
                    {
                        //球在左半场，向右半场给球加速
                        float ballXVelocity = (((Mathf.Abs(ball.transform.position.x - paimon.transform.position.x) * 12) + 5) + (Mathf.Abs(paimon.velocity.x * 10)));//-Mathf.Abs(ball.ballRig.velocity.x) - (Mathf.Abs(paimon.paimonRigbody2D.velocity.x) * 2);
                        ballYVelocity -= 4f;
                        ball.velocity.SetVelocity(new Vector3(ballXVelocity, ballYVelocity));
                    }
                    else
                    {
                        //球在右半场，向左半场给球加速
                        float ballXVelocity = -(((Mathf.Abs(ball.transform.position.x - paimon.transform.position.x) * 12) + 5) + (Mathf.Abs(paimon.velocity.x) * 10));//-Mathf.Abs(ball.ballRig.velocity.x) - (Mathf.Abs(paimon.paimonRigbody2D.velocity.x) * 2);
                        ballYVelocity -= 4f;
                        ball.velocity.SetVelocity(new Vector3(ballXVelocity, ballYVelocity));
                    }
                    ball.isPowerHit = true;
                }

            }

            //在于派蒙碰撞的时候，计算球的落点，用于AI来判断位置
            if (hadCollision)
            {
                ball.preTriggerTime = Time.time;
                ball.preTriggerPaimon = paimon.isLeftPaimon;
                GuessBallWillLandWhere(ball);
                paimon.computerShouldStandPos = -1;
                Debug.Log($"landPosx = {ball.guessLandPosX}");
            }

        }



    }

    public void GuessBallWillLandWhere(Ball ball)
    {
        FakeBall fakeBall = new FakeBall(ball);

        int loopTimes = 0;
        while (true)
        {
            //循环到球落地或者达到最大次数
            loopTimes++;

            float nextFakeBallPosX = fakeBall.xPos + (fakeBall.xVelocity * Time.deltaTime);
            //撞到墙上
            if (nextFakeBallPosX - ballRadius <= leftWallPos || nextFakeBallPosX + ballRadius >= rightWallPos)
            {
                
                fakeBall.xVelocity = -fakeBall.xVelocity;
            }

            float nextFakeBallPosY = fakeBall.yPos + (fakeBall.yVelocity * Time.deltaTime);
            //撞到天花板
            if (nextFakeBallPosY - ballRadius > ceilingYPos)
            {

                fakeBall.yVelocity = -fakeBall.yVelocity;
            }


            if((nextFakeBallPosX + ballRadius < 0.055f && nextFakeBallPosX + ballRadius > -0.045)||
                (nextFakeBallPosX - ballRadius < 0.055f && nextFakeBallPosX - ballRadius > -0.045))
            {
                //撞到网上
                //如果中心高度大于网的上端，代表球碰到网的上部，球的速度向上
                if(fakeBall.yPos > columTopPos && fakeBall.yPos - ballRadius < columTopPos)
                {
                    //将球的Y速度反转
                    fakeBall.yVelocity = -fakeBall.yVelocity;
                }else if(fakeBall.yPos < columTopPos && fakeBall.yPos - ballRadius < columTopPos)
                {
                    //撞到了网的下部，反转X方向的速度
                    if(fakeBall.xPos < 0)
                    {
                        //在左半场,反转速度向左
                        fakeBall.xVelocity = -Mathf.Abs(fakeBall.xVelocity);
                    }
                    else
                    {
                        //在右半场,反转速度向右
                        fakeBall.xVelocity = Mathf.Abs(fakeBall.xVelocity);
                    }
                }
            }

            //落地
            if(fakeBall.yPos - ballRadius < floorYpos || loopTimes >= loopMax)
            {
                //记录这个点
                break;
            }
            fakeBall.xPos += fakeBall.xVelocity * Time.deltaTime;
            fakeBall.yPos += fakeBall.yVelocity * Time.deltaTime;
            fakeBall.yVelocity -= 9.8f * Time.deltaTime;
        }

        ball.guessLandPosX = fakeBall.xPos;

    }


    public bool IsPaimonCollisionWithBall(Ball ball,Paimon paimon)
    {
        //判断派蒙是否与球发生碰撞
        float diffX = Mathf.Abs(ball.transform.position.x - paimon.transform.position.x);
        if(diffX < paimonHalfWidth + ballRadius)
        {
            float diffY = Mathf.Abs(ball.transform.position.y - paimon.transform.position.y);
            if(diffY < paimonHalfHighth + ballRadius)
            {
                Debug.Log("Paimon Collision");
                return true;
            }
        }
        return false;
    }

    public bool IsBallCollisionWithColumn(Ball ball)
    {
        //判断与柱子的碰撞
        float diffX = Mathf.Abs(ball.transform.position.x - columCenterXPos);
        if(diffX < columWidth + ballRadius)
        {
            float diffY = Mathf.Abs(ball.transform.position.y - columCenterYpos);
            if(diffY < columHighth + ballRadius)
            {
                Debug.Log("Column Collision");
                return true;
            }
        }
        return false;
    }

    public UserInput ComputerDecideControl(Paimon controlPaimon,Paimon otherPaimon,Ball ball)
    {
        UserInput controlInput = new UserInput();
        controlInput.xInput = 0;
        controlInput.jumpInput = 0;
        controlInput.powerHit = 0;

        //电脑控制派蒙行动
        if(ball.preTriggerPaimon != controlPaimon.isLeftPaimon)
        {
            //对方发球,，寻找可能的落地点，在附近等候
            if(ball.transform.position.x > -1)
            {
                var landPos = ball.guessLandPosX;
                if (landPos - controlPaimon.transform.position.x > Mathf.Epsilon + 0.05f)
                {
                    //球在派蒙右边,派蒙向右走
                    controlInput.xInput = 1;
                }
                else if (landPos - controlPaimon.transform.position.x < Mathf.Epsilon - 0.05f)
                {
                    //球在派蒙左边，派蒙向左走
                    controlInput.xInput = -1;
                }
            }


        }
        else
        {
            //己方发球,随机在靠中间的地方或者靠近网的地方等待

            if (controlPaimon.computerShouldStandPos == -1 && Mathf.Abs(ball.transform.position.x - controlPaimon.transform.position.x) > 5)
            {
                controlPaimon.computerShouldStandPos = Random.Range(0, 2);

            }
            else if(controlPaimon.computerShouldStandPos != -1 && Mathf.Abs(ball.transform.position.x - controlPaimon.transform.position.x) > 5)
            {
                if(controlPaimon.computerShouldStandPos == 0)
                {
                    //站网边
                    if(controlPaimon.isLeftPaimon == 0)
                    {
                        //左边的派蒙，往右边站
                        float targetPos = -1.2f;
                        if(controlPaimon.transform.position.x - targetPos < Mathf.Epsilon + 0.05f)
                        {
                            controlInput.xInput = 1;

                        }
                        else if(controlPaimon.transform.position.x - targetPos > Mathf.Epsilon - 0.5f)
                        {
                            controlInput.xInput = -1;
                        }
                    }
                    else
                    {
                        //右边的派蒙，往左边站
                        float targetPos = 1.2f;
                        if (controlPaimon.transform.position.x - targetPos < Mathf.Epsilon + 0.05f)
                        {
                            controlInput.xInput = 1;

                        }
                        else if (controlPaimon.transform.position.x - targetPos > Mathf.Epsilon - 0.05f)
                        {
                            controlInput.xInput = -1;
                        }
                    }
                }else
                {
                    //站中间

                    if (controlPaimon.isLeftPaimon == 0)
                    {
                        //左边的派蒙
                        float targetPos = leftWallPos / 2;
                        if (controlPaimon.transform.position.x - targetPos < Mathf.Epsilon + 0.05f)
                        {
                            controlInput.xInput = 1;

                        }
                        else if (controlPaimon.transform.position.x - targetPos > Mathf.Epsilon - 0.05f)
                        {
                            controlInput.xInput = -1;
                        }
                    }
                    else
                    {
                        //右边的派蒙
                        var targetPos = rightWallPos / 2;
                        if (controlPaimon.transform.position.x - targetPos < Mathf.Epsilon + 0.05f)
                        {
                            controlInput.xInput = 1;

                        }
                        else if (controlPaimon.transform.position.x - targetPos > Mathf.Epsilon - 0.05f)
                        {
                            controlInput.xInput = -1;
                        }
                    }

                }
            }
            else
            {
                //头顶起来了，稍微后退一点
                if(ball.transform.position.x > 0 && (ball.transform.position - controlPaimon.transform.position).magnitude < 5)
                {
                    //目标是球靠右的地方
                    if(controlPaimon.transform.position.x - ball.transform.position.x < 0.15f)
                    {
                        //向右走
                        controlInput.xInput = 1;
                    }
                    else if(controlPaimon.transform.position.x - ball.transform.position.x > 0.25f)
                    {
                        controlInput.xInput = -1;
                    }
                }
            }
            
        }


        //判断是否跳跃
        if(ball.transform.position.y < 1.2f
            && Mathf.Abs(ball.velocity.x) < 3
            && Mathf.Abs(controlPaimon.transform.position.x - ball.transform.position.x) < controlPaimon.computerBoldness + 0.5f)
        {
            //如果横向速度低，并且高度可以碰到,而且球在派蒙附近,尝试跳跃
            controlInput.jumpInput = 1;
            
        }

        if(controlPaimon.isJumping 
            && ball.preTriggerPaimon != controlPaimon.isLeftPaimon
            && !controlPaimon.isSmash)
        {
            //跳起来之后向球方向微调距离
            if (controlPaimon.transform.position.x - ball.transform.position.x < 0.1f)
            {
                controlInput.xInput = 1;

            }
            else if (controlPaimon.transform.position.x - ball.transform.position.x > -0.2f)
            {
                controlInput.xInput = -1;
            }
        }

        //因为预期的球距离有误差，所以在球飞过来的时候，微调距离
        if (controlPaimon.isLeftPaimon == 0)
        {
            //左派蒙微调
            if(ball.transform.position.x < 0 
                && Mathf.Abs(ball.transform.position.x - controlPaimon.transform.position.x) < 2 
                && controlPaimon.isJumping == false)
            {
                if (controlPaimon.transform.position.x - ball.transform.position.x < Mathf.Epsilon - 0.05f)
                {
                    controlInput.xInput = 1;

                }
                else if (controlPaimon.transform.position.x - ball.transform.position.x > Mathf.Epsilon + 0.05f)
                {
                    controlInput.xInput = -1;
                }
            }
        }
        else
        {
            //右派蒙微调
            if(ball.transform.position.x > 0 
                && Mathf.Abs(ball.transform.position.x - controlPaimon.transform.position.x) < 2
                && controlPaimon.isJumping == false)
            {
                if (controlPaimon.transform.position.x - ball.transform.position.x < Mathf.Epsilon - 0.05f)
                {
                    controlInput.xInput = 1;

                }
                else if (controlPaimon.transform.position.x - ball.transform.position.x > Mathf.Epsilon + 0.05f)
                {
                    controlInput.xInput = -1;
                }
            }
        }

        //在跳起来的时候，如果接近球,并且高度适合，尝试重击
        if (controlPaimon.isJumping && Mathf.Abs(ball.transform.position.x - controlPaimon.transform.position.x) < 0.5f)
        {
            if(ball.transform.position.y > -0.2)
            {
                controlInput.powerHit = 1;
            }
        }

        return controlInput;
    }

    public bool DecideWhetherInputPowerHit(Paimon paimon,Ball ball,Paimon otherPaimon)
    {

        return false;
    }

}

public class UserInput
{
    public int xInput;
    public int jumpInput;
    public int powerHit;
    public UserInput()
    {

    }
}



public class FakeBall{
    //用来计算球的落点
    public float xPos;
    public float yPos;
    public float xVelocity;
    public float yVelocity;

    public FakeBall(Ball ball)
    {
        xPos = ball.transform.position.x;
        yPos = ball.transform.position.y;
        xVelocity = ball.velocity.x;
        yVelocity = ball.velocity.y;

    }
}