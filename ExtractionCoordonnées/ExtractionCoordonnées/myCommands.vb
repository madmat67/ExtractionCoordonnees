' (C) Copyright 2013 by Matthieu Niess

#Region "Imports"
Imports System
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.DatabaseServices.Cell
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Colors
Imports Autodesk.AutoCAD.DatabaseServices.CellContent
Imports Autodesk.AutoCAD.DatabaseServices.Table
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports ExtractionCoordonnées.PaletteExtractXYZ

#End Region

<Assembly: CommandClass(GetType(ExtractionCoordonnées.MyCommands))> 

Namespace ExtractionCoordonnées

    Public Class MyCommands

        Public Structure LigneTableau
            Dim NumeroPoint As String
            Dim CoordX As String
            Dim CoordY As String
            Dim CoordZ As String
        End Structure

        <CommandMethod("ExtractXYZ")> _
        Public Sub AfficheLaPalette()

            Try
                'Commande pour afficher la palette et ses options (titre, etc...)
                Dim ps As Autodesk.AutoCAD.Windows.PaletteSet = Nothing
                ps = New Autodesk.AutoCAD.Windows.PaletteSet("Extraction de points XY")
                Dim myPalette As PaletteExtractXYZ = New PaletteExtractXYZ()
                ps.Add("Extraction de points XY", myPalette)
                ps.Visible = True
                ps.Style = Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowCloseButton

            Catch ex As System.Exception
                MsgBox(ex.ToString, MsgBoxStyle.Critical, "Erreur lors de l'execution de la commande")
                Exit Sub
            End Try

            Exit Sub

        End Sub

        Public Shared Sub ExecuteExtractXYZ(ByRef myUCS As String, ByRef myCalque As String, mySelection As SelectionSet, PTStoACAD As Boolean)

            'Décalration de variables
            Dim myDWG As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
            Dim myDB As Database = myDWG.Database
            Dim ed As Editor = myDWG.Editor
            Dim MyLigneTableau(99) As LigneTableau
            Dim my3DPointsCollection As Point3dCollection = New Point3dCollection
            Dim newMatrix As Matrix3d = New Matrix3d()
            Dim newMatrix1 As Matrix3d = New Matrix3d()
            Dim NoConversion As Integer = 0

            'On s'assure qu'aucune ancienne info ne traîne par là...
            newMatrix = Nothing
            newMatrix1 = Nothing
            my3DPointsCollection.Clear()

            If PTStoACAD = False Then GoTo PTStoXL

            Try
                'Démarrage de la transaction
                Using myTrans As Transaction = myDB.TransactionManager.StartTransaction()

                    Using myDWG.LockDocument

                        Dim myUCSTbl As UcsTable
                        Dim myUCSTblRec As UcsTableRecord

                        If myUCS = "Général" Then
                            NoConversion = 1
                        Else
                            myUCSTbl = myTrans.GetObject(myDWG.Database.UcsTableId, OpenMode.ForRead)
                            myUCSTblRec = myTrans.GetObject(myUCSTbl(myUCS), OpenMode.ForWrite)
                        End If

                        If mySelection.Count > 0 Then

                            'Dim objSelectSet As SelectionSet = mySelection.Value

                            Dim i As Integer = 2

                            For Each objAcad As Object In mySelection 'Boucle pour chaque objet selectionné

                                Dim myBloc As BlockReference = myTrans.GetObject(objAcad.ObjectId, OpenMode.ForRead)
                                Dim myAttribCollection As AttributeCollection = myBloc.AttributeCollection

                                Dim myAttribute As String = ""

                                For Each myAttribID As ObjectId In myAttribCollection 'Boucle pour chaque attribut du bloc concerné
                                    Dim myAttribRef As AttributeReference = myTrans.GetObject(myAttribID, OpenMode.ForRead)
                                    If myAttribRef.Tag.ToUpper = "POINT_TABLEAU" Then
                                        myAttribute = myAttribRef.TextString.ToString
                                    End If
                                Next myAttribID

                                Dim pt1(2) As Double
                                pt1(0) = CDbl(myBloc.Position.X.ToString) 'coord.X
                                pt1(1) = CDbl(myBloc.Position.Y.ToString) 'coord.Y
                                pt1(2) = CDbl(myBloc.Position.Z.ToString) 'coord.Z

                                Dim myTmpPoint3D As Point3d = New Point3d(pt1(0), pt1(1), pt1(2))

                                If NoConversion = 0 Then
                                    'Conversion du point depuis le repère local selectionné vers le repère général
                                    '******************************************************************************
                                    newMatrix = Matrix3d.AlignCoordinateSystem(myUCSTblRec.Origin, _
                                                                               myUCSTblRec.XAxis, _
                                                                               myUCSTblRec.YAxis, _
                                                                               New Vector3d(0, 0, 1), _
                                                                               Point3d.Origin, _
                                                                               Vector3d.XAxis, _
                                                                               Vector3d.YAxis, _
                                                                               Vector3d.ZAxis)

                                    myTmpPoint3D = myTmpPoint3D.TransformBy(newMatrix)
                                    newMatrix = Nothing
                                End If

                                my3DPointsCollection.Add(New Point3d(myTmpPoint3D.X, myTmpPoint3D.Y, myTmpPoint3D.Z))

                                MyLigneTableau(i).NumeroPoint = myAttribute.ToString
                                MyLigneTableau(i).CoordX = Math.Round(myTmpPoint3D.X).ToString
                                MyLigneTableau(i).CoordY = Math.Round(myTmpPoint3D.Y).ToString
                                MyLigneTableau(i).CoordZ = Math.Round(myTmpPoint3D.Z).ToString

                                i = i + 1
                            Next

                        Else
                            ed.WriteMessage("Aucun bloc sélectionné" + vbCrLf) 'message d'information à l'utilisateur
                            Exit Sub
                        End If

                        If myUCS <> "Général" Then
                            SCUGénéral.Switch_To_World_UCS()
                        End If

                        Dim myInsertionPointPrompt As PromptPointResult = ed.GetPoint(vbLf & "Point d'insertion: ")
                        Dim tmpX As Double = myInsertionPointPrompt.Value.X
                        Dim tmpY As Double = myInsertionPointPrompt.Value.Y
                        Dim tmpZ As Double = myInsertionPointPrompt.Value.Z
                        Dim myInsertionPoint As Point3d = New Point3d(tmpX, tmpY, tmpZ)

                        'ed.Document.SendStringToExecute("Tilemode 1 ", True, False, True)

                        If myInsertionPointPrompt.Status = PromptStatus.OK Then

                            Dim tsId As ObjectId = Autodesk.AutoCAD.DatabaseServices.ObjectId.Null
                            Dim sd As DBDictionary = DirectCast(myTrans.GetObject(myDB.TableStyleDictionaryId, OpenMode.ForRead), DBDictionary)
                            Try
                                tsId = sd.GetAt("Implantation")
                            Catch ex As System.Exception
                                MsgBox("Attention, il y a une erreur avec le style de tableau 'Implantation'", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Erreur de style de tableau")
                            End Try

                            Dim bt As BlockTable = DirectCast(myTrans.GetObject(myDWG.Database.BlockTableId, OpenMode.ForRead), BlockTable)

                            Dim tb As New Table()
                            Dim NbrePoints As Integer = my3DPointsCollection.Count

                            Dim myAddedRows As Integer = 0
                            If NbrePoints >= 3 Then myAddedRows = (NbrePoints - 3)
                            If NbrePoints < 3 Then myAddedRows = 0

                            tb.TableStyle = tsId

                            Dim myRows As Integer = 3
                            If PaletteExtractXYZ.TableauXYZ = True Then myRows = 4

                            tb.SetSize(NbrePoints + 2, myRows)
                            Dim myInsertionPoint_Z0 As Point3d = New Point3d(myInsertionPoint.X, myInsertionPoint.Y, 0)
                            tb.Position = myInsertionPoint_Z0
                            tb.SetRowHeight(7)
                            tb.SetColumnWidth(35)
                            tb.Columns(0).Width = 22
                            tb.Cells(0, 0).TextString = "Implantation"
                            tb.Cells(1, 0).TextString = "Nr. Point"
                            tb.Cells(1, 1).TextString = "Coord. X"
                            tb.Cells(1, 2).TextString = "Coord. Y"
                            If PaletteExtractXYZ.TableauXYZ = True Then tb.Cells(1, 3).TextString = "Coord. Z"

                            Dim dtp As New DataTypeParameter
                            dtp.DataType = Autodesk.AutoCAD.DatabaseServices.DataType.String
                            dtp.UnitType = Autodesk.AutoCAD.DatabaseServices.UnitType.Unitless

                            Dim strTbID As ObjectId = tb.ObjectId

                            If NbrePoints > 1 Then
                                For j As Integer = 2 To (NbrePoints + 1)
                                    If MyLigneTableau(j).NumeroPoint.ToUpper.StartsWith("JC") = True Then
                                        tb.Cells(j, 0).Alignment = CellAlignment.MiddleLeft
                                        tb.Cells(j, 0).ContentColor = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByAci, 4)

                                    ElseIf MyLigneTableau(j).NumeroPoint.ToUpper.StartsWith("E") = True Then
                                        tb.Cells(j, 0).Alignment = CellAlignment.MiddleLeft
                                        tb.Cells(j, 0).ContentColor = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByAci, 4)
                                    Else
                                        tb.Cells(j, 0).Alignment = CellAlignment.MiddleCenter
                                    End If

                                    tb.Cells(j, 0).TextString = MyLigneTableau(j).NumeroPoint

                                    tb.Cells(j, 1).TextString = ConvertirNombre.SeparateurMilliers(MyLigneTableau(j).CoordX)
                                    tb.Cells(j, 1).Contents(0).DataType = dtp
                                    'tb.Cells(j, 1).Contents(0).DataFormat = "%lu2%pr0%ds44%th32"

                                    tb.Cells(j, 2).TextString = ConvertirNombre.SeparateurMilliers(MyLigneTableau(j).CoordY)
                                    tb.Cells(j, 2).Contents(0).DataType = dtp
                                    'tb.Cells(j, 2).Contents(0).DataFormat = "%lu2%pr0%ds44%th32"

                                    If PaletteExtractXYZ.TableauXYZ = True Then
                                        tb.Cells(j, 3).TextString = ConvertirNombre.SeparateurMilliers(MyLigneTableau(j).CoordZ)
                                        tb.Cells(j, 3).Contents(0).DataType = dtp
                                    End If
                                Next

                            Else

                                tb.Cells(2, 0).Alignment = CellAlignment.MiddleCenter
                                tb.Cells(2, 0).TextString = MyLigneTableau(2).NumeroPoint

                                tb.Cells(2, 1).TextString = ConvertirNombre.SeparateurMilliers(MyLigneTableau(2).CoordX)
                                tb.Cells(2, 1).Contents(0).DataType = dtp
                                'tb.Cells(2, 1).Contents(0).DataFormat = "%lu2%pr0%ds44%th32"

                                tb.Cells(2, 2).TextString = ConvertirNombre.SeparateurMilliers(MyLigneTableau(2).CoordY)
                                tb.Cells(2, 2).Contents(0).DataType = dtp
                                'tb.Cells(2, 2).Contents(0).DataFormat = "%lu2%pr0%ds44%th32"

                                If PaletteExtractXYZ.TableauXYZ = True Then
                                    tb.Cells(2, 3).TextString = ConvertirNombre.SeparateurMilliers(MyLigneTableau(2).CoordZ)
                                    tb.Cells(2, 3).Contents(0).DataType = dtp
                                End If

                            End If

                            tb.GenerateLayout()

                            Dim btr As BlockTableRecord = myTrans.GetObject(bt(Autodesk.AutoCAD.DatabaseServices.BlockTableRecord.ModelSpace), Autodesk.AutoCAD.DatabaseServices.OpenMode.ForWrite)
                            btr.AppendEntity(tb)
                            myTrans.AddNewlyCreatedDBObject(tb, True)

                        End If
                        myTrans.Commit()
                    End Using
                End Using

            Catch ex As System.Exception
                MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
            End Try

            Exit Sub

PTStoXL:
            Try

                'On s'assure qu'aucune ancienne info ne traîne par là...
                newMatrix = Nothing
                newMatrix1 = Nothing
                my3DPointsCollection.Clear()

                'Démarrage de la transaction
                Using myTrans As Transaction = myDB.TransactionManager.StartTransaction()

                    Using myDWG.LockDocument

                        Dim myUCSTbl As UcsTable
                        Dim myUCSTblRec As UcsTableRecord

                        If myUCS = "Général" Then
                            NoConversion = 1
                        Else
                            myUCSTbl = myTrans.GetObject(myDWG.Database.UcsTableId, OpenMode.ForRead)
                            myUCSTblRec = myTrans.GetObject(myUCSTbl(myUCS), OpenMode.ForWrite)
                        End If

                        If mySelection.Count > 0 Then

                            'Dim objSelectSet As SelectionSet = mySelection.Value

                            Dim i As Integer = 1

                            For Each objAcad As Object In mySelection 'Boucle pour chaque objet selectionné

                                Dim myBloc As BlockReference = myTrans.GetObject(objAcad.ObjectId, OpenMode.ForRead)
                                Dim myAttribCollection As AttributeCollection = myBloc.AttributeCollection

                                Dim myAttribute As String = ""

                                For Each myAttribID As ObjectId In myAttribCollection 'Boucle pour chaque attribut du bloc concerné
                                    Dim myAttribRef As AttributeReference = myTrans.GetObject(myAttribID, OpenMode.ForRead)
                                    If myAttribRef.Tag.ToUpper = "POINT_TABLEAU" Then
                                        myAttribute = myAttribRef.TextString.ToString
                                    End If
                                Next myAttribID

                                Dim pt1(2) As Double
                                pt1(0) = CDbl(myBloc.Position.X.ToString) 'coord.X
                                pt1(1) = CDbl(myBloc.Position.Y.ToString) 'coord.Y
                                pt1(2) = CDbl(myBloc.Position.Z.ToString) 'coord.Z

                                Dim myTmpPoint3D As Point3d = New Point3d(pt1(0), pt1(1), pt1(2))

                                If NoConversion = 0 Then
                                    'Conversion du point depuis le repère local selectionné vers le repère général
                                    '******************************************************************************
                                    newMatrix = Matrix3d.AlignCoordinateSystem(myUCSTblRec.Origin, _
                                                                               myUCSTblRec.XAxis, _
                                                                               myUCSTblRec.YAxis, _
                                                                               New Vector3d(0, 0, 1), _
                                                                               Point3d.Origin, _
                                                                               Vector3d.XAxis, _
                                                                               Vector3d.YAxis, _
                                                                               Vector3d.ZAxis)

                                    myTmpPoint3D = myTmpPoint3D.TransformBy(newMatrix)
                                    newMatrix = Nothing
                                End If

                                my3DPointsCollection.Add(New Point3d(myTmpPoint3D.X, myTmpPoint3D.Y, myTmpPoint3D.Z))

                                MyLigneTableau(i).NumeroPoint = myAttribute.ToString
                                MyLigneTableau(i).CoordX = Math.Round(myTmpPoint3D.X).ToString
                                MyLigneTableau(i).CoordY = Math.Round(myTmpPoint3D.Y).ToString
                                MyLigneTableau(i).CoordZ = Math.Round(myTmpPoint3D.Z).ToString

                                i = i + 1
                            Next

                        Else
                            ed.WriteMessage("Aucun bloc sélectionné" + vbCrLf) 'message d'information à l'utilisateur
                            Exit Sub
                        End If

                    End Using
                End Using

                Dim appXL As Excel.Application
                Dim wbXl As Excel.Workbook
                Dim shXL As Excel.Worksheet
                'Dim raXL As Excel.Range

                ' Start Excel and get Application object.
                appXL = CreateObject("Excel.Application")
                appXL.Visible = True
                ' Add a new workbook.
                wbXl = appXL.Workbooks.Add
                shXL = wbXl.ActiveSheet

                Dim NbrePoints As Integer = my3DPointsCollection.Count

                For i = 1 To NbrePoints
                    With shXL
                        .Cells(i, 1).value = MyLigneTableau(i).NumeroPoint.ToString
                        .Cells(i, 2).value = MyLigneTableau(i).CoordX.ToString
                        .Cells(i, 3).value = MyLigneTableau(i).CoordY.ToString
                        If PaletteExtractXYZ.TableauXYZ = True Then
                            .Cells(i, 4).value = MyLigneTableau(i).CoordZ.ToString
                        End If
                    End With
                Next

            Catch ex As System.Exception
                MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
            End Try
        End Sub


        Public Shared Sub ExtractPtsToTXT(ByRef myUCS As String, ByRef myCalque As String, mySelection As SelectionSet, ConvertMeter As Boolean)

            'Décalration de variables
            Dim myDWG As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
            Dim myDB As Database = myDWG.Database
            Dim ed As Editor = myDWG.Editor
            Dim MyLigneTableau(99) As LigneTableau
            Dim my3DPointsCollection As Point3dCollection = New Point3dCollection
            Dim newMatrix As Matrix3d = New Matrix3d()
            Dim newMatrix1 As Matrix3d = New Matrix3d()
            Dim NoConversion As Integer = 0

            'On s'assure qu'aucune ancienne info ne traîne par là...
            newMatrix = Nothing
            newMatrix1 = Nothing
            my3DPointsCollection.Clear()

            Try
                'On s'assure qu'aucune ancienne info ne traîne par là...
                newMatrix = Nothing
                newMatrix1 = Nothing
                my3DPointsCollection.Clear()

                'Démarrage de la transaction
                Using myTrans As Transaction = myDB.TransactionManager.StartTransaction()

                    Using myDWG.LockDocument

                        Dim myUCSTbl As UcsTable
                        Dim myUCSTblRec As UcsTableRecord

                        If myUCS = "Général" Then
                            NoConversion = 1
                        Else
                            myUCSTbl = myTrans.GetObject(myDWG.Database.UcsTableId, OpenMode.ForRead)
                            myUCSTblRec = myTrans.GetObject(myUCSTbl(myUCS), OpenMode.ForWrite)
                        End If

                        If mySelection.Count > 0 Then

                            'Dim objSelectSet As SelectionSet = mySelection.Value

                            Dim i As Integer = 1

                            For Each objAcad As Object In mySelection 'Boucle pour chaque objet selectionné

                                Dim myBloc As BlockReference = myTrans.GetObject(objAcad.ObjectId, OpenMode.ForRead)
                                Dim myAttribCollection As AttributeCollection = myBloc.AttributeCollection

                                Dim myAttribute As String = ""

                                For Each myAttribID As ObjectId In myAttribCollection 'Boucle pour chaque attribut du bloc concerné
                                    Dim myAttribRef As AttributeReference = myTrans.GetObject(myAttribID, OpenMode.ForRead)
                                    If myAttribRef.Tag.ToUpper = "POINT_TABLEAU" Then
                                        myAttribute = myAttribRef.TextString.ToString
                                    End If
                                Next myAttribID

                                Dim pt1(2) As Double
                                pt1(0) = CDbl(myBloc.Position.X.ToString) 'coord.X
                                pt1(1) = CDbl(myBloc.Position.Y.ToString) 'coord.Y
                                pt1(2) = CDbl(myBloc.Position.Z.ToString) 'coord.Z

                                Dim myTmpPoint3D As Point3d = New Point3d(pt1(0), pt1(1), pt1(2))

                                If NoConversion = 0 Then
                                    'Conversion du point depuis le repère local selectionné vers le repère général
                                    '******************************************************************************
                                    newMatrix = Matrix3d.AlignCoordinateSystem(myUCSTblRec.Origin, _
                                                                               myUCSTblRec.XAxis, _
                                                                               myUCSTblRec.YAxis, _
                                                                               New Vector3d(0, 0, 1), _
                                                                               Point3d.Origin, _
                                                                               Vector3d.XAxis, _
                                                                               Vector3d.YAxis, _
                                                                               Vector3d.ZAxis)

                                    myTmpPoint3D = myTmpPoint3D.TransformBy(newMatrix)
                                    newMatrix = Nothing
                                End If

                                my3DPointsCollection.Add(New Point3d(myTmpPoint3D.X, myTmpPoint3D.Y, myTmpPoint3D.Z))

                                MyLigneTableau(i).NumeroPoint = myAttribute.ToString
                                MyLigneTableau(i).CoordX = Math.Round(myTmpPoint3D.X).ToString
                                MyLigneTableau(i).CoordY = Math.Round(myTmpPoint3D.Y).ToString
                                MyLigneTableau(i).CoordZ = Math.Round(myTmpPoint3D.Z).ToString

                                i = i + 1
                            Next

                        Else
                            ed.WriteMessage("Aucun bloc sélectionné" + vbCrLf) 'message d'information à l'utilisateur
                            Exit Sub
                        End If

                    End Using
                End Using

                Dim NbrePoints As Integer = my3DPointsCollection.Count
                Dim myText As String = ""
                Dim myTxtPath As String = ""

                Try
                    Using saveit = New System.Windows.Forms.SaveFileDialog
                        saveit.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                        saveit.Filter = "Fichier Texte (*.txt)|*.txt"
                        saveit.FilterIndex = 1
                        saveit.RestoreDirectory = True

                        Dim Builder As New StringBuilder

                        For i As Integer = 1 To NbrePoints
                            If ConvertMeter = True Then
                                If PaletteExtractXYZ.TableauXYZ = True Then
                                    Builder.Append(MyLigneTableau(i).NumeroPoint & "," & (MyLigneTableau(i).CoordX) / 1000 & "," & (MyLigneTableau(i).CoordY) / 1000 & "," & (MyLigneTableau(i).CoordZ) / 1000)
                                    Builder.AppendLine()
                                Else
                                    Builder.Append(MyLigneTableau(i).NumeroPoint & "," & (MyLigneTableau(i).CoordX) / 1000 & "," & (MyLigneTableau(i).CoordY) / 1000)
                                    Builder.AppendLine()
                                End If
                            Else
                                If PaletteExtractXYZ.TableauXYZ = True Then
                                    Builder.Append(MyLigneTableau(i).NumeroPoint & "," & MyLigneTableau(i).CoordX & "," & MyLigneTableau(i).CoordY & "," & MyLigneTableau(i).CoordZ)
                                    Builder.AppendLine()
                                Else
                                    Builder.Append(MyLigneTableau(i).NumeroPoint & "," & MyLigneTableau(i).CoordX & "," & MyLigneTableau(i).CoordY)
                                    Builder.AppendLine()
                                End If
                            End If
                        Next

                        myText = Builder.ToString
                        Builder = Nothing

                        If saveit.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            myTxtPath = saveit.FileName
                        Else
                            Exit Sub
                        End If
                    End Using

                    Using outfile As New StreamWriter(myTxtPath, True)
                        outfile.Write(myText)
                        outfile.Close()
                        outfile.Dispose()
                    End Using

                    If System.IO.File.Exists(myTxtPath) = True Then
                        Process.Start(myTxtPath)
                    Else
                        MsgBox("File Does Not Exist")
                    End If

                Catch ex As System.Exception
                    MsgBox(ex.ToString, vbCritical, "Erreur ouverture fichier")
                End Try

            Catch ex As System.Exception
                MsgBox(ex.ToString, MsgBoxStyle.Information, "Erreur lors de l'execution de la commande")
            End Try

        End Sub

    End Class
End Namespace




