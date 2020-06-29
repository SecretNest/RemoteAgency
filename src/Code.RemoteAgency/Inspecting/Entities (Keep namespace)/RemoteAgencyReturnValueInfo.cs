﻿using System;
using System.Collections.Generic;
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

        public ParameterInfo Parameter { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromReturnValueDefaultValue : RemoteAgencyReturnValueInfoBase
    {
        public override Type DataType => ReturnValueDataType;
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ReturnValueDefaultValue;
        public override bool IsIncludedInEntity => false;
        public override string GetDefaultPropertyName() => throw new NotSupportedException();

        public Type ReturnValueDataType { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromReturnValue : RemoteAgencyReturnValueInfoBase
    {
        public override Type DataType => ReturnValueDataType;
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ReturnValue;
        public override bool IsIncludedInEntity => true;
        public override string GetDefaultPropertyName() => "ReturnValue";

        public Type ReturnValueDataType { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromParameter : RemoteAgencyReturnValueInfoBase, IRemoteAgencyReturnValueInfoIncludedWhenExceptionThrown
    {
        public override Type DataType => Parameter.ParameterType;
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.Parameter;
        public override bool IsIncludedInEntity => true;
        public override string GetDefaultPropertyName() => Parameter.Name;

        public ParameterInfo Parameter { get; set; }
        public bool IsIncludedWhenExceptionThrown { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromParameterField : RemoteAgencyReturnValueInfoBase, IRemoteAgencyReturnValueInfoIncludedWhenExceptionThrown
    {
        public override Type DataType => ParameterField.FieldType;
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ParameterField;
        public override bool IsIncludedInEntity => true;
        public override string GetDefaultPropertyName() => Parameter.Name + "_" + ParameterField.Name;

        public ParameterInfo Parameter { get; set; }
        public FieldInfo ParameterField { get; set; }
        public bool IsIncludedWhenExceptionThrown { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromParameterProperty : RemoteAgencyReturnValueInfoBase, IRemoteAgencyReturnValueInfoIncludedWhenExceptionThrown
    {
        public override Type DataType => ParameterProperty.PropertyType;
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ParameterProperty;
        public override bool IsIncludedInEntity => true;
        public override string GetDefaultPropertyName() => Parameter.Name + "_" + ParameterProperty.Name;

        public ParameterInfo Parameter { get; set; }
        public PropertyInfo ParameterProperty { get; set; }
        public bool IsIncludedWhenExceptionThrown { get; set; }
    }

    class RemoteAgencyReturnValueInfoFromParameterHelperProperty : RemoteAgencyReturnValueInfoBase, IRemoteAgencyReturnValueInfoIncludedWhenExceptionThrown
    {
        public override Type DataType => ParameterHelperProperty.PropertyType;
        public override RemoteAgencyReturnValueSource ReturnValueSource => RemoteAgencyReturnValueSource.ParameterHelperProperty;
        public override bool IsIncludedInEntity => true;
        public override string GetDefaultPropertyName() => Parameter.Name + "_" + ParameterHelperProperty.Name;

        public ParameterInfo Parameter { get; set; }
        public Type ParameterHelperClass { get; set; }
        public PropertyInfo ParameterHelperProperty { get; set; }
        public bool IsIncludedWhenExceptionThrown { get; set; }
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


//class FoundOutputParameter
//{
//public ParameterInfo Parameter { get; set; }
//public bool IsIncludedWhenExceptionThrown { get; set; }
//public string ResponseEntityPropertyName { get; set; }

//public bool IsProperty { get; set; }
//public bool IsField { get; set; }
//public FieldInfo Field { get; set; }
//public PropertyInfo Property { get; set; }
//public bool IsHelperClass { get; set; }
//public Type HelperClass { get; set; }
//}