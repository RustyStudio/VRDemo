using InteractionDemo.Core;
using InteractionDemo.Interaction;
using InteractionDemo.WorldObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace InteractionDemo.SwordZone
{
    public class TestSlicer:MonoBehaviour
    {
        public bool Slice = false;

        public BasicSlicable Target;

        public Vector3 PlaneNormal;

        public Transform PlanePoint;

        public Color planeColor;

        public Color targetColor;

        void OnDrawGizmos()
        {
            DrawPlane(Target.transform.InverseTransformPoint(transform.position), Target.transform.InverseTransformDirection(PlaneNormal), targetColor);
            DrawPlane(PlanePoint.position, PlaneNormal, planeColor);
        }

        void Update()
        {
            if (Slice)
            {
                Slice = false;
                BeginSlicing();
            }
        }

        void BeginSlicing()
        {
            Plane p = new Plane(Target.transform.InverseTransformDirection(PlaneNormal), Target.transform.InverseTransformPoint(transform.position));
            Target.Slice(p);
        }

        void DrawPlane(Vector3 position,  Vector3 normal, Color col)
        {

            var v3 = new Vector3();

            if (normal.normalized != Vector3.forward)
                v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
            else
                v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

            var corner0 = position + v3;
            var corner2 = position - v3;
            var q = Quaternion.AngleAxis(90f, normal);
            v3 = q * v3;
            var corner1 = position + v3;
            var corner3 = position - v3;

            Debug.DrawLine(corner0, corner2, col);
            Debug.DrawLine(corner1, corner3, col);
            Debug.DrawLine(corner0, corner1, col);
            Debug.DrawLine(corner1, corner2, col);
            Debug.DrawLine(corner2, corner3, col);
            Debug.DrawLine(corner3, corner0, col);
            Debug.DrawRay(position, normal, Color.red);
        }
    }
}
