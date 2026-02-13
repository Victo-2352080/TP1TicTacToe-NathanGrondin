using UnityEngine;
using UnityEngine.Events;

public class GestionJeu : MonoBehaviour
{
    public static GestionJeu Instance { get; private set; }

    public UnityEvent<string> OnVictoire;
    public UnityEvent OnEgalite;
    public UnityEvent<bool> OnChangementTour;

    private int[,] grille = new int[3, 3]; // ligne proposé par ClaudeAI
    private bool tourJoueurX = true;
    private bool partieTerminee = false;
    private int nombreCoups = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitialiserGrille();
    }

    // Fonction généré partielement par ClaudeAI
    private void InitialiserGrille()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                grille[i, j] = 0;
            }
        }
        tourJoueurX = true;
        partieTerminee = false;
        nombreCoups = 0;

        OnChangementTour?.Invoke(tourJoueurX);
    }

    // Fonction généré par ClaudeAI
    public bool CaseLibreParCarre(int numeroCarre)
    {
        int ligne = numeroCarre / 3;
        int colonne = numeroCarre % 3;
        return grille[ligne, colonne] == 0;
    }

    public bool EstTourX()
    {
        return tourJoueurX;
    }

    public bool JeuFini()
    {
        return partieTerminee;
    }

    public void CoupJoue(int numeroCarre)
    {
        // Repris de la fonction par ClaudeAi plus haut
        int ligne = numeroCarre / 3;
        int colonne = numeroCarre % 3;

        grille[ligne, colonne] = tourJoueurX ? 1 : 2;
        nombreCoups++;

        if (VerifierVictoire())
        {
            partieTerminee = true;
            string gagnant = tourJoueurX ? "X" : "O";
            Debug.Log($"Joueur {gagnant} gagne !");
            OnVictoire?.Invoke(gagnant);
            return;
        }

        if (nombreCoups >= 9)
        {
            partieTerminee = true;
            Debug.Log("Égalité !");
            OnEgalite?.Invoke();
            return;
        }

        tourJoueurX = !tourJoueurX;
        OnChangementTour?.Invoke(tourJoueurX);
    }

    private bool VerifierVictoire()
    {
        // CLaudeAI pour la vérification de la grille

        int symbole = tourJoueurX ? 1 : 2;

        for (int i = 0; i < 3; i++)
        {
            if (grille[i, 0] == symbole && grille[i, 1] == symbole && grille[i, 2] == symbole)
                return true;
        }

        for (int j = 0; j < 3; j++)
        {
            if (grille[0, j] == symbole && grille[1, j] == symbole && grille[2, j] == symbole)
                return true;
        }

        if (grille[0, 0] == symbole && grille[1, 1] == symbole && grille[2, 2] == symbole)
            return true;

        if (grille[0, 2] == symbole && grille[1, 1] == symbole && grille[2, 0] == symbole)
            return true;

        return false;
    }

    public void NouvellePartie()
    {
        // généré par Claude
        CaseGrille[] cases = FindObjectsOfType<CaseGrille>();
        foreach (CaseGrille caseGrille in cases)
        {
            caseGrille.ViderCase();
        }

        InitialiserGrille();
        Debug.Log("Nouvelle partie commencée !");
    }
}