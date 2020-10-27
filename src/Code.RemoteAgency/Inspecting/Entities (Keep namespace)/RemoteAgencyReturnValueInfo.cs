using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    abstract class RemoteAgencyReturnValueInfoBase
    {
        public string PropertyName { get; set; } //Null: No need to put in entity

        public abstract Type DataType { get; }

        public abstract RemoteAgencyReturnValueSource ReturnValueSource { get; }

        public abstract bool IsIncludedInEntity { get; }
        public abstract string GetDefaultPropertyName();
        public abstract IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes();

    }

    interface IRemoteAgencyReturnValueInfoIncludedWhenExceptionThrown
    {
        bool IsIncludedWhenExceptionThrown { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromParameterDefaultValue : RemoteAgencyReturnValueInfoBase
    {
        public override Type DataType => Parameter.ParameterType;
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ParameterDefaultValue;
        public override bool IsIncludedInEntity => false;
        public override string GetDefaultPropertyName() => throw new NotSupportedException();
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            yield break;
        }

        public ParameterInfo Parameter { get; set; }
    }

    enum AsyncMethodOriginalReturnValueDataTypeClass
    {
        NotAsyncMethod,
        Void,
        Task,
        TaskOfType,
        ValueTask,
        ValueTaskOfType
    }


    class RemoteAgencyReturnValueInfoFromReturnValueDefaultValue : RemoteAgencyReturnValueInfoBase
    {
        public override Type DataType => ReturnValueDataType;
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ReturnValueDefaultValue;
        public override bool IsIncludedInEntity => false;
        public override string GetDefaultPropertyName() => throw new NotSupportedException();
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            yield break;
        }

        public Type ReturnValueDataType { get; set; }
        public AsyncMethodOriginalReturnValueDataTypeClass AsyncMethodOriginalReturnValueDataTypeClass { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromReturnValue : RemoteAgencyReturnValueInfoBase
    {
        public override Type DataType => ReturnValueDataType;
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ReturnValue;
        public override bool IsIncludedInEntity => true;
        public override string GetDefaultPropertyName() => "ReturnValue";
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            return SerializerParameterLevelAttributesOnReturnValue.Select(i =>
                new EntityPropertyAttribute(AttributePosition.ReturnValue, i));
        }

        public List<Attribute> SerializerParameterLevelAttributesOnReturnValue { get; set; }
        public Type ReturnValueDataType { get; set; }
        public AsyncMethodOriginalReturnValueDataTypeClass AsyncMethodOriginalReturnValueDataTypeClass { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromParameter : RemoteAgencyReturnValueInfoBase, IRemoteAgencyReturnValueInfoIncludedWhenExceptionThrown
    {
        public override Type DataType => Parameter.ParameterType;
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.Parameter;
        public override bool IsIncludedInEntity => true;
        public override string GetDefaultPropertyName() => Parameter.Name;
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            return SerializerParameterLevelAttributes.Select(i =>
                new EntityPropertyAttribute(AttributePosition.Parameter, i));
        }

        public ParameterInfo Parameter { get; set; }
        public bool IsIncludedWhenExceptionThrown { get; set; }
        public List<Attribute> SerializerParameterLevelAttributes { get; set; }
    }

    abstract class RemoteAgencyReturnValueInfoFromFieldBase : RemoteAgencyReturnValueInfoBase, IRemoteAgencyReturnValueInfoIncludedWhenExceptionThrown
    {
        public override Type DataType => ParameterField.FieldType;
        public override bool IsIncludedInEntity => true;
        public FieldInfo ParameterField { get; set; }
        public bool IsIncludedWhenExceptionThrown { get; set; }
        public List<Attribute> SerializerParameterLevelAttributesOnField { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromParameterField : RemoteAgencyReturnValueInfoFromFieldBase
    {
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ParameterField;
        public override string GetDefaultPropertyName() => Parameter.Name + "_" + ParameterField.Name;
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            return SerializerParameterLevelAttributes.Select(i =>
                new EntityPropertyAttribute(AttributePosition.Parameter, i)).Union(
                SerializerParameterLevelAttributesOnField.Select(i =>
                    new EntityPropertyAttribute(AttributePosition.FieldOfParameter, i)));
        }

        public ParameterInfo Parameter { get; set; }
        public List<Attribute> SerializerParameterLevelAttributes { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromParameterProperty : RemoteAgencyReturnValueInfoBase,
        IRemoteAgencyReturnValueInfoIncludedWhenExceptionThrown
    {
        public override Type DataType => ParameterProperty.PropertyType;
        public override bool IsIncludedInEntity => true;
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ParameterProperty;
        public override string GetDefaultPropertyName() => Parameter.Name + "_" + ParameterProperty.Name;
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            return SerializerParameterLevelAttributes.Select(i =>
                new EntityPropertyAttribute(AttributePosition.Parameter, i)).Union(
                SerializerParameterLevelAttributesOnProperty.Select(i =>
                    new EntityPropertyAttribute(AttributePosition.PropertyOfParameter, i)));
        }

        public ParameterInfo Parameter { get; set; }
        public List<Attribute> SerializerParameterLevelAttributes { get; set; }

        public PropertyInfo ParameterProperty { get; set; }
        public bool IsIncludedWhenExceptionThrown { get; set; }
        public List<Attribute> SerializerParameterLevelAttributesOnProperty { get; set; }
    }


    class RemoteAgencyReturnValueInfoFromParameterHelperProperty : RemoteAgencyReturnValueInfoBase,
        IRemoteAgencyReturnValueInfoIncludedWhenExceptionThrown
    {
        public override Type DataType => ParameterHelperProperty.PropertyType;
        public override bool IsIncludedInEntity => true;
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ParameterHelperProperty;
        public override string GetDefaultPropertyName() => Parameter.Name + "_" + ParameterHelperProperty.Name;
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            return SerializerParameterLevelAttributes.Select(i =>
                new EntityPropertyAttribute(AttributePosition.Parameter, i)).Union(
                SerializerParameterLevelAttributesOnHelperProperty.Select(i =>
                    new EntityPropertyAttribute(AttributePosition.PropertyOfHelper, i)));
        }

        public ParameterInfo Parameter { get; set; }
        public List<Attribute> SerializerParameterLevelAttributes { get; set; } 
        public Type ParameterHelperClass { get; set; }
        public PropertyInfo ParameterHelperProperty { get; set; }
        public bool IsIncludedWhenExceptionThrown { get; set; }
        public List<Attribute> SerializerParameterLevelAttributesOnHelperProperty { get; set; }
    }

    enum RemoteAgencyReturnValueSource
    {
        ParameterDefaultValue,
        ReturnValueDefaultValue,
        ReturnValue,
        Parameter,
        ParameterField,
        ParameterProperty,
        ParameterHelperProperty
    }
}
