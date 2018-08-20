using System.Collections.Generic;
using UnityEngine;

namespace InteractionDemo.SwordZone
{
    /// <summary>
    /// Basic 1 submesh slicable
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class BasicSlicable:MonoBehaviour, ISlicable
    {
        private Mesh BaseMesh;

        private Material BaseMaterial;

        private struct VertexInfo
        {
            public Vector3 Point;

            public Vector3 Normal;

            public Vector2 UV;

            public VertexInfo(Vector3 p, Vector3 n, Vector2 uv)
            {
                Point = p;
                Normal = n;
                UV = uv;
            }
        }

        private class AssignedVertex
        {
            public int AssignedIndex;

            public Assignment Assignment;
        }
        
      
        private struct IntKey
        {
            private const int HASH = 65537;
            public int A;
            public int B;

            public IntKey(int a, int b)
            {
                A = a;
                B = b;
            }

            public override bool Equals(object obj)
            {
                if (obj is IntKey)
                {
                    var obj2 = (IntKey)obj;
                    return (A == obj2.A && B == obj2.B) || (B == obj2.A && A == obj2.B);
                }
                else
                    return false;
            }

            public override int GetHashCode()
            {
                var hc = A < B ? A * HASH + B : B * HASH + A;
                return hc;
            }
        }

        private enum Assignment
        {
            unasigned,
            top,
            bottom
        }


        private class CiclycIndexer
        {
            public int cycleLength;

            public int GetNext(int index)
            {
                if (index < cycleLength - 1)
                    return index++;
                else
                    return 0;
            }
        }

        void Awake()
        {
            BaseMesh =  GetComponent<MeshFilter>().sharedMesh;
            BaseMaterial = GetComponent<MeshRenderer>().sharedMaterial;
        }

        public void Slice(Plane p)
        {
            var points = BaseMesh.vertices;
            var indicies = BaseMesh.GetIndices(0);
            var uvs = BaseMesh.uv;
            var normals = BaseMesh.normals;
            var asVert = new AssignedVertex[points.Length];

            var SplittedVertices = new List<Vector3>[3];
            var SplittedNormals = new List<Vector3>[3];
            var SplittedUVS = new List<Vector2>[3];
            var SplittedIndices = new List<int>[3];
            var SplittedBorderVertices = new Dictionary<IntKey, int>[3];

            SplittedVertices[(int)Assignment.bottom] = new List<Vector3>(points.Length);
            SplittedVertices[(int)Assignment.top] = new List<Vector3>(points.Length);
            SplittedUVS[(int)Assignment.bottom] = new List<Vector2>(uvs.Length);
            SplittedUVS[(int)Assignment.top] = new List<Vector2>(uvs.Length);
            SplittedNormals[(int)Assignment.bottom] = new List<Vector3>(points.Length);
            SplittedNormals[(int)Assignment.top] = new List<Vector3>(points.Length);
            SplittedIndices[(int)Assignment.bottom] = new List<int>(indicies.Length);
            SplittedIndices[(int)Assignment.top] = new List<int>(indicies.Length);

            SplittedBorderVertices[(int)Assignment.bottom] = new Dictionary<IntKey, int>(points.Length);
            SplittedBorderVertices[(int)Assignment.top] = new Dictionary<IntKey, int>(points.Length);

            var BorderPoints = new Dictionary<IntKey, VertexInfo>(points.Length);

            for (int i = 0; i < points.Length; i++)
            {
                asVert[i] = new AssignedVertex() { Assignment = Assignment.unasigned };
                asVert[i].Assignment = p.GetSide(points[i]) ? Assignment.top : Assignment.bottom;
                var curAssignment = asVert[i].Assignment;
                asVert[i].AssignedIndex = SplittedVertices[(int)curAssignment].Count;
                SplittedVertices[(int)curAssignment].Add(points[i]);
                SplittedNormals[(int)curAssignment].Add(normals[i]);
                SplittedUVS[(int)curAssignment].Add(uvs[i]);
            }
            for (int i = 0; i < indicies.Length; i += 3)
            {
                var currentAssignment = Assignment.unasigned;
                bool triangleIsSameSide = true;
                for (int j = 0; j < 3; j++)
                {
                    var offsetedIndex = indicies[i + j];
                    if (currentAssignment == Assignment.unasigned)
                    {
                        currentAssignment = asVert[offsetedIndex].Assignment;
                    }
                    else
                    {
                        if (asVert[offsetedIndex].Assignment != currentAssignment)
                            triangleIsSameSide = false;
                    }
                }
                if (triangleIsSameSide)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        SplittedIndices[(int)currentAssignment].Add(asVert[indicies[i + j]].AssignedIndex);
                    }
                }
                else
                {
                    var IntersectionPoints = FindPlaneIntersectionForTriangle(points, normals, uvs, p, indicies[i], indicies[i + 1], indicies[i + 2], ref BorderPoints);
                    for (int z = 0; z < IntersectionPoints.Length; z++)
                    {
                        for (int k = 1; k < 3; k++)
                        {
                            if (!SplittedBorderVertices[k].ContainsKey(IntersectionPoints[z]))
                            {
                                SplittedBorderVertices[k][IntersectionPoints[z]] = SplittedVertices[k].Count;
                                var workingVertex = BorderPoints[IntersectionPoints[z]];
                                SplittedVertices[k].Add(workingVertex.Point);
                                SplittedNormals[k].Add(workingVertex.Normal);
                                SplittedUVS[k].Add(workingVertex.UV);
                            }
                        }
                    }
                    var upperTri = new List<int>(6);
                    var lowerTri = new List<int>(6);
                    var startingAssignment = asVert[indicies[i]].Assignment;
                    bool splitInserted = false;
                    for (int z = 0; z < 3; z++)
                    {
                        var offset = i + z;
                        var vertexAssignment = asVert[indicies[offset]];
                        if (z == 2 && vertexAssignment.Assignment == startingAssignment)
                        {
                            InsertBorderVertices(startingAssignment, ref upperTri,
                             ref lowerTri,
                             ref IntersectionPoints,
                             ref SplittedBorderVertices[(int)Assignment.top],
                             ref SplittedBorderVertices[(int)Assignment.bottom]);
                            splitInserted = true;
                        }
                        switch (vertexAssignment.Assignment)
                        {
                            case Assignment.top:
                                {
                                    upperTri.Add(vertexAssignment.AssignedIndex);
                                }
                                break;

                            case Assignment.bottom:
                                {
                                    lowerTri.Add(vertexAssignment.AssignedIndex);
                                }
                                break;
                        }

                    }
                    if (!splitInserted)
                    {
                        InsertBorderVertices(startingAssignment, ref upperTri,
                            ref lowerTri,
                            ref IntersectionPoints,
                            ref SplittedBorderVertices[(int)Assignment.top],
                            ref SplittedBorderVertices[(int)Assignment.bottom]);
                    }
                    var upperTriangle = TriangulateSplit(SplittedNormals[(int)Assignment.top][upperTri[0]], ref SplittedVertices[(int)Assignment.top], upperTri);
                    var lowerTriangle = TriangulateSplit(SplittedNormals[(int)Assignment.bottom][lowerTri[0]], ref SplittedVertices[(int)Assignment.bottom], lowerTri);
                 
                    SplittedIndices[(int)Assignment.top].AddRange(upperTriangle);
                    SplittedIndices[(int)Assignment.bottom].AddRange(lowerTriangle);
                }


            }
            for (int z = 1; z < 3; z++)
            {
                List<int> borderIndicies = new List<int>(SplittedBorderVertices[z].Count);
                int k = 1;
                if ((Assignment)z == Assignment.top)
                    k = -1;
                var curNormal = p.normal * k;
                foreach (var item in SplittedBorderVertices[z].Values)
                {
                    borderIndicies.Add(SplittedVertices[z].Count);
                    SplittedVertices[z].Add(SplittedVertices[z][item]);
                    SplittedNormals[z].Add(curNormal);
                    SplittedUVS[z].Add(SplittedUVS[z][item]);
                }
               SplittedIndices[z].AddRange(TriangulateBorder(borderIndicies, SplittedVertices[z], curNormal));
                if (SplittedIndices[z].Count > 0)
                {
                    var mesh = ConstructMeshFromPoints(SplittedIndices[z].ToArray(), SplittedVertices[z], SplittedNormals[z], SplittedUVS[z]);
                    GenerateNewMeshObject(mesh);
                }
            }
            GameObject.Destroy(gameObject);
        }

        private void InsertBorderVertices(Assignment startAssignment, ref List<int>upperTri, ref List<int> lowerTri, ref IntKey[] keys, ref Dictionary<IntKey, int> UpperDict, ref Dictionary<IntKey, int> LowerDict)
        {
            switch (startAssignment)
            {
                case Assignment.top:
                    {
                        upperTri.Add(UpperDict[keys[0]]);
                        upperTri.Add(UpperDict[keys[1]]);
                        lowerTri.Add(LowerDict[keys[1]]);
                        lowerTri.Add(LowerDict[keys[0]]);
                    }
                    break;
                case Assignment.bottom:
                    {
                        upperTri.Add(UpperDict[keys[1]]);
                        upperTri.Add(UpperDict[keys[0]]);
                        lowerTri.Add(LowerDict[keys[0]]);
                        lowerTri.Add(LowerDict[keys[1]]);
                    }
                    break;
            }
        }

        private List<int> TriangulateBorder(List<int> borderIndicies, List<Vector3> Points, Vector3 PlaneNormal)
        {
            var res = new List<int>();         
            var centerPoint = new Vector3();
            var rotatedPoints = new List<IndexedVertex>(borderIndicies.Count);
            for (int i = 0; i < borderIndicies.Count; i++)
            {
                centerPoint += Points[borderIndicies[i]];
                rotatedPoints.Add(new IndexedVertex() { Index = borderIndicies[i], Vertex = Points[borderIndicies[i]] });
            }
            centerPoint /= borderIndicies.Count;
            Triangulator.center = centerPoint;
          
            var rotation = Quaternion.FromToRotation(PlaneNormal, Vector3.up);
            for (int i = 0; i < rotatedPoints.Count; i++)
            {
               var rotated = RotatePointArroundOrigin(rotatedPoints[i].Vertex, centerPoint, rotation);
                rotatedPoints[i] = new IndexedVertex() { Index = rotatedPoints[i].Index, Vertex = rotated };
            }

            rotatedPoints.Sort(new IndexedVertexComparator());
            var rotatedTris = Triangulator.Triangulate(rotatedPoints);

            for (int i = 0; i < rotatedPoints.Count - 2; i++)
            {
                res.AddRange(new int[] { rotatedPoints[0].Index, rotatedPoints[i+1].Index, rotatedPoints[i+2].Index });
            }
          
            return res;
        }
        

     

        private Vector3 RotatePointArroundOrigin(Vector3 point, Vector3 pivot, Quaternion rotation)
        {
            return rotation * (point - pivot) + pivot;
        }

        private int[] TriangulateSplit(Vector3 normal, ref List<Vector3> points, List<int> indices)
        {

            if (indices.Count == 3)
            {
                return new int[]
                 {
                    indices[0],indices[1],indices[2]
                 };
            }
            else
            {
                return new int[]
                {
                    indices[0],indices[1],indices[2],
                    indices[0],indices[2],indices[3]
                };
            }

        }

        private IntKey[] FindPlaneIntersectionForTriangle(Vector3[] points, Vector3[] normals, Vector2[] uv, Plane p, int a, int b, int c, ref Dictionary<IntKey, VertexInfo> CollisionPoints)
        {
            int contactCounter = 0;
            var ContactPoints = new IntKey[2];
            var testKeys = new IntKey[] { new IntKey(a, b), new IntKey(b, c), new IntKey(a, c) };
            for (int i = 0; i < testKeys.Length; i++)
            {
                if (CollisionPoints.ContainsKey(testKeys[i]))
                {
                    ContactPoints[contactCounter] = testKeys[i];
                    contactCounter++;
                }
                else
                {
                    Vector3 contactPoint;
                    if (GetPointIntersection(points[testKeys[i].A], points[testKeys[i].B], p, out contactPoint))
                    {

                        CollisionPoints[testKeys[i]] = new VertexInfo(contactPoint, (normals[testKeys[i].A] + normals[testKeys[i].B]).normalized, (uv[testKeys[i].A] + uv[testKeys[i].B])/2);
                        ContactPoints[contactCounter] = testKeys[i];
                        contactCounter++;
                    }
                }
            }
            return ContactPoints;
        }

        private bool GetPointIntersection(Vector3 a, Vector3 b, Plane p, out Vector3 point)
        {         
            var ray = new Ray(a, b - a);            
            var dist = 0f;
            if (p.Raycast(ray, out dist))
            {
                if (dist * dist > (b - a).sqrMagnitude)
                {
                    point = Vector3.zero;
                    return false;
                }
                point = a + (b - a).normalized * dist;
                return true;
            }
            point = Vector3.zero;
            return false;
        }


        private Mesh ConstructMeshFromPoints(int[] indices, List<Vector3> points, List<Vector3> normals, List<Vector2> uvs)
        {
            var mesh = new Mesh();
            mesh.SetVertices(points);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            return mesh;
        }

        private void GenerateNewMeshObject(Mesh m)
        {
            var newObject = new GameObject("PART");
            newObject.AddComponent<MeshFilter>().sharedMesh = m;
            newObject.AddComponent<MeshRenderer>().material = BaseMaterial;
            newObject.transform.SetParent(transform, false);
            newObject.transform.SetParent(null);
            newObject.AddComponent<Rigidbody>();
            var col =  newObject.AddComponent<BoxCollider>();
        
            
            newObject.AddComponent<BasicSlicable>();
            newObject.AddComponent<Interaction.RubberBandGrabbableObject>();
            newObject.layer = gameObject.layer;
            newObject.tag = gameObject.tag;
        }         

        private Vector3 GetPointOnPlane(Vector3 a, Vector3 b, Plane p)
        {
            var direction = b - a;
            var testRay = new Ray(a, direction);
            float result = 0;
            if (!p.Raycast(testRay, out result))
            {
                Debug.LogError("ERROR: RAY DOES NOT INTERSECT WITH PLANE");
            }
            return a + direction * result;
        }

        private Vector2 GenerateUV(Vector2 a, Vector2 b)
        {
            return a + (a - b) / 2;
        }
    }
}
