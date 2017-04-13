using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    public Sprite damageSprite;//收到攻击的图片

    private int hp = 2;

    /// <summary>
    /// 墙自身受到攻击
    /// </summary>
	public void TakeDamage()
    {
        hp -= 1;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        GetComponent<SpriteRenderer>().sprite = damageSprite;
    }
}
