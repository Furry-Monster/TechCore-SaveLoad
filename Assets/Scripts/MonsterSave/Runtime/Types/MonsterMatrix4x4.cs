// ReSharper disable InconsistentNaming

using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    [Serializable]
    public struct MonsterMatrix4x4
    {
        public float m00, m01, m02, m03;
        public float m10, m11, m12, m13;
        public float m20, m21, m22, m23;
        public float m30, m31, m32, m33;

        public MonsterMatrix4x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m03 = m03;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m30 = m30;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        public MonsterMatrix4x4(MonsterMatrix4x4 other)
        {
            m00 = other.m00;
            m01 = other.m01;
            m02 = other.m02;
            m03 = other.m03;
            m10 = other.m10;
            m11 = other.m11;
            m12 = other.m12;
            m13 = other.m13;
            m20 = other.m20;
            m21 = other.m21;
            m22 = other.m22;
            m23 = other.m23;
            m30 = other.m30;
            m31 = other.m31;
            m32 = other.m32;
            m33 = other.m33;
        }

        public MonsterMatrix4x4(Matrix4x4 uMat44)
        {
            m00 = uMat44.m00;
            m01 = uMat44.m01;
            m02 = uMat44.m02;
            m03 = uMat44.m03;
            m10 = uMat44.m10;
            m11 = uMat44.m11;
            m12 = uMat44.m12;
            m13 = uMat44.m13;
            m20 = uMat44.m20;
            m21 = uMat44.m21;
            m22 = uMat44.m22;
            m23 = uMat44.m23;
            m30 = uMat44.m30;
            m31 = uMat44.m31;
            m32 = uMat44.m32;
            m33 = uMat44.m33;
        }
    }

    public class Matrix4x4Adapter : ITypeAdapter<Matrix4x4, MonsterMatrix4x4>
    {
        public MonsterMatrix4x4 ConvertToSerializable(Matrix4x4 native)
        {
            return new MonsterMatrix4x4(native);
        }

        public Matrix4x4 ConvertFromSerializable(MonsterMatrix4x4 serializable)
        {
            var mat = new Matrix4x4();
            mat.m00 = serializable.m00;
            mat.m01 = serializable.m01;
            mat.m02 = serializable.m02;
            mat.m03 = serializable.m03;
            mat.m10 = serializable.m10;
            mat.m11 = serializable.m11;
            mat.m12 = serializable.m12;
            mat.m13 = serializable.m13;
            mat.m20 = serializable.m20;
            mat.m21 = serializable.m21;
            mat.m22 = serializable.m22;
            mat.m23 = serializable.m23;
            mat.m30 = serializable.m30;
            mat.m31 = serializable.m31;
            mat.m32 = serializable.m32;
            mat.m33 = serializable.m33;
            return mat;
        }
    }
}