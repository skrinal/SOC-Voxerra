using System.Collections;
using CommunityToolkit.Mvvm.Messaging;
using Voxerra.Services.Message;


namespace Voxerra.Pages;
public partial class ChatPage : ContentPage
{
	//public ChatPageViewModel _viewModel { get; set; }
	public ChatPage(ChatPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
		
		WeakReferenceMessenger.Default.Register<ScrollToBottomMessage>(this, (r, m) =>
		{
			if (m.Value)
			{
				ScrollToBottom();
			}
		});
	}
	
	private void ScrollToBottom()
	{
		if (MessagesCollectionView?.ItemsSource is IEnumerable items)
		{
			object lastItem = null;
			foreach (var item in items)
			{
				lastItem = item;
			}

			if (lastItem != null)
			{
				MainThread.BeginInvokeOnMainThread(() =>
				{
					MessagesCollectionView.ScrollTo(lastItem, position: ScrollToPosition.End, animate: false);
				});
			}
		}
	}
    
}