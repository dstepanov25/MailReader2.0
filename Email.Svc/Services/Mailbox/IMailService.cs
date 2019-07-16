using System.Collections.Generic;
using Email.Svc.Services.Mailbox.Dto;
using Email.Svc.Services.Settings.Dto;
using MimeKit;

namespace Email.Svc.Services.Mailbox {

    public interface IMailService {
        IEnumerable<MailDto> ReadMessages(MailAccountDto mailAccountInput);

        IEnumerable<MimeEntity> GetAttachmentsFromMail(MimeMessage message);

        void SaveAttachmentsToFolder(MimeEntity attachment, string path);
    }

}