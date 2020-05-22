using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Core.Services {
    public interface INotifyService {
        public Task<Message> SendTextMessage(string message);
    }

    public class NotifyService: INotifyService {
        private readonly string _token;
        private readonly string _chatId;
        private readonly TelegramBotClient _client;

        public NotifyService(string token, string chatId) {
            _token = token;
            _chatId = chatId;
            _client = new TelegramBotClient(_token);
            //_client.OnMessage += Client_OnMessage;
            //_client.StartReceiving();
        }

        //private async void Client_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e) {
        //    if(e.Message.Text != null) {
        //        await _client.SendTextMessageAsync(
        //            chatId: e.Message.Chat,
        //            text: "You said:\n" + e.Message.Text
        //        );
        //    }
        //}

        public async Task<Message> SendTextMessage(string message) {
            return await _client.SendTextMessageAsync(
                chatId: new ChatId(_chatId),
                text: message,
                parseMode: ParseMode.Markdown,
                disableNotification: true,
                replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(
                    "Unpaid invoices",
                    "http://89.108.99.44/Invoice?unpaid=true"

                )));// "https://"
        }
    }
}
