using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Pickup
{
    private AudioSource _audioSource;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlaySound()
    {
        _audioSource.Play();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //}
}
