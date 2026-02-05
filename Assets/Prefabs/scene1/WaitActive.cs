using UnityEngine;

public class WaitActive : MonoBehaviour
{
    private GameObject waitObject;
    private GameObject waitObject2;

    private void Awake()
    {
        // Cherche "Wait" dans tous les enfants
        waitObject = FindInChildren(transform, "Wait");

        if (waitObject != null)
            waitObject.SetActive(false);
        else
            Debug.LogWarning("Wait introuvable dans les enfants de " + name);


        waitObject2 = FindInChildren(transform, "Wait2");

        if (waitObject2 != null)
            waitObject2.SetActive(false);

    }

    public void ActivateWait()
    {
        if (waitObject != null)
            waitObject.SetActive(true);
    }

    public void DesactivateWait()
    {
        if (waitObject != null)
            waitObject.SetActive(false);

        if (waitObject2 != null)
            waitObject2.SetActive(true);
    }

    private GameObject FindInChildren(Transform parent, string name)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
                return child.gameObject;
        }
        return null;
    }
}
