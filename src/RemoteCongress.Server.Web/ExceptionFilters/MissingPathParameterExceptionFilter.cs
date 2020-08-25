using Microsoft.Extensions.Logging;
using RemoteCongress.Server.Web.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RemoteCongress.Server.Web.ExceptionFilters
{
    [ExcludeFromCodeCoverage]
    public sealed class MissingPathParameterExceptionFilter: BaseExceptionFilter
    {
        protected override int StatusCode => 400;
    
        public MissingPathParameterExceptionFilter(ILogger logger): base(logger) {}

        protected override bool CanHandle(Exception exception) =>
            exception is MissingPathParameterException;
    }
}