using Newtonsoft.Json;
using System;
using System.Text;

namespace IoT.Shared.Messaging
{
    public class MessageEnvelopeDto
    {
        public string ClientId { get; set; }
        public string MessageAssemTypeName { get; set; }
        public byte[] MessageData { get; set; }
    }

    public static class MessageEnvelopeUtls
    {
        public static object GetMessageObject(this MessageEnvelopeDto messageEnv)
        {
            if (messageEnv?.MessageData == null)
                return null;

            var objectText = Encoding.UTF8.GetString(messageEnv.MessageData);
            var objectType = Type.GetType(messageEnv.MessageAssemTypeName);

            var messageObject = JsonConvert.DeserializeObject(objectText, objectType);

            return messageObject;
        }
    }
}
