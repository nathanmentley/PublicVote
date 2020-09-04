/*
    RemoteCongress - A platform for conducting small secure public elections
    Copyright (C) 2020  Nathan Mentley

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published
    by the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using RemoteCongress.Common.Repositories.Queries;
using RemoteCongress.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemoteCongress.Server.Web.ModelBinders
{
    public class QueryModelBinder: IModelBinder
    {
        private readonly ICodec<IQuery> _codec;

        public QueryModelBinder(ICodec<IQuery> codec)
        {
            _codec = codec ??
                throw new ArgumentNullException(nameof(codec));
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            List<IQuery> queries = new List<IQuery>();

            if(bindingContext.HttpContext.Request.Query.TryGetValue("query", out StringValues values))
            {
                foreach(string value in values)
                {
                    IQuery query = await _codec.DecodeFromString(_codec.GetPreferredMediaType(), value);

                    if (!(query is null))
                        queries.Add(query);
                }
            }
            
            bindingContext.Result = ModelBindingResult.Success(new List<IQuery>());
        }
    }

}