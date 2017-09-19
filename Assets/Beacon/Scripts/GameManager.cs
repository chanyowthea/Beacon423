
// --功能--
// 4 有怪物，没有武器和装备
// 5 找到孙女可以开启光明模式，要火把才行 // 不需要火把，灯塔里面有灯光
// 6 怪物攻击，闻到敌人气息
// 7 体力下降，移动需要CD

// --战斗系统--
// 遇见敌人，孙女在一旁不动，爷爷战斗，以爷爷为中心展开战斗格子，怪物计算敌人自己胜率，如果大于一定值就不逃跑，
// 否则移动到边界就逃跑，每一次自己或者敌人受伤都计算一次自己的胜率
// 有恢复HP的地方
// 8 见到少年，如果老人伤害了孙女，那么孙女应少年请求，杀掉老人 // 重要！右边传来少女的清香
// 如果少年的诱惑次数达到一定值就触发少女反骨

// 9 灯塔前面的剧情
// [优化]只显示一部分，其余地方黑暗

// --策划原则--
// 不需要的功能坚决砍掉，尽量简化，突核心玩法

// --BUG--
// 首句爷爷说话后，显示了移动提示
// cong level 4 -> level3 // 不应该回去
// 从level4回到level3人物朝向错误

// --编程Tips--
// Clear和Init相互独立，Init内部不能调用Clear
// Player是一个独立于GameManager的系统
// 父级调用子级函数，但是子级只能通过时间来通知父级调用函数

//-- 剧情
// 苍山若影，银河如练
// 漆黑的夜，怒号的海浪扑灭了灰穆港唯一的灯塔——维希灯塔
// 维希灯塔坐落于临野峰峰顶，地形险峻，道路崎岖
// 但若没了灯塔，船只便难以确定方位，从而大大增加了村民行船事故发生率，众人对此头疼不已
// 此时，双目失明的姜游毛遂自荐，只身前往点亮灯塔
// 众人见状纷纷劝阻
// 姜游却声称多次探索过维希灯塔，对临野峰地形了然于胸
// 在姜游的一再坚持下，众人终于成全了姜游的心愿
// 镇民们本欲馈赠些许礼物，姜游却一一婉拒
// 次日破晓，姜游未待众人送行便踏上行途……

// 上楼
// 一阵微弱的哭声从楼下传来
// 下楼
// 你摸了摸四处的墙壁，没有发现什么
// 一阵微弱的哭声从楼下传来
// 你摸了摸四处的楼梯，没有发现什么
// 一阵微弱的哭声从楼下传来
// 你向楼梯内侧走近，听见哭声变大

// 触发角色，角色id，对话语句
// 剧情开始
// 爷爷：姜游，孙女：姜寻
// 孙女：爷爷，爷爷，你在哪？我好饿，好冷，好想回家……呜呜……呜呜呜……
// 爷爷：是寻儿！原来她竟在这里，找得我好苦啊！
// 爷爷：寻儿别害怕，我在这里，爷爷在这里，没事的！
// 孙女：爷爷！爷爷！你在哪？
// 爷爷：寻儿，我在这里，你听得到吗？
// 孙女：我看到了！爷爷！爷爷！
// 孙女扑进爷爷怀中
// 爷爷轻抚着孙女的长发
// 孙女：爷爷，爷爷你终于来了！这里好黑好可怕呜呜……
// 爷爷：没事的，我们等会儿就回去！
// 孙女：我们什么时候回去？
// 爷爷：等点亮灯塔就回去。
// 孙女：好的，爷爷我带你去！（由于点亮灯塔要走很远的路，因此先点亮，不需要很长时间）
// SystemMsg：光明模式开启


// 黑王子：真巧，碰到两个猎物。
// 孙女：你……你是谁？
// 黑王子：嘿，告诉你无妨。我是守护灯塔的黑王子，任何胆敢损坏灯塔的异类都要被清除。 // 黑王子已经统治了灯塔
// 爷爷：胡说，这个灯塔是隶属治淮镇的，不属于任何个人！
// 黑王子：我说它属于我就属于我，你们若是识相就该乖乖交出保护费，然后回去。
// 爷爷：我来此必要点亮灯塔，才可回去！
// 黑王子：灯塔是否要点亮，我说了算！
// 孙女：爷爷，我们还是……
// 爷爷：寻儿，没事的。
// 爷爷：你这恶霸，今日我偏要点亮了！
// 黑王子：敬酒不吃吃罚酒！
// 孙女：爷爷小心。



using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic; 

public class GameManager : MonoBehaviour
{
	public static GameManager _Instance; 

    void Start()
    {
        Init(); 
    }

    void Init() 
    {
		_Instance = this; 
		GameData._Instance.Init(); 
		UIManager._Instance.Init(); 

		Player._Instance.Init(); 
		Player._Instance._OnReset = Reset; 
		Player._Instance._OnClear = Clear; 

    }

    public void Reset()
    {
		GameData._Instance.Reset(); 
		Player._Instance.Reset(); 
		UIManager._Instance.Reset(); 
    }

	void OnDestroy()
	{
		Clear(); 
	}

	public void Clear()
	{
		GameData._Instance.Clear(); 
		UIManager._Instance.Clear(); 
		Player._Instance.Clear(); 
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit(); 
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			Player._Instance.transform.eulerAngles = Vector3.zero;
			GameData._CanRotateCamera = false; 
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			UIManager._Instance.SetMaskEnable(!GameData._isOpenMask); 
		}
	}
}
