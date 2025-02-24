﻿using System.Drawing;

namespace CLDV_POE.Models
{
    public class FileModel { 
    public string Name { get; set; }
    public long Size { get; set; }
    public DateTimeOffset? LastModified { get; set; }

    public string FilePath { get; set; }
    public string DisplaySize
    {
        get
        {
            if (Size >= 1024 * 1024)
                return $"{Size / 1024 / 1024} MB";
            if (Size >= 1024)
                return $"{Size / 1024} KB";
            return $"{Size} Bytes";
        }
    }
}
}