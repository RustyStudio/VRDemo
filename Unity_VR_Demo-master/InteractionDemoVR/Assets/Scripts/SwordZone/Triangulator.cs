using InteractionDemo.Core;
using InteractionDemo.Interaction;
using InteractionDemo.WorldObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace InteractionDemo.SwordZone
{
    public struct IndexedVertex
    {
        public int Index;

        public Vector3 Vertex;
    }


    public class IndexedVertexComparator : IComparer<IndexedVertex>
    {
        public int Compare(IndexedVertex x, IndexedVertex y)
        {
            var res = Triangulator.less((IndexedVertex)x, (IndexedVertex)y);
            if (res)
                return -1;
            else
                return 1;
        }
        
    }

    public static class Triangulator
    {
        public static Vector3 center;

        public static bool less(IndexedVertex a, IndexedVertex b)
        {
            if (a.Vertex.x - center.x >= 0 && b.Vertex.x - center.x < 0)
                return true;
            if (a.Vertex.x - center.x < 0 && b.Vertex.x - center.x >= 0)
                return false;
            if (a.Vertex.x - center.x == 0 && b.Vertex.x - center.x == 0)
            {
                if (a.Vertex.z - center.z >= 0 || b.Vertex.z - center.z >= 0)
                    return a.Vertex.z > b.Vertex.z;
                return b.Vertex.z > a.Vertex.z;
            }

            // compute the cross product of vectors (center -> a) x (center -> b)
            var det = (a.Vertex.x - center.x) * (b.Vertex.z - center.z) - (b.Vertex.x - center.x) * (a.Vertex.z - center.z);
            if (det < 0)
                return true;
            if (det > 0)
                return false;

            // points a and b are on the same line from the center
            // check which point is closer to the center
            var d1 = (a.Vertex.x - center.x) * (a.Vertex.x - center.x) + (a.Vertex.z - center.z) * (a.Vertex.z - center.z);
            var d2 = (b.Vertex.x - center.x) * (b.Vertex.x - center.x) + (b.Vertex.z - center.z) * (b.Vertex.z - center.z);
            return d1 > d2;
        }


        public static List<int> Triangulate(List<IndexedVertex> m_points)
        {
            List<int> indices = new List<int>();

            int n = m_points.Count;
            if (n < 3)
                return indices;

            int[] V = new int[n];
            if (Area(m_points) > 0)
            {
                for (int v = 0; v < n; v++)
                    V[v] = v;
            }
            else
            {
                for (int v = 0; v < n; v++)
                    V[v] = (n - 1) - v;
            }

            int nv = n;
            int count = 2 * nv;
            for (int m = 0, v = nv - 1; nv > 2;)
            {
                if ((count--) <= 0)
                    return indices;

                int u = v;
                if (nv <= u)
                    u = 0;
                v = u + 1;
                if (nv <= v)
                    v = 0;
                int w = v + 1;
                if (nv <= w)
                    w = 0;

                if (Snip(u, v, w, nv, V, m_points))
                {
                    int a, b, c, s, t;
                    a = V[u];
                    b = V[v];
                    c = V[w];
                    indices.Add(a);
                    indices.Add(b);
                    indices.Add(c);
                    m++;
                    for (s = v, t = v + 1; t < nv; s++, t++)
                        V[s] = V[t];
                    nv--;
                    count = 2 * nv;
                }
            }

            indices.Reverse();
            return indices;
        }

        private static float Area(List<IndexedVertex> m_points)
        {
            int n = m_points.Count;
            float A = 0.0f;
            for (int p = n - 1, q = 0; q < n; p = q++)
            {
                Vector3 pval = m_points[p].Vertex;
                Vector3 qval = m_points[q].Vertex;
                A += pval.x * qval.z - qval.x * pval.z;
            }
            return (A * 0.5f);
        }

        private static bool Snip(int u, int v, int w, int n, int[] V, List<IndexedVertex> m_points)
        {
            int p;
            Vector3 A = m_points[V[u]].Vertex;
            Vector3 B = m_points[V[v]].Vertex;
            Vector3 C = m_points[V[w]].Vertex;
            if (Mathf.Epsilon > (((B.x - A.x) * (C.z - A.z)) - ((B.z - A.z) * (C.x - A.x))))
                return false;
            for (p = 0; p < n; p++)
            {
                if ((p == u) || (p == v) || (p == w))
                    continue;
                Vector3 P = m_points[V[p]].Vertex;
                if (InsideTriangle(A, B, C, P))
                    return false;
            }
            return true;
        }

        private static bool InsideTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
        {
            float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
            float cCROSSap, bCROSScp, aCROSSbp;

            ax = C.x - B.x; ay = C.z - B.z;
            bx = A.x - C.x; by = A.z - C.z;
            cx = B.x - A.x; cy = B.z - A.z;
            apx = P.x - A.x; apy = P.z - A.z;
            bpx = P.x - B.x; bpy = P.z - B.z;
            cpx = P.x - C.x; cpy = P.z - C.z;

            aCROSSbp = ax * bpy - ay * bpx;
            cCROSSap = cx * apy - cy * apx;
            bCROSScp = bx * cpy - by * cpx;

            return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
        }
    }
}
