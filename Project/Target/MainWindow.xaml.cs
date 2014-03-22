using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Target {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel() { Name = "Foo" };
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged {

        private string _name;
        public string Name {
            get {
                return this._name;
            }
            set {
                if (this._name != value) {
                    this._name = value;
                    if (this.PropertyChanged != null) {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
