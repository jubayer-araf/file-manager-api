﻿namespace FileManagement.Helpers
{
    public static class FileSizeFormatter
    {
        static readonly string[] suffixes =
            {"Bytes", "KB", "MB", "GB", "TB", "PB" };
        public static string FormatSize(long size)
        {
            int counter = 0;
            float number = (float) size;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }
    }
}
