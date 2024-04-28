using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FindJob.core
{
    public class JobsPageViewModel
    {
        public ObservableCollection<JobViewModel> JobsList { get; set; } = new ObservableCollection<JobViewModel>();

        public Dictionary<int, string> TechDictionary { get; set; }
        public KeyValuePair<int, string> SelectedTech { get; set; }
        public Dictionary<int, string> LevelDictionary { get; set; }
        public KeyValuePair<int, string> SelectedLevel { get; set; }
        public string Location { get; set; }
        public ICommand SearchForJobCommand { get; set; }
        public JobsPageViewModel()
        {
            PracujClass pracujClass = new PracujClass();
            SearchForJobCommand = new RelayCommand(async () => await ScrapAllJobs(SelectedTech.Key,SelectedTech.Value,SelectedLevel.Key,SelectedLevel.Value,Location));
            JobTech jobTech = new JobTech();
            TechDictionary = jobTech.TechL;
            JobLevel jobLevel = new JobLevel();
            LevelDictionary = jobLevel.Level;

        }
        public async Task ScrapAllJobs(int key, string value,int lvl,string jlvl,string location)
        {
            JobsList.Clear();
            PracujClass pracujClass = new PracujClass();
            GetFromJJ getFromJJ = new GetFromJJ();

            var taskPracuj = Task.Run(() => pracujClass.GetHtmlFromPracujAsync(key, lvl, location));
            var taskJJIT = Task.Run(() => getFromJJ.GetHtmlFromJJIT(value, location, jlvl));

            var jobsFromPracuj = await taskPracuj;
            var jobsFromJJIT = await taskJJIT;

            foreach (var job in jobsFromPracuj)
            {
                JobsList.Add(job);
            }
            foreach (var job in jobsFromJJIT)
            {
                JobsList.Add(job);
            }
            if (JobsList.Count == 0)
            {
                   JobsList.Add(new JobViewModel { JobName = "brak wynikow", JobLink = "" });

            }
        }




    }
}
