using System;
using System.Collections.Generic;
using System.Text;
using Rimage.Client.Api;
using Rimage.Client.Api.Exception;
using System.Xml;
using System.Xml.Schema;
using System.Collections;
using System.IO;
using System.Windows.Forms;


namespace RimageKorea
{
    public class ServerEventListener : Form, IServerEventListener
    {
        public override void OnServerActive(string serverInfo)
        {
            try
            {
                //MessageBox.Show(serverInfo)

                //Dim config As New ProductionServerConfiguration
                //config = New ProductionServerConfiguration
                //config.Load(serverInfo)

                //may want to throw the config object to the GUI
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception");
            }
        }
        public override void OnServerAlert(string xmlAlert) 
        {
            Dim dialog As Dialog
            if (xmlAlert.IndexOf("AlertDialog") == -1)
            {
                MessageBox.Show(xmlAlert, "Error Dialog", MessageBoxButtons.OKCancel);
            }
            else
            {

            }
        }

        Dim dialog As Dialog
        Try
            If xmlAlert.IndexOf("AlertDialog") = -1 Then

                MessageBox.Show(xmlAlert, "Error Dialog", MessageBoxButtons.OKCancel)
            Dialog = New ErrorDialog
                Else
                MessageBox.Show(xmlAlert, "Alert Dialog", MessageBoxButtons.OKCancel)
            Dialog = New AlertDialog
                End If

            ' Parse the dialog
            dialog.GetType()
            dialog.Load(xmlAlert)

            ' Throw the dialog object to the GUI

        Catch ex As System.Exception
            MessageBox.Show("Exception")
            ' Log any errors, an Exception cannot be thrown back to the 
            ' calling function  
        End Try

    End Sub
    Public Overridable Sub OnServerDialogAcknowledged(ByVal serverId As String, ByVal dialogId As String) _
        Implements IServerEventListener.OnServerDialogAcknowledged
        Try
            MessageBox.Show(dialogId, "Dialog Acknowledged on " & serverId)
        Catch ex As System.Exception
            MessageBox.Show("Exception")
            ' Log any errors, an Exception cannot be thrown back to the 
            ' calling function  
        End Try

    End Sub
    Public Overridable Sub OnServerPause(ByVal serverId As String) _
        Implements IServerEventListener.OnServerPause
        Try
            MessageBox.Show(serverId, "OnServerPause")
        Catch ex As System.Exception
            MessageBox.Show("Exception")
            ' Log any errors, an Exception cannot be thrown back to the 
            ' calling function  
        End Try
    End Sub
    Public Overridable Sub OnServerPausePending(ByVal serverId As String) _
        Implements IServerEventListener.OnServerPausePending
        Try
            MessageBox.Show(serverId, "OnServerPausePending")
        Catch ex As System.Exception
            MessageBox.Show("Exception")
            ' Log any errors, an Exception cannot be thrown back to the 
            ' calling function  
        End Try
    End Sub
    Public Overridable Sub OnServerResume(ByVal serverId As String) _
        Implements IServerEventListener.OnServerResume
        Try
            MessageBox.Show(serverId, "OnServerResume")
        Catch ex As System.Exception
            MessageBox.Show("Exception")
            ' Log any errors, an Exception cannot be thrown back to the 
            ' calling function  
        End Try
    End Sub
    Public Overridable Sub OnServerShutdown(ByVal serverId As String) _
        Implements IServerEventListener.OnServerShutdown
        Try
            MessageBox.Show(serverId, "OnServerShutdown")
        Catch ex As System.Exception
            MessageBox.Show("Exception")
            ' Log any errors, an Exception cannot be thrown back to the 
            ' calling function  
        End Try
    End Sub
    Public Overridable Sub OnServerShutdownPending(ByVal serverId As String) _
        Implements IServerEventListener.OnServerShutdownPending
        Try
            MessageBox.Show(serverId, "OnServerShutdownPending")
        Catch ex As System.Exception
            MessageBox.Show("Exception")
            ' Log any errors, an Exception cannot be thrown back to the 
            ' calling function  
        End Try
    End Sub
    Public Overridable Sub OnServerStartPending(ByVal serverId As String) _
        Implements IServerEventListener.OnServerStartPending
        Try
            MessageBox.Show(serverId, "OnServerStartPending")
        Catch ex As System.Exception
            MessageBox.Show("Exception")
            ' Log any errors, an Exception cannot be thrown back to the 
            ' calling function  
        End Try
    End Sub
    Public Overridable Sub OnServerStartupMessasge(ByVal serverId As String, ByVal Message As String) _
    Implements IServerEventListener.OnServerStartUpMessage
        Try
            Dim RTBResults As String
            RTBResults = Message.ToString
            MessageBox.Show(Message, serverId, MessageBoxButtons.OK, MessageBoxIcon.None)

        Catch ex As System.Exception
            MessageBox.Show("Exception")
            ' Log any errors, an Exception cannot be thrown back to the 
            ' calling function  
        End Try
    End Sub
    }
}
