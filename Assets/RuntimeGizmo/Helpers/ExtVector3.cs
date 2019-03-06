using System;
using UnityEngine;

namespace RuntimeGizmos
{
	public static class ExtVector3
	{
        /// <summary>
        /// 方向归一化 并返回点乘值 即夹角的弧长
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="direction"></param>
        /// <param name="normalizeParameters"></param>
        /// <returns></returns>
		public static float MagnitudeInDirection(Vector3 vector, Vector3 direction, bool normalizeParameters = true)
		{
			if(normalizeParameters) direction.Normalize();
			return Vector3.Dot(vector, direction);
		}

        /// <summary>
        /// 获得正数向量
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
		public static Vector3 Abs(this Vector3 vector)
		{
			return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
		}

        /// <summary>
        /// 是否平行
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="otherDirection"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
		public static bool IsParallel(Vector3 direction, Vector3 otherDirection, float precision = .0001f)
		{
			return Vector3.Cross(direction, otherDirection).sqrMagnitude < precision;
		}

        /// <summary>
        /// 是不是相同方向
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="otherDirection"></param>
        /// <returns></returns>
		public static bool IsInDirection(Vector3 direction, Vector3 otherDirection)
		{
			return Vector3.Dot(direction, otherDirection) > 0f;
		}
	}
}