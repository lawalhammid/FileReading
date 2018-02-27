using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace JournalProcessingLIBGH
{
	public class JournalProcessorGH
	{
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			public static readonly JournalProcessorGH.<>c <>9;

			public static Predicate<string> <>9__46_1;

			static <>c()
			{
				// Note: this type is marked as 'beforefieldinit'.
				int num = -4403;
				int arg_1C_0 = num;
				num = -4403;
				switch ((arg_1C_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					IL_2F:
					break;
				case 1:
					goto IL_30;
				}
				goto IL_2F;
				IL_30:
				num = 1;
				if (num != 0)
				{
				}
				num = 0;
				if (num != 0)
				{
				}
				num = 0;
				JournalProcessorGH.<>c.<>9 = new JournalProcessorGH.<>c();
			}

			internal bool <GetErrorCodeAndCountsHYO>b__46_1(string f)
			{
				int num = 25635;
				int arg_1C_0 = num;
				num = 25635;
				switch ((arg_1C_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					IL_2F:
					break;
				case 1:
					goto IL_30;
				}
				goto IL_2F;
				IL_30:
				num = 1;
				if (num != 0)
				{
				}
				num = 0;
				if (num != 0)
				{
				}
				num = 0;
				return Regex.IsMatch(f.ToLower(), "opcode|error code|reject count|pick-up count|dispense count|request count|remain count|denomination|taken amount");
			}
		}

		public string filePath
		{
			[CompilerGenerated]
			get
			{
				int num = -8310;
				int arg_1C_0 = num;
				num = -8310;
				switch ((arg_1C_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					IL_2F:
					break;
				case 1:
					goto IL_30;
				}
				goto IL_2F;
				IL_30:
				num = 0;
				if (num != 0)
				{
				}
				num = 1;
				if (num != 0)
				{
				}
				num = 0;
				return this.<filePath>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				int num = -22799;
				int arg_1C_0 = num;
				num = -22799;
				switch ((arg_1C_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					IL_2F:
					break;
				case 1:
					goto IL_30;
				}
				goto IL_2F;
				IL_30:
				num = 1;
				if (num != 0)
				{
				}
				num = 0;
				if (num != 0)
				{
				}
				num = 0;
				this.<filePath>k__BackingField = value;
			}
		}

		public string ip
		{
			[CompilerGenerated]
			get
			{
				int num = 22167;
				int arg_1C_0 = num;
				num = 22167;
				switch ((arg_1C_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					IL_2F:
					break;
				case 1:
					goto IL_30;
				}
				goto IL_2F;
				IL_30:
				num = 1;
				if (num != 0)
				{
				}
				num = 0;
				num = 0;
				if (num != 0)
				{
				}
				return this.<ip>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				int num = 21997;
				int arg_1C_0 = num;
				num = 21997;
				switch ((arg_1C_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					IL_2F:
					break;
				case 1:
					goto IL_30;
				}
				goto IL_2F;
				IL_30:
				num = 1;
				if (num != 0)
				{
				}
				num = 0;
				num = 0;
				if (num != 0)
				{
				}
				this.<ip>k__BackingField = value;
			}
		}

		public string tid
		{
			[CompilerGenerated]
			get
			{
				int num = 10458;
				int arg_1C_0 = num;
				num = 10458;
				switch ((arg_1C_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					IL_2F:
					break;
				case 1:
					goto IL_30;
				}
				goto IL_2F;
				IL_30:
				num = 0;
				if (num != 0)
				{
				}
				num = 0;
				num = 1;
				if (num != 0)
				{
				}
				return this.<tid>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				int num = 11585;
				int arg_1C_0 = num;
				num = 11585;
				switch ((arg_1C_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					IL_2F:
					break;
				case 1:
					goto IL_30;
				}
				goto IL_2F;
				IL_30:
				num = 1;
				if (num != 0)
				{
				}
				num = 0;
				num = 0;
				if (num != 0)
				{
				}
				this.<tid>k__BackingField = value;
			}
		}

		public int brand
		{
			[CompilerGenerated]
			get
			{
				int num = -2011;
				int arg_1C_0 = num;
				num = -2011;
				switch ((arg_1C_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					IL_2F:
					break;
				case 1:
					goto IL_30;
				}
				goto IL_2F;
				IL_30:
				num = 1;
				if (num != 0)
				{
				}
				num = 0;
				num = 0;
				if (num != 0)
				{
				}
				return this.<brand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				int num = -572;
				int arg_1C_0 = num;
				num = -572;
				switch ((arg_1C_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					IL_2F:
					break;
				case 1:
					goto IL_30;
				}
				goto IL_2F;
				IL_30:
				num = 0;
				if (num != 0)
				{
				}
				num = 0;
				num = 1;
				if (num != 0)
				{
				}
				this.<brand>k__BackingField = value;
			}
		}

		private List<JournalsRawTranx> JournalPreJob(string filepath, string date, string ipaddress, string tid, int brand)
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
					goto IL_118;
				}
				string text;
				string[] array;
				List<JournalsRawTranx> list;
				List<JournalsRawTranx> result;
				bool flag17;
				string data;
				while (true)
				{
					IL_21:
					int num3;
					int num4;
					int num6;
					bool flag3;
					int num7;
					bool flag8;
					bool flag14;
					bool flag15;
					switch (num2)
					{
					case 0:
						goto IL_405;
					case 1:
					{
						bool flag;
						if (flag)
						{
							num = 35;
							num2 = num;
							continue;
						}
						text = text + array[num3] + "\n";
						num = 57;
						num2 = num;
						continue;
					}
					case 2:
					{
						text = text + array[num4] + "\n";
						list.Add(new JournalsRawTranx
						{
							filename = filepath,
							filedate = date,
							ip = ipaddress,
							terminalid = tid,
							trandata = text
						});
						text = "";
						int num5 = 0;
						num = 42;
						num2 = num;
						continue;
					}
					case 3:
						goto IL_3A1;
					case 4:
					{
						bool flag2;
						if (flag2)
						{
							num = 17;
							num2 = num;
							continue;
						}
						goto IL_405;
					}
					case 5:
						goto IL_961;
					case 6:
					{
						num = 1;
						if (num != 0)
						{
						}
						int num5 = 0;
						num4 = 0;
						goto IL_313;
					}
					case 7:
						goto IL_961;
					case 8:
						goto IL_8C4;
					case 9:
						goto IL_28E;
					case 10:
						goto IL_3A1;
					case 11:
						num6 = 0;
						num = 5;
						num2 = num;
						continue;
					case 12:
					{
						if (!flag3)
						{
							num = 16;
							num2 = num;
							continue;
						}
						bool flag4 = array[num3].Contains("TRANSACTION START");
						num = 34;
						num2 = num;
						continue;
					}
					case 13:
					{
						int num5 = 1;
						text = text + array[num4] + "\n";
						num = 27;
						num2 = num;
						continue;
					}
					case 14:
					{
						bool flag5;
						if (flag5)
						{
							num = 53;
							num2 = num;
							continue;
						}
						bool flag6 = array[num7].Contains("TRANSACTION END");
						num = 50;
						num2 = num;
						continue;
					}
					case 15:
						text = text + array[num6] + "\n";
						list.Add(new JournalsRawTranx
						{
							filename = filepath,
							filedate = date,
							ip = ipaddress,
							terminalid = tid,
							trandata = text
						});
						text = "";
						num = 55;
						num2 = num;
						continue;
					case 16:
						num = 29;
						num2 = num;
						continue;
					case 17:
						text = text + array[num4] + "\n";
						num = 0;
						num2 = num;
						continue;
					case 18:
					{
						bool flag7;
						if (flag7)
						{
							num = 11;
							num2 = num;
							continue;
						}
						goto IL_3A1;
					}
					case 19:
						goto IL_2AF;
					case 20:
						num = 3;
						num2 = num;
						continue;
					case 21:
						goto IL_47D;
					case 22:
						goto IL_47D;
					case 23:
						num3 = 0;
						num = 25;
						num2 = num;
						continue;
					case 24:
					{
						if (!flag8)
						{
							num = 56;
							num2 = num;
							continue;
						}
						bool flag9 = array[num4].Contains("TRANSACTION START");
						num = 38;
						num2 = num;
						continue;
					}
					case 25:
						goto IL_43E;
					case 26:
						goto IL_8C4;
					case 27:
						goto IL_597;
					case 28:
					{
						bool flag10;
						if (flag10)
						{
							num = 2;
							num2 = num;
							continue;
						}
						int num5;
						bool flag2 = num5 == 1;
						num = 4;
						num2 = num;
						continue;
					}
					case 29:
						goto IL_3A1;
					case 30:
						goto IL_28E;
					case 31:
					{
						bool flag11;
						if (flag11)
						{
							num = 43;
							num2 = num;
							continue;
						}
						bool flag12 = array[num6].Contains("END");
						num = 59;
						num2 = num;
						continue;
					}
					case 32:
					{
						bool flag13;
						if (flag13)
						{
							num = 47;
							num2 = num;
							continue;
						}
						bool flag7 = brand == 3;
						num = 18;
						num2 = num;
						continue;
					}
					case 33:
					{
						if (!flag14)
						{
							num = 20;
							num2 = num;
							continue;
						}
						num = 0;
						bool flag5 = array[num7].Contains("TRANSACTION STARTED");
						num = 14;
						num2 = num;
						continue;
					}
					case 34:
					{
						bool flag4;
						if (flag4)
						{
							num = 52;
							num2 = num;
							continue;
						}
						bool flag = array[num3].Contains("TRANSACTION END");
						num = 1;
						num2 = num;
						continue;
					}
					case 35:
						text = text + array[num3] + "\n";
						list.Add(new JournalsRawTranx
						{
							filename = filepath,
							filedate = date,
							ip = ipaddress,
							terminalid = tid,
							trandata = text
						});
						text = "";
						num = 22;
						num2 = num;
						continue;
					case 36:
					{
						if (!flag15)
						{
							num = 44;
							num2 = num;
							continue;
						}
						bool flag11 = array[num6].Contains("START");
						num = 31;
						num2 = num;
						continue;
					}
					case 37:
						goto IL_43E;
					case 38:
					{
						bool flag9;
						if (flag9)
						{
							num = 5851;
							int arg_4E4_0 = num;
							num = 5851;
							switch ((arg_4E4_0 == num) ? 1 : 0)
							{
							case 0:
							case 2:
								goto IL_313;
							case 1:
								IL_4F8:
								num = 0;
								if (num != 0)
								{
								}
								num = 13;
								num2 = num;
								continue;
							}
							goto IL_4F8;
						}
						bool flag10 = array[num4].Contains("TRANSACTION END");
						num = 28;
						num2 = num;
						continue;
					}
					case 39:
						goto IL_2AF;
					case 40:
						goto IL_597;
					case 41:
						return result;
					case 42:
						goto IL_597;
					case 43:
						text = text + array[num6] + "\n";
						num = 19;
						num2 = num;
						continue;
					case 44:
						num = 10;
						num2 = num;
						continue;
					case 45:
						text = text + array[num7] + "\n";
						list.Add(new JournalsRawTranx
						{
							filename = filepath,
							filedate = date,
							ip = ipaddress,
							terminalid = tid,
							trandata = text
						});
						text = "";
						num = 30;
						num2 = num;
						continue;
					case 46:
					{
						bool flag16;
						if (flag16)
						{
							num = 6;
							num2 = num;
							continue;
						}
						bool flag13 = brand == 2;
						num = 32;
						num2 = num;
						continue;
					}
					case 47:
						num7 = 0;
						num = 58;
						num2 = num;
						continue;
					case 48:
						goto IL_175;
					case 49:
						goto IL_28E;
					case 50:
					{
						bool flag6;
						if (flag6)
						{
							num = 45;
							num2 = num;
							continue;
						}
						text = text + array[num7] + "\n";
						num = 9;
						num2 = num;
						continue;
					}
					case 51:
						goto IL_3A1;
					case 52:
						text = text + array[num3] + "\n";
						num = 21;
						num2 = num;
						continue;
					case 53:
						text = text + array[num7] + "\n";
						num = 49;
						num2 = num;
						continue;
					case 54:
					{
						if (flag17)
						{
							num = 23;
							num2 = num;
							continue;
						}
						bool flag16 = brand == 1;
						num = 46;
						num2 = num;
						continue;
					}
					case 55:
						goto IL_2AF;
					case 56:
						num = 51;
						num2 = num;
						continue;
					case 57:
						goto IL_47D;
					case 58:
						goto IL_175;
					case 59:
					{
						bool flag12;
						if (flag12)
						{
							num = 15;
							num2 = num;
							continue;
						}
						text = text + array[num6] + "\n";
						num = 39;
						num2 = num;
						continue;
					}
					}
					goto IL_118;
					IL_175:
					flag14 = (num7 < array.Length);
					num = 33;
					num2 = num;
					continue;
					IL_28E:
					num7++;
					num = 48;
					num2 = num;
					continue;
					IL_2AF:
					num6++;
					num = 7;
					num2 = num;
					continue;
					IL_313:
					num = 8;
					num2 = num;
					continue;
					IL_3A1:
					JournalsStore journalsStore = new JournalsStore();
					journalsStore.data = data;
					journalsStore.filedate = "";
					journalsStore.filename = filepath;
					journalsStore.ip = ipaddress;
					journalsStore.terminalid = tid;
					journalsStore.totaltransactions = list.Count;
					result = list;
					num = 41;
					num2 = num;
					continue;
					IL_405:
					num = 40;
					num2 = num;
					continue;
					IL_43E:
					flag3 = (num3 < array.Length);
					num = 12;
					num2 = num;
					continue;
					IL_47D:
					num3++;
					num = 37;
					num2 = num;
					continue;
					IL_597:
					num4++;
					num = 26;
					num2 = num;
					continue;
					IL_8C4:
					flag8 = (num4 < array.Length);
					num = 24;
					num2 = num;
					continue;
					IL_961:
					flag15 = (num6 < array.Length);
					num = 36;
					num2 = num;
				}
				return result;
				IL_118:
				list = new List<JournalsRawTranx>();
				array = File.ReadAllLines(filepath);
				data = File.ReadAllText(filepath);
				int num8 = array.Length;
				text = "";
				flag17 = (brand == 0);
				num = 54;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		private List<OneJournalsRawTranx> GetSubTransactions(int brand, string fulltransaction)
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
					goto IL_E8;
				}
				string[] array;
				int num4;
				string text;
				List<OneJournalsRawTranx> result;
				List<OneJournalsRawTranx> list;
				while (true)
				{
					IL_21:
					int num3;
					bool flag5;
					bool flag6;
					int num5;
					bool flag9;
					bool flag10;
					bool flag11;
					bool arg_7BB_0;
					switch (num2)
					{
					case 0:
						if (brand != 1)
						{
							num = 3;
							num2 = num;
							continue;
						}
						num = 35;
						num2 = num;
						continue;
					case 1:
					{
						bool flag;
						if (flag)
						{
							num = 24;
							num2 = num;
							continue;
						}
						bool flag2 = array[num3].Contains("TRANSACTION END");
						num = 34;
						num2 = num;
						continue;
					}
					case 2:
						goto IL_3C0;
					case 3:
						num = 43;
						num2 = num;
						continue;
					case 4:
					{
						bool flag3;
						if (flag3)
						{
							num = 7;
							num2 = num;
							continue;
						}
						bool flag4 = brand == 3;
						num = 44;
						num2 = num;
						continue;
					}
					case 5:
						if (flag5)
						{
							num = 29;
							num2 = num;
							continue;
						}
						num4++;
						text = text + array[num3] + "\n";
						num = 30;
						num2 = num;
						continue;
					case 6:
					{
						if (!flag6)
						{
							num = 14;
							num2 = num;
							continue;
						}
						bool flag7 = array[num5].Contains("TRANSACTION DATA");
						num = 42;
						num2 = num;
						continue;
					}
					case 7:
						num5 = 0;
						num = 31;
						num2 = num;
						continue;
					case 8:
						goto IL_6BC;
					case 9:
						goto IL_4B7;
					case 10:
						goto IL_4D4;
					case 11:
					{
						bool flag8 = num4 > 1;
						num = 4625;
						int arg_20B_0 = num;
						num = 4625;
						switch ((arg_20B_0 == num) ? 1 : 0)
						{
						case 0:
						case 2:
							goto IL_561;
						case 1:
							IL_21F:
							num = 0;
							if (num != 0)
							{
							}
							num = 13;
							num2 = num;
							continue;
						}
						goto IL_21F;
					}
					case 12:
						num = 9;
						num2 = num;
						continue;
					case 13:
					{
						bool flag8;
						if (flag8)
						{
							num = 47;
							num2 = num;
							continue;
						}
						num4++;
						text = text + array[num5] + "\n";
						num = 8;
						num2 = num;
						continue;
					}
					case 14:
						num = 2;
						num2 = num;
						continue;
					case 15:
						goto IL_4D4;
					case 16:
						return result;
					case 17:
						goto IL_643;
					case 18:
						goto IL_6BC;
					case 19:
						num = 28;
						num2 = num;
						continue;
					case 20:
						num = 0;
						goto IL_8E8;
					case 21:
					{
						if (!flag9)
						{
							num = 37;
							num2 = num;
							continue;
						}
						bool flag = array[num3].Contains("PIN ENTERED");
						num = 1;
						num2 = num;
						continue;
					}
					case 22:
						goto IL_3C0;
					case 23:
						goto IL_69B;
					case 24:
						goto IL_561;
					case 25:
						goto IL_480;
					case 26:
						if (flag10)
						{
							num = 12;
							num2 = num;
							continue;
						}
						goto IL_4B7;
					case 27:
					{
						if (flag11)
						{
							num = 36;
							num2 = num;
							continue;
						}
						bool flag3 = brand == 0;
						num = 4;
						num2 = num;
						continue;
					}
					case 28:
						goto IL_3C0;
					case 29:
						num4++;
						text = text + array[num3] + "\n";
						list.Add(new OneJournalsRawTranx
						{
							filename = this.filePath,
							tran_date = "",
							ip = this.ip,
							terminalid = this.tid,
							trandata = text,
							tsn = ""
						});
						text = "";
						num = 25;
						num2 = num;
						continue;
					case 30:
						goto IL_480;
					case 31:
						goto IL_8E8;
					case 32:
						goto IL_643;
					case 33:
					{
						bool flag12;
						if (flag12)
						{
							num = 41;
							num2 = num;
							continue;
						}
						text = text + array[num5] + "\n";
						num = 46;
						num2 = num;
						continue;
					}
					case 34:
					{
						bool flag2;
						if (flag2)
						{
							num = 39;
							num2 = num;
							continue;
						}
						text = text + array[num3] + "\n";
						num = 40;
						num2 = num;
						continue;
					}
					case 35:
						arg_7BB_0 = true;
						goto IL_7BB;
					case 36:
						num3 = 0;
						num = 10;
						num2 = num;
						continue;
					case 37:
						num = 22;
						num2 = num;
						continue;
					case 38:
						goto IL_3C0;
					case 39:
						text = text + array[num3] + "\n";
						list.Add(new OneJournalsRawTranx
						{
							filename = this.filePath,
							tran_date = "",
							ip = this.ip,
							terminalid = this.tid,
							trandata = text,
							tsn = ""
						});
						text = "";
						num = 23;
						num2 = num;
						continue;
					case 40:
						goto IL_69B;
					case 41:
						text = text + array[num5] + "\n";
						list.Add(new OneJournalsRawTranx
						{
							filename = this.filePath,
							tran_date = "",
							ip = this.ip,
							terminalid = this.tid,
							trandata = text,
							tsn = ""
						});
						text = "";
						num = 17;
						num2 = num;
						continue;
					case 42:
					{
						bool flag7;
						if (flag7)
						{
							num = 11;
							num2 = num;
							continue;
						}
						bool flag12 = array[num5].Contains("TRANSACTION END");
						num = 33;
						num2 = num;
						continue;
					}
					case 43:
						arg_7BB_0 = (brand == 2);
						goto IL_7BB;
					case 44:
					{
						bool flag4;
						if (flag4)
						{
							num = 19;
							num2 = num;
							continue;
						}
						num = 38;
						num2 = num;
						continue;
					}
					case 45:
						goto IL_69B;
					case 46:
						goto IL_643;
					case 47:
						list.Add(new OneJournalsRawTranx
						{
							filename = this.filePath,
							tran_date = "",
							ip = this.ip,
							terminalid = this.tid,
							trandata = text,
							tsn = ""
						});
						text = "";
						num = 18;
						num2 = num;
						continue;
					}
					goto IL_E8;
					IL_3C0:
					flag10 = (list.Count > 1);
					num = 26;
					num2 = num;
					continue;
					IL_480:
					num = 45;
					num2 = num;
					continue;
					IL_4B7:
					result = list;
					num = 16;
					num2 = num;
					continue;
					IL_4D4:
					flag9 = (num3 < array.Length);
					num = 21;
					num2 = num;
					continue;
					IL_561:
					flag5 = (num4 > 0);
					num = 5;
					num2 = num;
					continue;
					IL_643:
					num5++;
					num = 20;
					num2 = num;
					continue;
					IL_69B:
					num3++;
					num = 15;
					num2 = num;
					continue;
					IL_6BC:
					num = 32;
					num2 = num;
					continue;
					IL_7BB:
					flag11 = arg_7BB_0;
					num = 27;
					num2 = num;
					continue;
					IL_8E8:
					flag6 = (num5 < array.Length);
					num = 6;
					num2 = num;
				}
				return result;
				IL_E8:
				list = new List<OneJournalsRawTranx>();
				text = "";
				array = fulltransaction.Split(new char[]
				{
					'\n'
				});
				num4 = 0;
				num = 1;
				if (num != 0)
				{
				}
				num = 0;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		private List<OneJournalsRawTranx> SplitSubTransaction(int brand, string subtransaction)
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
					goto IL_120;
				}
				List<OneJournalsRawTranx> list;
				string text;
				int num4;
				string[] array;
				bool flag;
				List<OneJournalsRawTranx> result;
				while (true)
				{
					IL_21:
					int num3;
					int num5;
					int num6;
					bool flag5;
					bool flag13;
					bool flag16;
					bool flag17;
					switch (num2)
					{
					case 0:
						num3 = 0;
						num = 36;
						num2 = num;
						continue;
					case 1:
						list.Add(new OneJournalsRawTranx
						{
							filename = this.filePath,
							tran_date = "",
							ip = this.ip,
							terminalid = this.tid,
							trandata = text,
							tsn = ""
						});
						text = "";
						num4++;
						text = text + array[num5] + "\n";
						num = 32;
						num2 = num;
						continue;
					case 2:
						text = text + array[num6] + "\n";
						list.Add(new OneJournalsRawTranx
						{
							filename = this.filePath,
							tran_date = "",
							ip = this.ip,
							terminalid = this.tid,
							trandata = text,
							tsn = ""
						});
						num = 3;
						num2 = num;
						continue;
					case 3:
						goto IL_741;
					case 4:
					{
						if (flag)
						{
							num = 0;
							num2 = num;
							continue;
						}
						num = 1;
						if (num != 0)
						{
						}
						bool flag2 = brand == 2;
						num = 8;
						num2 = num;
						continue;
					}
					case 5:
					{
						bool flag3 = num4 > 0;
						num = 59;
						num2 = num;
						continue;
					}
					case 6:
						goto IL_861;
					case 7:
						goto IL_7DB;
					case 8:
					{
						bool flag2;
						if (flag2)
						{
							num = 17;
							num2 = num;
							continue;
						}
						bool flag4 = brand == 0;
						num = 24;
						num2 = num;
						continue;
					}
					case 9:
						goto IL_C0C;
					case 10:
						goto IL_224;
					case 11:
						goto IL_326;
					case 12:
						list.Add(new OneJournalsRawTranx
						{
							filename = this.filePath,
							tran_date = "",
							ip = this.ip,
							terminalid = this.tid,
							trandata = text,
							tsn = ""
						});
						text = "";
						text = text + array[num6] + "\n";
						num = 43;
						num2 = num;
						continue;
					case 13:
						return result;
					case 14:
						num6 = 0;
						num = 61;
						num2 = num;
						continue;
					case 15:
						goto IL_1E5;
					case 16:
						goto IL_861;
					case 17:
						num5 = 0;
						num = 7;
						num2 = num;
						continue;
					case 18:
						if (flag5)
						{
							num = 0;
							num = -8712;
							int arg_9B8_0 = num;
							num = -8712;
							switch ((arg_9B8_0 == num) ? 1 : 0)
							{
							case 0:
							case 2:
								goto IL_3D6;
							case 1:
								IL_9CC:
								num = 0;
								if (num != 0)
								{
								}
								num = 23;
								num2 = num;
								continue;
							}
							goto IL_9CC;
						}
						goto IL_3A1;
					case 19:
					{
						bool flag6;
						if (flag6)
						{
							num = 26;
							num2 = num;
							continue;
						}
						text = text + array[num5] + "\n";
						num = 16;
						num2 = num;
						continue;
					}
					case 20:
						goto IL_7DB;
					case 21:
					{
						bool flag7;
						if (flag7)
						{
							num = 34;
							num2 = num;
							continue;
						}
						bool flag6 = array[num5].Contains("TRANSACTION END");
						num = 19;
						num2 = num;
						continue;
					}
					case 22:
						goto IL_959;
					case 23:
						num = 39;
						num2 = num;
						continue;
					case 24:
					{
						bool flag4;
						if (flag4)
						{
							num = 14;
							num2 = num;
							continue;
						}
						bool flag8 = brand == 3;
						num = 54;
						num2 = num;
						continue;
					}
					case 25:
						goto IL_1C4;
					case 26:
						list.Add(new OneJournalsRawTranx
						{
							filename = this.filePath,
							tran_date = "",
							ip = this.ip,
							terminalid = this.tid,
							trandata = text,
							tsn = ""
						});
						text = "";
						num = 6;
						num2 = num;
						continue;
					case 27:
						goto IL_741;
					case 28:
						list.Add(new OneJournalsRawTranx
						{
							filename = this.filePath,
							tran_date = "",
							ip = this.ip,
							terminalid = this.tid,
							trandata = text,
							tsn = ""
						});
						text = "";
						text = text + array[num3] + "\n";
						num = 33;
						num2 = num;
						continue;
					case 29:
					{
						bool flag9;
						if (flag9)
						{
							num = 5;
							num2 = num;
							continue;
						}
						bool flag10 = array[num3].Contains("TRANSACTION END");
						num = 50;
						num2 = num;
						continue;
					}
					case 30:
						goto IL_959;
					case 31:
					{
						bool flag11;
						if (flag11)
						{
							num = 12;
							num2 = num;
							continue;
						}
						num4++;
						text = text + array[num6] + "\n";
						num = 9;
						num2 = num;
						continue;
					}
					case 32:
						goto IL_3C0;
					case 33:
						goto IL_224;
					case 34:
					{
						bool flag12 = num4 > 0;
						num = 48;
						num2 = num;
						continue;
					}
					case 35:
						goto IL_3C0;
					case 36:
						goto IL_326;
					case 37:
						goto IL_1C4;
					case 38:
						goto IL_959;
					case 39:
						goto IL_3A1;
					case 40:
					{
						if (!flag13)
						{
							num = 56;
							num2 = num;
							continue;
						}
						bool flag14 = array[num6].Contains("TRANSACTION RECORD");
						num = 41;
						num2 = num;
						continue;
					}
					case 41:
					{
						bool flag14;
						if (flag14)
						{
							num = 51;
							num2 = num;
							continue;
						}
						bool flag15 = array[num6].Contains("TRANSACTION END");
						num = 55;
						num2 = num;
						continue;
					}
					case 42:
						num = 38;
						num2 = num;
						continue;
					case 43:
						goto IL_C0C;
					case 44:
						goto IL_741;
					case 45:
						goto IL_959;
					case 46:
					{
						if (!flag16)
						{
							num = 57;
							num2 = num;
							continue;
						}
						bool flag9 = array[num3].Contains("TRANSACTION REQUEST");
						num = 29;
						num2 = num;
						continue;
					}
					case 47:
						num = 30;
						num2 = num;
						continue;
					case 48:
					{
						bool flag12;
						if (flag12)
						{
							num = 1;
							num2 = num;
							continue;
						}
						num4++;
						text = text + array[num5] + "\n";
						num = 35;
						num2 = num;
						continue;
					}
					case 49:
						goto IL_1C4;
					case 50:
					{
						bool flag10;
						if (flag10)
						{
							num = 58;
							num2 = num;
							continue;
						}
						text = text + array[num3] + "\n";
						num = 25;
						num2 = num;
						continue;
					}
					case 51:
					{
						bool flag11 = num4 > 0;
						num = 31;
						num2 = num;
						continue;
					}
					case 52:
						goto IL_959;
					case 53:
					{
						if (!flag17)
						{
							num = 47;
							num2 = num;
							continue;
						}
						bool flag7 = array[num5].Contains("OPCODE");
						num = 21;
						num2 = num;
						continue;
					}
					case 54:
					{
						bool flag8;
						if (flag8)
						{
							num = 42;
							num2 = num;
							continue;
						}
						num = 52;
						num2 = num;
						continue;
					}
					case 55:
					{
						bool flag15;
						if (flag15)
						{
							num = 2;
							num2 = num;
							continue;
						}
						text = text + array[num6] + "\n";
						num = 44;
						num2 = num;
						continue;
					}
					case 56:
						num = 22;
						num2 = num;
						continue;
					case 57:
						num = 45;
						num2 = num;
						continue;
					case 58:
						text = text + array[num3] + "\n";
						list.Add(new OneJournalsRawTranx
						{
							filename = this.filePath,
							tran_date = "",
							ip = this.ip,
							terminalid = this.tid,
							trandata = text,
							tsn = ""
						});
						num = 37;
						num2 = num;
						continue;
					case 59:
					{
						bool flag3;
						if (flag3)
						{
							num = 28;
							num2 = num;
							continue;
						}
						num4++;
						text = text + array[num3] + "\n";
						num = 10;
						num2 = num;
						continue;
					}
					case 60:
						goto IL_3D6;
					case 61:
						goto IL_1E5;
					}
					goto IL_120;
					IL_1C4:
					num3++;
					num = 11;
					num2 = num;
					continue;
					IL_1E5:
					flag13 = (num6 < array.Length);
					num = 40;
					num2 = num;
					continue;
					IL_224:
					num = 49;
					num2 = num;
					continue;
					IL_326:
					flag16 = (num3 < array.Length);
					num = 46;
					num2 = num;
					continue;
					IL_3A1:
					num4 = 0;
					result = list;
					num = 13;
					num2 = num;
					continue;
					IL_3C0:
					num = 60;
					num2 = num;
					continue;
					IL_741:
					num6++;
					num = 15;
					num2 = num;
					continue;
					IL_7DB:
					flag17 = (num5 < array.Length);
					num = 53;
					num2 = num;
					continue;
					IL_861:
					num5++;
					num = 20;
					num2 = num;
					continue;
					IL_3D6:
					goto IL_861;
					IL_959:
					flag5 = (list.Count > 1);
					num = 18;
					num2 = num;
					continue;
					IL_C0C:
					num = 27;
					num2 = num;
				}
				return result;
				IL_120:
				list = new List<OneJournalsRawTranx>();
				text = "";
				array = subtransaction.Split(new char[]
				{
					'\n'
				});
				num4 = 0;
				flag = (brand == 1);
				num = 4;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public List<OneJournalsRawTranx> GetJournalTransactions(string filepath, string date, string ipaddress, string tid, int brand)
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
					goto IL_80;
				}
				List<OneJournalsRawTranx> list;
				List<JournalsRawTranx> list2;
				int num4;
				List<OneJournalsRawTranx> result;
				while (true)
				{
					IL_21:
					List<OneJournalsRawTranx> subTransactions;
					bool flag;
					int num3;
					bool flag2;
					switch (num2)
					{
					case 0:
						list.AddRange(subTransactions);
						num = 16;
						num2 = num;
						continue;
					case 1:
						num = 1;
						if (num != 0)
						{
						}
						goto IL_207;
					case 2:
						list.AddRange(subTransactions);
						num = 15;
						num2 = num;
						continue;
					case 3:
						goto IL_135;
					case 4:
						num = 3;
						num2 = num;
						continue;
					case 5:
						goto IL_207;
					case 6:
					{
						if (!flag)
						{
							num = 4;
							num2 = num;
							continue;
						}
						List<OneJournalsRawTranx> collection = this.SplitSubTransaction(brand, subTransactions[num3].trandata);
						list.AddRange(collection);
						num3++;
						num = 5;
						num2 = num;
						continue;
					}
					case 7:
						num3 = 0;
						num = 1;
						num2 = num;
						continue;
					case 8:
						list.AddRange(subTransactions);
						num = 13;
						num2 = num;
						continue;
					case 9:
					{
						while (true)
						{
							num = 0;
							if (!flag2)
							{
								break;
							}
							num = -29034;
							int arg_2F6_0 = num;
							num = -29034;
							switch ((arg_2F6_0 == num) ? 1 : 0)
							{
							case 0:
							case 2:
								continue;
							}
							goto IL_30A;
						}
						num = 10;
						num2 = num;
						continue;
						IL_30A:
						num = 0;
						if (num != 0)
						{
						}
						subTransactions = this.GetSubTransactions(brand, list2[num4].trandata);
						bool flag3 = brand == 2;
						num = 12;
						num2 = num;
						continue;
					}
					case 10:
						result = list;
						num = 17;
						num2 = num;
						continue;
					case 11:
					{
						bool flag4;
						if (flag4)
						{
							num = 8;
							num2 = num;
							continue;
						}
						bool flag5 = brand == 0;
						num = 20;
						num2 = num;
						continue;
					}
					case 12:
					{
						bool flag3;
						if (flag3)
						{
							num = 7;
							num2 = num;
							continue;
						}
						bool flag4 = brand == 1;
						num = 11;
						num2 = num;
						continue;
					}
					case 13:
						goto IL_135;
					case 14:
						goto IL_246;
					case 15:
						goto IL_135;
					case 16:
						goto IL_135;
					case 17:
						return result;
					case 18:
						goto IL_135;
					case 19:
						goto IL_246;
					case 20:
					{
						bool flag5;
						if (flag5)
						{
							num = 2;
							num2 = num;
							continue;
						}
						bool flag6 = brand == 3;
						num = 21;
						num2 = num;
						continue;
					}
					case 21:
					{
						bool flag6;
						if (flag6)
						{
							num = 0;
							num2 = num;
							continue;
						}
						num = 18;
						num2 = num;
						continue;
					}
					}
					goto IL_80;
					IL_135:
					num4++;
					num = 14;
					num2 = num;
					continue;
					IL_207:
					flag = (num3 < subTransactions.Count);
					num = 6;
					num2 = num;
					continue;
					IL_246:
					flag2 = (num4 < list2.Count);
					num = 9;
					num2 = num;
				}
				return result;
				IL_80:
				list = new List<OneJournalsRawTranx>();
				list2 = this.JournalPreJob(filepath, date, ipaddress, tid, brand);
				num4 = 0;
				num = 19;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string NCRDispenseErrorCodeParser(string transactionblock)
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
					goto IL_48;
				}
				string result;
				string text;
				while (true)
				{
					IL_21:
					bool flag;
					string[] array;
					int num3;
					switch (num2)
					{
					case 0:
						goto IL_1C4;
					case 1:
						goto IL_D7;
					case 2:
						return result;
					case 3:
						goto IL_10D;
					case 4:
						num = 1;
						if (num != 0)
						{
						}
						goto IL_D7;
					case 5:
					{
						if (!flag)
						{
							num = 3;
							num2 = num;
							continue;
						}
						num = 0;
						bool flag2 = array[num3].Contains("E*");
						num = 6;
						num2 = num;
						continue;
					}
					case 6:
					{
						bool flag2;
						if (flag2)
						{
							num = 7;
							num2 = num;
							continue;
						}
						num3++;
						num = 1;
						num2 = num;
						continue;
					}
					case 7:
						text = array[num3].Substring(array[num3].IndexOf('*'));
						num = 0;
						num2 = num;
						continue;
					}
					goto IL_48;
					IL_D7:
					flag = (num3 < array.Length);
					num = 5;
					num2 = num;
				}
				IL_10D:
				IL_185:
				result = text;
				goto IL_188;
				IL_1C4:
				goto IL_185;
				IL_48:
				num = -1151;
				int arg_64_0 = num;
				num = -1151;
				switch ((arg_64_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					IL_188:
					num = 2;
					num2 = num;
					goto IL_21;
				case 1:
				{
					IL_78:
					num = 0;
					if (num != 0)
					{
					}
					text = "";
					string[] array = transactionblock.Split(new char[]
					{
						'\n'
					});
					int num3 = 0;
					num = 4;
					num2 = num;
					goto IL_21;
				}
				}
				goto IL_78;
			}
			}
			goto IL_17;
		}

		public string NCROtherErrorCodeParser(string transactionblock)
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
					goto IL_48;
				}
				string text;
				while (true)
				{
					IL_21:
					bool flag;
					string[] array;
					int num3;
					switch (num2)
					{
					case 0:
						goto IL_1C4;
					case 1:
						goto IL_B9;
					case 2:
						goto IL_17F;
					case 3:
						goto IL_EF;
					case 4:
						goto IL_B9;
					case 5:
					{
						if (!flag)
						{
							num = 3;
							num2 = num;
							continue;
						}
						bool flag2 = array[num3].Contains("*D*");
						num = 0;
						num = 6;
						num2 = num;
						continue;
					}
					case 6:
					{
						bool flag2;
						if (flag2)
						{
							num = 7;
							num2 = num;
							continue;
						}
						num3++;
						num = 1;
						num2 = num;
						continue;
					}
					case 7:
						text = array[num3].Substring(array[num3].IndexOf('*'));
						num = 0;
						num2 = num;
						continue;
					}
					goto IL_48;
					IL_B9:
					flag = (num3 < array.Length);
					num = 5;
					num2 = num;
				}
				IL_EF:
				IL_167:
				string result = text;
				goto IL_16A;
				IL_17F:
				num = 1;
				if (num != 0)
				{
				}
				return result;
				IL_1C4:
				goto IL_167;
				IL_48:
				num = 16977;
				int arg_64_0 = num;
				num = 16977;
				switch ((arg_64_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					IL_16A:
					num = 2;
					num2 = num;
					goto IL_21;
				case 1:
				{
					IL_78:
					num = 0;
					if (num != 0)
					{
					}
					text = "";
					string[] array = transactionblock.Split(new char[]
					{
						'\n'
					});
					int num3 = 0;
					num = 4;
					num2 = num;
					goto IL_21;
				}
				}
				goto IL_78;
			}
			}
			goto IL_17;
		}

		public string ProcessNCRDispenseError(string transactionblock)
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
					goto IL_AC;
				}
				string result;
				string text;
				string text2;
				while (true)
				{
					IL_21:
					bool arg_269_0;
					bool arg_4A5_0;
					bool flag;
					bool arg_343_0;
					bool arg_541_0;
					bool flag2;
					bool flag3;
					bool flag4;
					switch (num2)
					{
					case 0:
						arg_269_0 = false;
						goto IL_269;
					case 1:
						return result;
					case 2:
						return result;
					case 3:
						text = "Indeterminate";
						num = 13;
						num2 = num;
						continue;
					case 4:
						arg_4A5_0 = false;
						goto IL_4A5;
					case 5:
						num = 6;
						num2 = num;
						continue;
					case 6:
					{
						string a;
						arg_269_0 = (a != "00");
						goto IL_269;
					}
					case 7:
						text = "Failed (Dispense Error)";
						num = 19;
						num2 = num;
						continue;
					case 8:
						if (flag)
						{
							num = 31;
							num2 = num;
							continue;
						}
						result = "";
						num = 2;
						num2 = num;
						continue;
					case 9:
					{
						string a;
						arg_343_0 = (a == "00");
						goto IL_343;
					}
					case 10:
						num = 11;
						num2 = num;
						continue;
					case 11:
						arg_541_0 = (text2 != null);
						goto IL_541;
					case 12:
						arg_541_0 = false;
						goto IL_541;
					case 13:
						goto IL_48D;
					case 14:
						if (text2 != "")
						{
							num = 10;
							num2 = num;
							continue;
						}
						num = 12;
						num2 = num;
						continue;
					case 15:
						if (flag2)
						{
							num = 7;
							num2 = num;
							continue;
						}
						goto IL_37B;
					case 16:
					{
						string a2;
						if (a2 == "2")
						{
							num = 32;
							num2 = num;
							continue;
						}
						num = 4;
						num2 = num;
						continue;
					}
					case 17:
						if (flag3)
						{
							num = 28;
							num2 = num;
							continue;
						}
						goto IL_2C4;
					case 18:
						goto IL_2C4;
					case 19:
						goto IL_37B;
					case 20:
					{
						string a2;
						if (a2 == "0")
						{
							num = 23;
							num2 = num;
							continue;
						}
						num = 26;
						num2 = num;
						continue;
					}
					case 21:
						goto IL_454;
					case 22:
					{
						num = 1;
						if (num != 0)
						{
						}
						string a2 = text2.Substring(text2.IndexOf("E*") + 2, 1);
						string text3 = text2.Substring(text2.IndexOf("E*") + 3, 8);
						string a = text2.Substring(text2.IndexOf("M-") + 2, 2);
						string text4 = text2.Substring(text2.IndexOf("R-") + 2, 5);
						num = 20;
						num2 = num;
						continue;
					}
					case 23:
						num = 9;
						num2 = num;
						continue;
					case 24:
					{
						string a2;
						if (a2 == "2")
						{
							num = 5;
							num2 = num;
							continue;
						}
						num = 0;
						num2 = num;
						continue;
					}
					case 25:
						if (flag4)
						{
							num = 3;
							num2 = num;
							continue;
						}
						goto IL_48D;
					case 26:
						arg_343_0 = false;
						goto IL_343;
					case 27:
					{
						bool flag5;
						if (flag5)
						{
							num = 22;
							num2 = num;
							continue;
						}
						text = "Indeterminate";
						num = 29;
						num2 = num;
						continue;
					}
					case 28:
						text = "Successful";
						num = 18;
						num2 = num;
						continue;
					case 29:
						goto IL_454;
					case 30:
					{
						string a;
						arg_4A5_0 = (a == "00");
						goto IL_4A5;
					}
					case 31:
					{
						bool flag5 = text2.Contains("E*");
						num = 27;
						num2 = num;
						continue;
					}
					case 32:
						num = 30;
						num2 = num;
						continue;
					}
					goto IL_AC;
					IL_269:
					flag2 = arg_269_0;
					num = 15;
					num2 = num;
					continue;
					IL_2C4:
					num = -7460;
					int arg_2E0_0 = num;
					num = -7460;
					switch ((arg_2E0_0 == num) ? 1 : 0)
					{
					case 0:
					case 2:
						IL_345:
						num = 17;
						num2 = num;
						continue;
					case 1:
						IL_2F4:
						num = 0;
						if (num != 0)
						{
						}
						num = 24;
						num2 = num;
						continue;
					}
					goto IL_2F4;
					IL_343:
					flag3 = arg_343_0;
					goto IL_345;
					IL_37B:
					num = 16;
					num2 = num;
					continue;
					IL_454:
					num = 0;
					result = text;
					num = 1;
					num2 = num;
					continue;
					IL_48D:
					num = 21;
					num2 = num;
					continue;
					IL_4A5:
					flag4 = arg_4A5_0;
					num = 25;
					num2 = num;
					continue;
					IL_541:
					flag = arg_541_0;
					num = 8;
					num2 = num;
				}
				return result;
				IL_AC:
				text = "";
				text2 = this.NCRDispenseErrorCodeParser(transactionblock);
				num = 14;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string ReFormatDate(string date)
		{
			switch (0)
			{
			case 0:
				goto IL_20;
			}
			int num;
			string result;
			bool flag;
			int num2;
			string[] array;
			while (true)
			{
				IL_0A:
				switch (num)
				{
				case 0:
					return result;
				case 1:
					goto IL_6C;
				case 2:
					if (flag)
					{
						num2 = 3;
						num = num2;
						continue;
					}
					goto IL_6C;
				case 3:
				{
					array[2] = "20" + array[2];
					num2 = -4864;
					int arg_C3_0 = num2;
					num2 = -4864;
					switch ((arg_C3_0 == num2) ? 1 : 0)
					{
					case 0:
					case 2:
						return result;
					case 1:
						IL_D7:
						num2 = 0;
						num2 = 1;
						if (num2 != 0)
						{
						}
						num2 = 0;
						if (num2 != 0)
						{
						}
						num2 = 1;
						num = num2;
						continue;
					}
					goto IL_D7;
				}
				}
				goto IL_20;
				IL_6C:
				result = string.Format("{0}/{1}/{2}", array[1], array[0], array[2]);
				num2 = 0;
				num = num2;
			}
			return result;
			IL_20:
			array = date.Split(new char[]
			{
				'/',
				'-',
				'\\'
			});
			flag = (array[2].Length < 4);
			num2 = 2;
			num = num2;
			goto IL_0A;
		}

		public DateTime GetDateTime(string date, string time)
		{
			int num;
			DateTime result;
			while (true)
			{
				DateTime dateTime = DateTime.MinValue;
				try
				{
					date = this.ReFormatDate(date);
					string text = date.Trim().Replace('\\', '/').Replace('-', '/') + " " + time.Trim();
					DateTime dateTime2 = DateTime.ParseExact(text.Trim(), "dd/MM/yyyy HH:mm", Thread.CurrentThread.CurrentCulture, DateTimeStyles.AssumeLocal);
					string text2 = dateTime2.ToString();
					dateTime = dateTime2;
					goto IL_0F;
				}
				catch (Exception var_4_71)
				{
					goto IL_0F;
				}
				IL_77:
				num = 0;
				num = -11775;
				int arg_AF_0 = num;
				num = -11775;
				switch ((arg_AF_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					continue;
				}
				break;
				IL_0F:
				result = dateTime;
				goto IL_77;
			}
			num = 1;
			if (num != 0)
			{
			}
			num = 0;
			if (num != 0)
			{
			}
			return result;
		}

		public string GetWithdrawal(string transactionblock)
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
					goto IL_64;
				}
				string result;
				string text2;
				string[] array;
				int num3;
				while (true)
				{
					IL_21:
					bool arg_298_0;
					bool flag;
					switch (num2)
					{
					case 0:
						goto IL_22C;
					case 1:
						return result;
					case 2:
						goto IL_243;
					case 3:
						arg_298_0 = true;
						goto IL_298;
					case 4:
					{
						string text;
						arg_298_0 = text.StartsWith("GHS");
						goto IL_298;
					}
					case 5:
					{
						num = 10433;
						int arg_EF_0 = num;
						num = 10433;
						switch ((arg_EF_0 == num) ? 1 : 0)
						{
						case 0:
						case 2:
							goto IL_2AF;
						case 1:
						{
							IL_103:
							num = 0;
							if (num != 0)
							{
							}
							string text;
							text2 = text.Substring(3);
							num = 0;
							num2 = num;
							continue;
						}
						}
						goto IL_103;
					}
					case 6:
					{
						string text;
						if (!text.StartsWith("NGN"))
						{
							num = 11;
							num2 = num;
							continue;
						}
						num = 3;
						num2 = num;
						continue;
					}
					case 7:
						goto IL_243;
					case 8:
						goto IL_194;
					case 9:
						goto IL_2AF;
					case 10:
					{
						if (!flag)
						{
							num = 7;
							num2 = num;
							continue;
						}
						num = 0;
						bool flag2 = array[num3].Contains("WITHDRAW");
						num = 14;
						num2 = num;
						continue;
					}
					case 11:
						num = 4;
						num2 = num;
						continue;
					case 12:
					{
						string text = array[num3].Split(new string[]
						{
							"WITHDRAW"
						}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
						num = 6;
						num2 = num;
						continue;
					}
					case 13:
						num = 1;
						if (num != 0)
						{
						}
						goto IL_194;
					case 14:
					{
						bool flag2;
						if (flag2)
						{
							num = 12;
							num2 = num;
							continue;
						}
						num3++;
						num = 13;
						num2 = num;
						continue;
					}
					}
					goto IL_64;
					IL_194:
					flag = (num3 < array.Length);
					num = 10;
					num2 = num;
					continue;
					IL_22C:
					num = 2;
					num2 = num;
					continue;
					IL_2AF:
					bool flag3;
					if (flag3)
					{
						num = 5;
						num2 = num;
						continue;
					}
					goto IL_22C;
					IL_243:
					result = text2;
					num = 1;
					num2 = num;
					continue;
					IL_298:
					flag3 = arg_298_0;
					num = 9;
					num2 = num;
				}
				return result;
				IL_64:
				text2 = "";
				array = transactionblock.Split(new char[]
				{
					'\n'
				});
				num3 = 0;
				num = 8;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string GetInquiry(string transactionblock)
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
					goto IL_48;
				}
				string[] array;
				int num3;
				string text;
				string result;
				while (true)
				{
					IL_21:
					bool flag;
					switch (num2)
					{
					case 0:
					{
						if (!flag)
						{
							num = 3;
							num2 = num;
							continue;
						}
						bool flag2 = array[num3].Contains("INQUIRY");
						num = 4;
						num2 = num;
						continue;
					}
					case 1:
						goto IL_77;
					case 2:
						goto IL_77;
					case 3:
						goto IL_121;
					case 4:
					{
						bool flag2;
						if (flag2)
						{
							num = 6;
							num2 = num;
							continue;
						}
						goto IL_C7;
					}
					case 5:
					{
						num = 24128;
						int arg_176_0 = num;
						num = 24128;
						switch ((arg_176_0 == num) ? 1 : 0)
						{
						case 0:
						case 2:
							goto IL_C7;
						case 1:
							IL_18A:
							num = 1;
							if (num != 0)
							{
							}
							num = 0;
							if (num != 0)
							{
							}
							goto IL_121;
						}
						goto IL_18A;
					}
					case 6:
						text = "INQUIRY";
						num = 5;
						num2 = num;
						continue;
					case 7:
						return result;
					}
					goto IL_48;
					IL_77:
					num = 0;
					flag = (num3 < array.Length);
					num = 0;
					num2 = num;
					continue;
					IL_C7:
					num3++;
					num = 1;
					num2 = num;
					continue;
					IL_121:
					result = text;
					num = 7;
					num2 = num;
				}
				return result;
				IL_48:
				text = "";
				array = transactionblock.Split(new char[]
				{
					'\n'
				});
				num3 = 0;
				num = 2;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string GetTransfer(string transactionblock)
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
					goto IL_70;
				}
				string result;
				string text2;
				string[] array;
				int num3;
				while (true)
				{
					IL_21:
					bool flag;
					string text;
					bool arg_232_0;
					bool flag3;
					switch (num2)
					{
					case 0:
						num = 1;
						if (num != 0)
						{
						}
						goto IL_215;
					case 1:
						goto IL_123;
					case 2:
						goto IL_215;
					case 3:
						num = 6;
						num2 = num;
						continue;
					case 4:
						return result;
					case 5:
					{
						if (flag)
						{
							num = 14;
							num2 = num;
							continue;
						}
						bool flag2 = text.StartsWith("N");
						num = 12;
						num2 = num;
						continue;
					}
					case 6:
						arg_232_0 = text.StartsWith("GHS");
						goto IL_232;
					case 7:
						if (!text.StartsWith("NGN"))
						{
							num = 3;
							num2 = num;
							continue;
						}
						num = 10;
						num2 = num;
						continue;
					case 8:
						text2 = text.Substring(1).Replace(")", "").Trim();
						num = 13;
						num2 = num;
						continue;
					case 9:
					{
						if (!flag3)
						{
							num = 0;
							num2 = num;
							continue;
						}
						num = 0;
						bool flag4 = array[num3].Contains("INTERBANK TRANSFER");
						num = 16;
						num2 = num;
						continue;
					}
					case 10:
						arg_232_0 = true;
						goto IL_232;
					case 11:
						text = array[num3].Split(new string[]
						{
							"INTERBANK TRANSFER"
						}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
						num = 7;
						num2 = num;
						continue;
					case 12:
					{
						bool flag2;
						if (flag2)
						{
							num = 8;
							num2 = num;
							continue;
						}
						goto IL_A2;
					}
					case 13:
						goto IL_A2;
					case 14:
						goto IL_328;
					case 15:
						goto IL_A2;
					case 16:
					{
						bool flag4;
						if (flag4)
						{
							num = 11;
							num2 = num;
							continue;
						}
						num3++;
						num = 17;
						num2 = num;
						continue;
					}
					case 17:
					{
						num = 8653;
						int arg_300_0 = num;
						num = 8653;
						switch ((arg_300_0 == num) ? 1 : 0)
						{
						case 0:
						case 2:
							goto IL_328;
						case 1:
							IL_314:
							num = 0;
							if (num != 0)
							{
							}
							goto IL_123;
						}
						goto IL_314;
					}
					}
					goto IL_70;
					IL_A2:
					num = 2;
					num2 = num;
					continue;
					IL_123:
					flag3 = (num3 < array.Length);
					num = 9;
					num2 = num;
					continue;
					IL_215:
					result = text2;
					num = 4;
					num2 = num;
					continue;
					IL_232:
					flag = arg_232_0;
					num = 5;
					num2 = num;
					continue;
					IL_328:
					text2 = text.Substring(3).Replace(")", "").Trim();
					num = 15;
					num2 = num;
				}
				return result;
				IL_70:
				text2 = "";
				array = transactionblock.Split(new char[]
				{
					'\n'
				});
				num3 = 0;
				num = 1;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string GetAvailableBalance(string transactionblock, int brand)
		{
			int num = 1;
			if (num != 0)
			{
			}
			num = 0;
			int num2 = num;
			switch (num2)
			{
			case 0:
			{
				IL_35:
				num = -18823;
				int arg_51_0 = num;
				num = -18823;
				string result;
				switch ((arg_51_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
					goto IL_840;
				case 1:
				{
					IL_65:
					num = 0;
					if (num != 0)
					{
					}
					string text = "";
					try
					{
						switch (0)
						{
						case 0:
							goto IL_162;
						}
						string[] array;
						while (true)
						{
							IL_8F:
							bool arg_632_0;
							int num3;
							int num4;
							bool flag6;
							bool flag8;
							bool flag9;
							bool arg_5AD_0;
							bool arg_766_0;
							bool flag10;
							bool flag11;
							switch (num2)
							{
							case 0:
								arg_632_0 = true;
								goto IL_632;
							case 1:
							{
								string text2;
								text = text2.Substring(3).Replace(")", "").Trim();
								num = 29;
								num2 = num;
								continue;
							}
							case 2:
							{
								string text3;
								if (!text3.StartsWith("NGN"))
								{
									num = 46;
									num2 = num;
									continue;
								}
								num = 0;
								num2 = num;
								continue;
							}
							case 3:
							{
								string text3;
								arg_632_0 = text3.StartsWith("GHS");
								goto IL_632;
							}
							case 4:
								num3 = 0;
								num = 9;
								num2 = num;
								continue;
							case 5:
								goto IL_302;
							case 6:
								goto IL_718;
							case 7:
								if (!array[num3].Contains("AVAIL.BA"))
								{
									num = 15;
									num2 = num;
									continue;
								}
								goto IL_2E7;
							case 8:
							{
								string text3 = array[num3].Split(new string[]
								{
									"AVAIL.BA",
									"AVAIL  ",
									"AVAILABL "
								}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
								num = 2;
								num2 = num;
								continue;
							}
							case 9:
								goto IL_2A9;
							case 10:
							{
								string text2 = array[num4].Split(new string[]
								{
									"AVAIL "
								}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
								bool flag = text2.StartsWith("NGN");
								num = 23;
								num2 = num;
								continue;
							}
							case 11:
							{
								bool flag2;
								if (flag2)
								{
									num = 10;
									num2 = num;
									continue;
								}
								num4++;
								num = 5;
								num2 = num;
								continue;
							}
							case 12:
								goto IL_302;
							case 13:
							{
								bool flag3;
								if (flag3)
								{
									num = 49;
									num2 = num;
									continue;
								}
								num = 21;
								num2 = num;
								continue;
							}
							case 14:
							{
								bool flag4;
								if (flag4)
								{
									num = 27;
									num2 = num;
									continue;
								}
								goto IL_1AB;
							}
							case 15:
								num = 31;
								num2 = num;
								continue;
							case 16:
							{
								bool flag5;
								if (flag5)
								{
									num = 19;
									num2 = num;
									continue;
								}
								goto IL_733;
							}
							case 17:
								goto IL_2A9;
							case 18:
								goto IL_6DE;
							case 19:
							{
								string text3;
								text = text3.Substring(1).Replace(")", "").Trim();
								num = 28;
								num2 = num;
								continue;
							}
							case 20:
								goto IL_1AB;
							case 21:
								goto IL_81C;
							case 22:
								num4 = 0;
								num = 12;
								num2 = num;
								continue;
							case 23:
							{
								bool flag;
								if (flag)
								{
									num = 1;
									num2 = num;
									continue;
								}
								string text2;
								bool flag4 = text2.StartsWith("N");
								num = 14;
								num2 = num;
								continue;
							}
							case 24:
								goto IL_832;
							case 25:
								goto IL_6DE;
							case 26:
								goto IL_718;
							case 27:
							{
								string text2;
								text = text2.Substring(1).Replace(")", "").Trim();
								num = 20;
								num2 = num;
								continue;
							}
							case 28:
								goto IL_733;
							case 29:
								goto IL_1AB;
							case 30:
								goto IL_81C;
							case 31:
								if (!array[num3].Contains("AVAIL  "))
								{
									num = 34;
									num2 = num;
									continue;
								}
								goto IL_2E7;
							case 32:
								if (!flag6)
								{
									num = 18;
									num2 = num;
									continue;
								}
								num = 7;
								num2 = num;
								continue;
							case 33:
							{
								bool flag7;
								if (flag7)
								{
									num = 22;
									num2 = num;
									continue;
								}
								bool flag3 = brand == 3;
								num = 13;
								num2 = num;
								continue;
							}
							case 34:
								num = 43;
								num2 = num;
								continue;
							case 35:
							{
								string text3;
								text = text3.Substring(3).Replace(")", "").Trim();
								num = 42;
								num2 = num;
								continue;
							}
							case 36:
								goto IL_81C;
							case 37:
							{
								if (flag8)
								{
									num = 4;
									num2 = num;
									continue;
								}
								bool flag7 = brand == 0;
								num = 33;
								num2 = num;
								continue;
							}
							case 38:
								if (flag9)
								{
									num = 8;
									num2 = num;
									continue;
								}
								num3++;
								num = 17;
								num2 = num;
								continue;
							case 39:
								arg_5AD_0 = true;
								goto IL_5AD;
							case 40:
								num = 48;
								num2 = num;
								continue;
							case 41:
								arg_766_0 = true;
								goto IL_766;
							case 42:
								goto IL_733;
							case 43:
								arg_5AD_0 = array[num3].Contains("AVAILABL ");
								goto IL_5AD;
							case 44:
								goto IL_81C;
							case 45:
							{
								if (!flag10)
								{
									num = 26;
									num2 = num;
									continue;
								}
								bool flag2 = array[num4].Contains("AVAIL ");
								num = 11;
								num2 = num;
								continue;
							}
							case 46:
								num = 3;
								num2 = num;
								continue;
							case 47:
								if (brand != 1)
								{
									num = 40;
									num2 = num;
									continue;
								}
								num = 41;
								num2 = num;
								continue;
							case 48:
								arg_766_0 = (brand == 2);
								goto IL_766;
							case 49:
								num = 36;
								num2 = num;
								continue;
							case 50:
							{
								if (flag11)
								{
									num = 35;
									num2 = num;
									continue;
								}
								string text3;
								bool flag5 = text3.StartsWith("N");
								num = 16;
								num2 = num;
								continue;
							}
							}
							goto IL_162;
							IL_1AB:
							num = 6;
							num2 = num;
							continue;
							IL_2A9:
							flag6 = (num3 < array.Length);
							num = 32;
							num2 = num;
							continue;
							IL_2E7:
							num = 39;
							num2 = num;
							continue;
							IL_302:
							flag10 = (num4 < array.Length);
							num = 45;
							num2 = num;
							continue;
							IL_5AD:
							flag9 = arg_5AD_0;
							num = 38;
							num2 = num;
							continue;
							IL_632:
							flag11 = arg_632_0;
							num = 50;
							num2 = num;
							continue;
							IL_6DE:
							num = 30;
							num2 = num;
							continue;
							IL_718:
							num = 44;
							num2 = num;
							continue;
							IL_733:
							num = 25;
							num2 = num;
							continue;
							IL_766:
							flag8 = arg_766_0;
							num = 37;
							num2 = num;
							continue;
							IL_81C:
							num = 24;
							num2 = num;
						}
						IL_832:
						goto IL_7D;
						IL_162:
						array = transactionblock.Split(new char[]
						{
							'\n'
						});
						num = 47;
						num2 = num;
						goto IL_8F;
					}
					catch (Exception var_17_837)
					{
						goto IL_7D;
					}
					goto IL_840;
					IL_7D:
					result = text;
					goto IL_840;
				}
				}
				goto IL_65;
				IL_840:
				num = 0;
				return result;
			}
			}
			goto IL_35;
		}

		public string GetCurrentLedgerBalance(string transactionblock, int brand)
		{
			int num = 0;
			num = 0;
			int num2 = num;
			switch (num2)
			{
			case 0:
			{
				IL_33:
				string text = "";
				try
				{
					num = 19738;
					int arg_60_0 = num;
					num = 19738;
					switch ((arg_60_0 == num) ? 1 : 0)
					{
					case 0:
					case 2:
						IL_5C5:
						num = 46;
						num2 = num;
						goto IL_8D;
					case 1:
						IL_74:
						num = 0;
						if (num != 0)
						{
						}
						switch (0)
						{
						case 0:
							goto IL_160;
						}
						goto IL_8D;
					}
					goto IL_74;
					string[] array;
					while (true)
					{
						IL_8D:
						bool arg_782_0;
						int num3;
						bool flag2;
						int num4;
						bool arg_5A7_0;
						bool flag4;
						bool flag7;
						bool flag9;
						bool flag10;
						bool arg_64E_0;
						switch (num2)
						{
						case 0:
							goto IL_74F;
						case 1:
							if (brand != 1)
							{
								num = 50;
								num2 = num;
								continue;
							}
							num = 2;
							num2 = num;
							continue;
						case 2:
							arg_782_0 = true;
							goto IL_782;
						case 3:
							num = 38;
							num2 = num;
							continue;
						case 4:
							goto IL_2A7;
						case 5:
						{
							bool flag;
							if (flag)
							{
								num = 25;
								num2 = num;
								continue;
							}
							num3++;
							num = 36;
							num2 = num;
							continue;
						}
						case 6:
							if (flag2)
							{
								goto IL_5C5;
							}
							num4++;
							num = 4;
							num2 = num;
							continue;
						case 7:
							goto IL_838;
						case 8:
							goto IL_734;
						case 9:
							num = 16;
							num2 = num;
							continue;
						case 10:
							goto IL_1A9;
						case 11:
							num = 19;
							num2 = num;
							continue;
						case 12:
							arg_782_0 = (brand == 2);
							goto IL_782;
						case 13:
							num3 = 0;
							num = 18;
							num2 = num;
							continue;
						case 14:
							goto IL_838;
						case 15:
							goto IL_74F;
						case 16:
							goto IL_838;
						case 17:
						{
							string text2;
							text = text2.Substring(1).Replace(")", "").Trim();
							num = 0;
							num2 = num;
							continue;
						}
						case 18:
							goto IL_300;
						case 19:
							arg_5A7_0 = array[num4].Contains("CURRENT");
							goto IL_5A7;
						case 20:
						{
							bool flag3;
							if (flag3)
							{
								num = 9;
								num2 = num;
								continue;
							}
							num = 42;
							num2 = num;
							continue;
						}
						case 21:
							goto IL_2A7;
						case 22:
						{
							if (flag4)
							{
								num = 39;
								num2 = num;
								continue;
							}
							bool flag5 = brand == 0;
							num = 31;
							num2 = num;
							continue;
						}
						case 23:
							if (!array[num4].Contains("CURR. BA"))
							{
								num = 44;
								num2 = num;
								continue;
							}
							goto IL_2E5;
						case 24:
							goto IL_734;
						case 25:
						{
							string text3 = array[num3].Split(new string[]
							{
								"LEDGER"
							}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
							bool flag6 = text3.StartsWith("NGN");
							num = 30;
							num2 = num;
							continue;
						}
						case 26:
							goto IL_84E;
						case 27:
						{
							if (!flag7)
							{
								num = 24;
								num2 = num;
								continue;
							}
							bool flag = array[num3].Contains("LEDGER");
							num = 5;
							num2 = num;
							continue;
						}
						case 28:
							goto IL_1A9;
						case 29:
						{
							string text3;
							text = text3.Substring(3).Replace(")", "").Trim();
							num = 28;
							num2 = num;
							continue;
						}
						case 30:
						{
							bool flag6;
							if (flag6)
							{
								num = 29;
								num2 = num;
								continue;
							}
							string text3;
							bool flag8 = text3.StartsWith("N");
							num = 32;
							num2 = num;
							continue;
						}
						case 31:
						{
							bool flag5;
							if (flag5)
							{
								num = 13;
								num2 = num;
								continue;
							}
							bool flag3 = brand == 3;
							num = 20;
							num2 = num;
							continue;
						}
						case 32:
						{
							bool flag8;
							if (flag8)
							{
								num = 37;
								num2 = num;
								continue;
							}
							goto IL_1A9;
						}
						case 33:
							if (!flag9)
							{
								num = 34;
								num2 = num;
								continue;
							}
							num = 23;
							num2 = num;
							continue;
						case 34:
							goto IL_6FA;
						case 35:
						{
							if (flag10)
							{
								num = 41;
								num2 = num;
								continue;
							}
							string text2;
							bool flag11 = text2.StartsWith("N");
							num = 48;
							num2 = num;
							continue;
						}
						case 36:
							goto IL_300;
						case 37:
						{
							string text3;
							text = text3.Substring(1).Replace(")", "").Trim();
							num = 10;
							num2 = num;
							continue;
						}
						case 38:
						{
							string text2;
							arg_64E_0 = text2.StartsWith("GHS");
							goto IL_64E;
						}
						case 39:
							num4 = 0;
							num = 21;
							num2 = num;
							continue;
						case 40:
							arg_64E_0 = true;
							goto IL_64E;
						case 41:
						{
							string text2;
							text = text2.Substring(3).Replace(")", "").Trim();
							num = 15;
							num2 = num;
							continue;
						}
						case 42:
							goto IL_838;
						case 43:
							if (!array[num4].Contains("LEDGER"))
							{
								num = 11;
								num2 = num;
								continue;
							}
							goto IL_2E5;
						case 44:
							num = 43;
							num2 = num;
							continue;
						case 45:
							arg_5A7_0 = true;
							goto IL_5A7;
						case 46:
						{
							num = 1;
							if (num != 0)
							{
							}
							string text2 = array[num4].Split(new string[]
							{
								"CURR. BA",
								"LEDGER",
								"CURRENT"
							}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
							num = 47;
							num2 = num;
							continue;
						}
						case 47:
						{
							string text2;
							if (!text2.StartsWith("NGN"))
							{
								num = 3;
								num2 = num;
								continue;
							}
							num = 40;
							num2 = num;
							continue;
						}
						case 48:
						{
							bool flag11;
							if (flag11)
							{
								num = 17;
								num2 = num;
								continue;
							}
							goto IL_74F;
						}
						case 49:
							goto IL_6FA;
						case 50:
							num = 12;
							num2 = num;
							continue;
						}
						goto IL_160;
						IL_1A9:
						num = 8;
						num2 = num;
						continue;
						IL_2A7:
						flag9 = (num4 < array.Length);
						num = 33;
						num2 = num;
						continue;
						IL_2E5:
						num = 45;
						num2 = num;
						continue;
						IL_300:
						flag7 = (num3 < array.Length);
						num = 27;
						num2 = num;
						continue;
						IL_5A7:
						flag2 = arg_5A7_0;
						num = 6;
						num2 = num;
						continue;
						IL_64E:
						flag10 = arg_64E_0;
						num = 35;
						num2 = num;
						continue;
						IL_6FA:
						num = 14;
						num2 = num;
						continue;
						IL_734:
						num = 7;
						num2 = num;
						continue;
						IL_74F:
						num = 49;
						num2 = num;
						continue;
						IL_782:
						flag4 = arg_782_0;
						num = 22;
						num2 = num;
						continue;
						IL_838:
						num = 26;
						num2 = num;
					}
					IL_84E:
					goto IL_3C;
					IL_160:
					array = transactionblock.Split(new char[]
					{
						'\n'
					});
					num = 1;
					num2 = num;
					goto IL_8D;
				}
				catch (Exception var_17_853)
				{
					goto IL_3C;
				}
				string result;
				return result;
				IL_3C:
				result = text;
				return result;
			}
			}
			goto IL_33;
		}

		public string GetSurcharge(string transactionblock, int brand)
		{
			int num = 0;
			num = 1;
			if (num != 0)
			{
			}
			num = 0;
			int num2 = num;
			switch (num2)
			{
			case 0:
			{
				IL_51:
				string text = "";
				try
				{
					switch (0)
					{
					case 0:
						goto IL_EA;
					}
					string[] array;
					int num3;
					while (true)
					{
						IL_6C:
						num = -25265;
						int arg_88_0 = num;
						num = -25265;
						bool flag;
						switch ((arg_88_0 == num) ? 1 : 0)
						{
						case 0:
						case 2:
							goto IL_24F;
						case 1:
						{
							IL_9C:
							num = 0;
							if (num != 0)
							{
							}
							bool flag4;
							switch (num2)
							{
							case 0:
								goto IL_2DF;
							case 1:
								goto IL_2F5;
							case 2:
								goto IL_18F;
							case 3:
								goto IL_179;
							case 4:
								goto IL_2DF;
							case 5:
								goto IL_18F;
							case 6:
							{
								string text2 = array[num3].Split(new string[]
								{
									"SURCHARGE:"
								}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
								flag = text2.StartsWith("NGN");
								num = 10;
								num2 = num;
								continue;
							}
							case 7:
								goto IL_179;
							case 8:
							{
								bool flag2;
								if (flag2)
								{
									num = 6;
									num2 = num;
									continue;
								}
								num3++;
								num = 5;
								num2 = num;
								continue;
							}
							case 9:
							{
								string text2;
								text = text2.Substring(3).Replace(")", "").Trim();
								num = 3;
								num2 = num;
								continue;
							}
							case 10:
								goto IL_24F;
							case 11:
							{
								string text2;
								text = text2.Substring(1).Replace(")", "").Trim();
								num = 7;
								num2 = num;
								continue;
							}
							case 12:
							{
								bool flag3;
								if (flag3)
								{
									num = 11;
									num2 = num;
									continue;
								}
								goto IL_179;
							}
							case 13:
							{
								if (!flag4)
								{
									num = 4;
									num2 = num;
									continue;
								}
								bool flag2 = array[num3].Contains("SURCHARGE:");
								num = 8;
								num2 = num;
								continue;
							}
							}
							goto IL_EA;
							IL_179:
							num = 0;
							num2 = num;
							continue;
							IL_18F:
							flag4 = (num3 < array.Length);
							num = 13;
							num2 = num;
							continue;
							IL_2DF:
							num = 1;
							num2 = num;
							continue;
						}
						}
						goto IL_9C;
						IL_24F:
						if (flag)
						{
							num = 9;
							num2 = num;
						}
						else
						{
							string text2;
							bool flag3 = text2.StartsWith("N");
							num = 12;
							num2 = num;
						}
					}
					IL_2F5:
					goto IL_5A;
					IL_EA:
					array = transactionblock.Split(new char[]
					{
						'\n'
					});
					num3 = 0;
					num = 2;
					num2 = num;
					goto IL_6C;
				}
				catch (Exception var_8_2FA)
				{
					goto IL_5A;
				}
				string result;
				return result;
				IL_5A:
				result = text;
				return result;
			}
			}
			goto IL_51;
		}

		public string GetTransactionComment(string transactionblock)
		{
			int num = 0;
			int num2 = num;
			switch (num2)
			{
			case 0:
			{
				IL_17:
				num = 0;
				string text = "";
				try
				{
					switch (0)
					{
					case 0:
						goto IL_ED;
					}
					string[] array;
					int num3;
					while (true)
					{
						IL_4E:
						bool flag;
						bool flag2;
						bool flag3;
						bool flag5;
						bool flag6;
						bool flag7;
						bool flag8;
						bool flag9;
						bool flag10;
						bool flag11;
						bool flag12;
						switch (num2)
						{
						case 0:
							if (flag)
							{
								num = 32;
								num2 = num;
								continue;
							}
							goto IL_4F2;
						case 1:
							goto IL_240;
						case 2:
							goto IL_16B;
						case 3:
							goto IL_46C;
						case 4:
							text = array[num3];
							num = 9;
							num2 = num;
							continue;
						case 5:
							num = 1;
							if (num != 0)
							{
							}
							goto IL_1DF;
						case 6:
							text = array[num3] + " " + array[num3 + 1];
							num = 17;
							num2 = num;
							continue;
						case 7:
							if (flag2)
							{
								num = 27;
								num2 = num;
								continue;
							}
							goto IL_369;
						case 8:
						{
							if (!flag3)
							{
								num = 11;
								num2 = num;
								continue;
							}
							bool flag4 = array[num3].StartsWith("THE");
							num = 37;
							num2 = num;
							continue;
						}
						case 9:
							goto IL_537;
						case 10:
							goto IL_285;
						case 11:
							num = 30;
							num2 = num;
							continue;
						case 12:
							goto IL_40A;
						case 13:
							if (flag5)
							{
								num = 26;
								num2 = num;
								continue;
							}
							goto IL_46C;
						case 14:
							text = array[num3];
							num = 2;
							num2 = num;
							continue;
						case 15:
							if (flag6)
							{
								num = 14;
								num2 = num;
								continue;
							}
							goto IL_16B;
						case 16:
							goto IL_327;
						case 17:
							goto IL_5C1;
						case 18:
						{
							num = 7776;
							int arg_3A9_0 = num;
							num = 7776;
							switch ((arg_3A9_0 == num) ? 1 : 0)
							{
							case 0:
							case 2:
								goto IL_4F2;
							case 1:
								IL_3BD:
								num = 0;
								if (num != 0)
								{
								}
								if (flag7)
								{
									num = 25;
									num2 = num;
									continue;
								}
								goto IL_57C;
							}
							goto IL_3BD;
						}
						case 19:
							if (flag8)
							{
								num = 20;
								num2 = num;
								continue;
							}
							goto IL_40A;
						case 20:
							text = array[num3];
							num = 12;
							num2 = num;
							continue;
						case 21:
							goto IL_285;
						case 22:
							text = array[num3] + " " + array[num3 + 1];
							num = 1;
							num2 = num;
							continue;
						case 23:
							goto IL_4F2;
						case 24:
							if (flag9)
							{
								num = 35;
								num2 = num;
								continue;
							}
							goto IL_1DF;
						case 25:
							text = array[num3];
							num = 36;
							num2 = num;
							continue;
						case 26:
							text = array[num3];
							num = 3;
							num2 = num;
							continue;
						case 27:
							text = array[num3];
							num = 31;
							num2 = num;
							continue;
						case 28:
							if (flag10)
							{
								num = 22;
								num2 = num;
								continue;
							}
							goto IL_240;
						case 29:
							if (flag11)
							{
								num = 6;
								num2 = num;
								continue;
							}
							goto IL_5C1;
						case 30:
							goto IL_656;
						case 31:
							goto IL_369;
						case 32:
							text = array[num3];
							num = 23;
							num2 = num;
							continue;
						case 33:
							text = array[num3];
							num = 16;
							num2 = num;
							continue;
						case 34:
							if (flag12)
							{
								num = 33;
								num2 = num;
								continue;
							}
							goto IL_327;
						case 35:
							text += array[num3];
							num = 5;
							num2 = num;
							continue;
						case 36:
							goto IL_57C;
						case 37:
						{
							bool flag4;
							if (flag4)
							{
								num = 4;
								num2 = num;
								continue;
							}
							goto IL_537;
						}
						}
						goto IL_ED;
						IL_16B:
						flag8 = array[num3].StartsWith("AN INVALID");
						num = 19;
						num2 = num;
						continue;
						IL_1DF:
						flag = array[num3].StartsWith("NOT");
						num = 0;
						num2 = num;
						continue;
						IL_240:
						flag11 = array[num3].StartsWith("YOUR FINANCIAL INSTITUTION IS");
						num = 29;
						num2 = num;
						continue;
						IL_285:
						flag3 = (num3 < array.Length);
						num = 8;
						num2 = num;
						continue;
						IL_327:
						flag2 = array[num3].StartsWith("INVALID");
						num = 7;
						num2 = num;
						continue;
						IL_369:
						flag7 = array[num3].StartsWith("PIN");
						num = 18;
						num2 = num;
						continue;
						IL_40A:
						flag10 = array[num3].StartsWith("THIS CARD IS INVALID");
						num = 28;
						num2 = num;
						continue;
						IL_46C:
						flag9 = array[num3].StartsWith("FINANCIAL");
						num = 24;
						num2 = num;
						continue;
						IL_4F2:
						flag6 = array[num3].StartsWith("ISSUER OR");
						num = 15;
						num2 = num;
						continue;
						IL_537:
						flag12 = array[num3].StartsWith("TRANSACTION");
						num = 34;
						num2 = num;
						continue;
						IL_57C:
						flag5 = array[num3].StartsWith("YOU HAVE");
						num = 13;
						num2 = num;
						continue;
						IL_5C1:
						num3++;
						num = 10;
						num2 = num;
					}
					IL_656:
					goto IL_3C;
					IL_ED:
					array = transactionblock.Split(new char[]
					{
						'\n'
					});
					num3 = 0;
					num = 21;
					num2 = num;
					goto IL_4E;
				}
				catch (Exception var_15_65B)
				{
					goto IL_3C;
				}
				string result;
				return result;
				IL_3C:
				result = text;
				return result;
			}
			}
			goto IL_17;
		}

		public string GetPrepaid(string transactionblock)
		{
			int num = 0;
			int num2 = num;
			switch (num2)
			{
			case 0:
			{
				IL_17:
				string text = "";
				try
				{
					switch (0)
					{
					case 0:
						goto IL_81;
					}
					int num3;
					string[] array;
					while (true)
					{
						IL_32:
						bool arg_2F8_0;
						string text2;
						bool flag2;
						bool flag4;
						switch (num2)
						{
						case 0:
							arg_2F8_0 = true;
							goto IL_2F8;
						case 1:
							arg_2F8_0 = text2.StartsWith("GHS");
							goto IL_2F8;
						case 2:
							goto IL_AD;
						case 3:
						{
							bool flag;
							if (flag)
							{
								num = 5;
								num2 = num;
								continue;
							}
							num3++;
							num = 13;
							num2 = num;
							continue;
						}
						case 4:
						{
							if (flag2)
							{
								num = 16;
								num2 = num;
								continue;
							}
							bool flag3 = text2.StartsWith("N");
							num = 10;
							num2 = num;
							continue;
						}
						case 5:
							text2 = array[num3].Split(new string[]
							{
								"ADVANCE PREPAID"
							}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
							num = 8;
							num2 = num;
							continue;
						case 6:
							goto IL_330;
						case 7:
						{
							if (!flag4)
							{
								num = 6;
								num2 = num;
								continue;
							}
							bool flag = array[num3].Contains("ADVANCE PREPAID");
							num = 3;
							num2 = num;
							continue;
						}
						case 8:
						{
							if (!text2.StartsWith("NGN"))
							{
								num = 15;
								num2 = num;
								continue;
							}
							num = 18833;
							int arg_27D_0 = num;
							num = 18833;
							switch ((arg_27D_0 == num) ? 1 : 0)
							{
							case 0:
							case 2:
								goto IL_128;
							case 1:
								IL_291:
								num = 0;
								if (num != 0)
								{
								}
								num = 0;
								num2 = num;
								continue;
							}
							goto IL_291;
						}
						case 9:
							goto IL_330;
						case 10:
						{
							bool flag3;
							if (flag3)
							{
								num = 11;
								num2 = num;
								continue;
							}
							goto IL_AD;
						}
						case 11:
							goto IL_128;
						case 12:
							goto IL_346;
						case 13:
							goto IL_161;
						case 14:
							goto IL_161;
						case 15:
							num = 1;
							num2 = num;
							continue;
						case 16:
							text = text2.Substring(3).Replace(")", "").Trim();
							num = 2;
							num2 = num;
							continue;
						case 17:
							goto IL_AD;
						}
						goto IL_81;
						IL_AD:
						num = 9;
						num2 = num;
						continue;
						IL_128:
						text = text2.Substring(1).Replace(")", "").Trim();
						num = 17;
						num2 = num;
						continue;
						IL_161:
						flag4 = (num3 < array.Length);
						num = 7;
						num2 = num;
						continue;
						IL_2F8:
						flag2 = arg_2F8_0;
						num = 4;
						num2 = num;
						continue;
						IL_330:
						num = 12;
						num2 = num;
					}
					IL_346:
					goto IL_20;
					IL_81:
					array = transactionblock.Split(new char[]
					{
						'\n'
					});
					num3 = 0;
					num = 14;
					num2 = num;
					goto IL_32;
				}
				catch (Exception var_8_34B)
				{
					goto IL_20;
				}
				goto IL_354;
				IL_20:
				string result = text;
				IL_354:
				num = 1;
				if (num != 0)
				{
				}
				num = 0;
				return result;
			}
			}
			goto IL_17;
		}

		public string GetFromAccount(string transactionblock, int brand)
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
					goto IL_48;
				}
				string[] array;
				int num3;
				string result;
				string text2;
				while (true)
				{
					IL_21:
					bool flag;
					switch (num2)
					{
					case 0:
					{
						if (!flag)
						{
							num = -27028;
							int arg_D1_0 = num;
							num = -27028;
							switch ((arg_D1_0 == num) ? 1 : 0)
							{
							case 0:
							case 2:
								goto IL_110;
							case 1:
								IL_E5:
								num = 0;
								if (num != 0)
								{
								}
								num = 5;
								num2 = num;
								continue;
							}
							goto IL_E5;
						}
						bool flag2 = array[num3].Contains("FROM");
						num = 7;
						num2 = num;
						continue;
					}
					case 1:
						return result;
					case 2:
						goto IL_95;
					case 3:
						goto IL_95;
					case 4:
					{
						string text = array[num3].Split(new string[]
						{
							"FROM"
						}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
						text2 = text;
						num = 6;
						num2 = num;
						continue;
					}
					case 5:
						goto IL_184;
					case 6:
						goto IL_184;
					case 7:
					{
						bool flag2;
						if (flag2)
						{
							num = 4;
							num2 = num;
							continue;
						}
						num3++;
						goto IL_110;
					}
					}
					goto IL_48;
					IL_95:
					flag = (num3 < array.Length);
					num = 0;
					num2 = num;
					continue;
					IL_110:
					num = 0;
					num = 3;
					num2 = num;
					continue;
					IL_184:
					result = text2;
					num = 1;
					num2 = num;
				}
				return result;
				IL_48:
				num = 1;
				if (num != 0)
				{
				}
				text2 = "";
				array = transactionblock.Split(new char[]
				{
					'\n'
				});
				num3 = 0;
				num = 2;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string GetToAccount(string transactionblock, int brand)
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
					goto IL_48;
				}
				string[] array;
				int num3;
				string result;
				string text2;
				while (true)
				{
					IL_21:
					bool flag;
					switch (num2)
					{
					case 0:
					{
						if (!flag)
						{
							num = 29711;
							int arg_B0_0 = num;
							num = 29711;
							switch ((arg_B0_0 == num) ? 1 : 0)
							{
							case 0:
							case 2:
								goto IL_10D;
							case 1:
								IL_C4:
								num = 1;
								if (num != 0)
								{
								}
								num = 0;
								if (num != 0)
								{
								}
								num = 5;
								num2 = num;
								continue;
							}
							goto IL_C4;
						}
						bool flag2 = array[num3].Contains("TO");
						num = 7;
						num2 = num;
						continue;
					}
					case 1:
						return result;
					case 2:
						goto IL_77;
					case 3:
						goto IL_77;
					case 4:
					{
						string text = array[num3].Split(new string[]
						{
							"TO"
						}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
						text2 = text;
						num = 6;
						num2 = num;
						continue;
					}
					case 5:
						goto IL_181;
					case 6:
						goto IL_181;
					case 7:
					{
						bool flag2;
						if (flag2)
						{
							num = 4;
							num2 = num;
							continue;
						}
						num3++;
						goto IL_10D;
					}
					}
					goto IL_48;
					IL_77:
					flag = (num3 < array.Length);
					num = 0;
					num2 = num;
					continue;
					IL_10D:
					num = 0;
					num = 3;
					num2 = num;
					continue;
					IL_181:
					result = text2;
					num = 1;
					num2 = num;
				}
				return result;
				IL_48:
				text2 = "";
				array = transactionblock.Split(new char[]
				{
					'\n'
				});
				num3 = 0;
				num = 2;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string GetNotesPresented(string transactionblock, int brand)
		{
			int num = 0;
			int num2 = num;
			switch (num2)
			{
			case 0:
			{
				IL_17:
				num = 1;
				if (num != 0)
				{
				}
				switch (0)
				{
				case 0:
					goto IL_66;
				}
				string[] array;
				int num3;
				string result;
				string text2;
				while (true)
				{
					IL_3F:
					bool flag;
					switch (num2)
					{
					case 0:
					{
						if (!flag)
						{
							num = 18941;
							int arg_CE_0 = num;
							num = 18941;
							switch ((arg_CE_0 == num) ? 1 : 0)
							{
							case 0:
							case 2:
								goto IL_10D;
							case 1:
								IL_E2:
								num = 0;
								if (num != 0)
								{
								}
								num = 5;
								num2 = num;
								continue;
							}
							goto IL_E2;
						}
						bool flag2 = array[num3].Contains("NOTES PRESENTED");
						num = 7;
						num2 = num;
						continue;
					}
					case 1:
						return result;
					case 2:
						goto IL_95;
					case 3:
						goto IL_95;
					case 4:
					{
						string text = array[num3].Split(new string[]
						{
							"NOTES PRESENTED"
						}, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
						text2 = text;
						num = 6;
						num2 = num;
						continue;
					}
					case 5:
						goto IL_181;
					case 6:
						goto IL_181;
					case 7:
					{
						bool flag2;
						if (flag2)
						{
							num = 4;
							num2 = num;
							continue;
						}
						num3++;
						goto IL_10D;
					}
					}
					goto IL_66;
					IL_95:
					flag = (num3 < array.Length);
					num = 0;
					num2 = num;
					continue;
					IL_10D:
					num = 0;
					num = 3;
					num2 = num;
					continue;
					IL_181:
					result = text2;
					num = 1;
					num2 = num;
				}
				return result;
				IL_66:
				text2 = "";
				array = transactionblock.Split(new char[]
				{
					'\n'
				});
				num3 = 0;
				num = 2;
				num2 = num;
				goto IL_3F;
			}
			}
			goto IL_17;
		}

		public string[] GetDenominations(string transactionblock)
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
					goto IL_6C;
				}
				string[] array;
				int num3;
				string[] array2;
				string[] result;
				while (true)
				{
					IL_21:
					bool flag;
					bool flag2;
					bool flag4;
					bool flag5;
					switch (num2)
					{
					case 0:
						if (flag)
						{
							num = 8;
							num2 = num;
							continue;
						}
						goto IL_224;
					case 1:
					{
						string text = array[num3].Split(new string[]
						{
							"DISPENSED"
						}, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
						array2[1] = text;
						num = 15;
						num2 = num;
						continue;
					}
					case 2:
					{
						string text2 = array[num3].Split(new string[]
						{
							"DENOMINATION"
						}, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
						array2[0] = text2;
						num = 11;
						num2 = num;
						continue;
					}
					case 3:
					{
						if (!flag2)
						{
							num = 14;
							num2 = num;
							continue;
						}
						bool flag3 = array[num3].Contains("DENOMINATION");
						num = 13;
						num2 = num;
						continue;
					}
					case 4:
						goto IL_25B;
					case 5:
						goto IL_2E1;
					case 6:
						if (flag4)
						{
							num = 1;
							num2 = num;
							continue;
						}
						goto IL_260;
					case 7:
						goto IL_164;
					case 8:
					{
						string text3 = array[num3].Split(new string[]
						{
							"REMAINING"
						}, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
						array2[3] = text3;
						num = 10;
						num2 = num;
						continue;
					}
					case 9:
						goto IL_164;
					case 10:
						goto IL_224;
					case 11:
						goto IL_2A2;
					case 12:
					{
						string text4 = array[num3].Split(new string[]
						{
							"REJECTED"
						}, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
						array2[2] = text4;
						num = 5;
						num2 = num;
						continue;
					}
					case 13:
					{
						bool flag3;
						if (flag3)
						{
							num = 2;
							num2 = num;
							continue;
						}
						goto IL_2A2;
					}
					case 14:
					{
						num = 3344;
						int arg_1B9_0 = num;
						num = 3344;
						switch ((arg_1B9_0 == num) ? 1 : 0)
						{
						case 0:
						case 2:
							goto IL_37C;
						case 1:
							IL_1CD:
							num = 0;
							if (num != 0)
							{
							}
							result = array2;
							num = 4;
							num2 = num;
							continue;
						}
						goto IL_1CD;
					}
					case 15:
						goto IL_37C;
					case 16:
						if (flag5)
						{
							num = 12;
							num2 = num;
							continue;
						}
						goto IL_2E1;
					}
					goto IL_6C;
					IL_164:
					flag2 = (num3 < array.Length);
					num = 3;
					num2 = num;
					continue;
					IL_224:
					num3++;
					num = 7;
					num2 = num;
					continue;
					IL_260:
					flag5 = array[num3].Contains("REJECTED");
					num = 16;
					num2 = num;
					continue;
					IL_37C:
					goto IL_260;
					IL_2A2:
					flag4 = array[num3].Contains("DISPENSED");
					num = 6;
					num2 = num;
					continue;
					IL_2E1:
					flag = array[num3].Contains("REMAINING");
					num = 1;
					if (num != 0)
					{
					}
					num = 0;
					num2 = num;
				}
				IL_25B:
				num = 0;
				return result;
				IL_6C:
				array2 = new string[0];
				array = transactionblock.Split(new char[]
				{
					'\n'
				});
				num3 = 0;
				num = 9;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string[] GetRequestAmount(string transactionblock)
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
					goto IL_8C;
				}
				string[] array;
				int num3;
				string[] array2;
				string[] result;
				while (true)
				{
					IL_21:
					num = 0;
					bool flag2;
					bool arg_317_0;
					bool flag4;
					switch (num2)
					{
					case 0:
						goto IL_12F;
					case 1:
						goto IL_170;
					case 2:
					{
						string text = array[num3].Split(new string[]
						{
							"CASH REQUEST:"
						}, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
						num = 0;
						num2 = num;
						continue;
					}
					case 3:
					{
						bool flag = array[num3].Split(new string[]
						{
							"CASH"
						}, StringSplitOptions.RemoveEmptyEntries).Length > 1;
						num = 11;
						num2 = num;
						continue;
					}
					case 4:
						if (flag2)
						{
							num = 3;
							num2 = num;
							continue;
						}
						goto IL_2F8;
					case 5:
						arg_317_0 = array[num3].Contains(":");
						goto IL_317;
					case 6:
						result = array2;
						num = 17;
						num2 = num;
						continue;
					case 7:
					{
						string text2 = array[num3].Split(new string[]
						{
							"CASH"
						}, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
						num = 8;
						num2 = num;
						continue;
					}
					case 8:
						goto IL_C2;
					case 9:
						goto IL_2F8;
					case 10:
						goto IL_170;
					case 11:
					{
						bool flag;
						if (flag)
						{
							goto IL_115;
						}
						goto IL_C2;
					}
					case 12:
						if (array[num3].Contains("CASH"))
						{
							num = 15;
							num2 = num;
							continue;
						}
						num = 16;
						num2 = num;
						continue;
					case 13:
					{
						bool flag3;
						if (flag3)
						{
							num = -8843;
							int arg_2BB_0 = num;
							num = -8843;
							switch ((arg_2BB_0 == num) ? 1 : 0)
							{
							case 0:
							case 2:
								goto IL_115;
							case 1:
								IL_2CF:
								num = 0;
								if (num != 0)
								{
								}
								num = 2;
								num2 = num;
								continue;
							}
							goto IL_2CF;
						}
						goto IL_12F;
					}
					case 14:
					{
						if (!flag4)
						{
							num = 6;
							num2 = num;
							continue;
						}
						bool flag3 = array[num3].Contains("CASH REQUEST:");
						num = 13;
						num2 = num;
						continue;
					}
					case 15:
						num = 5;
						num2 = num;
						continue;
					case 16:
						arg_317_0 = false;
						goto IL_317;
					case 17:
						goto IL_237;
					}
					goto IL_8C;
					IL_C2:
					num = 9;
					num2 = num;
					continue;
					IL_115:
					num = 7;
					num2 = num;
					continue;
					IL_12F:
					num = 12;
					num2 = num;
					continue;
					IL_170:
					flag4 = (num3 < array.Length);
					num = 14;
					num2 = num;
					continue;
					IL_2F8:
					num3++;
					num = 1;
					num2 = num;
					continue;
					IL_317:
					flag2 = arg_317_0;
					num = 4;
					num2 = num;
				}
				IL_237:
				num = 1;
				if (num != 0)
				{
				}
				return result;
				IL_8C:
				array2 = new string[2];
				array = transactionblock.Split(new char[]
				{
					'\n'
				});
				num3 = 0;
				num = 10;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string GetAmountEntered(string data)
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
					goto IL_64;
				}
				string text;
				int num3;
				string[] array;
				string result;
				while (true)
				{
					IL_21:
					bool arg_228_0;
					string text2;
					bool flag;
					switch (num2)
					{
					case 0:
						arg_228_0 = false;
						goto IL_228;
					case 1:
					{
						decimal d;
						text = decimal.Round(d, 2).ToString();
						num = 2;
						num2 = num;
						continue;
					}
					case 2:
						goto IL_20B;
					case 3:
						if (num3 >= array.Length)
						{
							num = 0;
							num = 8;
							num2 = num;
							continue;
						}
						text2 = array[num3];
						num = 5;
						num2 = num;
						continue;
					case 4:
						if (flag)
						{
							num = 6;
							num2 = num;
							continue;
						}
						goto IL_FE;
					case 5:
						if (text2.Contains("AMOUNT"))
						{
							num = 12;
							num2 = num;
							continue;
						}
						num = 0;
						num2 = num;
						continue;
					case 6:
						text = text2.Split(new char[]
						{
							' '
						})[1];
						num = 14;
						num2 = num;
						continue;
					case 7:
						goto IL_1B9;
					case 8:
					{
						decimal d = 0m;
						bool flag2 = decimal.TryParse(text, out d);
						num = 10;
						num2 = num;
						continue;
					}
					case 9:
						goto IL_1B9;
					case 10:
					{
						num = 1;
						if (num != 0)
						{
						}
						bool flag2;
						if (flag2)
						{
							num = 1;
							num2 = num;
							continue;
						}
						goto IL_20B;
					}
					case 11:
						return result;
					case 12:
						num = 13;
						num2 = num;
						continue;
					case 13:
						goto IL_134;
					case 14:
					{
						num = 32544;
						int arg_2B9_0 = num;
						num = 32544;
						switch ((arg_2B9_0 == num) ? 1 : 0)
						{
						case 0:
						case 2:
							goto IL_134;
						case 1:
							IL_2CD:
							num = 0;
							if (num != 0)
							{
							}
							goto IL_FE;
						}
						goto IL_2CD;
					}
					}
					goto IL_64;
					IL_FE:
					num3++;
					num = 9;
					num2 = num;
					continue;
					IL_1B9:
					num = 3;
					num2 = num;
					continue;
					IL_20B:
					result = text;
					num = 11;
					num2 = num;
					continue;
					IL_228:
					flag = arg_228_0;
					num = 4;
					num2 = num;
					continue;
					IL_134:
					arg_228_0 = text2.Contains("ENTERED");
					goto IL_228;
				}
				return result;
				IL_64:
				text = "";
				string[] array2 = data.Split(new char[]
				{
					'\n'
				}, StringSplitOptions.RemoveEmptyEntries);
				array = array2;
				num3 = 0;
				num = 7;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string GetOpCode(string data, int brand)
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
					goto IL_E0;
				}
				string[] array2;
				string text2;
				bool flag3;
				string result;
				while (true)
				{
					IL_21:
					num = 0;
					int num3;
					int num4;
					int num5;
					int num6;
					bool flag6;
					bool flag8;
					bool flag9;
					bool flag10;
					switch (num2)
					{
					case 0:
						goto IL_134;
					case 1:
					{
						bool flag;
						if (flag)
						{
							num = 21;
							num2 = num;
							continue;
						}
						bool flag2 = brand == 2;
						num = 26;
						num2 = num;
						continue;
					}
					case 2:
						num = 7;
						num2 = num;
						continue;
					case 3:
					{
						string[] array = array2[num3].Split(new string[]
						{
							"OPCODE"
						}, StringSplitOptions.RemoveEmptyEntries);
						string text = array[1].Replace('=', ' ').Trim();
						text2 = text;
						num = 38;
						num2 = num;
						continue;
					}
					case 4:
						goto IL_187;
					case 5:
						goto IL_2CD;
					case 6:
					{
						string[] array3 = array2[num4].Split(new string[]
						{
							"OPCode"
						}, StringSplitOptions.RemoveEmptyEntries);
						string text3 = array3[1].Replace('[', ' ').Replace(']', ' ').Trim();
						text2 = text3;
						num = 34;
						num2 = num;
						continue;
					}
					case 7:
						goto IL_2CD;
					case 8:
					{
						string[] array4 = array2[num5].Split(new string[]
						{
							"TRANSACTION REQUEST"
						}, StringSplitOptions.RemoveEmptyEntries);
						string text4 = array4[1].Replace('=', ' ').Trim();
						text2 = text4;
						num = 22;
						num2 = num;
						continue;
					}
					case 9:
						goto IL_1E3;
					case 10:
					{
						if (flag3)
						{
							num = 36;
							num2 = num;
							continue;
						}
						bool flag = brand == 1;
						num = 1;
						num2 = num;
						continue;
					}
					case 11:
						goto IL_28E;
					case 12:
						goto IL_343;
					case 13:
					{
						bool flag4;
						if (flag4)
						{
							num = 8;
							num2 = num;
							continue;
						}
						num5++;
						num = 31;
						num2 = num;
						continue;
					}
					case 14:
						goto IL_2CD;
					case 15:
						num3 = 0;
						num = 0;
						num2 = num;
						continue;
					case 16:
						goto IL_47F;
					case 17:
					{
						bool flag5;
						if (flag5)
						{
							num = 18;
							num2 = num;
							continue;
						}
						num = 5;
						num2 = num;
						continue;
					}
					case 18:
					{
						num6 = 0;
						num = -13742;
						int arg_406_0 = num;
						num = -13742;
						switch ((arg_406_0 == num) ? 1 : 0)
						{
						case 0:
						case 2:
							goto IL_152;
						case 1:
							IL_41A:
							num = 0;
							if (num != 0)
							{
							}
							num = 11;
							num2 = num;
							continue;
						}
						goto IL_41A;
					}
					case 19:
					{
						if (!flag6)
						{
							num = 37;
							num2 = num;
							continue;
						}
						bool flag4 = array2[num5].Contains("TRANSACTION REQUEST");
						num = 13;
						num2 = num;
						continue;
					}
					case 20:
					{
						bool flag7;
						if (flag7)
						{
							num = 6;
							num2 = num;
							continue;
						}
						num4++;
						num = 12;
						num2 = num;
						continue;
					}
					case 21:
						num5 = 0;
						num = 16;
						num2 = num;
						continue;
					case 22:
						goto IL_4BB;
					case 23:
						return result;
					case 24:
						goto IL_2CD;
					case 25:
						goto IL_28E;
					case 26:
					{
						bool flag2;
						if (flag2)
						{
							num = 15;
							num2 = num;
							continue;
						}
						bool flag5 = brand == 3;
						num = 17;
						num2 = num;
						continue;
					}
					case 27:
					{
						if (!flag8)
						{
							num = 4;
							num2 = num;
							continue;
						}
						bool flag7 = array2[num4].Contains("OPCode");
						num = 20;
						num2 = num;
						continue;
					}
					case 28:
						goto IL_2CD;
					case 29:
						goto IL_343;
					case 30:
						goto IL_134;
					case 31:
						num = 1;
						if (num != 0)
						{
						}
						goto IL_47F;
					case 32:
						if (flag9)
						{
							num = 3;
							num2 = num;
							continue;
						}
						num3++;
						num = 30;
						num2 = num;
						continue;
					case 33:
						if (!flag10)
						{
							num = 2;
							num2 = num;
							continue;
						}
						num6++;
						num = 25;
						num2 = num;
						continue;
					case 34:
						goto IL_187;
					case 35:
						goto IL_152;
					case 36:
						num4 = 0;
						num = 29;
						num2 = num;
						continue;
					case 37:
						goto IL_4BB;
					case 38:
						goto IL_1E3;
					}
					goto IL_E0;
					IL_134:
					bool flag11 = num3 < array2.Length;
					num = 35;
					num2 = num;
					continue;
					IL_152:
					if (!flag11)
					{
						num = 9;
						num2 = num;
						continue;
					}
					flag9 = array2[num3].Contains("OPCODE");
					num = 32;
					num2 = num;
					continue;
					IL_187:
					num = 24;
					num2 = num;
					continue;
					IL_1E3:
					num = 28;
					num2 = num;
					continue;
					IL_28E:
					flag10 = (num6 < array2.Length);
					num = 33;
					num2 = num;
					continue;
					IL_2CD:
					result = text2;
					num = 23;
					num2 = num;
					continue;
					IL_343:
					flag8 = (num4 < array2.Length);
					num = 27;
					num2 = num;
					continue;
					IL_47F:
					flag6 = (num5 < array2.Length);
					num = 19;
					num2 = num;
					continue;
					IL_4BB:
					num = 14;
					num2 = num;
				}
				return result;
				IL_E0:
				array2 = data.Split(new char[]
				{
					'\n'
				}, StringSplitOptions.RemoveEmptyEntries);
				text2 = "";
				flag3 = (brand == 0);
				num = 10;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string GetDatetimeTerminalId(string data)
		{
			int num = 14650;
			int arg_1C_0 = num;
			num = 14650;
			switch ((arg_1C_0 == num) ? 1 : 0)
			{
			case 0:
			case 2:
				goto IL_90;
			case 1:
				break;
			default:
				num = 0;
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
			int num2 = num;
			switch (num2)
			{
			case 0:
			{
				IL_90:
				string result;
				try
				{
					switch (0)
					{
					case 0:
						goto IL_E8;
					}
					string[] array;
					int num3;
					string text4;
					while (true)
					{
						IL_9D:
						bool flag;
						Match match;
						string text;
						string text2;
						string text3;
						bool success3;
						bool success4;
						switch (num2)
						{
						case 0:
						{
							if (!flag)
							{
								num = 5;
								num2 = num;
								continue;
							}
							string pattern = "((?<Date>\\d+[-|/|\\\\]\\d+[-|/|\\\\]\\d+)[ ]*(?<Time>\\d{2}:\\d{2})[ ]*(?<ATM>\\w+))";
							Regex regex = new Regex(pattern);
							match = regex.Match(array[num3]);
							text = "";
							text2 = "";
							text3 = "";
							bool success = match.Success;
							num = 1;
							num2 = num;
							continue;
						}
						case 1:
						{
							bool success;
							if (success)
							{
								num = 3;
								num2 = num;
								continue;
							}
							num3++;
							num = 6;
							num2 = num;
							continue;
						}
						case 2:
						{
							bool success2;
							if (success2)
							{
								num = 7;
								num2 = num;
								continue;
							}
							goto IL_37F;
						}
						case 3:
						{
							bool success2 = match.Groups["Date"].Success;
							num = 2;
							num2 = num;
							continue;
						}
						case 4:
							goto IL_37F;
						case 5:
							goto IL_3CA;
						case 6:
							goto IL_207;
						case 7:
							text = match.Groups["Date"].Captures[0].Value;
							num = 4;
							num2 = num;
							continue;
						case 8:
							goto IL_334;
						case 9:
							text2 = match.Groups["Time"].Captures[0].Value;
							num = 8;
							num2 = num;
							continue;
						case 10:
							goto IL_3E2;
						case 11:
							goto IL_2EF;
						case 12:
							goto IL_3CA;
						case 13:
							if (success3)
							{
								num = 9;
								num2 = num;
								continue;
							}
							goto IL_334;
						case 14:
							goto IL_207;
						case 15:
							if (success4)
							{
								num = 16;
								num2 = num;
								continue;
							}
							goto IL_2EF;
						case 16:
							text3 = match.Groups["ATM"].Captures[0].Value;
							num = 11;
							num2 = num;
							continue;
						}
						goto IL_E8;
						IL_207:
						flag = (num3 < array.Length);
						num = 0;
						num2 = num;
						continue;
						IL_2EF:
						text4 = string.Concat(new string[]
						{
							text3,
							"&",
							text,
							"&",
							text2
						});
						num = 12;
						num2 = num;
						continue;
						IL_334:
						success4 = match.Groups["ATM"].Success;
						num = 15;
						num2 = num;
						continue;
						IL_37F:
						success3 = match.Groups["Time"].Success;
						num = 13;
						num2 = num;
						continue;
						IL_3CA:
						result = text4;
						num = 10;
						num2 = num;
					}
					IL_3E2:
					return result;
					IL_E8:
					text4 = "";
					array = data.Split(new char[]
					{
						'\n'
					}, StringSplitOptions.RemoveEmptyEntries);
					num3 = 0;
					num = 14;
					num2 = num;
					goto IL_9D;
				}
				catch (Exception var_15_3E4)
				{
					result = "";
				}
				return result;
			}
			}
			goto IL_90;
		}

		public string GetStartandEndTime(string data)
		{
			int num = 0;
			int num2 = num;
			switch (num2)
			{
			case 0:
			{
				IL_17:
				num = 0;
				string result;
				try
				{
					switch (0)
					{
					case 0:
						goto IL_8F;
					}
					int num3;
					string str;
					string str2;
					string[] array;
					int num4;
					while (true)
					{
						IL_40:
						bool flag2;
						switch (num2)
						{
						case 0:
						{
							bool success;
							if (success)
							{
								num = 1;
								num2 = num;
								continue;
							}
							goto IL_142;
						}
						case 1:
						{
							num3++;
							bool flag = num3 <= 1;
							num = 8;
							num2 = num;
							continue;
						}
						case 2:
							result = str + "&" + str2;
							num = 11;
							num2 = num;
							continue;
						case 3:
						{
							Match match;
							str2 = match.Groups["Time"].Captures[0].Value;
							num = 6;
							num2 = num;
							continue;
						}
						case 4:
							goto IL_142;
						case 5:
							goto IL_37D;
						case 6:
							goto IL_1E6;
						case 7:
							goto IL_D0;
						case 8:
						{
							bool flag;
							if (flag)
							{
								num = -1837;
								int arg_1A9_0 = num;
								num = -1837;
								switch ((arg_1A9_0 == num) ? 1 : 0)
								{
								case 0:
								case 2:
									goto IL_D0;
								case 1:
									IL_1BD:
									num = 0;
									if (num != 0)
									{
									}
									num = 12;
									num2 = num;
									continue;
								}
								goto IL_1BD;
							}
							Match match;
							bool success2 = match.Groups["Time"].Success;
							num = 9;
							num2 = num;
							continue;
						}
						case 9:
						{
							bool success2;
							if (success2)
							{
								num = 3;
								num2 = num;
								continue;
							}
							goto IL_1E6;
						}
						case 10:
							goto IL_2CA;
						case 11:
							goto IL_3BE;
						case 12:
						{
							Match match;
							bool success3 = match.Groups["Time"].Success;
							num = 1;
							if (num != 0)
							{
							}
							num = 13;
							num2 = num;
							continue;
						}
						case 13:
						{
							bool success3;
							if (success3)
							{
								num = 17;
								num2 = num;
								continue;
							}
							goto IL_37D;
						}
						case 14:
							goto IL_2CA;
						case 15:
							goto IL_201;
						case 16:
						{
							if (!flag2)
							{
								num = 2;
								num2 = num;
								continue;
							}
							string pattern = "((?<Time>\\d{2}[:]\\d+[:]\\d+))";
							Regex regex = new Regex(pattern);
							Match match = regex.Match(array[num4]);
							bool success = match.Success;
							num = 0;
							num2 = num;
							continue;
						}
						case 17:
						{
							Match match;
							str = match.Groups["Time"].Captures[0].Value;
							num = 5;
							num2 = num;
							continue;
						}
						}
						goto IL_8F;
						IL_142:
						num4++;
						num = 15;
						num2 = num;
						continue;
						IL_1E6:
						num = 10;
						num2 = num;
						continue;
						IL_201:
						flag2 = (num4 < array.Length);
						num = 16;
						num2 = num;
						continue;
						IL_D0:
						goto IL_201;
						IL_2CA:
						num = 4;
						num2 = num;
						continue;
						IL_37D:
						num = 14;
						num2 = num;
					}
					IL_3BE:
					return result;
					IL_8F:
					num3 = 0;
					array = data.Split(new char[]
					{
						'\n'
					}, StringSplitOptions.RemoveEmptyEntries);
					str = "";
					str2 = "";
					num4 = 0;
					num = 7;
					num2 = num;
					goto IL_40;
				}
				catch (Exception var_15_3C0)
				{
					result = "";
				}
				return result;
			}
			}
			goto IL_17;
		}

		public string GetPAN(string data)
		{
			int num = 0;
			int num2 = num;
			switch (num2)
			{
			case 0:
			{
				IL_17:
				string[] array2;
				string text;
				int num4;
				string[] array3;
				switch (0)
				{
				case 0:
					break;
				default:
				{
					string result;
					while (true)
					{
						IL_21:
						int num3;
						switch (num2)
						{
						case 0:
							num = 12;
							num2 = num;
							continue;
						case 1:
							goto IL_298;
						case 2:
							return result;
						case 3:
						{
							string[] array = array2;
							num3 = 0;
							num = 1;
							num2 = num;
							continue;
						}
						case 4:
						{
							string text2;
							text = text2.Substring(text2.IndexOf("TRACK 2 DATA:") + 13);
							num = 10;
							num2 = num;
							continue;
						}
						case 5:
						{
							bool flag;
							if (flag)
							{
								num = 4;
								num2 = num;
								continue;
							}
							goto IL_110;
						}
						case 6:
						{
							num = 0;
							string[] array;
							if (num3 >= array.Length)
							{
								num = 0;
								num2 = num;
								continue;
							}
							string text3 = array[num3];
							bool flag2 = text3.Contains("XXXXXX");
							num = 17;
							num2 = num;
							continue;
						}
						case 7:
							goto IL_309;
						case 8:
						{
							num = 20622;
							int arg_146_0 = num;
							num = 20622;
							switch ((arg_146_0 == num) ? 1 : 0)
							{
							case 0:
							case 2:
								goto IL_8F;
							case 1:
								IL_15A:
								num = 0;
								if (num != 0)
								{
								}
								goto IL_309;
							}
							goto IL_15A;
						}
						case 9:
						{
							bool flag3;
							if (flag3)
							{
								num = 3;
								num2 = num;
								continue;
							}
							goto IL_1F6;
						}
						case 10:
							num = 1;
							if (num != 0)
							{
							}
							goto IL_110;
						case 11:
						{
							bool flag3 = string.IsNullOrEmpty(text);
							num = 9;
							num2 = num;
							continue;
						}
						case 12:
							goto IL_1F6;
						case 13:
							goto IL_298;
						case 14:
						{
							if (num4 >= array3.Length)
							{
								num = 11;
								num2 = num;
								continue;
							}
							string text2 = array3[num4];
							bool flag = text2.Contains("TRACK 2 DATA:");
							num = 5;
							num2 = num;
							continue;
						}
						case 15:
						{
							string text3;
							text = text3;
							num = 16;
							num2 = num;
							continue;
						}
						case 16:
							goto IL_A6;
						case 17:
						{
							bool flag2;
							if (flag2)
							{
								num = 15;
								num2 = num;
								continue;
							}
							goto IL_A6;
						}
						}
						goto IL_70;
						IL_A6:
						num3++;
						num = 13;
						num2 = num;
						continue;
						IL_110:
						num4++;
						num = 8;
						num2 = num;
						continue;
						IL_1F6:
						result = text;
						num = 2;
						num2 = num;
						continue;
						IL_298:
						num = 6;
						num2 = num;
						continue;
						IL_309:
						num = 14;
						num2 = num;
					}
					return result;
				}
				}
				IL_70:
				text = "";
				array2 = data.Split(new char[]
				{
					'\n'
				}, StringSplitOptions.RemoveEmptyEntries);
				array3 = array2;
				num4 = 0;
				IL_8F:
				num = 7;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string GetPANTSNRRN(string data)
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
					goto IL_40;
				}
				string result;
				string[] array;
				int num3;
				string text4;
				while (true)
				{
					IL_21:
					bool flag2;
					switch (num2)
					{
					case 0:
						return result;
					case 1:
						num = 0;
						try
						{
							switch (0)
							{
							case 0:
								goto IL_176;
							}
							Match match;
							string text;
							string text2;
							bool success4;
							string pattern;
							Regex regex;
							string input;
							string text3;
							while (true)
							{
								IL_123:
								bool success;
								bool success2;
								bool flag;
								switch (num2)
								{
								case 0:
									goto IL_2A2;
								case 1:
									text = match.Groups["TSN"].Captures[0].Value;
									num = 6;
									num2 = num;
									continue;
								case 2:
									goto IL_407;
								case 3:
									if (success)
									{
										num = 13;
										num2 = num;
										continue;
									}
									goto IL_407;
								case 4:
									if (success2)
									{
										num = 15;
										num2 = num;
										continue;
									}
									goto IL_2F7;
								case 5:
								{
									bool success3;
									if (success3)
									{
										num = 0;
										num2 = num;
										continue;
									}
									goto IL_523;
								}
								case 6:
									goto IL_254;
								case 7:
									text2 = this.GetPAN(data);
									num = 16;
									num2 = num;
									continue;
								case 8:
								{
									if (success4)
									{
										num = 14;
										num2 = num;
										continue;
									}
									pattern = "(?<TSN>\\d{4}$)";
									regex = new Regex(pattern);
									input = array[num3];
									match = regex.Match(input);
									bool success3 = match.Success;
									num = 5;
									num2 = num;
									continue;
								}
								case 9:
									if (flag)
									{
										num = 7;
										num2 = num;
										continue;
									}
									goto IL_487;
								case 10:
								{
									bool success5;
									if (success5)
									{
										num = 1;
										num2 = num;
										continue;
									}
									goto IL_254;
								}
								case 11:
									goto IL_4C7;
								case 12:
									goto IL_53A;
								case 13:
									text3 = match.Groups["RRN"].Captures[0].Value;
									num = 2;
									num2 = num;
									continue;
								case 14:
								{
									bool success5 = match.Groups["TSN"].Success;
									num = 10;
									num2 = num;
									continue;
								}
								case 15:
									text = match.Groups["TSN"].Captures[0].Value;
									num = 18;
									num2 = num;
									continue;
								case 16:
									goto IL_487;
								case 17:
									goto IL_523;
								case 18:
									goto IL_2F7;
								}
								goto IL_176;
								IL_254:
								success = match.Groups["RRN"].Success;
								num = 3;
								num2 = num;
								continue;
								IL_2A2:
								text2 = this.GetPAN(data);
								success2 = match.Groups["TSN"].Success;
								num = 4;
								num2 = num;
								continue;
								IL_407:
								flag = (text2 == "");
								num = -23508;
								int arg_431_0 = num;
								num = -23508;
								switch ((arg_431_0 == num) ? 1 : 0)
								{
								case 0:
								case 2:
									goto IL_2A2;
								case 1:
									IL_445:
									num = 0;
									if (num != 0)
									{
									}
									num = 9;
									num2 = num;
									continue;
								}
								goto IL_445;
								IL_2F7:
								text4 = string.Concat(new string[]
								{
									text2,
									"&",
									text,
									"&",
									text3
								});
								num = 17;
								num2 = num;
								continue;
								IL_487:
								text4 = string.Concat(new string[]
								{
									text2,
									"&",
									text,
									"&",
									text3
								});
								num = 11;
								num2 = num;
								continue;
								IL_523:
								num = 12;
								num2 = num;
							}
							IL_4C7:
							goto IL_100;
							IL_53A:
							goto IL_542;
							IL_176:
							pattern = "(?<TSN>\\d{4})[ ]+(?<RRN>\\d{6})";
							regex = new Regex(pattern);
							input = array[num3];
							text2 = "";
							text = "";
							text3 = "";
							match = regex.Match(input);
							success4 = match.Success;
							num = 8;
							num2 = num;
							goto IL_123;
						}
						catch (Exception var_17_53C)
						{
						}
						IL_542:
						num3++;
						num = 3;
						num2 = num;
						continue;
					case 2:
						if (!flag2)
						{
							num = 4;
							num2 = num;
							continue;
						}
						num = 1;
						num2 = num;
						continue;
					case 3:
						goto IL_91;
					case 4:
						goto IL_100;
					case 5:
						num = 1;
						if (num != 0)
						{
						}
						goto IL_91;
					}
					goto IL_40;
					IL_91:
					flag2 = (num3 < array.Length);
					num = 2;
					num2 = num;
					continue;
					IL_100:
					result = text4;
					num = 0;
					num2 = num;
				}
				return result;
				IL_40:
				text4 = "";
				array = data.Split(new char[]
				{
					'\n'
				}, StringSplitOptions.RemoveEmptyEntries);
				num3 = 0;
				num = 5;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string[] GetErrorCodeAndCountsNCR(string details, string tsn)
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
					goto IL_50;
				}
				bool flag2;
				string pattern3;
				string[] result;
				while (true)
				{
					IL_21:
					string text;
					string pattern2;
					string text2;
					string text3;
					string text4;
					string text5;
					string text6;
					switch (num2)
					{
					case 0:
						goto IL_2ED;
					case 1:
					{
						string pattern;
						Match match = Regex.Match(details, pattern);
						text = match.Groups["opcode"].Captures[0].Value;
						num = 6;
						num2 = num;
						continue;
					}
					case 2:
					{
						bool flag;
						if (flag)
						{
							num = 1;
							num2 = num;
							continue;
						}
						goto IL_2D1;
					}
					case 3:
						goto IL_243;
					case 4:
						goto IL_28F;
					case 5:
						if (flag2)
						{
							num = 9;
							num2 = num;
							continue;
						}
						goto IL_28F;
					case 6:
						goto IL_2D1;
					case 7:
						goto IL_2CC;
					case 8:
						num = 0;
						text2 = Regex.Match(details, pattern2).Groups["denom"].Captures[0].Value;
						text3 = Regex.Match(details, pattern2).Groups["rejected"].Captures[0].Value;
						text4 = Regex.Match(details, pattern2).Groups["dispensed"].Captures[0].Value;
						text5 = Regex.Match(details, pattern2).Groups["remain"].Captures[0].Value;
						num = 3;
						num2 = num;
						continue;
					case 9:
						text6 = Regex.Match(details, pattern3).Groups["request"].Captures[0].Value;
						num = 4;
						num2 = num;
						continue;
					}
					goto IL_50;
					IL_28F:
					string[] array = new string[]
					{
						text3,
						text4,
						text6,
						text5,
						text2,
						text
					};
					result = array;
					num = 7;
					num2 = num;
					continue;
					IL_2D1:
					bool flag3 = Regex.IsMatch(details, pattern2);
					num = 0;
					num2 = num;
				}
				IL_243:
				goto IL_30E;
				IL_2CC:
				num = 1;
				if (num != 0)
				{
				}
				return result;
				IL_50:
				num = -19706;
				int arg_6C_0 = num;
				num = -19706;
				switch ((arg_6C_0 == num) ? 1 : 0)
				{
				case 0:
				case 2:
				{
					IL_2ED:
					bool flag3;
					if (flag3)
					{
						num = 8;
						num2 = num;
						goto IL_21;
					}
					goto IL_30E;
				}
				case 1:
				{
					IL_80:
					num = 0;
					if (num != 0)
					{
					}
					string text3 = "";
					string text4 = "";
					string text6 = "";
					string text5 = "";
					string text2 = "";
					string text = "";
					string pattern = "(OPCODE[ ]*=[ ]*(?<opcode>\\w+))\\r*";
					string pattern2 = "(DENOMINATION[ ]*(?<denom>\\d{1,4}[ ]*\\d{1,4}[ ]*\\d{1,4}[ ]*\\d{1,4}))\\r*(DISPENSED[ ]*(?<dispensed>\\d{5,6}[ ]*\\d{5,6}[ ]*\\d{5,6}[ ]*\\d{5,6}))\\r*(REJECTED[ ]*(?<rejected>\\d{5,6}[ ]*\\d{5,6}[ ]*\\d{5,6}[ ]*\\d{5,6}))\\r*(REMAINING[ ]*(?<remain>\\d{5,6}[ ]*\\d{5,6}[ ]*\\d{5,6}[ ]*\\d{5,6}))\\r*";
					pattern3 = "(\\d{2}[:]\\d{2}[:]\\d{2} CASH REQUEST: (?<request>\\d+))";
					bool flag = Regex.IsMatch(details, pattern);
					num = 2;
					num2 = num;
					goto IL_21;
				}
				}
				goto IL_80;
				IL_30E:
				flag2 = Regex.IsMatch(details, pattern3);
				num = 5;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string[] GetErrorCodeAndCountsWincor(string details, string tsn)
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
				string pattern;
				bool flag4;
				string pattern2;
				string text;
				string text2;
				string[] result;
				string pattern3;
				string text3;
				while (true)
				{
					IL_21:
					bool flag2;
					switch (num2)
					{
					case 0:
						IL_12E:
						goto IL_248;
					case 1:
					{
						bool flag = Regex.IsMatch(details, pattern);
						num = 10;
						num2 = num;
						continue;
					}
					case 2:
						if (flag2)
						{
							num = 7;
							num2 = num;
							continue;
						}
						goto IL_248;
					case 3:
						goto IL_2A8;
					case 4:
					{
						num = 1;
						if (num != 0)
						{
						}
						bool flag3;
						if (flag3)
						{
							num = 12;
							num2 = num;
							continue;
						}
						goto IL_133;
					}
					case 5:
						if (flag4)
						{
							num = 1;
							num2 = num;
							continue;
						}
						goto IL_1D5;
					case 6:
						goto IL_1D5;
					case 7:
						text = Regex.Match(details, pattern2).Groups["request"].Captures[0].Value;
						num = 0;
						num2 = num;
						continue;
					case 8:
					{
						Match match = Regex.Match(details, pattern);
						text2 = match.Groups["opcode"].Captures[0].Value;
						num = 0;
						num = 3;
						num2 = num;
						continue;
					}
					case 9:
						return result;
					case 10:
					{
						bool flag;
						if (flag)
						{
							num = 8;
							num2 = num;
							continue;
						}
						goto IL_2A8;
					}
					case 11:
						goto IL_133;
					case 12:
						text3 = Regex.Match(details, pattern3).Groups["denom_cas"].Captures[0].Value;
						num = 11;
						num2 = num;
						continue;
					}
					goto IL_5C;
					IL_133:
					flag2 = Regex.IsMatch(details, pattern2);
					num = 2;
					num2 = num;
					continue;
					IL_1D5:
					string[] array = new string[]
					{
						text,
						text3,
						text2
					};
					result = array;
					num = 9;
					num2 = num;
					continue;
					IL_248:
					num = 6;
					num2 = num;
					continue;
					IL_2A8:
					num = 18;
					int arg_2C4_0 = num;
					num = 18;
					switch ((arg_2C4_0 == num) ? 1 : 0)
					{
					case 0:
					case 2:
						goto IL_12E;
					case 1:
					{
						IL_2D8:
						num = 0;
						if (num != 0)
						{
						}
						bool flag3 = Regex.IsMatch(details, pattern3);
						num = 4;
						num2 = num;
						continue;
					}
					}
					goto IL_2D8;
				}
				return result;
				IL_5C:
				pattern = "(\\d{2}[:]\\d{2}[:]\\d{2} TRANSACTION REQUEST[ ]*(?<opcode>\\w+))";
				pattern3 = "(\\d{2}[:]\\d{2}[:]\\d{2} CASH (<?denom_cas>(\\d{1}[:]\\d{1}[,]\\d+)+))";
				pattern2 = "(\\d{2}[:]\\d{2}[:]\\d{2} CASH REQUEST: (?<request>\\d+))";
				text = "";
				text3 = "";
				text2 = "";
				flag4 = !string.IsNullOrEmpty(details);
				num = 5;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string[] GetErrorCodeAndCountsHYO(string details, string tsn)
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
					goto IL_184;
				}
				bool flag;
				string text2;
				string[] result;
				string text3;
				string text4;
				string text5;
				string text6;
				string[] array2;
				string text7;
				string text8;
				string text11;
				string text12;
				string text13;
				while (true)
				{
					IL_21:
					uint num3;
					string[] array3;
					int num4;
					bool flag5;
					switch (num2)
					{
					case 0:
						goto IL_55D;
					case 1:
						num = 35;
						num2 = num;
						continue;
					case 2:
						goto IL_55D;
					case 3:
						num = 75;
						num2 = num;
						continue;
					case 4:
						goto IL_E1B;
					case 5:
						num = 72;
						num2 = num;
						continue;
					case 6:
						goto IL_365;
					case 7:
						goto IL_55D;
					case 8:
						goto IL_55D;
					case 9:
						num = 78;
						num2 = num;
						continue;
					case 10:
						if (num3 != 2849802023u)
						{
							num = 70;
							num2 = num;
							continue;
						}
						num = 82;
						num2 = num;
						continue;
					case 11:
						goto IL_55D;
					case 12:
						goto IL_433;
					case 13:
						if (num3 != 4283620186u)
						{
							num = 62;
							num2 = num;
							continue;
						}
						num = 41;
						num2 = num;
						continue;
					case 14:
						if (flag)
						{
							num = 60;
							num2 = num;
							continue;
						}
						goto IL_E1B;
					case 15:
						num = 59;
						num2 = num;
						continue;
					case 16:
						num = 0;
						num2 = num;
						continue;
					case 17:
						goto IL_55D;
					case 18:
					{
						bool flag2;
						if (flag2)
						{
							num = 65;
							num2 = num;
							continue;
						}
						goto IL_B0D;
					}
					case 19:
						if (num3 != 473399424u)
						{
							num = 69;
							num2 = num;
							continue;
						}
						num = 63;
						num2 = num;
						continue;
					case 20:
						num = 13;
						num2 = num;
						continue;
					case 21:
						goto IL_55D;
					case 22:
						goto IL_55D;
					case 23:
					{
						bool flag3;
						if (flag3)
						{
							num = 64;
							num2 = num;
							continue;
						}
						goto IL_433;
					}
					case 24:
						num = 81;
						num2 = num;
						continue;
					case 25:
					{
						string text;
						if (!(text == "taken amount"))
						{
							num = 80;
							num2 = num;
							continue;
						}
						char[] trimChars = new char[]
						{
							'[',
							']'
						};
						string value;
						text2 = value.Trim(trimChars);
						num = 43;
						num2 = num;
						continue;
					}
					case 26:
						return result;
					case 27:
						goto IL_55D;
					case 28:
						goto IL_55D;
					case 29:
						if (num3 <= 1748467085u)
						{
							num = 30;
							num2 = num;
							continue;
						}
						num = 10;
						num2 = num;
						continue;
					case 30:
						num = 48;
						num2 = num;
						continue;
					case 31:
						goto IL_55D;
					case 32:
						num = 34;
						num2 = num;
						continue;
					case 33:
						goto IL_55D;
					case 34:
						if (num3 != 1224072165u)
						{
							num = 16;
							num2 = num;
							continue;
						}
						num = 46;
						num2 = num;
						continue;
					case 35:
						goto IL_55D;
					case 36:
						if (num3 != 1748467085u)
						{
							num = 49;
							num2 = num;
							continue;
						}
						num = 39;
						num2 = num;
						continue;
					case 37:
						num = 1;
						if (num != 0)
						{
						}
						num = 12;
						num2 = num;
						continue;
					case 38:
						num = 32;
						num2 = num;
						continue;
					case 39:
					{
						string text;
						if (!(text == "pick-up count"))
						{
							num = 67;
							num2 = num;
							continue;
						}
						char[] trimChars2 = new char[]
						{
							'[',
							']'
						};
						string value;
						text3 = value.Trim(trimChars2);
						num = 45;
						num2 = num;
						continue;
					}
					case 40:
						num = 21;
						num2 = num;
						continue;
					case 41:
					{
						string text;
						if (!(text == "error code"))
						{
							num = 55;
							num2 = num;
							continue;
						}
						char[] trimChars3 = new char[]
						{
							'[',
							']'
						};
						string value;
						text4 = value.Trim(trimChars3);
						num = 8;
						num2 = num;
						continue;
					}
					case 42:
						goto IL_B5E;
					case 43:
						goto IL_55D;
					case 44:
						goto IL_55D;
					case 45:
						goto IL_55D;
					case 46:
					{
						string text;
						if (!(text == "request count"))
						{
							num = 54;
							num2 = num;
							continue;
						}
						char[] trimChars4 = new char[]
						{
							'[',
							']'
						};
						string value;
						text5 = value.Trim(trimChars4);
						num = 44;
						num2 = num;
						continue;
					}
					case 47:
						if (num3 != 1190922813u)
						{
							num = 38;
							num2 = num;
							continue;
						}
						num = 84;
						num2 = num;
						continue;
					case 48:
						if (num3 != 1652719129u)
						{
							num = 15;
							num2 = num;
							continue;
						}
						num = 76;
						num2 = num;
						continue;
					case 49:
						num = 74;
						num2 = num;
						continue;
					case 50:
					{
						string text;
						if (!(text == "remain count"))
						{
							num = 40;
							num2 = num;
							continue;
						}
						char[] trimChars5 = new char[]
						{
							'[',
							']'
						};
						string value;
						text6 = value.Trim(trimChars5);
						num = 7;
						num2 = num;
						continue;
					}
					case 51:
					{
						string[] array;
						string[] arg_872_0 = array;
						Predicate<string> arg_872_1;
						if ((arg_872_1 = JournalProcessorGH.<>c.<>9__46_1) == null)
						{
							arg_872_1 = (JournalProcessorGH.<>c.<>9__46_1 = new Predicate<string>(JournalProcessorGH.<>c.<>9.<GetErrorCodeAndCountsHYO>b__46_1));
						}
						array2 = Array.FindAll<string>(arg_872_0, arg_872_1);
						string pattern = "((?<desc>\\w+.*)[\\t]+(?<value>([\\[]\\w+.*[\\]])))";
						bool flag3 = array2 != null;
						num = 0;
						num = 23;
						num2 = num;
						continue;
					}
					case 52:
						goto IL_8D9;
					case 53:
						num = 71;
						num2 = num;
						continue;
					case 54:
						num = 68;
						num2 = num;
						continue;
					case 55:
						num = 22;
						num2 = num;
						continue;
					case 56:
						goto IL_8D9;
					case 57:
						num = 27;
						num2 = num;
						continue;
					case 58:
						goto IL_55D;
					case 59:
						num = 36;
						num2 = num;
						continue;
					case 60:
					{
						string[] separator = new string[]
						{
							"\r\n",
							"\n",
							"\r"
						};
						string[] array = text7.Split(separator, StringSplitOptions.RemoveEmptyEntries);
						tsn.Equals("3299");
						num = 51;
						num2 = num;
						continue;
					}
					case 61:
						num = 42;
						num2 = num;
						continue;
					case 62:
						num = 28;
						num2 = num;
						continue;
					case 63:
					{
						string text;
						if (!(text == "denomination"))
						{
							num = 9;
							num2 = num;
							continue;
						}
						char[] trimChars6 = new char[]
						{
							'[',
							']'
						};
						string value;
						text8 = value.Trim(trimChars6);
						num = 11;
						num2 = num;
						continue;
					}
					case 64:
						array3 = array2;
						num4 = 0;
						num = 56;
						num2 = num;
						continue;
					case 65:
					{
						string pattern;
						string input;
						Match match = Regex.Match(input, pattern);
						string value2 = match.Groups["desc"].Captures[0].Value;
						string value = match.Groups["value"].Captures[0].Value;
						string text9 = value2.ToLower().Trim(new char[]
						{
							'\t'
						});
						string text10 = text9;
						bool flag4 = text9 != null;
						num = 77;
						num2 = num;
						continue;
					}
					case 66:
					{
						string text10;
						string text = text10;
						num3 = <PrivateImplementationDetails>.ComputeStringHash(text);
						num = 85;
						num2 = num;
						continue;
					}
					case 67:
						num = 33;
						num2 = num;
						continue;
					case 68:
						goto IL_55D;
					case 69:
					{
						num = 911;
						int arg_9A3_0 = num;
						num = 911;
						switch ((arg_9A3_0 == num) ? 1 : 0)
						{
						case 0:
						case 2:
							goto IL_B5E;
						case 1:
							IL_9B7:
							num = 0;
							if (num != 0)
							{
							}
							num = 61;
							num2 = num;
							continue;
						}
						goto IL_9B7;
					}
					case 70:
						num = 3;
						num2 = num;
						continue;
					case 71:
						goto IL_55D;
					case 72:
						if (num3 <= 750404356u)
						{
							num = 83;
							num2 = num;
							continue;
						}
						num = 47;
						num2 = num;
						continue;
					case 73:
					{
						if (!flag5)
						{
							num = 37;
							num2 = num;
							continue;
						}
						string input = array3[num4];
						string pattern;
						bool flag2 = Regex.IsMatch(input, pattern);
						num = 18;
						num2 = num;
						continue;
					}
					case 74:
						goto IL_55D;
					case 75:
						if (num3 != 3395806334u)
						{
							num = 86;
							num2 = num;
							continue;
						}
						num = 50;
						num2 = num;
						continue;
					case 76:
					{
						string text;
						if (!(text == "reject count"))
						{
							num = 24;
							num2 = num;
							continue;
						}
						char[] trimChars7 = new char[]
						{
							'[',
							']'
						};
						string value;
						text11 = value.Trim(trimChars7);
						num = 2;
						num2 = num;
						continue;
					}
					case 77:
					{
						bool flag4;
						if (flag4)
						{
							num = 66;
							num2 = num;
							continue;
						}
						goto IL_365;
					}
					case 78:
						goto IL_55D;
					case 79:
						goto IL_B0D;
					case 80:
						num = 58;
						num2 = num;
						continue;
					case 81:
						goto IL_55D;
					case 82:
					{
						string text;
						if (!(text == "opcode"))
						{
							num = 53;
							num2 = num;
							continue;
						}
						char[] trimChars8 = new char[]
						{
							'[',
							']'
						};
						string value;
						text12 = value.Trim(trimChars8);
						num = 17;
						num2 = num;
						continue;
					}
					case 83:
						num = 19;
						num2 = num;
						continue;
					case 84:
					{
						string text;
						if (!(text == "dispense count"))
						{
							num = 1;
							num2 = num;
							continue;
						}
						char[] trimChars9 = new char[]
						{
							'[',
							']'
						};
						string value;
						text13 = value.Trim(trimChars9);
						num = 31;
						num2 = num;
						continue;
					}
					case 85:
						if (num3 <= 1224072165u)
						{
							num = 5;
							num2 = num;
							continue;
						}
						num = 29;
						num2 = num;
						continue;
					case 86:
						num = 20;
						num2 = num;
						continue;
					}
					goto IL_184;
					IL_365:
					num = 79;
					num2 = num;
					continue;
					IL_433:
					num = 4;
					num2 = num;
					continue;
					IL_55D:
					num = 6;
					num2 = num;
					continue;
					IL_8D9:
					flag5 = (num4 < array3.Length);
					num = 73;
					num2 = num;
					continue;
					IL_B0D:
					num4++;
					num = 52;
					num2 = num;
					continue;
					IL_B5E:
					if (num3 != 750404356u)
					{
						num = 57;
						num2 = num;
						continue;
					}
					num = 25;
					num2 = num;
					continue;
					IL_E1B:
					string[] array4 = new string[]
					{
						text4,
						text11,
						text3,
						text13,
						text5,
						text6,
						text8,
						text12,
						text2
					};
					result = array4;
					num = 26;
					num2 = num;
				}
				return result;
				IL_184:
				text4 = "";
				text11 = "";
				text3 = "";
				text13 = "";
				text5 = "";
				text6 = "";
				text8 = "";
				text12 = "";
				text2 = "";
				string[] separator2 = new string[]
				{
					"JPR CONTENTS"
				};
				string[] array5 = details.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
				text7 = Array.Find<string>(array5, delegate(string str)
				{
					int num5 = 23201;
					int arg_1C_0 = num5;
					num5 = 23201;
					switch ((arg_1C_0 == num5) ? 1 : 0)
					{
					case 0:
					case 2:
						IL_2F:
						break;
					case 1:
						goto IL_30;
					}
					goto IL_2F;
					IL_30:
					num5 = 1;
					if (num5 != 0)
					{
					}
					num5 = 0;
					if (num5 != 0)
					{
					}
					num5 = 0;
					return str.Contains(string.Format("[{0}]", tsn));
				});
				array2 = null;
				flag = !string.IsNullOrEmpty(text7);
				num = 14;
				num2 = num;
				goto IL_21;
			}
			}
			goto IL_17;
		}

		public string GetTransactionStatusNew(List<comment> comments, string comment, string errorcode, string detail)
		{
			int num = 0;
			int num2 = num;
			switch (num2)
			{
			case 0:
			{
				IL_17:
				num = 1;
				if (num != 0)
				{
				}
				num = 0;
				string result;
				try
				{
					switch (0)
					{
					case 0:
						goto IL_256;
					}
					while (true)
					{
						IL_6B:
						string text;
						bool arg_BA5_0;
						bool flag2;
						bool flag3;
						string arg_2F9_0;
						bool arg_FC8_0;
						bool arg_A37_0;
						bool flag9;
						bool arg_D2C_0;
						bool arg_DFD_0;
						bool arg_C58_0;
						bool flag12;
						int num3;
						bool flag14;
						bool arg_58B_0;
						bool flag15;
						bool flag16;
						bool arg_92B_0;
						bool flag18;
						bool flag19;
						bool flag20;
						switch (num2)
						{
						case 0:
						{
							bool flag;
							if (flag)
							{
								num = 23;
								num2 = num;
								continue;
							}
							num = 66;
							num2 = num;
							continue;
						}
						case 1:
							goto IL_89E;
						case 2:
							goto IL_ED1;
						case 3:
							result = text;
							num = 45;
							num2 = num;
							continue;
						case 4:
							goto IL_10D4;
						case 5:
							if (comment == "")
							{
								num = 98;
								num2 = num;
								continue;
							}
							goto IL_B8D;
						case 6:
							arg_BA5_0 = false;
							goto IL_BA5;
						case 7:
							if (flag2)
							{
								num = 81;
								num2 = num;
								continue;
							}
							num = 93;
							num2 = num;
							continue;
						case 8:
							text = "Failed(Dispense Error)";
							result = text;
							num = 65;
							num2 = num;
							continue;
						case 9:
							num = 79;
							num2 = num;
							continue;
						case 10:
							goto IL_B72;
						case 11:
							if (!detail.Contains("CASH PRESENTED"))
							{
								num = 39;
								num2 = num;
								continue;
							}
							goto IL_573;
						case 12:
							goto IL_824;
						case 13:
							text = "Successful";
							num = 95;
							num2 = num;
							continue;
						case 14:
							num = 18;
							num2 = num;
							continue;
						case 15:
							goto IL_ED1;
						case 16:
							goto IL_F95;
						case 17:
						{
							if (flag3)
							{
								num = 30;
								num2 = num;
								continue;
							}
							bool flag4 = detail.Contains("THE TRANSACTION COULD NOT BE COMPLETED");
							num = 36;
							num2 = num;
							continue;
						}
						case 18:
							if (errorcode != "0000000")
							{
								num = 10;
								num2 = num;
								continue;
							}
							goto IL_B8D;
						case 19:
							if (comment == "")
							{
								num = 1;
								num2 = num;
								continue;
							}
							num = 97;
							num2 = num;
							continue;
						case 20:
							goto IL_ED1;
						case 21:
							arg_2F9_0 = "";
							goto IL_2F9;
						case 22:
							arg_FC8_0 = (errorcode != null);
							goto IL_FC8;
						case 23:
							text = "Failed";
							result = text;
							num = 12;
							num2 = num;
							continue;
						case 24:
							num = 57;
							num2 = num;
							continue;
						case 25:
						{
							bool flag5 = comments.Where(delegate(comment c)
							{
								int num4 = 20629;
								int arg_1C_0 = num4;
								num4 = 20629;
								switch ((arg_1C_0 == num4) ? 1 : 0)
								{
								case 0:
								case 2:
									IL_2F:
									break;
								case 1:
									goto IL_30;
								}
								goto IL_2F;
								IL_30:
								num4 = 1;
								if (num4 != 0)
								{
								}
								num4 = 0;
								if (num4 != 0)
								{
								}
								num4 = 0;
								return c.comment_desc.ToUpper().Contains(comment);
							}).Any<comment>();
							num = 69;
							num2 = num;
							continue;
						}
						case 26:
							if (!(errorcode == ""))
							{
								num = 9;
								num2 = num;
								continue;
							}
							num = 78;
							num2 = num;
							continue;
						case 27:
							if (errorcode.ToLower().Contains("e*0"))
							{
								num = 61;
								num2 = num;
								continue;
							}
							num = 103;
							num2 = num;
							continue;
						case 28:
							if (comment != null)
							{
								num = 99;
								num2 = num;
								continue;
							}
							goto IL_FB0;
						case 29:
							goto IL_5DC;
						case 30:
							text = "Successful";
							result = text;
							num = 29;
							num2 = num;
							continue;
						case 31:
						{
							bool flag6;
							if (flag6)
							{
								num = 101;
								num2 = num;
								continue;
							}
							bool flag7 = detail.Contains("971A2");
							num = 116;
							num2 = num;
							continue;
						}
						case 32:
							num = 5;
							num2 = num;
							continue;
						case 33:
							goto IL_481;
						case 34:
							goto IL_702;
						case 35:
							num = 70;
							num2 = num;
							continue;
						case 36:
						{
							bool flag4;
							if (flag4)
							{
								num = 80;
								num2 = num;
								continue;
							}
							bool flag8 = detail.Contains("NOT DISPENSABLE");
							num = 111;
							num2 = num;
							continue;
						}
						case 37:
							text = "Failed";
							num = 33;
							num2 = num;
							continue;
						case 38:
							if (errorcode.ToLower().Contains("e*2"))
							{
								num = 24;
								num2 = num;
								continue;
							}
							num = 118;
							num2 = num;
							continue;
						case 39:
							num = 114;
							num2 = num;
							continue;
						case 40:
							goto IL_427;
						case 41:
							text = "Failed";
							num = 85;
							num2 = num;
							continue;
						case 42:
							num = 19;
							num2 = num;
							continue;
						case 43:
							if (errorcode.ToLower().Contains("e*2"))
							{
								num = 90;
								num2 = num;
								continue;
							}
							num = 75;
							num2 = num;
							continue;
						case 44:
							text = "Indeterminate";
							num = 15;
							num2 = num;
							continue;
						case 45:
							goto IL_4D7;
						case 46:
							if (!(errorcode == ""))
							{
								num = 120;
								num2 = num;
								continue;
							}
							goto IL_F95;
						case 47:
							num = 43;
							num2 = num;
							continue;
						case 48:
							goto IL_443;
						case 49:
							arg_A37_0 = comments.Where(delegate(comment c)
							{
								int num4 = 3211;
								int arg_1C_0 = num4;
								num4 = 3211;
								switch ((arg_1C_0 == num4) ? 1 : 0)
								{
								case 0:
								case 2:
									IL_2F:
									break;
								case 1:
									goto IL_30;
								}
								goto IL_2F;
								IL_30:
								num4 = 0;
								if (num4 != 0)
								{
								}
								num4 = 1;
								if (num4 != 0)
								{
								}
								num4 = 0;
								return c.comment_desc.ToUpper() == comment;
							}).Any<comment>();
							goto IL_A37;
						case 50:
						{
							if (flag9)
							{
								num = 72;
								num2 = num;
								continue;
							}
							bool flag10 = errorcode.ToLower().Contains("971A2");
							num = 76;
							num2 = num;
							continue;
						}
						case 51:
							goto IL_63E;
						case 52:
							num = 77;
							num2 = num;
							continue;
						case 53:
							text = "Failed(Dispense Error)";
							result = text;
							num = 34;
							num2 = num;
							continue;
						case 54:
							text = "Failed (Dispense Error)";
							num = 2;
							num2 = num;
							continue;
						case 55:
							goto IL_ED1;
						case 56:
							arg_BA5_0 = (errorcode != null);
							goto IL_BA5;
						case 57:
							arg_D2C_0 = !errorcode.ToLower().Contains("m-00");
							goto IL_D2C;
						case 58:
							arg_DFD_0 = errorcode.ToLower().Contains("m-00");
							goto IL_DFD;
						case 59:
							arg_C58_0 = errorcode.ToLower().Contains("m-00");
							goto IL_C58;
						case 60:
						{
							bool flag11;
							if (flag11)
							{
								num = 37;
								num2 = num;
								continue;
							}
							num = 40;
							num2 = num;
							continue;
						}
						case 61:
							num = 58;
							num2 = num;
							continue;
						case 62:
						{
							if (!flag12)
							{
								num = 107;
								num2 = num;
								continue;
							}
							comment comment2 = comments[num3];
							bool flag13 = detail != null;
							num = 84;
							num2 = num;
							continue;
						}
						case 63:
							goto IL_10BC;
						case 64:
							if (flag14)
							{
								num = 47;
								num2 = num;
								continue;
							}
							num = 28;
							num2 = num;
							continue;
						case 65:
							goto IL_6A0;
						case 66:
							if (comment != null)
							{
								num = 42;
								num2 = num;
								continue;
							}
							goto IL_89E;
						case 67:
							goto IL_10BC;
						case 68:
							goto IL_764;
						case 69:
						{
							bool flag5;
							if (flag5)
							{
								num = 82;
								num2 = num;
								continue;
							}
							text = "Indeterminate";
							num = 89;
							num2 = num;
							continue;
						}
						case 70:
							arg_58B_0 = detail.Contains("CASH TAKEN");
							goto IL_58B;
						case 71:
							if (flag15)
							{
								num = 13;
								num2 = num;
								continue;
							}
							num = 83;
							num2 = num;
							continue;
						case 72:
							text = "Successful";
							num = 55;
							num2 = num;
							continue;
						case 73:
							if (flag16)
							{
								num = 25;
								num2 = num;
								continue;
							}
							text = "Indeterminate";
							num = 105;
							num2 = num;
							continue;
						case 74:
							arg_58B_0 = true;
							goto IL_58B;
						case 75:
							arg_C58_0 = false;
							goto IL_C58;
						case 76:
						{
							bool flag10;
							if (flag10)
							{
								num = 41;
								num2 = num;
								continue;
							}
							text = "Indeterminate";
							num = 20;
							num2 = num;
							continue;
						}
						case 77:
							if (comment != "")
							{
								num = 110;
								num2 = num;
								continue;
							}
							num = 104;
							num2 = num;
							continue;
						case 78:
							arg_92B_0 = true;
							goto IL_911;
						case 79:
							arg_92B_0 = (errorcode == "0000000");
							goto IL_911;
						case 80:
							text = "Failed";
							result = text;
							num = 51;
							num2 = num;
							continue;
						case 81:
							text = "Failed";
							num = 115;
							num2 = num;
							continue;
						case 82:
							text = "Failed";
							num = 102;
							num2 = num;
							continue;
						case 83:
							if (comment == null)
							{
								num = 52;
								num2 = num;
								continue;
							}
							goto IL_9F1;
						case 84:
						{
							bool flag13;
							if (flag13)
							{
								num = 109;
								num2 = num;
								continue;
							}
							goto IL_427;
						}
						case 85:
							goto IL_ED1;
						case 86:
							arg_FC8_0 = false;
							goto IL_FC8;
						case 87:
						{
							bool flag17;
							if (flag17)
							{
								num = 53;
								num2 = num;
								continue;
							}
							bool flag6 = detail.Contains("ERROR CODE = 971A2");
							num = 31;
							num2 = num;
							continue;
						}
						case 88:
							goto IL_443;
						case 89:
							goto IL_1085;
						case 90:
							num = 59;
							num2 = num;
							continue;
						case 91:
							comment = ((comment == null) ? "" : comment);
							detail = detail.Replace(Environment.NewLine, " ");
							text = "Indeterminate";
							num3 = 0;
							num = 48;
							num2 = num;
							continue;
						case 92:
							if (errorcode == "0000000")
							{
								num = 16;
								num2 = num;
								continue;
							}
							goto IL_FB0;
						case 93:
							if (comment != null)
							{
								num = 32;
								num2 = num;
								continue;
							}
							goto IL_B00;
						case 94:
							goto IL_7C6;
						case 95:
							goto IL_10BC;
						case 96:
							if (flag18)
							{
								num = 54;
								num2 = num;
								continue;
							}
							num = 27;
							num2 = num;
							continue;
						case 97:
							arg_92B_0 = false;
							goto IL_92B;
						case 98:
							goto IL_B00;
						case 99:
							num = 46;
							num2 = num;
							continue;
						case 100:
							arg_2F9_0 = errorcode;
							goto IL_2F9;
						case 101:
							text = "Failed(Dispense Error)";
							result = text;
							num = 68;
							num2 = num;
							continue;
						case 102:
							goto IL_1085;
						case 103:
							arg_DFD_0 = false;
							goto IL_DFD;
						case 104:
							arg_A37_0 = false;
							goto IL_A37;
						case 105:
							goto IL_10BC;
						case 106:
							if (errorcode != null)
							{
								num = 108;
								num2 = num;
								continue;
							}
							num = 21;
							num2 = num;
							continue;
						case 107:
							goto IL_481;
						case 108:
						{
							num = -608;
							int arg_2A2_0 = num;
							num = -608;
							switch ((arg_2A2_0 == num) ? 1 : 0)
							{
							case 0:
							case 2:
								goto IL_443;
							case 1:
								IL_2B6:
								num = 0;
								if (num != 0)
								{
								}
								num = 100;
								num2 = num;
								continue;
							}
							goto IL_2B6;
						}
						case 109:
						{
							comment comment2;
							bool flag11 = detail.ToUpper().Contains(comment2.comment_desc.Trim());
							num = 60;
							num2 = num;
							continue;
						}
						case 110:
							goto IL_9F1;
						case 111:
						{
							bool flag8;
							if (flag8)
							{
								num = 8;
								num2 = num;
								continue;
							}
							bool flag17 = detail.Contains("CDU DEVICE ERROR");
							num = 87;
							num2 = num;
							continue;
						}
						case 112:
							if (!(errorcode != ""))
							{
								num = 14;
								num2 = num;
								continue;
							}
							goto IL_B72;
						case 113:
							if (flag19)
							{
								num = 44;
								num2 = num;
								continue;
							}
							num = 38;
							num2 = num;
							continue;
						case 114:
							if (!detail.Contains("INQUIRY"))
							{
								num = 35;
								num2 = num;
								continue;
							}
							goto IL_573;
						case 115:
							goto IL_10BC;
						case 116:
						{
							bool flag7;
							if (flag7)
							{
								num = 119;
								num2 = num;
								continue;
							}
							bool flag = detail.Contains("TRANSACTION FAILED");
							num = 0;
							num2 = num;
							continue;
						}
						case 117:
							if (flag20)
							{
								num = 3;
								num2 = num;
								continue;
							}
							num = 11;
							num2 = num;
							continue;
						case 118:
							arg_D2C_0 = false;
							goto IL_D2C;
						case 119:
							text = "Failed(Dispense Error)";
							result = text;
							num = 94;
							num2 = num;
							continue;
						case 120:
							num = 92;
							num2 = num;
							continue;
						}
						goto IL_256;
						IL_2F9:
						errorcode = arg_2F9_0;
						num = 91;
						num2 = num;
						continue;
						IL_427:
						num3++;
						num = 88;
						num2 = num;
						continue;
						IL_443:
						flag12 = (num3 < comments.Count);
						num = 62;
						num2 = num;
						continue;
						IL_481:
						flag20 = (text != "Indeterminate");
						num = 117;
						num2 = num;
						continue;
						IL_573:
						num = 74;
						num2 = num;
						continue;
						IL_58B:
						flag3 = arg_58B_0;
						num = 17;
						num2 = num;
						continue;
						IL_89E:
						num = 26;
						num2 = num;
						continue;
						IL_92B:
						flag15 = arg_92B_0;
						num = 71;
						num2 = num;
						continue;
						IL_911:
						goto IL_92B;
						IL_9F1:
						num = 49;
						num2 = num;
						continue;
						IL_A37:
						flag2 = arg_A37_0;
						num = 7;
						num2 = num;
						continue;
						IL_B00:
						num = 112;
						num2 = num;
						continue;
						IL_B72:
						num = 56;
						num2 = num;
						continue;
						IL_B8D:
						num = 6;
						num2 = num;
						continue;
						IL_BA5:
						flag14 = arg_BA5_0;
						num = 64;
						num2 = num;
						continue;
						IL_C58:
						flag19 = arg_C58_0;
						num = 113;
						num2 = num;
						continue;
						IL_D2C:
						flag18 = arg_D2C_0;
						num = 96;
						num2 = num;
						continue;
						IL_DFD:
						flag9 = arg_DFD_0;
						num = 50;
						num2 = num;
						continue;
						IL_ED1:
						num = 67;
						num2 = num;
						continue;
						IL_F95:
						num = 22;
						num2 = num;
						continue;
						IL_FB0:
						num = 86;
						num2 = num;
						continue;
						IL_FC8:
						flag16 = arg_FC8_0;
						num = 73;
						num2 = num;
						continue;
						IL_1085:
						num = 63;
						num2 = num;
						continue;
						IL_10BC:
						result = text;
						num = 4;
						num2 = num;
					}
					IL_4D7:
					IL_5DC:
					IL_63E:
					IL_6A0:
					IL_702:
					IL_764:
					IL_7C6:
					IL_824:
					IL_10D4:
					return result;
					IL_256:
					num = 106;
					num2 = num;
					goto IL_6B;
				}
				catch (Exception var_25_10D6)
				{
					result = "Indeterminate";
				}
				return result;
			}
			}
			goto IL_17;
		}
	}
}
