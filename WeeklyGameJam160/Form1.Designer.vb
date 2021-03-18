<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.gamePanel = New System.Windows.Forms.Panel()
        Me.musicTimer = New System.Windows.Forms.Timer(Me.components)
        Me.particleTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'gamePanel
        '
        Me.gamePanel.BackColor = System.Drawing.Color.White
        Me.gamePanel.Enabled = False
        Me.gamePanel.Location = New System.Drawing.Point(0, 0)
        Me.gamePanel.Name = "gamePanel"
        Me.gamePanel.Size = New System.Drawing.Size(500, 625)
        Me.gamePanel.TabIndex = 0
        '
        'musicTimer
        '
        Me.musicTimer.Interval = 500
        '
        'particleTimer
        '
        Me.particleTimer.Interval = 1
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(500, 625)
        Me.Controls.Add(Me.gamePanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Tetro"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gamePanel As System.Windows.Forms.Panel
    Friend WithEvents musicTimer As System.Windows.Forms.Timer
    Friend WithEvents particleTimer As System.Windows.Forms.Timer

End Class
