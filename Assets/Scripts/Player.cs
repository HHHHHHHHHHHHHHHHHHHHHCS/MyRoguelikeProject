using UnityEngine;
using System.Collections;
 

public class Player : MonoBehaviour
{
    public AudioClip chop1Audio;
    public AudioClip chop2Audio;
    public AudioClip step1Audio;
    public AudioClip step2Audio;

    public AudioClip soda1Audio;
    public AudioClip soda2Audio;
    public AudioClip fruit1Audio;
    public AudioClip fruit2Audio;


    private float smoothing = 4;
    private float restTime = 0.5f;
    private float restTimer = 0;

    private Vector2 targetPos = new Vector2(1, 1);
    private Rigidbody2D myRigibody;
    private Animator myAnimator;

    private int checkLayer; 


    public float Smoothing
    {
        get
        {
            return smoothing;
        }

        set
        {
            smoothing = value;
        }
    }

    public Vector2 TargetPos
    {
        get
        {
            return targetPos;
        }

        set
        {
            targetPos = value;
        }
    }


    // Use this for initialization
    void Awake () {
        
        myRigibody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        checkLayer = ~(LayerMask.GetMask(MyLayerTag.Player));//取反  除了 Player层的物体
    }
	
	// Update is called once per frame
	void Update ()
    {
        myRigibody.MovePosition(Vector2.Lerp(transform.position, TargetPos, smoothing * Time.deltaTime));

        if (GameManager.Instance.Food <= 0 || GameManager.Instance.IsEnd)
        {
            return;
        }

        restTimer += Time.deltaTime;
        if (restTimer < restTime)
        {
            return;
        }    
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if(h>0)
        {
            v = 0;
        }
        if(h!=0||v!=0)
        {
            bool isMove = true;
            RaycastHit2D hit = Physics2D.Linecast(TargetPos, TargetPos + new Vector2(h, v), checkLayer);
            if(hit.transform==null)
            {
                AudioManager.Instance.RandomPlay(step1Audio, step2Audio);
                TargetPos += new Vector2(h, v);
            }
            else
            {
                switch(hit.collider.tag)
                {
                    case MyLayerTag.OutWall:
                        {
                            isMove = false;
                            break;
                        }
                    case MyLayerTag.Wall:
                        {
                            myAnimator.SetTrigger("Attack");
                            AudioManager.Instance.RandomPlay(chop1Audio, chop2Audio);
                            hit.transform.GetComponent<Wall>().TakeDamage();
                            break;
                        }
                    case MyLayerTag.Food:
                        {
                            GameManager.Instance.AddFood(10);
                            targetPos += new Vector2(h, v);
                            AudioManager.Instance.RandomPlay(step1Audio, step2Audio);
                            Destroy(hit.collider.gameObject);
                            AudioManager.Instance.RandomPlay(fruit1Audio, fruit2Audio);
                            break;
                        }
                    case MyLayerTag.Soda:
                        {
                            GameManager.Instance.AddFood(20);
                            targetPos += new Vector2(h, v);
                            AudioManager.Instance.RandomPlay(step1Audio, step2Audio);
                            Destroy(hit.collider.gameObject);
                            AudioManager.Instance.RandomPlay(soda1Audio, soda2Audio);
                            break;
                        }
                    case MyLayerTag.Enemy:
                        {
                            break;
                        }
                }    
            }
            if (isMove)
            {
                GameManager.Instance.ReduceFood(1);
                GameManager.Instance.OnPlayerMove();
                restTimer = 0;//不管是攻击还是移动都需要休息

            }       
        }
    }

    public void TakeDamage(int lossFood)
    {
        GameManager.Instance.ReduceFood(lossFood);
        myAnimator.SetTrigger("Damage");
    }
}
