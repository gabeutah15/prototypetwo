using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    AIInput aiInput;
    Animator animator;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        aiInput = (AIInput)unitInput;
        aiInput.OnDied += AiInput_OnDied;
        animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

    }

    private void AiInput_OnDied()
    {
        data.NumEnemiesRemaining--;
        animator.SetBool("Dead", true);
        _audioSource.Play();
        if(data.NumEnemiesRemaining <= 0)
        {
            data.EndGame();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
}
