using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BLTools.Language {
    public class TTranslatorCommon : TTranslator
    {
        public const string TRANS_COMMON_FILENAME = "common.trans";

        public const string TXT_AVAILABLE = "Available";
        public const string TXT_NOT_AVAILABLE = "NotAvailable";
        public const string TXT_INITIALIZE = "Init";
        public const string TXT_DONE = "Done";
        public const string TXT_SUCCESSFUL = "Successful";
        public const string TXT_FAILED = "Failed";
        public const string TXT_PENDING = "Pending";
        public const string TXT_STARTED = "Started";
        public const string TXT_STOPPED = "Stopped";
        public const string TXT_CANCELLED = "Cancelled";

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        public TTranslatorCommon() : base()
        {
            using (Stream CommonStream = _LoadResourceFile(TRANS_COMMON_FILENAME))
            {
                Merge(CommonStream);
            }
        }

        public TTranslatorCommon(string filename) : base()
        {
            using (Stream CommonStream = _LoadResourceFile(TRANS_COMMON_FILENAME))
            {
                Merge(CommonStream);
            }
            Merge(filename);
        }

        public TTranslatorCommon(Stream stream) : base()
        {
            using (Stream CommonStream = _LoadResourceFile(TRANS_COMMON_FILENAME))
            {
                Merge(CommonStream);
            }
            Merge(stream);
        } 
        #endregion --- Constructor(s) ------------------------------------------------------------------------------
    }
}
