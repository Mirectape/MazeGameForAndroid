using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider player)
    {
        if(player.CompareTag("Player"))
        {
            transform.gameObject.SetActive(false);
            if(this.gameObject.CompareTag("Bonus"))
            {
                FindObjectOfType<GameManager>().isBonusTaken = true;
            }
            if(this.gameObject.CompareTag("TimeTurner"))
            {
                FindObjectOfType<GameManager>().isTimeTurnerTaken = true;
            }
        }
    }
}
