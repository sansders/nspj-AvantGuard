using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Layout.Models
{
    class FileModel
    {

        public static string fileBytes { get; set; }
        public static string UserID { get; set; } 
        public static string fileName { get; set; } 
        public static string fileSize { get; set; } 
        public static string lastModified { get; set; } 
        public static string isFavorite { get; set; } 
        public static string isDeleted { get; set; } 
        public static string fileType { get; set; } 
        public static string sharedBy { get; set; }
        public static Boolean show { get; set; } 

        
     

        public FileModel(string id , string filename , string filebytes , string filesize , string lastmodified , string isfavorite, string isdeleted , string filetype , string sharedby)
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

        public static Boolean returnShow()
        {
            return show;
        }

        

        public static string ReturnFileBytes()
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
