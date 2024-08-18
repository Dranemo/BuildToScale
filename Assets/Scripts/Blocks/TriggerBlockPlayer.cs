using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBlockPlayer : MonoBehaviour
{
    [SerializeField] GameObject block;
    ScalableBlock scalableBlock;




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger block");
            scalableBlock.cantScaleUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger block");
            scalableBlock.cantScaleUp = false;
        }
    }


    private Vector3 initialScale;

    void Start()
    {
        // Enregistrer l'�chelle initiale de l'objet
        initialScale = transform.localScale;
        scalableBlock = block.GetComponent<ScalableBlock>();
    }

    private void Update()
    {
        SetScale();
    }


    private void SetScale()
    {
        // Calculer l'�chelle mondiale actuelle de l'objet
        transform.localScale = initialScale;
        Vector3 worldScale = transform.lossyScale;

        // Calculer le facteur de correction pour maintenir l'�chelle mondiale � 1 sur l'axe Y
        float correctionFactor = 1 / worldScale.y;

        // Appliquer le facteur de correction � l'�chelle locale de l'objet
        transform.localScale = new Vector3(initialScale.x, initialScale.y * correctionFactor, initialScale.z);
        //Debug.Log("Trigger block scale: " + transform.localScale);
    }
}
