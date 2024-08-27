using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVerifTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Player")
        {
            Player.GetPlayer().transform.position += Vector3.up;
        }
    }

    private void Update()
    {
        if (Player.GetPlayer().transform.position.y >= 8)
        {
            Player.GetPlayer().transform.position = new Vector3(Player.GetPlayer().transform.position.x, 2, Player.GetPlayer().transform.position.z) ;
        }
    }

}
