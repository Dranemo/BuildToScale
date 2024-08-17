using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerPower : MonoBehaviour
{


    [Header("Player Power Settings")]
    [SerializeField] private Camera camera;
    [SerializeField] LayerMask layerMask;

    [Header("Block Settings")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject blockTrigger;


    private GameObject lastCollision;
    public bool canSummonBlock = true;



    // Update is called once per frame
    void Update()
    {
        RaycastBlock();

        if(Input.GetButtonDown("SummonBlock"))
            SummonBlock();
    }



    private void RaycastBlock()
    {
        if (lastCollision != null && (!lastCollision.GetComponent<ScalableBlock>().moving && !lastCollision.GetComponent<ScalableBlock>().rescaling))
        {
            lastCollision.GetComponent<ScalableBlock>().ResetColoredFace();
            lastCollision = null;
        }


        RaycastHit hit;
        Vector3 destination = camera.transform.position + camera.transform.forward * 10;

        if (Physics.Linecast(camera.transform.position, destination, out hit, layerMask))
        {
            if (hit.collider != null && hit.collider.CompareTag("Scalable"))
            {
                Debug.DrawLine(camera.transform.position, destination, Color.red);
                if (lastCollision == null || (lastCollision != null && (!lastCollision.GetComponent<ScalableBlock>().rescaling)))
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

    private void SummonBlock()
    {
        if(canSummonBlock)
        {
            Debug.Log("Summoning block");

            //Instantiate(blockPrefab, blockTrigger.transform.position, Quaternion.identity);
            Instantiate(blockPrefab, blockTrigger.transform.position, blockTrigger.transform.rotation);

        }
    }
}