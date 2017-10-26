using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using dto.import;
using System.IO;

namespace business.import
{
    public abstract class ImporterBase
    {
        public abstract ImportedAccount Import(Stream stream);
    }


    public enum ImportFileTypes
    {
        Unknown,
        QIF,
        OFX,
    }
}