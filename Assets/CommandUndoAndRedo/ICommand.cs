using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    //Excute表示执行该ICommand引用的目标的所包含的动作（或动作序列）。
    void Excute();
}
