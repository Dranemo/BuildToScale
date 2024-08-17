using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerPower : MonoBehaviour
{
    


    [SerializeField] private Camera camera;
    private GameObject lastCollision;

    

    // Update is called once per frame
    void Update()
    {
        if(lastCollision != null && (!lastCollision.GetComponent<ScalableBlock>().moving && !lastCollision.GetComponent<ScalableBlock>().rescaling))
        {
            lastCollision.GetComponent<ScalableBlock>().ResetColoredFace();
            lastCollision = null;
        }


        RaycastHit hit;
        Vector3 destination = camera.transform.position + camera.transform.forward * 10;

        if (Physics.Linecast(camera.transform.position, destination, out hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("Scalable"))
            {
                Debug.DrawLine(camera.transform.position, destination, Color.red);
                if(lastCollision == null || (lastCollision != null && (!lastCollision.GetComponent<ScalableBlock>().rescaling)))
                {
                    hit.collider.GetComponent<ScalableBlock>().SetColoredFace(hit);
                }
                lastCollision = hit.collider.gameObject;
            }
            else
            {
                Debug.DrawLine(camera.transform.position, destination, Color.green);
            }
        }
    }



    
}