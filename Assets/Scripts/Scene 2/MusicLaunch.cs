using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLaunch : MonoBehaviour
{
    [SerializeField] private GameObject MusicCollider;
    [SerializeField] private AudioSource Music;

    [SerializeField] private Transform FlyPoint;

    [SerializeField] private float smooth = 1f;

    [SerializeField] private GameObject InvisiblePlane;
    Vector3 currentVelocity;

    void OnTriggerEnter()
    {
        Music.Play();
        MusicCollider.SetActive(false);
        StartCoroutine(Fly());

    }

    IEnumerator Fly()
    {
        yield return new WaitForSeconds(13);
        Vector3 dist = FlyPoint.position - this.transform.position;
        CharacterController control = GetComponent<CharacterController>();
        control.enabled = false;
        while (Vector3.Distance(transform.position, FlyPoint.position) > 0.1f)
        {
            this.transform.position = Vector3.SmoothDamp(transform.position, FlyPoint.position, ref currentVelocity, smooth);
            dist = FlyPoint.position - this.transform.position;
            yield return null;
        }
        InvisiblePlane.SetActive(true);
        control.enabled = true;
    }

}
