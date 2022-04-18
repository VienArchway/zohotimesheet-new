using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Slack.Webhooks;
using api.Infrastructure.Interfaces;

namespace api.Infrastructure.Clients
{
    public class SlackWebHookClient : ISlackWebHookClient
    {
        private readonly string channel;

        private readonly SlackClient client;

        public SlackWebHookClient(IConfiguration configuration)
        {
           this.channel = configuration.GetValue<string>("Slack:Channel");
           this.client = new SlackClient(configuration.GetValue<string>("Slack:WebHookUri"));
        }

        public void SendMessage(string headerTitle, string color, string emoji, string username, List<SlackField> messages)
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
