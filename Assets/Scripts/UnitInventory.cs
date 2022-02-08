using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInventory : MonoBehaviour
{
    [HideInInspector]
    public List<Pickup> Pickups;
    public int NumCoins;
    [SerializeField]
    Text CointText;

    // Start is called before the first frame update
    void Start()
    {
        Pickups = new List<Pickup>();
        CointText.text = NumCoins.ToString();
    }

    public bool AddPickup(Pickup pickup)
    {
        Pickups.Add(pickup);//later check if maxed out on this pickup type or somethinging

        switch(pickup)
        {
            case Coin c:
                NumCoins++;
                CointText.text = NumCoins.ToString();
                //Debug.Log("picked up coin");
                break;
            default:
                break;
        }

        return true;//maybe some conditions later could make this fail, like if at max
    }

    public bool RemovePickup(Pickup pickup)//needs to be a ref? this for placing it back on the ground maybe?
    {
        if(Pickups.Remove(pickup))
        {
            switch(pickup)
            {
                case Coin c:
                    NumCoins--;
                    CointText.text = NumCoins.ToString();
                    //Debug.Log("picked up coin");
                    break;
                default:
                    break;
            }
            return true;
        }

        return false;
    }

    public bool PayCoin()
    {
        if(NumCoins > 0)
        {
            for(int i = 0; i < Pickups.Count; i++)
            {
                if((Coin)Pickups[i])
                {
                    Pickups.RemoveAt(i);//remove some coin, but how prevent it from being turned back on with undos?
                    //so this should prevent an undo from turning a coin back on i think?
                }
            }
            NumCoins--;
            CointText.text = NumCoins.ToString();
            return true;
        }
        return false;
    }

}
