using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//委托
public delegate void CbSimpleBool(bool b);

//一个堆栈来记录所有已执行的可撤销命令，为了实现反撤销的功能
//，同样，我们也需要一个堆栈来记录所有已撤销的命令
public interface ICommandManager
{
    void ExcuteCommand(ICommand command);
    void Undo();
    void ReverseUndo();//反撤销

    //以下事件可用于控制撤销与反撤销图标的启用
    event CbSimpleBool UndoStateChanged; //bool参数表明当前是否有可撤销的操作
    event CbSimpleBool ReverseUndoStateChanged; //bool参数表明当前是否有可反撤销的操作
}
