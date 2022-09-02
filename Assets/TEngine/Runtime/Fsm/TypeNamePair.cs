using System;
using System.Runtime.InteropServices;

namespace TEngine
{
    /// <summary>
    /// ���ͺ����Ƶ����ֵ��
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct TypeNamePair : IEquatable<TypeNamePair>
    {
        private readonly Type m_Type;
        private readonly string m_Name;

        /// <summary>
        /// ��ʼ�����ͺ����Ƶ����ֵ����ʵ����
        /// </summary>
        /// <param name="type">���͡�</param>
        public TypeNamePair(Type type)
            : this(type, string.Empty)
        {
        }

        /// <summary>
        /// ��ʼ�����ͺ����Ƶ����ֵ����ʵ����
        /// </summary>
        /// <param name="type">���͡�</param>
        /// <param name="name">���ơ�</param>
        public TypeNamePair(Type type, string name)
        {
            if (type == null)
            {
                throw new Exception("Type is invalid.");
            }

            m_Type = type;
            m_Name = name ?? string.Empty;
        }

        /// <summary>
        /// ��ȡ���͡�
        /// </summary>
        public Type Type
        {
            get
            {
                return m_Type;
            }
        }

        /// <summary>
        /// ��ȡ���ơ�
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// ��ȡ���ͺ����Ƶ����ֵ�ַ�����
        /// </summary>
        /// <returns>���ͺ����Ƶ����ֵ�ַ�����</returns>
        public override string ToString()
        {
            if (m_Type == null)
            {
                throw new Exception("Type is invalid.");
            }

            string typeName = m_Type.FullName;
            return string.IsNullOrEmpty(m_Name) ? typeName : string.Format("{0}.{1}", typeName, m_Name);
        }

        /// <summary>
        /// ��ȡ����Ĺ�ϣֵ��
        /// </summary>
        /// <returns>����Ĺ�ϣֵ��</returns>
        public override int GetHashCode()
        {
            return m_Type.GetHashCode() ^ m_Name.GetHashCode();
        }

        /// <summary>
        /// �Ƚ϶����Ƿ���������ȡ�
        /// </summary>
        /// <param name="obj">Ҫ�ȽϵĶ���</param>
        /// <returns>���ȽϵĶ����Ƿ���������ȡ�</returns>
        public override bool Equals(object obj)
        {
            return obj is TypeNamePair && Equals((TypeNamePair)obj);
        }

        /// <summary>
        /// �Ƚ϶����Ƿ���������ȡ�
        /// </summary>
        /// <param name="value">Ҫ�ȽϵĶ���</param>
        /// <returns>���ȽϵĶ����Ƿ���������ȡ�</returns>
        public bool Equals(TypeNamePair value)
        {
            return m_Type == value.m_Type && m_Name == value.m_Name;
        }

        /// <summary>
        /// �ж����������Ƿ���ȡ�
        /// </summary>
        /// <param name="a">ֵ a��</param>
        /// <param name="b">ֵ b��</param>
        /// <returns>���������Ƿ���ȡ�</returns>
        public static bool operator ==(TypeNamePair a, TypeNamePair b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// �ж����������Ƿ���ȡ�
        /// </summary>
        /// <param name="a">ֵ a��</param>
        /// <param name="b">ֵ b��</param>
        /// <returns>���������Ƿ���ȡ�</returns>
        public static bool operator !=(TypeNamePair a, TypeNamePair b)
        {
            return !(a == b);
        }
    }
}
