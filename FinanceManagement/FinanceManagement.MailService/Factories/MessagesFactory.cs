using FinanceManagement.Core.Exceptions;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.MessageService.Constants;
using FinanceManagement.MessageService.TemplateModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace FinanceManagement.MessageService.Factories
{
    public static class MessagesFactory
    {
        public static MessageDto EmailResetPasswordMessage(string email, string userLogin, string newPasswordLink)
        {
			var dto = new MessageDto { Receiver = email, MessageType = MessageTypeEnum.Email };

			var templatePath = Path.Combine(AppContext.BaseDirectory, EmailTempatesConstants.Base, EmailTempatesConstants.EmailReset);

			if (!File.Exists(templatePath))
			{
				throw new BaseException(string.Format("There is no template on path {0}", templatePath));
			}

			EmailResetModel emailResetModel = new EmailResetModel();

			string template = File.ReadAllText(templatePath);

			dto.Title = emailResetModel.NewPasswordMailTitle;

			var dict = new Dictionary<string, string>();

			foreach (var filed in typeof(EmailResetModel).GetProperties())
			{
				string fieldValue = filed.GetValue(emailResetModel).ToString();
				string value = fieldValue;
				dict.Add(fieldValue, value);
			}

			dict[emailResetModel.UserLogin] = userLogin;
			dict[emailResetModel.NewPasswordLink] = newPasswordLink;

			string content = template;

			foreach (string key in dict.Keys)
			{
				string textToReplace = string.Format("{{{{ {0} }}}}", key);
				content = content.Replace(textToReplace, dict[key]);
			}

			dto.Content = content;

			return dto;
		}
    }
}
