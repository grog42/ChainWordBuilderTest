<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LetterTest
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.MainDisplay = New System.Windows.Forms.PictureBox()
        Me.TextInput = New System.Windows.Forms.TextBox()
        Me.MainBox = New System.Windows.Forms.TableLayoutPanel()
        Me.TopBarLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.FontSelector = New System.Windows.Forms.ComboBox()
        Me.SpacingSelector = New System.Windows.Forms.NumericUpDown()
        CType(Me.MainDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainBox.SuspendLayout()
        Me.TopBarLayout.SuspendLayout()
        CType(Me.SpacingSelector, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainDisplay
        '
        Me.MainDisplay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainDisplay.Location = New System.Drawing.Point(3, 56)
        Me.MainDisplay.Name = "MainDisplay"
        Me.MainDisplay.Size = New System.Drawing.Size(794, 391)
        Me.MainDisplay.TabIndex = 0
        Me.MainDisplay.TabStop = False
        '
        'TextInput
        '
        Me.TextInput.Location = New System.Drawing.Point(3, 3)
        Me.TextInput.Name = "TextInput"
        Me.TextInput.Size = New System.Drawing.Size(99, 20)
        Me.TextInput.TabIndex = 2
        '
        'MainBox
        '
        Me.MainBox.ColumnCount = 1
        Me.MainBox.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.MainBox.Controls.Add(Me.TopBarLayout, 0, 0)
        Me.MainBox.Controls.Add(Me.MainDisplay, 0, 1)
        Me.MainBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainBox.Location = New System.Drawing.Point(0, 0)
        Me.MainBox.Name = "MainBox"
        Me.MainBox.RowCount = 2
        Me.MainBox.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.MainBox.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 397.0!))
        Me.MainBox.Size = New System.Drawing.Size(800, 450)
        Me.MainBox.TabIndex = 3
        '
        'TopBarLayout
        '
        Me.TopBarLayout.ColumnCount = 3
        Me.TopBarLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TopBarLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 203.0!))
        Me.TopBarLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 486.0!))
        Me.TopBarLayout.Controls.Add(Me.TextInput, 0, 0)
        Me.TopBarLayout.Controls.Add(Me.FontSelector, 1, 0)
        Me.TopBarLayout.Controls.Add(Me.SpacingSelector, 2, 0)
        Me.TopBarLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TopBarLayout.Location = New System.Drawing.Point(3, 3)
        Me.TopBarLayout.Name = "TopBarLayout"
        Me.TopBarLayout.RowCount = 1
        Me.TopBarLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TopBarLayout.Size = New System.Drawing.Size(794, 47)
        Me.TopBarLayout.TabIndex = 1
        '
        'FontSelector
        '
        Me.FontSelector.FormattingEnabled = True
        Me.FontSelector.Location = New System.Drawing.Point(108, 3)
        Me.FontSelector.Name = "FontSelector"
        Me.FontSelector.Size = New System.Drawing.Size(121, 21)
        Me.FontSelector.TabIndex = 3
        '
        'SpacingSelector
        '
        Me.SpacingSelector.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.SpacingSelector.Location = New System.Drawing.Point(311, 3)
        Me.SpacingSelector.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.SpacingSelector.Name = "SpacingSelector"
        Me.SpacingSelector.Size = New System.Drawing.Size(120, 20)
        Me.SpacingSelector.TabIndex = 4
        '
        'LetterTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.MainBox)
        Me.Name = "LetterTest"
        Me.Text = "LetterTest"
        CType(Me.MainDisplay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainBox.ResumeLayout(False)
        Me.TopBarLayout.ResumeLayout(False)
        Me.TopBarLayout.PerformLayout()
        CType(Me.SpacingSelector, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MainDisplay As PictureBox
    Friend WithEvents TextInput As TextBox
    Friend WithEvents MainBox As TableLayoutPanel
    Friend WithEvents TopBarLayout As TableLayoutPanel
    Friend WithEvents FontSelector As ComboBox
    Friend WithEvents SpacingSelector As NumericUpDown
End Class
