using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using MVP_Tema_3.Commands;
using MVP_Tema_3.Views;

namespace MVP_Tema_3.ViewModels
{
    public class InterventionsViewModel
    {

        public ObservableCollection<Intervention> Interventions { get; set; }
        public ICollectionView InterventionsCollectionView { get; set; }

        public InterventionsViewModel()
        {
            try
            {
                DatabaseContext databaseContext = new DatabaseContext();
                var listOfUsers = databaseContext.Interventions.ToList();
                //remove admin
                Interventions = new ObservableCollection<Intervention>(listOfUsers);

                InterventionsCollectionView = CollectionViewSource.GetDefaultView(Interventions);
                InterventionsCollectionView.Filter += o => ((Intervention)o).Deleted == "n";
                AddInterventionCommand = new RelayCommand(AddIntervention);
                UpdateInterventionCommand = new RelayCommand(UpdateIntervention, IsNotNull);
                DeleteInterventionCommand = new RelayCommand(DeleteIntervention, IsNotNull);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        private void UpdateIntervention(object o)
        {
            AddOrUpdateInterventionView addOrUpdateInterventionView = new AddOrUpdateInterventionView();
            AddOrUpdateInterventionViewModel addOrUpdateInterventionViewModel =
                ((AddOrUpdateInterventionViewModel) addOrUpdateInterventionView.DataContext);
            addOrUpdateInterventionViewModel.InterventionsViewModel = this;
            addOrUpdateInterventionViewModel.Action = Enums.Action.Update;
            addOrUpdateInterventionViewModel.Intervention = (Intervention)o;
            addOrUpdateInterventionViewModel.Price = ((Intervention)o).Prices.Last();
            addOrUpdateInterventionViewModel.LastPrice =((Intervention) o).Prices.Last().Value;
            addOrUpdateInterventionViewModel.LastDate = ((Intervention) o).Prices.Last().StartingDate;
            addOrUpdateInterventionView.Show();
        }

        private void AddIntervention(object o)
        {
            AddOrUpdateInterventionView addOrUpdateInterventionView = new AddOrUpdateInterventionView();
            AddOrUpdateInterventionViewModel addOrUpdateInterventionViewModel =
                ((AddOrUpdateInterventionViewModel)addOrUpdateInterventionView.DataContext);

            addOrUpdateInterventionViewModel.InterventionsViewModel = this;
            addOrUpdateInterventionViewModel.Action = Enums.Action.Add;
            addOrUpdateInterventionView.Show();
        }

        private bool IsNotNull(object o) => o != null;
        private void DeleteIntervention(object o)
        {
            DatabaseContext databaseContext = new DatabaseContext();
            int id = ((Intervention)o).IdIntervention;
            Intervention intervention = databaseContext.Interventions.Find(id);
            if (intervention != null)
            {
                intervention.Deleted = "y";
                ((Intervention) o).Deleted = "y";
            }
            databaseContext.SaveChanges();
            InterventionsCollectionView.Refresh();

        }

        public ICommand AddInterventionCommand { get; set; }
        public ICommand UpdateInterventionCommand { get; set; }
        public ICommand DeleteInterventionCommand { get; set; }
    }
}
