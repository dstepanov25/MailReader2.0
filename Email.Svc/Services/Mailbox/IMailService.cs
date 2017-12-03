using System.Collections.Generic;
using Email.Svc.Models;
using Email.Svc.Services.Mailbox.Dto;
using MimeKit;

namespace Email.Svc.Services.Mailbox {

    public interface IMailService {
        IEnumerable<MailDto> ReadMessages(MailAccount mailAccountInput);

        IEnumerable<MimeEntity> GetAttachmentsFromMail(MimeMessage message);
    }

}