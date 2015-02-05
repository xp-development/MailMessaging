using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Contracts.Services;

namespace MailMessaging.Plain.Core.Commands
{
    public class ListCommand : CommandBase<ListCommand, ListCommand.ListResponse>
    {
        public override string Request
        {
            get { return PrepareCommand(string.Format("LIST \"{0}\" \"{1}\"", _referenceName, _mailboxName)); }
        }

        public ListCommand(ITagService tagService, string referenceName, string mailboxName)
            : base(tagService)
        {
            _referenceName = referenceName;
            _mailboxName = mailboxName;
        }

        public override ListResponse ParseResponse(string responseMessage)
        {
            return ListResponse.Create(Tag, responseMessage);
        }

        public static ListCommand CreateFullListCommand(ITagService tagService)
        {
            return new ListCommand(tagService, "", "*");
        }

        private readonly string _mailboxName;
        private readonly string _referenceName;

        public class ListResponse : ResponseBase
        {
            public IEnumerable<ListFolder> Folders { get; private set; }

            private ListResponse(IEnumerable<ListFolder> folders)
            {
                Folders = folders;
            }

            public static ListResponse Create(string expectedTag, string responseMessage)
            {
                var lines = responseMessage.Split(new []{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

                var matches = Regex.Matches(lines.Last(), @"^" + expectedTag + "\\s(OK|NO|BAD)\\s(.*)$");
                if (matches.Count == 0)
                    throw new InvalidOperationException();

                var listFolders = new List<ListFolder>();

                for (var i = 1; i < lines.Length -1; ++i)
                {
                    var match = Regex.Match(lines[i], @"^\*\sLIST\s+\((.*?)\)\s+\""(.*?)\""\s+(.*?)$");

                    if (!match.Success)
                        throw new NotSupportedException("Invalid response for LIST command: " + Environment.NewLine + responseMessage);

                    var attributes = match.Groups[1].Value;

                    listFolders.Add(new ListFolder(attributes.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries), match.Groups[2].Value, match.Groups[3].Value));
                }

                var response = new ListResponse(listFolders)
                {
                    Result = (ResponseResult)Enum.Parse(typeof(ResponseResult), matches[0].Groups[1].Value),
                    Message = responseMessage
                };

                return response;
            }
        }

        public class ListFolder
        {
            public IEnumerable<string> Attributes { get; private set; }
            public string HierarchyDelimiter { get; private set; }
            public string Name { get; private set; }

            public ListFolder(IEnumerable<string> attributes, string hierarchyDelimiter, string name)
            {
                Attributes = attributes;
                HierarchyDelimiter = hierarchyDelimiter;
                Name = name;
            }
        }
    }
}