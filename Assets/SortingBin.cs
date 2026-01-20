using UnityEngine;

public class SortingBin : MonoBehaviour
{
    public string acceptedCategory; // La catégorie que cette boîte accepte
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        FolderData folder = other.GetComponent<FolderData>();

        if (folder != null)
        {
            if (folder.category == acceptedCategory)
            {
                gameManager.AddPoint();
                // Détruire le dossier ou le déplacer pour libérer de l'espace
                Destroy(other.gameObject);
            }
        }
    }
}