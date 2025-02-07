using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI.Extensions;
using System;

[ExecuteAlways]
[AddComponentMenu("UI/UI Polygon Image")]
public class UIPolygonImage : Image
{
	[SerializeField]
	[Range(3, 1000)]
	private int sides = 3; // 多边形边数，最小3边

	[SerializeField]
	[Range(0, 360)]
	private float rotation; // 旋转角度

	[SerializeField]
	[Range(0, 1)]
	private float hollowRatio = 0.5f; // 挖空比例 0-1之间

	[SerializeField]
	[Range(0, 1)]
	private float _progress = 1f; // 进度 0-1

	private enum BarType
	{
		//进度条圆心走
		Center,
		//进度条垂直走
		Vertical,
	}
	[SerializeField]
	private BarType barType = BarType.Center; // 进度类型

	private Vector4 _uv;
	private Vector4 _v;
	private float _width;
	private float _height;

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
		if (sprite == null)
		{
			return;
		}
		_uv = (overrideSprite != null) ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
		_v = GetDrawingDimensions();
		_width = _v.z - _v.x;
		_height = _v.w - _v.y;

		// 根据hollowRatio决定是否生成内圈
		if (hollowRatio > 0)
		{
			if (barType == BarType.Center)
			{
				PopulateMesh_Hollow_Center(vh);
			}
			else
			{
				PopulateMesh_Hollow_Vertical(vh);
			}
		}
		else
		{
			PopulateMesh_Solid(vh);
		}
	}


	private void PopulateMesh_Hollow_Center(VertexHelper vh)
	{
		Vector2 cc = new(_v.x + _width / 2f, _v.y + _height / 2f);
		float radius = Mathf.Min(_width, _height) / 2f;
		float startRot = rotation * Mathf.Deg2Rad;
		float anglePerSide = 2f * Mathf.PI / sides;
		float progressAngle = startRot + 2f * Mathf.PI * _progress;

		UIVertex vt = UIVertex.simpleVert;
		vt.color = color;

		//外圈
		Vector2 ext_aa = Vector2.zero;
		Vector2 ext_bb = Vector2.zero;
		//内圈
		Vector2 int_aa = Vector2.zero;
		Vector2 int_bb = Vector2.zero;

		int addVtIndex = -1;
		int triangleCount = 0;
		for (int i = 0; i < sides + 1; i++)
		{
			float angle = startRot + anglePerSide * i;
			ext_bb.x = cc.x + radius * Mathf.Cos(angle);
			ext_bb.y = cc.y + radius * Mathf.Sin(angle);
			int_bb.x = cc.x + hollowRatio * radius * Mathf.Cos(angle);
			int_bb.y = cc.y + hollowRatio * radius * Mathf.Sin(angle);
			if (progressAngle >= angle)
			{
				AddVt(vh, vt, ext_bb, ref addVtIndex);
				AddVt(vh, vt, int_bb, ref addVtIndex);
			}


			if (progressAngle <= angle)
			{
				triangleCount = i;
				var cur_point = GetProgressAnglePoint(ext_aa, ext_bb, cc, progressAngle);
				AddVt(vh, vt, cur_point, ref addVtIndex);
				var int_cur_point = GetProgressAnglePoint(int_aa, int_bb, cc, progressAngle);
				AddVt(vh, vt, int_cur_point, ref addVtIndex);
				break;
			}
			ext_aa.x = ext_bb.x;
			ext_aa.y = ext_bb.y;
			int_aa.x = int_bb.x;
			int_aa.y = int_bb.y;
		}

		//add triangle
		int addTriangleCount = 0;
		for (int i = 0; i < triangleCount; i++)
		{
			AddTriangle(vh, 2 * i, 2 * i + 1, 2 * i + 2, ref addTriangleCount);
			AddTriangle(vh, 2 * i + 1, (2 * i + 3) % (addVtIndex + 1), (2 * i + 2) % (addVtIndex + 1), ref addTriangleCount);
		}
	}

	private void PopulateMesh_Hollow_Vertical(VertexHelper vh)

	{
		float startRot = rotation * Mathf.Deg2Rad;
		float progressAngle = startRot + 2f * Mathf.PI * _progress;
		if (progressAngle <= 0) return;
		Vector2 cc = new(_v.x + _width / 2f, _v.y + _height / 2f);
		float radius = Mathf.Min(_width, _height) / 2f;
		float anglePerSide = 2f * Mathf.PI / sides;
		UIVertex vt = UIVertex.simpleVert;
		vt.color = color;
		//aa 是当前点 bb 是下一个点 cc 是圆心
		//外圈顶点
		Vector2 ext_aa = Vector2.zero;
		Vector2 ext_bb = Vector2.zero;
		//内圈
		Vector2 int_aa = Vector2.zero;
		Vector2 int_bb = Vector2.zero;
		int addVtIndex = -1;
		int addTriangleCnt = 0;
		for (int i = 0; i < sides + 1; i++)
		{
			float angle = startRot + anglePerSide * i;
			ext_bb.x = cc.x + radius * Mathf.Cos(angle);
			ext_bb.y = cc.y + radius * Mathf.Sin(angle);
			int_bb.x = cc.x + hollowRatio * radius * Mathf.Cos(angle);
			int_bb.y = cc.y + hollowRatio * radius * Mathf.Sin(angle);
			if (progressAngle >= angle)
			{
				AddVt(vh, vt, ext_bb, ref addVtIndex);
				AddVt(vh, vt, int_bb, ref addVtIndex);
				if (addVtIndex > 1)
				{
					AddTriangle(vh, addVtIndex - 1, addVtIndex - 3, addVtIndex - 2, ref addTriangleCnt);
					AddTriangle(vh, addVtIndex - 1, addVtIndex - 2, addVtIndex, ref addTriangleCnt);
				}
			}
			if (progressAngle <= angle)
			{
				var cur_point = GetProgressAnglePoint(ext_aa, ext_bb, cc, progressAngle);
				float isOnSegment = IsProjectionPointOnSegment(int_aa, int_bb, cur_point);
				Vector2 projectionPoint = Vector2.zero;
				if (isOnSegment < 0)
				{
					AddVt(vh, vt, cur_point, ref addVtIndex);
					AddTriangle(vh, addVtIndex, addVtIndex - 2, addVtIndex - 1, ref addTriangleCnt);
				}
				else
				{
					AddVt(vh, vt, cur_point, ref addVtIndex);
					if (isOnSegment > 1)
					{

						projectionPoint = int_bb;
					}
					else
					{
						projectionPoint = GetProjectionPoint(int_aa, int_bb, cur_point);
					}
					AddVt(vh, vt, projectionPoint, ref addVtIndex);
					AddTriangle(vh, addVtIndex - 1, addVtIndex - 3, addVtIndex - 2, ref addTriangleCnt);
					AddTriangle(vh, addVtIndex - 1, addVtIndex - 2, addVtIndex, ref addTriangleCnt);
				}
				break;
			}
			ext_aa.x = ext_bb.x;
			ext_aa.y = ext_bb.y;
			int_aa.x = int_bb.x;
			int_aa.y = int_bb.y;
		}
	}

	private void PopulateMesh_Solid(VertexHelper vh)
	{
		Vector2 cc = new(_v.x + _width / 2f, _v.y + _height / 2f);
		float radius = Mathf.Min(_width, _height) / 2f;
		float startRot = rotation * Mathf.Deg2Rad;
		float anglePerSide = 2f * Mathf.PI / sides;
		float progressAngle = startRot + 2f * Mathf.PI * _progress;
		UIVertex vt = UIVertex.simpleVert;
		vt.color = color;

		// 当hollowRatio为0时，只生成实心多边形
		// 生成外圈顶点
		// aa 是当前点 bb 是下一个点 cc 是圆心
		Vector2 aa = Vector2.one / 2;
		Vector2 bb = Vector2.one / 2;
		int addVtIndex = -1;
		int triangleCount = 0;
		for (int i = 0; i < sides + 1; i++)
		{
			float angle = startRot + anglePerSide * i;
			bb.x = cc.x + radius * Mathf.Cos(angle);
			bb.y = cc.y + radius * Mathf.Sin(angle);
			if (progressAngle >= angle)
			{
				AddVt(vh, vt, bb, ref addVtIndex);
			}
			if (progressAngle <= angle)
			{
				triangleCount = i;
				var cur_point = GetProgressAnglePoint(aa, bb, cc, progressAngle);
				AddVt(vh, vt, cur_point, ref addVtIndex);
				break;
			}
			aa.x = bb.x;
			aa.y = bb.y;
		}

		// 添加中心点
		AddVt(vh, vt, cc, ref addVtIndex);
		int addTriangleCount = 0;
		// 生成三角形（扇形）
		for (int i = 0; i < triangleCount; i++)
		{
			AddTriangle(vh, addVtIndex, (i + 1) % addVtIndex, i, ref addTriangleCount);
		}
		Debug.Log($"End OnPopulateMesh");
	}

	private void AddVt(VertexHelper vh, UIVertex vt, Vector2 pos, ref int addVtIndex)
	{
		vt.position = pos;
		vt.uv0 = new Vector2(
			Mathf.Lerp(_uv.x, _uv.z, (pos.x - _v.x) / _width),
			Mathf.Lerp(_uv.y, _uv.w, (pos.y - _v.y) / _height)
		);
		vh.AddVert(vt);
		Debug.Log($"AddVert[i:{++addVtIndex}]: {pos.x} {pos.y}");
	}

	private void AddTriangle(VertexHelper vh, int a, int b, int c, ref int cnt)
	{
		vh.AddTriangle(a, b, c);
		Debug.Log($"AddTriangle[i:{++cnt}]: {a} {b} {c}");
	}

	//根据progressAngle计算角度是progressAngle的经过cc的直线与aabb的交点
	private Vector2 GetProgressAnglePoint(Vector2 aa, Vector2 bb, Vector2 cc, float progressAngle)
	{
		// 处理水平线的情况
		if (Mathf.Approximately(bb.y, aa.y))
		{
			float yy = aa.y;
			float xx = cc.x + (yy - cc.y) / Mathf.Tan(progressAngle);
			return new Vector2(xx, yy);
		}

		// 处理垂直线的情况
		if (Mathf.Approximately(bb.x, aa.x))
		{
			float xx = aa.x;
			float yy = cc.y + Mathf.Tan(progressAngle) * (xx - cc.x);
			return new Vector2(xx, yy);
		}

		// 处理正常情况
		float k1 = (bb.y - aa.y) / (bb.x - aa.x);
		float b1 = aa.y - k1 * aa.x;
		float k2 = Mathf.Tan(progressAngle);
		float b2 = cc.y - k2 * cc.x;
		float x = Mathf.Approximately(k1, k2) ? aa.x : (b2 - b1) / (k1 - k2);
		float y = k1 * x + b1;
		return new Vector2(x, y);
	}

	/// Image's dimensions used for drawing. X = left, Y = bottom, Z = right, W = top.
	private Vector4 GetDrawingDimensions()
	{
		Vector4 padding = sprite == null ? Vector4.zero : DataUtility.GetPadding(sprite);
		Vector2 size = sprite == null ? Vector2.zero : new Vector2(sprite.rect.width, sprite.rect.height);

		Rect r = GetPixelAdjustedRect();

		int spriteW = Mathf.RoundToInt(size.x);
		int spriteH = Mathf.RoundToInt(size.y);
		Vector4 v = new(
			padding.x / spriteW,
			padding.y / spriteH,
			(spriteW - padding.z) / spriteW,
			(spriteH - padding.w) / spriteH);
		v = new Vector4(
			r.x + r.width * v.x,
			r.y + r.height * v.y,
			r.x + r.width * v.z,
			r.y + r.height * v.w
		);
		return v;
	}

	// 计算投影点D是否在线段AB上
	private float IsProjectionPointOnSegment(Vector2 A, Vector2 B, Vector2 C)
	{
		Vector2 AB = B - A;
		Vector2 AC = C - A;
		float proj = Vector2.Dot(AC, AB) / AB.sqrMagnitude;
		return proj;
	}

	private Vector2 GetProjectionPoint(Vector2 A, Vector2 B, Vector2 C)
	{
		Vector2 AB = B - A;
		Vector2 AC = C - A;
		float proj = Vector2.Dot(AC, AB) / AB.sqrMagnitude;
		Vector2 D = A + proj * AB;
		return D;
	}

#if UNITY_EDITOR
	/*
		private void OnDrawGizmos()
		{
			if (!enabled || sprite == null) return;

			// 获取RectTransform组件
			var rectTransform = transform as RectTransform;
			if (rectTransform == null) return;

			Vector4 v = GetDrawingDimensions();
			float width = v.z - v.x;
			float height = v.w - v.y;
			Vector2 center = new(v.x + width / 2f, v.y + height / 2f);
			float radius = Mathf.Min(width, height) / 2f;

			float startRot = rotation * Mathf.Deg2Rad;
			float anglePerSide = 2f * Mathf.PI / sides;

			// 设置Gizmos颜色
			Gizmos.color = Color.yellow;

			// 绘制中心点
			Gizmos.DrawSphere(transform.TransformPoint(center), 1f);
			UnityEditor.Handles.Label(transform.TransformPoint(center), $"V{sides}\n\nCenter");

			// 绘制外围顶点和连线
			for (int i = 0; i < sides; i++)
			{
				float angle = startRot + anglePerSide * i;
				Vector2 point = new(
					center.x + radius * Mathf.Cos(angle),
					center.y + radius * Mathf.Sin(angle)
				);

				// 绘制顶点
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(transform.TransformPoint(point), 1f);

				// 显示顶点序号和坐标
				UnityEditor.Handles.color = Color.white;
				UnityEditor.Handles.Label(transform.TransformPoint(point), $"\n\n\nV{i}\n({point.x:F1}, {point.y:F1})");
			}
			//内圈
			for (int i = 0; i < sides; i++)
			{
				float angle = startRot + anglePerSide * i;
				Vector2 point = new(
					center.x + hollowRatio * radius * Mathf.Cos(angle),
					center.y + hollowRatio * radius * Mathf.Sin(angle)
				);
				Gizmos.color = Color.blue;
				Gizmos.DrawSphere(transform.TransformPoint(point), 1f);
				UnityEditor.Handles.Label(transform.TransformPoint(point), $"\n\n\nV{sides + i}\n({point.x:F1}, {point.y:F1})");
			}
		}
	*/
#endif
}