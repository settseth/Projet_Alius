using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI message;
    
    public void Start_Tutorial()
    {
        message.text = "Bienvenu dans le programme de réinsertion";
    }
    
}
