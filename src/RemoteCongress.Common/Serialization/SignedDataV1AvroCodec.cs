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
using Avro.IO;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RemoteCongress.Common.Serialization
{
    /// <summary>
    /// An <see cref="ICodec"/> for a version 1 avro representation of a <see cref="SignedData"/>.
    /// </summary>
    public class SignedDataV1AvroCodec: ICodec<SignedData>
    {
        /// <summary>
        /// An <see cref="ILogger"/> instance to log against.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">
        /// An <see cref="ILogger"/> instance to log against.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="logger"/> is null.
        /// </exception>
        public SignedDataV1AvroCodec(ILogger<SignedDataV1AvroCodec> logger)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// The <see cref="RemoteCongressMediaType"/> handled by this codec.
        /// </summary>
        public readonly static RemoteCongressMediaType MediaType =
            new RemoteCongressMediaType(
                "application",
                "avro",
                "remotecongress.signeddata",
                1
            );

        /// <summary>
        /// Gets the preferred <see cref="RemoteCongressMediaType"/> for the codec.
        /// </summary>
        /// <returns>
        /// The preferred <see cref="RemoteCongressMediaType"/>.
        /// </returns>
        public RemoteCongressMediaType GetPreferredMediaType() =>
            MediaType;

        /// <summary>
        /// Checks if <paramref name="mediaType"/> can be handled by the codec.
        /// </summary>
        /// <param name="mediaType">
        /// The <see cref="RemoteCongressMediaType"/> to check if it can be handled.
        /// </param>
        /// <returns>
        /// True if <paramref name="mediaType"/> can be handled.
        /// </returns>
        public bool CanHandle(RemoteCongressMediaType mediaType) =>
            MediaType.Equals(mediaType);

        /// <summary>
        /// Decodes a <paramref name="data"/> into a <typeparamref name="TData"/>.
        /// </summary>
        /// <param name="mediaType">
        /// The <see cref="RemoteCongressMediaType"/> to decode the data from.
        /// </param>
        /// <param name="data">
        /// The <see cref="Stream"/> to decode dat from.
        /// </param>
        /// <returns>
        /// The <typeparamref name="TData"/> from <paramref name="data"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        /// Thrown if <paramref name="mediaType"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException"/>
        /// Thrown if <paramref name="data"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException"/>
        /// Thrown if the <paramref name="mediaType"/> cannot be handled.
        /// </exception>
        public Task<SignedData> Decode(RemoteCongressMediaType mediaType, Stream data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            if (!CanHandle(mediaType))
                throw new InvalidOperationException(
                    $"{GetType()} cannot handle {mediaType}"
                );

            Decoder decoder = new BinaryDecoder(data);

            string id = decoder.ReadString();
            string publicKey = decoder.ReadString();
            string blockContent = decoder.ReadString();
            string blockMediaType = decoder.ReadString();
            byte[] signature =  decoder.ReadBytes();

            return Task.FromResult(
                new SignedData(
                    publicKey,
                    blockContent,
                    signature,
                    RemoteCongressMediaType.Parse(blockMediaType)
                )
                {
                    Id = id
                }
            );
        }

        /// <summary>
        /// Encodes <paramref name="data"/> into <paramref name="mediaType"/>.
        /// </summary>
        /// <param name="mediaType">
        /// The <see cref="RemoteCongressMediaType"/> to encode the data to.
        /// </param>
        /// <param name="data">
        /// The data to encode.
        /// </param>
        /// <returns>
        /// A <see cref="Stream"/> containing the encoded data.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        /// Thrown if <paramref name="mediaType"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException"/>
        /// Thrown if <paramref name="data"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException"/>
        /// Thrown if the <paramref name="mediaType"/> cannot be handled.
        /// </exception>
        public Task<Stream> Encode(RemoteCongressMediaType mediaType, SignedData data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            if (!CanHandle(mediaType))
                throw new InvalidOperationException(
                    $"{GetType()} cannot handle {mediaType}"
                );

            MemoryStream stream = new MemoryStream();

            Encoder encoder = new BinaryEncoder(stream);

            encoder.WriteString(data.Id ?? string.Empty);
            encoder.WriteString(data.PublicKey);
            encoder.WriteString(data.BlockContent);
            encoder.WriteString(data.MediaType.ToString());
            encoder.WriteBytes(data.Signature);

            stream.Seek(0, SeekOrigin.Begin);

            return Task.FromResult(stream as Stream);
        }
    }
}