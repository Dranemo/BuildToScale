using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScalableBlock : MonoBehaviour
{
    [SerializeField] private Transform coloredFace;
    [SerializeField] private Material[] materials;


    public bool rescaling = false;
    public bool moving = false;

    float minDistanceMoving = 0;
    bool completelyReseted = true;

    float positionOffset = 0.5f + 0.001f;

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
    Vector3 lastPlayerPos = Vector3.zero;


    private FaceDirection GetFaceDirection(RaycastHit hit)
    {
        if (hit.normal == Vector3.up)
        {
            return FaceDirection.Up;
        }
        else if (hit.normal == Vector3.down)
        {
            return FaceDirection.Down;
        }
        else if (hit.normal == Vector3.forward)
        {
            return FaceDirection.North;
        }
        else if (hit.normal == Vector3.back)
        {
            return FaceDirection.South;
        }
        else if (hit.normal == Vector3.right)
        {
            return FaceDirection.East;
        }
        else if (hit.normal == Vector3.left)
        {
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

    }

    public void ResetColoredFace()
    {
        coloredFace.gameObject.SetActive(false);
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

            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Click droit");

                moving = true;
                rescaling = false;

                coloredFace.GetComponent<MeshRenderer>().material = materials[1]; // Rose


                minDistanceMoving = Vector3.Distance(Player.GetMainCamera().transform.position, transform.position);

                lastPlayerPos = Player.GetPlayer().transform.position;

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

            minDistanceMoving = 0;
        }




        if (moving) // Rose
        {
            MoveBlock();
            lastPlayerPos = Player.GetPlayer().transform.position;
        }
        else if (rescaling) // Rouge
        {
            Rescale();
            lastPlayerPos = Player.GetPlayer().transform.position;
        }
    }



    /*private void Rescale()
    {
        Transform player = Player.GetPlayer().transform;
        Transform camera = Player.GetMainCamera().transform;


        Vector3 playerMovement = player.position - lastPlayerPos;
        Vector3 offset = transform.position - camera.position;




        switch (faceDirection)
        {
            case FaceDirection.Up:
                if(transform.localScale.y + playerMovement.y >= 0.5f)
                {
                    transform.localScale += new Vector3(0, playerMovement.y, 0);
                    transform.position += new Vector3(0, playerMovement.y / 2, 0);
                }
                break;
            case FaceDirection.Down:
                if (transform.localScale.y + playerMovement.y >= 0.5f)
                {
                    transform.localScale -= new Vector3(0, playerMovement.y, 0);
                    transform.position += new Vector3(0, playerMovement.y / 2, 0);
                }
                break;
            case FaceDirection.North:
                Debug.Log("North");
                if (transform.localScale.z + playerMovement.z >= 0.5f)
                {
                    transform.localScale += new Vector3(0, 0, playerMovement.z);
                    transform.position += new Vector3(0, 0, playerMovement.z / 2);
                }
                break;
            case FaceDirection.South:
                Debug.Log("South");
                if (transform.localScale.z - playerMovement.z >= 0.5f)
                {
                    transform.localScale -= new Vector3(0, 0, playerMovement.z);
                    transform.position += new Vector3(0, 0, playerMovement.z / 2);
                }
                break;
            case FaceDirection.East:
                Debug.Log("East");
                if (transform.localScale.x + playerMovement.x >= 0.5f)
                {
                    transform.localScale += new Vector3(playerMovement.x, 0, 0);
                    transform.position += new Vector3(playerMovement.x / 2, 0, 0);
                }
                break;
            case FaceDirection.West:
                Debug.Log("West");
                if (transform.localScale.x - playerMovement.x >= 0.5f)
                {
                    transform.localScale -= new Vector3(playerMovement.x, 0, 0);
                    transform.position += new Vector3(playerMovement.x / 2, 0, 0);
                }
                break;
        }

    }*/


    private void Rescale()
    {
        Transform player = Player.GetPlayer().transform;
        Transform camera = Player.GetMainCamera().transform;

        Vector3 playerMovement = player.position - lastPlayerPos;

        // Récupérer la normale de la face
        Vector3 faceNormal = GetFaceNormal(faceDirection);
        Vector3 faceCenter = GetFaceCenter(faceDirection);

        Vector3 cameraForward = camera.forward;
        Vector3 cameraPosition = camera.position;

        Vector3 intersectionPoint = CalculateIntersection(faceCenter, faceNormal, cameraPosition, cameraForward);


        // Calculer la distance entre le centre de la face et le point d'intersection
        Vector3 distanceInterFace = intersectionPoint - faceCenter;


        //Debug.DrawLine(camera.transform.position, camera.transform.forward * 10, Color.green);
        //Debug.DrawLine(faceCenter, intersectionPoint, Color.red);
        Debug.DrawLine(faceCenter, faceCenter + distanceInterFace, Color.blue);









        // Utiliser l'intersection pour ajuster la taille et la position du bloc
        switch (faceDirection)
        {
            case FaceDirection.Up:
                if (transform.localScale.y + distanceInterFace.y >= 0.5f)
                {
                    transform.localScale += new Vector3(0, distanceInterFace.y, 0);
                    transform.position += new Vector3(0, distanceInterFace.y / 2, 0);
                }
                break;
            case FaceDirection.Down:
                if(transform.localScale.y - distanceInterFace.y >= 0.5f)
                {
                    transform.localScale -= new Vector3(0, distanceInterFace.y, 0);
                    transform.position += new Vector3(0, distanceInterFace.y / 2, 0);
                }
                break;
            case FaceDirection.North:
                if(transform.localScale.z + distanceInterFace.z >= 0.5f)
                {
                    transform.localScale += new Vector3(0, 0, distanceInterFace.z);
                    transform.position += new Vector3(0, 0, distanceInterFace.z / 2);
                }
                break;
            case FaceDirection.South:
                if (transform.localScale.z - distanceInterFace.z >= 0.5f)
                { 
                    transform.localScale -= new Vector3(0, 0, distanceInterFace.z);
                    transform.position += new Vector3(0, 0, distanceInterFace.z / 2);
                }
                break;
            case FaceDirection.East:
                if(transform.localScale.x + distanceInterFace.x >= 0.5f)
                {
                    transform.localScale += new Vector3(distanceInterFace.x, 0, 0);
                    transform.position += new Vector3(distanceInterFace.x / 2, 0, 0);
                }
                break;
            case FaceDirection.West:
                if(transform.localScale.x - distanceInterFace.x >= 0.5f)
                {
                    transform.localScale -= new Vector3(distanceInterFace.x, 0, 0);
                    transform.position += new Vector3(distanceInterFace.x / 2, 0, 0);
                }
                break;
        }
    }






    private void MoveBlock()
    {
        Transform cameraTransform = Player.GetMainCamera().transform;
        Transform player = Player.GetPlayer().transform;

        Vector3 offset = transform.position - cameraTransform.position;
        Vector3 playerMovement = player.position - lastPlayerPos;

        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * offset.magnitude;




        if (targetPosition.y >= 0 + transform.localScale.y / 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10);
        }
        transform.position += playerMovement;


        if (Vector3.Distance(cameraTransform.position, transform.position) < minDistanceMoving)
        {
            transform.position = cameraTransform.position + cameraTransform.forward * minDistanceMoving;
        }

    }




    private Vector3 GetFaceCenter(FaceDirection direction)
    {
        Vector3 center = transform.position;
        Vector3 halfScale = transform.localScale / 2;

        switch (direction)
        {
            case FaceDirection.Up: return center + new Vector3(0, halfScale.y, 0);
            case FaceDirection.Down: return center - new Vector3(0, halfScale.y, 0);
            case FaceDirection.North: return center + new Vector3(0, 0, halfScale.z);
            case FaceDirection.South: return center - new Vector3(0, 0, halfScale.z);
            case FaceDirection.East: return center + new Vector3(halfScale.x, 0, 0);
            case FaceDirection.West: return center - new Vector3(halfScale.x, 0, 0);
            default: return center;
        }
    }

    private Vector3 GetFaceNormal(FaceDirection direction)
    {
        switch (direction)
        {
            case FaceDirection.Up: return Vector3.up;
            case FaceDirection.Down: return Vector3.down;
            case FaceDirection.North: return Vector3.forward;
            case FaceDirection.South: return Vector3.back;
            case FaceDirection.East: return Vector3.right;
            case FaceDirection.West: return Vector3.left;
            default: return Vector3.up;
        }
    }

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
