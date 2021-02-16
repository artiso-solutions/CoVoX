using System;

namespace CustomCommands.Messages
{
    internal class ActivityEvent
    {
        public string ChannelId { get; set; }

        public Conversation Conversation { get; set; }
        
        public From From { get; set; }
        
        public string Id { get; set; }
        
        public string Locale { get; set; }
        
        public string Name { get; set; }
        
        public string InputHint { get; set; }
        
        public Recipient Recipient { get; set; }
        
        public string ReplyToId { get; set; }
        
        public string ServiceUrl { get; set; }
        
        public DateTime TimeStamp { get; set; }
        
        public string Type { get; set; }
    }
    
    internal class Conversation
    {
        public string Id { get; set; }

        public bool IsGroup { get; set; }
    }

    internal class From
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    internal class Recipient
    {
        public string Id { get; set; }
    }
}
