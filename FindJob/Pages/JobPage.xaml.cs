using FindJob.core;
using System.Windows.Controls;

namespace FindJob
{
    public partial class JobPage : Page
    {
        public JobPage()
        {
            InitializeComponent();

            DataContext = new JobsPageViewModel();
        }
    }
}
