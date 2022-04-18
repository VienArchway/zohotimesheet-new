using System.Collections.Generic;
using Slack.Webhooks;

namespace api.Infrastructure.Interfaces
{
    public interface ISlackWebHookClient
    {
        void SendMessage(string headerTitle, string color, string emoji, string username, List<SlackField> messages);
    }
}
