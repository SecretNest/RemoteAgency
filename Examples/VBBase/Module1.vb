Imports SecretNest.RemoteAgency

Module Module1
    Dim sites As New Dictionary(Of Guid, RemoteAgencyManagerEncapsulated)
    Dim WithEvents clientSite As New RemoteAgencyManagerEncapsulated(True, False)
    Dim WithEvents serverSite As New RemoteAgencyManagerEncapsulated(False, True)

    Sub Main()
        clientSite.DefaultTargetSiteId = serverSite.SiteId
        sites.Add(clientSite.SiteId, clientSite)
        sites.Add(serverSite.SiteId, serverSite)
        clientSite.Connect()
        serverSite.Connect()

        'More testing code should be written here.

        Console.ReadKey() 'Pause before quit.
    End Sub


    Sub OnMessageForSendingPrepared(sender As Object, e As RemoteAgencyManagerMessageForSendingEventArgs(Of String)) Handles clientSite.MessageForSendingPrepared, serverSite.MessageForSendingPrepared
        'Async mode
        Task.Run(Sub() sites(e.TargetSiteId).ProcessPackagedMessage(e.Message))
    End Sub

End Module
