Imports System
Imports System.Windows
Imports DevExpress.Xpf.Core
Imports DevExpress.Xpf.Printing
Imports DevExpress.Xpf.Core.Native
Imports System.Windows.Controls
Imports System.Windows.Input
Imports DevExpress.Mvvm
Imports DevExpress.Mvvm.UI
Imports System.ComponentModel

Namespace MinimalisticReportPreviewDemo
    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow
        Inherits Window

        Private scrollViewer As ScrollViewer

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, AddressOf MainWindow_Loaded
            InitDataContext()
        End Sub

        Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            scrollViewer = TryCast(LayoutHelper.FindElementByName(TryCast(sender, MainWindow), "documentPreviewScrollViewer"), ScrollViewer)
            EventRegistration.RootElement = scrollViewer
        End Sub

        Private Sub InitDataContext()
            Dim report As New Report()
            Dim model As New CustomXtraReportPreviewModel(report, True)
            report.CreateDocument(True)
            DataContext = model
        End Sub

        Public Class CustomXtraReportPreviewModel
            Inherits XtraReportPreviewModel
            Implements INotifyPropertyChanged


            Private isHandToolEnabled_Renamed As Boolean

            Public Property IsHandToolEnabled() As Boolean
                Get
                    Return isHandToolEnabled_Renamed
                End Get
                Set(ByVal value As Boolean)
                    isHandToolEnabled_Renamed = value
                    RaisePropertyChangedEvent("IsHandToolEnabled")
                End Set
            End Property
            Public Property HandToolCommand() As ICommand
            #Region "PropertyChanged"
            Public Event PropertyChangedEvent As PropertyChangedEventHandler
            Public Sub RaisePropertyChangedEvent(ByVal propertyName As String)
                Dim handler As PropertyChangedEventHandler = PropertyChangedEventEvent
                If handler IsNot Nothing Then
                    handler(Me, New PropertyChangedEventArgs(propertyName))
                End If
            End Sub
            #End Region

            Public Sub New(ByVal report As DevExpress.XtraReports.IReport, ByVal enableSimpleScrolling As Boolean)
                MyBase.New(report)
                IsHandToolEnabled = False
                HandToolCommand = New DelegateCommand(AddressOf OnEnableHandToolExecuted)
                UseSimpleScrolling = enableSimpleScrolling
            End Sub

            Private Sub OnEnableHandToolExecuted()
                IsHandToolEnabled = Not IsHandToolEnabled
                If IsHandToolEnabled Then
                    EventRegistration.Register()
                Else
                    EventRegistration.Unregister()
                End If
            End Sub

        End Class


        Public NotInheritable Class EventRegistration

            Private Sub New()
            End Sub


            Public Shared Property RootElement() As FrameworkElement

            Public Shared Sub Register()
                MouseTouchDevice.RegisterEvents(RootElement)
            End Sub

            Public Shared Sub Unregister()
                MouseTouchDevice.UnregisterEvents(RootElement)
            End Sub
        End Class
    End Class
End Namespace
