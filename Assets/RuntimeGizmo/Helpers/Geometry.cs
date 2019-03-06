using System;
using UnityEngine;

namespace RuntimeGizmos
{
    /// <summary>
    /// 几何
    /// </summary>
	public static class Geometry
	{
        /// <summary>
        /// 线面距离
        /// </summary>
        /// <param name="linePoint"></param>
        /// <param name="lineVec"></param>
        /// <param name="planePoint"></param>
        /// <param name="planeNormal"></param>
        /// <returns></returns>
		public static float LinePlaneDistance(Vector3 linePoint, Vector3 lineVec, Vector3 planePoint, Vector3 planeNormal)
		{
            //计算线点与线平面相交点之间的距离
            float dotNumerator = Vector3.Dot((planePoint - linePoint), planeNormal);
			float dotDenominator = Vector3.Dot(lineVec, planeNormal);

            //直线和平面是不平行的。
            if (dotDenominator != 0f)
			{
				return dotNumerator / dotDenominator;
			}
			
			return 0;
		}

        //注意这条线是无限的，这不是一条线段平面相交。
        public static Vector3 LinePlaneIntersect(Vector3 linePoint, Vector3 lineVec, Vector3 planePoint, Vector3 planeNormal)
		{
			float distance = LinePlaneDistance(linePoint, lineVec, planePoint, planeNormal);

            //直线和平面是不平行的。
            if (distance != 0f)
			{
				return linePoint + (lineVec * distance);	
			}

			return Vector3.zero;
		}

        //返回2点，从第1行开始，将会有一个最近的点到第2行，在第2行，将会有一个最近的点到第1行。
        public static IntersectPoints ClosestPointsOnTwoLines(Vector3 point1, Vector3 point1Direction, Vector3 point2, Vector3 point2Direction)
		{
			IntersectPoints intersections = new IntersectPoints();
			
			//I dont think we need to normalize
			//point1Direction.Normalize();
			//point2Direction.Normalize();

			float a = Vector3.Dot(point1Direction, point1Direction);
			float b = Vector3.Dot(point1Direction, point2Direction);
			float e = Vector3.Dot(point2Direction, point2Direction);
 
			float d = a*e - b*b;
 
			//This is a check if parallel, howeverm since we are not normalizing the directions, it seems even if they are parallel they will not == 0
			//so they will get past this point, but its seems to be alright since it seems to still give a correct point (although a point very fary away).
			//Also, if they are parallel and we dont normalize, the deciding point seems randomly choses on the lines, which while is still correct,
			//our ClosestPointsOnTwoLineSegments gets undesireable results when on corners. (for example when using it in our ClosestPointOnTriangleToLine).
			if(d != 0f)
			{
				Vector3 r = point1 - point2;
				float c = Vector3.Dot(point1Direction, r);
				float f = Vector3.Dot(point2Direction, r);
 
				float s = (b*f - c*e) / d;
				float t = (a*f - c*b) / d;
 
				intersections.first = point1 + point1Direction * s;
				intersections.second = point2 + point2Direction * t;
			}else{
                //线是平行的，选择相邻的任何点。
                intersections.first = point1;
				intersections.second = point2 + Vector3.Project(point1 - point2, point2Direction);
			}

			return intersections;
		}

		public static IntersectPoints ClosestPointsOnSegmentToLine(Vector3 segment0, Vector3 segment1, Vector3 linePoint, Vector3 lineDirection)
		{
			IntersectPoints closests = ClosestPointsOnTwoLines(segment0, segment1 - segment0, linePoint, lineDirection);
			closests.first = ClampToSegment(closests.first, segment0, segment1);

			return closests;
		}

        //假设这个点已经在某处了。
        public static Vector3 ClampToSegment(Vector3 point, Vector3 linePoint1, Vector3 linePoint2)
		{
			Vector3 lineDirection = linePoint2 - linePoint1;

			if(!ExtVector3.IsInDirection(point - linePoint1, lineDirection))
			{
				point = linePoint1;
			}
			else if(ExtVector3.IsInDirection(point - linePoint2, lineDirection))
			{
				point = linePoint2;
			}

			return point;
		}
	}
}