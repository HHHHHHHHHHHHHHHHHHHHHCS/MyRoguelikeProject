using UnityEngine;
using System.Collections;

public class Load : MonoBehaviour
{
    public GameObject gameManager;

    void Awake()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(gameManager);
        }
        
    }
}
