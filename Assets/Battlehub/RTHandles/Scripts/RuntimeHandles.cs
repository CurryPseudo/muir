﻿using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Battlehub.RTHandles
{
    public enum RuntimeHandleAxis
    {
        None,
        X,
        Y,
        Z,
        XY,
        XZ,
        YZ,
        Screen,
        Free
    }

    public static class RuntimeHandles 
    {
        public static readonly Color32 XColor = new Color32(187, 70, 45, 255);
        public static readonly Color32 XColorTransparent = new Color32(187, 70, 45, 128);
        public static readonly Color32 YColor = new Color32(139, 206, 74, 255);
        public static readonly Color32 YColorTransparent = new Color32(139, 206, 74, 128);
        public static readonly Color32 ZColor = new Color32(55, 115, 244, 255);
        public static readonly Color32 ZColorTransparent = new Color32(55, 115, 244, 128);
        public static readonly Color32 AltColor = new Color32(192, 192, 192, 224);
        public static readonly Color32 SelectionColor = new Color32(239, 238, 64, 255);
        private static readonly Mesh Arrows;
        private static readonly Mesh SelectionArrowY;
        private static readonly Mesh SelectionArrowX;
        private static readonly Mesh SelectionArrowZ;

        private static readonly Mesh SelectionCube;
        private static readonly Mesh CubeX;
        private static readonly Mesh CubeY;
        private static readonly Mesh CubeZ;
        private static readonly Mesh CubeUniform;

        private static readonly Material ShapesMaterial;
        private static readonly Material LinesMaterial;
        private static readonly Material LinesClipMaterial;
        private static readonly Material LinesBillboardMaterial;
        
        
        static RuntimeHandles()
        {
            LinesMaterial = new Material(Shader.Find("Battlehub/RTHandles/VertexColor"));
            LinesMaterial.color = Color.white;

            LinesClipMaterial = new Material(Shader.Find("Battlehub/RTHandles/VertexColorClip"));
            LinesClipMaterial.color = Color.white;

            LinesBillboardMaterial = new Material(Shader.Find("Battlehub/RTHandles/VertexColorBillboard"));
            LinesBillboardMaterial.color = Color.white;

            ShapesMaterial = new Material(Shader.Find("Battlehub/RTHandles/Shape"));
            ShapesMaterial.color = Color.white;
            
            Mesh selectionArrowMesh = CreateConeMesh(SelectionColor);

            CombineInstance yArrow = new CombineInstance();
            yArrow.mesh = selectionArrowMesh;
            yArrow.transform = Matrix4x4.TRS(Vector3.up, Quaternion.identity, Vector3.one);
            SelectionArrowY = new Mesh();
            SelectionArrowY.CombineMeshes(new[] { yArrow }, true);
            SelectionArrowY.RecalculateNormals();

            CombineInstance xArrow = new CombineInstance();
            xArrow.mesh = selectionArrowMesh;
            xArrow.transform = Matrix4x4.TRS(Vector3.right, Quaternion.AngleAxis(-90, Vector3.forward), Vector3.one);
            SelectionArrowX = new Mesh();
            SelectionArrowX.CombineMeshes(new[] { xArrow }, true);
            SelectionArrowX.RecalculateNormals();

            CombineInstance zArrow = new CombineInstance();
            zArrow.mesh = selectionArrowMesh;
            zArrow.transform = Matrix4x4.TRS(Vector3.forward, Quaternion.AngleAxis(90, Vector3.right), Vector3.one);
            SelectionArrowZ = new Mesh();
            SelectionArrowZ.CombineMeshes(new[] { zArrow }, true);
            SelectionArrowZ.RecalculateNormals();

            yArrow.mesh = CreateConeMesh(YColor);
            xArrow.mesh = CreateConeMesh(XColor);
            zArrow.mesh = CreateConeMesh(ZColor);
            Arrows = new Mesh();
            Arrows.CombineMeshes(new[] { yArrow, xArrow, zArrow }, true);
            Arrows.RecalculateNormals();

            SelectionCube = CreateCubeMesh(SelectionColor);
            CubeX = CreateCubeMesh(XColor);
            CubeY = CreateCubeMesh(YColor);
            CubeZ = CreateCubeMesh(ZColor);
            CubeUniform = CreateCubeMesh(AltColor);
        }

        private static Mesh CreateCubeMesh(Color color)
        {
            const float cubeLength = 1.0f / 10;
            const float cubeWidth = 1.0f / 10;
            const float cubeHeight = 1.0f / 10;
            Vector3 vertice_0 = new Vector3(-cubeLength * .5f, -cubeWidth * .5f, cubeHeight * .5f);
            Vector3 vertice_1 = new Vector3(cubeLength * .5f, -cubeWidth * .5f, cubeHeight * .5f);
            Vector3 vertice_2 = new Vector3(cubeLength * .5f, -cubeWidth * .5f, -cubeHeight * .5f);
            Vector3 vertice_3 = new Vector3(-cubeLength * .5f, -cubeWidth * .5f, -cubeHeight * .5f);
            Vector3 vertice_4 = new Vector3(-cubeLength * .5f, cubeWidth * .5f, cubeHeight * .5f);
            Vector3 vertice_5 = new Vector3(cubeLength * .5f, cubeWidth * .5f, cubeHeight * .5f);
            Vector3 vertice_6 = new Vector3(cubeLength * .5f, cubeWidth * .5f, -cubeHeight * .5f);
            Vector3 vertice_7 = new Vector3(-cubeLength * .5f, cubeWidth * .5f, -cubeHeight * .5f);
            Vector3[] vertices = new []
            {
                // Bottom Polygon
                vertice_0, vertice_1, vertice_2, vertice_3,
                // Left Polygon
                vertice_7, vertice_4, vertice_0, vertice_3,
                // Front Polygon
                vertice_4, vertice_5, vertice_1, vertice_0,
                // Back Polygon
                vertice_6, vertice_7, vertice_3, vertice_2,
                // Right Polygon
                vertice_5, vertice_6, vertice_2, vertice_1,
                // Top Polygon
                vertice_7, vertice_6, vertice_5, vertice_4
            };

            int[] triangles = new []
            {
                // Cube Bottom Side Triangles
                3, 1, 0,
                3, 2, 1,    
                // Cube Left Side Triangles
                3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
                3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
                // Cube Front Side Triangles
                3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
                3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
                // Cube Back Side Triangles
                3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
                3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
                // Cube Rigth Side Triangles
                3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
                3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
                // Cube Top Side Triangles
                3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
                3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
            };
            
            Color[] colors = new Color[vertices.Length];
            for (int i = 0; i < colors.Length; ++i)
            {
                colors[i] = color;
            }

            Mesh cubeMesh = new Mesh();
            cubeMesh.name = "cube";
            cubeMesh.vertices = vertices;
            cubeMesh.triangles = triangles;
            cubeMesh.colors = colors;
            cubeMesh.RecalculateNormals();
            return cubeMesh;
        }

        private static Mesh CreateConeMesh(Color color)
        {
            int segmentsCount = 12;
            float size = 1.0f / 5;

            Vector3[] vertices = new Vector3[segmentsCount * 3 + 1];
            int[] triangles = new int[segmentsCount * 6];
            Color[] colors = new Color[vertices.Length];
            for (int i = 0; i < colors.Length; ++i)
            {
                colors[i] = color;
            }

            float radius = size / 2.6f;
            float height = size;
            float deltaAngle = Mathf.PI * 2.0f / segmentsCount;

            float y = -height;

            vertices[vertices.Length - 1] = new Vector3(0, -height, 0);
            for (int i = 0; i < segmentsCount; i++)
            {
                float angle = i * deltaAngle;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                vertices[i] = new Vector3(x, y, z);
                vertices[segmentsCount + i] = new Vector3(0, 0.01f, 0);
                vertices[2 * segmentsCount + i] = vertices[i];
            }

            for(int i = 0; i < segmentsCount; i++)
            {
                triangles[i * 6] = i;
                triangles[i * 6 + 1] = segmentsCount + i;
                triangles[i * 6 + 2] = (i + 1) % segmentsCount;

                triangles[i * 6 + 3] = vertices.Length - 1;
                triangles[i * 6 + 4] = 2 * segmentsCount + i;
                triangles[i * 6 + 5] = 2 * segmentsCount + (i + 1) % segmentsCount;
            }

            Mesh cone = new Mesh();
            cone.name = "Cone";
            cone.vertices = vertices;
            cone.triangles = triangles;
            cone.colors = colors;

            return cone;
        }

    
        public static float GetScreenScale(Vector3 position, Camera camera)
        {
            float h = camera.pixelHeight;
            if (camera.orthographic)
            {
                return camera.orthographicSize * 2f / h * 90;
            }

            
            Transform transform = camera.transform;
            float distance = Vector3.Dot(position - transform.position, transform.forward);
            float scale = 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            return scale / h * 90;
        }

        private static void DoAxes(Vector3 position, Matrix4x4 transform, RuntimeHandleAxis selectedAxis)
        {
            
            Vector3 x = Vector3.right;
            Vector3 y = Vector3.up;
            Vector3 z = Vector3.forward;

            x = transform.MultiplyVector(x);
            y = transform.MultiplyVector(y);
            z = transform.MultiplyVector(z);
            
            GL.Color(selectedAxis != RuntimeHandleAxis.X ? XColor : SelectionColor);
            GL.Vertex(position);
            GL.Vertex(position + x);
            GL.Color(selectedAxis != RuntimeHandleAxis.Y ? YColor : SelectionColor);
            GL.Vertex(position);
            GL.Vertex(position + y);
            GL.Color(selectedAxis != RuntimeHandleAxis.Z ? ZColor : SelectionColor);
            GL.Vertex(position);
            GL.Vertex(position + z);
        }

        public static void DoPositionHandle(Vector3 position, Quaternion rotation, RuntimeHandleAxis selectedAxis = RuntimeHandleAxis.None)
        {
            float screenScale = GetScreenScale(position, Camera.current);
            Matrix4x4 transform = Matrix4x4.TRS(position, rotation, new Vector3(screenScale, screenScale, screenScale));

            LinesMaterial.SetPass(0);

            GL.Begin(GL.LINES);
            DoAxes(position, transform, selectedAxis);

            const float s = 0.2f;
            Vector3 x = Vector3.right * s;
            Vector3 y = Vector3.up * s;
            Vector3 z = Vector3.forward * s;
            
            Camera camera = Camera.current;
            Vector3 toCam = camera.transform.position - position;

            float fx = Mathf.Sign(Vector3.Dot(toCam, x));
            float fy = Mathf.Sign(Vector3.Dot(toCam, y));
            float fz = Mathf.Sign(Vector3.Dot(toCam, z));

            x.x *= fx;
            y.y *= fy;
            z.z *= fz;

            Vector3 xy = x + y;
            Vector3 xz = x + z;
            Vector3 yz = y + z;

            x = transform.MultiplyPoint(x);
            y = transform.MultiplyPoint(y);
            z = transform.MultiplyPoint(z);
            xy = transform.MultiplyPoint(xy);
            xz = transform.MultiplyPoint(xz);
            yz = transform.MultiplyPoint(yz);

            GL.Color(selectedAxis != RuntimeHandleAxis.XZ ? YColor : SelectionColor);
            GL.Vertex(position);
            GL.Vertex(z);
            GL.Vertex(z);
            GL.Vertex(xz);
            GL.Vertex(xz);
            GL.Vertex(x);
            GL.Vertex(x);
            GL.Vertex(position);
            GL.Color(selectedAxis != RuntimeHandleAxis.XY ? ZColor : SelectionColor);
            GL.Vertex(position);
            GL.Vertex(y);
            GL.Vertex(y);
            GL.Vertex(xy);
            GL.Vertex(xy);
            GL.Vertex(x);
            GL.Vertex(x);
            GL.Vertex(position);
            GL.Color(selectedAxis != RuntimeHandleAxis.YZ ? XColor : SelectionColor);
            GL.Vertex(position);
            GL.Vertex(y);
            GL.Vertex(y);
            GL.Vertex(yz);
            GL.Vertex(yz);
            GL.Vertex(z);
            GL.Vertex(z);
            GL.Vertex(position);
            GL.End();

            GL.Begin(GL.QUADS);
            GL.Color(YColorTransparent);
            GL.Vertex(position);
            GL.Vertex(z);
            GL.Vertex(xz);
            GL.Vertex(x);
            GL.Color(ZColorTransparent);
            GL.Vertex(position);
            GL.Vertex(y);
            GL.Vertex(xy);
            GL.Vertex(x);
            GL.Color(XColorTransparent);
            GL.Vertex(position);
            GL.Vertex(y);
            GL.Vertex(yz);
            GL.Vertex(z);
            GL.End();

            ShapesMaterial.SetPass(0);
            Graphics.DrawMeshNow(Arrows, transform);
            if(selectedAxis == RuntimeHandleAxis.X)
            {
                Graphics.DrawMeshNow(SelectionArrowX, transform);
            }
            else if (selectedAxis == RuntimeHandleAxis.Y)
            {
                Graphics.DrawMeshNow(SelectionArrowY, transform);
            }
            else if (selectedAxis == RuntimeHandleAxis.Z)
            {
                Graphics.DrawMeshNow(SelectionArrowZ, transform);
            }
        }

        public static void DoRotationHandle(Quaternion rotation, Vector3 position,  RuntimeHandleAxis selectedAxis = RuntimeHandleAxis.None)
        {
            float screenScale = GetScreenScale(position, Camera.current);
            float radius = 1;
            Vector3 scale = new Vector3(screenScale, screenScale, screenScale);
            Matrix4x4 xTranform = Matrix4x4.TRS(Vector3.zero, rotation * Quaternion.AngleAxis(-90, Vector3.up), Vector3.one);
            Matrix4x4 yTranform = Matrix4x4.TRS(Vector3.zero, rotation * Quaternion.AngleAxis(-90, Vector3.right), Vector3.one);
            Matrix4x4 zTranform = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
            Matrix4x4 objToWorld = Matrix4x4.TRS(position, Quaternion.identity, scale);
       
            LinesClipMaterial.SetPass(0);
            GL.PushMatrix();
            GL.MultMatrix(objToWorld);
            
            GL.Begin(GL.LINES);
            GL.Color(selectedAxis != RuntimeHandleAxis.X ? XColor : SelectionColor);
            DrawCircle(xTranform, radius);
            GL.Color(selectedAxis != RuntimeHandleAxis.Y ? YColor : SelectionColor);
            DrawCircle(yTranform, radius);
            GL.Color(selectedAxis != RuntimeHandleAxis.Z ? ZColor : SelectionColor);
            DrawCircle(zTranform, radius);
            GL.End();

            GL.PopMatrix();

            LinesBillboardMaterial.SetPass(0);
            GL.PushMatrix();
            GL.MultMatrix(objToWorld);

            GL.Begin(GL.LINES);
            GL.Color(selectedAxis != RuntimeHandleAxis.Free ? AltColor : SelectionColor);
            DrawCircle(Matrix4x4.identity, radius);
            GL.Color(selectedAxis != RuntimeHandleAxis.Screen ? AltColor : SelectionColor);
            DrawCircle(Matrix4x4.identity, radius * 1.1f);
            GL.End();

            GL.PopMatrix();

        }

        private static void DrawCircle(Matrix4x4 transform, float radius)
        {
            const int pointsPerCircle = 32;
            float angle = 0.0f;
            float z = 0.0f;
            Vector3 prevPoint = transform.MultiplyPoint(new Vector3(radius, 0, z));
            for (int i = 0; i < pointsPerCircle; i++)
            {
                GL.Vertex(prevPoint);
                angle += 2 * Mathf.PI / pointsPerCircle;
                float x = radius * Mathf.Cos(angle);
                float y = radius * Mathf.Sin(angle);
                Vector3 point = transform.MultiplyPoint(new Vector3(x, y, z));
                GL.Vertex(point);
                prevPoint = point;
            }
        }

        public static void DoScaleHandle(Vector3 scale, Vector3 position, Quaternion rotation, RuntimeHandleAxis selectedAxis = RuntimeHandleAxis.None)
        {
            float sScale = GetScreenScale(position, Camera.current);
            Matrix4x4 linesTransform = Matrix4x4.TRS(position, rotation, scale * sScale);

            LinesMaterial.SetPass(0);

            GL.Begin(GL.LINES);
            DoAxes(position, linesTransform, selectedAxis);
            GL.End();

            Matrix4x4 rotM = Matrix4x4.TRS(Vector3.zero, rotation, scale);
            ShapesMaterial.SetPass(0);
            Vector3 screenScale = new Vector3(sScale, sScale, sScale);
            Vector3 xOffset = rotM.MultiplyVector(Vector3.right) * sScale;
            Vector3 yOffset = rotM.MultiplyVector(Vector3.up) * sScale;
            Vector3 zOffset = rotM.MultiplyVector(Vector3.forward) * sScale;
            if (selectedAxis == RuntimeHandleAxis.X)
            {
                Graphics.DrawMeshNow(SelectionCube, Matrix4x4.TRS(position + xOffset, rotation, screenScale));
                Graphics.DrawMeshNow(CubeY, Matrix4x4.TRS(position + yOffset, rotation, screenScale));
                Graphics.DrawMeshNow(CubeZ, Matrix4x4.TRS(position + zOffset, rotation, screenScale));
                Graphics.DrawMeshNow(CubeUniform, Matrix4x4.TRS(position, rotation, screenScale * 1.35f));
            }
            else if (selectedAxis == RuntimeHandleAxis.Y)
            {
                Graphics.DrawMeshNow(CubeX, Matrix4x4.TRS(position + xOffset, rotation, screenScale));
                Graphics.DrawMeshNow(SelectionCube, Matrix4x4.TRS(position + yOffset, rotation, screenScale));
                Graphics.DrawMeshNow(CubeZ, Matrix4x4.TRS(position + zOffset, rotation, screenScale));
                Graphics.DrawMeshNow(CubeUniform, Matrix4x4.TRS(position, rotation, screenScale * 1.35f));
            }
            else if (selectedAxis == RuntimeHandleAxis.Z)
            {
                Graphics.DrawMeshNow(CubeX, Matrix4x4.TRS(position + xOffset, rotation, screenScale));
                Graphics.DrawMeshNow(CubeY, Matrix4x4.TRS(position + yOffset, rotation, screenScale));
                Graphics.DrawMeshNow(SelectionCube, Matrix4x4.TRS(position + zOffset, rotation, screenScale));
                Graphics.DrawMeshNow(CubeUniform, Matrix4x4.TRS(position, rotation, screenScale * 1.35f));
            }
            else if(selectedAxis == RuntimeHandleAxis.Free)
            {
                Graphics.DrawMeshNow(CubeX, Matrix4x4.TRS(position + xOffset, rotation, screenScale));
                Graphics.DrawMeshNow(CubeY, Matrix4x4.TRS(position + yOffset, rotation, screenScale));
                Graphics.DrawMeshNow(CubeZ, Matrix4x4.TRS(position + zOffset, rotation, screenScale));
                Graphics.DrawMeshNow(SelectionCube, Matrix4x4.TRS(position, rotation, screenScale * 1.35f));
            }
            else
            {
                Graphics.DrawMeshNow(CubeX, Matrix4x4.TRS(position + xOffset, rotation, screenScale));
                Graphics.DrawMeshNow(CubeY, Matrix4x4.TRS(position + yOffset, rotation, screenScale));
                Graphics.DrawMeshNow(CubeZ, Matrix4x4.TRS(position + zOffset, rotation, screenScale));
                Graphics.DrawMeshNow(CubeUniform, Matrix4x4.TRS(position, rotation, screenScale * 1.35f));
            }
        }
    }

}
