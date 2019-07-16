using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Email.EntityFramework;
using Email.Svc.Services.Settings;
using Email.EntityFramework.Models;

namespace Email.Svc.Services.Suppliers {

    public class SupplierService : ISupplierService {
        private readonly EmailContext _emailContext;
        private readonly ISettingsService _settingsService;

        public SupplierService(EmailContext emailContext, ISettingsService settingsService) {
            _emailContext = emailContext;
            _settingsService = settingsService;
        }

        public async Task<Supplier> GetSupplier(string address) {
            try {
                address = address.ToLower();
                var mailHost = address.Split('@')[1];
                return _emailContext.Suppliers.FirstOrDefault(t => t.Emails.Contains($"|{address}|")
                                                     || t.Emails.Contains($"|*@{mailHost}|"))
                       ?? await NewSupplier(address, mailHost);
            } catch (Exception ex) {

                return null;
            }
        }

        public async Task<PriceList> GetPriceList(string fileName) {
            try {
                return _emailContext.PriceLists.FirstOrDefault(t => t.PriceListName.Contains(fileName))
                       ?? await NewPriceList(fileName);
            } catch (Exception ex) {

                return null;
            }
        }

        private async Task<PriceList> NewPriceList(string fileName) {
            var fileInfo = new FileInfo(fileName);
            string attachmentName = GetAttachmentName(fileInfo);

            var newPriceList = new PriceList {

            };
            return newPriceList;
        }

        private async Task<Supplier> NewSupplier(string address, string mailHost) {
            var publicEmailHosts = await _settingsService.GetPublicEmailHosts();

            var emails = $"|{address}|";
            var supplierName = address;
            var supplierEmailHost = _emailContext.Suppliers.FirstOrDefault(t => t.Emails.Contains($"@{mailHost}|"));

            if (!publicEmailHosts.Contains(mailHost) && supplierEmailHost == null) {
                emails += $"*@{mailHost}|";
                supplierName = mailHost;
            }

            var newSupplier = new Supplier {
                Contacts = address,
                Name = supplierName,
                FolderName = NormalizeName(supplierName),
                Emails = emails,
                IsNew = true
            };

            _emailContext.Suppliers.Add(newSupplier);
            _emailContext.SaveChanges();

            return _emailContext.Suppliers.FirstOrDefault(t => t.Name == supplierName && t.IsNew);
        }

        private string NormalizeName(string folderName) {
            return Path.GetInvalidPathChars().Aggregate(folderName, (current, c) => current.Replace(char.ToString(c), ""));
        }

        private static string GetAttachmentName(FileInfo file)
        {
            string attachmentName = file.Name.Replace(file.Extension, "").ToLower();
            var r = new Regex(@"(\d{4}\.\d{2}\.\d{2})|(\d{4},\d{2},\d{2})|(\d{4}_\d{2}_\d{2})|(\d{4}\-\d{2}\-\d{2})|(\d{4}\s\d{2}\s\d{2})");
            var matches = new List<Match>();
            foreach (Match match in r.Matches(attachmentName))
            {
                var capture = match.Captures[0].Value;
                var tempDate = new DateTime();
                var capture1 = capture.Replace("_", ".").Replace(",", ".").Replace(" ", ".");
                if (DateTime.TryParse(capture1, out tempDate) && tempDate.Year == DateTime.Now.Year)
                    attachmentName = attachmentName.Replace(capture, "$Date$");
            }

            r = new Regex(@"(\d{1,2}\.\d{1,2}\.\d{2,4})|(\d{1,2},\d{1,2},\d{2,4})|(\d{1,2}_\d{1,2}_\d{2,4})|(\d{1,2}\-\d{1,2}\-\d{2,4})|(\d{1,2}\s\d{1,2}\s\d{2,4})");
            matches = new List<Match>();
            foreach (Match match in r.Matches(attachmentName))
            {
                var capture = match.Captures[0].Value;
                var tempDate = new DateTime();
                var capture1 = capture.Replace("_", ".").Replace(",", ".").Replace(" ", ".");
                if (DateTime.TryParse(capture1, out tempDate) && tempDate.Year == DateTime.Now.Year)
                    attachmentName = attachmentName.Replace(capture, "$Date$");
            }
            r = new Regex(@"([^\d]|\A)\d{6}([^\d]|\Z)");
            matches = new List<Match>();
            foreach (Match match in r.Matches(attachmentName))
            {
                var capture = match.Captures[0].Value;
                var r1 = new Regex(@"\d{6}");
                var matches1 = new List<Match>();
                foreach (Match match1 in r1.Matches(capture))
                {
                    var capture1 = match1.Captures[0].Value;
                    var capture2 = capture1.Insert(4, ".");
                    capture2 = capture2.Insert(2, ".");
                    var tempDate = new DateTime();
                    if (DateTime.TryParse(capture2, out tempDate) && tempDate.Year == DateTime.Now.Year)
                        attachmentName = attachmentName.Replace(capture1, "$Date$");
                }
            }
            r = new Regex(@"([^\d]|\A)\d{8}([^\d]|\Z)");
            matches = new List<Match>();
            foreach (Match match in r.Matches(attachmentName))
            {
                var capture = match.Captures[0].Value;
                var r1 = new Regex(@"\d{8}");
                var matches1 = new List<Match>();
                foreach (Match match1 in r1.Matches(capture))
                {
                    var capture1 = match1.Captures[0].Value;
                    var capture2 = capture1.Insert(4, ".");
                    capture2 = capture2.Insert(2, ".");
                    var tempDate = new DateTime();
                    if (DateTime.TryParse(capture2, out tempDate) && tempDate.Year == DateTime.Now.Year)
                    {
                        attachmentName = attachmentName.Replace(capture1, "$Date$");
                        continue;
                    }

                    capture1 = match1.Captures[0].Value;
                    capture2 = capture1.Insert(6, ".");
                    capture2 = capture2.Insert(4, ".");
                    tempDate = new DateTime();
                    if (DateTime.TryParse(capture2, out tempDate) && tempDate.Year == DateTime.Now.Year)
                        attachmentName = attachmentName.Replace(capture1, "$Date$");
                }
            }
            r = new Regex(@"(\d{2}\.\d{2})|(\d{2}_\d{2})|(\d{2}\-\d{2})");
            matches = new List<Match>();
            foreach (Match match in r.Matches(attachmentName))
            {
                var capture = match.Captures[0].Value;
                var tempDate = new DateTime();
                var capture1 = capture.Replace("-", ".").Replace("_", ".") + ".2011";
                if (DateTime.TryParse(capture1, out tempDate) && tempDate.Year == DateTime.Now.Year)
                    attachmentName = attachmentName.Replace(capture, "$Date$");
            }
            r = new Regex("\\d{1,2}.*(январь|февраль|март|апрель|май|июнь|июль|август|сентябрь|октябрь|ноябрь|декабрь|"
                          + "января|февраля|марта|апреля|мая|июня|июля|августа|сентября|октября|ноября|декабря|"
                          + "січень|лютий|березень|квітень|травень|червень|липень|серпень|вересень|жовтень|листопад|грудень|"
                          + "січня|лютого|березня|квітеня|травня|червня|липня|серпня|вересня|жовтня|листопада|грудня).*\\d{0,4}");
            matches = new List<Match>();
            foreach (Match match in r.Matches(attachmentName))
            {
                var capture = match.Captures[0].Value;
                attachmentName = attachmentName.Replace(capture, "$Date$");
            }
            return attachmentName;
        }
    }

}