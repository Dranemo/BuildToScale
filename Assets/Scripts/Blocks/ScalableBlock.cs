using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScalableBlock : MonoBehaviour
{
    [SerializeField] private Transform coloredFace;
    [SerializeField] private Transform triggerBlock;

    [SerializeField] private Material[] materials;


    public bool rescaling = false;
    public bool moving = false;
    public bool cantScaleUp = false;

    bool completelyReseted = true;

    float positionOffset = 0.5f + 0.001f; //Offset - Colored face
    float positionOffsetTrigger = 0.5f; //Offset - Trigger

    private void Start()
    {
        coloredFace.localScale = Vector3.one / 10;
    }

    private enum FaceDirection
    {
        Up,
        Down,
        North,
        South,
        East,
        West
    }

    FaceDirection faceDirection;
    float minDistanceMoving = 0;
    Vector3 lastPlayerPos = Vector3.zero;
    Vector3 playerRotation = Vector3.zero;


    private FaceDirection GetFaceDirection(RaycastHit hit)
    {
        if (hit.normal == transform.up)
        {
            //Debug.Log("Up");
            return FaceDirection.Up;
        }
        else if (hit.normal == -transform.up)
        {
            //Debug.Log("Down");
            return FaceDirection.Down;
        }
        else if (hit.normal == transform.forward)
        {
            //Debug.Log("North");
            return FaceDirection.North;
        }
        else if (hit.normal == -transform.forward)
        {
            //Debug.Log("South");
            return FaceDirection.South;
        }
        else if (hit.normal == transform.right)
        {
            //Debug.Log("East");
            return FaceDirection.East;
        }
        else if (hit.normal == -transform.right)
        {
            //Debug.Log("West");
            return FaceDirection.West;
        }
        else
        {
            return FaceDirection.Up;
        }
    }


    public void SetColoredFace(RaycastHit hit)
    {
        faceDirection = GetFaceDirection(hit);
        //coloredFace.localScale = transform.localScale / 10;

        switch (faceDirection)
        {
            case FaceDirection.Up:
                coloredFace.localRotation = Quaternion.Euler(0, 0, 0);
                coloredFace.localPosition = new Vector3(0, positionOffset, 0);
                break;
            case FaceDirection.Down:
                coloredFace.localRotation = Quaternion.Euler(0, 0, 180);
                coloredFace.localPosition = new Vector3(0, -positionOffset, 0);
                break;
            case FaceDirection.North:
                coloredFace.localRotation = Quaternion.Euler(90, 0, 0);
                coloredFace.localPosition = new Vector3(0, 0, positionOffset);
                break;
            case FaceDirection.South:
                coloredFace.localRotation = Quaternion.Euler(-90, 0, 0);
                coloredFace.localPosition = new Vector3(0, 0, -positionOffset);
                break;
            case FaceDirection.East:
                coloredFace.localRotation = Quaternion.Euler(90, 90, 0);
                coloredFace.localPosition = new Vector3(positionOffset , 0, 0);
                break;
            case FaceDirection.West:
                coloredFace.localRotation = Quaternion.Euler(90, -90, 0);
                coloredFace.localPosition = new Vector3(-positionOffset, 0, 0);
                break;
        }
        
        coloredFace.gameObject.SetActive(true);
        SetTrigger(faceDirection);

    }

    public void ResetColoredFace()
    {
        coloredFace.gameObject.SetActive(false);
    }

    private void SetTrigger(FaceDirection faceDirection)
    {
        Vector3 halfTrigger = triggerBlock.localScale / 2;
        float position = positionOffsetTrigger + halfTrigger.y;

        switch (faceDirection)
        {
            case FaceDirection.Up:
                triggerBlock.localRotation = Quaternion.Euler(0, 0, 0);
                triggerBlock.localPosition = new Vector3(0, position, 0);
                break;
            case FaceDirection.Down:
                triggerBlock.localRotation = Quaternion.Euler(0, 0, 180);
                triggerBlock.localPosition = new Vector3(0, -position, 0);
                break;
            case FaceDirection.North:
                triggerBlock.localRotation = Quaternion.Euler(90, 0, 0);
                triggerBlock.localPosition = new Vector3(0, 0, position);
                break;
            case FaceDirection.South:
                triggerBlock.localRotation = Quaternion.Euler(-90, 0, 0);
                triggerBlock.localPosition = new Vector3(0, 0, -position);
                break;
            case FaceDirection.East:
                triggerBlock.localRotation = Quaternion.Euler(90, 90, 0);
                triggerBlock.localPosition = new Vector3(position, 0, 0);
                break;
            case FaceDirection.West:
                triggerBlock.localRotation = Quaternion.Euler(90, -90, 0);
                triggerBlock.localPosition = new Vector3(-position, 0, 0);
                break;
        }
    }




    private void Update()
    {
        



        if (moving || rescaling)
        {
            if (Input.GetMouseButtonUp(0) && rescaling)
            {
                rescaling = false;

                coloredFace.GetComponent<MeshRenderer>().material = materials[2]; // Vert
            }

            else if (Input.GetMouseButtonUp(1) && moving)
            {
                moving = false;

                coloredFace.GetComponent<MeshRenderer>().material = materials[2]; // Vert
            }
        }


        else if (coloredFace.gameObject.activeSelf)
        {
            completelyReseted = false;

            if (Input.GetButtonDown("DeleteBlock"))
            {
                StartCoroutine(DeleteBlock());
            }



            else if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Click droit");


                moving = true;
                rescaling = false;

                coloredFace.GetComponent<MeshRenderer>().material = materials[1]; // Rose


                minDistanceMoving = Vector3.Distance(Player.GetMainCamera().transform.position, transform.position);

                lastPlayerPos = Player.GetPlayer().transform.position;
                playerRotation = Player.GetPlayer().transform.eulerAngles;

            }
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Click gauche");

                rescaling = true;
                moving = false;

                coloredFace.GetComponent<MeshRenderer>().material = materials[0]; // Rouge

                lastPlayerPos = Player.GetPlayer().transform.position;
            }

        }


        

        else if(!completelyReseted)
        {
            completelyReseted = true;
            moving = false;
            rescaling = false;

            coloredFace.GetComponent<MeshRenderer>().material = materials[2];

            lastPlayerPos = Vector3.zero;
            playerRotation = Vector3.zero;

            minDistanceMoving = 0;
        }




        if (moving) // Rose
        {
            if(cantScaleUp && faceDirection == FaceDirection.Up)
            {
                return;
            }
            else
            {

                MoveBlock();
                lastPlayerPos = Player.GetPlayer().transform.position;
                playerRotation = Player.GetPlayer().transform.eulerAngles;
            }
        }
        else if (rescaling) // Rouge
        {
            Rescale();
            lastPlayerPos = Player.GetPlayer().transform.position;
            SetTrigger(faceDirection);
        }
    }





    IEnumerator DeleteBlock()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
        Player.GetPlayer().GetComponent<PlayerPower>().canSummonBlock = true;
    }



    // -------------------------------------------------------------------------------- Fonctions Move Block -------------------------------------------------------------------------------- //
    // Principal Fonction
    // Déplacer le bloc en fonction de la position de la caméra et du joueur
    private void MoveBlock()
    {
        Transform cameraTransform = Player.GetMainCamera().transform;
        Transform player = Player.GetPlayer().transform;

        Vector3 offset = transform.position - cameraTransform.position;
        Vector3 playerMovement = player.position - lastPlayerPos;

        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * offset.magnitude;

        Vector3 PlayerRotationDiff = playerRotation - player.eulerAngles;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10);
        

        // Ajouter le mouvement du joueur
        transform.position += playerMovement;
        // Ajouter la rotation du joueur
        transform.eulerAngles -= PlayerRotationDiff;

        // Si le bloc est trop proche de la caméra, le déplacer
        if (Vector3.Distance(cameraTransform.position, transform.position) < minDistanceMoving)
        {
            transform.position = cameraTransform.position + cameraTransform.forward * minDistanceMoving;
        }



        // MoveTowards pour déplacer l'objet
        if (transform.position.y < 0 + transform.localScale.y / 2)
        {
            Debug.Log("en dessous");
            transform.position = new Vector3(transform.position.x, 0 + transform.localScale.y / 2, transform.position.z);
        }
    }




    // -------------------------------------------------------------------------------- Fonctions Rescale Block -------------------------------------------------------------------------------- //
    // Principal Fonction
    // Redimensionner le bloc en fonction de la position de la caméra et du joueur
    private void Rescale()
    {
        Transform player = Player.GetPlayer().transform;
        Transform camera = Player.GetMainCamera().transform;


        Vector3 cameraPos = camera.position;
        Vector3 cameraForward = camera.forward;

        Vector3 faceCenter = GetFaceCenter(faceDirection);
        Vector3 faceNormal = GetFaceNormal(faceDirection);
        Vector3 faceCenterCube = transform.InverseTransformPoint(faceCenter);


        Vector3 intersectionPoint = CalculateIntersection(faceCenter, faceNormal, cameraPos, cameraForward);
        Vector3 intersectionPointCube = transform.InverseTransformPoint(intersectionPoint);

        Vector3 distanceInterFace = intersectionPointCube - faceCenterCube;





        int negative = 1;
        if(faceDirection == FaceDirection.Down || faceDirection == FaceDirection.South || faceDirection == FaceDirection.West)
        {
            negative = -1;
        }

        if(transform.localScale.x + distanceInterFace.x * negative < 0.5f || transform.localScale.y + distanceInterFace.y * negative < 0.5f || transform.localScale.z + distanceInterFace.z * negative < 0.5f ||
           transform.localScale.x + distanceInterFace.x * negative > 10 || transform.localScale.y + distanceInterFace.y * negative > 10 || transform.localScale.z + distanceInterFace.z * negative > 10)
        {
            return;
        }
        else if (cantScaleUp && (transform.localScale.x + distanceInterFace.x * negative > transform.localScale.x || transform.localScale.y + distanceInterFace.y * negative > transform.localScale.y || transform.localScale.z + distanceInterFace.z * negative > transform.localScale.z))
        {
            return;
        }
        else
        {
            transform.localScale += distanceInterFace * negative;
            transform.position += transform.TransformDirection(distanceInterFace) / 2;
        }

    }


    // Recuperer le centre de la face du bloc en fonction de la direction
    private Vector3 GetFaceCenter(FaceDirection direction)
    {
        Vector3 center = transform.position;
        Vector3 halfScale = transform.localScale / 2;

        switch (direction)
        {
            case FaceDirection.Up: return center + transform.up * halfScale.y;
            case FaceDirection.Down: return center - transform.up * halfScale.y;
            case FaceDirection.North: return center + transform.forward * halfScale.z;
            case FaceDirection.South: return center - transform.forward * halfScale.z;
            case FaceDirection.East: return center + transform.right * halfScale.x;
            case FaceDirection.West: return center - transform.right * halfScale.x;
            default: return center;
        }
    }

    // Recuperer la normale de la face
    private Vector3 GetFaceNormal(FaceDirection direction)
    {
        switch (direction)
        {
            case FaceDirection.Up: return transform.up;
            case FaceDirection.Down: return -transform.up;
            case FaceDirection.North: return transform.forward;
            case FaceDirection.South: return -transform.forward;
            case FaceDirection.East: return transform.right;
            case FaceDirection.West: return -transform.right;
            default: return transform.up;
        }
    }

    // Recuperer un point d'intersection entre deux vecteurs 
    // (utilisé pour connaitre le point d'intersection entre la normale de la face et le vecteur de la caméra, afin de savoir ou arreter le rescale)
    private Vector3 CalculateIntersection(Vector3 pointA, Vector3 directionA, Vector3 pointB, Vector3 directionB)
    {
        // Les directions doivent être normalisées
        directionA.Normalize();
        directionB.Normalize();

        // Calculer les vecteurs entre les points
        Vector3 diff = pointB - pointA;

        // Calculer les produits croisés
        Vector3 crossD = Vector3.Cross(directionA, directionB);
        Vector3 crossDiffD = Vector3.Cross(diff, directionB);

        // Si les vecteurs sont parallèles, il n'y a pas d'intersection
        if (crossD.sqrMagnitude < Mathf.Epsilon)
        {
            return Vector3.zero; // Pas d'intersection
        }

        // Calculer le paramètre t pour la ligne A
        float t = Vector3.Dot(crossDiffD, crossD) / crossD.sqrMagnitude;

        // Calculer le point d'intersection
        Vector3 intersection = pointA + t * directionA;

        return intersection;
    }



}
