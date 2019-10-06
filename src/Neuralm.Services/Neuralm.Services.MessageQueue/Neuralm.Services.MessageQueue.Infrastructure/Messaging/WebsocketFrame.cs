using System;
using System.Collections;
using System.Linq;

namespace Neuralm.Services.MessageQueue.Infrastructure.Messaging
{
    /// <summary>
    /// Represents the <see cref="WebSocketFrame"/> struct.
    /// According to RFC 6455.
    /// https://tools.ietf.org/html/rfc6455#page-30
    /// </summary>
    public readonly struct WebSocketFrame
    {
        public bool Fin { get; }
        public bool Rsv1 { get; }
        public bool Rsv2 { get; }
        public bool Rsv3 { get; }
        public Opcode Opcode { get; }
        public bool Masked { get; }
        public long PayloadLength { get; }
        public byte[] MaskingKey { get; }
        public byte[] PayloadData { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketFrame"/> struct.
        /// Used for incoming frames.
        /// </summary>
        /// <param name="incomingFrame">The incoming frame.</param>
        public WebSocketFrame(byte[] incomingFrame)
        {
            byte firstByte = incomingFrame[0];
            BitArray firstBitArray = new BitArray(BitConverter.GetBytes(firstByte).ToArray());
            Fin = Convert.ToBoolean(firstBitArray[0]);
            Rsv1 = Convert.ToBoolean(firstBitArray[1]);
            Rsv2 = Convert.ToBoolean(firstBitArray[2]);
            Rsv3 = Convert.ToBoolean(firstBitArray[3]);
            uint[] num = new uint[1];
            firstBitArray.CopyTo(num, 4);
            Opcode = (Opcode) num[0];

            byte secondByte = incomingFrame[1];
            BitArray secondBitArray = new BitArray(BitConverter.GetBytes(secondByte).ToArray());
            Masked = Convert.ToBoolean(secondBitArray[0]);

            num = new uint[1];
            secondBitArray.CopyTo(num, 1);
            PayloadLength = num[0];

            const long maskLength = 4;
            long offset;
            long totalLength;
            switch (PayloadLength)
            {
                case var length when length >= 0 && length < 126:
                {
                    offset = 2;
                    totalLength = PayloadLength + maskLength + offset;
                    break;
                }
                case var length when length == 126:
                {
                    PayloadLength = BitConverter.ToInt16(new[]
                    {
                        incomingFrame[3],
                        incomingFrame[2]
                    }, 0);
                    offset = 4;
                    totalLength = PayloadLength + maskLength + offset;
                    break;
                }
                case var length when length == 127:
                {
                    PayloadLength = BitConverter.ToInt64(new[]
                    {
                        incomingFrame[9], incomingFrame[8],
                        incomingFrame[7], incomingFrame[6],
                        incomingFrame[5], incomingFrame[4],
                        incomingFrame[3], incomingFrame[2]
                    }, 0);
                    offset = 10;
                    totalLength = PayloadLength + maskLength + offset;
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(PayloadLength), "The payload length must be 0 - 125, 126 or 127.");
                }
            }

            if (totalLength > incomingFrame.Length)
                throw new ArgumentOutOfRangeException(nameof(incomingFrame), "The incoming frame length is smaller than the total length.");

            MaskingKey = Masked ? new byte[4]
            {
                incomingFrame[offset], 
                incomingFrame[offset + 1], 
                incomingFrame[offset + 2], 
                incomingFrame[offset + 3]
            } : new byte[0];

            PayloadData = new byte[PayloadLength];

            Array.Copy(incomingFrame, offset + maskLength, PayloadData, 0, PayloadLength);

            for (int i = 0; i < PayloadData.Length; i++)
            {
                PayloadData[i] = (byte)(PayloadData[i] ^ MaskingKey[i % 4]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketFrame"/> struct.
        /// Used for a outgoing payload.
        /// </summary>
        /// <param name="outgoingPayload">The outgoing payload.</param>
        /// <param name="opcode">The op code.</param>
        public WebSocketFrame(byte[] outgoingPayload, Opcode opcode)
        {
            Fin = true;
            Rsv1 = false;
            Rsv2 = false;
            Rsv3 = false;
            Masked = false;
            MaskingKey = new byte[0];
            PayloadData = outgoingPayload;
            PayloadLength = outgoingPayload.Length;
            Opcode = opcode;
        }

        /// <summary>
        /// Converts the current web socket frame to a bytearray.
        /// </summary>
        /// <returns>Returns the current web socket frame as bytearray.</returns>
        public byte[] ToOutgoingFrame()
        {
            byte[] outgoingFrame;
            const int finRsv1Rsv2Rsv3Opcode = 1; // 1 byte = 1 + 1 + 1 + 1 + 4
            const int maskPayloadLength = 1; // 1 byte = 1 + 7
            const int extendedPayloadLength16 = 2; // 2 bytes
            const int extendedPayloadLength64 = 8; // 8 bytes

            if (PayloadLength < 126)
            {
                outgoingFrame = new byte[finRsv1Rsv2Rsv3Opcode + maskPayloadLength + PayloadLength];
                outgoingFrame[1] = (byte)PayloadLength;
                Array.Copy(PayloadData, 0, outgoingFrame, 2, PayloadLength);
            }
            else if (PayloadLength >= 126 && PayloadLength <= 65535)
            {
                outgoingFrame = new byte[finRsv1Rsv2Rsv3Opcode + maskPayloadLength + extendedPayloadLength16 + PayloadLength];
                outgoingFrame[1] = 126;
                outgoingFrame[2] = (byte)((PayloadLength >> 8) & 255);
                outgoingFrame[3] = (byte)(PayloadLength & 255);
                Array.Copy(PayloadData, 0, outgoingFrame, 4, PayloadLength);
            }
            else
            {
                outgoingFrame = new byte[finRsv1Rsv2Rsv3Opcode + maskPayloadLength + extendedPayloadLength64 + PayloadLength];
                outgoingFrame[1] = 127;
                outgoingFrame[2] = (byte)((PayloadLength >> 56) & 255);
                outgoingFrame[3] = (byte)((PayloadLength >> 48) & 255);
                outgoingFrame[4] = (byte)((PayloadLength >> 40) & 255);
                outgoingFrame[5] = (byte)((PayloadLength >> 32) & 255);
                outgoingFrame[6] = (byte)((PayloadLength >> 24) & 255);
                outgoingFrame[7] = (byte)((PayloadLength >> 16) & 255);
                outgoingFrame[8] = (byte)((PayloadLength >> 8) & 255);
                outgoingFrame[9] = (byte)(PayloadLength & 255);
                Array.Copy(PayloadData, 0, outgoingFrame, 10, PayloadLength);
            }
            // 1 0 0 0 0 0 1 0 == (Fin 1, Rsv1 = 0, Rsv2 = 0, Rsv3 = 0, (0010 = Opcode = 2))
            outgoingFrame[0] = (byte)((byte)Opcode | 128);

            return outgoingFrame;
        }
    }

    /// <summary>
    /// Represents the <see cref="Opcode"/> enumeration.
    /// According to RFC 6455.
    /// https://tools.ietf.org/html/rfc6455#page-30
    /// </summary>
    public enum Opcode
    {
        // frame-opcode-cont
        // %x0 ; frame continuation
        Fragment = 0,
        
        // frame-opcode-non-control
        // %x1; text frame
        Text = 1,
        // %x2 ; binary frame
        Binary = 2,
        // %x3-7 reserved for further non-control frames
        ReservedNonControl = 3 | 4 | 5 | 6 | 7,

        // frame-opcode-control
        // %x8 ; connection close
        CloseConnection = 8,
        // %x9 ; ping
        Ping = 9,
        // %xA ; pong
        Pong = 10,
        // %xB-F ; reserved for further control
        ReservedForControl = 11 | 12 | 13 | 14 | 15
    }
}
