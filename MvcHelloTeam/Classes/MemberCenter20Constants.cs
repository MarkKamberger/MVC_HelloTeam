using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberCenter20NS
{
    public static class MemberCenter20Constants
    {
        public const string FRONT_OFFICE_TEXT = "Front Office";
        public const string STUDENT_CENTER_TEXT = "Student Center";
        public const string GROUPING_CENTER_TEXT = "Classrooms & Groups";
        public const string CLASSROOM_CENTER_TEXT = "Lesson Cycles";
        public const string TESTING_CENTER_TEXT = "Testing Center";
        public const string REPORTS_CENTER_TEXT = "Report Center";
        public const string MY_CENTER_TEXT = "My Center";
        public const string LEADING_FOR_SUCCESS = "Leading For Success";

        public const string MY_CENTER_COLOR = "#d0ac12";
        public const string MY_CENTER_IMAGE = "~/Graphics/Layout/ColorIcon_Yellow.jpg";

        public const string TABLE_HEADER_ROW_COLOR = "#e4e4e4";
        public const string TABLE_HEADER_ROW_COLOR_SIENNA = "#C35817";

        public enum SFAFMemberCenter20ExceptionCodes
        {
            DefaultExceptionCode = 9999,
            SessionExceptionCode = 1432,
            SecurityExceptionCode = 2891,
            GenericExceptionCode = 3723
        }

        public enum SFAFMemberCenter20ValidUploadFileTypes
        {
            txt = 0,
            pdf = 1,
            xls = 2,
            doc = 3,
            htm = 4,
            html = 5,
            gif = 6,
            jpg = 7,
            jpeg = 8,
            jpe = 9,
            png = 10,
            mp3 = 11,
            mpe = 12,
            mpg = 13,
            mpa = 14,
            mpeg = 15
        }

        public enum SFAFMemberCenter20FileMIMETypes
        {
            PlainText = 4,
            MSWord = 2,
            MSExcel = 1,
            PDF = 3,
            Html = 5,
            Gif = 6,
            Jpeg = 7,
            Png = 8,
            AudioMpeg = 9,
            VideoMpeg = 10,
            AudioWav = 11
            //Rtf = 12
        }

        public enum SFAFMemberCenter20SFAFDataTypes
        {
            Char = 1,
            CheckBox = 2,
            Date = 6,
            File = 12,
            Money = 9,
            Multi_Selection = 10,
            Numeric = 7,
            Percent = 13,
            Query = 11,
            Radio = 3,
            Selection = 5,
            Text = 4
        }

        public const string SFAFSessionExpiredExceptionMessage = "Your session is invalid, possibly because it has expired. Please login again.";
    }
}