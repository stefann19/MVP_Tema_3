using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using MVP_Tema_3.Annotations;
using MVP_Tema_3.Commands;
using MVP_Tema_3.Views;
using Action = MVP_Tema_3.Enums.Action;

namespace MVP_Tema_3.ViewModels
{
    public class DoctorsViewModel :INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; }
        public ICollectionView DoctorsCollectionView { get; set; }

        public DoctorsViewModel()
        {
            try
            {
                DatabaseContext databaseContext = new DatabaseContext();
                var listOfUsers = databaseContext.Users.ToList();
                //remove admin
                var listOfDoctors = listOfUsers.Where(user => user.Username != "admin" && user.Deleted == "n");
                Users = new ObservableCollection<User>(listOfDoctors);
                DoctorsCollectionView = CollectionViewSource.GetDefaultView(Users);
                DoctorsCollectionView.Filter += o => ((User)o).Deleted == "n";

                DeleteUserCommand = new RelayCommand(DeleteDoctor, IsNotNull);
                AddUserCommand = new RelayCommand(AddDoctor);
                UpdateUserCommand = new RelayCommand(UpdateDoctor, IsNotNull);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

        private bool IsNotNull(object o) => o != null;

        private void UpdateDoctor(object o)
        {
            AddOrUpdateDoctorView addOrUpdateDoctorView = new AddOrUpdateDoctorView();
            ((AddOrUpdateDoctorViewModel)addOrUpdateDoctorView.DataContext).AdminViewModel = this;
            ((AddOrUpdateDoctorViewModel)addOrUpdateDoctorView.DataContext).Action = Action.Update;
            ((AddOrUpdateDoctorViewModel) addOrUpdateDoctorView.DataContext).User = (User)o;
            addOrUpdateDoctorView.Show();
        }
        private void AddDoctor(object o)
        {
            AddOrUpdateDoctorView addOrUpdateDoctorView = new AddOrUpdateDoctorView();
            ((AddOrUpdateDoctorViewModel) addOrUpdateDoctorView.DataContext).AdminViewModel = this;
            ((AddOrUpdateDoctorViewModel)addOrUpdateDoctorView.DataContext).Action = Action.Add;

            addOrUpdateDoctorView.Show();
        }
        private void DeleteDoctor(object o)
        {
            DatabaseContext databaseContext = new DatabaseContext();

            User user = (User) o;
            object[] sqlParams = {
                new SqlParameter
                {
                    ParameterName = "@idUser",
                    Value = user.IdUser,
                    Direction = System.Data.ParameterDirection.Input
                },

            };

            databaseContext.Database.ExecuteSqlCommand("exec DeleteDoctor @idUser", sqlParams);
            databaseContext.SaveChanges();

            Users.First(u => u.IdUser == user.IdUser).Deleted = "y";
            DoctorsCollectionView.Refresh();

        }

       

        public ICommand AddUserCommand { get; set; }
        public ICommand UpdateUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
       

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
           
        }
    }
}
