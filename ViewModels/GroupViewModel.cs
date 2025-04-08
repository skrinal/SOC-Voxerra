using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Voxerra.Services.GroupChat;
using Voxerra.Services.Message;

namespace Voxerra.ViewModels;

public class GroupViewModel : INotifyPropertyChanged, IQueryAttributable
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        FromUserId = int.Parse(HttpUtility.UrlDecode(query["fromUserId"].ToString()));
        ToGroupId = int.Parse(HttpUtility.UrlDecode(query["toGroupId"].ToString()));

        Initialize();
    }

    private ServiceProvider _serviceProvider;
    private ChatHub _chatHub;
    private DataCenterService _dataCenterService;

    public GroupViewModel(
        ServiceProvider serviceProvider,
        ChatHub chatHub,
        DataCenterService stateService)
    {
        _serviceProvider = serviceProvider;
        _chatHub = chatHub;
        _dataCenterService = stateService;


        BackToHome = new Command(async () => { await Shell.Current.GoToAsync($"//GroupCenterPage"); });

        SendMessageCommand = new Command(async () =>
        {
            try
            {
                if (Message.Trim() != "")
                {
                    /*ScrollUpdating = "KeepLastItemInView";
                    OnPropertyChanged(nameof(ScrollUpdating));*/

                    //await _chatHub.SendMessageToUser(FromUserId, ToUserId, Message);
                    
                    
                    // TODO: Message to group
                    
                    
                    Messages.Add(new GroupMessages
                    {
                        Content = Message,
                        FromUserId = fromUserId,
                        ToGroupId = toGroupId,
                        SendDateTime = DateTime.Now,
                    });

                    WeakReferenceMessenger.Default.Send(new ScrollToBottomMessage(true));

                    Message = "";
                }
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        });
    }

    public async Task GetMessages()
    {
        
        var response = await _serviceProvider.CallWebApi<int, GroupMessagesResponse>
            ("/GroupChat/GetGroupMessages", HttpMethod.Post, ToGroupId);

        if (response.StatusCode == 200)
        {
            FriendsInfo = new ObservableCollection<User>(response.FriendsInfo);
            Messages = new ObservableCollection<GroupMessages>(response.Messages);
                
        }
        else
        {
            await AppShell.Current.DisplayAlert("Voxerra", response.StatusMessage, "OK");
        }
    }
    
    
    public void Initialize()
    {
        Task.Run(async () =>
        {
            IsRefreshing = true;
            await GetMessages();
        }).GetAwaiter().OnCompleted(() => { IsRefreshing = false; });
    }

    private void OnReceiveMessage(int fromUserId, string message)
    {
        Messages.Add(new GroupMessages
        {
            Content = message,
            FromUserId = ToGroupId,
            //ToUserId = FromUserId,
            SendDateTime = DateTime.Now,
        });
    }


    private int fromUserId;
    private int toGroupId;
    private ObservableCollection<User> friendsInfo;
    private ObservableCollection<GroupMessages> messages;
    private bool isRefreshing;
    private string message;
    private string scrollUpdating;

    private CollectionView messagesCollectionView;

    public CollectionView MessagesCollectionView
    {
        get => messagesCollectionView;
        set
        {
            messagesCollectionView = value;
            OnPropertyChanged();
        }
    }


    public int FromUserId
    {
        get { return fromUserId; }
        set
        {
            fromUserId = value;
            OnPropertyChanged();
        }
    }

    public int ToGroupId
    {
        get { return toGroupId; }
        set
        {
            toGroupId = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<User> FriendsInfo
    {
        get { return friendsInfo; }
        set
        {
            friendsInfo = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<GroupMessages> Messages
    {
        get { return messages; }
        set
        {
            messages = value;
            OnPropertyChanged();
        }
    }

    public bool IsRefreshing
    {
        get { return isRefreshing; }
        set
        {
            isRefreshing = value;
            OnPropertyChanged();
        }
    }

    public string Message
    {
        get { return message; }
        set
        {
            message = value;
            OnPropertyChanged();
        }
    }
    

    public ICommand SendMessageCommand { get; set; }
    public ICommand LoadOlderMessagesCommand { get; set; }
    public ICommand BackToHome { get; set; }
}

public class ScrollToBottomMessage : ValueChangedMessage<bool>
{
    public ScrollToBottomMessage(bool value) : base(value)
    {
    }
}