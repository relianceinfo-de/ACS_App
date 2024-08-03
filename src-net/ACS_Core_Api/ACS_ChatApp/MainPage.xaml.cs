using ACS_ChatApp.Services;
using System.Collections.ObjectModel;

namespace ACS_ChatApp
{
    public partial class MainPage : ContentPage
    {
        private readonly ChatService _chatService;
        private ObservableCollection<string> _messages;

        public MainPage()
        {
            InitializeComponent();
            string connectionString = "<YOUR_CONNECTION_STRING>";
            _chatService = new ChatService(connectionString);
            _chatService.MessageReceived += OnMessageReceived;
            _messages = new ObservableCollection<string>();
            MessagesListView.ItemsSource = _messages;
        }

        private async void OnCreateChatThreadClicked(object sender, EventArgs e)
        {
            await _chatService.CreateChatThreadAsync("My Chat Topic");
        }

        private async void OnSendMessageClicked(object sender, EventArgs e)
        {
            var message = MessageEntry.Text;
            if (!string.IsNullOrWhiteSpace(message))
            {
                await _chatService.SendMessageAsync(message);
                MessageEntry.Text = string.Empty;
            }
        }

        private void OnMessageReceived(object sender, string message)
        {
            // Update the UI with the new message
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _messages.Add(message);
            });
        }
    }


}
