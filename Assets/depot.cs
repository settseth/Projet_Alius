using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class depot : MonoBehaviour
{
    public String name;
    public TextMeshProUGUI message;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log(name);
        if(collision.gameObject.name == name)
        {
            Debug.Log("yipi");
            message.text = "Action accomplie";
            message.gameObject.SetActive(true);
            StartCoroutine(CacherApresDelay(4f));
            Destroy(collision.gameObject);
        }
        else
        {
            message.text = "Faute commise";
            message.gameObject.SetActive(true);
            StartCoroutine(CacherApresDelay(4f));
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator CacherApresDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        message.gameObject.SetActive(false);
        
    }

}
