using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MVP_Tema_3.Annotations;
using MVP_Tema_3.Commands;
using MVP_Tema_3.Views;
using Action = MVP_Tema_3.Enums.Action;

namespace MVP_Tema_3.ViewModels
{
    internal class AddOrUpdateInterventionViewModel :INotifyPropertyChanged
    {
        private Intervention _intervention;
        private Price _price;
        public int LastPrice { get; set; }
        public DateTime LastDate { get; set; }
        public Intervention Intervention {
            get => _intervention;
            set {
                if (Equals(value, _intervention)) return;
                _intervention = value;
                OnPropertyChanged();
            }
        }

        public Price Price {
            get => _price;
            set {
                if (Equals(value, _price)) return;
                _price = value;
                OnPropertyChanged();
            }
        }

        public Action Action { get; set; }
        public InterventionsViewModel InterventionsViewModel { get; set; }
        public AddOrUpdateInterventionViewModel()
        {
            Intervention = new Intervention {Deleted = "n"};
            Intervention.Prices.Add(Price);
            Price = new Price {Deleted = "n"};
            AddOrUpdateCommand = new RelayCommand(AddOrUpdate);
        }

        private void AddOrUpdate(object o)
        {
            DatabaseContext databaseContext = new DatabaseContext();
            switch (Action)
            {
                case Action.Add: {
                    databaseContext.Interventions.Add(Intervention);
                    databaseContext.Prices.Add(Price);
                    databaseContext.SaveChanges();
                    InterventionsViewModel.Interventions.Add(Intervention);
                }
                    break;
                case Action.Update:
                    int id = Intervention.IdIntervention;
                    Intervention interv= databaseContext.Interventions.Find(id);
                    if (interv != null)
                    {
                        interv.Description = Intervention.Description;
                        interv.Duration = Intervention.Duration;
                        if (LastPrice != Price.Value || LastDate != Price.StartingDate)
                        {
                            interv.Prices.Last().FinalDate = Price.StartingDate.AddDays(-1);
                            Price price = new Price
                            {
                                Value = Price.Value,
                                StartingDate = Price.StartingDate,
                                Deleted = "n"
                            };

                            interv.Prices.Add(price);
                            databaseContext.Prices.Add(price);
                        }
                        /*else
                        {
                            interv.Prices.Last().StartingDate = Price.StartingDate;
                        }*/
                    }
/*
                    databaseContext.Interventions.AddOrUpdate(Intervention);
*/
                    databaseContext.SaveChanges();
                    break;
                default:
                    break;
            }
            InterventionsViewModel.InterventionsCollectionView.Refresh();
            Application.Current.Windows.OfType<AddOrUpdateInterventionView>().First().Close();
        }


        public ICommand AddOrUpdateCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}