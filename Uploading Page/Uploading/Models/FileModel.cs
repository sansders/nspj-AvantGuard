using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Layout.Models
{
    class FileModel
    {

        private string filePath;
        private string UserID;
        private string fileName;
        private string fileSize;
        private string lastModified;
        private string isFavorite;
        private string isDeleted;
        private string fileType;
        private string sharedBy;

        public FileModel(String UserID, String fileName, String filePath , String fileSize, String lastModified , String isFavorite , String isDeleted , String fileType , String sharedBy )
        {
            this.UserID = UserID;
            this.fileName = fileName;
            this.filePath = filePath;
            this.fileSize = fileSize;
            this.lastModified = lastModified;
            this.isFavorite = isFavorite;
            this.isDeleted = isDeleted;
            this.fileType = fileType;
            this.sharedBy = sharedBy;
        }

        public void setFilePath(String filePath){
            this.filePath = filePath;
                        
        }

        public string ReturnFilePath()
        {
            return filePath;
        }

        public void setUserID(String UserID)
        {
            this.UserID = UserID;

        }

        public string ReturnUserID()
        {
            return UserID;
        }

        public string ReturnfileName()
        {
            return fileName;
        }

        public string ReturnFileSize()
        {
            return fileSize;
        }

        public string ReturnLastModified()
        {
            return lastModified;
        }

        public string ReturnIsFavorite()
        {
            return isFavorite;
        }
        public string ReturnIsDeleted()
        {
            return isDeleted;
        }
        public string ReturnFileType()
        {
            return fileType;
        }
        public string ReturnSharedBy()
        {
            return sharedBy;
        }
      
    }
}
