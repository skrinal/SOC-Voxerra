using System.Collections;


namespace Voxerra.Pages;
public partial class ChatPage : ContentPage
{
	//public ChatPageViewModel _viewModel { get; set; }
	public ChatPage(ChatPageViewModel viewModel)
	{
		InitializeComponent();

		//_viewModel = viewModel;
		
		this.BindingContext = viewModel;
		viewModel.MessagesCollectionView = MessagesCollectionView;
		
		MessagingCenter.Subscribe<ChatPageViewModel>(this, "ScrollToLastMessage", (sender) =>
		{
			if (MessagesCollectionView.ItemsSource is IEnumerable items)
			{
				object lastItem = null;
				foreach (var item in items)
				{
					lastItem = item; // Keep updating until the last item
				}

				if (lastItem != null)
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						MessagesCollectionView.ScrollTo(lastItem, position: ScrollToPosition.End, animate: true);
					});
				}
			}
		});
	}
	
	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		MessagingCenter.Unsubscribe<ChatPageViewModel>(this, "ScrollToLastMessage");
	}
	

	// protected override async void OnAppearing()
	// {
	// 	base.OnAppearing();
	// 	if (_viewModel != null)
	// 	{
	// 		await _viewModel.GetMessages(); // 🔄 Load messages before page appears
	// 	}
	//
	// 	
	//
	// 	if (_viewModel.Messages.Any())
	// 	{
	// 		MainThread.BeginInvokeOnMainThread(() =>
	// 		{
	// 			MessagesCollectionView?.ScrollTo(_viewModel.Messages.Last(), position: ScrollToPosition.End, animate: false);
	// 		});
	// 	}
	// 	
	// }
	
	
	private async void OnCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
	{
		// Check if we are at the absolute top
		if (e.VerticalOffset <= 0)
		{
			// Load older messages when the user reaches the top
			await (BindingContext as ChatPageViewModel)?.GetOlderMessages();
		}
	}

    
}