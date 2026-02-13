using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InterfaceJeu : MonoBehaviour
{

    [SerializeField] private PlacementGrille placementGrille;

    [SerializeField] private Button btnReplacerGrille;
    [SerializeField] private Button btnNouvellePartie;

    [SerializeField] private TextMeshProUGUI txtTourActuel;
    [SerializeField] private TextMeshProUGUI txtResultat;

    [SerializeField] private GameObject panelResultat;

    private void Start()
    {
        if (btnReplacerGrille != null)
            btnReplacerGrille.onClick.AddListener(ReplacerGrille);

        if (btnNouvellePartie != null)
            btnNouvellePartie.onClick.AddListener(NouvellePartie);

        if (GestionJeu.Instance != null)
        {
            GestionJeu.Instance.OnVictoire.AddListener(AfficherVictoire);
            GestionJeu.Instance.OnEgalite.AddListener(AfficherEgalite);
            GestionJeu.Instance.OnChangementTour.AddListener(MettreAJourTour);
        }

        if (panelResultat != null)
            panelResultat.SetActive(false);
    }

    private void Update()
    {
        // Vérifier si la grille est placée
        if (placementGrille != null)
        {
            if (placementGrille.GrilleEstPlacee())
            {
                // Afficher le tour actuel
                MettreAJourTour(GestionJeu.Instance != null && GestionJeu.Instance.EstTourX());
            }
            else
            {
                // Afficher le message de scan
                if (txtTourActuel != null)
                {
                    txtTourActuel.text = "Scannez une surface... puis appuis dessus";
                }
            }
        }
    }

    /// <summary>
    /// Replace la grille lorsque'on appuis sur le bouton 
    /// </summary>
    private void ReplacerGrille()
    {
        if (placementGrille != null)
        {
            placementGrille.ResetGrille();
        }
        NouvellePartie();
    }

    /// <summary>
    /// Lance une nouvelle game
    /// </summary>
    private void NouvellePartie()
    {
        if (GestionJeu.Instance != null)
        {
            GestionJeu.Instance.NouvellePartie();
        }

        if (panelResultat != null)
            panelResultat.SetActive(false);
    }

    /// <summary>
    /// met le bon gagnant et affiche le panel
    /// </summary>
    /// <param name="gagnant">Le gagnant O ou X</param>
    private void AfficherVictoire(string gagnant)
    {
        if (txtResultat != null)
        {
            txtResultat.text = $"Joueur {gagnant} gagne !";
        }

        if (panelResultat != null)
            panelResultat.SetActive(true);
    }

    private void AfficherEgalite()
    {
        if (txtResultat != null)
        {
            txtResultat.text = "Égalité !";
        }

        if (panelResultat != null)
            panelResultat.SetActive(true);
    }

    private void MettreAJourTour(bool tourX)
    {
        if (txtTourActuel != null)
        {
            txtTourActuel.text = tourX ? "Tour : X" : "Tour : O";
        }
    }

    private void OnDestroy()
    {
        if (btnReplacerGrille != null)
            btnReplacerGrille.onClick.RemoveListener(ReplacerGrille);

        if (btnNouvellePartie != null)
            btnNouvellePartie.onClick.RemoveListener(NouvellePartie);

        if (GestionJeu.Instance != null)
        {
            GestionJeu.Instance.OnVictoire.RemoveListener(AfficherVictoire);
            GestionJeu.Instance.OnEgalite.RemoveListener(AfficherEgalite);
            GestionJeu.Instance.OnChangementTour.RemoveListener(MettreAJourTour);
        }
    }
}