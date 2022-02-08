using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    //don't need a destructor as c#
    public abstract bool Execute();
    public abstract bool Undo();

}
