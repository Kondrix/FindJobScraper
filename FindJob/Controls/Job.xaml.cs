using System.Windows.Controls;
using System.Windows.Navigation;

namespace FindJob
{
    /// <summary>
    /// Logika interakcji dla klasy Job.xaml
    /// </summary>
    public partial class Job : UserControl
    {
        public Job()
        {
            InitializeComponent();

            
           
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
