using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Primitives;
using System;

namespace Server.Shared
{
    public class RequiredCommandAttribute : Attribute, IActionConstraint
    {
        public RequiredCommandAttribute(string header, string value)
        {
            Header = header;
            Value = value;
        }

        public int Order => 0;

        public string Header { get; }
        public string Value { get; }

        public bool Accept(ActionConstraintContext context)
        {
            if (context.RouteContext.HttpContext.Request.Headers.TryGetValue(this.Header, out StringValues command))
            {
                return command[0] == this.Value;
            }

            return false;
        }
    }
}
