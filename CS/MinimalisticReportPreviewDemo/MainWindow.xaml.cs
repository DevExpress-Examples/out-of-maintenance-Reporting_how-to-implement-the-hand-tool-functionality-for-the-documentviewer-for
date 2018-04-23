using System;
using System.Windows;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Core.Native;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using System.ComponentModel;

namespace MinimalisticReportPreviewDemo {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        ScrollViewer scrollViewer;

        public MainWindow() {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            InitDataContext();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            scrollViewer = LayoutHelper.FindElementByName(sender as MainWindow, "documentPreviewScrollViewer") as ScrollViewer;
            EventRegistration.RootElement = scrollViewer;
        }

        private void InitDataContext()
        {
            Report report = new Report();
            CustomXtraReportPreviewModel model = new CustomXtraReportPreviewModel(report, true);
            report.CreateDocument(true);
            DataContext = model;
        }

        public class CustomXtraReportPreviewModel : XtraReportPreviewModel, INotifyPropertyChanged {
            bool isHandToolEnabled;

            public bool IsHandToolEnabled
            {
                get { return isHandToolEnabled; }
                set { isHandToolEnabled = value; RaisePropertyChanged("IsHandToolEnabled"); }
            }
            public ICommand HandToolCommand { get; set; }
            #region PropertyChanged
            public event PropertyChangedEventHandler PropertyChangedEvent;
            public void RaisePropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChangedEvent;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
            
            public CustomXtraReportPreviewModel(DevExpress.XtraReports.IReport report, bool enableSimpleScrolling) : base(report)
            {
                IsHandToolEnabled = false;
                HandToolCommand = new DelegateCommand(OnEnableHandToolExecuted);
                UseSimpleScrolling = enableSimpleScrolling;
            }

            private void OnEnableHandToolExecuted()
            {
                IsHandToolEnabled = !IsHandToolEnabled;
                if (IsHandToolEnabled)
                    EventRegistration.Register();
                else
                    EventRegistration.Unregister();
            }
        
        }


        public static class EventRegistration {

            public static FrameworkElement RootElement { get; set; }

            public static void Register()
            {
                MouseTouchDevice.RegisterEvents(RootElement);
            }

            public static void Unregister()
            {
                MouseTouchDevice.UnregisterEvents(RootElement);
            }
        }
    }
}
