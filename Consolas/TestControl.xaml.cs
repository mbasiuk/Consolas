using System;
using System.Windows;
using System.Windows.Controls;

namespace Consolas
{
    /// <summary>
    /// Interaction logic for TestControl.xaml
    /// </summary>
    public partial class TestControl : UserControl
    {
        public TestControl()
        {
            InitializeComponent();
        }

        public void SetAnimationTime()
        {
            if (DataContext is Command)
            {
                TimeSpan? avarageTime = (DataContext as Command).LastDuration;
                if (avarageTime.HasValue)
                {
                    PlayAnimation.Duration = avarageTime.Value;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetAnimationTime();
        }

 
    }
}
