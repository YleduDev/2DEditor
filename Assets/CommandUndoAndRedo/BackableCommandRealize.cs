using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackableCommandRealize : IBackableCommand
{
    public Action ExcuteEvent;

    public Action BackablexcuteEvent;
    public void Excute()
    {
        ExcuteEvent?.Invoke();
    }

    public void Undo()
    {
        BackablexcuteEvent?.Invoke();
    }
}
