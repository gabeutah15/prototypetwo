using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitInput : MonoBehaviour
{
    protected Unit thisUnit;
    //protected Stack<Command> commands;//this should actually live on unit, as it should be able to add any kind of command
    /// <summary>
    /// actually i want a generic unitinput parent that stores theinput but player nad ai input sub classes
    /// the formertakes keyboard input
    /// the latter take just like whatever ai determined input
    /// </summary>

    // Start is called before the first frame update
    public virtual void Start()
    {
        thisUnit = GetComponent<Unit>();//can i set this in an abstract class?
        //commands = new Stack<Command>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract ref Command HandleInput(ref bool choseMove);
}
