using System;

namespace Src
{
    public class EventSourcePublishFailureException : Exception
    {
        public EventSourcePublishFailureException(string statusCode, string topicArn, Exception innerException)
            : base($"Publish to SNS Topic {topicArn} with response code {statusCode}", innerException)
        {
        }

        public EventSourcePublishFailureException(string statusCode, string topicArn)
            : base($"Publish to SNS Topic {topicArn} with response code {statusCode}")
        {
        }
    }
}
