
// --功能--
// 主角可以自己找到孙女，也可以通过提示找到孙女
// 4 有怪物，没有武器和装备
// 5 找到孙女可以开启光明模式，要火把才行
// 6 怪物攻击，闻到敌人气息
// 7 体力下降，移动需要CD
// 8 见到少年，如果老人伤害了孙女，那么孙女应少年请求，杀掉老人

// --策划原则--
// 不需要的功能坚决砍掉，尽量简化，突核心玩法

// --BUG--


// --编程Tips--
// Clear和Init相互独立，Init内部不能调用Clear
// Player是一个独立于GameManager的系统
// 父级调用子级函数，但是子级只能通过时间来通知父级调用函数

//-- 剧情
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



using UnityEngine;
using System.Collections;
using System; 

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
	}
}
