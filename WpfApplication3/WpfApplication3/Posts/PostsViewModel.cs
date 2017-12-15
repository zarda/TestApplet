using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApplication3
{
    class PostsViewModel : INotifyPropertyChanged
    {
        public Posts posts { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        //定義一個ICommand型別的參數，他會回傳實作ICommand介面的RelayCommand類別。
        public ICommand UpdateTitleName { get { return new RelayCommand(UpdateTitleExecute, CanUpdateTitleExecute); } }
        
        public PostsViewModel()
        {
            posts = new Posts { postsText = "", postsTitle = "Unknown", postSize = 10 };
        }

        public string PostsTitle
        {
            get { return posts.postsTitle; }
            set
            {
                if (posts.postsTitle != value)
                {
                    posts.postsTitle = value;            
                }
                else
                {
                    posts.postsTitle = "Unknown";
                }
                RaisePropertyChanged("postsTitle");
            }
        }
        public int PostSize
        {
            get { return posts.postSize; }
            set
            {
                switch (posts.postSize)
                {
                    case 10:
                        posts.postSize = 20;
                        break;
                    case 20:
                        posts.postSize = 10;
                        break;
                }
                RaisePropertyChanged("posts");
            }
        }
 

        //產生事件的方法
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //更新Title，原本放在View那邊的邏輯，藉由繫結的方式來處理按下Button的事件。
        void UpdateTitleExecute()
        {
            PostsTitle = "SkyMVVM";
            PostSize = 10;
        }

        //定義是否可以更新Title
        bool CanUpdateTitleExecute()
        {
            return true;
        }
    }
}
