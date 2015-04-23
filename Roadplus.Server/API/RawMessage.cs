using System;

namespace Roadplus.Server.API
{
    public class RawMessage
    {
        public string SourceAddress { get; private set; }
        public LinkType SourceType { get; private set; }
        public string Format { get; private set; }
        public string Content { get; private set; }

        public RawMessage(string source, 
                          LinkType sourcetype, 
                          string format, 
                          string content)
        {
            SourceAddress = source;
            SourceType = sourcetype;
            Format = format;
            Content = content;
        }
    }
}

