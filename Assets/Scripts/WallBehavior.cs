using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehavior : MonoBehaviour
{
    public AudioSource wallSound; //sound collision with player(ball) causes to produce
    private void OnCollisionEnter(Collision player)
    {
        if(player.gameObject.CompareTag("Player"))
        {
            wallSound.Play();
        }
    }
}
