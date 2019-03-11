<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PaletteExtractXYZ
    Inherits System.Windows.Forms.UserControl

    'UserControl remplace la méthode Dispose pour nettoyer la liste des composants.
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
        Me.BtInsertBloc = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.CBxCalques = New System.Windows.Forms.ComboBox()
        Me.BtClear = New System.Windows.Forms.Button()
        Me.BtSelectionnerBlocs = New System.Windows.Forms.Button()
        Me.CmbSCU = New System.Windows.Forms.ComboBox()
        Me.LblObjSelectionnés = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtMajCalques = New System.Windows.Forms.Button()
        Me.BtMajSCU = New System.Windows.Forms.Button()
        Me.ChkXYZ = New System.Windows.Forms.CheckBox()
        Me.ChkXY = New System.Windows.Forms.CheckBox()
        Me.myGroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.BtPtsToXL = New System.Windows.Forms.Button()
        Me.BtTracerTableau = New System.Windows.Forms.Button()
        Me.BtPtsToTXT = New System.Windows.Forms.Button()
        Me.SaveIt = New System.Windows.Forms.SaveFileDialog()
        Me.ChkConvertMeter = New System.Windows.Forms.CheckBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.myGroupBox1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtInsertBloc
        '
        Me.BtInsertBloc.Location = New System.Drawing.Point(9, 24)
        Me.BtInsertBloc.Name = "BtInsertBloc"
        Me.BtInsertBloc.Size = New System.Drawing.Size(170, 22)
        Me.BtInsertBloc.TabIndex = 1
        Me.BtInsertBloc.Text = "Insertion du bloc ""implantation"""
        Me.BtInsertBloc.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.SystemColors.Control
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PictureBox1.Image = Global.ExtractionCoordonnées.My.Resources.Resources.compas2
        Me.PictureBox1.Location = New System.Drawing.Point(0, 514)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(264, 185)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 32
        Me.PictureBox1.TabStop = False
        '
        'CBxCalques
        '
        Me.CBxCalques.FormattingEnabled = True
        Me.CBxCalques.Location = New System.Drawing.Point(6, 43)
        Me.CBxCalques.Name = "CBxCalques"
        Me.CBxCalques.Size = New System.Drawing.Size(210, 21)
        Me.CBxCalques.TabIndex = 1
        '
        'BtClear
        '
        Me.BtClear.Location = New System.Drawing.Point(137, 56)
        Me.BtClear.Name = "BtClear"
        Me.BtClear.Size = New System.Drawing.Size(42, 22)
        Me.BtClear.TabIndex = 3
        Me.BtClear.Text = "Clear"
        Me.BtClear.UseVisualStyleBackColor = True
        '
        'BtSelectionnerBlocs
        '
        Me.BtSelectionnerBlocs.Location = New System.Drawing.Point(9, 56)
        Me.BtSelectionnerBlocs.Name = "BtSelectionnerBlocs"
        Me.BtSelectionnerBlocs.Size = New System.Drawing.Size(122, 23)
        Me.BtSelectionnerBlocs.TabIndex = 2
        Me.BtSelectionnerBlocs.Text = "Selectionner blocs"
        Me.BtSelectionnerBlocs.UseVisualStyleBackColor = True
        '
        'CmbSCU
        '
        Me.CmbSCU.FormattingEnabled = True
        Me.CmbSCU.Location = New System.Drawing.Point(6, 90)
        Me.CmbSCU.Name = "CmbSCU"
        Me.CmbSCU.Size = New System.Drawing.Size(210, 21)
        Me.CmbSCU.TabIndex = 3
        '
        'LblObjSelectionnés
        '
        Me.LblObjSelectionnés.AutoSize = True
        Me.LblObjSelectionnés.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblObjSelectionnés.Location = New System.Drawing.Point(12, 89)
        Me.LblObjSelectionnés.Name = "LblObjSelectionnés"
        Me.LblObjSelectionnés.Size = New System.Drawing.Size(120, 15)
        Me.LblObjSelectionnés.TabIndex = 29
        Me.LblObjSelectionnés.Text = "Aucun bloc sélectionné"
        Me.LblObjSelectionnés.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(190, 13)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "Calque à utiliser pour tracer le tableau :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(238, 13)
        Me.Label2.TabIndex = 35
        Me.Label2.Text = "SCU à utiliser pour l'extraction des coordonnées :"
        '
        'BtMajCalques
        '
        Me.BtMajCalques.Location = New System.Drawing.Point(221, 41)
        Me.BtMajCalques.Name = "BtMajCalques"
        Me.BtMajCalques.Size = New System.Drawing.Size(33, 23)
        Me.BtMajCalques.TabIndex = 2
        Me.BtMajCalques.Text = "màj"
        Me.BtMajCalques.UseVisualStyleBackColor = True
        '
        'BtMajSCU
        '
        Me.BtMajSCU.Location = New System.Drawing.Point(221, 88)
        Me.BtMajSCU.Name = "BtMajSCU"
        Me.BtMajSCU.Size = New System.Drawing.Size(33, 23)
        Me.BtMajSCU.TabIndex = 4
        Me.BtMajSCU.Text = "màj"
        Me.BtMajSCU.UseVisualStyleBackColor = True
        '
        'ChkXYZ
        '
        Me.ChkXYZ.AutoSize = True
        Me.ChkXYZ.Location = New System.Drawing.Point(89, 24)
        Me.ChkXYZ.Name = "ChkXYZ"
        Me.ChkXYZ.Size = New System.Drawing.Size(81, 17)
        Me.ChkXYZ.TabIndex = 2
        Me.ChkXYZ.Text = "Coord. XYZ"
        Me.ChkXYZ.UseVisualStyleBackColor = True
        '
        'ChkXY
        '
        Me.ChkXY.AutoSize = True
        Me.ChkXY.Location = New System.Drawing.Point(9, 24)
        Me.ChkXY.Name = "ChkXY"
        Me.ChkXY.Size = New System.Drawing.Size(74, 17)
        Me.ChkXY.TabIndex = 1
        Me.ChkXY.Text = "Coord. XY"
        Me.ChkXY.UseVisualStyleBackColor = True
        '
        'myGroupBox1
        '
        Me.myGroupBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.myGroupBox1.Controls.Add(Me.Label1)
        Me.myGroupBox1.Controls.Add(Me.CmbSCU)
        Me.myGroupBox1.Controls.Add(Me.CBxCalques)
        Me.myGroupBox1.Controls.Add(Me.Label2)
        Me.myGroupBox1.Controls.Add(Me.BtMajSCU)
        Me.myGroupBox1.Controls.Add(Me.BtMajCalques)
        Me.myGroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.myGroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.myGroupBox1.Name = "myGroupBox1"
        Me.myGroupBox1.Size = New System.Drawing.Size(264, 126)
        Me.myGroupBox1.TabIndex = 1
        Me.myGroupBox1.TabStop = False
        Me.myGroupBox1.Text = "Réglages"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(255, 39)
        Me.Label3.TabIndex = 44
        Me.Label3.Text = "Cet utilitaire a été conçu pour extraire les coord. XYZ" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "des blocs dont le nom es" & _
    "t ""Bloc_Implantation.dwg""." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Attribut de bloc filtré = ""Point_Tableau"")"
        '
        'GroupBox4
        '
        Me.GroupBox4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.GroupBox4.Controls.Add(Me.Label3)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox4.Location = New System.Drawing.Point(0, 438)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(264, 76)
        Me.GroupBox4.TabIndex = 45
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Information utilisateur"
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.GroupBox2.Controls.Add(Me.BtInsertBloc)
        Me.GroupBox2.Controls.Add(Me.LblObjSelectionnés)
        Me.GroupBox2.Controls.Add(Me.BtSelectionnerBlocs)
        Me.GroupBox2.Controls.Add(Me.BtClear)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox2.Location = New System.Drawing.Point(0, 126)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(264, 119)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Blocs"
        '
        'GroupBox3
        '
        Me.GroupBox3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.GroupBox3.Controls.Add(Me.ChkConvertMeter)
        Me.GroupBox3.Controls.Add(Me.ChkXYZ)
        Me.GroupBox3.Controls.Add(Me.ChkXY)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox3.Location = New System.Drawing.Point(0, 245)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(264, 89)
        Me.GroupBox3.TabIndex = 3
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Tableau à tracer"
        '
        'BtPtsToXL
        '
        Me.BtPtsToXL.Location = New System.Drawing.Point(9, 322)
        Me.BtPtsToXL.Name = "BtPtsToXL"
        Me.BtPtsToXL.Size = New System.Drawing.Size(119, 38)
        Me.BtPtsToXL.TabIndex = 4
        Me.BtPtsToXL.Text = "Exporter les coord." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "des pts vers Excel"
        Me.BtPtsToXL.UseVisualStyleBackColor = True
        '
        'BtTracerTableau
        '
        Me.BtTracerTableau.Location = New System.Drawing.Point(135, 322)
        Me.BtTracerTableau.Name = "BtTracerTableau"
        Me.BtTracerTableau.Size = New System.Drawing.Size(119, 38)
        Me.BtTracerTableau.TabIndex = 3
        Me.BtTracerTableau.Text = "Tracer le tableau" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "dans Autocad"
        Me.BtTracerTableau.UseVisualStyleBackColor = True
        '
        'BtPtsToTXT
        '
        Me.BtPtsToTXT.Location = New System.Drawing.Point(9, 366)
        Me.BtPtsToTXT.Name = "BtPtsToTXT"
        Me.BtPtsToTXT.Size = New System.Drawing.Size(119, 38)
        Me.BtPtsToTXT.TabIndex = 46
        Me.BtPtsToTXT.Text = "Exporter les coord. des pts vers TXT"
        Me.BtPtsToTXT.UseVisualStyleBackColor = True
        '
        'ChkConvertMeter
        '
        Me.ChkConvertMeter.AutoSize = True
        Me.ChkConvertMeter.Checked = True
        Me.ChkConvertMeter.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkConvertMeter.Location = New System.Drawing.Point(9, 47)
        Me.ChkConvertMeter.Name = "ChkConvertMeter"
        Me.ChkConvertMeter.Size = New System.Drawing.Size(141, 17)
        Me.ChkConvertMeter.TabIndex = 3
        Me.ChkConvertMeter.Text = "Convertir TXT en mètres"
        Me.ChkConvertMeter.UseVisualStyleBackColor = True
        '
        'PaletteExtractXYZ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Controls.Add(Me.BtPtsToXL)
        Me.Controls.Add(Me.BtTracerTableau)
        Me.Controls.Add(Me.BtPtsToTXT)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.myGroupBox1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Name = "PaletteExtractXYZ"
        Me.Size = New System.Drawing.Size(264, 699)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.myGroupBox1.ResumeLayout(False)
        Me.myGroupBox1.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BtInsertBloc As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents CBxCalques As System.Windows.Forms.ComboBox
    Friend WithEvents BtClear As System.Windows.Forms.Button
    Friend WithEvents BtSelectionnerBlocs As System.Windows.Forms.Button
    Friend WithEvents CmbSCU As System.Windows.Forms.ComboBox
    Friend WithEvents LblObjSelectionnés As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents BtMajCalques As System.Windows.Forms.Button
    Friend WithEvents BtMajSCU As System.Windows.Forms.Button
    Friend WithEvents ChkXYZ As System.Windows.Forms.CheckBox
    Friend WithEvents ChkXY As System.Windows.Forms.CheckBox
    Friend WithEvents myGroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents BtPtsToXL As System.Windows.Forms.Button
    Friend WithEvents BtTracerTableau As System.Windows.Forms.Button
    Friend WithEvents BtPtsToTXT As System.Windows.Forms.Button
    Friend WithEvents SaveIt As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ChkConvertMeter As System.Windows.Forms.CheckBox

End Class
