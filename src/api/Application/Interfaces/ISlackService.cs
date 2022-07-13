using System.Collections.Generic;
using Slack.Webhooks;

namespace api.Application.Interfaces
{
  public interface ISlackService
    {
        void SendMessage(String headerTitle, String color, String emoji, String username, List<SlackField> messages);
    }
}
