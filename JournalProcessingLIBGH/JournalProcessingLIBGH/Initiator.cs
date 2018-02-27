using Microsoft.ApplicationBlocks.Data;
using Models;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WBaseLib;

namespace JournalProcessingLIBGH
{
	public class Initiator
	{
		private string constr = ConfigurationManager.ConnectionStrings["AppDBConnect"].ConnectionString;

		public Terminal GetTerminal(string ip)
		{
			Terminal result;
			int num;
			while (true)
			{
				DataTable dataTable = SqlHelper.ExecuteDataset(this.constr, CommandType.Text, "select * from Terminal where ip=@ip", new SqlParameter[]
				{
					new SqlParameter("ip", ip)
				}).Tables[0];
				Terminal terminal = IEnumerableExtensions.ToList<Terminal>(dataTable).FirstOrDefault<Terminal>();
				result = terminal;
				num = 9079;
				int arg_5E_0 = num;
				num = 9079;
				switch ((arg_5E_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					continue;
				}
				break;
			}
			num = 1;
			if (num != 0)
			{
			}
			num = 0;
			if (num != 0)
			{
			}
			num = 0;
			return result;
		}

		public void Init()
		{
			switch (0)
			{
			case 0:
				goto IL_20;
			}
			int num2;
			while (true)
			{
				IL_0A:
				int num;
				switch (num)
				{
				case 0:
					goto IL_124;
				case 1:
					Task.Factory.StartNew(delegate
					{
						int num3 = 30471;
						int arg_1C_0 = num3;
						num3 = 30471;
						switch ((arg_1C_0 == num3) ? 1 : 0)
						{
						case 0:
						case 2:
							return;
						case 1:
						{
							IL_30:
							num3 = 0;
							if (num3 != 0)
							{
							}
							num3 = 0;
							num3 = 1;
							if (num3 != 0)
							{
							}
							num3 = 0;
							int num4 = num3;
							switch (num4)
							{
							case 0:
							{
								IL_90:
								IEnumerator enumerator = <>c__DisplayClass2_.table.Rows.GetEnumerator();
								try
								{
									num3 = 2;
									num4 = num3;
									while (true)
									{
										switch (num4)
										{
										case 0:
										{
											if (!enumerator.MoveNext())
											{
												num3 = 4;
												num4 = num3;
												continue;
											}
											DataRow dataRow = (DataRow)enumerator.Current;
											<>c__DisplayClass2_.jstoreid = int.Parse(dataRow["id"].ToString());
											string filepath = dataRow["filename"].ToString();
											string text = dataRow["filename"].ToString();
											string ipaddress = dataRow["ip"].ToString();
											string fdate = dataRow["filedate"].ToString();
											string filepath2 = filepath;
											string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filepath2);
											string value = Regex.Match(fileNameWithoutExtension, "\\d{8,12}").Value;
											num3 = 6;
											num4 = num3;
											continue;
										}
										case 2:
											switch (0)
											{
											case 0:
												goto IL_E4;
											}
											continue;
										case 3:
											goto IL_330;
										case 4:
											goto IL_31B;
										case 5:
											try
											{
												Terminal term = <>c__DisplayClass2_.<>4__this.GetTerminal(<>c__DisplayClass2_2.ipaddress);
												Library.writeErrorLog("BEGIN PROCESSING " + <>c__DisplayClass2_2.filepath + ">>>");
												Task.Factory.StartNew(delegate
												{
													int num5 = 0;
													num5 = -9228;
													int arg_38_0 = num5;
													num5 = -9228;
													switch ((arg_38_0 == num5) ? 1 : 0)
													{
													case 0:
													case 2:
														IL_4B:
														break;
													case 1:
														goto IL_4C;
													}
													goto IL_4B;
													IL_4C:
													num5 = 1;
													if (num5 != 0)
													{
													}
													num5 = 0;
													if (num5 != 0)
													{
													}
													string text2 = ConfigurationManager.AppSettings["iscrguy"];
													Library.writeErrorLog("Custome Journal...");
													int num6 = <>c__DisplayClass2_2.CS$<>8__locals1.pjf.ProcessJournal(<>c__DisplayClass2_2.filepath, <>c__DisplayClass2_2.fdate, <>c__DisplayClass2_2.ipaddress, term.terminalId, term.brandId.Value, term.brandname);
													Library.writeErrorLog(string.Concat(new object[]
													{
														"JOURNAL FILE ",
														<>c__DisplayClass2_2.filepath,
														" PROCESSED FOUND ",
														num6,
														" TOTAL TRANSACTIONS AND ADDED TO DB"
													}));
													Library.writeErrorLog("<<END PROCESSING " + <>c__DisplayClass2_2.filepath + ">>>>");
												});
												goto IL_154;
											}
											catch (Exception ex)
											{
												Library.writeErrorLog("ERROR PROCESSING FILE:" + <>c__DisplayClass2_2.filepath + " >>" + ex.Message);
												goto IL_154;
											}
											goto IL_31B;
										case 6:
											try
											{
												Terminal terminal = <>c__DisplayClass2_.<>4__this.GetTerminal(<>c__DisplayClass2_2.ipaddress.Trim());
												int value2 = terminal.brandId.Value;
												string value;
												<>c__DisplayClass2_2.fdate = <>c__DisplayClass2_.<>4__this.GetDateFromFile(value, value2);
												goto IL_26B;
											}
											catch (Exception ex2)
											{
												Library.writeErrorLog("ERROR GETTING ATM " + ex2.ToString() + ">>>");
												goto IL_26B;
											}
											goto IL_154;
											IL_26B:
											num3 = 5;
											num4 = num3;
											continue;
										}
										goto IL_E4;
										IL_154:
										num3 = 1;
										num4 = num3;
										continue;
										IL_235:
										num3 = 0;
										num4 = num3;
										continue;
										IL_E4:
										goto IL_235;
										IL_31B:
										num3 = 3;
										num4 = num3;
									}
									IL_330:;
								}
								finally
								{
									switch (0)
									{
									case 0:
										goto IL_34F;
									}
									IDisposable disposable;
									while (true)
									{
										IL_33C:
										switch (num4)
										{
										case 0:
											if (disposable != null)
											{
												num3 = 2;
												num4 = num3;
												continue;
											}
											goto IL_399;
										case 1:
											goto IL_397;
										case 2:
											disposable.Dispose();
											num3 = 1;
											num4 = num3;
											continue;
										}
										goto IL_34F;
									}
									IL_397:
									IL_399:
									goto EndFinally_9;
									IL_34F:
									disposable = (enumerator as IDisposable);
									num3 = 0;
									num4 = num3;
									goto IL_33C;
									EndFinally_9:;
								}
								return;
							}
							}
							goto IL_90;
						}
						}
						goto IL_30;
					});
					num2 = 3;
					num = num2;
					continue;
				case 2:
				{
					bool flag;
					if (flag)
					{
						num2 = 1;
						num = num2;
						continue;
					}
					Library.writeErrorLog("NO JOURNAL FILES FOUND FOR PROCESSING");
					num2 = 0;
					num = num2;
					continue;
				}
				case 3:
					goto IL_153;
				}
				goto IL_20;
			}
			IL_124:
			return;
			IL_153:
			num2 = 638;
			int arg_16F_0 = num2;
			num2 = 638;
			switch ((arg_16F_0 == num2) ? 1 : 0)
			{
			case 0:
			case 2:
			{
				IL_20:
				num2 = 1;
				if (num2 != 0)
				{
				}
				num2 = 0;
				ParseJournalFile pjf = new ParseJournalFile();
				DataTable table = SqlHelper.ExecuteDataset(this.constr, CommandType.Text, "select * from JournalsStore where transactioncount=0").Tables[0];
				Library.writeErrorLog("FOUND " + table.Rows.Count + " - FILES FOR PROCESSING....");
				int jstoreid = 0;
				bool flag = table.Rows.Count > 0;
				num2 = 2;
				int num = num2;
				goto IL_0A;
			}
			case 1:
				IL_183:
				num2 = 0;
				if (num2 != 0)
				{
				}
				return;
			}
			goto IL_183;
		}

		public string GetDateFromFile(string datefoldername, int brand)
		{
			int num = 0;
			int num2 = num;
			switch (num2)
			{
			case 0:
			{
				IL_17:
				switch (0)
				{
				case 0:
					goto IL_5C;
				}
				string result;
				string text;
				string text2;
				string text3;
				bool flag4;
				while (true)
				{
					IL_21:
					switch (num2)
					{
					case 0:
						goto IL_18C;
					case 1:
					{
						num = 12195;
						int arg_DC_0 = num;
						num = 12195;
						switch ((arg_DC_0 == num) ? 1 : 0)
						{
						case 0:
						case 2:
							goto IL_156;
						case 1:
						{
							IL_F0:
							num = 0;
							if (num != 0)
							{
							}
							bool flag;
							if (flag)
							{
								num = 11;
								num2 = num;
								continue;
							}
							goto IL_18C;
						}
						}
						goto IL_F0;
					}
					case 2:
						goto IL_18C;
					case 3:
					{
						bool flag2;
						if (flag2)
						{
							num = 10;
							num2 = num;
							continue;
						}
						bool flag3 = brand == 2;
						num = 7;
						num2 = num;
						continue;
					}
					case 4:
						return result;
					case 5:
						text = datefoldername.Substring(4, 4);
						text2 = datefoldername.Substring(2, 2);
						text3 = datefoldername.Substring(0, 2);
						num = 0;
						num2 = num;
						continue;
					case 6:
						num = 0;
						num = 1;
						if (num != 0)
						{
						}
						text = datefoldername.Substring(4, 4);
						text2 = datefoldername.Substring(2, 2);
						text3 = datefoldername.Substring(0, 2);
						num = 2;
						num2 = num;
						continue;
					case 7:
					{
						bool flag3;
						if (flag3)
						{
							num = 5;
							num2 = num;
							continue;
						}
						bool flag = brand == 3;
						num = 1;
						num2 = num;
						continue;
					}
					case 8:
						goto IL_18C;
					case 9:
					{
						if (flag4)
						{
							num = 6;
							num2 = num;
							continue;
						}
						bool flag2 = brand == 1;
						goto IL_156;
					}
					case 10:
						text = datefoldername.Substring(0, 4);
						text2 = datefoldername.Substring(2, 2);
						text3 = datefoldername.Substring(4, 2);
						num = 12;
						num2 = num;
						continue;
					case 11:
						text = datefoldername.Substring(6, 2);
						text2 = datefoldername.Substring(2, 2);
						text3 = datefoldername.Substring(0, 2);
						num = 8;
						num2 = num;
						continue;
					case 12:
						goto IL_18C;
					}
					goto IL_5C;
					IL_156:
					num = 3;
					num2 = num;
					continue;
					IL_18C:
					string text4 = string.Concat(new string[]
					{
						text,
						"-",
						text2,
						"-",
						text3
					});
					result = text4;
					num = 4;
					num2 = num;
				}
				return result;
				IL_5C:
				text = "";
				text2 = "";
				text3 = "";
				flag4 = (brand == 0);
				num = 9;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}
	}
}
