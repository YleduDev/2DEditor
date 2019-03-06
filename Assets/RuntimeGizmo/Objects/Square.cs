using System;
using UnityEngine;

namespace RuntimeGizmos
{
    /// <summary>
    /// 矩形
    /// </summary>
	public struct Square
	{
		public Vector3 bottomLeft;//左下
		public Vector3 bottomRight;//右下
		public Vector3 topLeft;//左上
		public Vector3 topRight;//右上

		public Vector3 this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return this.bottomLeft;
					case 1:
						return this.bottomRight;
					case 2:
						return this.topLeft;
					case 3:
						return this.topRight;
					case 4:
						return this.bottomLeft; //so we wrap around back to start
					default:
						return Vector3.zero;
				}
			}
		}
	}
}