' (C) Copyright 2013 by Matthieu Niess
#Region "Imports"
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
#End Region

Public Class SCUGénéral

    Public Shared vect1 As Vector3d
    Public Shared vect2 As Vector3d
    Public Shared org As Point3d

    Public Shared Sub Switch_To_World_UCS() 'Rappelle le SCU Général

        Dim myDWG As Document = Application.DocumentManager.MdiActiveDocument
        Dim myDB As Database = myDWG.Database
        vect1 = myDB.Ucsxdir
        vect2 = myDB.Ucsydir
        org = myDB.Ucsorg
        myDWG.Editor.CurrentUserCoordinateSystem = Matrix3d.Identity

    End Sub

End Class
