using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0X0391 : SendPacket
    {
        private long GroupQQ { get; }
        private byte[] MessageIndex { get; }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        public Send_0X0391(QQUser user, long groupQQ, byte[] messageIndex)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X0391;
            GroupQQ = groupQQ;
            this.MessageIndex = messageIndex;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            Writer.Write(new byte[] { 0x04, 0x00, 0x00 });
            Writer.Write(User.TXProtocol.DwClientType);
            Writer.Write(User.TXProtocol.DwPubNo);
            Writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            BodyWriter.Write(new byte[] { 0x00, 0x00, 0x00, 0x0D });
            var data = new BinaryWriter(new MemoryStream());
            data.Write(new byte[] { 0x0A, 0x12, 0x08 });
            data.Write(Util.HexStringToByteArray(Util.PB_toLength(GroupQQ)));
            data.Write(new byte[] { 0x12, 0x0a, 0x38, 0x00, 0x40, 0x00, 0x4a, 0x04, 0x08, 0x00, 0x10, 0x00 });
            //数据长度
            BodyWriter.BeWrite(data.BaseStream.Length);
            BodyWriter.Write(new byte[] { 0x08, 0x01, 0x12, 0x09 });

            var data12 = new BinaryWriter(new MemoryStream());
            data12.Write(
                Util.HexStringToByteArray(
                    Util.PB_toLength(Convert.ToInt64(Util.ToHex(MessageIndex).Replace(" ", ""), 16))));
            data12.Write(new byte[] { 0x88, 0x01, 0x04, 0x98, 0x01, 0x00 });

            BodyWriter.Write((byte) data12.BaseStream.Length);
            BodyWriter.Write(data12.BaseStream.ToBytesArray());

            //数据
            BodyWriter.Write(data.BaseStream.ToBytesArray());
        }
    }
}