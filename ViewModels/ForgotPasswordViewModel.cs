
namespace Voxerra.ViewModels
{
    public class ForgotPasswordViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        ForgotPasswordViewModel()
        {

        }

        bool isProcessing;
        string email;
        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        public ICommand RegisterCommand { get; set; }
        public ICommand ResetPassword { get; set; }
    }
}
