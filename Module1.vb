Imports System
Imports System.IO
Imports System.Net
Imports Microsoft.JScript





Module Module1
    Sub Main()

    End Sub




    Function GetCSV(ByVal Url)
        Dim Content
        Dim OutputFile
        Try

            Dim _WebClient As WebClient = New WebClient
            Dim _Response As Stream = _WebClient.OpenRead(Url)
            Dim _StreamReader As New IO.StreamReader(_Response)

            Content = _StreamReader.ReadToEnd()
            _StreamReader.Close()
        Catch ex As Exception
            Return " Plz entr valid Address"
        End Try

        Content = Trim(Content)
        OutputFile = Content
        Return OutputFile

    End Function




End Module
