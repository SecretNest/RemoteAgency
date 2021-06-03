Imports SecretNest.RemoteAgency

Public Class RemoteAgencyRouter(Of TSerialized, TEntityBase)
    Private ReadOnly _instances As New Dictionary(Of Guid, RemoteAgency(Of TSerialized, TEntityBase))

    Public Sub AddRemoteAgencyInstance(instance As RemoteAgency(Of TSerialized, TEntityBase))
        Dim id = instance.SiteId
        _instances.Add(id, instance)
        AddHandler instance.MessageForSendingPrepared, AddressOf Instance_MessageForSendingPrepared
    End Sub

    Public Sub RemoveRemoteAgencyInstance(id As Guid)
        _instances.Remove(id)
    End Sub

    Private Sub Instance_MessageForSendingPrepared(sender As Object, e As MessageBodyEventArgs(Of TSerialized, TEntityBase))
        'serialize
        Dim serialized = e.Serialize()

        Dim targetInstance As RemoteAgency(Of TSerialized, TEntityBase) = Nothing

        If Not _instances.TryGetValue(e.TargetSiteId, targetInstance) Then
            Throw New Exception("Target instance doesn't exist.")
        End If

        'Process without serialization
        'targetInstance.ProcessReceivedMessage(e.MessageBody)

        'Process with serialized data
        Try
            targetInstance.ProcessReceivedSerializedMessage(serialized)
        Catch ex As Exception
            Console.WriteLine($"Processing exception: \n  ExceptionType: {ex.GetType().FullName}\n  ExceptionMessage: {ex.Message}")
        End Try
    End Sub
End Class
