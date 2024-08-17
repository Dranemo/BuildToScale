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
    float minDistanceMoving = 0;
    Vector3 lastPlayerPos = Vector3.zero;
    Vector3 playerRotation = Vector3.zero;


    private FaceDirection GetFaceDirection(RaycastHit hit)
    {
        if (hit.normal == transform.up)
        {
            return FaceDirection.Up;
        }
        else if (hit.normal == -transform.up)
        {
            return FaceDirection.Down;
        }
        else if (hit.normal == transform.forward)
        {
            return FaceDirection.North;
        }
        else if (hit.normal == -transform.forward)
        {
            return FaceDirection.South;
        }
        else if (hit.normal == transform.right)
        {
            return FaceDirection.East;
        }
        else if (hit.normal == -transform.right)
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
            MoveBlock();
            lastPlayerPos = Player.GetPlayer().transform.position;
            playerRotation = Player.GetPlayer().transform.eulerAngles;
        }
        else if (rescaling) // Rouge
        {
            Rescale();
            lastPlayerPos = Player.GetPlayer().transform.position;
        }
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

        // MoveTowards pour déplacer l'objet
        if (targetPosition.y >= 0 + transform.localScale.y / 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10);
        }

        // Ajouter le mouvement du joueur
        transform.position += playerMovement;
        // Ajouter la rotation du joueur
        transform.eulerAngles -= PlayerRotationDiff;

        // Si le bloc est trop proche de la caméra, le déplacer
        if (Vector3.Distance(cameraTransform.position, transform.position) < minDistanceMoving)
        {
            transform.position = cameraTransform.position + cameraTransform.forward * minDistanceMoving;
        }

    }




    // -------------------------------------------------------------------------------- Fonctions Rescale Block -------------------------------------------------------------------------------- //
    // Principal Fonction
    // Redimensionner le bloc en fonction de la position de la caméra et du joueur
    private void Rescale()
    {
        Transform player = Player.GetPlayer().transform;
        Transform camera = Player.GetMainCamera().transform;

        Vector3 playerMovement = player.position - lastPlayerPos;

        Vector3 faceNormal = GetFaceNormal(faceDirection);
        Vector3 faceCenter = GetFaceCenter(faceDirection);

        Vector3 cameraForward = camera.forward;
        Vector3 cameraPosition = camera.position;

        Vector3 intersectionPoint = CalculateIntersection(faceCenter, faceNormal, cameraPosition, cameraForward);


        Vector3 distanceInterFace = intersectionPoint - faceCenter;


        

        Vector3 lastPosition = transform.position;
        Vector3 lastScale = transform.localScale;

        Debug.DrawLine(faceCenter, faceCenter + distanceInterFace, Color.blue);

        
            switch (faceDirection)
            {
                case FaceDirection.Up:
                    if (transform.localScale.y + distanceInterFace.y >= 0.5f && transform.localScale.y + distanceInterFace.y <= 10)
                    {
                        transform.localScale += new Vector3(0, distanceInterFace.y, 0);
                        transform.position += new Vector3(0, distanceInterFace.y / 2, 0);
                    }
                    break;
                case FaceDirection.Down:
                    if (transform.localScale.y - distanceInterFace.y >= 0.5f && transform.localScale.y - distanceInterFace.y <= 10)
                    {
                        transform.localScale -= new Vector3(0, distanceInterFace.y, 0);
                        transform.position += new Vector3(0, distanceInterFace.y / 2, 0);
                    }
                    break;
                case FaceDirection.North:
                    if (transform.localScale.z + distanceInterFace.z >= 0.5f && transform.localScale.z + distanceInterFace.z <= 10)
                    {
                        transform.localScale += new Vector3(0, 0, distanceInterFace.z);
                        transform.position += new Vector3(0, 0, distanceInterFace.z / 2);
                    }
                    break;
                case FaceDirection.South:
                    if (transform.localScale.z - distanceInterFace.z >= 0.5f && transform.localScale.z - distanceInterFace.z <= 10)
                    {
                        transform.localScale -= new Vector3(0, 0, distanceInterFace.z);
                        transform.position += new Vector3(0, 0, distanceInterFace.z / 2);
                    }
                    break;
                case FaceDirection.East:
                    if (transform.localScale.x + distanceInterFace.x >= 0.5f && transform.localScale.x + distanceInterFace.x <= 10)
                    {
                        transform.localScale += new Vector3(distanceInterFace.x, 0, 0);
                        transform.position += new Vector3(distanceInterFace.x / 2, 0, 0);
                    }
                    break;
                case FaceDirection.West:
                    if (transform.localScale.x - distanceInterFace.x >= 0.5f && transform.localScale.x - distanceInterFace.x <= 10)
                    {
                        transform.localScale -= new Vector3(distanceInterFace.x, 0, 0);
                        transform.position += new Vector3(distanceInterFace.x / 2, 0, 0);
                    }
                    break;
        }




        Vector3 closestAngle = GetAngles(faceDirection)[0];
        foreach (Vector3 angle in GetAngles(faceDirection))
        {
            Debug.DrawLine(faceCenter, angle, Color.red);

            if (Vector3.Distance(camera.position, angle) <= Vector3.Distance(camera.position, closestAngle))
            {
                closestAngle = angle;
            }
        }

        if (Vector3.Distance(camera.position, closestAngle) < 2f)
        {
            transform.position = lastPosition;
            transform.localScale = lastScale;
        }

    }





    // Recuperer le centre de la face du bloc en fonction de la direction
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

    private Vector3[] GetAngles(FaceDirection direction)
    {
        Vector3[] angles = new Vector3[4];
        Vector3 faceCenter = GetFaceCenter(direction);


        switch (direction)
        {
            case FaceDirection.Up:
                angles[0] = faceCenter + new Vector3(-transform.localScale.x / 2, 0, -transform.localScale.z / 2);
                angles[1] = faceCenter + new Vector3(transform.localScale.x / 2, 0, -transform.localScale.z / 2);
                angles[2] = faceCenter + new Vector3(transform.localScale.x / 2, 0, transform.localScale.z / 2);
                angles[3] = faceCenter + new Vector3(-transform.localScale.x / 2, 0, transform.localScale.z / 2);
                break;
            case FaceDirection.Down:
                angles[0] = faceCenter + new Vector3(-transform.localScale.x / 2, 0, -transform.localScale.z / 2);
                angles[1] = faceCenter + new Vector3(transform.localScale.x / 2, 0, -transform.localScale.z / 2);
                angles[2] = faceCenter + new Vector3(transform.localScale.x / 2, 0, transform.localScale.z / 2);
                angles[3] = faceCenter + new Vector3(-transform.localScale.x / 2, 0, transform.localScale.z / 2);
                break;
            case FaceDirection.North:
                angles[0] = faceCenter + new Vector3(-transform.localScale.x / 2, -transform.localScale.y / 2, 0);
                angles[1] = faceCenter + new Vector3(transform.localScale.x / 2, -transform.localScale.y / 2, 0);
                angles[2] = faceCenter + new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, 0);
                angles[3] = faceCenter + new Vector3(-transform.localScale.x / 2, transform.localScale.y / 2, 0);
                break;
            case FaceDirection.South:
                angles[0] = faceCenter + new Vector3(-transform.localScale.x / 2, -transform.localScale.y / 2, 0);
                angles[1] = faceCenter + new Vector3(transform.localScale.x / 2, -transform.localScale.y / 2, 0);
                angles[2] = faceCenter + new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, 0);
                angles[3] = faceCenter + new Vector3(-transform.localScale.x / 2, transform.localScale.y / 2, 0);
                break;


        }

        return angles;
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
