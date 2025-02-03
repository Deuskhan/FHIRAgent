using System.Windows;

namespace Agent.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PatientOverlayWebBrowser.Navigate("https://galaxyhealth.web.app/patients");
        }
    }
} 