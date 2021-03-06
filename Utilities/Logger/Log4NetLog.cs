﻿using System;
using log4net;

namespace Utilities.Logger
{
	public class Log4NetLog : ILog, ILog<Log4NetLog> 
	{
		private global::log4net.ILog _logger;

		public void InitializeFor(string loggerName)
		{
			_logger = LogManager.GetLogger(loggerName);
		}

		public void Debug(string message, params object[] formatting)
		{
			if (_logger.IsDebugEnabled) _logger.DebugFormat(DecorateMessageWithAuditInformation(message), formatting);
		}

		public void Debug(Func<string> message)
		{
			if (_logger.IsDebugEnabled) _logger.Debug(DecorateMessageWithAuditInformation(message.Invoke()));
		}

		public void Fatal(string message, Exception exception)
		{
			_logger.Fatal(DecorateMessageWithAuditInformation(message), exception);
		}

		public void Info(string message, params object[] formatting)
		{
			if (_logger.IsInfoEnabled) _logger.InfoFormat(DecorateMessageWithAuditInformation(message), formatting);
		}

		public void Info(Func<string> message)
		{
			if (_logger.IsInfoEnabled) _logger.Info(DecorateMessageWithAuditInformation(message.Invoke()));
		}

		public void Warn(string message, params object[] formatting)
		{
			if (_logger.IsWarnEnabled) _logger.WarnFormat(DecorateMessageWithAuditInformation(message), formatting);
		}

		public void Warn(Func<string> message)
		{
			if (_logger.IsWarnEnabled) _logger.Warn(DecorateMessageWithAuditInformation(message.Invoke()));
		}

		public void Error(string message, params object[] formatting)
		{
			// don't need to check for enabled at this level
			_logger.ErrorFormat(DecorateMessageWithAuditInformation(message), formatting);
		}

		public void Error(Func<string> message)
		{
			// don't need to check for enabled at this level
			_logger.Error(DecorateMessageWithAuditInformation(message.Invoke()));
		}

		public void Error(string message, Exception exception)
		{
			_logger.Error(DecorateMessageWithAuditInformation(message), exception);
		}

		public void Fatal(string message, params object[] formatting)
		{
			// don't need to check for enabled at this level
			_logger.FatalFormat(DecorateMessageWithAuditInformation(message), formatting);
		}

		public void Fatal(Func<string> message)
		{
			// don't need to check for enabled at this level
			_logger.Fatal(DecorateMessageWithAuditInformation(message.Invoke()));
		}

		public string DecorateMessageWithAuditInformation(string message)
		{
			//common method to add additional details to message

			return message;
		}
	}
}