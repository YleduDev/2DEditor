using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DevelopEngine;
using UnityEngine.EventSystems;

namespace RuntimeGizmos
{
    [RequireComponent(typeof(Camera))]
    public class TransformGizmo : MonoSingleton<TransformGizmo>
    {
        public TransformSpace space = TransformSpace.Local;
        public TransformType type = TransformType.Move;

        Color xColor = new Color(1, 0, 0, 0.8f);
        Color yColor = new Color(0, 1, 0, 0.8f);
        Color zColor = new Color(0, 0, 1, 0.8f);
        Color allColor = new Color(.7f, .7f, .7f, 0.8f);
        Color selectedColor = new Color(1, 1, 0, 0.8f);

        readonly float handleLength = .25f;
        readonly float triangleSize = .03f;
        readonly float boxSize = .01f;
        readonly int circleDetail = 40;
        readonly float minSelectedDistanceCheck = .02f;
        readonly float moveSpeedMultiplier = 1f;
        readonly float scaleSpeedMultiplier = 1f;
        readonly float rotateSpeedMultiplier = 200f;
        readonly float allRotateSpeedMultiplier = 20f;

        AxisVectors handleLines = new AxisVectors();
        AxisVectors handleTriangles = new AxisVectors();
        AxisVectors handleSquares = new AxisVectors();
        AxisVectors circlesLines = new AxisVectors();
        AxisVectors drawCurrentCirclesLines = new AxisVectors();

        bool isTransforming;//是否在编辑
        float totalScaleAmount;//缩放总额
        Quaternion totalRotationAmount;//旋转总额
        Axis selectedAxis = Axis.None;
        AxisInfo axisInfo;
        public Transform target;
        public Transform lastTarget;
        Camera myCamera;

        static Material lineMaterial;

        void Awake()
        {
            myCamera = GetComponent<Camera>();
            //轴ui 渲染材质
            SetMaterial();
        }

        void Update()
        {
            //按键选择模式
            //SetSpaceAndType();
            //现在方向轴
            SelectAxis();
            //获取目标物体
            GetTarget();

            if (target == null) return;

            TransformSelected();
        }

        void LateUpdate()
        {
            if (target == null) return;

            //设置轴的方向和大小
            SetAxisInfo();
            //设置线的相关属性
            SetLines();
        }


        //拓展 表示 能否编辑 
        bool move = true;
        bool rotation = true;
        bool scale = true;
        //

        /// <summary>
        /// 在摄像机渲染场景之后执行的脚本生命周期
        /// 用于渲染轴UI
        /// </summary>
		void OnPostRender()
        {
            if (target == null) return;

            lineMaterial.SetPass(0);

            Color xColor = (selectedAxis == Axis.X) ? selectedColor : this.xColor;
            Color yColor = (selectedAxis == Axis.Y) ? selectedColor : this.yColor;
            Color zColor = (selectedAxis == Axis.Z) ? selectedColor : this.zColor;
            Color allColor = (selectedAxis == Axis.Any) ? selectedColor : this.allColor;

            if (type == TransformType.Move && move)
            {
                DrawLines(handleLines.x, xColor);
                DrawLines(handleLines.y, yColor);
                DrawLines(handleLines.z, zColor);

                DrawTriangles(handleTriangles.x, xColor);
                DrawTriangles(handleTriangles.y, yColor);
                DrawTriangles(handleTriangles.z, zColor);
            }
            else if (type == TransformType.Scale && scale)
            {
                DrawLines(handleLines.x, xColor);
                DrawLines(handleLines.y, yColor);
                DrawLines(handleLines.z, zColor);

                DrawSquares(handleSquares.x, xColor);
                DrawSquares(handleSquares.y, yColor);
                DrawSquares(handleSquares.z, zColor);
                DrawSquares(handleSquares.all, allColor);
            }
            else if (type == TransformType.Rotate && rotation)
            {
                AxisVectors rotationAxisVector = circlesLines;
                if (isTransforming && space == TransformSpace.Global && type == TransformType.Rotate)
                {
                    rotationAxisVector = drawCurrentCirclesLines;

                    AxisInfo axisInfo = new AxisInfo();
                    axisInfo.xDirection = totalRotationAmount * Vector3.right;
                    axisInfo.yDirection = totalRotationAmount * Vector3.up;
                    axisInfo.zDirection = totalRotationAmount * Vector3.forward;
                    SetCircles(axisInfo, drawCurrentCirclesLines);
                }

                DrawCircles(rotationAxisVector.x, xColor);
                DrawCircles(rotationAxisVector.y, yColor);
                DrawCircles(rotationAxisVector.z, zColor);
                DrawCircles(rotationAxisVector.all, allColor);
            }

        }

        #region 点击拖动相关事件

        /// 选中轴方法
        void TransformSelected()
        {
            if (selectedAxis != Axis.None && Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {

                }
                else
                {
                    StartCoroutine(TransformSelected(type));
                }
            }
        }

        //鼠标点击及拖动协程
        IEnumerator TransformSelected(TransformType type)
        {
            //isTransforming = true;
            //totalScaleAmount = 0;
            //totalRotationAmount = Quaternion.identity;

            //Vector3 originalTargetPosition = target.position;
            //Vector3 planeNormal = (transform.position - target.position).normalized;
            //Vector3 axis = GetSelectedAxisDirection();
            //Vector3 projectedAxis = Vector3.ProjectOnPlane(axis, planeNormal).normalized;
            //Vector3 previousMousePosition = Vector3.zero;

            ////记录执行前的位置信息
            //Vector3 lastPos = target.transform.position;
            //Vector3 lastScale = target.transform.localScale;
            //Quaternion lastRotate = target.transform.rotation;
            //while (!Input.GetMouseButtonUp(0))
            //{
            //    Ray mouseRay = myCamera.ScreenPointToRay(Input.mousePosition);
            //    Vector3 mousePosition = Geometry.LinePlaneIntersect(mouseRay.origin, mouseRay.direction, originalTargetPosition, planeNormal);

            //    if (previousMousePosition != Vector3.zero && mousePosition != Vector3.zero)
            //    {
            //        AssetRelation assetRelation = target.GetComponent<AssetRelation>();

            //        Transform model = assetRelation != null ? assetRelation.assetModel : target;

            //        if (type == TransformType.Move)
            //        {
            //            float moveAmount = ExtVector3.MagnitudeInDirection(mousePosition - previousMousePosition, projectedAxis) * moveSpeedMultiplier;
            //            target.Translate(axis * moveAmount, Space.World);
            //        }

            //        if (type == TransformType.Scale)
            //        {
            //            Vector3 projected = (selectedAxis == Axis.Any) ? transform.right : projectedAxis;
            //            float scaleAmount = ExtVector3.MagnitudeInDirection(mousePosition - previousMousePosition, projected) * scaleSpeedMultiplier;

            //            //在unity 5.4和5.5中有一个缺陷，它会导致反变换方向受到缩放比例的影响
            //            Vector3 localAxis = (space == TransformSpace.Local && selectedAxis != Axis.Any) ? model.InverseTransformDirection(axis) : axis;

            //            if (selectedAxis == Axis.Any) model.localScale += (ExtVector3.Abs(model.localScale.normalized) * scaleAmount);
            //            else model.localScale += (localAxis * scaleAmount);

            //            totalScaleAmount += scaleAmount;
            //        }

            //        if (type == TransformType.Rotate)
            //        {
            //            if (selectedAxis == Axis.Any)
            //            {
            //                Vector3 rotation = transform.TransformDirection(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0));
            //                model.Rotate(rotation * allRotateSpeedMultiplier, Space.World);
            //                totalRotationAmount *= Quaternion.Euler(rotation * allRotateSpeedMultiplier);
            //            }
            //            else
            //            {
            //                Vector3 projected = (selectedAxis == Axis.Any || ExtVector3.IsParallel(axis, planeNormal)) ? planeNormal : Vector3.Cross(axis, planeNormal);
            //                float rotateAmount = (ExtVector3.MagnitudeInDirection(mousePosition - previousMousePosition, projected) * rotateSpeedMultiplier) / GetDistanceMultiplier();
            //                model.Rotate(axis, rotateAmount, Space.World);
            //                totalRotationAmount *= Quaternion.Euler(axis * rotateAmount);
            //            }
            //        }


            //    }

            //    previousMousePosition = mousePosition;

            //    //UI
            //    UIManager.Instance.UI_GO.ShowGO(target);

            //    
            //}
            //记录执行后的位置信息
            //Vector3 newPos = target.transform.position;
            //Vector3 newScale = target.transform.localScale;
            //Quaternion newRotate = target.transform.rotation;

            ////注册撤销和删除
            //CommandManager.CommandMan.AddCammand(() =>
            //{
            //    target.position = newPos;
            //    target.rotation = newRotate;
            //    target.localScale = newScale;
            //}, () =>
            //{
            //    target.position = lastPos;
            //    target.rotation = lastRotate;
            //    target.localScale = lastScale;
            //});


            //totalRotationAmount = Quaternion.identity;
            //totalScaleAmount = 0;
            //isTransforming = false;
            ////
            yield return null;
        }




        //获取选中轴的方向
        Vector3 GetSelectedAxisDirection()
        {
            if (selectedAxis != Axis.None)
            {
                if (selectedAxis == Axis.X) return axisInfo.xDirection;
                if (selectedAxis == Axis.Y) return axisInfo.yDirection;
                if (selectedAxis == Axis.Z) return axisInfo.zDirection;
                if (selectedAxis == Axis.Any) return Vector3.one;
            }
            return Vector3.zero;
        }

        //获取目标物体 并显示描边
        void GetTarget()
        {
            //if (EventSystem.current.IsPointerOverGameObject()) return;
            //if (selectedAxis == Axis.None && Input.GetMouseButtonDown(0))
            //{
            //    RaycastHit hitInfo;
            //    if (Physics.Raycast(myCamera.ScreenPointToRay(Input.mousePosition), out hitInfo))
            //    {
            //        if (!string.Equals(hitInfo.transform.tag, Global.assetModelTag)) return;

            //        //target = hitInfo.transform.GetComponent<ModelSelfAction>().EditorTarget;
            //        //上次的目标物体
            //        lastTarget = target;
            //        //注册撤销和重做对象

            //        CommandManager.CommandMan.AddCammand(() => { target = hitInfo.transform.parent ? hitInfo.transform.parent : hitInfo.transform; },
            //            () => { target = lastTarget; });

            //        ChangeEditTarget(target);

            //        // 空间编辑属性
            //        CheckTransformConfig();
            //    }
            //    //else
            //    //{
            //    //    lastTarget = target;
            //    //    target = null;
            //    //    //隐藏ui
            //    //    //UIManager.Instance.UI_GO.gameObject.SetActive(false);
            //    //}
            //}
        }

        public void ChangeEditTarget(Transform newTarget)
        {
            //if (target != newTarget)
            //{
            //    target = newTarget;
            //}

            //AssetRelation assetRelation = target.GetComponent<AssetRelation>();

            //if (assetRelation != null)
            //{
            //    HighlightingManager.Instance.DynamicContour(assetRelation.assetModel.gameObject, assetRelation.monitorItem.Status);

            //    ModelItemListManager.Instance.CreatModelList(assetRelation.monitorItem.Catalog);

            //    AssetAttributeControl.Instance.UpdateAssetAttribute(assetRelation.monitorItem);

            //    if (!EditorManager.Instance.newDisplayAssetDic.ContainsKey(assetRelation.monitorItem.FullName))
            //    {
            //        if (EditorManager.Instance.updataDisplayAssetDic.ContainsKey(assetRelation.monitorItem.FullName))
            //        {
            //            EditorManager.Instance.updataDisplayAssetDic[assetRelation.monitorItem.FullName] = target;
            //        }
            //        else
            //        {
            //            EditorManager.Instance.updataDisplayAssetDic.Add(assetRelation.monitorItem.FullName, target);
            //        }
            //    }
            //}

            //UIManager.Instance.UI_GO.ShowGO(target);
        }

        AxisVectors selectedLinesBuffer = new AxisVectors();

        //选择轴 并判断当前选中的轴向，没有返回空轴
        void SelectAxis()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            //如果属性表示无法编辑，直接退出
            if (CloseSelectedAxis())
            {
                return;
            }
            //UI检测
            if (EventSystem.current.IsPointerOverGameObject())
            {

            }
            else
            {
                selectedAxis = Axis.None;

                float xClosestDistance = float.MaxValue;
                float yClosestDistance = float.MaxValue;
                float zClosestDistance = float.MaxValue;
                float allClosestDistance = float.MaxValue;
                float minSelectedDistanceCheck = this.minSelectedDistanceCheck * GetDistanceMultiplier();

                if (type == TransformType.Move || type == TransformType.Scale)
                {
                    selectedLinesBuffer.Clear();
                    selectedLinesBuffer.Add(handleLines);
                    if (type == TransformType.Move) selectedLinesBuffer.Add(handleTriangles);
                    else if (type == TransformType.Scale) selectedLinesBuffer.Add(handleSquares);

                    xClosestDistance = ClosestDistanceFromMouseToLines(selectedLinesBuffer.x);
                    yClosestDistance = ClosestDistanceFromMouseToLines(selectedLinesBuffer.y);
                    zClosestDistance = ClosestDistanceFromMouseToLines(selectedLinesBuffer.z);
                    allClosestDistance = ClosestDistanceFromMouseToLines(selectedLinesBuffer.all);
                }
                else if (type == TransformType.Rotate)
                {
                    xClosestDistance = ClosestDistanceFromMouseToLines(circlesLines.x);
                    yClosestDistance = ClosestDistanceFromMouseToLines(circlesLines.y);
                    zClosestDistance = ClosestDistanceFromMouseToLines(circlesLines.z);
                    allClosestDistance = ClosestDistanceFromMouseToLines(circlesLines.all);
                }

                if (type == TransformType.Scale && allClosestDistance <= minSelectedDistanceCheck) selectedAxis = Axis.Any;
                else if (xClosestDistance <= minSelectedDistanceCheck && xClosestDistance <= yClosestDistance && xClosestDistance <= zClosestDistance) selectedAxis = Axis.X;

                else if (yClosestDistance <= minSelectedDistanceCheck && yClosestDistance <= xClosestDistance && yClosestDistance <= zClosestDistance) selectedAxis = Axis.Y;
                else if (zClosestDistance <= minSelectedDistanceCheck && zClosestDistance <= xClosestDistance && zClosestDistance <= yClosestDistance) selectedAxis = Axis.Z;
                //else if (type == TransformType.Rotate && target != null)
                //{
                //    Ray mouseRay = myCamera.ScreenPointToRay(Input.mousePosition);
                //    Vector3 mousePlaneHit = Geometry.LinePlaneIntersect(mouseRay.origin, mouseRay.direction, target.position, (transform.position - target.position).normalized);
                //    if ((target.position - mousePlaneHit).sqrMagnitude <= (handleLength * GetDistanceMultiplier()).Squared()) selectedAxis = Axis.Any;
                //}
            }
        }

        #endregion

        #region 工具
        //从鼠标到直线的最近距离。
        float ClosestDistanceFromMouseToLines(List<Vector3> lines)
        {
            Ray mouseRay = myCamera.ScreenPointToRay(Input.mousePosition);

            float closestDistance = float.MaxValue;
            for (int i = 0; i < lines.Count; i += 2)
            {                                                            //在线段上最接近的点。
                IntersectPoints points = Geometry.ClosestPointsOnSegmentToLine(lines[i], lines[i + 1], mouseRay.origin, mouseRay.direction);
                float distance = Vector3.Distance(points.first, points.second);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                }
            }
            return closestDistance;
        }

        //检测编辑属性
        private void CheckTransformConfig()
        {
          //  TransAbility tranAb = target.GetComponent<TransAbility>();
            //if (tranAb)
            //{
            //    move = tranAb.move;
            //    rotation = tranAb.rotation;
            //    scale = tranAb.scale;
            //}
            //else
            //{
            //    move = true;
            //    rotation = true;
            //    scale = true;
            //}
        }

        //设置轴的方向和大小
        void SetAxisInfo()
        {
            float size = handleLength * GetDistanceMultiplier();
            axisInfo.Set(target, size, space);

            if (isTransforming && type == TransformType.Scale)
            {
                if (selectedAxis == Axis.Any) axisInfo.Set(target, size + totalScaleAmount, space);
                if (selectedAxis == Axis.X) axisInfo.xAxisEnd += (axisInfo.xDirection * totalScaleAmount);
                if (selectedAxis == Axis.Y) axisInfo.yAxisEnd += (axisInfo.yDirection * totalScaleAmount);
                if (selectedAxis == Axis.Z) axisInfo.zAxisEnd += (axisInfo.zDirection * totalScaleAmount);
            }
        }

        //这有助于保持尺寸一致，无论我们离它有多远
        float GetDistanceMultiplier()
        {
            if (target == null) return 0f;
            return Mathf.Max(.01f, Mathf.Abs(ExtVector3.MagnitudeInDirection(target.position - transform.position, myCamera.transform.forward)) / 2);
        }

        /// 设置低级图像库shader
        void SetMaterial()
        {
            if (lineMaterial == null)
            {
                lineMaterial = new Material(Shader.Find("Custom/Lines"));
                #region Shader code
                /*
				Shader "Custom/Lines"
				{
					SubShader
					{
						Pass
						{
							Blend SrcAlpha OneMinusSrcAlpha
							ZWrite Off
							ZTest Always
							Cull Off
							Fog { Mode Off }

							BindChannels
							{
								Bind "vertex", vertex
								Bind "color", color
							}
						}
					}
				}
				*/
                #endregion
            }
        }

        //获取当前目标物体
        public Transform GetCurrentGO()
        {
            return target;
        }

        //对外提供的设置聚焦物体
        public void SetTarget(Transform targetTF)
        {
            if (!targetTF) return;
            target = targetTF;

            //UIManager.Instance.UI_GO.gameObject.SetActive(true);
            //UIManager.Instance.UI_GO.ShowGO(target);

            CheckTransformConfig();

          //  AssetRelation assetRelation = targetTF.GetComponent<AssetRelation>();

            //if (assetRelation != null)
            //{
            //    HighlightingManager.Instance.DynamicContour(assetRelation.assetModel.gameObject, assetRelation.monitorItem.Status);
            //}
            //else
            //{
            //    HighlightingManager.Instance.DynamicContour(targetTF.gameObject, Color.yellow);
            //}
        }

        //根据可编辑字段 判断不进行编辑的属性并关闭 选中轴
        //主要关闭算法通道
        private bool CloseSelectedAxis()
        {
            if (!move && type == TransformType.Move || !rotation && type == TransformType.Rotate || !scale && type == TransformType.Scale)
            {
                selectedAxis = Axis.None;
                return true;
            }
            return false;
        }

        #endregion

        #region 设置、画出初始图像(轴UI）
        //设置原型图像点
        void SetLines()
        {
            SetHandleLines();
            SetHandleTriangles();
            SetHandleSquares();
            SetCircles(axisInfo, circlesLines);
        }

        //设置委托线
        void SetHandleLines()
        {
            handleLines.Clear();

            if (type == TransformType.Move || type == TransformType.Scale)
            {
                handleLines.x.Add(target.position);
                handleLines.x.Add(axisInfo.xAxisEnd);
                handleLines.y.Add(target.position);
                handleLines.y.Add(axisInfo.yAxisEnd);
                handleLines.z.Add(target.position);
                handleLines.z.Add(axisInfo.zAxisEnd);
            }
        }

        //设置委托三角形
        void SetHandleTriangles()
        {
            handleTriangles.Clear();

            if (type == TransformType.Move)
            {
                float triangleLength = triangleSize * GetDistanceMultiplier();
                AddTriangles(axisInfo.xAxisEnd, axisInfo.xDirection, axisInfo.yDirection, axisInfo.zDirection, triangleLength, handleTriangles.x);
                AddTriangles(axisInfo.yAxisEnd, axisInfo.yDirection, axisInfo.xDirection, axisInfo.zDirection, triangleLength, handleTriangles.y);
                AddTriangles(axisInfo.zAxisEnd, axisInfo.zDirection, axisInfo.yDirection, axisInfo.xDirection, triangleLength, handleTriangles.z);
            }
        }
        //添加三角形
        void AddTriangles(Vector3 axisEnd, Vector3 axisDirection, Vector3 axisOtherDirection1, Vector3 axisOtherDirection2, float size, List<Vector3> resultsBuffer)
        {
            Vector3 endPoint = axisEnd + (axisDirection * (size * 2f));
            Square baseSquare = GetBaseSquare(axisEnd, axisOtherDirection1, axisOtherDirection2, size / 2f);

            resultsBuffer.Add(baseSquare.bottomLeft);
            resultsBuffer.Add(baseSquare.topLeft);
            resultsBuffer.Add(baseSquare.topRight);
            resultsBuffer.Add(baseSquare.topLeft);
            resultsBuffer.Add(baseSquare.bottomRight);
            resultsBuffer.Add(baseSquare.topRight);

            for (int i = 0; i < 4; i++)
            {
                resultsBuffer.Add(baseSquare[i]);
                resultsBuffer.Add(baseSquare[i + 1]);
                resultsBuffer.Add(endPoint);
            }
        }

        //设置委托矩形
        void SetHandleSquares()
        {
            handleSquares.Clear();

            if (type == TransformType.Scale)
            {
                float boxLength = boxSize * GetDistanceMultiplier();
                AddSquares(axisInfo.xAxisEnd, axisInfo.xDirection, axisInfo.yDirection, axisInfo.zDirection, boxLength, handleSquares.x);
                AddSquares(axisInfo.yAxisEnd, axisInfo.yDirection, axisInfo.xDirection, axisInfo.zDirection, boxLength, handleSquares.y);
                AddSquares(axisInfo.zAxisEnd, axisInfo.zDirection, axisInfo.xDirection, axisInfo.yDirection, boxLength, handleSquares.z);
                AddSquares(target.position - (axisInfo.xDirection * boxLength), axisInfo.xDirection, axisInfo.yDirection, axisInfo.zDirection, boxLength, handleSquares.all);
            }
        }

        //添加矩形
        void AddSquares(Vector3 axisEnd, Vector3 axisDirection, Vector3 axisOtherDirection1, Vector3 axisOtherDirection2, float size, List<Vector3> resultsBuffer)
        {
            Square baseSquare = GetBaseSquare(axisEnd, axisOtherDirection1, axisOtherDirection2, size);
            Square baseSquareEnd = GetBaseSquare(axisEnd + (axisDirection * (size * 2f)), axisOtherDirection1, axisOtherDirection2, size);

            resultsBuffer.Add(baseSquare.bottomLeft);
            resultsBuffer.Add(baseSquare.topLeft);
            resultsBuffer.Add(baseSquare.bottomRight);
            resultsBuffer.Add(baseSquare.topRight);

            resultsBuffer.Add(baseSquareEnd.bottomLeft);
            resultsBuffer.Add(baseSquareEnd.topLeft);
            resultsBuffer.Add(baseSquareEnd.bottomRight);
            resultsBuffer.Add(baseSquareEnd.topRight);

            for (int i = 0; i < 4; i++)
            {
                resultsBuffer.Add(baseSquare[i]);
                resultsBuffer.Add(baseSquare[i + 1]);
                resultsBuffer.Add(baseSquareEnd[i + 1]);
                resultsBuffer.Add(baseSquareEnd[i]);
            }
        }
        //获取原型矩形
        Square GetBaseSquare(Vector3 axisEnd, Vector3 axisOtherDirection1, Vector3 axisOtherDirection2, float size)
        {
            Square square;
            Vector3 offsetUp = ((axisOtherDirection1 * size) + (axisOtherDirection2 * size));
            Vector3 offsetDown = ((axisOtherDirection1 * size) - (axisOtherDirection2 * size));
            //These arent really the proper directions, as in the bottomLeft isnt really at the bottom left...
            square.bottomLeft = axisEnd + offsetDown;
            square.topLeft = axisEnd + offsetUp;
            square.bottomRight = axisEnd - offsetDown;
            square.topRight = axisEnd - offsetUp;
            return square;
        }

        //设置圆
        void SetCircles(AxisInfo axisInfo, AxisVectors axisVectors)
        {
            axisVectors.Clear();

            if (type == TransformType.Rotate)
            {
                float circleLength = handleLength * GetDistanceMultiplier();
                AddCircle(target.position, axisInfo.xDirection, circleLength, axisVectors.x);
                AddCircle(target.position, axisInfo.yDirection, circleLength, axisVectors.y);
                AddCircle(target.position, axisInfo.zDirection, circleLength, axisVectors.z);
                AddCircle(target.position, (target.position - transform.position).normalized, circleLength, axisVectors.all, false);
            }
        }
        //添加圆形
        void AddCircle(Vector3 origin, Vector3 axisDirection, float size, List<Vector3> resultsBuffer, bool depthTest = true)
        {
            Vector3 up = axisDirection.normalized * size;
            Vector3 forward = Vector3.Slerp(up, -up, .5f);
            Vector3 right = Vector3.Cross(up, forward).normalized * size;

            Matrix4x4 matrix = new Matrix4x4();

            matrix[0] = right.x;
            matrix[1] = right.y;
            matrix[2] = right.z;

            matrix[4] = up.x;
            matrix[5] = up.y;
            matrix[6] = up.z;

            matrix[8] = forward.x;
            matrix[9] = forward.y;
            matrix[10] = forward.z;

            Vector3 lastPoint = origin + matrix.MultiplyPoint3x4(new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)));
            Vector3 nextPoint = Vector3.zero;
            float multiplier = 360f / circleDetail;

            Plane plane = new Plane((transform.position - target.position).normalized, target.position);

            for (var i = 0; i < circleDetail + 1; i++)
            {
                nextPoint.x = Mathf.Cos((i * multiplier) * Mathf.Deg2Rad);
                nextPoint.z = Mathf.Sin((i * multiplier) * Mathf.Deg2Rad);
                nextPoint.y = 0;

                nextPoint = origin + matrix.MultiplyPoint3x4(nextPoint);

                if (!depthTest || plane.GetSide(lastPoint))
                {
                    resultsBuffer.Add(lastPoint);
                    resultsBuffer.Add(nextPoint);
                }

                lastPoint = nextPoint;
            }
        }

        void DrawLines(List<Vector3> lines, Color color)
        {
            GL.Begin(GL.LINES);
            GL.Color(color);

            for (int i = 0; i < lines.Count; i += 2)
            {
                GL.Vertex(lines[i]);
                GL.Vertex(lines[i + 1]);
            }

            GL.End();
        }

        void DrawTriangles(List<Vector3> lines, Color color)
        {
            GL.Begin(GL.TRIANGLES);
            GL.Color(color);

            for (int i = 0; i < lines.Count; i += 3)
            {
                GL.Vertex(lines[i]);
                GL.Vertex(lines[i + 1]);
                GL.Vertex(lines[i + 2]);
            }

            GL.End();
        }

        void DrawSquares(List<Vector3> lines, Color color)
        {
            GL.Begin(GL.QUADS);
            GL.Color(color);

            for (int i = 0; i < lines.Count; i += 4)
            {
                GL.Vertex(lines[i]);
                GL.Vertex(lines[i + 1]);
                GL.Vertex(lines[i + 2]);
                GL.Vertex(lines[i + 3]);
            }

            GL.End();
        }

        void DrawCircles(List<Vector3> lines, Color color)
        {
            GL.Begin(GL.LINES);
            GL.Color(color);

            for (int i = 0; i < lines.Count; i += 2)
            {
                GL.Vertex(lines[i]);
                GL.Vertex(lines[i + 1]);
            }

            GL.End();
        }

        #endregion

        #region UIEvent

        public void ChangeTransType(int TranType)
        {
            TransformType newType = (TransformType)Mathf.Clamp(TranType, 0, 2);
            if (newType == type) return;
            type = newType;
            if (type == TransformType.Scale) space = TransformSpace.Local;
        }

        public void SetSpaceChange()
        {
            if (space == TransformSpace.Global) space = TransformSpace.Local;
            else if (space == TransformSpace.Local) space = TransformSpace.Global;
            if (type == TransformType.Scale) space = TransformSpace.Local;
        }

        public float GetCameraSize()
        {
            float size = Math.Abs(Camera.main.transform.position.x -
             target.position.x);
            size = Mathf.Clamp(size, 1f, 5f);
            return size;
        }

        #endregion


    }
}