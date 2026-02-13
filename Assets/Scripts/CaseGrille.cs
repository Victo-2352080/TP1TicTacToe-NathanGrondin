using System.Runtime.CompilerServices;
using UnityEngine;

public class CaseGrille : MonoBehaviour
{
    [SerializeField]
    private int numeroCarre;
    [SerializeField]
    private GameObject prefabX;
    [SerializeField]
    private GameObject prefabO;

    private GameObject pieceActuelle = null;

    /// <summary>
    /// Lorsque le joueur souhaite placer sa piece sur cette case
    /// </summary>
    public void PlacerPiece()
    {
        if (pieceActuelle != null)
        {
            return;
        }

        if (GestionJeu.Instance.JeuFini())
        {
            return;
        }

        GameObject prefab = GestionJeu.Instance.EstTourX() ? prefabX : prefabO;

        // pour qu'il soit un petit peu au dessus
        Vector3 positionPiece = transform.position + new Vector3(0, 0.05f, 0);
        pieceActuelle = Instantiate(prefab, positionPiece, Quaternion.identity);

        GestionJeu.Instance.CoupJoue(numeroCarre);
    }

    // nettoyer cette case pour recommencer une parti
    public void ViderCase()
    {
        if (pieceActuelle != null)
        {
            Destroy(pieceActuelle);
            pieceActuelle = null;
        }
    }
}