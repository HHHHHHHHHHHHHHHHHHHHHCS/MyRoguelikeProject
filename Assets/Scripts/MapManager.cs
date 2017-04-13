using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{

    public Transform mapHolder;
    public GameObject[] outWallArray;
    public GameObject[] floorArray;
    public GameObject[] wallArray;
    public GameObject[] foodArray;
    public GameObject[] enemyArray;
    public GameObject exitPrefab;

    public int rows = 10;//行
    public int cols = 10;//列

    public int minCountWall = 2;
    public int maxCountWall = 8;

    private List<Vector2> positionList = new List<Vector2>();

    private GameManager gameManager;



    /// <summary>
    /// 初始化地图
    /// </summary>
    public void InitMap()
    {
        mapHolder = new GameObject("MapHolder").transform;
        gameManager = GetComponent<GameManager>();
        //创建围墙和地板
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject go;
                if (x == 0 || y == 0
                    || x == cols - 1 || y == rows - 1)
                {
                    int index = Random.Range(0, outWallArray.Length);
                    go = (GameObject)GameObject.Instantiate(outWallArray[index], new Vector3(x, y, 0), Quaternion.identity);
                }
                else
                {
                    int index = Random.Range(0, floorArray.Length);
                    go = (GameObject)GameObject.Instantiate(floorArray[index], new Vector3(x, y, 0), Quaternion.identity);
                }
                go.transform.SetParent(mapHolder);
            }
        }
        //初始化游戏主要地图
        positionList.Clear();
        for (int x = 2; x < cols - 2; x++)
        {
            for (int y = 2; y < rows - 2; y++)
            {
                positionList.Add(new Vector2(x, y));
            }
        }

        /*--Start--创建障碍物 食物 敌人-----*/
        //创建障碍物
        int wallCount = Random.Range(minCountWall, maxCountWall + 1);
        InstantiateItems(wallCount, wallArray);
        //创建食物 2-level*2
        int foodCount = Random.Range(2, gameManager.Level * 2 + 1);
        InstantiateItems(foodCount, foodArray);
        //创建敌人 数量为level/2
        int enemyCount = gameManager.Level / 2;
        InstantiateItems(enemyCount, enemyArray);
        /*--End--创建障碍物 食物 敌人 -----*/

        //创建出口
        /*GameObject exitGo = (GameObject)*/GameObject.Instantiate(exitPrefab, new Vector3(cols - 2, rows - 2), Quaternion.identity);
    }

    private Vector2 RandomPosition()
    {
        //随机获得位置
        int positionIndex = Random.Range(0, positionList.Count);
        Vector2 pos = positionList[positionIndex];
        positionList.RemoveAt(positionIndex);
        return pos;
    }

    private GameObject RandomPrefab(GameObject[] prefabs)
    {
        //随机获得数组物体
        int index = Random.Range(0, prefabs.Length);
        GameObject newGo = prefabs[index];
        return newGo;
    }

    private void InstantiateItems(int itemNumber,GameObject[] itemArray)
    {
        for (int i = 0; i < itemNumber; i++)
        {
            //随机获得位置
            Vector2 pos = RandomPosition();
            //随机创建障碍物在随机的位置上
            GameObject newPrefab = RandomPrefab(itemArray);
            GameObject newGo = (GameObject)GameObject.Instantiate(newPrefab, pos, Quaternion.identity);
            newGo.transform.SetParent(mapHolder);
        }
    }
}
