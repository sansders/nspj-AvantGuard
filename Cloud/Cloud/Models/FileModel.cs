using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Layout.Models
{
    class FileModel
    {

        public static byte[] fileBytes { get; set; }
        public static string UserID { get; set; } 
        public static string fileName { get; set; } 
        public static string fileSize { get; set; } 
        public static string lastModified { get; set; } 
        public static string isFavorite { get; set; } 
        public static string isDeleted { get; set; } 
        public static string fileType { get; set; } 
        public static string sharedBy { get; set; }
        public static Boolean show { get; set; }
        public static FileModel currentModel;
        
     

        public FileModel(string id , string filename , byte[] filebytes , string filesize , string lastmodified , string isfavorite, string isdeleted , string filetype , string sharedby)
        {

            UserID = id;
            fileName = filename;
            fileBytes = filebytes;
            fileSize = filesize;
            lastModified = lastmodified;
            isFavorite = isfavorite;
            isDeleted = isdeleted;
            fileType = filetype;
            sharedBy = sharedby;
               
            
        }

        public  Boolean getShow()
        {
            return show;
        }

        public  void setShow(bool value)
        {
            show = value;
        } 
        
        public static FileModel getFileModel()
        {
            return currentModel;
        }
        
        public static void setFileModel(FileModel value)
        {
            currentModel = value;
        }

        public byte[] ReturnFileBytes()
        {
            return fileBytes;
        }

       

        public static string ReturnUserID()
        {
            return UserID;
        }

        public static string ReturnfileName()
        {
            return fileName;
        }

        public static string ReturnFileSize()
        {
            return fileSize;
        }

        public static string ReturnLastModified()
        {
            return lastModified;
        }

        public static string ReturnIsFavorite()
        {
            return isFavorite;
        }
        public static string ReturnIsDeleted()
        {
            return isDeleted;
        }
        public static string ReturnFileType()
        {
            return fileType;
        }
        public static string ReturnSharedBy()
        {
            return sharedBy;
        }
      
    }
}
