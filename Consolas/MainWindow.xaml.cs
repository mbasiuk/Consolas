using System;
using System.Windows;
using System.Xaml;
namespace Consolas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = XamlServices.Load("CommandControl.xaml");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (DataContext is CommandCollection)
            {
                CommandCollection newCommands = DataContext as CommandCollection;
                XamlServices.Save("CommandControl.xaml", newCommands);                
            }
        }
    }
}
