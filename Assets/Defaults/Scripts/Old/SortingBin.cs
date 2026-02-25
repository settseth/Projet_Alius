using UnityEngine;

public class SortingBin : MonoBehaviour
{
    public string acceptedCategory; 
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        FolderData folder = other.GetComponent<FolderData>();

        if (folder != null)
        {
            if (folder.category == acceptedCategory)
            {
                gameManager.AddPoint();
                Destroy(other.gameObject);
            }
        }
    }
}