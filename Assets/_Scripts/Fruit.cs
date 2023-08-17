using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.sharedInstance.MakeInvincibleFor(8.0f);

            Destroy(this.gameObject);

        }
    }
}
