using System.Collections;
using System.Collections.Generic;
using System;

public class UnBackableCommandRealize: IUnBackableCommand
{
    public Action ExcuteEvent;
    
    public void Excute()
    {
        if(ExcuteEvent!=null)
        ExcuteEvent();
    }
    
}
