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
    /// Логика взаимодействия для TypeUserWin.xaml
    /// </summary>
    public partial class TypeUserWin : Window
    {
        AuthWin main = new AuthWin();
        public TypeUserWin()
        {
            InitializeComponent();
            GetTypeUser();
        }

        public async void GetTypeUser()
        {
            var response = await main.client.GetStringAsync("TypeUsers");
            var typeUser = JsonConvert.DeserializeObject<List<TypeUser>>(response);
            dgTypeUser.ItemsSource = typeUser;
            dgTypeUser.Columns[0].Visibility = Visibility.Hidden;
            dgTypeUser.Columns[1].Header = "Название типа пользователя";
            dgTypeUser.Columns[2].Header = "Удалена";
        }
        public async void AddTypeUser(TypeUser typeUser)
        {
            await main.client.PostAsJsonAsync("TypeUsers", typeUser);
            GetTypeUser();
        }
        public async void UpdateTypeUser(TypeUser typeUser)
        {
            var response = await main.client.GetStringAsync("TypeUsers");
            var typeUsers = JsonConvert.DeserializeObject<List<TypeUser>>(response);
            foreach (var c in typeUsers)
            {
                if (c.IdTypeUser == typeUser.IdTypeUser)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("TypeUsers/" + typeUser.IdTypeUser, typeUser);
                        break;
                    }
                }
            }
            GetTypeUser();
        }

        public async void DelTypeUser(int TypeUserId)
        {
            var response = await main.client.GetStringAsync("TypeUsers");
            var typeUsers = JsonConvert.DeserializeObject<List<TypeUser>>(response);
            foreach (var c in typeUsers)
            {
                if (c.IdTypeUser == TypeUserId)
                {
                    if (c.IsDeleted == false)
                    {
                        var typeUser = new TypeUser()
                        {
                            IdTypeUser = c.IdTypeUser,
                            NameTypeUser = c.NameTypeUser,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("TypeUsers/" + typeUser.IdTypeUser, typeUser);
                        break;
                    }
                    else
                    {
                        var typeUser = new TypeUser()
                        {
                            IdTypeUser = c.IdTypeUser,
                            NameTypeUser = c.NameTypeUser,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("TypeUsers/" + typeUser.IdTypeUser, typeUser);
                        break;
                    }
                }
            }
            GetTypeUser();
        }
        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var typeUser = new TypeUser()
                {
                    NameTypeUser = name.Text,
                    IsDeleted = false
                };
                AddTypeUser(typeUser);
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
                var typeUser = new TypeUser()
                {
                    IdTypeUser = dgTypeUser.SelectedIndex + 1,
                    NameTypeUser = name.Text,
                    IsDeleted = false
                };
                UpdateTypeUser(typeUser);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelTypeUser(dgTypeUser.SelectedIndex + 1);
        }
    }
}
