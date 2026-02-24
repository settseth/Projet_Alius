using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySong : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource son;
    
    void OnTriggerEnter()
    {
        Debug.Log("heyyy");
        son.Play();

    }


}
