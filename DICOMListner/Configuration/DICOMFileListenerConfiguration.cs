using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.DICOM.Listener.Configuration
{
    public class DICOMFileListenerConfiguration
    {
        public int Port { get; set; }
        public string FolderStructure { get; set; }
    }
}
