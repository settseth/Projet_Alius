using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShapeMinigame : MonoBehaviour
{
    [System.Serializable]
    public struct ShapeConfig
    {
        public string shapeName;
        public GameObject prefab;
        public Collider targetZone;
    }

    [Header("Configuration")]
    public Transform spawnPoint;
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
            UnityEngine.Debug.LogError("Erreur : Aucune forme configurée !");
            return;
        }

        isGameActive = true;
        currentScore = 0;
        UnityEngine.Debug.Log("--- DÉBUT JEU FORMES ---");
        SpawnNextShape();
    }

    void SpawnNextShape()
    {
        if (!isGameActive) return;

        currentShapeConfig = availableShapes[UnityEngine.Random.Range(0, availableShapes.Count)];

        if (currentObjectInstance != null) Destroy(currentObjectInstance);

        currentObjectInstance = Instantiate(currentShapeConfig.prefab, spawnPoint.position, spawnPoint.rotation);

        var detector = currentObjectInstance.AddComponent<ShapeHitDetector>();
        detector.Setup(this, currentShapeConfig.targetZone);
    }

    public void OnShapeValidated(GameObject validatedObject)
    {
        if (!isGameActive) return;

        if (validatedObject == currentObjectInstance)
        {
            UnityEngine.Debug.Log("Forme correcte !");

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

    void EndGame()
    {
        isGameActive = false;

        UnityEngine.Debug.Log($"BRAVO ! Score final : {currentScore}/{shapesToWin}");

        if (currentObjectInstance != null) Destroy(currentObjectInstance);

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.EndShapePhase();
        }
        else
        {
            // CORRECTION ICI : On précise UnityEngine.Debug
            UnityEngine.Debug.LogError("Impossible de trouver le GameManager !");
        }
    }
}

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