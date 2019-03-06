using System;
using UnityEngine;

namespace RuntimeGizmos
{
    /// <summary>
    /// 相交点
    /// </summary>
	public struct IntersectPoints
	{
		public Vector3 first;
        //第一
		public Vector3 second;//第二

        //构造函数
		public IntersectPoints(Vector3 first, Vector3 second)
		{
			this.first = first;
			this.second = second;
		}
	}
}