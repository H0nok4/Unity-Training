using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpriteAnimator//自定义的Sprite Animator基类，用来控制各类角色的自定义动画。
{
    SpriteRenderer spriteRenderer;
    //工作原理：按照固定的时间间隔按顺序修改物体的Sprite，模拟动画的播放。
    //需要：一个顺序存放需要播放的动画的Sprte List  一个时间间隔 
    //额外设置 一个变量表示当前播放到哪一帧 一个变量表示当前经过的时间 经过的时间大于间隔后就播放到下一帧。
    List<Sprite> Frames;//这个数组用来存放当前动画所需要的Sprite
    public List<Sprite> frames
    {
        get;
    }
    float frameRate;

    int currentFrames;
    float timer;
    public SpriteAnimator(List<Sprite> frames, SpriteRenderer spriteRenderer, float frameRate = 0.16f)//初始化当前的自定义Animator
    {
        this.spriteRenderer = spriteRenderer;
        this.frames = frames;
        this.frameRate = frameRate;
    }

    public void Start()//初始化变量，并且设置初始帧
    {
        currentFrames = 0;
        timer = 0f;
        spriteRenderer.sprite = frames[0];
    }

    public void HandleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            currentFrames = (currentFrames + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrames];
            timer -= frameRate;
        }
    }


}
