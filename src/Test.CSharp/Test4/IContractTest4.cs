namespace Test.CSharp.Test4
{
    interface IContractTest4
    {
        T MakeDouble<T>(T value) where T : struct;

        string GetGenericTypeName<T>();
    }
}
