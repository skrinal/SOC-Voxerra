using System.Collections.ObjectModel;

namespace Voxerra.Services;

public class DataCenterService
{
    public User UserInfo { get; set; } = new User();
    public ObservableCollection<User> UserFriends { get; set; } = new ObservableCollection<User>();
    public ObservableCollection<LastestMessage> LastestMessages { get; set; } = new ObservableCollection<LastestMessage>();
    public ObservableCollection<UserSearch> UserSearch { get; set; } = new ObservableCollection<UserSearch>();
    
    public void Reset()
    {
        UserInfo = new User();
        UserFriends.Clear();
        LastestMessages.Clear();
        UserSearch.Clear();
    }
}