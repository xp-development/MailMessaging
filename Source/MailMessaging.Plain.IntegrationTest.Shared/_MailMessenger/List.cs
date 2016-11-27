using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Core;
using MailMessaging.Plain.Core.Commands;
using Xunit;

namespace MailMessaging.Plain.IntegrationTest._MailMessenger
{
    public class List : TestBase
    {
        [Theory]
        [InlineData("", "*", true, true, true, true, true, true, true, true)]
        [InlineData("INBOX", "*", false, false, false, true, true, true, false, false)]
        [InlineData("INBOX/Subfolder", "", false, false, false, false, true, false, false, false)]
        [InlineData("INBOX/Subfolder", "/Subsubfolder", false, false, false, false, false, true, false, false)]
        [InlineData("INBOX", "/Subfolder/Subsubfolder", false, false, false, false, false, true, false, false)]
        [InlineData("INBOX", "/Subfold*", false, false, false, false, true, true, false, false)]
        [InlineData("INBOX", "Subfolder", false, false, false, false, false, false, false, false)]
        [InlineData("", "INBOX/Subfolder", false, false, false, false, true, false, false, false)]
        [InlineData("", "INBOX/Subfolder*", false, false, false, false, true, true, false, false)]
        [InlineData("", "INBOX/Subfolder/Subsubfolder", false, false, false, false, false, true, false, false)]
        [InlineData("Sent", "", false, false, true, false, false, false, false, false)]
        [InlineData("Trash", "", false, true, false, false, false, false, false, false)]
        [InlineData("", "Drafts", true, false, false, false, false, false, false, false)]
        public void ShouldReceiveAllFolders(string referenceName, string mailboxName, bool checkDraftsFolder, bool checkTrashFolder, bool checkSentFolder, bool checkInboxFolder,
            bool checkSubfolderFolder, bool checkSubsubfolderFolder, bool checkOutboxFolder, bool checkJunkFolder)
        {
            var messenger = GetLoggedInMailMessenger();

            var response = messenger.SendAsync(new ListCommand(TagService, referenceName, mailboxName)).Result;

            response.Result.Should().Be(ResponseResult.OK);
            var folders = response.Folders.ToArray();

            CheckDraftsFolder(checkDraftsFolder, folders);
            CheckTrashFolder(checkTrashFolder, folders);
            CheckSentFolder(checkSentFolder, folders);
            CheckInboxFolder(checkInboxFolder, folders);
            CheckSubfolderFolder(checkSubfolderFolder, folders);
            CheckSubsubfolderFolder(checkSubsubfolderFolder, folders);
            CheckOutboxFolder(checkOutboxFolder, folders);
            CheckJunkFolder(checkJunkFolder, folders);
        }

        [Fact]
        public void ShouldReceiveFolderWithSpaces()
        {
            var messenger = GetLoggedInMailMessenger();

            const string folderName = "Folder with spaces";
            var response = messenger.SendAsync(new ListCommand(TagService, folderName, "*")).Result;

            response.Result.Should().Be(ResponseResult.OK);
            var folders = response.Folders.ToArray();

            var listFolder = folders.SingleOrDefault(item => item.Name == folderName);
            listFolder.Should().NotBeNull($"No '{folderName}' folder found!");
            listFolder.Name.Should().Be(folderName);
            listFolder.HierarchyDelimiter.Should().Be("/");
            listFolder.Attributes.Count().Should().Be(1);
            listFolder.Attributes.ElementAt(0).Should().Be(@"\NoInferiors");
        }

        [Fact]
        public void ShouldReceiveNoResponseIfLoginDataAreInvalid()
        {
            var account = new Account(TestServer, TestPort, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(tcpClient);
            messenger.ConnectAsync(account).Result.Should().Be(ConnectResult.Connected);

            var response = messenger.SendAsync(new ListCommand(TagService, "", "*")).Result;

            response.Result.Should().Be(ResponseResult.NO);
            response.Message.Should().Be("A0001 NO please login first\r\n");
        }

        private static void CheckDraftsFolder(bool checkDraftFolder, IEnumerable<ListCommand.ListFolder> listFolders)
        {
            const string folderName = "Drafts";

            var listFolder = listFolders.SingleOrDefault(item => item.Name == folderName);
            if (checkDraftFolder)
            {
                listFolder.Should().NotBeNull($"No '{folderName}' folder found!");
                listFolder.Name.Should().Be(folderName);
                listFolder.HierarchyDelimiter.Should().Be("/");
                listFolder.Attributes.Count().Should().Be(2);
                listFolder.Attributes.ElementAt(0).Should().Be(@"\Drafts");
                listFolder.Attributes.ElementAt(1).Should().Be(@"\NoInferiors");
            }
            else
                listFolder.Should().BeNull($"'{folderName}' folder was found, but not expected!");
        }

        private static void CheckTrashFolder(bool checkTrashFolder, IEnumerable<ListCommand.ListFolder> listFolders)
        {
            const string folderName = "Trash";
            var listFolder = listFolders.SingleOrDefault(item => item.Name == folderName);
            if (checkTrashFolder)
            {
                listFolder.Should().NotBeNull($"No '{folderName}' folder found!");
                listFolder.Name.Should().Be(folderName);
                listFolder.HierarchyDelimiter.Should().Be("/");
                listFolder.Attributes.Count().Should().Be(2);
                listFolder.Attributes.ElementAt(0).Should().Be(@"\Trash");
                listFolder.Attributes.ElementAt(1).Should().Be(@"\HasNoChildren");
            }
            else
                listFolder.Should().BeNull($"'{folderName}' folder was found, but not expected!");
        }

        private static void CheckSentFolder(bool checkSentFolder, IEnumerable<ListCommand.ListFolder> listFolders)
        {
            const string folderName = "Sent";
            var listFolder = listFolders.SingleOrDefault(item => item.Name == folderName);
            if (checkSentFolder)
            {
                listFolder.Should().NotBeNull($"No '{folderName}' folder found!");
                listFolder.Name.Should().Be(folderName);
                listFolder.HierarchyDelimiter.Should().Be("/");
                listFolder.Attributes.Count().Should().Be(2);
                listFolder.Attributes.ElementAt(0).Should().Be(@"\Sent");
                listFolder.Attributes.ElementAt(1).Should().Be(@"\NoInferiors");
            }
            else
                listFolder.Should().BeNull($"'{folderName}' folder was found, but not expected!");
        }

        private static void CheckInboxFolder(bool checkInboxFolder, IEnumerable<ListCommand.ListFolder> listFolders)
        {
            const string folderName = "INBOX";
            var listFolder = listFolders.SingleOrDefault(item => item.Name == folderName);
            if (checkInboxFolder)
            {
                listFolder.Should().NotBeNull($"No '{folderName}' folder found!");
                listFolder.Name.Should().Be(folderName);
                listFolder.HierarchyDelimiter.Should().Be("/");
                listFolder.Attributes.Count().Should().Be(1);
                listFolder.Attributes.ElementAt(0).Should().Be(@"\HasChildren");
            }
            else
                listFolder.Should().BeNull($"'{folderName}' folder was found, but not expected!");
        }

        private static void CheckSubfolderFolder(bool checkSubfolderFolder, IEnumerable<ListCommand.ListFolder> listFolders)
        {
            const string folderName = "INBOX/Subfolder";
            var listFolder = listFolders.SingleOrDefault(item => item.Name == folderName);
            if (checkSubfolderFolder)
            {
                listFolder.Should().NotBeNull($"No '{folderName}' folder found!");
                listFolder.Name.Should().Be(folderName);
                listFolder.HierarchyDelimiter.Should().Be("/");
                listFolder.Attributes.Count().Should().Be(1);
                listFolder.Attributes.ElementAt(0).Should().Be(@"\HasChildren");
            }
            else
                listFolder.Should().BeNull($"'{folderName}' folder was found, but not expected!");
        }

        private static void CheckSubsubfolderFolder(bool checkSubsubfolderFolder, IEnumerable<ListCommand.ListFolder> listFolders)
        {
            const string folderName = "INBOX/Subfolder/Subsubfolder";
            var listFolder = listFolders.SingleOrDefault(item => item.Name == folderName);
            if (checkSubsubfolderFolder)
            {
                listFolder.Should().NotBeNull($"No '{folderName}' folder found!");
                listFolder.Name.Should().Be(folderName);
                listFolder.HierarchyDelimiter.Should().Be("/");
                listFolder.Attributes.Count().Should().Be(1);
                listFolder.Attributes.ElementAt(0).Should().Be(@"\NoInferiors");
            }
            else
                listFolder.Should().BeNull($"'{folderName}' folder was found, but not expected!");
        }

        private static void CheckOutboxFolder(bool checkOutboxFolder, IEnumerable<ListCommand.ListFolder> listFolders)
        {
            const string folderName = "OUTBOX";
            var listFolder = listFolders.SingleOrDefault(item => item.Name == folderName);
            if (checkOutboxFolder)
            {
                listFolder.Should().NotBeNull($"No '{folderName}' folder found!");
                listFolder.Name.Should().Be(folderName);
                listFolder.HierarchyDelimiter.Should().Be("/");
                listFolder.Attributes.Count().Should().Be(1);
                listFolder.Attributes.ElementAt(0).Should().Be(@"\NoInferiors");
            }
            else
                listFolder.Should().BeNull($"'{folderName}' folder was found, but not expected!");
        }

        private static void CheckJunkFolder(bool checkJunkFolder, IEnumerable<ListCommand.ListFolder> listFolders)
        {
            const string folderName = "Junk";
            var listFolder = listFolders.SingleOrDefault(item => item.Name == folderName);
            if (checkJunkFolder)
            {
                listFolder.Should().NotBeNull($"No '{folderName}' folder found!");
                listFolder.Name.Should().Be(folderName);
                listFolder.HierarchyDelimiter.Should().Be("/");
                listFolder.Attributes.Count().Should().Be(2);
                listFolder.Attributes.ElementAt(0).Should().Be(@"\Junk");
                listFolder.Attributes.ElementAt(1).Should().Be(@"\NoInferiors");
            }
            else
                listFolder.Should().BeNull($"'{folderName}' folder was found, but not expected!");
        }
    }
}