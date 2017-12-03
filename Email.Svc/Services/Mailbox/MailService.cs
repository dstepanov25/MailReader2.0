using System.Collections.Generic;
using System.IO;
using System.Linq;
using Email.Svc.Models;
using Email.Svc.Services.Mailbox.Dto;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace Email.Svc.Services.Mailbox {

    public class MailService : IMailService {
        private readonly string[] _accessibleExtension = {".xls", ".zip", ".rar", ".xlsx"};

        public IEnumerable<MailDto> ReadMessages(MailAccount mailAccountInput) {
            var mailDtoList = new List<MailDto>();
            using (var client = GetImapConnection(mailAccountInput)) {
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite);

                var items = inbox.Fetch(0, -1, MessageSummaryItems.UniqueId  | MessageSummaryItems.Size | MessageSummaryItems.Flags);

                foreach (var item in items) {
                    var message = inbox.GetMessage(item.UniqueId);
                    var attachments = GetAttachmentsFromMail(message);

                    mailDtoList.Add(new MailDto {
                        From = string.Join("|", message.From),
                        Subject = message.Subject,
                        Attachments = attachments
                    });
                }

            }

            return mailDtoList;
        }

        public IEnumerable<MimeEntity> GetAttachmentsFromMail(MimeMessage message) {
            var attachments = new List<MimeEntity>();

            foreach (var attachment in message.Attachments) {
                var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                var x = new FileInfo(fileName);
                if (!_accessibleExtension.Contains(x.Extension.ToLower())) {
                    continue;
                }
                attachments.Add(attachment);
            }

            return attachments;
        }

        public void SaveAttachmentsToFolder(MimeEntity attachment, string path) {
            var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;

            using (var stream = File.Create(path + fileName)) {
                var messagePart = attachment as MessagePart;
                if (messagePart != null) {
                    var rfc822 = messagePart;

                    rfc822.Message.WriteTo(stream);
                } else {
                    var part = (MimePart) attachment;

                    part.ContentObject.DecodeTo(stream);
                }
            }
        }


        private ImapClient GetImapConnection(MailAccount mailAccountInput) {
            var client = new ImapClient {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };

            client.Connect(mailAccountInput.ConnectSsl, 993, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(mailAccountInput.UserName, mailAccountInput.Password);

            return client;
        }
    }

}