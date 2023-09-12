using Admin.Models;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using VapeAPI.Models;

namespace Admin
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AuthWin : Window
    {
        public HttpClient client = new HttpClient();
        RegistryKey currentUserKey = Registry.CurrentUser;
        AuthUser user = new AuthUser();
        AdminMenu admin = new AdminMenu();
        string Login;
        string Password;
        public AuthWin()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("https://192.168.3.30:7135/api/"); 
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );

            RegistryKey helloKey = currentUserKey.OpenSubKey("WindowSize");
            this.Height = Convert.ToDouble(helloKey.GetValue("Height"));
            this.Width = Convert.ToDouble(helloKey.GetValue("Width"));
            this.Left = Convert.ToDouble(helloKey.GetValue("Left"));
            this.Top = Convert.ToDouble(helloKey.GetValue("Top"));
            helloKey.Close();
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RegistryKey helloKey = currentUserKey.CreateSubKey("WindowSize");
            helloKey.SetValue("Height", this.Height);
            helloKey.SetValue("Width", this.Width);
            helloKey.SetValue("Left", this.Left);
            helloKey.SetValue("Top", this.Top);
            helloKey.Close();
        }
        private async void btAuth_Click(object sender, RoutedEventArgs e)
        {
            user.LoginUsers = tbLogin.Text;
            user.PasswordUsers = tbPass.Text;
            var response = await client.PostAsJsonAsync("Users/signinAd", user);
            if (response.IsSuccessStatusCode)
            {
                RegistryKey helloKey = currentUserKey.CreateSubKey("LastUser");

                var responseUser = await client.GetStringAsync("Users");
                var user = JsonConvert.DeserializeObject<List<User>>(responseUser);
                foreach(var item in user)
                {
                    if(item.LoginUsers == tbLogin.Text)
                    {
                        Login = item.LoginUsers;
                        Password = item.PasswordUsers; 
                    }
                }

                helloKey.SetValue("login", Login);
                helloKey.SetValue("password", Password);
                helloKey.Close();
                MessageBox.Show("Авторизация прошла успшено!");
                admin.Show();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль!");
            }
        }

    }
}