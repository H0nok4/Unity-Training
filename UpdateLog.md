
战棋游戏的Demo正在做战斗系统。
目前规划的模块
地图 —— 半完成  
战斗状态 —— 用状态设计模式做了一套循环，目前是可以玩家选择角色→选择移动位置→选择行动→如果是攻击的话选择攻击的目标，待机的话就结束→所有玩家的单位移动过后进入AI回合→AI回合结束后再次进入玩家回合  
单位 —— 差不多完成  
AI —— 刚写完步兵的一个AI状态，准备等战斗完善一点后再添加多几个状态和多几个种类的角色  
寻路 —— A*算法
战斗 —— 攻击√道具√等待√ 技能×
装备 —— 做了个雏形  

2021/5/15  
在战斗中添加了道具使用，并且做了SmallPotion,Bandage,Bomb三种道具  
给战斗加了俩个小UI，左下角显示光标所在处的角色血量信息，右下角显示光标所在处的地形信息  
2021/5/16  
制作了一个简易的自制脚本语言解释器，在Script/Core/plotSystem下。  
暂时只完成了语法分析，连检错报错都还没完成，测试的剧本文本在Resource/Scenario下，包含了这个语言目前设计的所有语法。  
(从图书馆借了本编译原理来看，看看几天能搞定)  
2021/5/17  
完成了一个简易的剧情系统，通过读取并解释Resource/Scenario下的剧本，可以开始播放剧情，做出选项引出不同的对话等，并且播放剧情完毕后游戏状态会翻转回播放剧情之前的状态继续游戏。  
不过解释器最重要的检错报错还没有完善，对于脚本里的语法错误没有提示，明天完善一下。   