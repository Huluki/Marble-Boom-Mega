using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public Collider collider;
    public MeshRenderer meshRenderer;
    [SerializeField] static int num=0;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //if instance then timer

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collider.enabled = false;
            meshRenderer.enabled = false;
            StartCoroutine(Wait(5));
        }
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        collider.enabled = true;
        meshRenderer.enabled = true;
    }
    
}
