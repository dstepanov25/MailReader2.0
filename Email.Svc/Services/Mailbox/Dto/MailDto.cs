using System.Collections.Generic;
using MimeKit;

namespace Email.Svc.Services.Mailbox.Dto {

    public class MailDto {
        public string From;

        public string Subject;

        public IEnumerable<MimeEntity> Attachments;
    }

}