
/* Reading of of data from two Sources, 
   source 1 is from Database, source 2
   from file(Excel), then move  the Excel file 
   to destination folder after reading. After the reading, 
   then Compare the two data from the the two sources for reconciliation*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.ServiceProcess;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Threading;
using System.Drawing;
using System.Configuration;
using System.Reflection;
using System.Globalization;
using Sybase.Data.AseClient;
using Excel;
using System.Data.OleDb;
//using Microsoft.VisualBasic.FileIO;
using iTextSharp.text.pdf; //Download this from nuget packet manager
using iTextSharp.text.pdf.parser;
using iTextSharp.text;
//using TechReconWindowService.Models;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using TechReconWindowService.DAL.Interfaces;
using TechReconWindowService.DAL.Implementation;
using TechReconWindowService.Repository.Repositories;
using System.Diagnostics;
using TechReconWindowService.DAL;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using TechReconWindowService.BAL.Helpers;

namespace TechReconWindowService
{
    public class Library
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmReconDataSourcesRepository repoadmReconDataSourcesRepository;
        private readonly IadmSourceAccountRepository repoadmSourceAccountRepository;
        private readonly IPostillionTransationTestRepository repoPostillionTransationTestRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly IadmTerminalRepository repoadmTerminalRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly IAfricaWorldTransactionRepository repoAfricaWorldTransactionRepository;
        private readonly ICBSAfricaWorldTransactionRepository repoCBSAfricaWorldTransactionRepository;
        private readonly IadmFailledFileRepository repoadmFailledFileRepository;
        private readonly IadmFileProcessedRepository repoadmFileProcessedRepository;

        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/Library.cs";
        public Library()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmReconDataSourcesRepository = new admReconDataSourcesRepository(idbfactory);
            repoadmSourceAccountRepository = new admSourceAccountRepository(idbfactory);
            repoPostillionTransationTestRepository = new PostillionTransationTestRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoadmTerminalRepository = new admTerminalRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoAfricaWorldTransactionRepository = new AfricaWorldTransactionRepository(idbfactory);
            repoCBSAfricaWorldTransactionRepository = new CBSAfricaWorldTransactionRepository(idbfactory);
            repoadmFailledFileRepository = new admFailledFileRepository(idbfactory);
            repoadmFileProcessedRepository = new admFileProcessedRepository(idbfactory);
        }

        
        //Move file after reading function below
        public string MoveFile(string filepath, string PathMovedTo, string fileNamenAndType)
        {
            try
            {
                LogManager.SaveLog("Moving File Source File directory: " + filepath + " Destination Folder:  " + PathMovedTo);
                string Source = filepath;
                string chkExistPathMovedTo = PathMovedTo + "\\" + fileNamenAndType;
                string originalFile = PathMovedTo + "\\" + fileNamenAndType;
                if (File.Exists(chkExistPathMovedTo))
                {
                    string folder = System.IO.Path.GetDirectoryName(filepath);
                    string filename = System.IO.Path.GetFileNameWithoutExtension(filepath);
                    string extension = System.IO.Path.GetExtension(filepath);
                    int number = 1;

                    Match regex = Regex.Match(filepath, @"(.+) \((\d+)\)\.\w+");

                    if (regex.Success)
                    {
                        filename = regex.Groups[1].Value;
                        number = int.Parse(regex.Groups[2].Value);
                    }
                    do
                    {
                        number++;
                        chkExistPathMovedTo = System.IO.Path.Combine(folder, string.Format("{0} ({1}){2}", filename, number, extension));
                    }

                    while (File.Exists(chkExistPathMovedTo));

                    string fnn = PathMovedTo;
                    string Destination = @fnn + "\\" + fileNamenAndType;
                    if (File.Exists(Destination))
                    {
                        do
                        {
                            number++;
                            Destination = System.IO.Path.Combine(folder, string.Format("{0} ({1}){2}", filename, number, extension));
                        }

                        while (File.Exists(chkExistPathMovedTo));
                    }

                    string hhh = "_" + number.ToString() + string.Format("{0:yyyyMMddhhttss}", DateTime.Now);

                    File.Move((Source), originalFile.ToString().Replace(fileNamenAndType, fileNamenAndType + hhh));
                    LogManager.SaveLog("Moving File Source File directory " + filepath + "Destination Folder : " + PathMovedTo + " Moved Successfully");
                    return "FileMovedSuccessfully";
                }
                else
                {
                    string fnn = PathMovedTo;
                    string fn = @fnn + "\\" + fileNamenAndType;
                    File.Move((filepath), fn);
                    LogManager.SaveLog("Moving File Source File directory " + filepath + "Destination Folder :" + PathMovedTo + " Moved Successfully");
                    return "FileMovedSuccessfully";
                }
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured Moving File: "+ " Moving File Source File directory:1` " + filepath + "Destination Folder " + PathMovedTo + " In Line" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }

            return string.Empty;
        }

        // Reading pdf file data function below
        public string readpdftotext(string filename)
        {

            try
            {

                StringBuilder strPdfContent = new StringBuilder();


                //' ''Dim document As New iTextSharp.text.Document()
                //' ''Dim writer As PdfWriter = PdfWriter.GetInstance(document, filename.ToString)
                //' ''document.Open()
                //' ''Dim cb As PdfContentByte = writer.getDirectContent()

                // ''Dim reader As PdfReader = Nothing
                // ''reader = New PdfReader(filename)

                // ''For i As Integer = 1 To reader.NumberOfPages


                // ''    Dim objExtractStrategy As ITextExtractionStrategy = New SimpleTextExtractionStrategy()

                // ''    Dim strLineText As String = PdfTextExtractor.GetTextFromPage(reader, i, objExtractStrategy)

                // ''    strLineText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.[Default], Encoding.UTF8, Encoding.[Default].GetBytes(strLineText)))

                // ''    strPdfContent.Append(strLineText)

                // ''    reader.Close()


                // ''    strPdfContent.Append("<br/>")
                // ''Next

                // ''Return strPdfContent.ToString

                string originalFile = filename;
                //'Using fs As New FileStream(originalFile, FileMode.Create, FileAccess.Write, FileShare.None)
                //'    Using doc As New iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER)
                //'        Using writer As PdfWriter = PdfWriter.GetInstance(doc, fs)
                //'            doc.Open()
                //'            doc.Add(New iTextSharp.text.Paragraph("Hi! I'm Original"))
                //'            doc.Close()
                //'        End Using
                //'    End Using
                //'End Using
                PdfReader reader = new PdfReader(originalFile);


                for (int i = 1; i <= reader.NumberOfPages; i++)
                {

                    ITextExtractionStrategy objExtractStrategy = new SimpleTextExtractionStrategy();

                    string strLineText = PdfTextExtractor.GetTextFromPage(reader, i, objExtractStrategy);

                    strLineText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(strLineText)));

                    strPdfContent.Append(strLineText);

                    reader.Close();


                    strPdfContent.Append("<br/>");
                }

                return strPdfContent.ToString();

                /*'Using fs As New FileStream(copyOfOriginal, FileMode.Create, FileAccess.Write, FileShare.None)
                '    ' Creating iTextSharp.text.pdf.PdfStamper object to write
                '    ' Data from iTextSharp.text.pdf.PdfReader object to FileStream object
                '    Using stamper As New PdfStamper(reader, fs)
                '    End Using
                'End Using */

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Read all terminals
        public async Task<List<DataList>> TerminalList()
        {
            var dataList = new List<DataList>();
            try
            {
                var dtSouceCon1 = await repoadmReconDataSourcesRepository.GetAsync(c => c.SourceName == "Terminal");

                #region  //MS SQL SERVER Below

                if (dtSouceCon1.DatabaseType == "MSSQL".Trim())
                {
                    string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["SQLconnection"].ToString();

                    dtSouceCon1.Password = dtSouceCon1.Password == null ? "" : Decrypt(dtSouceCon1.Password);

                    connstring = connstring.Replace("{{Data Source}}", dtSouceCon1.ipAddress = dtSouceCon1.ipAddress == null ? "" : dtSouceCon1.ipAddress.Trim());
                    connstring = connstring.Replace("{{port}}", dtSouceCon1.PortNumber = dtSouceCon1.PortNumber == null ? "" : dtSouceCon1.PortNumber.Trim());
                    connstring = connstring.Replace("{{database}}", dtSouceCon1.DatabaseName = dtSouceCon1.DatabaseName == null ? "" : dtSouceCon1.DatabaseName.Trim());
                    connstring = connstring.Replace("{{uid}}", dtSouceCon1.UserName = dtSouceCon1.UserName == null ? "" : dtSouceCon1.UserName.Trim());
                    connstring = connstring.Replace("{{pwd}}", dtSouceCon1.Password);

                    SqlConnection con = new SqlConnection(connstring);
                    try
                    {
                        con.Open();
                        if (con.State.ToString() == "Open")
                        {
                            LogManager.SaveLog("SQL Connection  open for Ip" + dtSouceCon1.ipAddress);
                        }
                        else
                        {
                            LogManager.SaveLog("SQL Connection not Open for Ip" + dtSouceCon1.ipAddress);
                        }
                    }
                    catch (Exception ex)
                    {
                        var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                        var stackTrace = new StackTrace(ex);
                        var thisasm = Assembly.GetExecutingAssembly();
                        _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                        _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                        LogManager.SaveLog("An error occured in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                    }

                    int SqlExecuteType = 0;
                    SqlExecuteType = 1;
                    string sqlScript = string.Empty;
                    if (SqlExecuteType == 1) // this means the Command type is Script not procedure
                    {

                        sqlScript = "Select * from eftc_terminal_lookup_susp_acc";



                        var returndata = GetConnect(sqlScript, con, SqlExecuteType);

                        var TerminalRepositoryTBL = new admTerminal();


                        for (int col = 0; col < returndata.Rows.Count; col++)
                        {
                            TerminalRepositoryTBL.Name = returndata.Rows[col][0].ToString() == string.Empty.Trim() ? null : returndata.Rows[col][0].ToString();

                            repoadmTerminalRepository.Add(TerminalRepositoryTBL);
                            var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                            if (ret)
                            {

                            }
                        }
                    }


                    con.Close();
                    con.Dispose();
                }

                #endregion
                return dataList;
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
            }
            return dataList;
        }

        //ADO Connection string to MSSQL server connection to pull record
        public static DataTable GetConnect(string SqlScript, SqlConnection con, int ScriptType)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(SqlScript, con);
                cmd.CommandType = ScriptType == 0 ? CommandType.StoredProcedure : CommandType.Text;

                SqlDataReader dr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                ds.Load(dr, LoadOption.OverwriteChanges, "Results");
                dt = ds.Tables[0];
                dr.Close();

                return dt;
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                // _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                // _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                //LogManager.SaveLog("An error occured in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
            }

            return dt;
        }

        //ADO Connection string to MSSQL server connection to pull record for procedure that has many parameters
        public static DataTable GetConnectionParam(string SqlScript, SqlParameter[] sqlcmd, string connstring)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connstring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(SqlScript, con);

                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < sqlcmd.Count(); i++)
                {
                    SqlParameter paramuserId = new SqlParameter();
                    paramuserId.ParameterName = sqlcmd[i].ParameterName;
                    paramuserId.Value = sqlcmd[i].Value;
                    cmd.Parameters.Add(paramuserId);
                }

                SqlDataReader dr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                ds.Load(dr, LoadOption.OverwriteChanges, "Results");

                dt = ds.Tables[0];
                dr.Close();
                con.Close();


            }
            return dt;
        }

        //Reading CSV file function below
        public DataTable CSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();

            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //read column names
                    string[] colFields = csvReader.ReadFields();

                    var count = colFields.Count();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }

                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                var ex1c = ex;
            }
            return csvData;
        }
        public async Task<DataTable> ReadExcel(string FileDirectory, int ReconTypeId)
        {
            DataTable dTable = new DataTable();

            try
            {
                DirectoryInfo d = new DirectoryInfo(FileDirectory);
                List<FileInfo> DLIST = null;
                DLIST = d.GetFiles("*" + ".xlsx").ToList();
                DLIST.AddRange(d.GetFiles("*" + ".xls").ToList());

                string fileNamenAndType = string.Empty;

                fileNamenAndType = (from f in DLIST orderby f.LastWriteTime ascending select f).First().Name;

                var getReconTypeInfo = await repoReconTypeRepository.GetAsync(c => c.ReconTypeId == ReconTypeId);
                if (!fileNamenAndType.Contains(getReconTypeInfo.FileNamingConvention))
                {
                    var failFileTBL = new FailledFile();
                    failFileTBL.ReconTypeId = ReconTypeId;
                    failFileTBL.FileDirectory = FileDirectory;
                    failFileTBL.FileName = fileNamenAndType;
                    failFileTBL.NameConvention = getReconTypeInfo.FileNamingConvention;
                    failFileTBL.DateProcessed = DateTime.Now;
                    failFileTBL.ErrCode = -1;
                    failFileTBL.ErrText = "Unrecognized file name";
                    failFileTBL.DateCreated = DateTime.Now;
                    failFileTBL.UserId = 1;

                    LogManager.SaveLog("Couldn't read File, the Naming Convention  does not match for Recon type :" + getReconTypeInfo.ReconName + " from FileDirectory: " + FileDirectory + " . The file Name and type is: " + fileNamenAndType + " and the conventional name must be: " + getReconTypeInfo.FileNamingConvention);
                 
                    repoadmFailledFileRepository.Add(failFileTBL);

                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                        string MvFile = MoveFile(FileDirectory + "\\" + fileNamenAndType, getReconTypeInfo.RejectedFileDirectory, fileNamenAndType);
                        return dTable;
                    }
                }
                else
                {
                    var chkfile = await repoadmFileProcessedRepository.GetAsync(c => c.FileName == fileNamenAndType);
                    if (chkfile != null)
                    {
                        var failFileTBL = new FailledFile();
                        failFileTBL.ReconTypeId = ReconTypeId;
                        failFileTBL.FileDirectory = FileDirectory;
                        failFileTBL.FileName = fileNamenAndType;
                        failFileTBL.NameConvention = getReconTypeInfo.FileNamingConvention;
                        failFileTBL.DateProcessed = DateTime.Now;
                        failFileTBL.ErrCode = -1;
                        failFileTBL.ErrText = "Couldn't process File more than once";
                        failFileTBL.DateCreated = DateTime.Now;
                        failFileTBL.UserId = 1;

                        LogManager.SaveLog("Couldn't read the File more than once, File already Read for  Recon type :" + getReconTypeInfo.ReconName + " from  FileDirectory: " + FileDirectory + " The file Name and it type is: " + fileNamenAndType);
                        repoadmFailledFileRepository.Add(failFileTBL);

                        var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                            string MvFile = MoveFile(FileDirectory + "\\" + fileNamenAndType, getReconTypeInfo.RejectedFileDirectory, fileNamenAndType);
                            return dTable;
                        }
                    }

                }
                IExcelDataReader excelReader = null;
                using (MemoryStream ms = new MemoryStream())
                using (FileStream file = new FileStream(FileDirectory + "\\" + fileNamenAndType, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);
                    if (fileNamenAndType.Contains("xlsx"))
                    {
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(new MemoryStream(bytes));
                    }
                    else if (fileNamenAndType.Contains("xls"))
                    {
                        excelReader = ExcelReaderFactory.CreateBinaryReader(new MemoryStream(bytes));
                    }
                    else if (fileNamenAndType.Contains("XLS"))
                    {

                        excelReader = ExcelReaderFactory.CreateBinaryReader(new MemoryStream(bytes));
                    }
                }

                var result = excelReader.AsDataSet();

                excelReader.IsFirstRowAsColumnNames = true;
                dTable = result.Tables[0];

                return dTable;
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in  Line Library ReadExcel File Source:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
               
            }
            return dTable;

        }
        public async Task<DataTable> ReadExcelUnityLink(string FileDirectory, int ReconTypeId, string fileNamenAndType)
        {
            DataTable dTable = new DataTable();
            try
            {
                var getReconTypeInfo = await repoReconTypeRepository.GetAsync(c => c.ReconTypeId == ReconTypeId);
                if (!fileNamenAndType.Contains(getReconTypeInfo.FileNamingConvention))
                {
                    var failFileTBL = new FailledFile();
                    failFileTBL.ReconTypeId = ReconTypeId;
                    failFileTBL.FileDirectory = FileDirectory;
                    failFileTBL.FileName = fileNamenAndType;
                    failFileTBL.NameConvention = getReconTypeInfo.FileNamingConvention;
                    failFileTBL.DateProcessed = DateTime.Now;
                    failFileTBL.ErrCode = -1;
                    failFileTBL.ErrText = "Unrecognized file name";
                    failFileTBL.DateCreated = DateTime.Now;
                    failFileTBL.UserId = 1;

                    LogManager.SaveLog("Cannot read File, the Naming Convention  does not match for Recon type :" + getReconTypeInfo.ReconName + " from FileDirectory: " + FileDirectory + " . The file Name and type is: " + fileNamenAndType + " and the conventional name must be: " + getReconTypeInfo.FileNamingConvention);

                    repoadmFailledFileRepository.Add(failFileTBL);

                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                        string fnn = getReconTypeInfo.RejectedFileDirectory;
                        string fn = @fnn + "\\" + fileNamenAndType;
                        File.Move((FileDirectory + "\\" + fileNamenAndType), fn);

                        return dTable;
                    }
                }
                else
                {
                    var chkfile = await repoadmFileProcessedRepository.GetAsync(c => c.FileName == fileNamenAndType);
                    if (chkfile != null)
                    {
                        var failFileTBL = new FailledFile();
                        failFileTBL.ReconTypeId = ReconTypeId;
                        failFileTBL.FileDirectory = FileDirectory;
                        failFileTBL.FileName = fileNamenAndType;
                        failFileTBL.NameConvention = getReconTypeInfo.FileNamingConvention;
                        failFileTBL.DateProcessed = DateTime.Now;
                        failFileTBL.ErrCode = -1;
                        failFileTBL.ErrText = "Couldn't process File more than once";
                        failFileTBL.DateCreated = DateTime.Now;
                        failFileTBL.UserId = 1;

                        LogManager.SaveLog("Cannot read the File more than once, File already Read for  Recon type :" + getReconTypeInfo.ReconName + " from  FileDirectory: " + FileDirectory + " The file Name and it type is: " + fileNamenAndType);
                        repoadmFailledFileRepository.Add(failFileTBL);

                        var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                            string fnn = getReconTypeInfo.RejectedFileDirectory;
                            string fn = @fnn + "\\" + FileDirectory + fileNamenAndType;
                            File.Move((FileDirectory + "\\" + fileNamenAndType), fn);

                            return dTable;
                        }
                    }

                }
                IExcelDataReader excelReader = null;
                using (MemoryStream ms = new MemoryStream())
                using (FileStream file = new FileStream(FileDirectory + "\\" + fileNamenAndType, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);
                    if (fileNamenAndType.Contains("xlsx"))
                    {
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(new MemoryStream(bytes));
                    }
                    else if (fileNamenAndType.Contains("xls"))
                    {
                        excelReader = ExcelReaderFactory.CreateBinaryReader(new MemoryStream(bytes));
                    }
                    else if (fileNamenAndType.Contains("XLS"))
                    {

                        excelReader = ExcelReaderFactory.CreateBinaryReader(new MemoryStream(bytes));
                    }
                }

                var result = excelReader.AsDataSet();

                excelReader.IsFirstRowAsColumnNames = true;

                var dt = new DataTable();
                dt = result.Tables[0];


                return dTable;
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in  Line Library ReadExcel File Source:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                throw;

            }
            return dTable;

        }      
        public async Task<DataTable> ReadUnityLink(string FileDirectory, int ReconTypeId, string fileNamenAndType)
        {
            var dTable = new DataTable();

            try
            {
                var getReconTypeInfo = await repoReconTypeRepository.GetAsync(c => c.ReconTypeId == ReconTypeId);
                if (!fileNamenAndType.Contains(getReconTypeInfo.FileNamingConvention))
                {
                    var failFileTBL = new FailledFile();
                    failFileTBL.ReconTypeId = ReconTypeId;
                    failFileTBL.FileDirectory = FileDirectory;
                    failFileTBL.FileName = fileNamenAndType;
                    failFileTBL.NameConvention = getReconTypeInfo.FileNamingConvention;
                    failFileTBL.DateProcessed = DateTime.Now;
                    failFileTBL.ErrCode = -1;
                    failFileTBL.ErrText = "Unrecognized file name";
                    failFileTBL.DateCreated = DateTime.Now;
                    failFileTBL.UserId = 1;

                    LogManager.SaveLog("Cannot read File, the Naming Convention  does not match for Recon type :" + getReconTypeInfo.ReconName + " from FileDirectory: " + FileDirectory + " . The file Name and type is: " + fileNamenAndType + " and the conventional name must be: " + getReconTypeInfo.FileNamingConvention);

                    repoadmFailledFileRepository.Add(failFileTBL);

                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                        string fnn = getReconTypeInfo.RejectedFileDirectory;
                        string fn = @fnn + "\\" + fileNamenAndType;
                        File.Move((FileDirectory + "\\" + fileNamenAndType), fn);

                        return dTable;
                    }
                }
                else
                {
                    var chkfile = await repoadmFileProcessedRepository.GetAsync(c => c.FileName == fileNamenAndType);
                    if (chkfile != null)
                    {
                        var failFileTBL = new FailledFile();
                        failFileTBL.ReconTypeId = ReconTypeId;
                        failFileTBL.FileDirectory = FileDirectory;
                        failFileTBL.FileName = fileNamenAndType;
                        failFileTBL.NameConvention = getReconTypeInfo.FileNamingConvention;
                        failFileTBL.DateProcessed = DateTime.Now;
                        failFileTBL.ErrCode = -1;
                        failFileTBL.ErrText = "Couldn't processed the same File more than once";
                        failFileTBL.DateCreated = DateTime.Now;
                        failFileTBL.UserId = 1;

                        LogManager.SaveLog("Cannot read the File more than once, File already Read for  Recon type :" + getReconTypeInfo.ReconName + " from  FileDirectory: " + FileDirectory + " The file Name and it type is: " + fileNamenAndType);
                        repoadmFailledFileRepository.Add(failFileTBL);

                        var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                            string fnn = getReconTypeInfo.RejectedFileDirectory;
                            string fn = @fnn + "\\" + FileDirectory + fileNamenAndType;
                            File.Move((FileDirectory + "\\" + fileNamenAndType), fn);

                            return dTable;
                        }
                    }

                }

                OleDbConnection excelConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileDirectory + "\\" + fileNamenAndType + ";Extended Properties='Excel 12.0 xml;HDR=YES;'");

                excelConnection.Open();

                DataTable Sheets = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sht = "";
                foreach (DataRow dr in Sheets.Rows)
                {
                    sht = dr[2].ToString().Replace("'", "");
                    // if (sht == "Sheet1$")
                    // {
                    OleDbCommand cmd = new OleDbCommand("select * from [" + sht + "]", excelConnection);
                    
                  

                    SqlDataAdapter da = new SqlDataAdapter();

                    //excelConnection.Open();
                    OleDbDataReader dReader;
                    dReader = cmd.ExecuteReader();

                    //dt = dReader.GetSchemaTable(); We can use this one to get the column table name
                    dTable.Load(dReader);
                    //int count = 0;

                    int numberofCol = dTable.Columns.Count;

                    var hhh = dTable;
                }



                return dTable;
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in  Line Library ReadExcel File Source:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                throw;
            }

            return dTable;

        }
        public List<DataList> GetDataTabletFromCSVFile(string csv_file_path, List<DataList> dataList)
        {
            DataTable csvData = new DataTable();

            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //read column names
                    string[] colFields = csvReader.ReadFields();

                    var count = colFields.Count();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }

                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                var ex1 = ex;

                throw;
            }

            foreach (DataRow row in csvData.Rows.Cast<DataRow>())
            {

                dataList.Add(new DataList
                {
                    column1 = row[0].ToString(),
                    column2 = row[1].ToString(),
                    // column3 = row[3].ToString(),
                    // column4 = row[4].ToString(),
                    // column5 = row[5].ToString(),
                });
            }
            return dataList;
        }

      
        //Security,  Encryption part
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        //Security,  Decryption part
        public string Decrypt(string cipherText)
        {
            /*The below variable(EncryptionKey) value must be same as the key used in the above 
                Encrypt function*/
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        //Connection to Oracle database
        public System.Data.DataTable OracOLEds(string SqlString, string constring, int rowlimit)
        {
            DataTable dt = new DataTable();
            if (rowlimit > 0)
            {
                try
                {
                    using (System.Data.OracleClient.OracleConnection OracConnect = new System.Data.OracleClient.OracleConnection(constring))
                    {
                        OracConnect.Open();
                        System.Data.OracleClient.OracleCommand OraCmd = new System.Data.OracleClient.OracleCommand(SqlString, OracConnect);
                        System.Data.OracleClient.OracleDataReader Oraxreader = OraCmd.ExecuteReader();
                        DataSet ds = new DataSet();
                        ds.Load(Oraxreader, LoadOption.OverwriteChanges, "Result");
                        var ddt = ds.Tables[0].AsEnumerable().Take(rowlimit);
                        dt = ddt.CopyToDataTable<DataRow>();
                        Oraxreader.Close();
                        OracConnect.Close();
                    }
                }
                catch (Exception exOrac)
                {
                    var ex1 = exOrac.Message;
                }
            }
            else
            {
                try
                {
                    using (System.Data.OracleClient.OracleConnection OracConnect = new System.Data.OracleClient.OracleConnection(constring))
                    {
                        OracConnect.Open();
                        System.Data.OracleClient.OracleCommand OraCmd = new System.Data.OracleClient.OracleCommand(SqlString, OracConnect);
                        System.Data.OracleClient.OracleDataReader Oraxreader = OraCmd.ExecuteReader();
                        DataSet ds = new DataSet();
                        ds.Load(Oraxreader, LoadOption.OverwriteChanges, "Result");
                        dt = ds.Tables[0];
                        Oraxreader.Close();
                        OracConnect.Close();
                    }
                }
                catch (Exception exOrac)
                {
                    var ex1 = exOrac.Message;
                }
            }
            return dt;
        }
    }
  
    // Classes to hold the the data
    public class DataList
    {
        public string column0 { get; set; }
        public string column1 { get; set; }
        public string column2 { get; set; }
        public string column3 { get; set; }
        public string column4 { get; set; }
        public string column5 { get; set; }
        public string column6 { get; set; }
        public string column7 { get; set; }
        public string column8 { get; set; }
        public string column9 { get; set; }
        public string column10 { get; set; }
        public string column11 { get; set; }
        public string column12 { get; set; }
        public string column13 { get; set; }
        public string column14 { get; set; }
        public string column15 { get; set; }
        public string column16 { get; set; }
        public string column17 { get; set; }
        public string column18 { get; set; }
        public string column19 { get; set; }
        public string column20 { get; set; }
        public string column21 { get; set; }
        public string column22 { get; set; }
        public string column23 { get; set; }
        public string column24 { get; set; }
        public string column25 { get; set; }
        public string column26 { get; set; }
        public string column27 { get; set; }
        public string column28 { get; set; }
        public string column29 { get; set; }
        public string column30 { get; set; }
        public string column31 { get; set; }
        public string column32 { get; set; }
        public string column33 { get; set; }
        public string column34 { get; set; }
        public string column35 { get; set; }
        public string column36 { get; set; }
        public string column37 { get; set; }
        public string column38 { get; set; }
        public string column39 { get; set; }
        public string column40 { get; set; }

        public int NoOfColumn { get; set; }
        
       
    }
}