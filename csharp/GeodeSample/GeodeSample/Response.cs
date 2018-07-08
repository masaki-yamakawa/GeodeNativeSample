using Apache.Geode.Client;
using System;

namespace GeodeSample
{
    public class Response : IGeodeSerializable
    {
        #region Private members
        private long id;
        private string resValue;
        #endregion

//        #region Private methods
//        private UInt32 GetObjectSize(IGeodeSerializable obj)
//        {
//            return (obj == null ? 0 : obj.ObjectSize);
//    }
//        #endregion

        #region Public accessors
        public long Id
        {
            get
            {
                return id;
            }
        }

        public string ResValue
        {
            get
            {
                return resValue;
            }
        }

        public override string ToString()
        {
            return "Response [Id=" + id + " ResValue=" + resValue + "]";
        }
        #endregion

        #region Constructors
        public Response()
        {
            this.id = 0;
            this.resValue = null;
        }

        public Response(int id, string resValue)
        {
            this.id = id;
            this.resValue = resValue;
        }
        #endregion

        #region IGeodeSerializable Members
        public IGeodeSerializable FromData(DataInput input)
        {
            id = input.ReadInt64();
            resValue = input.ReadUTF();
            return this;
        }

        public void ToData(DataOutput output)
        {
            output.WriteInt64(id);
            output.WriteUTF(resValue);
        }

        public UInt32 ObjectSize
        {
            get
            {
                UInt32 objectSize = 0;
                objectSize += (UInt32) sizeof(long);
                objectSize += (UInt32) (resValue.Length * sizeof(char));
                return objectSize;
            }
        }

        public UInt32 ClassId
        {
            get
            {
                return 0x07;
            }
        }
        #endregion

        public static IGeodeSerializable CreateDeserializable()
        {
            return new Response();
        }
    }
}
