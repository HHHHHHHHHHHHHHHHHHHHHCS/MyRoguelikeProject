using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public AudioClip attackAduio;


    [SerializeField]
    private int lossFood = 10;

    private Vector2 targetPos;
    private Rigidbody2D myRigibody;
    private Animator myAnimator;
    private BoxCollider2D myBoxCollider;


    private Transform playerTS;

    private float smoothing = 3;


    void Start()
    {
        myRigibody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        playerTS =GameManager.Instance.Player.transform;
        targetPos = transform.position;
        GameManager.Instance.EnemyList.Add(this);
    }

    void Update()
    {
        myRigibody.MovePosition(Vector2.Lerp(transform.position, targetPos, smoothing * Time.deltaTime));
    }

    public void Move()
    {
        Vector2 offset = playerTS.position - transform.position;
        if (offset.magnitude < 1.1f)//距离是一格  1.1f 为了防止 临界值
        {//距离过小     攻击
            myAnimator.SetTrigger("Attack");
            AudioManager.Instance.RandomPlay(attackAduio);
            playerTS.GetComponent<Player>().TakeDamage(lossFood);
        }
        else
        {//距离过大追
            float x = 0, y = 0;
            if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
            {//按照y轴移动
                if (offset.y < 0)
                {//敌人在玩家的上边
                    y = -1;
                }
                else
                {
                    y = 1;
                }
            }
            else
            {//按照x轴移动
                if (offset.x > 0)
                {//敌人在玩家左方
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }
            //设置目标位置之前先做检测
            myBoxCollider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPos, targetPos + new Vector2(x, y));
            myBoxCollider.enabled = true;
            if (hit.transform == null)
            {
                targetPos += new Vector2(x, y);
            }
            else
            {
                if (hit.collider.tag == MyLayerTag.Food || hit.collider.tag == MyLayerTag.Soda)
                {
                    targetPos += new Vector2(x, y);
                }
            }
        }
    }
}
