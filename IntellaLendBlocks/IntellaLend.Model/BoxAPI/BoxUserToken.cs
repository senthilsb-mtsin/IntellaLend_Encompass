using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{

    public class BoxUserToken
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int ExpireTime { get; set; }
        public string TokenType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }

    public class BoxItem
    {
        public string BoxID { get; set; }
        public string ItemType { get; set; }
        public Int32 Priority { get; set; }
    }

    public class BoxDuplicatedFilesFolder
    {
        public List<BoxFolderFileDuplicates> FolderFilesCount { get; set; }
        public List<BoxFolderFileDuplicates> FilesExistsCount { get; set; }
    }

    public class BoxFolderFileDuplicates
    {
        public string FolderID { get; set; }
        public string Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Int32 Priority { get; set; }
    }

}
