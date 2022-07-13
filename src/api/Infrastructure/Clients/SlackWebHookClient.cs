using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Slack.Webhooks;
using api.Infrastructure.Interfaces;

namespace api.Infrastructure.Clients
{
    public class SlackWebHookClient : ISlackWebHookClient
    {
        private readonly String channel;

        private readonly SlackClient client;

        public SlackWebHookClient(IConfiguration configuration)
        {
           this.channel = configuration.GetValue<String>("Slack:Channel");
           this.client = new SlackClient(configuration.GetValue<String>("Slack:WebHookUri"));
        }

        public void SendMessage(String headerTitle, String color, String emoji, String username, List<SlackField> messages)
        {
            var slackMessage = new SlackMessage
            {
                Channel = channel,
                IconEmoji = emoji,
                Username = username
            };

            var slackAttachment = new SlackAttachment
            {
                Color = color,
                Title = headerTitle,
                Fields = messages
            };

            slackMessage.Attachments = new List<SlackAttachment> { slackAttachment };
            client.Post(slackMessage);
        }
    }
}
