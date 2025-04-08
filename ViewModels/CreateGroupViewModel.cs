using System.Text.RegularExpressions;

namespace Voxerra.ViewModels;

public class CreateGroupViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    private ServiceProvider _serviceProvider;

    public CreateGroupViewModel(ServiceProvider serviceProvider)
    {
        ResultColor = "transparent";
        ResultText = "";
        
        _serviceProvider = serviceProvider;

        CreateGroupCommand = new Command(() =>
        {
            if (!isNameValid) return;
            
            if (IsProcessing) return;
            
            IsProcessing = true;
            CreateGroupSend().GetAwaiter().OnCompleted(() =>
            {
                IsProcessing = false;
            });
        });
        
        GoBackCommand = new Command(OnGoBack);
    }

    private async void OnGoBack()
    {
        await Shell.Current.GoToAsync($"..");
        GroupName = "";
    }
    
    private async Task CreateGroupSend()
    {
        try
        {
            var response = await _serviceProvider.CallWebApi<string, BaseResponse>
                ("/GroupChat/CreateGroup", HttpMethod.Post, GroupName);

            if (response.StatusCode == 200)
            {
                ResultColor = "Group has been created.";
                ResultText = "green";
                //await AppShell.Current.DisplayAlert("Voxerra", "Group has been created.", "OK");
            }
            else
            {
                ResultColor = "Failed to create group.";
                ResultText = "red";
                //await AppShell.Current.DisplayAlert("Voxerra", response.StatusMessage, "OK");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private bool CheckName(string groupNameTest)
    {
        ButtonStatus = false;
        
        RuleColor1 = "white";
        RuleColor2 = "white";
        RuleColor3 = "white";
        
        LabelIcon = "";
        LabelColor = "transparent";
        
        ResultText = "";
        ResultColor = "transparent";
        

        if (string.IsNullOrWhiteSpace(groupNameTest))
        {
            RuleColor1 = "white";
            RuleColor2 = "white";
            RuleColor3 = "white";
            LabelIcon = "";
            LabelColor = "transparent";
            return false;
        }

        if (groupNameTest.StartsWith(" ") || groupNameTest.EndsWith(" "))
        {
            RuleColor3 = "red";
            LabelIcon = "close";
            LabelColor = "Red";
            return false;
        }

        if (groupNameTest.Length >= 18 || groupNameTest.Length < 3)
        {
            RuleColor1 = "red";
            LabelIcon = "close";
            LabelColor = "Red";
            return false;
        }


        if (!Regex.IsMatch(groupNameTest, @"^[a-zA-Z0-9]+$"))
        {
            RuleColor2 = "red";
            LabelIcon = "close";
            LabelColor = "Red";
            return false;
        }
            
        RuleColor1 = "white";
        RuleColor2 = "white";
        RuleColor3 = "white";
        LabelIcon = "check";
        LabelColor = "Green";
        ButtonStatus = true;
        return true;
    }
    
    
    private bool isProcessing;
    private string groupName;
    private bool isNameValid;
    
    private bool buttonStatus;
    
    private string labelIcon;
    private string labelColor;

    private string ruleColor1;
    private string ruleColor2;
    private string ruleColor3;

    private string resultColor;
    private string resultText;
    public bool IsProcessing
    {
        get { return isProcessing; }
        set { isProcessing = value; OnPropertyChanged(); }
    }
    public string GroupName
    {
        get { return groupName; }
        set
        {
            ButtonStatus = false;
            
            groupName = value; 
            OnPropertyChanged();

            isNameValid = CheckName(value);
        }
    }
    
    public bool ButtonStatus
    {
        get { return buttonStatus; }
        set
        {
            buttonStatus = value;
            OnPropertyChanged();
        }
    }

    public string LabelIcon
    {
        get { return labelIcon; }
        set
        {
            labelIcon = value;
            OnPropertyChanged();
        }
    }

    public string LabelColor
    {
        get { return labelColor; }
        set
        {
            labelColor = value;
            OnPropertyChanged();
        }
    }

    public string RuleColor1
    {
        get { return ruleColor1; }
        set
        {
            ruleColor1 = value;
            OnPropertyChanged();
        }
    }

    public string RuleColor2
    {
        get { return ruleColor2; }
        set
        {
            ruleColor2 = value;
            OnPropertyChanged();
        }
    }

    public string RuleColor3
    {
        get { return ruleColor3; }
        set
        {
            ruleColor3 = value;
            OnPropertyChanged();
        }
    }

    public string ResultColor
    {
        get { return resultColor; }
        set
        {
            resultColor = value;
            OnPropertyChanged();
        }
    }

    public string ResultText
    {
        get { return resultText; }
        set
        {
            resultText = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand CreateGroupCommand { get; set; }
    public ICommand GoBackCommand { get; set; }
}