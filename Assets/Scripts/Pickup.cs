using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    public virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public abstract void PlaySound();

    public void TurnOff()
    {
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
    }

    public void TurnOn()
    {
        boxCollider.enabled = true;
        spriteRenderer.enabled = true;
    }

    public void ConsumePickup()
    {
        //player invneotry might call this later, maybe this should be abstract as all pickups are consumed in different ways
        //coins to buys stuff, decrementing coins
        //potions to add health, incerasing health
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
        {
            Unit player = collision.GetComponent<Player>();
            PickupItemCommand command = new PickupItemCommand(ref player, this);
            if(command.Execute())
            {
                player.commands.Push(command);
                PlaySound();
            }

        }
    }

}

