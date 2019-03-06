using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBackableCommand : ICommand
{
    //可撤销的命令也是一种命令，所以从ICommand继承：
    void Undo();
}
