using System.Collections.Generic;
using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using Slack.Webhooks;

namespace api.Application
{
  public class SlackService : ISlackService
    {
        private readonly ISlackWebHookClient client;

        public SlackService(ISlackWebHookClient client)
        {
            this.client = client;
        }

        public void SendMessage(String headerTitle, String color, String emoji, String username, List<SlackField> messages)
        {
            client.SendMessage(headerTitle, color, emoji, username, messages);
        }
  }
}
