using Microsoft.Extensions.Logging;
using RemoteCongress.Server.Web.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RemoteCongress.Server.Web.ExceptionFilters
{
    [ExcludeFromCodeCoverage]
    public sealed class UnacceptableMediaTypeExceptionFilter: BaseExceptionFilter
    {
        protected override int StatusCode => 406;
    
        public UnacceptableMediaTypeExceptionFilter(ILogger logger): base(logger) {}

        protected override bool CanHandle(Exception exception) =>
            exception is UnacceptableMediaTypeException;
    }
}