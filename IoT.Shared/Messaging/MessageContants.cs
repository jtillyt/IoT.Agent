using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Messaging
{
    public static class MessageConstants
    {
        public const string WebHubMethodName = "OnMessageSentToHub";
        public const string WebClientMethodName = "OnMessageSentToClient";

        public const string MobileLogin = nameof(MobileLogin);

        public const string OnNumericNodeValueReceived = nameof(OnNumericNodeValueReceived);
    }
}
