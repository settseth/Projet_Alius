using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class ShapeMinigame : MonoBehaviour
{
    [System.Serializable]
    public struct ShapeConfig
    {
        public string shapeName;
        public GameObject prefab;
        public Collider targetZone;     // Le Collider de la zone cible (Cocher "Is Trigger")
    }

    [Header("Configuration")]
    public Transform spawnPoint;
    // On a enlevé maxTime ici car c'est le GameManager qui décide
    public int shapesToWin = 5;
    public List<ShapeConfig> availableShapes;

    private int currentScore = 0;
    private bool isGameActive = false;
    private ShapeConfig currentShapeConfig;
    private GameObject currentObjectInstance;

    public void StartGame()
    {
        if (availableShapes.Count == 0)
        {
            UnityEngine.Debug.LogError("Erreur : Aucune forme configurée dans ShapeMinigame !");
            return;
        }

        isGameActive = true;
        currentScore = 0;

        UnityEngine.Debug.Log("--- DÉBUT JEU FORMES (Mode Trigger - Sans Timer Interne) ---");

        SpawnNextShape();
        // On ne lance plus de Coroutine de timer ici
    }

    void SpawnNextShape()
    {
        if (!isGameActive) return;

        // 1. Choisir une forme au hasard
        currentShapeConfig = availableShapes[UnityEngine.Random.Range(0, availableShapes.Count)];

        // 2. Nettoyer l'ancien objet s'il existe
        if (currentObjectInstance != null) Destroy(currentObjectInstance);

        // 3. Faire apparaître l'objet
        currentObjectInstance = Instantiate(currentShapeConfig.prefab, spawnPoint.position, spawnPoint.rotation);

        // 4. Ajouter le détecteur
        var detector = currentObjectInstance.AddComponent<ShapeHitDetector>();
        detector.Setup(this, currentShapeConfig.targetZone);
    }

    public void OnShapeValidated(GameObject validatedObject)
    {
        if (!isGameActive) return;

        if (validatedObject == currentObjectInstance)
        {
            UnityEngine.Debug.Log("Forme correcte (Zone Trigger atteinte) !");

            Destroy(validatedObject, 0.2f);
            currentObjectInstance = null;

            currentScore++;
            CheckWinCondition();
        }
    }

    void CheckWinCondition()
    {
        if (currentScore >= shapesToWin)
        {
            EndGame();
        }
        else
        {
            Invoke("SpawnNextShape", 1.0f);
        }
    }

    // Fonction appelée uniquement quand le joueur a fini toutes les formes
    void EndGame()
    {
        isGameActive = false;

        UnityEngine.Debug.Log("--- FIN DU JEU DE FORMES (Toutes les formes triées) ---");
        UnityEngine.Debug.Log($"BRAVO ! Score final : {currentScore}/{shapesToWin}");

        if (currentObjectInstance != null) Destroy(currentObjectInstance);
    }
}

// --- PETIT SCRIPT UTILITAIRE ---
public class ShapeHitDetector : MonoBehaviour
{
    private ShapeMinigame manager;
    private Collider targetTrigger;
    private bool hasTriggered = false;

    public void Setup(ShapeMinigame mgr, Collider target)
    {
        manager = mgr;
        targetTrigger = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other == targetTrigger)
        {
            hasTriggered = true;
            manager.OnShapeValidated(this.gameObject);
        }
    }
}