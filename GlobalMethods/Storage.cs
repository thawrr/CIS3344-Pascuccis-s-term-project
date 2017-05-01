using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalMethods
{
    [Serializable]
    public class Storage
    {
        private String FName;
        private String FType;
        private int FSize;
        private float FVersion;

        public String FileName
        {
            get { return FName; }
            set { FName = value; }
        }

        public String FileType
        {
            get { return FType; }
            set { FType = value; }
        }

        public int FileSize
        {
            get { return FSize; }
            set { FSize = value; }
        }

        public float FileVersion
        {
            get { return FVersion; }
            set { FVersion = value; }
        }
    }
}
