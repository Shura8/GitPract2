using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VapeAPI.Models;

namespace Admin
{
    /// <summary>
    /// Логика взаимодействия для UserWin.xaml
    /// </summary>
    public partial class UserWin : Window
    {
        AuthWin main = new AuthWin();
        public UserWin()
        {
            InitializeComponent();
            GetUser();
        }

        public async void GetUser()
        {
            var response = await main.client.GetStringAsync("Users");
            var user = JsonConvert.DeserializeObject<List<User>>(response);
            dgUsers.ItemsSource = user;
            dgUsers.Columns[0].Visibility = Visibility.Hidden;
            dgUsers.Columns[1].Header = "Фамилия";
            dgUsers.Columns[2].Header = "Имя";
            dgUsers.Columns[3].Header = "Отчество";
            dgUsers.Columns[4].Header = "Id типа пользователя";
            dgUsers.Columns[5].Header = "Логин";
            dgUsers.Columns[6].Header = "Пароль";
            dgUsers.Columns[7].Header = "Соль";
            dgUsers.Columns[8].Header = "Удалена";
        }
        public async void AddUser(User user)
        {
            await main.client.PostAsJsonAsync("Users/signup", user);
            GetUser();
        }
        public async void UpdateUser(User user)
        {
            var response = await main.client.GetStringAsync("Users");
            var users = JsonConvert.DeserializeObject<List<User>>(response);
            foreach (var c in users)
            {
                if (c.IdUsers == user.IdUsers)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("Users/" + user.IdUsers, user);
                        break;
                    }
                }
            }
            GetUser();
        }

        public async void DelUser(int UserId)
        {
            var response = await main.client.GetStringAsync("Users");
            var users = JsonConvert.DeserializeObject<List<User>>(response);
            foreach (var c in users)
            {
                if (c.IdUsers == UserId)
                {
                    if (c.IsDeleted == false)
                    {
                        var user = new User()
                        {
                            IdUsers = c.IdUsers,
                            SurnameUsers = c.SurnameUsers,
                            NameUsers = c.NameUsers,
                            FatherNameUsers = c.FatherNameUsers,
                            TypeUsersId = c.TypeUsersId,
                            LoginUsers = c.LoginUsers,
                            PasswordUsers = c.PasswordUsers,
                            SaltUsers = c.SaltUsers,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("Users/" + user.IdUsers, user);
                        break;
                    }
                    else
                    {
                        var user = new User()
                        {
                            IdUsers = c.IdUsers,
                            SurnameUsers = c.SurnameUsers,
                            NameUsers = c.NameUsers,
                            FatherNameUsers = c.FatherNameUsers,
                            TypeUsersId = c.TypeUsersId,
                            LoginUsers = c.LoginUsers,
                            PasswordUsers = c.PasswordUsers,
                            SaltUsers = c.SaltUsers,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("Users/" + user.IdUsers, user);
                        break;
                    }
                }
            }
            GetUser();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = new User()
                {
                    SurnameUsers = surname.Text,
                    NameUsers = name.Text,
                    FatherNameUsers = fatherName.Text,
                    TypeUsersId = Convert.ToInt32(typeId.Text),
                    LoginUsers = login.Text,
                    PasswordUsers = password.Text,
                    SaltUsers = "",
                    IsDeleted = false
                };
                AddUser(user);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = new User()
                {
                    IdUsers = dgUsers.SelectedIndex + 1,
                    SurnameUsers = surname.Text,
                    NameUsers = name.Text,
                    FatherNameUsers = fatherName.Text,
                    TypeUsersId = Convert.ToInt32(typeId.Text),
                    LoginUsers = login.Text,
                    PasswordUsers = password.Text,
                    SaltUsers = "",
                    IsDeleted = false
                };
                UpdateUser(user);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelUser(dgUsers.SelectedIndex + 1);
        }
    }
}
