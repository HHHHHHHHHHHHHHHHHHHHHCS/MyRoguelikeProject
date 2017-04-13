using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public AudioClip dieClip;

    private Player player;
    private MapManager mapManager;

    private Text foodText;
    private Text dayText;
    private Image dayBg;
    private Text failText;

    private int level = 1;
    private int food = 20;

    private bool isEnd = false;//是否到达终点

    private List<Enemy> enemyList = new List<Enemy>();

    private bool sleepStep = true;

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    public int Food
    {
        get
        {
            return food;
        }

        set
        {
            food = value;
        }
    }

    public List<Enemy> EnemyList
    {
        get
        {
            return enemyList;
        }

        set
        {
            enemyList = value;
        }
    }

    public Player Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public bool IsEnd
    {
        get
        {
            return isEnd;
        }

        set
        {
            isEnd = value;
        }
    }

    void Awake()
    {
        if(_instance==null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);   
        }
        InitGame();
    }

    void InitGame()
    {
        //初始化地图
        mapManager = GetComponent<MapManager>();
        mapManager.InitMap();

        //初始化UI
        player = GameObject.FindGameObjectWithTag(MyLayerTag.Player).GetComponent<Player>();
        mapManager = GetComponent<MapManager>();
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        dayBg = GameObject.Find("DayBg").GetComponent<Image>();
        dayText = GameObject.Find("DayText").GetComponent<Text>();
        failText = GameObject.Find("FailText").GetComponent<Text>();
        failText.gameObject.SetActive(false);
        UpdateFoodText(0);

        //初始化参数
        isEnd = false;
        enemyList.Clear();
        UpdateDay();
        Invoke("HideBlack", 1.0f);
    }

    void UpdateDay()
    {
        dayText.text = "Day " + level;
    }

    void UpdateFoodText(int foodChange)
    {//foodChange 食物的改变
        string str = "";
        if (foodChange > 0)
        {
            str = "+" + foodChange;
        }
        else
        {
            str = foodChange.ToString();
        }
        foodText.text = str + "   " + "Food:" + food;
    }


    public void ReduceFood(int count)
    {
        food -= count;
        UpdateFoodText(-count);
        if(food<=0)
        {
            Fail();
        }
    }

    public void AddFood(int count)
    {
        food += count;
        UpdateFoodText(count);     
    }

    public void OnPlayerMove()
    {
        if(sleepStep)
        {
            sleepStep = false;
        }
        else
        {
            foreach(var enemy in EnemyList)
            {
                enemy.Move();
            }
            sleepStep = true;
        }
        //检测有没有到达终点
        if(player.TargetPos.x==mapManager.cols-2&&player.TargetPos.y==mapManager.rows-2)
        {
            IsEnd = true;
            //加载下一个关卡
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Fail()
    {
        AudioManager.Instance.RandomPlay(dieClip);
        failText.gameObject.SetActive(true);
    }

    void OnLevelWasLoaded(int secenLevel)
    {
        Level++;
        InitGame();
    }

    void HideBlack()
    {
        dayBg.gameObject.SetActive(false);
    }
}
