using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/Card")]
public class Card : ScriptableObject
{
    [Header("Information")]
    public string nom;
    public int force;
    public Sprite image;
    public string description;


    [Header("Entree en jeu")]
    // Voyance revele les liens d'une carte en entrant en jeu
    public bool aVoyance;
    // Cupidon lie 2 carte en entrant en jeu
    public bool aCupidon;
    // Amoureux se lie a une carte en entrant en jeu
    public bool aAmoureux;
    // Vol transfert tous les liens d'une carte sur celle-ci en entrant en jeu
    public bool aVol;



    [Header("Attaque")]
    // Lien de sang cree un lien entre 1 carte de chaque camp en attaquant
    public bool aLienDeSang;
    // Sacage bris un lien aleatoire d'une carte en attaquant
    public bool aSacage;


    [Header("Mort")]
    // Post Mortem ajoute un lien entre deux carte en mourant
    public bool aPostMortem;
    // Brise Lien detruit tous les liens d'une carte en mourant
    public bool aBriseLien;


    [Header("Autres")]
    // Un pacifique ne peut pas attaquer
    public bool aPacifisme;
    // Une brute ne peut pas bloquer
    public bool aBrutal;
}
