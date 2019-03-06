using System;
using UnityEngine;

namespace RuntimeGizmos
{
    //所有轴状态
	public struct AxisInfo
	{
        //结束
		public Vector3 xAxisEnd;
		public Vector3 yAxisEnd;
		public Vector3 zAxisEnd;
        //方向
		public Vector3 xDirection;
		public Vector3 yDirection;
		public Vector3 zDirection;

        /// <summary>
        /// 设置轴的方向 和轴的长度大小
        /// </summary>
        /// <param name="target"></param>
        /// <param name="handleLength"></param>
        /// <param name="space"></param>
		public void Set(Transform target, float handleLength, TransformSpace space)
		{
			if(space == TransformSpace.Global)
			{
				xDirection = Vector3.right;
				yDirection = Vector3.up;
				zDirection = Vector3.forward;
			}
			else if(space == TransformSpace.Local)
			{
				xDirection = target.right;
				yDirection = target.up;
				zDirection = target.forward;
			}

			xAxisEnd = target.position + (xDirection * handleLength);
			yAxisEnd = target.position + (yDirection * handleLength);
			zAxisEnd = target.position + (zDirection * handleLength);
		}
	}
}