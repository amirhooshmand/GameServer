using System;
using System.Collections.Generic;
using System.Text;

namespace Bindings
{
    public class PacketBuffer : IDisposable
    {
        List<byte> bufferList;
        byte[] readBuffer;
        int readPos;
        bool buffUpdate = false;

        public PacketBuffer()
        {
            bufferList = new List<byte>();
            readPos = 0;
        }
        public int GetReadPos()
        {
            return readPos;
        }
        public byte[] ToArray()
        {
            return bufferList.ToArray();
        }
        public int Count()
        {
            return bufferList.Count;
        }
        public int Lenght()
        {
            return Count();
        }
        public void Clear()
        {
            bufferList.Clear();
            readPos = 0;
        }
        

        //WriteData
        public void WriteBytes(byte[] input)
        {
            bufferList.AddRange(input);
            buffUpdate = true;
        }
        public void WriteByte(byte input)
        {
            bufferList.Add(input);
            buffUpdate = true;
        }
        public void WriteInteger(int input)
        {
            bufferList.AddRange(BitConverter.GetBytes(input));
            buffUpdate = true;
        }
        public void WriteFloat(float input)
        {
            bufferList.AddRange(BitConverter.GetBytes(input));
            buffUpdate = true;
        }
        public void WriteString(string input)
        {
            bufferList.AddRange(BitConverter.GetBytes(input.Length));
            bufferList.AddRange(Encoding.ASCII.GetBytes(input));
            buffUpdate = true;
        }

        //ReadData
        public int ReadInteger(bool peek = true)
        {
            if (bufferList.Count > readPos)
            {
                if (buffUpdate)
                {
                    readBuffer = bufferList.ToArray();
                    buffUpdate = false;
                }

                int value = BitConverter.ToInt32(readBuffer, readPos);
                if (peek & bufferList.Count > readPos)
                {
                    readPos += 4;
                }
                return value;
            }
            else
                throw new Exception("Buffer is its Limit!");
        }

        internal void WriteInteger(ServerPackets sConnectionOK)
        {
            throw new NotImplementedException();
        }

        public float ReadIFloat(bool peek = true)
        {
            if (bufferList.Count > readPos)
            {
                if (buffUpdate)
                {
                    readBuffer = bufferList.ToArray();
                    buffUpdate = false;
                }

                float value = BitConverter.ToSingle(readBuffer, readPos);
                if (peek & bufferList.Count > readPos)
                {
                    readPos += 4;
                }
                return value;
            }
            else
                throw new Exception("Buffer is its Limit!");
        }
        public byte ReadByte(bool peek = true)
        {
            if (bufferList.Count > readPos)
            {
                if (buffUpdate)
                {
                    readBuffer = bufferList.ToArray();
                    buffUpdate = false;
                }

                byte value = readBuffer[readPos];
                if (peek & bufferList.Count > readPos)
                {
                    readPos += 1;
                }
                return value;
            }
            else
                throw new Exception("Buffer is its Limit!");
        }
        public byte[] ReadBytes(int length, bool peek = true)
        {

            if (buffUpdate)
            {
                readBuffer = bufferList.ToArray();
                buffUpdate = false;
            }

            byte[] value = bufferList.GetRange(readPos, length).ToArray();
            if (peek & bufferList.Count > readPos)
            {
                readPos += 1;
            }
            return value;

        }
        public string ReadString(bool peek = true)
        {
            int length = ReadInteger(true);
            if (buffUpdate)
            {
                readBuffer = bufferList.ToArray();
                buffUpdate = false;
            }

            string value = Encoding.ASCII.GetString(readBuffer, readPos, length);
            if (peek & bufferList.Count > readPos)
            {
                readPos += 4;
            }
            return value;

        }

        //IDisposable
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    bufferList.Clear();
                }
                readPos = 0;
            }
            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
