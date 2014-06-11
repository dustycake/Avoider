// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;

namespace Core
{
	public static class DateUtils
	{
		/// <summary>
		/// Returns a unix timestamp
		/// </summary>
		/// <returns>The timestamp.</returns>
		public static long GetTimestamp()
		{
			return (long)(DateTime.Now - UnixEpoch).TotalSeconds;
		}

		/// <summary>
		/// Standard unix epoch
		/// </summary>
		/// <value>The unix epoch.</value>
		public static DateTime UnixEpoch
		{
			get 
			{
				return new DateTime(1970, 1, 1);
			}
		}

		/// <summary>
		/// Returns a DateTime object for a given unix timestamp
		/// </summary>
		/// <returns>The timestamp.</returns>
		/// <param name="timestamp">Timestamp.</param>
		public static DateTime FromTimestamp(long timestamp)
		{
			return UnixEpoch.AddSeconds(timestamp);
		}
	}
}