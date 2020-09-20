using System;
using UnityEngine;

namespace ExternalAssets.SourceScripts.Utilities
{
    public static class QuadUtils
    {
        private const float HalfPi = Mathf.PI / 2;
        
        public static UIVertex[] GetBillboardNotRotatedQuad(Vector2 position, Vector2 size, Color32 color, Vector4 uv)
        {
            var quad = new UIVertex[4];
            var corner1 = new Vector2(position.x - size.x, position.y - size.y);
            var corner2 = new Vector2(position.x + size.x, position.y + size.y);
                    
            quad[0].position = new Vector2(corner1.x, corner1.y);
            quad[1].position = new Vector2(corner1.x, corner2.y);
            quad[2].position = new Vector2(corner2.x, corner2.y);
            quad[3].position = new Vector2(corner2.x, corner1.y);
            
            quad[0].color = color;
            quad[1].color = color;
            quad[2].color = color;
            quad[3].color = color;
            
            quad[0].uv0 = new Vector2(uv.x, uv.y);
            quad[1].uv0 = new Vector2(uv.x, uv.w);
            quad[2].uv0 = new Vector2(uv.z, uv.w);
            quad[3].uv0 = new Vector2(uv.z, uv.y);
            
            return quad;
        }
        
        public static UIVertex[] GetBillboardRotatedQuad(Vector2 position, float rotation, Vector2 size, Color32 color, Vector4 uv)
        {
            var quad = new UIVertex[4];
            var rotation90 = rotation + HalfPi;
            
            var right = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation)) * size.x;
            var up = new Vector2(Mathf.Cos(rotation90), Mathf.Sin(rotation90)) * size.y;

            quad[0].position = position - right - up;
            quad[1].position = position - right + up;
            quad[2].position = position + right + up;
            quad[3].position = position + right - up;
            
            quad[0].color = color;
            quad[1].color = color;
            quad[2].color = color;
            quad[3].color = color;
            
            quad[0].uv0 = new Vector2(uv.x, uv.y);
            quad[1].uv0 = new Vector2(uv.x, uv.w);
            quad[2].uv0 = new Vector2(uv.z, uv.w);
            quad[3].uv0 = new Vector2(uv.z, uv.y);
            
            return quad;
        }
        
        public static UIVertex[] GenerateModel(float width, float height, float scalefactor, Color32 color, Vector4 uv)
        {
            var quad = new UIVertex[4];
            var halfWidth = width / 2;
            var scaledHeight = -height * scalefactor;
            
            quad[0].position = new Vector2(-halfWidth, scaledHeight);
            quad[1].position = new Vector2(-halfWidth, 0);
            quad[2].position = new Vector2(halfWidth, 0);
            quad[3].position = new Vector2(halfWidth, scaledHeight);
            
            quad[0].color = color;
            quad[1].color = color;
            quad[2].color = color;
            quad[3].color = color;
            
            quad[0].uv0 = new Vector2(uv.z, uv.y);
            quad[1].uv0 = new Vector2(uv.x, uv.y);
            quad[2].uv0 = new Vector2(uv.x, uv.w);
            quad[3].uv0 = new Vector2(uv.z, uv.w);
           
            return quad;
        }

        public static UIVertex[] ApplyPositionAndRotationTransform(UIVertex[] quad, Vector2 position, float angle)
        {
            var sin = Math.Sin(angle);
            var cos = Math.Cos(angle);
            
            quad[0].position = GetRotatedAndMovedPoint(quad[0].position, sin, cos, position);
            quad[1].position = GetRotatedAndMovedPoint(quad[1].position, sin, cos, position);
            quad[2].position = GetRotatedAndMovedPoint(quad[2].position, sin, cos, position);
            quad[3].position = GetRotatedAndMovedPoint(quad[3].position, sin, cos, position);

            return quad;
        }
        
        private static Vector2 GetRotatedAndMovedPoint(Vector2 curr, double sin, double cos, Vector2 pos)
        {
            var x = curr.x * cos - curr.y * sin + pos.x;
            var y = curr.x * sin + curr.y * cos + pos.y;
            
            return new Vector2((float)x, (float)y);
        }
    }
}