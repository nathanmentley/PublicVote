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

using System;

namespace RemoteCongress.Common.Repositories.Queries
{
    /// <summary>
    /// </summary>
    public class BillIdQuery: IQuery
    {
        /// <summary>
        /// </summary>
        public string BillId { get; }

        /// <summary>
        /// </summary>
        /// <param name="billId">
        /// </param>
        public BillIdQuery(string billId)
        {
            if (string.IsNullOrWhiteSpace(billId))
                throw new ArgumentNullException(nameof(billId));

            BillId = billId;
        }
    }
}