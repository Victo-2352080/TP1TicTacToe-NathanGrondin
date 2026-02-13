using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementGrille : MonoBehaviour
{
    [SerializeField]
    private ARRaycastManager rayManager;
    [SerializeField]
    private ARAnchorManager anchorManager;
    [SerializeField]
    private GameObject grillePrefab;
    [SerializeField]
    private Camera Cam;

    private GameObject grilleInstance;
    private ARAnchor grilleAnchor;
    private InputSystem_Actions playerControl;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        playerControl = new InputSystem_Actions();

        if (Cam == null)
        {
            Cam = Camera.main;
        }
    }

    private void OnEnable()
    {
        playerControl.Enable();
        playerControl.Player.Tap.performed += Clique;
    }

    private void OnDisable()
    {
        playerControl.Disable();
        playerControl.Player.Tap.performed -= Clique;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    private void Clique(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = playerControl.Player.PointPosition.ReadValue<Vector2>();

        // Si la grille n'est pas placée, placer la grille
        if (grilleInstance == null)
        {
            if (rayManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                ARPlane plane = hits[0].trackable as ARPlane;
                if (plane != null)
                {
                    grilleAnchor = anchorManager.AttachAnchor(plane, hitPose);

                    if (grilleAnchor != null)
                    {
                        grilleInstance = Instantiate(grillePrefab, hitPose.position, hitPose.rotation);
                        grilleInstance.transform.parent = grilleAnchor.transform;
                    }
                }
            }
        }
        // Si la grille est placée, détecter les cases
        else
        {
            Ray ray = Cam.ScreenPointToRay(touchPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CaseGrille caseGrille = hit.collider.GetComponent<CaseGrille>();

                if (caseGrille != null)
                {
                    caseGrille.PlacerPiece();
                }
            }
        }
    }

    /// <summary>
    /// Détruit le grille et l'ancre pour la replacer
    /// </summary>
    public void ResetGrille()
    {
        if (grilleInstance != null)
        {
            Destroy(grilleInstance);
        }
        if (grilleAnchor != null)
        {
            Destroy(grilleAnchor.gameObject);
        }
        grilleInstance = null;
        grilleAnchor = null;
    }


    /// <summary>
    /// Pour connaitre si la grille est placé
    /// </summary>
    /// <returns>Vrai si </returns>
    public bool GrilleEstPlacee()
    {
        return grilleInstance != null;
    }
}