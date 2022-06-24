using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Operations.Transients;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.MessageService.Processors
{
    internal class EmailProccessor : BaseProccessor
    {
        private readonly IMessageOperation messageOperation;
        private readonly IMailSender mailSender;
        private bool isStoping;

        public EmailProccessor(IMessageOperation messageOperation, IMailSender mailSender)
        {
            this.messageOperation = messageOperation;
            this.mailSender = mailSender;
        }

        public override async Task Process()
        {
            while (!isStoping)
            {
                var createdEmails = await messageOperation.GetCreatedEmailMessages(100);

                if (!createdEmails.Any())
                {
                    await Task.Delay(5000);
                }

                foreach (var email in createdEmails)
                {
                    try
                    {
                        mailSender.SendHtmlEmail(email);
                        email.MessageStatus = MessageStatusEnum.Sent;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Exception in EmailProccessor.Process");
                        email.MessageStatus = MessageStatusEnum.Error;
                    }
                }

                await messageOperation.UpdateMessages(createdEmails);

                await Task.Delay(200);
            }
        }

        public override async Task Stop()
        {
            isStoping = true;

            await Task.CompletedTask;
        }
    }
}
