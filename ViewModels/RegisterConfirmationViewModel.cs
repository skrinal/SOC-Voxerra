
namespace Voxerra.ViewModels
{
    public class RegisterConfirmationViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ServiceProvider _serviceProvider;
        private RegisterPageViewModel _registerPageViewModel;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RegisterConfirmationViewModel(ServiceProvider serviceProvider, RegisterPageViewModel viewModel)
        {
            IsProcessing = false;
            ConfirmRegistrationCommand = new Command(() =>
            {
                if (IsProcessing) return;

                IsProcessing = true;
                RegisterConfirmation().GetAwaiter().OnCompleted(() =>
                {
                    isProcessing = false;
                });

            });
            GoBackCommand = new Command(OnGoBack);

            _serviceProvider = serviceProvider;
            _registerPageViewModel = viewModel;
        }

        public async Task RegisterConfirmation()
        {
            try
            {
                var request = new RegistrationConfirmationRequest
                {
                    Email = Email,
                    Code = RegistrationCode
                };
                var response = await _serviceProvider.CallWebApi<RegistrationConfirmationRequest, BaseResponse>
                    ("/Registration/ConfirmRegistration", HttpMethod.Post, request);
                
                if (response.StatusCode == 200)
                {
                    ResetEntry();
                    _registerPageViewModel.ClearEnteries();
                    await Shell.Current.GoToAsync($"//LoginPage");
                }
                else
                {
                    await AppShell.Current.DisplayAlert("Voxerra", response.StatusMessage, "OK");
                }

            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        }
       
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            Email = HttpUtility.UrlDecode(query["email"].ToString());

        }
        private async void OnGoBack()
        {
            ResetEntry();
            await Shell.Current.GoToAsync("RegisterPage");
        }

        private void ResetEntry()
        {
            Entry1 = "";
            Entry2 = "";
            Entry3 = "";
            Entry4 = "";
            Entry5 = "";
        }
        
        private int registrationCode;
        private bool isProcessing;



        private string entry1;
        private string entry2;
        private string entry3;
        private string entry4;
        private string entry5;
        private string email;

        public string Entry1
        {
            get => entry1;
            set
            {
                entry1 = value;
                OnPropertyChanged();
            }
        }

        public string Entry2
        {
            get => entry2;
            set
            {
                entry2 = value;
                OnPropertyChanged();
            }
        }

        public string Entry3
        {
            get => entry3;
            set
            {
                entry3 = value;
                OnPropertyChanged();
            }
        }

        public string Entry4
        {
            get => entry4;
            set
            {
                entry4 = value;
                OnPropertyChanged();
            }
        }

        public string Entry5
        {
            get => entry5;
            set
            {
                entry5 = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        public int RegistrationCode
        {
            get
            {
                int.TryParse($"{Entry1}{Entry2}{Entry3}{Entry4}{Entry5}", out int code);
                return code;
            }
        }
        
        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }
        public ICommand ConfirmRegistrationCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
    }
}
