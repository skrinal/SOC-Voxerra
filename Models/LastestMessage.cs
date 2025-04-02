using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Models
{
    public class LastestMessage : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Id { get; set; }
        public int UserId { get; set; }

        [JsonProperty("userFriendInfo")]
        public User UserFiendInfo { get; set; }

        private string content;
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged();
            }
        }
        public DateTime SendDateTime { get; set; }
        public bool IsRead { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

         
        

        //private int id;
        //private int userId;
        //private User userFiendInfo;
        //private string content;
        //private DateTime sendDateTime;
        //private bool isRead;

        //public int Id
        //{
        //    get => id;
        //    set { id = value; OnPropertyChanged(); }
        //}

        //public int UserId
        //{
        //    get => userId;
        //    set { userId = value; OnPropertyChanged(); }
        //}

        //[JsonProperty("userFriendInfo")]
        //public User UserFiendInfo
        //{
        //    get => userFiendInfo;
        //    set { userFiendInfo = value; OnPropertyChanged(); }
        //}

        //public string Content
        //{
        //    get => content;
        //    set { content = value; OnPropertyChanged(); }
        //}

        //public DateTime SendDateTime
        //{
        //    get => sendDateTime;
        //    set { sendDateTime = value; OnPropertyChanged(); }
        //}

        //public bool IsRead
        //{
        //    get => isRead;
        //    set { isRead = value; OnPropertyChanged(); }
        //}
    }
}
