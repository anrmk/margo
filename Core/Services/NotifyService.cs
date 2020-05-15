using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Core.Services.Managers {
    public interface INotifyService {
        public Task<Message> SendTextMessage(string message);
    }

    public class NotifyService: INotifyService {
        private readonly string _token;
        private readonly string _chatId;

        public TelegramBotClient Client { get; set; }

        public NotifyService(string token, string chatId) {
            _token = token;
            _chatId = chatId;
            Client = new TelegramBotClient(_token);
        }

        public async Task<Message> SendTextMessage(string message) {
            return await Client.SendTextMessageAsync(new ChatId(_chatId), message, ParseMode.Markdown);
        }
    }
}
