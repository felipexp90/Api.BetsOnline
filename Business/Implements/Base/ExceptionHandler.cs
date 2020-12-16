using Business.Interfaces.Base;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;

namespace Business.Implements.Base
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger) => _logger = logger;

        public string GetMessage(Exception ex)
        {
            if (ex is DbUpdateException dbUpdateEx)
                if (dbUpdateEx.InnerException?.InnerException != null)
                    if (dbUpdateEx.InnerException.InnerException is SqlException sqlException)
                    {
                        _logger.LogError(
                            MessagesEnum.DbUpdateException,
                            sqlException,
                            $"[InnerException: {ex.InnerException}][StackTrace: {ex.StackTrace}]");

                        return MessagesEnum.DbError;
                    }

            if (ex is ApplicationException applicationEx)
            {
                _logger.LogInformation(MessagesEnum.OwnError, ex, ex.Message.ToString());
                return ex.Message;
            }

            if (ex != null)
            {
                return ex.Message;
            }

            _logger.LogCritical(MessagesEnum.FatalError, ex, $"[InnerException: {ex.InnerException}][StackTrace: {ex.StackTrace}]");
            return MessagesEnum.AdminError;
        }
    }
}