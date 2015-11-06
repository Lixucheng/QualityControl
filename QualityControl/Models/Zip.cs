using System;
using System.IO;
using System.IO.Packaging;

namespace QualityControl.Models
{
    internal static class Zip
    {
        /// Add a folder along with its subfolders to a Package
        /// </summary>
        /// <param name="folderName">The folder to add</param>
        /// <param name="compressedFileName">The package to create</param>
        /// <param name="overrideExisting">Override exsisitng files</param>
        /// <returns></returns>
        public static bool PackageFolder(string folderName, string compressedFileName, bool overrideExisting)
        {
            if (folderName.EndsWith(@"\"))
                folderName = folderName.Remove(folderName.Length - 1);
            var result = false;
            if (!Directory.Exists(folderName))
            {
                return result;
            }

            if (!overrideExisting && File.Exists(compressedFileName))
            {
                return result;
            }
            try
            {
                using (var package = Package.Open(compressedFileName, FileMode.Create))
                {
                    var fileList = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);
                    foreach (var fileName in fileList)
                    {
                        //The path in the package is all of the subfolders after folderName
                        string pathInPackage;
                        pathInPackage = Path.GetDirectoryName(fileName).Replace(folderName, string.Empty) + "/" +
                                        Path.GetFileName(fileName);

                        var partUriDocument = PackUriHelper.CreatePartUri(new Uri(pathInPackage, UriKind.Relative));
                        var packagePartDocument = package.CreatePart(partUriDocument, "", CompressionOption.Maximum);
                        using (var fileStream = new FileStream(fileName,FileMode.Open, FileAccess.Read))
                        {
                            //var srSource = new StreamReader(fileStream, System.Text.Encoding.Unicode).ReadToEnd();                          
                            //var t = System.Text.Encoding.Unicode.GetBytes(srSource);
                            //packagePartDocument.GetStream().Write(t,0,t.Length);
                            fileStream.CopyTo(packagePartDocument.GetStream());
                        }
                    }
                }
                result = true;
            }
            catch (Exception e)
            {
                throw new Exception("Error zipping folder " + folderName, e);
            }

            return result;
        }
    }
}