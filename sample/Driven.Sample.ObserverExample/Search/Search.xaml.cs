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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ObserverExample.Search
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : UserControl
    {
        protected SearchViewModel ViewModel
        {
            get { return this.DataContext as SearchViewModel; }
            set { this.DataContext = value; }
        }

        public Search()
        {
            InitializeComponent();

            ViewModel = new SearchViewModel();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ViewModel.Search();
        }
    }
}
