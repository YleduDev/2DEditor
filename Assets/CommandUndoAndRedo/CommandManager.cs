using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopEngine;
using System;

public class CommandManager :ICommandManager
{ 
    /*
(1)当执行完任何一个command后，reverseUndo堆栈清空
(2)当执行完一个不可撤销的command后，undo堆栈清空
(3)当执行完一个可撤销的command后，将其压入undo堆栈
(4)当撤销一个command后，将其转移到reverseUndo堆栈
    */
    private static CommandManager commandManger;
    public static CommandManager CommandMan
    {
       get
        {
            if (commandManger == null)
            {
                commandManger = new CommandManager();
            }
            return commandManger;
        }
    }

    private Stack undoStack = new Stack();
    private Stack reverseStack = new Stack();

    public event CbSimpleBool UndoStateChanged;
    public event CbSimpleBool ReverseUndoStateChanged;

    public CommandManager()
    {
        this.UndoStateChanged += new CbSimpleBool(CommandManager_UndoStateChanged);
        this.ReverseUndoStateChanged += new CbSimpleBool(CommandManager_UndoStateChanged);
    }

    private void CommandManager_UndoStateChanged(bool val)
    {

    }

    #region ICommandManager 成员

    public void ExcuteCommand(ICommand command)
    {
        command.Excute();
        this.reverseStack.Clear();

        if (command is IBackableCommand)
        {
            this.undoStack.Push(command);
        }
        else
        {
            this.undoStack.Clear();
        }

        this.UndoStateChanged(this.undoStack.Count > 0);
    }

    public void Undo()
    {
        if (this.undoStack.Count <= 0) return;
        IBackableCommand command = (IBackableCommand)this.undoStack.Pop();
        if (command == null)
        {
            return;
        }

        command.Undo();
        this.reverseStack.Push(command);
        
        this.ReverseUndoStateChanged(this.reverseStack.Count > 0);
    }

    public void ReverseUndo()
    {
        if (this.reverseStack.Count <= 0) return;

        IBackableCommand command = (IBackableCommand)this.reverseStack.Pop();
        if (command == null)
        {
            return;
        }

        command.Excute();
        this.undoStack.Push(command);

        this.UndoStateChanged(this.undoStack.Count > 0);
        this.ReverseUndoStateChanged(this.reverseStack.Count > 0);
    }

    public void AddCammand( Action Excute,Action back)
    {       
            BackableCommandRealize backRe = new BackableCommandRealize
            {
                ExcuteEvent = Excute,

                //撤销
                BackablexcuteEvent = back
            };

            //执行
            CommandMan.ExcuteCommand(backRe);
    }

    #endregion
}
