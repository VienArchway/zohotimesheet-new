using System.Collections.Generic;
using Slack.Webhooks;

namespace api.Application.Interfaces
{
  public interface ISlackService
    {
        void SendMessage(string headerTitle, string color, string emoji, string username, List<SlackField> messages);
    }
}
