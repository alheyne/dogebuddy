Imports System.Web.Script.Serialization
Imports System.Net
Imports System.IO
Imports System.Text



Public Class Form1

    Private MouseDownLoc As Point 'setup mouse for moving form when it is clicked

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Refresh()
        update_data()
        Dim quit As Integer = 1
        CheckBox1.Checked = My.Settings.ontoponoff
        TextBox1.Text = My.Settings.userwalletaddress
        Timer1.Start() ' start timer to update data


    End Sub

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown

        MouseDownLoc.X = e.X
        MouseDownLoc.Y = e.Y

    End Sub

    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove

        If e.Button = Windows.Forms.MouseButtons.Left Then
            Me.Left += e.X - MouseDownLoc.X
            Me.Top += e.Y - MouseDownLoc.Y
        End If

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        update_data()
        gettradehistory()


    End Sub


    '/////////////////////button click to end program//////////////////////////
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        My.Settings.userwalletaddress = TextBox1.Text
        My.Settings.Save()
        notifyicon1.Visible = False
        MsgBox("If you like this program, please Consider Donating!" & (Chr(13)) & "It will give you a warm fuzzy feeling and" & (Chr(13)) & "I'll send you a copy of dogebuddy without the beg screens :Ð" & (Chr(13)) & "D6FgVwm7LzZwAxXCwAXVUbU9aAjc2C57Rd", MsgBoxStyle.Information)
        End

    End Sub
    '////////////////////////////////////////////////////////////////////////////////////////////////
    '--------------------------------------timer sub------------------------------------------------
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        update_data()
        gettradehistory()
    End Sub
    '//////////////////////////////////////////////////////////////////////////////////////////////////
    Private Sub update_data()

        Dim doge As String
        Dim exchange As String
        Dim dogevalue As String

        gettradehistory()

        If TextBox1.Text = "" Then
            My.Computer.Audio.Play(My.Resources.dog_bark6, AudioPlayMode.Background)
            TextBox2.Text = "Plz Enter Ur Wallet Address"
            Exit Sub
        End If

        dogevalue = GetCSV("https://www.dogeapi.com/wow/?a=get_current_price&convert_to=USD&amount_doge=1")
        GlobalVariables.WalletAddress = TextBox1.Text
        Dim curblock As String = GetCSV("https://www.dogeapi.com/wow/?a=get_current_block")
        exchange = GetCSV("https://moolah.ch/api/rates?f=USD&t=DOGE&a=1")
        doge = GetCSV("http://dogechain.info/chain/Dogecoin/q/addressbalance/" & GlobalVariables.WalletAddress)
        GlobalVariables.difficulty = GetCSV("http://dogechain.info/chain/Dogecoin/q/getdifficulty")
        GlobalVariables.WalletBalance = GetCSV("http://dogechain.info/chain/Dogecoin/q/addressbalance/" & GlobalVariables.WalletAddress)
        Dim totalcirculation As String = GetCSV("https://dogechain.info/chain/Dogecoin/q/totalbc")

        TextBox4.Text = FormatNumber(totalcirculation, 0)
        curblock = curblock.Replace(Chr(34), "")
        TextBox2.Refresh()
        Label3.Text = FormatCurrency(dogevalue, 6)
        Label7.Text = ("Such difficult: " & GlobalVariables.difficulty)
        Label5.Text = "Ð" & FormatNumber(exchange, 5)
        Label8.Text = ("Current Block - " & curblock)
        '//////////////////////////////////// update the wallet balance //////////////////////
        If IsNumeric(GlobalVariables.WalletBalance) = True Then

            TextBox2.Text = "Ð" & FormatNumber(GlobalVariables.WalletBalance, 2)
            TextBox2.Refresh()
            Exit Sub

        Else

            My.Computer.Audio.Play(My.Resources.dog_bark6, AudioPlayMode.Background)
            TextBox2.Text = "Plz Enter Ur Wallet Address"
            Exit Sub

        End If
    End Sub
    '/////////////////////////////////////////////////////////////////////////////////////////////////

    Private Function WalletBalance(p1 As Boolean) As Boolean
        Throw New NotImplementedException
    End Function


    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged

        If CheckBox1.Checked Then
            Me.TopMost = True
        Else
            Me.TopMost = False
        End If

    End Sub
    '///////////////////////////////////////////////////////////////////////////////////////////////////
    Private Sub Form1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDoubleClick

        Timer3.Start()

        Me.WindowState = FormWindowState.Minimized
        Me.Hide()

        notifyicon1.Visible = True
        notifyicon1.BalloonTipText = "Your Wallet Balance = Ð " & GlobalVariables.WalletBalance
        notifyicon1.ShowBalloonTip(500)


    End Sub
    '///////////////////////////////////////////////////////////////////////////////////////////////////

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles notifyicon1.MouseDoubleClick

        Me.Show()
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.visible = False


    End Sub

    '//////////////////////////////// get the trading price///////////////////////////////////////
    '//////////////////////////////////////////////////////////////////////////////////////////////
    Private Sub gettradehistory()

        Dim fixedhistory As String
        fixedhistory = GetCSV("http://www.cryptocoincharts.info/v2/api/tradingPair/doge_ltc")
        fixedhistory = Replace(fixedhistory, "{", "")
        fixedhistory = Replace(fixedhistory, "id", "")
        fixedhistory = Replace(fixedhistory, Chr(34), "")
        fixedhistory = Replace(fixedhistory, Chr(30), "")
        fixedhistory = Replace(fixedhistory, Chr(47), "")
        fixedhistory = Replace(fixedhistory, Chr(92), "")
        fixedhistory = Replace(fixedhistory, "price", "")
        fixedhistory = Replace(fixedhistory, Chr(58), "")
        fixedhistory = Replace(fixedhistory, Chr(44), "")
        fixedhistory = Replace(fixedhistory, "", "")
        fixedhistory = Replace(fixedhistory, "doge", "")
        fixedhistory = Replace(fixedhistory, "ltc", "")

        GlobalVariables.lastprice = Microsoft.VisualBasic.Left(fixedhistory, 10)
        TextBox3.Text = GlobalVariables.lastprice

    End Sub

    '///////////////////////////////////////////////////////////////////////////////////
    '////////////////////////////////////////////////////////////////////////////////////
    'Dim request As WebRequest = _
    '      WebRequest.Create("http://pubapi.cryptsy.com/api.php?method=singlemarketdata&marketid=135")
    ' If required by the server, set the credentials.
    '    request.Credentials = CredentialCache.DefaultCredentials
    ' Get the response.
    '    Dim response As WebResponse = request.GetResponse()
    ' Display the status.
    '    Console.WriteLine(CType(response, HttpWebResponse).StatusDescription)
    ' Get the stream containing content returned by the server.
    '    Dim dataStream As Stream = response.GetResponseStream()
    ' Open the stream using a StreamReader for easy access.
    '    Dim reader As New StreamReader(dataStream)
    ' Read the content.
    '    Dim responseFromServer As String = reader.ReadToEnd()
    ' Clean up the streams and the response.
    '    TextBox3.Text = responseFromServer
    '    reader.Close()
    '    response.Close()

    '    GlobalVariables.currentvalue = TextBox3.Text
    '////////////////////////////  Timer 3    //////////////////////////////////////////////////

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick

        notifyicon1.BalloonTipText = " Last Trade Price = Ð" & GlobalVariables.lastprice
        notifyicon1.ShowBalloonTip(200)
        Exit Sub

    End Sub
    '///////////////////////////////////////////////////////////////////////////////////////////

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        notifyicon1.Visible = False
        MsgBox("If you like this program, please Consider Donating!" & (Chr(13)) & "It will give you a warm fuzzy feeling and" & (Chr(13)) & "I'll send you a copy of dogebuddy without the beg screens :Ð" & (Chr(13)) & "D6FgVwm7LzZwAxXCwAXVUbU9aAjc2C57Rd", MsgBoxStyle.Information)
        End
    End Sub

    Private Sub RestoreToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestoreToolStripMenuItem.Click

        Me.Show()
        Me.WindowState = FormWindowState.Normal
        notifyicon1.Visible = False

    End Sub


    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Process.Start("http://www.bigcrazyal.com/dogebuddy.html")

    End Sub
End Class


