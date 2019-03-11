' (C) Copyright 2013 by Matthieu Niess
#Region "Diffusion"
Imports Autodesk.AutoCAD
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices.Filters
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.DatabaseServices
Imports acApp = Autodesk.AutoCAD.ApplicationServices.Application
Imports Autodesk.AutoCAD.Geometry
Imports System.Diagnostics.Process
Imports Autodesk.AutoCAD.EditorInput
Imports System.Drawing
Imports System.Windows.Forms
Imports Autodesk.AutoCAD.DatabaseServices.Font
Imports System.IO
Imports Excel = Microsoft.Office.Interop.Excel
#End Region

Public Class PaletteExtractXYZ

    Public Shared SCUActuel As String
    Public Shared CalqueActuel As String
    Public Shared SelectionActuelle As SelectionSet 
    Public Shared TableauXYZ As Boolean = False

    Public Sub Palette_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Try
            'Sub qui initialise la palette (remplissage de la liste déroulante avec la liste des calques du dessin en cours)

            SelectionActuelle = Nothing

            ' Get the current document and database, and start a transaction '
            '-------------------------------------
            Dim myDWG As Document = acApp.DocumentManager.MdiActiveDocument
            Dim myDB As Database = myDWG.Database
            '-------------------------------------

            Using myTrans As Transaction = myDB.TransactionManager.StartTransaction() 'Démarre la transaction
                Dim acLayrTbl As LayerTable = myTrans.GetObject(myDB.LayerTableId, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead)

                'Vide la ComboBox avant d'ajouter la liste des calques
                CBxCalques.Items.Clear()

                'Boucle qui ajoute tous les calques à la liste déroulante
                '-------------------------------------
                For Each acObjId As ObjectId In acLayrTbl 'Pour chaque calque dans la liste
                    Dim acLayrTblRec As LayerTableRecord = myTrans.GetObject(acObjId, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead)
                    CBxCalques.Items.Add(acLayrTblRec.Name) 'Ajoute le nom du calque à la liste déroulante
                Next
                ' Fin de la transaction
                '-------------------------------------

                CmbSCU.Items.Clear()
                CmbSCU.Items.Add("Général")

                Dim acUCSTbl As UcsTable = myTrans.GetObject(myDB.UcsTableId, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead)
                'Boucle qui ajoute tous les SCU à la liste déroulante
                '-------------------------------------
                For Each acObjId2 As ObjectId In acUCSTbl 'Pour chaque SCU dans la liste
                    Dim acUCSTblRec As UcsTableRecord = myTrans.GetObject(acObjId2, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead)
                    CmbSCU.Items.Add(acUCSTblRec.Name) 'Ajoute le nom du SCU à la liste déroulante
                Next
                SCUActuel = CmbSCU.Text.ToString
            End Using

        Catch ex As System.Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
            Exit Sub
        End Try

    End Sub


    Private Sub CBxCalques_SelectedIndexChanged_1(sender As System.Object, e As System.EventArgs) Handles CBxCalques.SelectedIndexChanged

        Try
            'Déclaration des variables
            '----------------------------
            Dim myDWG As Document = acApp.DocumentManager.MdiActiveDocument
            Dim myDB As DatabaseServices.Database
            Dim myTransMan As DatabaseServices.TransactionManager
            Dim myTrans As DatabaseServices.Transaction
            '----------------------------

            'On bloque le document pour pouvoir lancer la transaction et écrire dans celui-ci
            Using myLock As DocumentLock = myDWG.LockDocument()

                'Démarrage de la transaction
                '------------------------------------
                myDB = myDWG.Database
                myTransMan = myDWG.TransactionManager
                myTrans = myTransMan.StartTransaction
                '------------------------------------

                Dim myLT As DatabaseServices.LayerTable
                Dim myLayer As New DatabaseServices.LayerTableRecord
                Dim myLayerId As ObjectId

                myLT = CType(myTransMan.GetObject(myDB.LayerTableId, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead, True, True), LayerTable)
                myLayerId = myLT.Item(CBxCalques.Text.ToString)

                'Change le calque courant par celui selectionné dans la liste déroulante
                myDB.Clayer = myLayerId

                'Termine la transaction
                myTrans.Commit()
                myTrans.Dispose()
                myTransMan.Dispose()


            End Using 'On débloque le fichier

            CalqueActuel = CBxCalques.Text.ToString

        Catch ex As System.Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
            Exit Sub
        End Try

        Exit Sub

    End Sub

    Private Sub CmbSCU_SelectedIndexChanged_1(sender As System.Object, e As System.EventArgs) Handles CmbSCU.SelectedIndexChanged

        Try
            Dim myDWG As Document = acApp.DocumentManager.MdiActiveDocument

            Try
                If CmbSCU.Text = "Général" Then
                    myDWG.SendStringToExecute("scu" & vbCr & "ge" & vbCr, True, False, True)
                Else
                    Dim myCommandText As String = ("scu" & vbCr & "nom" & vbCr & "r" & vbCr & CmbSCU.Text & vbCr)
                    myDWG.SendStringToExecute(myCommandText, True, False, True)
                End If
            Catch
                MsgBox("Erreur de selection du SCU" & vbCrLf & "le SCU Général a été selectionné", vbCritical, "Pour éviter un plantage...")
                myDWG.SendStringToExecute("scu" & vbCr & "ge" & vbCr, True, False, True)
            End Try

            SCUActuel = CmbSCU.Text.ToString

        Catch ex As System.Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
            Exit Sub
        End Try

        Exit Sub

    End Sub

    Private Sub BtInsertBloc_Click(sender As System.Object, e As System.EventArgs) Handles BtInsertBloc.Click


        Dim myDWG As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
        Dim myDB As Database = myDWG.Database
        Dim ed As Editor = myDWG.Editor
        Try
            Dim RepertoireActuel As String = My.Application.Info.DirectoryPath 'Repertoire actuel où est sauvegardé le dll

            'pick a point to insert block
            Dim myNewPoint As Point3d = ed.GetPoint("Pick a point: ").Value

            myDWG.SendStringToExecute("_-insert" & vbCr & "Bloc_Implantation.dwg" & vbCr & myNewPoint.X & "," & myNewPoint.Y & vbCr & 1 & vbCr & 1 & vbCr & 0 & vbCr & "_explode" & vbCr & "(entlast)" & vbCr & vbCr, True, False, False) 'insert le dwg

        Catch ex As System.Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution du programme")
            Exit Sub
        End Try

        Exit Sub

    End Sub

    Private Sub BtSelectionnerBlocs_Click_1(sender As System.Object, e As System.EventArgs) Handles BtSelectionnerBlocs.Click

        Try
            If CmbSCU.Text = "" Then
                MsgBox("Veuillez d'abord sélectionner un SCU", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            Dim myDWG As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
            Dim myDB As Database = myDWG.Database
            Dim ed As Editor = myDWG.Editor

            'Creation du filtre de selection
            '------------------------------------------------------------------------
            Dim values(0) As TypedValue
            'Ne selectionne que des soildes 3D -> recherche diesel : (cdr(assoc 0 (entget (car (entsel)))))
            values(0) = New TypedValue(DxfCode.Start, "Insert") ' Ne selectionne que des blocs
            Dim selFilter As New SelectionFilter(values) 'Définition du filtre
            Dim MySelection As PromptSelectionResult = ed.GetSelection(selFilter) 'Demande l'utilisateur de sélectionner des objets
            If Not MySelection.Status = PromptStatus.OK Then
                ed.WriteMessage(" Aucun bloc sélectionné")
                Return
            End If

            SelectionActuelle = MySelection.Value

            'Mise à jour du Label
            ed.WriteMessage(MySelection.Value.Count & " blocs sélectionnés")
            LblObjSelectionnés.Visible = True
            LblObjSelectionnés.Text = SelectionActuelle.Count & " blocs sélectionnés"

            Return
        Catch ex As System.Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
            Exit Sub
        End Try

        Exit Sub

    End Sub

    Private Sub BtClear_Click_1(sender As System.Object, e As System.EventArgs) Handles BtClear.Click
        Try
            SelectionActuelle = Nothing
            LblObjSelectionnés.Text = "Aucun bloc sélectionné"
            LblObjSelectionnés.Visible = False

        Catch ex As System.Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
            Exit Sub
        End Try

        Exit Sub

    End Sub

    Private Sub BtTracerTableau_Click_1(sender As System.Object, e As System.EventArgs) Handles BtTracerTableau.Click

        Try
            If CBxCalques.Text = "" Then
                MsgBox("Veuillez d'abord sélectionner un Calque", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            If CmbSCU.Text = "" Then
                MsgBox("Veuillez d'abord sélectionner un SCU", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            If LblObjSelectionnés.Visible = False Then
                MsgBox("Veuillez d'abord sélectionner des blocs", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            If SelectionActuelle Is Nothing Then
                MsgBox("Veuillez d'abord sélectionner des blocs", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            If ChkXY.Checked = False And ChkXYZ.Checked = False Then
                MsgBox("Veuillez sélectionner un type de tableau", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            ExtractionCoordonnées.MyCommands.ExecuteExtractXYZ(SCUActuel, CalqueActuel, SelectionActuelle, True)

            'CmbSCU.Text = ""

            acApp.DocumentManager.MdiActiveDocument.Editor.WriteMessage("Extraction effectuée avec succès." & vbCr)

        Catch ex As System.Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
            Exit Sub
        End Try

        Exit Sub

    End Sub

  
    Public Sub BtMajCalques_Click(sender As System.Object, e As System.EventArgs) Handles BtMajCalques.Click

        Try
            'Sub qui initialise la palette (remplissage de la liste déroulante avec la liste des calques du dessin en cours)
            SelectionActuelle = Nothing

            ' Get the current document and database, and start a transaction '
            '-------------------------------------
            Dim myDWG As Document = acApp.DocumentManager.MdiActiveDocument
            Dim myDB As Database = myDWG.Database
            Dim ed As Editor = myDWG.Editor
            '-------------------------------------

            Using myTrans As Transaction = myDB.TransactionManager.StartTransaction() 'Démarre la transaction
                Dim acLayrTbl As LayerTable = myTrans.GetObject(myDB.LayerTableId, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead)

                'Vide la ComboBox avant d'ajouter la liste des calques
                CBxCalques.Items.Clear()

                'Boucle qui ajoute tous les calques à la liste déroulante
                '-------------------------------------
                For Each acObjId As ObjectId In acLayrTbl 'Pour chaque calque dans la liste
                    Dim acLayrTblRec As LayerTableRecord = myTrans.GetObject(acObjId, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead)
                    CBxCalques.Items.Add(acLayrTblRec.Name) 'Ajoute le nom du calque à la liste déroulante
                Next
                ed.WriteMessage("Liste des calques mise à jour." & vbCr)
                ' Fin de la transaction
                '-------------------------------------
            End Using

        Catch ex As System.Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
            Exit Sub
        End Try

        Exit Sub

    End Sub

    Private Sub BtMajSCU_Click(sender As System.Object, e As System.EventArgs) Handles BtMajSCU.Click

        Try
            'Sub qui initialise la palette (remplissage de la liste déroulante avec la liste des calques du dessin en cours)
            SelectionActuelle = Nothing

            ' Get the current document and database, and start a transaction '
            '-------------------------------------
            Dim myDWG As Document = acApp.DocumentManager.MdiActiveDocument
            Dim myDB As Database = myDWG.Database
            Dim ed As Editor = myDWG.Editor
            '-------------------------------------

            Using myTrans As Transaction = myDB.TransactionManager.StartTransaction() 'Démarre la transaction
                CmbSCU.Items.Clear()
                CmbSCU.Items.Add("Général")

                Dim acUCSTbl As UcsTable = myTrans.GetObject(myDB.UcsTableId, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead)
                'Boucle qui ajoute tous les SCU à la liste déroulante
                '-------------------------------------
                For Each acObjId2 As ObjectId In acUCSTbl 'Pour chaque SCU dans la liste
                    Dim acUCSTblRec As UcsTableRecord = myTrans.GetObject(acObjId2, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead)
                    CmbSCU.Items.Add(acUCSTblRec.Name) 'Ajoute le nom du SCU à la liste déroulante
                Next
                SCUActuel = CmbSCU.Text.ToString
                ed.WriteMessage("Liste des SCU mise à jour." & vbCr)
            End Using

        Catch ex As System.Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
            Exit Sub
        End Try

        Exit Sub

    End Sub

    Private Sub ChkXY_CheckedChanged(sender As Object, e As EventArgs) Handles ChkXY.CheckedChanged
        If ChkXY.Checked = True Then
            ChkXYZ.Checked = False
            TableauXYZ = False
        Else
            ChkXYZ.Checked = True
            TableauXYZ = True
        End If
    End Sub

    Private Sub ChkXYZ_CheckedChanged(sender As Object, e As EventArgs) Handles ChkXYZ.CheckedChanged
        If ChkXYZ.Checked = True Then
            ChkXY.Checked = False
            TableauXYZ = True
        Else
            ChkXY.Checked = True
            TableauXYZ = False
        End If
    End Sub

    Private Sub BtPtsToXL_Click(sender As Object, e As EventArgs) Handles BtPtsToXL.Click

        Try
            If CBxCalques.Text = "" Then
                MsgBox("Veuillez d'abord sélectionner un Calque", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            If CmbSCU.Text = "" Then
                MsgBox("Veuillez d'abord sélectionner un SCU", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            If LblObjSelectionnés.Visible = False Then
                MsgBox("Veuillez d'abord sélectionner des blocs", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            If SelectionActuelle is nothing Then
                MsgBox("Veuillez d'abord sélectionner des blocs", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            If ChkXY.Checked = False And ChkXYZ.Checked = False Then
                MsgBox("Veuillez sélectionner un type de tableau", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            Dim xlApp As Excel.Application = New Microsoft.Office.Interop.Excel.Application()

            If xlApp Is Nothing Then
                MsgBox("Excel n'est probablement pas installé." & vbCrLf & "A vérifier...", vbCritical + vbOKOnly, "Erreur du programme")
                Return
            End If

            ExtractionCoordonnées.MyCommands.ExecuteExtractXYZ(SCUActuel, CalqueActuel, SelectionActuelle, False)

        Catch ex As System.Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
        End Try

    End Sub

    Private Sub BtPtsToTXT_Click(sender As Object, e As EventArgs) Handles BtPtsToTXT.Click

        Try
            If CBxCalques.Text = "" Then
                MsgBox("Veuillez d'abord sélectionner un Calque", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            If CmbSCU.Text = "" Then
                MsgBox("Veuillez d'abord sélectionner un SCU", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            If LblObjSelectionnés.Visible = False Then
                MsgBox("Veuillez d'abord sélectionner des blocs", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            If SelectionActuelle Is Nothing Then
                MsgBox("Veuillez d'abord sélectionner des blocs", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            If ChkXY.Checked = False And ChkXYZ.Checked = False Then
                MsgBox("Veuillez sélectionner un type de tableau", MsgBoxStyle.Information & vbOKOnly, "Attention")
                Exit Sub
            End If

            Dim xlApp As Excel.Application = New Microsoft.Office.Interop.Excel.Application()

            If xlApp Is Nothing Then
                MsgBox("Excel n'est probablement pas installé." & vbCrLf & "A vérifier...", vbCritical + vbOKOnly, "Erreur du programme")
                Return
            End If

            If ChkConvertMeter.Checked = True Then _
            ExtractionCoordonnées.MyCommands.ExtractPtsToTXT(SCUActuel, CalqueActuel, SelectionActuelle, True)

            If ChkConvertMeter.Checked = False Then _
            ExtractionCoordonnées.MyCommands.ExtractPtsToTXT(SCUActuel, CalqueActuel, SelectionActuelle, False)

        Catch ex As System.Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
        End Try

    End Sub

    Private Sub ChkConvertMeter_CheckedChanged(sender As Object, e As EventArgs) Handles ChkConvertMeter.CheckedChanged

    End Sub
End Class
