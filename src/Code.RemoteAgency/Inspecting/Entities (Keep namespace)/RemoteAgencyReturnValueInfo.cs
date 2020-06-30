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

    abstract class RemoteAgencyReturnValueInfoFromReturnValueDefaultValueBase : RemoteAgencyReturnValueInfoBase
    {
        public override Type DataType => ReturnValueDataType;
        public override bool IsIncludedInEntity => false;
        public override string GetDefaultPropertyName() => throw new NotSupportedException();
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            yield break;
        }

        public Type ReturnValueDataType { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromReturnValueDefaultValue : RemoteAgencyReturnValueInfoFromReturnValueDefaultValueBase
    {
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ReturnValueDefaultValue;
    }

    class RemoteAgencyReturnValueInfoFromAssetPropertyReturnValueDefaultValue : RemoteAgencyReturnValueInfoFromReturnValueDefaultValueBase
    {
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.AssetPropertyReturnValueDefaultValue;
    }

    abstract class RemoteAgencyReturnValueInfoFromReturnValueBase : RemoteAgencyReturnValueInfoBase
    {
        public override Type DataType => ReturnValueDataType;
        public override bool IsIncludedInEntity => true;
        public override string GetDefaultPropertyName() => "ReturnValue";

        public Type ReturnValueDataType { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromReturnValue : RemoteAgencyReturnValueInfoFromReturnValueBase
    {
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ReturnValue;
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            return SerializerParameterLevelAttributesOnReturnValue.Select(i =>
                new EntityPropertyAttribute(AttributePosition.ReturnValue, i));
        }

        public List<Attribute> SerializerParameterLevelAttributesOnReturnValue { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromAssetPropertyReturnValue : RemoteAgencyReturnValueInfoFromReturnValueBase
    {
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.AssetPropertyReturnValue;
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            return SerializerParameterLevelAttributesOnAsset.Select(i =>
                new EntityPropertyAttribute(AttributePosition.AssetProperty, i));
        }

        public List<Attribute> SerializerParameterLevelAttributesOnAsset { get; set; }
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

    class RemoteAgencyReturnValueInfoFromAssetPropertyValueField : RemoteAgencyReturnValueInfoFromFieldBase
    {
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.AssetPropertyValueField;
        public override string GetDefaultPropertyName() => ParameterField.Name;
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            return SerializerParameterLevelAttributesOnAsset.Select(i =>
                new EntityPropertyAttribute(AttributePosition.AssetProperty, i)).Union(
                SerializerParameterLevelAttributesOnField.Select(i =>
                    new EntityPropertyAttribute(AttributePosition.FieldOfParameter, i)));
        }

        public List<Attribute> SerializerParameterLevelAttributesOnAsset { get; set; }
    }

    abstract class RemoteAgencyReturnValueInfoFromPropertyBase : RemoteAgencyReturnValueInfoBase,
        IRemoteAgencyReturnValueInfoIncludedWhenExceptionThrown
    {
        public override Type DataType => ParameterProperty.PropertyType;
        public override bool IsIncludedInEntity => true;

        public PropertyInfo ParameterProperty { get; set; }
        public bool IsIncludedWhenExceptionThrown { get; set; }
        public List<Attribute> SerializerParameterLevelAttributesOnProperty { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromParameterProperty : RemoteAgencyReturnValueInfoFromPropertyBase
    {
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
    }

    class RemoteAgencyReturnValueInfoFromAssetPropertyValueProperty : RemoteAgencyReturnValueInfoFromPropertyBase
    {
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.AssetPropertyValueProperty;
        public override string GetDefaultPropertyName() => ParameterProperty.Name;
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            return SerializerParameterLevelAttributesOnAsset.Select(i =>
                new EntityPropertyAttribute(AttributePosition.AssetProperty, i)).Union(
                SerializerParameterLevelAttributesOnProperty.Select(i =>
                    new EntityPropertyAttribute(AttributePosition.PropertyOfParameter, i)));
        }

        public List<Attribute> SerializerParameterLevelAttributesOnAsset { get; set; }
    }

    abstract class RemoteAgencyReturnValueInfoFromHelperPropertyBase : RemoteAgencyReturnValueInfoBase,
        IRemoteAgencyReturnValueInfoIncludedWhenExceptionThrown
    {
        public override Type DataType => ParameterHelperProperty.PropertyType;
        public override bool IsIncludedInEntity => true;

        public Type ParameterHelperClass { get; set; }
        public PropertyInfo ParameterHelperProperty { get; set; }
        public bool IsIncludedWhenExceptionThrown { get; set; }
        public List<Attribute> SerializerParameterLevelAttributesOnHelperProperty { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromParameterHelperProperty : RemoteAgencyReturnValueInfoFromHelperPropertyBase
    {
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
    } 

    class RemoteAgencyReturnValueInfoFromAssetPropertyValueHelperProperty : RemoteAgencyReturnValueInfoFromHelperPropertyBase
    {
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.AssetPropertyValueHelperProperty;
        public override string GetDefaultPropertyName() => ParameterHelperProperty.Name;
        public override IEnumerable<EntityPropertyAttribute> GetEntityPropertyAttributes()
        {
            return SerializerParameterLevelAttributesOnAsset.Select(i =>
                new EntityPropertyAttribute(AttributePosition.AssetProperty, i)).Union(
                SerializerParameterLevelAttributesOnHelperProperty.Select(i =>
                    new EntityPropertyAttribute(AttributePosition.PropertyOfHelper, i)));
        }

        public List<Attribute> SerializerParameterLevelAttributesOnAsset { get; set; }
    } 

    enum RemoteAgencyReturnValueSource
    {
        ParameterDefaultValue,
        ReturnValueDefaultValue,
        AssetPropertyReturnValueDefaultValue,
        ReturnValue,
        AssetPropertyReturnValue,
        Parameter,
        ParameterField,
        AssetPropertyValueField,
        ParameterProperty,
        AssetPropertyValueProperty,
        ParameterHelperProperty,
        AssetPropertyValueHelperProperty
    }
}
