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
using RemoteCongress.Common;
using System.Collections.Generic;
using System.Threading;

namespace RemoteCongress.Server.DataSeeder
{
    /// <summary>
    /// An interface that defines a type that's able to provide data for seeding
    /// </summary>
    public interface IDataProvider
    {
        IAsyncEnumerable<Member> GetMembers(CancellationToken cancellationToken);

        IAsyncEnumerable<(Bill, string)> GetBills(CancellationToken cancellationToken);

        IAsyncEnumerable<(Vote vote, string memberPrivateKey, string memberPublicKey)> GetVotes(
            string id,
            VerifiedData<Bill> bill,
            CancellationToken cancellationToken
        );
    }
}