using System;

namespace Account
{
	[Serializable]
	public static class AccountInfo
	{
		private static string _id;
		private static string _nickname;

		public static string Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public static string Nickname
		{
			get { return _nickname; }
			set { _nickname = value; }
		}
	}
}
