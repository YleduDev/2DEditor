using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TDE
{
    public enum ScaleCenter
    {
        LeftUp,
        LeftDown,
        RightUp,
        RightDown
    }
    
    public enum LinePointType
    {
       Origin,End
    }

    public enum LineShapeType
    {
        Straight,//÷±œﬂ
        Broken,//’€œﬂ
        Curve
    }
    public class TDE_Enum
    {
    }

    public enum LineBeginShape
    {
        BeginLine,
        BeginArrows
    }
    public enum LineEndShape
    {
        EndLine,
        EndArrows
    }
    public enum MessageState
    {
        NORMAL,
        ERROR,
        WARNING
    }
    public enum AssetKindType
    {
        device,
        probe
    }
    public enum AssetCheckType
    {
        name,
        caption
    }
}