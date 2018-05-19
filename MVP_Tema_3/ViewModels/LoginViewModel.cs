using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MVP_Tema_3.Commands;
using MVP_Tema_3.Views;

namespace MVP_Tema_3.ViewModels
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            Username = "admin";
            Password = "admin";
            LoginCommand = new RelayCommand(Login,CanLogin);
        }

        private bool CanLogin(object o)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                return dbContext.Users.Any(user => user.Username.Equals(Username) && user.Password.Equals(Password));
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        private void Login(object o)
        {

            DatabaseContext dbContext = new DatabaseContext();
            User loggedUser = dbContext.Users.FirstOrDefault(user => user.Username==Username && user.Password == Password);
            if (loggedUser == null)
            {
                Debug.WriteLine("User not found");
                return;
            }

            if (loggedUser.Type == "admin")
            {
                DoctorsView doctorsView = new DoctorsView();
                doctorsView.Show();
                InterventionsView interventionsView = new InterventionsView();
                interventionsView.Show();
            }
            else
            {
                DoctorMenuView doctorView = new DoctorMenuView();
                doctorView.Show();
            }

            Application.Current.Windows.OfType<LoginView>().First().Close();//sau hide , dunno

        }

        public string Username { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; set; }
    }
}
