' (C) Copyright 2013 by Matthieu Niess
#Region "Imports"
Imports System
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.EditorInput
#End Region

<Assembly: ExtensionApplication(GetType(ExtractionCoordonnées.MyPlugin))> 

Namespace ExtractionCoordonnées

    Public Class MyPlugin
        Implements IExtensionApplication

        Public Sub Initialize() Implements IExtensionApplication.Initialize
 
        End Sub

        Public Sub Terminate() Implements IExtensionApplication.Terminate

            Exit Sub
        End Sub
    End Class

End Namespace