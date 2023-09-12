using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Window
    {
        RegistryKey currentUserKey = Registry.CurrentUser;
        public AdminMenu()
        {
            InitializeComponent();
            RegistryKey helloKey = currentUserKey.OpenSubKey("WindowAdminSize");
            this.Height = Convert.ToDouble(helloKey.GetValue("Height"));
            this.Width = Convert.ToDouble(helloKey.GetValue("Width"));
            this.Left = Convert.ToDouble(helloKey.GetValue("Left"));
            this.Top = Convert.ToDouble(helloKey.GetValue("Top"));
            helloKey.Close();
        }

        private void btCigarette_Click(object sender, RoutedEventArgs e)
        {
            Cigarette cigarette = new Cigarette();
            cigarette.Show();
        }

        private void btElCigarette_Click(object sender, RoutedEventArgs e)
        {
            ElCigarette elCigarette = new ElCigarette();
            elCigarette.Show();
        }

        private void btFluid_Click(object sender, RoutedEventArgs e)
        {
            Fluid fluid = new Fluid();
            fluid.Show();
        }

        private void btTaste_Click(object sender, RoutedEventArgs e)
        {
            Taste taste = new Taste();
            taste.Show();
        }

        private void btTobacco_Click(object sender, RoutedEventArgs e)
        {
            Tobacco tobacco = new Tobacco();
            tobacco.Show();
        }

        private void btOrderCigarette_Click(object sender, RoutedEventArgs e)
        {
            OrderCigaretteWin orderCigarette = new OrderCigaretteWin();
            orderCigarette.Show();
        }

        private void btOrderElCigarette_Click(object sender, RoutedEventArgs e)
        {
            OrderElCigaretteWin orderElCigaretteWin = new OrderElCigaretteWin();
            orderElCigaretteWin.Show();
        }

        private void btOrderFluid_Click(object sender, RoutedEventArgs e)
        {
            OrderFluidWin orderFluidWin = new OrderFluidWin();
            orderFluidWin.Show();
        }

        private void btOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderWin orderWin = new OrderWin();
            orderWin.Show();
        }

        private void btShop_Click(object sender, RoutedEventArgs e)
        {
            ShopWin shopWin = new ShopWin();
            shopWin.Show();
        }

        private void btStatusOrder_Click(object sender, RoutedEventArgs e)
        {
            StatusOrderWin statusOrderWin = new StatusOrderWin();
            statusOrderWin.Show(); 
        }

        private void btStrength_Click(object sender, RoutedEventArgs e)
        {
            StrengthWin strengthWin = new StrengthWin();
            strengthWin.Show();
        }

        private void btTypeElCigarette_Click(object sender, RoutedEventArgs e)
        {
            TypeElCigaretteWin typeElCigaretteWin = new TypeElCigaretteWin();
            typeElCigaretteWin.Show();
        }

        private void btUser_Click(object sender, RoutedEventArgs e)
        {
            UserWin userWin = new UserWin();
            userWin.Show();
        }

        private void btTypeUser_Click(object sender, RoutedEventArgs e)
        {
            TypeUserWin typeUserWin = new TypeUserWin();
            typeUserWin.Show();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RegistryKey helloKey = currentUserKey.CreateSubKey("WindowAdminSize");
            helloKey.SetValue("Height", this.Height);
            helloKey.SetValue("Width", this.Width);
            helloKey.SetValue("Left", this.Left);
            helloKey.SetValue("Top", this.Top);
            helloKey.Close();
        }

    }
}
