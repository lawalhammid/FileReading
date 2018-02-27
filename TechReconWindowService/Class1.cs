using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReconWindowService
{
    class Class1
    {

                #region Source 1
//                #region   // Sybase Server below

//                if (dtSouceCon1.DatabaseType == "SYBASE")
//                {
//                    try
//                    {
//                        SaveLog("PullDataNostro  for Sybase Start in Library");
//                        var value1 = string.Empty;
//                        value1 = string.Empty;

//                        var value2 = string.Empty;
//                        value2 = string.Empty;
//                        int scriptExecTtype = 0;

//                        string FromDateParam = string.Empty;
//                        string ToDateParam = string.Empty;
//                        string LastRecordId = string.Empty;
//                        string CBSRecordCount = string.Empty;
//                        string MaxTransDate = string.Empty;
//                        string dateTest = string.Empty;
//                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == ReconType.ReconTypeId && c.TableName == "CBSNostroTrans");

//                        if (controlTable != null)
//                        {
//                            FromDateParam = controlTable.FromDateParam == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.FromDateParam));
//                            FromDateParam = "'" + FromDateParam + "'";
//                            ToDateParam = controlTable.ToDateParam == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.ToDateParam));
//                            ToDateParam = "'" + ToDateParam + "'";
//                            LastRecordId = controlTable.LastRecordId;
//                        }
//                        else
//                        {
//                            LastRecordId = "0";
//                            LastRecordId = "0";
//                            FromDateParam = "'20170901'";
//                            ToDateParam = "'20170923'";
//                        }

//                        string SqlString = "select    "
//+ "      A.acct_no as AcctNo   "
//+ "  ,   A.acct_type as AcctType   "
//+ "  ,   A.create_dt as TransDate   "
//+ "  ,   A.amt   "
//+ "  ,   A.description   "
//+ "  ,   str_replace((left(A.description,14)),'/', '') as TransRef   "
//+ "  ,   case when tran_code > 549 then 'DR'  else 'CR'  end DebitCredit   "
//+ "  ,   br.short_name as OriginatingBranch   "
//+ "  ,   rsm.name as PostedBy    "
//+ "  ,   ccy.iso_code as Currency   "
//+ "  ,   A.ptid   "
//+ "  From gl_history A   "
//+ "  ,   ad_gb_branch br   "
//+ "  ,   ad_gb_crncy Ccy   "
//+ "  ,   ad_gb_rsm rsm   "
////+ "  where A.acct_no IN ('01-001-1-11396')   "
//+ " where A.acct_no IN (" + acctlistSource1 + ")  " 
//+ "  and A.acct_type = 'GL'   "
//+ "  and A.crncy_id = ccy.crncy_id   "
//+ "  and A.orig_branch_no = br.branch_no   "
//+ "  and a.empl_id = rsm.employee_id   "
//+ "  and A.amt <> 0   "
//+ "  and A.ptid >  " + LastRecordId
//+ "  and A.create_dt >  " + ToDateParam
//+ "  and left(a.description,3) in ('GDT','GTT','GAP','GOA','GBC','GEB','GLC','GEL')   "
//+ "  union all   "
//+ "  select   "
//+ "      A.acct_no as AcctNo   "
//+ "  ,   A.acct_type as AcctType   "
//+ "  ,   A.create_dt as TransDate   "
//+ "  ,   A.amt   "
//+ "  ,   A.description   "
//+ "  ,   str_replace((substring(A.description,4,14)),'/', '') as TransRef   "
//+ "  ,   case when tran_code > 549 then 'DR'  else 'CR'  end DebitCredit   "
//+ "  ,   br.short_name as OriginatingBranch   "
//+ "  ,   rsm.name as PostedBy    "
//+ "  ,   ccy.iso_code as Currency   "
//+ "  ,   A.ptid   "
//+ "  From gl_history A   "
//+ "  ,   ad_gb_branch br   "
//+ "  ,   ad_gb_crncy Ccy   "
//+ "  ,   ad_gb_rsm rsm   "
////+ "  where A.acct_no IN ('01-001-1-11396')   "
//+ " where A.acct_no IN (" + acctlistSource1 + ")  " 
//+ "  and A.acct_type = 'GL'   "
//+ "  and A.crncy_id = ccy.crncy_id   "
//+ "  and A.orig_branch_no = br.branch_no   "
//+ "  and a.empl_id = rsm.employee_id   "
//+ "  and A.amt <> 0   "
//+ "  and A.ptid >   " + LastRecordId
//+ "  and A.create_dt >  " + ToDateParam
//+ "  and left(a.description,4) in ('COR/','REV/')   "
//+ "  union all   "
//+ "  select   "
//+ "      A.acct_no as AcctNo   "
//+ "  ,   A.acct_type as AcctType   "
//+ "  ,   A.create_dt as TransDate   "
//+ "  ,   A.amt   "
//+ "  ,   A.description   "
//+ "  ,   reference as TransRef   "
//+ "  ,   case when tran_code > 549 then 'DR'  else 'CR'  end DebitCredit   "
//+ "  ,   br.short_name as OriginatingBranch   "
//+ "  ,   rsm.name as PostedBy    "
//+ "  ,   ccy.iso_code as Currency   "
//+ "  ,   A.ptid   "
//+ "  From gl_history A   "
//+ "  ,   ad_gb_branch br   "
//+ "  ,   ad_gb_crncy Ccy   "
//+ "  ,   ad_gb_rsm rsm   "
////+ "  where A.acct_no IN ('01-001-1-11396')  "
//+ " where A.acct_no IN (" + acctlistSource1 + ")  " 
//+ "  and A.acct_type = 'GL'  "
//+ "  and A.crncy_id = ccy.crncy_id   "
//+ "  and A.orig_branch_no = br.branch_no   "
//+ "  and a.empl_id = rsm.employee_id   "
//+ "  and A.amt <> 0   "
//+ "  and A.ptid >  " + LastRecordId
//+ "  and A.create_dt >  " + ToDateParam
//+ "  and left(a.description,3) not in ('GDT','GTT','GAP','GOA','GBC','GEB','GLC','GEL')   "
//+ "  and left(a.description,4) not in ('COR/','REV/')    "
//+ "  order by A.ptid  ";


//                        string pdt = "";// parametername.Split(' ')[1];
//                        List<AseParameter> parameterPasses = new List<AseParameter>()
//                            {              
//                                new AseParameter() {ParameterName = pdt, AseDbType = AseDbType.VarChar, Value = value1},  
//                                new AseParameter() {ParameterName = pdt, AseDbType = AseDbType.VarChar, Value = value2}
//                            };

//                        string connstring = System.Configuration.ConfigurationManager.AppSettings["sybconnection"].ToString();

//                        connstring = connstring.Replace("{{Data Source}}", dtSouceCon1.ipAddress);// dt.SourceName);
//                        connstring = connstring.Replace("{{port}}", dtSouceCon1.PortNumber);
//                        connstring = connstring.Replace("{{database}}", dtSouceCon1.DatabaseName);
//                        connstring = connstring.Replace("{{uid}}", dtSouceCon1.UserName);
//                        connstring = connstring.Replace("{{pwd}}", Decrypt(dtSouceCon1.Password));
//                        // connstring = connstring.Replace("{{pwd}}", null);//  Decrypt(dtSouceCon.Password));//would later remove this, cos my own sybase doesnt have password that y i put null

//                        DataSet dsGetData = new DataSet();

//                        using (AseConnection theCons = new AseConnection(connstring))
//                        {
//                            DataSet ds = new DataSet();
//                            try
//                            {
//                                theCons.Open();
//                                if (theCons.State.ToString() == "Open")
//                                {
//                                    SaveLog("Sybase Connection  open for Ip " + dtSouceCon1.ipAddress);
//                                }
//                                else
//                                {
//                                    SaveLog("Sybase Connection not Open for Ip " + dtSouceCon1.ipAddress);
//                                }
//                            }
//                            catch (Exception ex)
//                            {

//                                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
//                                var stackTrace = new StackTrace(ex);
//                                var thisasm = Assembly.GetExecutingAssembly();
//                                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
//                                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
//                                SaveLog("An error occured Library CBSNostroVostroTrans Sybase in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
//                            }

//                            try
//                            {

//                                AseCommand cmd = new AseCommand(SqlString, theCons);
//                               // cmd.Connection = theCons;
//                               // cmd.CommandText = SqlString;
//                                //cmd.CommandTimeout = 180;
//                                //i.e check if the parameters are not null, if the are null, that means scriptExecTtype = 1, 
//                                // would be using CommandText not store procdure
//                                if (!string.IsNullOrWhiteSpace(value1) && !string.IsNullOrWhiteSpace(value1))
//                                {
//                                    if (parameterPasses != null)
//                                    {
//                                       // cmd.Parameters.AddRange(parameterPasses.ToArray());
//                                        var gggg = (parameterPasses.ToArray());
//                                    }
//                                }
//                                else
//                                {
//                                    scriptExecTtype = 1;
//                                }

//                               // cmd.CommandType = scriptExecTtype == 0 ? CommandType.StoredProcedure : CommandType.Text;
//                                    DataTable dt = new DataTable();
//                                    SaveLog("Reader NostroVostro ");
//                                    AseDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
//                                    DataTable dtschema = dr.GetSchemaTable();
//                                    //dt.DataSet.EnforceConstraints = false;
//                                    List<DataColumn> listCols = new List<DataColumn>();
//                                    if (dtschema != null)
//                                    {
//                                        foreach (DataRow drow  in dtschema.Rows)
                                        
//                                        {
//                                            string columnName = drow[0].ToString();
//                                            DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
//                                            //column.Unique = (bool)drow["IsUnique"];
//                                            column.AllowDBNull = (bool)drow["AllowDBNull"];
//                                            //column.AutoIncrement = (bool)drow["IsAutoIncrement"];
//                                            listCols.Add(column);
//                                            dt.Columns.Add(column);
//                                            foreach(DataColumn property in dtschema.Columns)
//                                            {
//                                                string columnName1 = drow[property].ToString();
//                                            }
//                                        }
//                                    }

//                                int county = 0;
//                                    while (dr.Read())
//                                    {
//                                        county++;
//                                        DataRow dataRow = dt.NewRow();
                                        
//                                        for (int i = 0; i < listCols.Count; i++)
//                                        {
//                                            dataRow[((DataColumn)listCols[i])] = dr[i];
                                           
//                                        }
                                       
//                                        dt.Rows.Add(dataRow);                              
//                                    }
//                                var returndata = dt;
                                
//                                SaveLog("ds NostroVostro  count: " + returndata.Rows.Count);    
                               
//                                if (returndata.Rows.Count > 0)
//                                {
                                    
//                                    SaveLog("PullDataCBSNostroTrans  for Sybase returndata.Rows.Count " + returndata.Rows.Count);
//                                    var CBSNostroTransTBL = new CBSNostroTran();
//                                    var CBSNostroTransTBLErr = new CBSNostroTransError();

//                                    SaveLog("Total records pulled record CBSNostroTrans: " + returndata.Rows.Count + "for the period from day: " + FromDateParam + "till :" + ToDateParam);
//                                    int count = 0;
//                                    int countTrans = 0;
//                                    for (int col = 0; col < returndata.Rows.Count; col++)
//                                    {
//                                        try
//                                        {
//                                            CBSNostroTransTBL.AcctNo = returndata.Rows[col][0] == null ? null : returndata.Rows[col][0].ToString();
//                                            CBSNostroTransTBL.AcctType = returndata.Rows[col][1] == null ? null : returndata.Rows[col][1].ToString();
//                                            CBSNostroTransTBL.TransDate = returndata.Rows[col][2] == null ? (DateTime?)null : Convert.ToDateTime(returndata.Rows[col][2]); ;
                                            
//                                            dateTest = CBSNostroTransTBL.TransDate.ToString();
//                                            if(countTrans > 1)
//                                            {
//                                                MaxTransDate = CBSNostroTransTBL.TransDate.ToString();
                                               
//                                                if(Convert.ToDateTime(dateTest) > Convert.ToDateTime(MaxTransDate))
//                                                {
//                                                    MaxTransDate = dateTest;
                                                   
//                                                }
//                                                else
//                                                {
//                                                    MaxTransDate = MaxTransDate;
//                                                }
//                                            }
//                                            CBSNostroTransTBL.Amount = returndata.Rows[col][3] == null ? 0 : Math.Round(Convert.ToDecimal(returndata.Rows[col][3]), 2);
//                                            CBSNostroTransTBL.Description = returndata.Rows[col][4] == null ? null : returndata.Rows[col][4].ToString();
//                                            CBSNostroTransTBL.Reference = returndata.Rows[col][5] == null ? null : returndata.Rows[col][5].ToString();
//                                            CBSNostroTransTBL.DebitCredit = returndata.Rows[col][6] == null ? null : returndata.Rows[col][6].ToString();
//                                            CBSNostroTransTBL.OriginatingBranch = returndata.Rows[col][7] == null ? null : returndata.Rows[col][7].ToString();
//                                            CBSNostroTransTBL.PostedBy = returndata.Rows[col][8] == null ? null : returndata.Rows[col][8].ToString();
//                                            CBSNostroTransTBL.Currency = returndata.Rows[col][9] == null ? null : returndata.Rows[col][9].ToString();
//                                            CBSNostroTransTBL.PtId = returndata.Rows[col][10] == null ? null : returndata.Rows[col][10].ToString();
//                                            LastRecordId = CBSNostroTransTBL.PtId;
//                                            CBSNostroTransTBL.MatchingStatus = "N";
//                                            CBSNostroTransTBL.ReconDate = DateTime.Now;
//                                            CBSNostroTransTBL.UserId = 1;
                                             
//                                            countTrans += 1;
                                           
//                                            if (!string.IsNullOrWhiteSpace(CBSNostroTransTBL.Reference))
//                                            {
//                                                var exist = await repoCBSNostroVostroTransRepository.GetAsync(c => c.Reference == CBSNostroTransTBL.Reference);
//                                                if (exist != null)
//                                                {
//                                                    CBSNostroTransTBLErr.AcctNo = CBSNostroTransTBL.AcctNo;
//                                                    CBSNostroTransTBLErr.AcctType = CBSNostroTransTBL.AcctType;
//                                                    CBSNostroTransTBLErr.TransDate = CBSNostroTransTBL.TransDate;
//                                                    CBSNostroTransTBLErr.Amount = CBSNostroTransTBL.Amount;
//                                                    CBSNostroTransTBLErr.Description = CBSNostroTransTBL.Description;
//                                                    CBSNostroTransTBLErr.Reference = CBSNostroTransTBL.Reference;
//                                                    CBSNostroTransTBLErr.DebitCredit = CBSNostroTransTBL.DebitCredit;
//                                                    CBSNostroTransTBLErr.OriginatingBranch = CBSNostroTransTBL.OriginatingBranch;
//                                                    CBSNostroTransTBLErr.PostedBy = CBSNostroTransTBL.PostedBy;
//                                                    CBSNostroTransTBLErr.Currency = CBSNostroTransTBL.Currency;
//                                                    CBSNostroTransTBLErr.PtId = CBSNostroTransTBL.PtId;
//                                                    LastRecordId = CBSNostroTransTBLErr.PtId;
//                                                    CBSNostroTransTBLErr.MatchingStatus = CBSNostroTransTBL.MatchingStatus;
//                                                    CBSNostroTransTBLErr.ReconDate = CBSNostroTransTBL.ReconDate;
//                                                    CBSNostroTransTBLErr.UserId = 1;
//                                                    CBSNostroTransTBLErr.ErrorMsg = "Duplicate transaction record";

//                                                    repoCBSNostroVostroTransErrorRepository.Add(CBSNostroTransTBLErr);

//                                                    var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
//                                                    if (ret1)
//                                                    {
//                                                        continue;
//                                                    }
//                                                    continue;
//                                                }
//                                                else
//                                                {
//                                                    repoCBSNostroVostroTransRepository.Add(CBSNostroTransTBL);
//                                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
//                                                    if (ret)
//                                                    {
//                                                         count += 1;
//                                                         CBSRecordCount = count.ToString();
//                                                        //SaveLog("No of Record Inserted count: " + CBSRecordCount);
//                                                         continue;
//                                                    }
//                                                }
//                                            }
//                                            else
//                                            {
//                                                CBSNostroTransTBLErr.AcctNo = CBSNostroTransTBL.AcctNo;
//                                                CBSNostroTransTBLErr.AcctType = CBSNostroTransTBL.AcctType;
//                                                CBSNostroTransTBLErr.TransDate = CBSNostroTransTBL.TransDate;
//                                                CBSNostroTransTBLErr.Amount = CBSNostroTransTBL.Amount;
//                                                CBSNostroTransTBLErr.Description = CBSNostroTransTBL.Description;
//                                                CBSNostroTransTBLErr.Reference = CBSNostroTransTBL.Reference;
//                                                CBSNostroTransTBLErr.DebitCredit = CBSNostroTransTBL.DebitCredit;
//                                                CBSNostroTransTBLErr.OriginatingBranch = CBSNostroTransTBL.OriginatingBranch;
//                                                CBSNostroTransTBLErr.PostedBy = CBSNostroTransTBL.PostedBy;
//                                                CBSNostroTransTBLErr.Currency = CBSNostroTransTBL.Currency;
//                                                CBSNostroTransTBLErr.PtId = CBSNostroTransTBL.PtId;
//                                                LastRecordId = CBSNostroTransTBLErr.PtId;
//                                                CBSNostroTransTBLErr.MatchingStatus = CBSNostroTransTBL.MatchingStatus;
//                                                CBSNostroTransTBLErr.ReconDate = CBSNostroTransTBL.ReconDate;
//                                                CBSNostroTransTBLErr.UserId = 1;
//                                                CBSNostroTransTBLErr.ErrorMsg = "No reference number";

//                                                repoCBSNostroVostroTransErrorRepository.Add(CBSNostroTransTBLErr);

//                                                var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
//                                                if (ret1)
//                                                {
//                                                    continue;
//                                                }
//                                            }
//                                        }
//                                        catch (Exception ex)
//                                        {
//                                            var exErr = ex == null ? ex.InnerException.Message : ex.Message;
//                                            var stackTrace = new StackTrace(ex);
//                                            var thisasm = Assembly.GetExecutingAssembly();
//                                            _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
//                                            _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
//                                            SaveLog("An error occured Library CBSNostroTrans Sybase1 in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
//                                            try
//                                            {
//                                                CBSNostroTransTBLErr.AcctNo = CBSNostroTransTBL.AcctNo;
//                                                CBSNostroTransTBLErr.AcctType = CBSNostroTransTBL.AcctType;
//                                                CBSNostroTransTBLErr.TransDate = CBSNostroTransTBL.TransDate;
//                                                CBSNostroTransTBLErr.Amount = CBSNostroTransTBL.Amount;
//                                                CBSNostroTransTBLErr.Description = CBSNostroTransTBL.Description;
//                                                CBSNostroTransTBLErr.Reference = CBSNostroTransTBL.Reference;
//                                                CBSNostroTransTBLErr.DebitCredit = CBSNostroTransTBL.DebitCredit;
//                                                CBSNostroTransTBLErr.OriginatingBranch = CBSNostroTransTBL.OriginatingBranch;
//                                                CBSNostroTransTBLErr.PostedBy = CBSNostroTransTBL.PostedBy;
//                                                CBSNostroTransTBLErr.Currency = CBSNostroTransTBL.Currency;
//                                                CBSNostroTransTBLErr.PtId = CBSNostroTransTBL.PtId;
//                                                LastRecordId = CBSNostroTransTBLErr.PtId;
//                                                CBSNostroTransTBLErr.MatchingStatus = CBSNostroTransTBL.MatchingStatus;
//                                                CBSNostroTransTBLErr.ReconDate = CBSNostroTransTBL.ReconDate;
//                                                CBSNostroTransTBLErr.UserId = 1;
//                                                CBSNostroTransTBLErr.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;

//                                                repoCBSNostroVostroTransErrorRepository.Add(CBSNostroTransTBLErr);

//                                                var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
//                                                if (ret1)
//                                                {
//                                                    continue;
//                                                }
//                                            }
//                                            catch (Exception ex1)
//                                            {
//                                                var exErr2 = ex1 == null ? ex.InnerException.Message : ex.Message;
//                                                var stackTrace2 = new StackTrace(ex);
//                                                var thisas = Assembly.GetExecutingAssembly();
//                                                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisas).Name;
//                                                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
//                                                SaveLog("An error occured Library CBSNostroVostroTrans Sybase2 in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
//                                                continue;
//                                            }
//                                            continue;
//                                        }
//                                    }
//                                    if (controlTable != null)
//                                    {
//                                        //Update with below
//                                        SaveLog("ReconType Id " + ReconType.ReconTypeId);
//                                        string updatScript = "update CBSNostroTrans set VostroAcctNo = b.VostroAcctNo "
//                                                       + " from CBSNostroTrans a,admSourceAccount b "
//                                                       + " where a.AcctNo = b.AcctNo "
//                                                       + " and b.ReconTypeId = " + ReconType.ReconTypeId;

//                                        string connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                                        SqlConnection con = new SqlConnection(connectionstring);

//                                        con.Open();
//                                        SqlCommand commd = new SqlCommand(updatScript, con);
//                                        commd.CommandType = CommandType.Text;
//                                        commd.ExecuteNonQuery();
//                                        con.Close(); 
                                        
//                                        controlTable.LastRecordId = LastRecordId;
//                                        controlTable.LastRecordTimeStamp = DateTime.Now;
//                                        string dateee = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate));
//                                        controlTable.ReconDate = Convert.ToDateTime(dateee);
//                                        controlTable.RecordsCount = Convert.ToInt32(CBSRecordCount);
//                                        //controlTable.FromDateParam = Convert.ToDateTime(ToDateParam);
//                                        //controlTable.ToDateParam = Convert.ToDateTime(ToDateParam);
//                                        repoadmDataPoollingControlRepository.Update(controlTable);
//                                        var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
//                                        if (ret)
//                                        {
//                                        }
//                                    }
//                                    else
//                                    {
//                                        SaveLog("ReconType Id " + ReconType.ReconTypeId);
//                                        string updatScript = "update CBSNostroTrans set VostroAcctNo = b.VostroAcctNo "
//                                                       + " from CBSNostroTrans a,admSourceAccount b "
//                                                       + " where a.AcctNo = b.AcctNo "
//                                                       + " and b.ReconTypeId = " + ReconType.ReconTypeId;

//                                        string connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                                        SqlConnection con = new SqlConnection(connectionstring);

//                                        con.Open();
//                                        SqlCommand commd = new SqlCommand(updatScript, con);
//                                        commd.CommandType = CommandType.Text;
//                                        commd.ExecuteNonQuery();
//                                        con.Close();

//                                        var admDataPoollingControlTBL = new admDataPoollingControl();
//                                        admDataPoollingControlTBL.ReconTypeId = ReconType.ReconTypeId;
//                                        admDataPoollingControlTBL.FileType = "Table";
//                                        admDataPoollingControlTBL.TableName = "CBSNostroTrans";
//                                        admDataPoollingControlTBL.DateCreated = DateTime.Now;
//                                        admDataPoollingControlTBL.UserId = 1;
//                                        //admDataPoollingControlTBL.FromDateParam = Convert.ToDateTime(ToDateParam);
//                                        //admDataPoollingControlTBL.ToDateParam = Convert.ToDateTime(ToDateParam);
//                                        admDataPoollingControlTBL.LastRecordTimeStamp = DateTime.Now;
//                                        admDataPoollingControlTBL.LastRecordId = LastRecordId;
//                                        admDataPoollingControlTBL.ReconLevel = 1;
//                                        string dateee = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate));
//                                        admDataPoollingControlTBL.ReconDate = Convert.ToDateTime(dateee);
//                                        admDataPoollingControlTBL.RecordsCount = Convert.ToInt32(CBSRecordCount);
//                                        repoadmDataPoollingControlRepository.Add(admDataPoollingControlTBL);
//                                        var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
//                                        if (ret)
//                                        {
//                                        }
//                                    }
//                                }

//                               // reader.Close();
//                                theCons.Close();

//                            }
//                            catch (Exception ex)
//                            {
//                                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
//                                var stackTrace = new StackTrace(ex);
//                                var thisasm = Assembly.GetExecutingAssembly();
//                                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
//                                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
//                                SaveLog("An error occured Library CBSNostroVostroTrans Sybase3 in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
//                                throw;
//                            }

//                        }
//                    }

//                    catch (Exception ex)
//                    {
//                        var exErr = ex == null ? ex.InnerException.Message : ex.Message;
//                        var stackTrace = new StackTrace(ex);
//                        var thisasm = Assembly.GetExecutingAssembly();
//                        _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
//                        _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
//                        SaveLog("An error occured in Line CBSNostroVostroTrans Sybase 4:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
//                        throw;
//                    }
//                }

//                #endregion
                #endregion




    }
}
