using System.Collections.Generic;
using Slack.Webhooks;

namespace api.Infrastructure.Interfaces
{
    public interface ISlackWebHookClient
    {
        void SendMessage(String headerTitle, String color, String emoji, String username, List<SlackField> messages);
    }
}
