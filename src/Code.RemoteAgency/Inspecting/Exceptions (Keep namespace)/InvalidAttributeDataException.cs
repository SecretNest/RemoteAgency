using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    /// <summary>
    /// The exception that is thrown when the invalid attribute or data within attribute is found.
    /// </summary>
    [Serializable]
    public class InvalidAttributeDataException : Exception
    {
        /// <summary>
        /// Gets the attribute that cause this exception.
        /// </summary>
        public Attribute Attribute { get; }

        /// <summary>
        /// Gets the member path.
        /// </summary>
        public Stack<MemberInfo> MemberPath { get; }

        /// <summary>
        /// Initializes an instance of InvalidAttributeDataException.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="attribute">Attribute that cause this exception.</param>
        /// <param name="memberPath">Member path.</param>
        public InvalidAttributeDataException(string message, Attribute attribute, Stack<MemberInfo> memberPath) : base(message)
        {
            Attribute = attribute;
            MemberPath = new Stack<MemberInfo>(memberPath);
        }

        /// <summary>
        /// Initializes an instance of InvalidAttributeDataException.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="attribute">Attribute that cause this exception.</param>
        /// <param name="memberInfo">Member where this attribute is marked on.</param>
        /// <param name="memberParentPath">Parent path.</param>
        public InvalidAttributeDataException(string message, Attribute attribute, MemberInfo memberInfo, Stack<MemberInfo> memberParentPath) : base(message)
        {
            Attribute = attribute;
            MemberPath = new Stack<MemberInfo>(memberParentPath);
            MemberPath.Push(memberInfo);
        }

        /// <summary>
        /// Initializes a new instance of the InvalidAttributeDataException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected InvalidAttributeDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            var attributeType = (Type)info.GetValue("AttributeType", typeof(Type));
            Attribute = (Attribute)info.GetValue("Attribute", attributeType);
            var count= info.GetInt32("MemberPathDeep");
            MemberInfo[] path = new MemberInfo[count];
            for (int i = 0; i < count; i++)
            {
                path[i] = (MemberInfo) info.GetValue($"MemberPath{i}", typeof(MemberInfo));
            }
            MemberPath = new Stack<MemberInfo>(path);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("AttributeType", Attribute.GetType());
            info.AddValue("Attribute", Attribute);
            var path = MemberPath.ToArray();
            var count = path.Length;
            info.AddValue("MemberPathDeep", count);
            for (int i = 0; i < count; i++)
            {
                info.AddValue($"MemberPath{i}", path[i]);
            }
        }
    }
}
