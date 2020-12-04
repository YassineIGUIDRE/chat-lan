Imports System.Net
Imports System.IO
Imports System.Net.Sockets
Public Class Form1
    Dim client As TcpClient
    Dim sWriter As StreamWriter

    Sub xLoad() Handles Me.Load
        Me.Text &= " " & TextBox1.Text
    End Sub

    Delegate Sub _xUpdate(ByVal str As String)
    Sub xUpdate(ByVal str As String)
        If InvokeRequired Then
            Invoke(New _xUpdate(AddressOf xUpdate), str)
        Else
            TextBox3.AppendText(str & vbNewLine)
        End If
    End Sub

    Sub read(ByVal ar As IAsyncResult)
        Try
            xUpdate(New StreamReader(client.GetStream).ReadLine)
            client.GetStream.BeginRead(New Byte() {0}, 0, 0, AddressOf read, Nothing)

        Catch ex As Exception
            xUpdate("You have disconnecting from server")
            Exit Sub
        End Try
    End Sub
    Private Sub send(ByVal str As String)
        Try
            sWriter = New StreamWriter(client.GetStream)
            sWriter.WriteLine(str)
            sWriter.Flush()
        Catch ex As Exception
            xUpdate("You're not server")
        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Connect" Then
            Try
                client = New TcpClient(TextBox2.Text, 8000)
                client.GetStream.BeginRead(New Byte() {0}, 0, 0, New AsyncCallback(AddressOf read), Nothing)
                Button1.Text = "Disconnect"
            Catch ex As Exception
                xUpdate("Can't connect to the server!")
            End Try
        Else
            client.Client.Close()
            client = Nothing
            Button1.Text = "Connect"
        End If
    End Sub
    Private Sub TextBox4_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox4.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            send(TextBox1.Text & " says : " & TextBox4.Text)
            TextBox4.Clear()
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        send(TextBox1.Text & " says : " & TextBox4.Text)
        TextBox4.Clear()
    End Sub

    Private Sub xLoad(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class

