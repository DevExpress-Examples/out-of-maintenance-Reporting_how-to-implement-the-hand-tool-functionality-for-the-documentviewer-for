Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Input
Imports System.Windows.Media

Namespace MinimalisticReportPreviewDemo
    Public Class MouseTouchDevice
        Inherits TouchDevice

        #Region "Class Members"

        Private Shared device As MouseTouchDevice

        Public Property Position() As Point

        #End Region

        #Region "Public Static Methods"

        Public Shared Sub RegisterEvents(ByVal root As FrameworkElement)
            AddHandler root.PreviewMouseDown, AddressOf MouseDown
            AddHandler root.PreviewMouseMove, AddressOf MouseMove
            AddHandler root.PreviewMouseUp, AddressOf MouseUp
        End Sub

        Public Shared Sub UnregisterEvents(ByVal root As FrameworkElement)
            RemoveHandler root.PreviewMouseDown, AddressOf MouseDown
            RemoveHandler root.PreviewMouseMove, AddressOf MouseMove
            RemoveHandler root.PreviewMouseUp, AddressOf MouseUp
        End Sub

        #End Region

        #Region "Private Static Methods"

        Private Shared Sub MouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If device IsNot Nothing AndAlso device.IsActive Then
                device.ReportUp()
                device.Deactivate()
                device = Nothing
            End If
            device = New MouseTouchDevice(e.MouseDevice.GetHashCode())
            device.SetActiveSource(e.MouseDevice.ActiveSource)
            device.Position = e.GetPosition(Nothing)
            device.Activate()
            device.ReportDown()
            e.Handled = True
        End Sub

        Private Shared Sub MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
            If device IsNot Nothing AndAlso device.IsActive Then
                device.Position = e.GetPosition(Nothing)
                device.ReportMove()
                e.Handled = True
            End If
        End Sub

        Private Shared Sub MouseUp(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If device IsNot Nothing AndAlso device.IsActive Then
                device.Position = e.GetPosition(Nothing)
                device.ReportUp()
                device.Deactivate()
                device = Nothing
                e.Handled = True
            End If
        End Sub

        #End Region

        #Region "Constructors"

        Public Sub New(ByVal deviceId As Integer)
            MyBase.New(deviceId)
            Position = New Point()
        End Sub

        #End Region

        #Region "Overridden methods"

        Public Overrides Function GetIntermediateTouchPoints(ByVal relativeTo As IInputElement) As TouchPointCollection
            Return New TouchPointCollection()
        End Function

        Public Overrides Function GetTouchPoint(ByVal relativeTo As IInputElement) As TouchPoint
            Dim point As Point = Position
            If relativeTo IsNot Nothing Then
                point = Me.ActiveSource.RootVisual.TransformToDescendant(DirectCast(relativeTo, Visual)).Transform(Position)
            End If

            Dim rect As New Rect(point, New Size(1, 1))

            Return New TouchPoint(Me, point, rect, TouchAction.Move)
        End Function

        #End Region

    End Class
End Namespace
