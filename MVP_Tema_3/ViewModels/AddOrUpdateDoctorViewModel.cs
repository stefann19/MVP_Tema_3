using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
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
    

    public class AddOrUpdateDoctorViewModel :INotifyPropertyChanged
    {
        private User _user;
        public DoctorsViewModel AdminViewModel { get; set; }

        public Action Action { get; set; }
        public AddOrUpdateDoctorViewModel()
        {
            User = new User
            {
                Type = "doctor",
                Deleted = "n"
            };
            AddOrModifyUserCommand = new RelayCommand(AddOrModifyUser);
        }

        private void AddOrModifyUser(object o)
        {
            DatabaseContext dbContext = new DatabaseContext();
            switch (Action)
            {
                case Action.Add:
                {
                    object[] sqlParams =
                    {
                        new SqlParameter
                        {
                            ParameterName = "@username",
                            Value = User.Username,
                            Direction = System.Data.ParameterDirection.Input
                        },
                        new SqlParameter
                        {
                            ParameterName = "@password",
                            Value = User.Password,
                            Direction = System.Data.ParameterDirection.Input
                        },
                        new SqlParameter
                        {
                            ParameterName = "@firstName",
                            Value = User.FirstName,
                            Direction = System.Data.ParameterDirection.Input
                        },
                        new SqlParameter
                        {
                            ParameterName = "@lastName",
                            Value = User.LastName,
                            Direction = System.Data.ParameterDirection.Input
                        },
                    };
                    dbContext.Database.ExecuteSqlCommand("exec InsertDoctor @username,@password,@firstName,@lastName",
                        sqlParams);
                    dbContext.SaveChanges();
                    AdminViewModel.Users.Add(User);
                }
                    break;
                case Action.Update:
                {
                    object[] sqlParams =
                    {
                        new SqlParameter
                        {
                            ParameterName = "@idUser",
                            Value = User.IdUser,
                            Direction = System.Data.ParameterDirection.Input
                        },
                        new SqlParameter
                        {
                            ParameterName = "@username",
                            Value = User.Username,
                            Direction = System.Data.ParameterDirection.Input
                        },
                        new SqlParameter
                        {
                            ParameterName = "@password",
                            Value = User.Password,
                            Direction = System.Data.ParameterDirection.Input
                        },
                        new SqlParameter
                        {
                            ParameterName = "@firstName",
                            Value = User.FirstName,
                            Direction = System.Data.ParameterDirection.Input
                        },
                        new SqlParameter
                        {
                            ParameterName = "@lastName",
                            Value = User.LastName,
                            Direction = System.Data.ParameterDirection.Input
                        },
                    };
                    dbContext.Database.ExecuteSqlCommand("exec UpdateDoctor @idUser,@username,@password,@firstName,@lastName",sqlParams);
                    dbContext.SaveChanges();
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();

            }
            AdminViewModel.DoctorsCollectionView.Refresh();
            Application.Current.Windows.OfType<AddOrUpdateDoctorView>().First().Close();
            
        }



        public User User {
            get => _user;
            set {
                if (Equals(value, _user)) return;
                _user = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddOrModifyUserCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}