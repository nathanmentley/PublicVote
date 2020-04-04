/*
    PublicVote - A platform for conducting small secure public elections
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
using System;
using System.Threading.Tasks;
using PublicVote.Common;
using PublicVote.Common.Repositories;

namespace PublicVote.Server.DAL
{
    public class VoteRepository: IVoteRepository
    {
        private readonly IBlockchainClient _client;

        public VoteRepository(IBlockchainClient client)
        {
            _client = client ??
                throw new ArgumentNullException(nameof(client));
        }

        public async Task<Vote> Create(Vote vote)
        {
            var id = await _client.AppendToChain(vote);

            return new Vote(id, vote);
        }

        public async Task<Vote> Fetch(string id) =>
            new Vote(id, await _client.FetchFromChain(id));
    }
}