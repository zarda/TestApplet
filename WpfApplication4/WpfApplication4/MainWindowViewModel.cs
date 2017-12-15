using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MouseOverEventMVVM
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Private property
        private ICommand _navigateCommand1;
        private ICommand _navigateCommand2;
        private ICommand _navigateCommand3;
        #endregion

        #region Public property
        public event PropertyChangedEventHandler PropertyChanged;
        public WFP_Property property1 { get; set; } = new WFP_Property("property1", 20);
        public WFP_Property property2 { get; set; } = new WFP_Property("property2", 20);
        public WFP_Property property3 { get; set; } = new WFP_Property("property3", 20);
        #endregion

        #region Public method
        public ICommand CommandMouseEnterButton1
        {
            get
            {
                return _navigateCommand1 ?? (
                    _navigateCommand1 = new RelayCommand(
                          x =>
                          {
                              DoStuff(property1);
                          }));
            }
        }

        public ICommand CommandMouseLeaveButton1
        {
            get
            {
                return _navigateCommand1 ?? (
                    _navigateCommand1 = new RelayCommand(
                          x =>
                          {
                              DoStuff(property1);
                          }));
            }
        }

        public ICommand CommandMouseClickButton1
        {
            get
            {
                return new RelayCommand(
                    x =>
                    {
                        MessageBox.Show("Event1");
                    });
            }
        }

        public ICommand CommandMouseEnterButton2
        {
            get
            {
                return _navigateCommand2 ?? (
                    _navigateCommand2 = new RelayCommand(
                          x =>
                          {
                              DoStuff(property2);
                          }));
            }
        }

        public ICommand CommandMouseLeaveButton2
        {
            get
            {
                return _navigateCommand2 ?? (
                    _navigateCommand2 = new RelayCommand(
                          x =>
                          {
                              DoStuff(property2);
                          }));
            }
        }

        public ICommand CommandMouseClickButton2
        {
            get
            {
                return new RelayCommand(
                    x =>
                    {
                        MessageBox.Show("Event2");
                    });
            }
        }

        public ICommand CommandMouseEnterButton3
        {
            get
            {
                return _navigateCommand3 ?? (
                    _navigateCommand3 = new RelayCommand(
                          x =>
                          {
                              DoStuff(property3);
                          }));
            }
        }

        public ICommand CommandMouseLeaveButton3
        {
            get
            {
                return _navigateCommand3 ?? (
                    _navigateCommand3 = new RelayCommand(
                          x =>
                          {
                              DoStuff(property3);
                          }));
            }
        }

        public ICommand CommandMouseClickButton3
        {
            get
            {
                return new RelayCommand(
                    x =>
                    {
                        MessageBox.Show("Event3");
                    });
            }
        }
        #endregion

        #region Public help method
        public void DoStuff(WFP_Property _property)
        {
            //MessageBox.Show("Mouse Over Button Event");
            switch ((int)_property.fontSize)
            {
                case 30:
                    _property.fontSize = 20;
                    break;
                case 20:
                    _property.fontSize = 30;
                    break;
            }
            PropertyChange.RaisePropertyChanged(
                this,
                _property.name,
                PropertyChanged);
        }
        #endregion
    }
}
